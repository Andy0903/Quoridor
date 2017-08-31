using Lidgren.Network;
using Microsoft.Xna.Framework;
using QuoridorNetwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorServer
{
    class Game
    {
        List<Player> myPlayers = new List<Player>();
        int myPlayerIndexThisTurn = -1;
        Tile[,] myWideTiles = new Tile[9, 9];
        Tile[,] myVerticals = new Tile[8, 9];
        Tile[,] myHorizontals = new Tile[9, 8];

        Player CurrentPlayer => myPlayers[myPlayerIndexThisTurn];
        bool PlayerInGoal => myWideTiles[CurrentPlayer.WideTilePosition.X, CurrentPlayer.WideTilePosition.Y].Color == CurrentPlayer.Color;

        public Game(List<Tuple<string, long>> aClients)
        {
            for (int i = 0; i < aClients.Count; i++)
            {
                myPlayers.Add(new Player(aClients[i].Item1, aClients[i].Item2, (aClients.Count == 2 ? 10 : 5), i));
            }

            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                for (int k = 0; k < myWideTiles.GetLength(1); k++)
                {
                    myWideTiles[i, k] = new Tile(new Point(i, k));
                }
            }

            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                myWideTiles[i, 0].Color = Color.Red;
                myWideTiles[i, myWideTiles.GetLength(0) - 1].Color = Color.Blue;
            }

            myWideTiles[4, 8].IsOccupied = true;
            myWideTiles[4, 0].IsOccupied = true;

            if (myPlayers.Count == 4)
            {
                for (int i = 0; i < myWideTiles.GetLength(1); i++)
                {
                    myWideTiles[0, i].Color = Color.Yellow;
                    myWideTiles[myWideTiles.GetLength(1) - 1, i].Color = Color.Green;
                }

                myWideTiles[0, 4].IsOccupied = true;
                myWideTiles[8, 4].IsOccupied = true;
            }

            for (int i = 0; i < myVerticals.GetLength(0); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(1); k++)
                {
                    myVerticals[i, k] = new Tile(new Point(i, k));
                }
            }

            for (int i = 0; i < myVerticals.GetLength(1); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(0); k++)
                {
                    myHorizontals[i, k] = new Tile(new Point(i, k));
                }
            }
        }

        public void Start()
        {
            NetworkManager.OnPlayerMoved += NetworkManager_OnPlayerMoved;
            NetworkManager.OnNewWallPlaced += NetworkManager_OnNewWallPlaced;
            NetworkManager.Send(new GameReadyToStartMessage(myPlayers.Select(i => i.Name).ToList()));
            NextTurn();
        }

        private void NetworkManager_OnNewWallPlaced(object sender, PlaceWallMessage e)
        {
            if (e.Sender.RemoteUniqueIdentifier != CurrentPlayer.RemoteUniqueIdentifier)
                return;

            bool validWallPlacement = PlaceWall(e);
            if (validWallPlacement)
            {
                NetworkManager.Send(new PlaceWallMessage(e.WallType, e.Column, e.Row, myPlayerIndexThisTurn));
                NextTurn();
            }
            else
            {
                NetworkManager.Send(new ActionRejectMessage(), e.Sender);
            }
        }

        private void NetworkManager_OnPlayerMoved(object sender, PlayerMoveMessage e)
        {
            if (e.Sender.RemoteUniqueIdentifier != CurrentPlayer.RemoteUniqueIdentifier)
                return;

            bool validMove = Move(e);
            if (validMove)
            {
                NetworkManager.Send(new PlayerMoveMessage(e.Column, e.Row, myPlayerIndexThisTurn));

                if (PlayerInGoal)
                {
                    NetworkManager.Send(new PlayerWonMessage(myPlayerIndexThisTurn));
                }
                else
                {
                    NextTurn();
                }
            }
            else
            {
                NetworkManager.Send(new ActionRejectMessage(), e.Sender);
            }
        }

        public void NextTurn()
        {
            myPlayerIndexThisTurn = (myPlayerIndexThisTurn + 1) % myPlayers.Count;
            NetworkManager.Send(new NewTurnMessage(myPlayerIndexThisTurn));
        }

        private bool IsWithinGameBoard(int aColumn, int aRow)
        {
            return (0 <= aColumn && aColumn < myWideTiles.GetLength(0) &&
                    0 <= aRow && aRow < myWideTiles.GetLength(1));
        }

        private List<Point> ValidMovesFromTilePosition(Point aPosition, bool isInFirstStep = true)
        {
            List<Point> validPoints = new List<Point>();

            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i == 0 && k == 0)
                        continue;

                    Point p = new Point(aPosition.X + i, aPosition.Y + k);

                    if (!IsWithinGameBoard(p.X, p.Y) || !CanMoveBetween(aPosition, p))
                        continue;

                    if (!myWideTiles[p.X, p.Y].IsOccupied)
                    {
                        validPoints.Add(p);
                    }
                    else if (isInFirstStep)
                    {
                        validPoints.AddRange(ValidMovesFromTilePosition(p, false));
                    }
                }
            }

            return validPoints;
        }

        private bool CanMoveBetween(Point aStartPoint, Point aEndPoint)
        {
            if (aStartPoint.X == aEndPoint.X)
            {
                int minRow = Math.Min(aStartPoint.Y, aEndPoint.Y);
                return !myHorizontals[aStartPoint.X, minRow].IsOccupied;
            }
            else if (aStartPoint.Y == aEndPoint.Y)
            {
                int minColumn = Math.Min(aStartPoint.X, aEndPoint.X);
                return !myVerticals[minColumn, aStartPoint.Y].IsOccupied;
            }

            return false;
        }

        private bool Move(PlayerMoveMessage aMsg)
        {
            if (!IsWithinGameBoard(aMsg.Column, aMsg.Row))
                return false;
            if (myWideTiles[aMsg.Column, aMsg.Row].IsOccupied)
                return false;

            List<Point> validMoves = ValidMovesFromTilePosition(CurrentPlayer.WideTilePosition);
            Point newPosition = new Point(aMsg.Column, aMsg.Row);

            if (!validMoves.Contains(newPosition))
                return false;

            int oldColumn = CurrentPlayer.WideTilePosition.X;
            int oldRow = CurrentPlayer.WideTilePosition.Y;
            myWideTiles[oldColumn, oldRow].IsOccupied = false;

            CurrentPlayer.WideTilePosition = newPosition;
            myWideTiles[aMsg.Column, aMsg.Row].IsOccupied = true;

            return true;
        }

        public bool PlaceWall(PlaceWallMessage aMsg)
        {
            int row = aMsg.Row;
            int column = aMsg.Column;

            if (CurrentPlayer.NumberOfWalls <= 0)
                return false;

            switch (aMsg.WallType)
            {
                case TileType.NarrowVertical:
                    if (column < 0 || myVerticals.GetLength(0) <= column)
                        return false;
                    if (row < 0 || myVerticals.GetLength(1) - 1 <= row)
                        return false;
                    if (myVerticals[column, row].IsOccupied || myVerticals[column, row + 1].IsOccupied)
                        return false;

                    myVerticals[column, row].IsOccupied = true;
                    myVerticals[column, row + 1].IsOccupied = true;

                    if (!PathToGoalExists())
                    {
                        myVerticals[column, row].IsOccupied = false;
                        myVerticals[column, row + 1].IsOccupied = false;
                        return false;
                    }
                    break;
                case TileType.NarrowHorizontal:
                    if (column < 0 || myHorizontals.GetLength(0) - 1 <= column)
                        return false;
                    if (row < 0 || myHorizontals.GetLength(1) <= row)
                        return false;
                    if (myHorizontals[column, row].IsOccupied || myHorizontals[column + 1, row].IsOccupied)
                        return false;

                    myHorizontals[column, row].IsOccupied = true;
                    myHorizontals[column + 1, row].IsOccupied = true;

                    if (!PathToGoalExists())
                    {
                        myHorizontals[column, row].IsOccupied = false;
                        myHorizontals[column + 1, row].IsOccupied = false;
                        return false;
                    }

                    break;
                default:
                    return false;
            }

            CurrentPlayer.DecrementWalls();
            return true;
        }

        private bool PathToGoalExists()
        {
            SortedSet<Tile> S = new SortedSet<Tile>();
            Queue<Tile> Q = new Queue<Tile>();

            foreach (Player p in myPlayers)
            {
                Tile root = myWideTiles[p.WideTilePosition.X, p.WideTilePosition.Y];
                S.Add(root);
                Q.Enqueue(root);

                while (Q.Count > 0)
                {
                    Tile current = Q.Dequeue();

                    if (current.Color == p.Color) //Current is a goal tile for the player.
                        return true;

                    foreach (Tile t in GetAdjacentTiles(current.Position))
                    {
                        if (S.Contains(t) == false)
                        {
                            S.Add(t);
                            Q.Enqueue(t);
                        }
                    }
                }
            }

            return false;
        }

        private List<Tile> GetAdjacentTiles(Point aIndex)
        {
            List<Tile> adjacent = new List<Tile>();

            if (aIndex.Y - 1 >= 0)
            {
                if (myHorizontals[aIndex.X, aIndex.Y - 1].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X, aIndex.Y - 1]); //Up
                }
            }
            if (aIndex.X - 1 >= 0)
            {
                if (myVerticals[aIndex.X - 1, aIndex.Y].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X - 1, aIndex.Y]); //Left
                }
            }
            if (aIndex.Y + 1 < myWideTiles.Length)
            {
                if (myHorizontals[aIndex.Y, aIndex.X].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X, aIndex.Y + 1]); //Down
                }
            }
            if (aIndex.X + 1 < myWideTiles.Length)
            {
                if (myVerticals[aIndex.X, aIndex.Y].IsOccupied == false)
                {
                    adjacent.Add(myWideTiles[aIndex.X + 1, aIndex.Y]);  //Right
                }
            }

            return adjacent;
        }
    }
}

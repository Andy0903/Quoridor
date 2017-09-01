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
        List<Player> players = new List<Player>();
        int playerIndexThisTurn = -1;
        Tile[,] wideTiles = new Tile[9, 9];
        Tile[,] verticals = new Tile[8, 9];
        Tile[,] horizontals = new Tile[9, 8];

        Player CurrentPlayer => players[playerIndexThisTurn];
        bool PlayerInGoal => wideTiles[CurrentPlayer.WideTilePosition.X, CurrentPlayer.WideTilePosition.Y].Color == CurrentPlayer.Color;

        public Game(List<Tuple<string, long>> clients)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                players.Add(new Player(clients[i].Item1, clients[i].Item2, (clients.Count == 2 ? 10 : 5), i));
            }

            for (int i = 0; i < wideTiles.GetLength(0); i++)
            {
                for (int k = 0; k < wideTiles.GetLength(1); k++)
                {
                    wideTiles[i, k] = new Tile(new Point(i, k));
                }
            }

            for (int i = 0; i < wideTiles.GetLength(0); i++)
            {
                wideTiles[i, 0].Color = Color.Red;
                wideTiles[i, wideTiles.GetLength(0) - 1].Color = Color.Blue;
            }

            wideTiles[4, 8].IsOccupied = true;
            wideTiles[4, 0].IsOccupied = true;

            if (players.Count == 4)
            {
                for (int i = 0; i < wideTiles.GetLength(1); i++)
                {
                    wideTiles[0, i].Color = Color.Yellow;
                    wideTiles[wideTiles.GetLength(1) - 1, i].Color = Color.Green;
                }

                wideTiles[0, 4].IsOccupied = true;
                wideTiles[8, 4].IsOccupied = true;
            }

            for (int i = 0; i < verticals.GetLength(0); i++)
            {
                for (int k = 0; k < verticals.GetLength(1); k++)
                {
                    verticals[i, k] = new Tile(new Point(i, k));
                }
            }

            for (int i = 0; i < verticals.GetLength(1); i++)
            {
                for (int k = 0; k < verticals.GetLength(0); k++)
                {
                    horizontals[i, k] = new Tile(new Point(i, k));
                }
            }
        }

        public void Start()
        {
            NetworkManager.OnPlayerMoved += NetworkManager_OnPlayerMoved;
            NetworkManager.OnNewWallPlaced += NetworkManager_OnNewWallPlaced;
            NetworkManager.Send(new GameReadyToStartMessage(players.Select(i => i.Name).ToList()));
            NextTurn();
        }

        private void NetworkManager_OnNewWallPlaced(object sender, PlaceWallMessage e)
        {
            if (e.Sender.RemoteUniqueIdentifier != CurrentPlayer.RemoteUniqueIdentifier)
                return;

            bool validWallPlacement = PlaceWall(e);
            if (validWallPlacement)
            {
                NetworkManager.Send(new PlaceWallMessage(e.WallType, e.Column, e.Row, playerIndexThisTurn));
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
                NetworkManager.Send(new PlayerMoveMessage(e.Column, e.Row, playerIndexThisTurn));

                if (PlayerInGoal)
                {
                    NetworkManager.Send(new PlayerWonMessage(playerIndexThisTurn));
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
            playerIndexThisTurn = (playerIndexThisTurn + 1) % players.Count;
            NetworkManager.Send(new NewTurnMessage(playerIndexThisTurn));
        }

        private bool IsWithinGameBoard(int column, int row)
        {
            return (0 <= column && column < wideTiles.GetLength(0) &&
                    0 <= row && row < wideTiles.GetLength(1));
        }

        private List<Point> ValidMovesFromTilePosition(Point position, bool isInFirstStep = true)
        {
            List<Point> validPoints = new List<Point>();

            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i == 0 && k == 0)
                        continue;

                    Point p = new Point(position.X + i, position.Y + k);

                    if (!IsWithinGameBoard(p.X, p.Y) || !CanMoveBetween(position, p))
                        continue;

                    if (!wideTiles[p.X, p.Y].IsOccupied)
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

        private bool CanMoveBetween(Point startPoint, Point endPoint)
        {
            if (startPoint.X == endPoint.X)
            {
                int minRow = Math.Min(startPoint.Y, endPoint.Y);
                return !horizontals[startPoint.X, minRow].IsOccupied;
            }
            else if (startPoint.Y == endPoint.Y)
            {
                int minColumn = Math.Min(startPoint.X, endPoint.X);
                return !verticals[minColumn, startPoint.Y].IsOccupied;
            }

            return false;
        }

        private bool Move(PlayerMoveMessage msg)
        {
            if (!IsWithinGameBoard(msg.Column, msg.Row))
                return false;
            if (wideTiles[msg.Column, msg.Row].IsOccupied)
                return false;

            List<Point> validMoves = ValidMovesFromTilePosition(CurrentPlayer.WideTilePosition);
            Point newPosition = new Point(msg.Column, msg.Row);

            if (!validMoves.Contains(newPosition))
                return false;

            int oldColumn = CurrentPlayer.WideTilePosition.X;
            int oldRow = CurrentPlayer.WideTilePosition.Y;
            wideTiles[oldColumn, oldRow].IsOccupied = false;

            CurrentPlayer.WideTilePosition = newPosition;
            wideTiles[msg.Column, msg.Row].IsOccupied = true;

            return true;
        }

        public bool PlaceWall(PlaceWallMessage msg)
        {
            int row = msg.Row;
            int column = msg.Column;

            if (CurrentPlayer.NumberOfWalls <= 0)
                return false;

            switch (msg.WallType)
            {
                case TileType.NarrowVertical:
                    if (column < 0 || verticals.GetLength(0) <= column)
                        return false;
                    if (row < 0 || verticals.GetLength(1) - 1 <= row)
                        return false;
                    if (verticals[column, row].IsOccupied || verticals[column, row + 1].IsOccupied)
                        return false;

                    verticals[column, row].IsOccupied = true;
                    verticals[column, row + 1].IsOccupied = true;

                    if (!PathToGoalExists())
                    {
                        verticals[column, row].IsOccupied = false;
                        verticals[column, row + 1].IsOccupied = false;
                        return false;
                    }
                    break;
                case TileType.NarrowHorizontal:
                    if (column < 0 || horizontals.GetLength(0) - 1 <= column)
                        return false;
                    if (row < 0 || horizontals.GetLength(1) <= row)
                        return false;
                    if (horizontals[column, row].IsOccupied || horizontals[column + 1, row].IsOccupied)
                        return false;

                    horizontals[column, row].IsOccupied = true;
                    horizontals[column + 1, row].IsOccupied = true;

                    if (!PathToGoalExists())
                    {
                        horizontals[column, row].IsOccupied = false;
                        horizontals[column + 1, row].IsOccupied = false;
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

            foreach (Player p in players)
            {
                Tile root = wideTiles[p.WideTilePosition.X, p.WideTilePosition.Y];
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

        private List<Tile> GetAdjacentTiles(Point index)
        {
            List<Tile> adjacent = new List<Tile>();

            if (index.Y - 1 >= 0)
            {
                if (horizontals[index.X, index.Y - 1].IsOccupied == false)
                {
                    adjacent.Add(wideTiles[index.X, index.Y - 1]); //Up
                }
            }
            if (index.X - 1 >= 0)
            {
                if (verticals[index.X - 1, index.Y].IsOccupied == false)
                {
                    adjacent.Add(wideTiles[index.X - 1, index.Y]); //Left
                }
            }
            if (index.Y + 1 < wideTiles.Length)
            {
                if (horizontals[index.Y, index.X].IsOccupied == false)
                {
                    adjacent.Add(wideTiles[index.X, index.Y + 1]); //Down
                }
            }
            if (index.X + 1 < wideTiles.Length)
            {
                if (verticals[index.X, index.Y].IsOccupied == false)
                {
                    adjacent.Add(wideTiles[index.X + 1, index.Y]);  //Right
                }
            }

            return adjacent;
        }
    }
}

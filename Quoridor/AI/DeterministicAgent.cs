using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Quoridor.AI
{
    class DeterministicAgent : Agent
    {
        int roundCounter = 0;
        GameData data;
        Random rnd = new Random();

        public Action DoAction(GameData data)
        {
            this.data = data;
            roundCounter++;

            if (roundCounter % 2 == 0)
            {
                Player rndPlayer = GetRandomOpponent();
                Tile opponentNextTile = PathToGoal(rndPlayer);
                WallOrientation alignment;

                if (rndPlayer.Position.X == opponentNextTile.Position.X)
                {
                    alignment = WallOrientation.Horizontal;
                    return new PlaceWallAction(opponentNextTile.Position.X, opponentNextTile.Position.Y, alignment);
                }
                else if (rndPlayer.Position.Y == opponentNextTile.Position.Y)
                {
                    alignment = WallOrientation.Vertical;
                    return new PlaceWallAction(opponentNextTile.Position.X, opponentNextTile.Position.Y, alignment);
                }
            }

            Tile nextTile = PathToGoal(this.data.Self);
            return new MoveAction(nextTile.Position.X, nextTile.Position.Y);
        }

        public Action RedoAction(GameData status)
        {
            return DoAction(status);
        }

        private Player GetRandomOpponent()
        {
            int index = rnd.Next(0, data.Players.Count - 1);
            Player player = data.Players[index];

            if (player == data.Self)
            {
                player = data.Players[data.Players.Count - 1];
            }

            return player;
        }

        private Tile PathToGoal(Player player)
        {
            Dictionary<Tile, Tile> shortestPath = new Dictionary<Tile, Tile>();
            HashSet<Tile> S = new HashSet<Tile>();
            Queue<Tile> Q = new Queue<Tile>();

            Tile root = data.Tiles[player.Position.X, player.Position.Y];
            S.Add(root);
            Q.Enqueue(root);

            while (Q.Count > 0)
            {
                Tile current = Q.Dequeue();

                if (data.Tiles[current.Position.X, current.Position.Y].Color == player.Color)
                {
                    Tile p = current;
                    while (shortestPath.ContainsKey(p) && shortestPath[p] != root)
                    {
                        p = shortestPath[p];
                    }
                    return p;
                }

                foreach (Tile p in ValidMovesFromTilePosition(current))
                {
                    if (S.Contains(p) == false)
                    {
                        S.Add(p);
                        Q.Enqueue(p);
                        shortestPath.Add(p, current);
                    }
                }
            }

            return null;
        }

        private bool IsWithinGameBoard(int column, int row)
        {
            return (0 <= column && column < data.Tiles.GetLength(0) &&
                    0 <= row && row < data.Tiles.GetLength(1));
        }

        private List<Tile> ValidMovesFromTilePosition(Tile tile, bool isInFirstStep = true)
        {
            List<Tile> validTiles = new List<Tile>();

            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if ((i != 0 && k != 0) || (i == 0 && k == 0))
                        continue;

                    int column = tile.Position.X + i;
                    int row = tile.Position.Y + k;

                    if (!IsWithinGameBoard(column, row))
                        continue;

                    Tile t = data.Tiles[column, row];

                    if (!CanMoveBetween(tile, t))
                        continue;


                    if (!data.Tiles[t.Position.X, t.Position.Y].IsOccupied)
                    {
                        validTiles.Add(t);
                    }
                    else if (isInFirstStep)
                    {
                        validTiles.AddRange(ValidMovesFromTilePosition(t, false));
                    }
                }
            }

            return validTiles;
        }

        private bool CanMoveBetween(Tile start, Tile end)
        {
            if (start.Position.X == end.Position.X)
            {
                int minRow = Math.Min(start.Position.Y, end.Position.Y);
                return !data.HorizontalWall[start.Position.X, minRow];
            }
            else if (start.Position.Y == end.Position.Y)
            {
                int minColumn = Math.Min(start.Position.X, end.Position.X);
                return !data.VerticalWall[minColumn, start.Position.Y];
            }

            return false;
        }

    }
}

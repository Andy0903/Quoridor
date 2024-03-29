﻿using System;
using System.Collections.Generic;

namespace Quoridor.AI
{
    class PlayerInputMode : Agent
    {
        GameData data;

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

        private bool IsWithinGameBoard(int column, int row)
        {
            return (0 <= column && column < data.Tiles.GetLength(0) &&
                    0 <= row && row < data.Tiles.GetLength(1));
        }

        public static void Main()
        {
            new PlayerInputMode().Start();
        }

        public override Action DoAction(GameData status)
        {
            data = status;
            Point position = data.Self.Position;
            Tile tile = data.Tiles[position.X, position.Y];
            List<Tile> validMoves = ValidMovesFromTilePosition(tile);

            InputMessenger.Update();

            if (InputMessenger.PressedW || InputMessenger.PressedA || InputMessenger.PressedS || InputMessenger.PressedD)
            {
                int posX = position.X + (InputMessenger.PressedA ? -1 : (InputMessenger.PressedD ? 1 : 0));
                int posY = position.Y + (InputMessenger.PressedW ? -1 : (InputMessenger.PressedS ? 1 : 0));

                if (data.Tiles[posX, posY].IsOccupied)
                {
                    if (InputMessenger.PressedW || InputMessenger.PressedA || InputMessenger.PressedS || InputMessenger.PressedD)
                    {
                        int jumpX = posX + (InputMessenger.PressedA ? -1 : (InputMessenger.PressedD ? 1 : 0));
                        int jumpY = posY + (InputMessenger.PressedW ? -1 : (InputMessenger.PressedS ? 1 : 0));

                        if (IsWithinGameBoard(jumpX, jumpY) && validMoves.Contains(data.Tiles[jumpX, jumpY]))
                            return new MoveAction(jumpX, jumpY);
                    }
                }
                else
                {
                    if (IsWithinGameBoard(posX, posY) && validMoves.Contains(data.Tiles[posX, posY]))
                        return new MoveAction(posX, posY);
                }

            }

            if (InputMessenger.PressedLMB)
            {
                for (int i = 0; i < data.HorizontalWall.GetLength(0); i++)
                {
                    for (int k = 0; k < data.HorizontalWall.GetLength(1); k++)
                    {
                        if (IsWithinGameBoard(i, k) && IsWithinGameBoard(i + 1, k) &&
                            InputMessenger.MousePosition == data.HorizontalWallPixelPosition[i, k] &&
                            data.HorizontalWall[i, k] == false && data.HorizontalWall[i + 1, k] == false)
                        {
                            return new PlaceWallAction(i, k, WallOrientation.Horizontal);
                        }
                    }
                }

                for (int i = 0; i < data.VerticalWall.GetLength(0); i++)
                {
                    for (int k = 0; k < data.VerticalWall.GetLength(1); k++)
                    {
                        if (IsWithinGameBoard(i, k) && IsWithinGameBoard(i, k + 1) &&
                            InputMessenger.MousePosition == data.VerticalWallPixelPosition[i, k] &&
                            data.VerticalWall[i, k] == false && data.VerticalWall[i, k + 1] == false)
                        {
                            return new PlaceWallAction(i, k, WallOrientation.Vertical);
                        }
                    }
                }
            }

            return null;
        }
    }
}

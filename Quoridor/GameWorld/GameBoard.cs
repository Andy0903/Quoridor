using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using QuoridorNetwork;

namespace Quoridor
{
    public class GameBoard
    {
        SpriteFont spriteFont;
        WideTile[,] wideTiles = new WideTile[9, 9];
        VerticalTile[,] verticals = new VerticalTile[8, 9];
        HorizontalTile[,] horizontals = new HorizontalTile[9, 8];
        List<GraphicalObject> walls = new List<GraphicalObject>();
        bool someoneWon = false;
        Color winTextColor;
        string winnerName;

        AI.Agent agent;
        int clientSlot;

        List<Player> players = new List<Player>();
        int currentSlotTurn;
        int numberOfTurns;

        public bool TileNotOccupied(int column, int row)
        {
            return wideTiles[column, row].IsOccupied == false;
        }

        public void SetWideTilesOccupied(int column, int row, bool state)
        {
            wideTiles[column, row].IsOccupied = state;
        }

        public bool IsWithinGameBoard(int column, int row)
        {
            return (0 <= column && column < wideTiles.GetLength(0) &&
                    0 <= row && row < wideTiles.GetLength(1));
        }

        public Vector2 GetPositionOfTile(int column, int row)
        {
            return wideTiles[column, row].Position;
        }

        public void PlayerWon(int slot)
        {
            someoneWon = true;
            winTextColor = players[slot].Color;
            winnerName = players[slot].Name;
        }

        public void PlaceWall(int slot, TileType type, int column, int row)
        {
            if (type == TileType.Vertical)
            {
                walls.Add(new VerticalWall(verticals[column, row].Position, players[slot].Color));
                verticals[column, row].IsOccupied = true;
                verticals[column, row + 1].IsOccupied = true;
            }
            else if (type == TileType.Horizontal)
            {
                walls.Add(new HorizontalWall(horizontals[column, row].Position, players[slot].Color));
                horizontals[column, row].IsOccupied = true;
                horizontals[column + 1, row].IsOccupied = true;
            }

            players[slot].NumberOfWalls--;
        }

        public GameBoard(List<string> playerNames, int thisClientSlot, AI.Agent agent)
        {
            clientSlot = thisClientSlot;
            spriteFont = Game1.CM.Load<SpriteFont>(@"GeonBit.UI/themes/hd/fonts/Regular");

            int numberOfPlayers = playerNames.Count;
            BuildBoard(numberOfPlayers);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new Player(playerNames[i], i, (playerNames.Count == 2 ? 10 : 5), this));
            }
            this.agent = agent;

            NetworkManager.OnActionRejected += NetworkManager_OnActionRejected;
            NetworkManager.OnNewTurn += NetworkManager_OnNewTurn;
            NetworkManager.OnPlayerMoved += NetworkManager_OnPlayerMoved;
            NetworkManager.OnNewWallPlaced += NetworkManager_OnNewWallPlaced;
            NetworkManager.OnPlayerWon += NetworkManager_OnPlayerWon;
        }

        private void NetworkManager_OnPlayerWon(object sender, PlayerWonMessage e)
        {
            PlayerWon(e.PlayerSlot);
        }

        private void NetworkManager_OnNewWallPlaced(object sender, PlaceWallMessage e)
        {
            PlaceWall(e.PlayerSlot, e.WallType, e.Column, e.Row);
        }

        private void NetworkManager_OnPlayerMoved(object sender, PlayerMoveMessage e)
        {
            wideTiles[e.Column, e.Row].IsOccupied = true;
            wideTiles[players[e.PlayerSlot].WideTilePosition.X, players[e.PlayerSlot].WideTilePosition.Y].IsOccupied = false;
            players[currentSlotTurn].WideTilePosition = new Point(e.Column, e.Row);
        }

        private void NetworkManager_OnNewTurn(object sender, NewTurnMessage e)
        {
            currentSlotTurn = e.PlayerSlot;
            numberOfTurns++;

            if (currentSlotTurn == clientSlot)
            {
                DoTurn();
            }
        }

        private void DoTurn()
        {
            AI.GameData status = new AI.GameData(players, horizontals, verticals, wideTiles, clientSlot);
            AI.Action action = agent.DoAction(status);
            PerformAction(action);
        }

        private void NetworkManager_OnActionRejected(object sender, ActionRejectMessage e)
        {
            AI.GameData status = new AI.GameData(players, horizontals, verticals, wideTiles, clientSlot);
            AI.Action action = agent.RedoAction(status);
            PerformAction(action);
        }

        private void PerformAction(AI.Action action)
        {
            if (action is AI.MoveAction)
            {
                AI.MoveAction move = (AI.MoveAction)action;
                NetworkManager.Send(new PlayerMoveMessage(move.Column, move.Row, clientSlot));
            }
            else if (action is AI.PlaceWallAction)
            {
                AI.PlaceWallAction placeWall = (AI.PlaceWallAction)action;

                TileType type = placeWall.WallAlignment == AI.WallOrientation.Horizontal ? TileType.Horizontal : TileType.Vertical;
                NetworkManager.Send(new PlaceWallMessage(type, placeWall.Column, placeWall.Row, clientSlot));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void BuildBoard(int numberOfPlayers)
        {
            const int boarderPadding = 10;
            const int tileWidth = 64;
            const int tilePadding = 16;
            for (int i = 0; i < wideTiles.GetLength(0); i++)
            {
                for (int k = 0; k < wideTiles.GetLength(1); k++)
                {
                    wideTiles[i, k] = new WideTile(new Vector2(boarderPadding + (i * (tileWidth + tilePadding)),
                        boarderPadding + (k * (tileWidth + tilePadding))));
                }
            }

            for (int i = 0; i < wideTiles.GetLength(0); i++)
            {
                wideTiles[i, 0].Color = new Color(1, 0.4f, 0.4f);
                wideTiles[i, wideTiles.GetLength(0) - 1].Color = new Color(0.4f, 0.4f, 1f);
            }

            wideTiles[4, 8].IsOccupied = true;
            wideTiles[4, 0].IsOccupied = true;

            if (numberOfPlayers == 4)
            {
                for (int i = 0; i < wideTiles.GetLength(1); i++)
                {
                    wideTiles[0, i].Color = new Color(0.8f, 0.8f, 0);
                    wideTiles[wideTiles.GetLength(1) - 1, i].Color = new Color(0.4f, 1f, 0.4f);
                }

                wideTiles[0, 4].IsOccupied = true;
                wideTiles[8, 4].IsOccupied = true;
            }

            for (int i = 0; i < verticals.GetLength(0); i++)
            {
                for (int k = 0; k < verticals.GetLength(1); k++)
                {
                    verticals[i, k] = new VerticalTile(new Vector2(boarderPadding + tileWidth + i * (tileWidth + tilePadding),
                        boarderPadding + k * (tileWidth + tilePadding)));
                }
            }

            for (int i = 0; i < verticals.GetLength(1); i++)
            {
                for (int k = 0; k < verticals.GetLength(0); k++)
                {
                    horizontals[i, k] = new HorizontalTile(new Vector2(boarderPadding + i * (tileWidth + tilePadding),
                        boarderPadding + tileWidth + k * (tileWidth + tilePadding)));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WideTile wTile in wideTiles)
            {
                wTile.Draw(spriteBatch);
            }

            foreach (GraphicalObject wall in walls)
            {
                wall.Draw(spriteBatch);
            }

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Draw(spriteBatch);
                spriteBatch.DrawString(spriteFont, players[i].Name, new Vector2(800, 150 * i + 50), players[i].Color);
                spriteBatch.DrawString(spriteFont, "Walls: " + players[i].NumberOfWalls.ToString(), new Vector2(800, 150 * i + 75), players[i].Color);
            }

            spriteBatch.DrawString(spriteFont, "-->", new Vector2(750, 150 * currentSlotTurn + 50), players[currentSlotTurn].Color);

            if (numberOfTurns >= 0)
            {
                spriteBatch.DrawString(spriteFont, "Turns: " + numberOfTurns, new Vector2(800, 10), Color.DarkGray);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, "Waiting for players..", new Vector2(725, 10), Color.DarkGray);
            }

            if (someoneWon)
            {
                OutlinedText.DrawCenteredText(spriteBatch, spriteFont, 3.5f, winnerName + " wins!", new Vector2(500, 360), winTextColor);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using QuoridorNetwork;

namespace Quoridor
{
    class GameBoard
    {
        SpriteFont mySF;
        WideTile[,] myWideTiles = new WideTile[9, 9];
        NarrowVerticalTile[,] myVerticals = new NarrowVerticalTile[8, 9];
        NarrowHorizontalTile[,] myHorizontals = new NarrowHorizontalTile[9, 8];
        List<GraphicalObject> myWalls = new List<GraphicalObject>();
        bool mySomeoneWon = false;
        Color myWinTextColor;
        string myWinnerName;

        List<Player> myPlayers = new List<Player>();
        int myCurrentSlotTurn;
        int myNumberOfTurns;

        public bool TileNotOccupied(int aColumn, int aRow)
        {
            return myWideTiles[aColumn, aRow].IsOccupied == false;
        }

        public void SetWideTilesOccupied(int aColumn, int aRow, bool aState)
        {
            myWideTiles[aColumn, aRow].IsOccupied = aState;
        }

        public bool IsWithinGameBoard(int aColumn, int aRow)
        {
            return (0 <= aColumn && aColumn < myWideTiles.GetLength(0) &&
                    0 <= aRow && aRow < myWideTiles.GetLength(1));
        }

        public Vector2 GetPositionOfTile(int aColumn, int aRow)
        {
            return myWideTiles[aColumn, aRow].Position;
        }

        public void PlayerWon(int aSlot)
        {
            mySomeoneWon = true;
            myWinTextColor = myPlayers[aSlot].Color;
            myWinnerName = myPlayers[aSlot].Name;
        }

        public void PlaceWall(int aSlot, TileType aType, int aColumn, int aRow)
        {
            if (aType == TileType.NarrowVertical)
            {
                myWalls.Add(new VerticalWall(myVerticals[aColumn, aRow].Position, myPlayers[aSlot].Color));
                myVerticals[aColumn, aRow].IsOccupied = true;
                myVerticals[aColumn, aRow + 1].IsOccupied = true;
            }
            else if (aType == TileType.NarrowHorizontal)
            {
                myWalls.Add(new HorizontalWall(myHorizontals[aColumn, aRow].Position, myPlayers[aSlot].Color));
                myHorizontals[aColumn, aRow].IsOccupied = true;
                myHorizontals[aColumn + 1, aRow].IsOccupied = true;
            }

            myPlayers[aSlot].NumberOfWalls--;
        }

        public GameBoard(List<string> aPlayerNames)
        {
            mySF = Program.Game.Content.Load<SpriteFont>(@"GeonBit.UI/themes/hd/fonts/Regular"); //How to reach from GUI lib?

            BuildBoard();
            for (int i = 0; i < aPlayerNames.Count; i++)
            {
                myPlayers.Add(new Player(aPlayerNames[i], i, (aPlayerNames.Count == 2 ? 10 : 5), this));
            }

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
            myPlayers[myCurrentSlotTurn].Position = GetPositionOfTile(e.Column, e.Row);
            MoveOccupation(e.Column, e.Row, e.PlayerSlot);
        }

        private void NetworkManager_OnNewTurn(object sender, NewTurnMessage e)
        {
            myCurrentSlotTurn = e.PlayerSlot;
            myNumberOfTurns++;
        }

        private void NetworkManager_OnActionRejected(object sender, ActionRejectMessage e)
        {
            //Call on AI to do something (Ask what AI want to do?)
            throw new NotImplementedException();
        }

        private void BuildBoard()
        {
            const int boarderPadding = 10;
            const int tileWidth = 64;
            const int tilePadding = 16;
            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                for (int k = 0; k < myWideTiles.GetLength(1); k++)
                {
                    myWideTiles[i, k] = new WideTile(new Vector2(boarderPadding + (i * (tileWidth + tilePadding)),
                        boarderPadding + (k * (tileWidth + tilePadding))));
                }
            }

            for (int i = 0; i < myWideTiles.GetLength(0); i++)
            {
                myWideTiles[i, 0].Color = new Color(1, 0.4f, 0.4f);
                myWideTiles[i, myWideTiles.GetLength(0) - 1].Color = new Color(0.4f, 0.4f, 1f);
            }

            myWideTiles[4, 8].IsOccupied = true;
            myWideTiles[4, 0].IsOccupied = true;

            if (myPlayers.Count == 4)
            {
                for (int i = 0; i < myWideTiles.GetLength(1); i++)
                {
                    myWideTiles[0, i].Color = new Color(0.8f, 0.8f, 0);
                    myWideTiles[myWideTiles.GetLength(1) - 1, i].Color = new Color(0.4f, 1f, 0.4f);
                }

                myWideTiles[0, 4].IsOccupied = true;
                myWideTiles[8, 4].IsOccupied = true;
            }

            for (int i = 0; i < myVerticals.GetLength(0); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(1); k++)
                {
                    myVerticals[i, k] = new NarrowVerticalTile(new Vector2(boarderPadding + tileWidth + i * (tileWidth + tilePadding),
                        boarderPadding + k * (tileWidth + tilePadding)));
                }
            }

            for (int i = 0; i < myVerticals.GetLength(1); i++)
            {
                for (int k = 0; k < myVerticals.GetLength(0); k++)
                {
                    myHorizontals[i, k] = new NarrowHorizontalTile(new Vector2(boarderPadding + i * (tileWidth + tilePadding),
                        boarderPadding + tileWidth + k * (tileWidth + tilePadding)));
                }
            }
        }

        public void MoveOccupation(int aColumn, int aRow, int aPlayerSlot)
        {
            myWideTiles[aColumn, aRow].IsOccupied = true;
            myWideTiles[myPlayers[aPlayerSlot].WideTilePosition.X, myPlayers[aPlayerSlot].WideTilePosition.Y].IsOccupied = false;
        }

        public void NothingHappened(int aSlotThatShouldDoSomethingNew)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch aSB)
        {
            foreach (WideTile wTile in myWideTiles)
            {
                wTile.Draw(aSB);
            }

            foreach (GraphicalObject wall in myWalls)
            {
                wall.Draw(aSB);
            }

            for (int i = 0; i < myPlayers.Count; i++)
            {
                myPlayers[i].Draw(aSB);
                aSB.DrawString(mySF, myPlayers[i].Name, new Vector2(800, 150 * i + 50), myPlayers[i].Color);
                aSB.DrawString(mySF, "Walls: " + myPlayers[i].NumberOfWalls.ToString(), new Vector2(800, 150 * i + 75), myPlayers[i].Color);
            }

            aSB.DrawString(mySF, "-->", new Vector2(750, 150 * myCurrentSlotTurn + 50), myPlayers[myCurrentSlotTurn].Color);

            if (myNumberOfTurns >= 0)
            {
                aSB.DrawString(mySF, "Turns: " + myNumberOfTurns, new Vector2(800, 10), Color.DarkGray);
            }
            else
            {
                aSB.DrawString(mySF, "Waiting for players..", new Vector2(725, 10), Color.DarkGray);
            }

            if (mySomeoneWon)
            {
                OutlinedText.DrawCenteredText(aSB, mySF, 3.5f, myWinnerName + " wins!", new Vector2(500, 360), myWinTextColor);
            }
        }
    }
}

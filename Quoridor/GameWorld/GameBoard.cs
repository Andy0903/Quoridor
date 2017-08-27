using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Quoridor
{
    public class GameBoard
    {
        private SpriteFont mySF;
        WideTile[,] myWideTiles = new WideTile[9, 9];
        public NarrowVerticalTile[,] myVerticals = new NarrowVerticalTile[8, 9];
        public NarrowHorizontalTile[,] myHorizontals = new NarrowHorizontalTile[9, 8];
        List<GraphicalObject> myWalls = new List<GraphicalObject>();
        bool mySomeoneWon = false;
        Color myWinTextColor;
        string myWinnerName;

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
            myWinTextColor = PlayerManager.myPlayers[aSlot].Color;
            myWinnerName = PlayerManager.myPlayers[aSlot].Name;
        }

        public void PlaceWall(int aSlot, Tile.TileType aType, int aColumn, int aRow)
        {
            if (aType == Tile.TileType.NarrowVertical)
            {
                myWalls.Add(new VerticalWall(myVerticals[aColumn, aRow].Position, PlayerManager.myPlayers[aSlot].Color));
                myVerticals[aColumn, aRow].IsOccupied = true;
                myVerticals[aColumn, aRow + 1].IsOccupied = true;
            }
            else if (aType == Tile.TileType.NarrowHorizontal)
            {
                myWalls.Add(new HorizontalWall(myHorizontals[aColumn, aRow].Position, PlayerManager.myPlayers[aSlot].Color));
                myHorizontals[aColumn, aRow].IsOccupied = true;
                myHorizontals[aColumn + 1, aRow].IsOccupied = true;
            }

            PlayerManager.myPlayers[aSlot].NumberOfWalls--;
        }

        public GameBoard()
        {
            mySF = Program.Game.Content.Load<SpriteFont>(@"GeonBit.UI/themes/hd/fonts/Regular");
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

            if (Program.Game.PlayerNumbers == NumberOfPlayers.FourPlayers)
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

        public void MoveOccupation(int movedToColumn, int movedToRow, int oldColumn, int oldRow)
        {
            myWideTiles[movedToColumn, movedToRow].IsOccupied = true;
            myWideTiles[oldColumn, oldRow].IsOccupied = false;
        }

        public void Update()
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.K)) //DEBUGGING
            //{
            //    PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].PlaceWall(Tile.TileType.NarrowVertical, 4, 4);
            //    // PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].PlaceWall(Tile.TileType.NarrowHorizontal, 4, 4);
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //{
            //    PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].Move(4, 7);
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.B))
            //{
            //    PlayerWon(0);
            //}
        }

        public void Draw(SpriteBatch aSB)
        {
            foreach (WideTile wTile in myWideTiles)
            {
                wTile.Draw(aSB);
            }

            //foreach (NarrowVerticalTile vTile in myVerticals)
            //{
            //    vTile.Draw(aSB);
            //}

            //foreach (NarrowHorizontalTile hTile in myHorizontals)
            //{
            //    hTile.Draw(aSB);
            //} 
                 
            foreach (GraphicalObject wall in myWalls)
            {
                wall.Draw(aSB);
            }

            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
            {
                aSB.DrawString(mySF, PlayerManager.myPlayers[i].Name, new Vector2(800, 150 * i + 50), PlayerManager.myPlayers[i].Color);
                aSB.DrawString(mySF, "Walls: " + PlayerManager.myPlayers[i].NumberOfWalls.ToString(), new Vector2(800, 150 * i + 75), PlayerManager.myPlayers[i].Color);
            }
            aSB.DrawString(mySF, "-->", new Vector2(750, 150 * PlayerManager.CurrentSlotTurn + 50), PlayerManager.myPlayers[PlayerManager.CurrentSlotTurn].Color);

            if (PlayerManager.NumberOfTurns >= 0)
            {
                aSB.DrawString(mySF, "Turns: " + PlayerManager.NumberOfTurns, new Vector2(800, 10), Color.DarkGray);
            }
            else
            {
                aSB.DrawString(mySF, "Waiting for players..", new Vector2(725, 10), Color.DarkGray);
            }

            if (mySomeoneWon == true)
            {
                OutlinedText.DrawCenteredText(aSB, mySF, 3.5f, myWinnerName + " wins!", new Vector2(500, 360), myWinTextColor);
            }
        }
    }
}

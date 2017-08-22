using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Quoridor
{
    public class GameBoard
    {
        private SpriteFont mySF;
        WideTile[,] myWideTiles = new WideTile[9, 9];
        NarrowVerticalTile[,] myVerticals = new NarrowVerticalTile[8, 9];
        NarrowHorizontalTile[,] myHorizontals = new NarrowHorizontalTile[9, 8];
        List<Wall> myWalls = new List<Wall>();

        public Vector2 GetPositionOfTile(int aColumn, int aRow)
        {
            return myWideTiles[aColumn, aRow].Position;
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

            if (Program.Game.PlayerNumbers == NumberOfPlayers.FourPlayers)
            {
                for (int i = 0; i < myWideTiles.GetLength(1); i++)
                {
                    myWideTiles[0, i].Color = new Color(0.8f, 0.8f, 0);
                    myWideTiles[myWideTiles.GetLength(1) - 1, i].Color = new Color(0.4f, 1f, 0.4f);
                }
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

        public void Update()
        {

        }

        public void Draw(SpriteBatch aSB)
        {
            foreach (WideTile wTile in myWideTiles)
            {
                wTile.Draw(aSB);
            }

            foreach (NarrowVerticalTile vTile in myVerticals)
            {
                vTile.Draw(aSB);
            }

            foreach (NarrowHorizontalTile hTile in myHorizontals)
            {
                hTile.Draw(aSB);
            }

            for (int i = 0; i < PlayerManager.myPlayers.Count; i++)
            {
                aSB.DrawString(mySF, PlayerManager.myPlayers[i].Name, new Vector2(800, 150 * i + 50), PlayerManager.myPlayers[i].Color);
                aSB.DrawString(mySF, "Walls: " + PlayerManager.myPlayers[i].NumberOfWalls.ToString(), new Vector2(800, 150 * i + 75), PlayerManager.myPlayers[i].Color);
            }
        }
    }
}

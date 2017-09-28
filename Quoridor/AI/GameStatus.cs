using System.Collections.Generic;
using System.Linq;

namespace Quoridor.AI
{
    /// <summary>
    /// Class used to contain information/status about the game.
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// Horizontally aligned walls pixel positions, quick way to get player input working.
        /// </summary>
        public Point[,] HorizontalWallPixelPosition { get; private set; }
        /// <summary>
        /// Vertically aligned walls pixel positions, quick way to get player input working.
        /// </summary>
        public Point[,] VerticalWallPixelPosition { get; private set; }
        /// <summary>
        /// List of all the players.
        /// </summary>
        public List<Player> Players { get; private set; }
        /// <summary>
        /// Horizontally aligned walls.
        /// </summary>
        public bool[,] HorizontalWall { get; private set; }
        /// <summary>
        /// Vertically aligned walls.
        /// </summary>
        public bool[,] VerticalWall { get; private set; }
        /// <summary>
        /// 2D array of all the walkable tiles.
        /// </summary>
        public Tile[,] Tiles { get; private set; }
        /// <summary>
        /// The agent itself.
        /// </summary>
        public Player Self { get; private set; }

        public GameData(List<Quoridor.Player> players, HorizontalTile[,] hTiles, VerticalTile[,] vTiles, WideTile[,] wTiles, int slot)
        {
            Players = players.Select(p => new Player(p.NumberOfWalls, p.WideTilePosition, p.Color)).ToList();
            Self = Players[slot];

            HorizontalWall = new bool[hTiles.GetLength(0), hTiles.GetLength(1)];
            HorizontalWallPixelPosition = new Point[hTiles.GetLength(0), hTiles.GetLength(1)];

            for (int i = 0; i < hTiles.GetLength(0); i++)
            {
                for (int k = 0; k < hTiles.GetLength(1); k++)
                {
                    HorizontalWall[i, k] = hTiles[i, k].IsOccupied;
                    HorizontalWallPixelPosition[i, k] = hTiles[i, k].Position;
                }
            }

            VerticalWall = new bool[vTiles.GetLength(0), vTiles.GetLength(1)];
            VerticalWallPixelPosition = new Point[vTiles.GetLength(0), vTiles.GetLength(1)];

            for (int i = 0; i < vTiles.GetLength(0); i++)
            {
                for (int k = 0; k < vTiles.GetLength(1); k++)
                {
                    VerticalWall[i, k] = vTiles[i, k].IsOccupied;
                    VerticalWallPixelPosition[i, k] = vTiles[i, k].Position;
                }
            }

            Tiles = new Tile[wTiles.GetLength(0), wTiles.GetLength(1)];

            for (int i = 0; i < wTiles.GetLength(0); i++)
            {
                for (int k = 0; k < wTiles.GetLength(1); k++)
                {
                    Tiles[i, k] = new Tile(wTiles[i, k].IsOccupied, new Microsoft.Xna.Framework.Point(i, k), wTiles[i, k].Color);
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.AI
{
    class DeterministicAgent : Agent
    {
        int roundCounter = 0;

        public Action Behavior(GameStatus status)
        {
            if (roundCounter % 2 == 0)
            {
                roundCounter++;
                Point nextTile = (Point)PathToGoal(status);
                return new MoveAction(nextTile.X, nextTile.Y);
            }
            else
            {
                roundCounter++;
                QuoridorNetwork.TileType alignment;
                Point placeWallAt = (Point)ShortestPathToGoalForOpponents(status, out alignment);
                return new PlaceWallAction(placeWallAt.X, placeWallAt.Y, alignment);
            }
        }

        private Point? ShortestPathToGoalForOpponents(GameStatus s, out QuoridorNetwork.TileType type) //Stupid, only cares about the last player in the list.
        {
            Dictionary<Point, Point> shortestPath = new Dictionary<Point, Point>();
            SortedSet<Point> S = new SortedSet<Point>();
            Queue<Point> Q = new Queue<Point>();

            foreach (Player player in s.Players)
            {
                if (player.Color == s.Self.Color)
                    continue;

                Point root = player.Position;
                S.Add(root);
                Q.Enqueue(root);

                while (Q.Count > 0)
                {
                    Point current = Q.Dequeue();

                    if (s.Tiles[current.X, current.Y].Color == s.Self.Color)
                    {
                        Point p = current;
                        while (shortestPath.ContainsKey(p) && shortestPath[p] != root)
                        {
                            p = shortestPath[p];
                        }

                        if (player.Color == Color.Red || player.Color == Color.Blue)
                        {
                            type = QuoridorNetwork.TileType.NarrowHorizontal;
                        }
                        else
                        {
                            type = QuoridorNetwork.TileType.NarrowVertical;
                        }
                        return p;
                    }

                    foreach (Point p in GetAdjacentTiles(current, s))
                    {
                        if (S.Contains(p) == false)
                        {
                            S.Add(p);
                            Q.Enqueue(p);
                            shortestPath.Add(p, current);
                        }
                    }
                }
            }

            type = QuoridorNetwork.TileType.NarrowHorizontal;
            return null;
        }

        private Point? PathToGoal(GameStatus s)
        {
            Dictionary<Point, Point> shortestPath = new Dictionary<Point, Point>();
            SortedSet<Point> S = new SortedSet<Point>();
            Queue<Point> Q = new Queue<Point>();

            Point root = s.Self.Position;
            S.Add(root);
            Q.Enqueue(root);

            while (Q.Count > 0)
            {
                Point current = Q.Dequeue();

                if (s.Tiles[current.X, current.Y].Color == s.Self.Color)
                {
                    Point p = current;
                    while (shortestPath.ContainsKey(p) && shortestPath[p] != root)
                    {
                        p = shortestPath[p];
                    }
                    return p;
                }

                foreach (Point p in GetAdjacentTiles(current, s))
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

        private List<Point> GetAdjacentTiles(Point index, GameStatus status)
        {
            List<Point> adjacent = new List<Point>();

            if (index.Y - 1 >= 0)
            {
                if (status.HorizontalWall[index.X, index.Y - 1] == false)
                {
                    adjacent.Add(status.Tiles[index.X, index.Y - 1].Position); //Up
                }
            }
            if (index.X - 1 >= 0)
            {
                if (status.VerticalWall[index.X - 1, index.Y] == false)
                {
                    adjacent.Add(status.Tiles[index.X - 1, index.Y].Position); //Left
                }
            }
            if (index.Y + 1 < status.Tiles.GetLength(1))
            {
                if (status.HorizontalWall[index.Y, index.X] == false)
                {
                    adjacent.Add(status.Tiles[index.X, index.Y + 1].Position); //Down
                }
            }
            if (index.X + 1 < status.Tiles.GetLength(0))
            {
                if (status.VerticalWall[index.X, index.Y] == false)
                {
                    adjacent.Add(status.Tiles[index.X + 1, index.Y].Position);  //Right
                }
            }

            return adjacent;
        }
    }
}

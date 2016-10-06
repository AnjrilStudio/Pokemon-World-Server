using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class PositionUtils
    {
        public static Position GetDirPosition(Direction dir, bool inverted)
        {
            var inversion = inverted ? -1 : 1;

            switch (dir)
            {
                case Direction.Up:
                    return new Position(0, inversion * 1);
                case Direction.Right:
                    return new Position(1, 0);
                case Direction.Down:
                    return new Position(0, inversion * -1);
                case Direction.Left:
                    return new Position(-1, 0);
                default:
                    return new Position(0, 0);
            }
        }

        public static int[,] InitShortestPath(Arena arena, Position origin, Position target, out List<Position> pathToTarget, bool pokemonObstacle)
        {
            int[,] distanceMatrix = new int[arena.ArenaSize, arena.ArenaSize];

            for (int i = 0; i < arena.ArenaSize; i++)
            {
                for (int j = 0; j < arena.ArenaSize; j++)
                {
                    distanceMatrix[i, j] = 99999;
                }
            }

            distanceMatrix[origin.X, origin.Y] = 0;
            pathToTarget = updateDist(arena, distanceMatrix, origin.X, origin.Y, new List<Position>(), target, pokemonObstacle);
            return distanceMatrix;

        }

        public static int[,] InitShortestPath(Arena arena, Position origin, bool pokemonObstacle)
        {
            List<Position> path;
            return InitShortestPath(arena, origin, null, out path, pokemonObstacle);
        }

        private static List<Position> updateDist(Arena arena, int[,] matrix, int x, int y, List<Position> path, Position target, bool pokemonObstacle)
        {
            List<Position> result = null;
            if (target != null && target.X == x && target.Y == y)
            {
                return new List<Position>(path);
            }

            int dist = matrix[x, y];

            var xa = x - 1;
            var ya = y;
            var xb = x;
            var yb = y - 1;
            var xc = x + 1;
            var yc = y;
            var xd = x;
            var yd = y + 1;

            if (IsShortestPathValidTile(arena, xa, ya, pokemonObstacle) && matrix[xa, ya] > dist + 1)
            {
                matrix[xa, ya] = dist + 1;
                path.Add(new Position(xa, ya));
                var res = updateDist(arena, matrix, xa, ya, path, target, pokemonObstacle);
                if (res != null) result = res;
                path.RemoveAt(path.Count - 1);
            }

            if (IsShortestPathValidTile(arena, xb, yb, pokemonObstacle) && matrix[xb, yb] > dist + 1)
            {
                matrix[xb, yb] = dist + 1;
                path.Add(new Position(xb, yb));
                var res = updateDist(arena, matrix, xb, yb, path, target, pokemonObstacle);
                if (res != null) result = res;
                path.RemoveAt(path.Count - 1);
            }

            if (IsShortestPathValidTile(arena, xc, yc, pokemonObstacle) && matrix[xc, yc] > dist + 1)
            {
                matrix[xc, yc] = dist + 1;
                path.Add(new Position(xc, yc));
                var res = updateDist(arena, matrix, xc, yc, path, target, pokemonObstacle);
                if (res != null) result = res;
                path.RemoveAt(path.Count - 1);
            }

            if (IsShortestPathValidTile(arena, xd, yd, pokemonObstacle) && matrix[xd, yd] > dist + 1)
            {
                matrix[xd, yd] = dist + 1;
                path.Add(new Position(xd, yd));
                var res = updateDist(arena, matrix, xd, yd, path, target, pokemonObstacle);
                if (res != null) result = res;
                path.RemoveAt(path.Count - 1);
            }

            return result;
        }

        private static bool IsShortestPathValidTile(Arena arena, int x, int y, bool pokemonObstacle)
        {
            return Position.isInMap(x, y, arena.ArenaSize) && !(pokemonObstacle && arena.Pokemons[x, y] != null);
        }
    }
}

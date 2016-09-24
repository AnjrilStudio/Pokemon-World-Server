using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class DirectionUtils
    {
        #region private fields

        private static readonly Random RAND = new Random();

        #endregion

        #region random methods

        public static Direction RandomDirection()
        {
            switch (RAND.Next(4))
            {
                case 0:
                    return Direction.Down;
                case 1:
                    return Direction.Up;
                case 2:
                    return Direction.Right;
                case 3:
                    return Direction.Left;
                default:
                    return Direction.None;
            }
        }

        #endregion

        #region serialization methods

        public static string ToString(Direction dir)
        {
            switch (dir)
            {
                case Direction.Down:
                    return "D";
                case Direction.Up:
                    return "U";
                case Direction.Right:
                    return "R";
                case Direction.Left:
                    return "L";
                default:
                    return "N";
            }
        }

        public static Direction FromString(string dir)
        {
            switch (dir)
            {
                case "D":
                    return Direction.Down;
                case "U":
                    return Direction.Up;
                case "R":
                    return Direction.Right;
                case "L":
                    return Direction.Left;
                default:
                    return Direction.None;
            }
        }

        #endregion

        public static Direction FromPosition(Position p1, Position p2)
        {
            if (Position.Distance(p1, p2) != 1)
            {
                return Direction.None;
            }

            if (p1.X > p2.X)
            {
                return Direction.Left;
            }
            if (p1.X < p2.X)
            {
                return Direction.Right;
            }
            if (p1.Y > p2.Y)
            {
                return Direction.Up;
            }
            if (p1.Y < p2.Y)
            {
                return Direction.Down;
            }

            return Direction.None;
        }
    }
}

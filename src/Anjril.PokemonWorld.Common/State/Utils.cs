using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.State
{

    public class Utils
    {
        private static Random random = new Random();

        public static Direction RandomDirection()
        {
            switch (random.Next(4))
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

        public static string DirectionToString(Direction dir)
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

        public static Direction DirectionFromString(string dir)
        {
            switch (dir)
            {
                case "Down":
                    return Direction.Down;
                case "Up":
                    return Direction.Up;
                case "Right":
                    return Direction.Right;
                case "Left":
                    return Direction.Left;
                default:
                    return Direction.None;
            }
        }

        public static Position GetDirPosition(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return new Position(0, 1);
                case Direction.Right:
                    return new Position(1, 0);
                case Direction.Down:
                    return new Position(0, -1);
                case Direction.Left:
                    return new Position(-1, 0);
                default:
                    return new Position(0, 0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.State
{

    public class Utils
    {
        private static Random random = new Random();

        public static Direction RandomDirection()
        {
            switch (random.Next(4))
            {
                case 0:
                    return Direction.DOWN;
                case 1:
                    return Direction.UP;
                case 2:
                    return Direction.RIGHT;
                case 3:
                    return Direction.LEFT;
                default:
                    return Direction.NONE;
            }
        }

        public static string DirectionToString(Direction dir)
        {
            switch (dir)
            {
                case Direction.DOWN:
                    return "D";
                case Direction.UP:
                    return "U";
                case Direction.RIGHT:
                    return "R";
                case Direction.LEFT:
                    return "L";
                default:
                    return "N";
            }
        }
    }
}

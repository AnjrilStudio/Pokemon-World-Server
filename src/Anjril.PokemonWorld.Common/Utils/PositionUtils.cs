using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class PositionUtils
    {
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

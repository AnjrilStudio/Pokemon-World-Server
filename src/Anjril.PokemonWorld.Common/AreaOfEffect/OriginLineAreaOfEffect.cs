using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public class OriginLineAreaOfEffect : AbstractAreaOfEffect
    {
        public OriginLineAreaOfEffect()
        {
        }

        public override bool InArea(Arena arena, Position origin, Position target, Position actionOrigin, Direction dir)
        {
            switch (dir)
            {
                case Direction.Down:
                    return target.X == origin.X && target.Y > actionOrigin.Y && target.Y <= origin.Y;
                case Direction.Right:
                    return target.Y == origin.Y && target.X > actionOrigin.X && target.X <= origin.X;
                case Direction.Up:
                    return target.X == origin.X && target.Y < actionOrigin.Y && target.Y >= origin.Y;
                case Direction.Left:
                    return target.Y == origin.Y && target.X < actionOrigin.X && target.X >= origin.X;
                default:
                    return false;
            }
        }
    }
}

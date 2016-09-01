using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public class RightAreaOfEffect : AbstractAreaOfEffect
    {
        public int Dist { get; private set; }

        public RightAreaOfEffect(int dist)
        {
            Dist = dist;
        }

        public override bool InArea(Position origin, Position target, Position actionOrigin, Direction dir)
        {
            if (origin.Equals(target))
            {
                return true;
            }

            switch (dir)
            {
                case Direction.Up:
                    return target.X < origin.X && target.X >= origin.X - Dist && target.Y == origin.Y;
                case Direction.Right:
                    return target.X == origin.X && target.Y > origin.Y && target.Y <= origin.Y + Dist;
                case Direction.Down:
                    return target.X > origin.X && target.X <= origin.X + Dist && target.Y == origin.Y;
                case Direction.Left:
                    return target.X == origin.X && target.Y < origin.Y && target.Y >= origin.Y - Dist;
                default:
                    return false;
            }
        }
    }
}

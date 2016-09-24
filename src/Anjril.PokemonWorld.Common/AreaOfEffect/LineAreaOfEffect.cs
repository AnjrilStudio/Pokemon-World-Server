using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public class LineAreaOfEffect : AbstractAreaOfEffect
    {
        public int Dist { get; private set; }

        public LineAreaOfEffect(int dist)
        {
            Dist = dist;
        }

        public override bool InArea(BattleArena arena, Position origin, Position target, Position actionOrigin, Direction dir)
        {
            switch (dir)
            {
                case Direction.Down:
                    return target.X == origin.X && target.Y >= origin.Y && target.Y - Dist < origin.Y;
                case Direction.Right:
                    return target.Y == origin.Y && target.X >= origin.X && target.X - Dist < origin.X;
                case Direction.Up:
                    return target.X == origin.X && target.Y <= origin.Y && target.Y + Dist > origin.Y;
                case Direction.Left:
                    return target.Y == origin.Y && target.X <= origin.X && target.X + Dist > origin.X;
                default:
                    return false;
            }
        }
    }
}

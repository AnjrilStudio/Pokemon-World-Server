using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public class DistanceAreaOfEffect : AbstractAreaOfEffect
    {
        public int Dist { get; private set; }
        public int MinDist { get; private set; }

        public DistanceAreaOfEffect(int dist)
        {
            Dist = dist;
            MinDist = 0;
        }

        public DistanceAreaOfEffect(int mindist, int dist)
        {
            Dist = dist;
            MinDist = mindist;
        }

        public override bool InArea(BattleArena arena, Position origin, Position target, Position actionOrigin, Direction dir)
        {
            var dist = Math.Abs(origin.X - target.X) + Math.Abs(origin.Y - target.Y);
            if (dist <= Dist && dist >= MinDist)
            {
                return true;
            }
            return false;
        }
    }
}

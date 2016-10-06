using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class DistanceRange : AbstractRange
    {
        public int Dist { get; private set; }

        public DistanceRange(int dist)
        {
            Dist = dist;
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            var origin = self.CurrentPos;
            var dist = Math.Abs(origin.X - target.X) + Math.Abs(origin.Y - target.Y);
            if (dist <= Dist && dist != 0)
            {
                return true;
            }
            return false;
        }
    }
}

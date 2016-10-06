using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class DistanceMPRange : AbstractRange
    {
        public int Factor { get; private set; }

        public DistanceMPRange(int factor)
        {
            Factor = factor;
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            var origin = self.CurrentPos;
            var dist = Math.Abs(origin.X - target.X) + Math.Abs(origin.Y - target.Y);
            if (dist <= self.MP * Factor && dist != 0)
            {
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class DistanceMPAPRange : AbstractRange
    {
        public int Factor { get; private set; }
        public int APLimit { get; private set; }

        public DistanceMPAPRange(int factor, int apValue)
        {
            Factor = factor;
            APLimit = apValue;
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            var origin = self.CurrentPos;
            var dist = Math.Abs(origin.X - target.X) + Math.Abs(origin.Y - target.Y);
            var apValue = Math.Min(APLimit, self.AP);
            if (dist <= (self.MP + apValue) * Factor && dist != 0)
            {
                return true;
            }
            return false;
        }
    }
}

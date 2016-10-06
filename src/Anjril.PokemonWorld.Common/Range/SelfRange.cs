using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class SelfRange : AbstractRange
    {
        public SelfRange()
        {
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            return self != null && target.Equals(self.CurrentPos);
        }
    }
}

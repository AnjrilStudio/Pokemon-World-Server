using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class ArenaRange : AbstractRange
    {
        public ArenaRange()
        {
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            return true;
        }
    }
}

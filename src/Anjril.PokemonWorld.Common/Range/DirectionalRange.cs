using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public abstract class DirectionalRange : AbstractRange
    {
        public override bool InRange(Arena arena, BattleEntity self, Position target)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (InRange(arena, self, target, dir))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
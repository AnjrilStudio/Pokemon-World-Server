using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    //effets s'appliquant sur la case visée
    public abstract class GroundEffect
    {
        public abstract void apply(BattleEntity self, Position target, Direction dir, BattleArena arena);

        public void apply(BattleEntity self, Position target, BattleArena arena)
        {
            apply(self, target, Direction.None, arena);
        }
    }
}

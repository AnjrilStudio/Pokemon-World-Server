using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    //effets s'appliquant sur les cibles touchées
    public abstract class HitEffect
    {
        public abstract void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena);

        public void apply(BattleEntity self, BattleEntity target, BattleArena arena)
        {
            apply(self, target, Direction.None, arena);
        }
    }
}

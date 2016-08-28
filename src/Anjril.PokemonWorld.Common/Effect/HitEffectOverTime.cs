using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    public abstract class HitEffectOverTime : HitEffect
    {
        public int Duration { get; protected set; }

        public override void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena)
        {
            target.addOverTimeEffect(self, this, Duration);
        }

        public abstract void applyOverTime(BattleEntity self, BattleEntity target, BattleArena arena);
    }
}

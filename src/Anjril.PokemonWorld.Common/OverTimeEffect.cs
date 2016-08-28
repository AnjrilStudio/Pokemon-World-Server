using Anjril.PokemonWorld.Common.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common
{
    public class OverTimeEffect
    {
        public BattleEntity Origin { get; private set; }
        public int Duration { get; set; }
        public HitEffectOverTime Effect { get; private set; }

        public OverTimeEffect(BattleEntity origin, HitEffectOverTime effect, int duration)
        {
            Origin = origin;
            Effect = effect;
            Duration = duration;
        }
    }
}

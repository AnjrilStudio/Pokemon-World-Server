﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class MaxMPEffect : HitEffectOverTime
    {
        public int Value { get; private set; }

        public MaxMPEffect(int value, int duration)
        {
            Value = value;
            Duration = duration;
        }

        public override void applyOverTime(BattleEntity self, BattleEntity target, BattleArena arena)
        {
            target.MaxMP = target.MaxMP + Value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class StatEffect : HitEffectOverTime
    {
        public int Value { get; private set; }
        public Stat Stat { get; private set; }

        public StatEffect(Stat stat, int value, int duration)
        {
            Value = value;
            Duration = duration;
            Stat = stat;
        }

        public override void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena)
        {
            target.addOverTimeEffect(self, this, Duration);
            applyOverTime(self, target, arena);
        }

        public override void applyOverTime(BattleEntity self, BattleEntity target, BattleArena arena)
        {
            switch (Stat)
            {
                default:
                case Stat.HP:
                    // ?
                    break;
                case Stat.Attack:
                    target.AtkStage += Value;
                    break;
                case Stat.Defense:
                    target.DefStage += Value;
                    break;
                case Stat.Spe_Attack:
                    target.AtkSpeStage += Value;
                    break;
                case Stat.Spe_Defense:
                    target.DefSpeStage += Value;
                    break;
                case Stat.Speed:
                    target.SpeedStage += Value;
                    break;
            }
            
        }
    }
}

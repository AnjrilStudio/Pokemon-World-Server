using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class StatusEffect : HitEffectOverTime
    {
        private Random rnd = new Random();
        public Status Status;

        public StatusEffect(double chance, Status status, int duration)
        {
            Chance = chance;
            Status = status;
            Duration = duration;
        }

        public override void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena)
        {
            target.addOverTimeEffect(self, this, Duration, Status);
        }

        public override void applyOverTime(BattleEntity self, BattleEntity target, BattleArena arena)
        {
            switch (Status)
            {
                default:
                case Status.None:
                    break;
                case Status.Paralysis:
                    target.MaxAP += -2;
                    target.SpeedStage += -1;
                    break;
                case Status.Burn:
                    target.AtkStage -= 2;
                    target.HP -= target.MaxHP / 8;
                    break;
                case Status.Poison:
                    target.HP -= target.MaxHP / 8;
                    break;
                case Status.Freeze:
                    target.MaxAP -= 9999;
                    target.MaxMP -= 9999;
                    break;
                case Status.Sleep:
                    target.MaxAP -= 9999;
                    target.MaxMP -= 9999;
                    break;
            }

            if (target.HP < 0)
            {
                target.HP = 0;
            }
        }
    }
}

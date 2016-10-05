using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class DamageEffect : HitEffect
    {
        private Random rnd = new Random();
        public int Value { get; private set; }

        public DamageEffect(int value)
        {
            Value = value;
            Chance = 1.00;
        }

        public override void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena)
        {
            var lvl = self.Level;
            var atk = self.Atk; //gérer spe
            var def = target.Def; //gérer spe

            double damage = 2;
            damage *= (2d * lvl + 10d) / 250d;
            damage *= (double) atk / (double) def;
            damage *= Value;
            damage *= 0.85 + rnd.NextDouble() * 15 / 100; //random
                                                          //gérer stab, crit, type, etc

            int damageInt = (int)Math.Ceiling(damage);

            target.HP -= damageInt;
            if (target.HP < 0)
            {
                target.HP = 0;
            }


        }

    }
}

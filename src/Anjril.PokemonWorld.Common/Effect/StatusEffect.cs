using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class StatusEffect : HitEffect
    {
        private Random rnd = new Random();
        public int Chance { get; private set; }
        public Status Status;

        public StatusEffect(int chance, Status status)
        {
            Chance = chance;
            Status = status;
        }

        public override void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena)
        {

            //todo

        }

    }
}

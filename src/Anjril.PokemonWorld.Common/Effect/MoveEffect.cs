using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;


namespace Anjril.PokemonWorld.Common.Effect
{
    public class MoveEffect : GroundEffect
    {
        public MoveEffect()
        {
        }

        public override void apply(BattleEntity self, Position target, Direction dir, BattleArena arena)
        {
            arena.MoveBattleEntity(self, target);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    public abstract class GroundEffectOverTime : GroundEffect
    {
        public int Duration { get;  protected set; }
        public GroundEffectOverTimeId Id { get; protected set; }

        public GroundEffectOverTime(GroundEffectOverTimeId id)
        {
            Id = id;
        }

        public abstract void applyOverTime(OverTimeGroundEffect effect, BattleArena arena);

        public abstract void applyOnCollision(OverTimeGroundEffect effect, BattleEntity other, BattleArena arena);
    }
}

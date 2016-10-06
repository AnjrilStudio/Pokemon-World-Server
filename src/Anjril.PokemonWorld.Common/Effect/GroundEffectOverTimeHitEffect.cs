using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class GroundEffectOverTimeHitEffect : GroundEffectOverTime
    {
        public List<HitEffect> HitEffects { get; private set; }
        private int Count;

        public GroundEffectOverTimeHitEffect(int duration, int count)
        {
            Duration = duration;
            Count = count;
            HitEffects = new List<HitEffect>();
        }

        public override void apply(BattleEntity self, Position target, Direction dir, BattleArena arena, int turnIndex)
        {
            var effect = arena.addOverTimeGroundEffect(self, this, turnIndex, dir);
            effect.Position = target;
            effect.Duration = Duration;
            effect.Count = Count;

        }

        public override void applyOverTime(OverTimeGroundEffect effect, BattleArena arena)
        {
            //DoNothing
        }

        public override void applyOnCollision(OverTimeGroundEffect effect, BattleEntity other, BattleArena arena)
        {
            foreach(HitEffect hiteffect in HitEffects)
            {
                hiteffect.apply(effect.Origin, other, arena);
            }
        }

        public void AddHitEffect(HitEffect effect)
        {
            HitEffects.Add(effect);
        }
    }
}

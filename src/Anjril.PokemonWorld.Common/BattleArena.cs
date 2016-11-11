
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Effect;

namespace Anjril.PokemonWorld.Common
{
    public class BattleArena : Arena
    {
        private int groundEffectSequence;
        private List<OverTimeGroundEffect> overTimeGroundEffects;

        public BattleArena(int size) : base(size)
        {
            overTimeGroundEffects = new List<OverTimeGroundEffect>();
            groundEffectSequence = 0;
        }

        public override bool MoveBattleEntity(BattleEntity entity, Position target)
        {
            bool result = true;
            base.MoveBattleEntity(entity, target);

            //effets au sol
            foreach(OverTimeGroundEffect effect in overTimeGroundEffects)
            {
                if (target.Equals(effect.Position))
                {
                    effect.Effect.applyOnCollision(effect, entity, this);
                    result = false;
                }
            }

            return result;
        }

        public OverTimeGroundEffect addOverTimeGroundEffect(BattleEntity origin, GroundEffectOverTime effect, int turnIndex, Direction dir)
        {
            var res = new OverTimeGroundEffect(groundEffectSequence++, origin, effect, turnIndex, dir);
            overTimeGroundEffects.Add(res);
            return res;
        }

        public void applyOverTimeGroundEffects(int turnIndex)
        {
            List<OverTimeGroundEffect> toRemove = new List<OverTimeGroundEffect>();
            foreach (OverTimeGroundEffect effect in overTimeGroundEffects)
            {
                if (effect.Duration <= 0)
                {
                    toRemove.Add(effect);
                }
                else if (effect.TurnIndex == turnIndex)
                {
                    effect.Duration -= 1;
                    effect.Effect.applyOverTime(effect, this);
                }
            }

            foreach (OverTimeGroundEffect effect in toRemove)
            {
                overTimeGroundEffects.Remove(effect);
            }
        }

        public int GetGroundEffectMaxTurnIndex()
        {
            var max = 0;
            foreach(OverTimeGroundEffect effect in overTimeGroundEffects)
            {
                if (effect.TurnIndex > max) max = effect.TurnIndex;
            }

            return max;
        }

        public List<OverTimeGroundEffect> GetGroundEffects()
        {
            return new List<OverTimeGroundEffect>(overTimeGroundEffects);
        }
    }
    
}

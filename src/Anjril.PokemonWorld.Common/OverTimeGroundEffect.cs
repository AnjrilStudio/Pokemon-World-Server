using Anjril.PokemonWorld.Common.Effect;
using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common
{
    public class OverTimeGroundEffect
    {
        public int Id { get; private set; }
        public BattleEntity Origin { get; private set; }
        public int Duration { get; set; }
        public int Count { get; set; }
        public int TurnIndex { get; private set; }
        public GroundEffectOverTime Effect { get; private set; }
        public Position Position { get; set; }
        public Direction Direction { get; private set; }

        public OverTimeGroundEffect(int id, BattleEntity origin, GroundEffectOverTime effect, int turnIndex, Direction dir)
        {
            Id = id;
            Origin = origin;
            Effect = effect;
            TurnIndex = turnIndex;
            Direction = dir;
        }
    }
}

using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public class BattleStateGroundEffect
    {
        public int InstanceId { get; private set; }
        public int EffectId { get; private set; }
        public Position Position { get; private set; }

        public BattleStateGroundEffect(string effectStr)
        {
            var i = 0;
            InstanceId = Int32.Parse(effectStr.Split(',')[i++]);
            EffectId = Int32.Parse(effectStr.Split(',')[i++]);
            Position = new Position(effectStr.Split(',')[i++]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class APActionCost : AbstractActionCost
    {
        public int Value { get; protected set; }

        public APActionCost(int value)
        {
            Value = value;
        }

        public override void ApplyCost(BattleEntity self, Position target)
        {
            self.AP -= Value;
        }

        public override bool CheckCost(BattleEntity self, Position target)
        {
            return self.AP >= Value;
        }
    }
}

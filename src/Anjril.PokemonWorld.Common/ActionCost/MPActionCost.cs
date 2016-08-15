using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class MPActionCost : AbstractActionCost
    {
        public MPActionCost(int value)
        {
            Value = value;
        }

        public override void ApplyCost(BattleEntity self, Position target)
        {
            self.MP -= Value;
        }
    }
}

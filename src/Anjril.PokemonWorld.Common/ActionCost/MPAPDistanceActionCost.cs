using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class MPAPDistanceActionCost : AbstractActionCost
    {
        public MPAPDistanceActionCost(int value)
        {
            Value = value;
        }

        public override void ApplyCost(BattleEntity self, Position target)
        {
            int cost = Position.Distance(self.CurrentPos, target) * Value;
            self.MP -= cost;
            if (self.MP < 0)
            {
                self.AP += self.MP;
                self.MP = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class MPAPDistanceActionCost : AbstractActionCost
    {
        public int Factor { get; protected set; }

        public MPAPDistanceActionCost(int factor)
        {
            Factor = factor;
        }

        private int ComputeTotalCost(BattleEntity self, Position target)
        {
            return Position.Distance(self.CurrentPos, target) * Factor;
        }

        private int ComputeMPCost(BattleEntity self, Position target)
        {
            int mpCost = ComputeTotalCost(self, target);
            if (self.MP < mpCost)
            {
                mpCost = self.MP;
            }
            return mpCost;
        }

        private int ComputeAPCost(BattleEntity self, Position target)
        {
            int totalCost = ComputeTotalCost(self, target);
            int mpCost = totalCost;
            if (self.MP < totalCost)
            {
                mpCost = self.MP;
            }
            int apCost = totalCost - mpCost;

            return apCost;
        }

        public override void ApplyCost(BattleEntity self, Position target)
        {
            int mpCost = ComputeMPCost(self, target);
            int apCost = ComputeAPCost(self, target);
            self.MP -= mpCost;
            self.AP -= apCost;
        }

        public override bool CheckCost(BattleEntity self, Position target)
        {
            return self.MP >= ComputeMPCost(self, target) && self.AP >= ComputeAPCost(self, target);
        }
    }
}

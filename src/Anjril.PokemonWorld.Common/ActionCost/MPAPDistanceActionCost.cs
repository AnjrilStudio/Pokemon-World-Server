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

        protected virtual int ComputeTotalCost(BattleArena arena, BattleEntity self, Position target)
        {
            return Position.Distance(self.CurrentPos, target) * Factor;
        }

        private int ComputeMPCost(BattleArena arena, BattleEntity self, Position target)
        {
            int mpCost = ComputeTotalCost(arena, self, target);
            if (self.MP < mpCost)
            {
                mpCost = self.MP;
            }
            return mpCost;
        }

        private int ComputeAPCost(BattleArena arena, BattleEntity self, Position target)
        {
            int totalCost = ComputeTotalCost(arena, self, target);
            int mpCost = totalCost;
            if (self.MP < totalCost)
            {
                mpCost = self.MP;
            }
            int apCost = totalCost - mpCost;

            return apCost;
        }

        public override void ApplyCost(BattleArena arena, BattleEntity self, Position target)
        {
            int mpCost = ComputeMPCost(arena, self, target);
            int apCost = ComputeAPCost(arena, self, target);
            self.MP -= mpCost;
            self.AP -= apCost;
            self.APMP -= apCost; //garde le compte d'AP utilisé
        }

        public override bool CheckCost(BattleArena arena, BattleEntity self, Position target)
        {
            return self.MP >= ComputeMPCost(arena, self, target) && self.AP >= ComputeAPCost(arena, self, target);
        }
    }
}

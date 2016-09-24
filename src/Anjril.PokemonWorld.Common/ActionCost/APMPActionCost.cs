using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class APMPActionCost : AbstractActionCost
    {
        public int APValue { get; protected set; }
        public int MPValue { get; protected set; }

        public APMPActionCost(int APvalue, int MPvalue)
        {
            APValue = APvalue;
            MPValue = MPvalue;
        }

        public override void ApplyCost(BattleArena arena, BattleEntity self, Position target)
        {
            self.AP -= APValue;
            self.MP -= MPValue;
        }

        public override bool CheckCost(BattleArena arena, BattleEntity self, Position target)
        {
            return self.AP >= APValue && self.MP >= MPValue;
        }
    }
}

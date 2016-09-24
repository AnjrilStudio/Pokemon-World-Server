﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class MPActionCost : AbstractActionCost
    {
        public int Value { get; protected set; }

        public MPActionCost(int value)
        {
            Value = value;
        }

        public override void ApplyCost(BattleArena arena, BattleEntity self, Position target)
        {
            self.MP -= Value;
        }

        public override bool CheckCost(BattleArena arena, BattleEntity self, Position target)
        {
            return self.MP >= Value;
        }
    }
}

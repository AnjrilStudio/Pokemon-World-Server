using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public abstract class AbstractActionCost
    {
        public abstract void ApplyCost(BattleArena arena, BattleEntity self, Position target);
        public abstract bool CheckCost(BattleArena arena, BattleEntity self, Position target);
    }
}

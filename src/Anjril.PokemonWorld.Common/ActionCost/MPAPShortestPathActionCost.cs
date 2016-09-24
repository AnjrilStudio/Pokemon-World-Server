using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Common.ActionCost
{
    public class MPAPShortestPathActionCost : MPAPDistanceActionCost
    {
        public MPAPShortestPathActionCost(int factor) : base(factor)
        {
        }

        protected override int ComputeTotalCost(BattleArena arena, BattleEntity self, Position target)
        {
            List<Position> path;
            PositionUtils.InitShortestPath(arena, self.CurrentPos, target, out path, true);

            return path.Count * Factor;
        }
    }
}

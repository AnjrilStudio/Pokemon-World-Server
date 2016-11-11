using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Common.Range
{
    public class ShortestPathMPAPRange : AbstractRange
    {
        public int Factor { get; private set; }
        public int APLimit { get; private set; }

        private int[,] distanceMatrix;

        public ShortestPathMPAPRange(int factor, int apValue)
        {
            Factor = factor;
            APLimit = apValue;
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            var dist = distanceMatrix[target.X, target.Y];
            var apValue = Math.Min(APLimit + self.APMP, self.AP);
            if (dist <= (self.MP + apValue) * Factor && dist != 0)
            {
                return true;
            }
            return false;
        }

        public override void Init(Arena arena, BattleEntity self)
        {
            distanceMatrix = PositionUtils.InitShortestPath(arena, self.CurrentPos, true);
            base.Init(arena, self);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public class ShortestPathAreaOfEffect : AbstractAreaOfEffect
    {
        private List<Position> area;

        public override bool InArea(Arena arena, Position origin, Position target, Position actionOrigin, Direction dir)
        {
            return area.Contains(target);
        }

        public override void Init(Arena arena, Position target, Position actionOrigin)
        {
            PositionUtils.InitShortestPath(arena, actionOrigin, target, out area, true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class WildRange : AbstractRange
    {
        public WildRange()
        {
        }

        public override bool InRange(BattleArena arena, BattleEntity self, Position target, Direction dir)
        {
            return arena.Pokemons[target.X, target.Y] != null && arena.Pokemons[target.X, target.Y].PlayerId == -1;
        }
    }
}

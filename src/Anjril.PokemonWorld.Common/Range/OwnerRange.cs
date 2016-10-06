using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public class OwnerRange : AbstractRange
    {
        public OwnerRange()
        {
        }

        public override bool InRange(Arena arena, BattleEntity self, Position target, Direction dir)
        {
            return self != null && arena.Pokemons[target.X, target.Y] != null && arena.Pokemons[target.X, target.Y].PlayerId == self.PlayerId;
        }
    }
}

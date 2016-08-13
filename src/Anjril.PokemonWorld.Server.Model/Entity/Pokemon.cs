using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class Pokemon : WorldEntity
    {
        public int PokedexId { get; private set; }

        public Pokemon(int pokemonId) : base()
        {
            Type = EntityType.Pokemon;
            PokedexId = pokemonId;
        }
    }
}

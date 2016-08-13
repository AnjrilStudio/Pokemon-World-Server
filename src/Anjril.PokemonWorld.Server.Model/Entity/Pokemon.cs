using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class Pokemon : WorldEntity
    {
        #region public properties

        public int PokedexId { get; private set; }

        #endregion

        #region constructor

        public Pokemon(int pokemonId) : base()
        {
            Type = EntityType.Pokemon;
            PokedexId = pokemonId;
        }

        #endregion
    }
}

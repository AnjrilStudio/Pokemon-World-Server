using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Persistence.Dto
{
    public class PokemonDto
    {
        public int PokedexId { get; set; }
        public int Level { get; set; }
        public int TotalXp { get; set; }

        internal PokemonDto()
        {
            PokedexId = -1;
            Level = 1;
            TotalXp = 0;
        }
    }
}

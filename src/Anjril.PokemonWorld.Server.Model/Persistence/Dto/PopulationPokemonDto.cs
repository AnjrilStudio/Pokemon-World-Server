using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Persistence.Dto
{
    public class PopulationPokemonDto
    {
        public int PokedexId { get; set; }
        public int Level { get; set; }
        public int TotalXp { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public PositionDto Position { get; set; }
        public int NoRepTime { get; set; }

        internal PopulationPokemonDto()
        {
            PokedexId = -1;
            Level = 1;
            TotalXp = 0;
            Age = 0;
            Gender = Gender.Male;
            Position = new PositionDto();
            NoRepTime = 0;
        }
    }
}

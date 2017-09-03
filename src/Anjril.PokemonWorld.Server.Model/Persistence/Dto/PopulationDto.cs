using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Persistence.Dto
{
    public class PopulationDto
    {
        public List<PopulationPokemonDto> WorldPopulation { get; private set; }

        public PopulationDto()
        {
            WorldPopulation = new List<PopulationPokemonDto>();
            foreach(Pokemon pokemon in World.Instance.Population)
            {
                var dto = new PopulationPokemonDto();
                dto.Level = pokemon.Level;
                dto.PokedexId = pokemon.PokedexSheet.NationalId;
                dto.TotalXp = pokemon.Xp;
                dto.Position.X = pokemon.HiddenPosition.X;
                dto.Position.Y = pokemon.HiddenPosition.Y;
                dto.Gender = pokemon.Gender;
                dto.NoRepTime = pokemon.NoRepTime;
                dto.Age = pokemon.Age;

                WorldPopulation.Add(dto);
            }
            
        }
    }
}

using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Server.Model.Persistence.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Persistence
{
    interface IPopulationDao
    {
        void SavePopulation(PopulationDto population);

        PopulationDto LoadPopulation();
    }
}

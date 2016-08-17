using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Server.Model.Index.Evolution
{
    public interface IEvolutionCondition
    {
        bool Fake { get; set; }
    }
}
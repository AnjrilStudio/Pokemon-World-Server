using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Index.Evolution
{
    public interface IEvolutionCondition
    {
        bool Fake { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Index.Evolution
{
    class FakeCondition : IEvolutionCondition
    {
        public bool Fake { get; set; }
    }
}

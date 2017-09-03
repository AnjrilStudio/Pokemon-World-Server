using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Persistence.Dto
{
    public class PositionDto
    {
        public int X { get; set; }
        public int Y { get; set; }

        internal PositionDto()
        {
            X = 0;
            Y = 0;
        }
    }
}

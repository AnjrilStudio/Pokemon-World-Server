using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public abstract class AbstractAreaOfEffect
    {
        public abstract bool InArea(Position origin, Position target, Position actionOrigin, Direction dir);

        public bool InArea(Position origin, Position target, Position actionOrigin)
        {
            return InArea(origin, target, actionOrigin, Direction.None);
        }
    }
}

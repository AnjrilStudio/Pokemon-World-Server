using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public abstract class AbstractAreaOfEffect
    {
        public int MaxArea { get; protected set; }

        public abstract bool InArea(Position origin, Position target, Direction dir);

        public bool InArea(Position origin, Position target)
        {
            return InArea(origin, target, Direction.None);
        }
    }
}

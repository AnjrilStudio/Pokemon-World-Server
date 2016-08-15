using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class StateUtils
    {
        #region serialization methods

        public static string ToString(EntityState state)
        {
            switch (state)
            {
                case EntityState.Flying:
                    return "F";
                case EntityState.Swimming:
                    return "S";
                case EntityState.Walking:
                    return "W";
                default:
                    return "U";
            }
        }

        public static EntityState FromString(string state)
        {
            switch (state)
            {
                case "F":
                    return EntityState.Flying;
                case "S":
                    return EntityState.Swimming;
                case "W":
                    return EntityState.Walking;
                default:
                    return EntityState.Undefined;
            }
        }

        #endregion
    }
}

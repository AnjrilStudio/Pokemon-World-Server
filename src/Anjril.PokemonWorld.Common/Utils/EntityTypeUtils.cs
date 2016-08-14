using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class EntityTypeUtils
    {
        #region serialization methods

        public static string ToString(EntityType entity)
        {
            switch (entity)
            {
                case EntityType.Player:
                    return "Play";
                case EntityType.Pokemon:
                    return "Poke";
                default:
                    return "None";
            }
        }

        public static EntityType FromString(string entity)
        {
            switch (entity)
            {
                case "Play":
                    return EntityType.Player;
                case "Poke":
                    return EntityType.Pokemon;
                default:
                    return EntityType.Undefined;
            }
        }

        #endregion
    }
}

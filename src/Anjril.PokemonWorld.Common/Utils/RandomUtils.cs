using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class RandomUtils
    {
        #region private field

        private static readonly Random RAND = new Random();

        #endregion

        #region methods

        public static bool CoinToss()
        {
            return RAND.Next(2) == 0 ? true : false;
        }

        public static int RandomInt(int maxValue)
        {
            return RAND.Next(maxValue);
        }

        public static double RandomDouble()
        {
            return RAND.NextDouble();
        }

        #endregion
    }
}

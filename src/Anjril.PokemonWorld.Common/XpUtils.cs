using Anjril.PokemonWorld.Common.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common
{
    public static class XpUtils
    {
        public static int getXpForLevel(int level, XpFormula formula)
        {
            double levelDbl = Convert.ToDouble(level);
            double result = -1;
            switch (formula)
            {
                case XpFormula.Medium_Fast:
                    result = Math.Pow(levelDbl, 3);
                    break;
                default:
                    Console.WriteLine("Aucune formule d'xp correspondante");
                    break;
            }

            return (int)Math.Floor(result);
        }

        public static int getXpGain(BattleEntity faintedPokemon)
        {
            int baseXpGain = Pokedex.GetPokemonSheetByNationalId(faintedPokemon.PokedexId).XpGain;
            return baseXpGain * faintedPokemon.Level / 7;
        }
    }
}
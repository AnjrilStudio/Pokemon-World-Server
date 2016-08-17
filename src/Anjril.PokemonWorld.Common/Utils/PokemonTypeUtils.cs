using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class PokemonTypeUtils
    {
        #region private fields

        private static Dictionary<PokemonType, Dictionary<PokemonType, double>> TYPE_CHART;

        static PokemonTypeUtils()
        {
            TYPE_CHART = new Dictionary<PokemonType, Dictionary<PokemonType, double>>();

            TYPE_CHART.Add(PokemonType.Normal, GetTypeChart(rockModifier: 0.5, ghostModifier: 0, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Fire, GetTypeChart(fireModifier: 0.5, waterModifier: 0.5, grassModifier: 2, iceModifier: 2, bugModifier: 2, rockModifier: 0.5, dragonModifier: 0.5, steelModifier: 2));
            TYPE_CHART.Add(PokemonType.Water, GetTypeChart(fireModifier: 2, waterModifier: 0.5, grassModifier: 0.5, groundModifier: 0.5, rockModifier: 2, dragonModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Electric, GetTypeChart(waterModifier: 2, elecModifier: 0.5, grassModifier: 0.5, groundModifier: 0, flyModifier: 2, dragonModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Grass, GetTypeChart(fireModifier: 0.5, elecModifier: 2, grassModifier: 0.5, poisonModifier: 0.5, groundModifier: 2, flyModifier: 0.5, bugModifier: 0.5, rockModifier: 2, dragonModifier: 0.5, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Ice, GetTypeChart(fireModifier: 0.5, waterModifier: 0.5, grassModifier: 2, iceModifier: 0.5, groundModifier: 2, flyModifier: 2, dragonModifier: 2, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Fighting, GetTypeChart(normalModifier: 2, iceModifier: 2, fightModifier: 0.5, poisonModifier: 0.5, flyModifier: 0.5, psyModifier: 0.5, bugModifier: 0.5, rockModifier: 2, ghostModifier: 0, darkModifier: 2, steelModifier: 2, fairyModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Poison, GetTypeChart(grassModifier: 2, poisonModifier: 0.5, groundModifier: 0.5, rockModifier: 0.5, ghostModifier: 0.5, steelModifier: 0, fairyModifier: 2));
            TYPE_CHART.Add(PokemonType.Ground, GetTypeChart(fireModifier: 2, elecModifier: 2, grassModifier: 0.5, poisonModifier: 2, flyModifier: 0, bugModifier: 0.5, rockModifier: 2, steelModifier: 2));
            TYPE_CHART.Add(PokemonType.Flying, GetTypeChart(elecModifier: 0.5, grassModifier: 2, fightModifier: 2, bugModifier: 2, rockModifier: 0.5, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Psychic, GetTypeChart(fightModifier: 2, poisonModifier: 2, psyModifier: 0.5, darkModifier: 0, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Bug, GetTypeChart(waterModifier: 0.5, grassModifier: 2, fightModifier: 0.5, poisonModifier: 0.5, flyModifier: 0.5, psyModifier: 2, ghostModifier: 0.5, darkModifier: 2, steelModifier: 0.5, fairyModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Rock, GetTypeChart(fireModifier: 2, iceModifier: 2, fightModifier: 0.5, flyModifier: 2, bugModifier: 2, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Ghost, GetTypeChart(normalModifier: 0, psyModifier: 2, ghostModifier: 2, darkModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Dragon, GetTypeChart(dragonModifier: 2, steelModifier: 0.5, fairyModifier: 0));
            TYPE_CHART.Add(PokemonType.Dark, GetTypeChart(fightModifier: 0.5, psyModifier: 2, ghostModifier: 2, darkModifier: 0.5, fairyModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Steel, GetTypeChart(fireModifier: 0.5, waterModifier: 0.5, grassModifier: 0.5, iceModifier: 2, rockModifier: 2, steelModifier: 0.5, fairyModifier: 2));
            TYPE_CHART.Add(PokemonType.Fairy, GetTypeChart(fireModifier: 0.5, fightModifier: 2, poisonModifier: 0.5, dragonModifier: 2, darkModifier: 2, steelModifier: 0.5));
            TYPE_CHART.Add(PokemonType.Unknown, GetTypeChart());
        }

        private static Dictionary<PokemonType, double> GetTypeChart(double normalModifier = 1, double fireModifier = 1, double waterModifier = 1, double elecModifier = 1, double grassModifier = 1, double iceModifier = 1, double fightModifier = 1, double poisonModifier = 1, double groundModifier = 1, double flyModifier = 1, double psyModifier = 1, double bugModifier = 1, double rockModifier = 1, double ghostModifier = 1, double dragonModifier = 1, double darkModifier = 1, double steelModifier = 1, double fairyModifier = 1)
        {
            var result = new Dictionary<PokemonType, double>();

            result.Add(PokemonType.Normal, normalModifier);
            result.Add(PokemonType.Fire, fireModifier);
            result.Add(PokemonType.Water, waterModifier);
            result.Add(PokemonType.Electric, elecModifier);
            result.Add(PokemonType.Grass, grassModifier);
            result.Add(PokemonType.Ice, iceModifier);
            result.Add(PokemonType.Fighting, fightModifier);
            result.Add(PokemonType.Poison, poisonModifier);
            result.Add(PokemonType.Ground, groundModifier);
            result.Add(PokemonType.Flying, flyModifier);
            result.Add(PokemonType.Psychic, psyModifier);
            result.Add(PokemonType.Bug, bugModifier);
            result.Add(PokemonType.Rock, rockModifier);
            result.Add(PokemonType.Ghost, ghostModifier);
            result.Add(PokemonType.Dragon, dragonModifier);
            result.Add(PokemonType.Dark, darkModifier);
            result.Add(PokemonType.Steel, steelModifier);
            result.Add(PokemonType.Fairy, fairyModifier);
            result.Add(PokemonType.Unknown, 1);

            return result;
        }

        #endregion

        #region effectiveness methods

        public static double GetEffectiveness(PokemonType attack, PokemonType defense)
        {
            return GetEffectiveness(attack, defense, PokemonType.Unknown);
        }

        public static double GetEffectiveness(PokemonType attack, PokemonType defense1, PokemonType defense2)
        {
            return TYPE_CHART[attack][defense1] * TYPE_CHART[attack][defense2];
        }

        #endregion

        #region serialization methods

        public static string ToString(PokemonType type)
        {
            switch (type)
            {
                case PokemonType.Normal:
                    return "N";
                case PokemonType.Fire:
                    return "F1";
                case PokemonType.Water:
                    return "W";
                case PokemonType.Electric:
                    return "E";
                case PokemonType.Grass:
                    return "G1";
                case PokemonType.Ice:
                    return "I";
                case PokemonType.Fighting:
                    return "F2";
                case PokemonType.Poison:
                    return "P1";
                case PokemonType.Ground:
                    return "G2";
                case PokemonType.Flying:
                    return "F3";
                case PokemonType.Psychic:
                    return "P2";
                case PokemonType.Bug:
                    return "B";
                case PokemonType.Rock:
                    return "R";
                case PokemonType.Ghost:
                    return "G3";
                case PokemonType.Dragon:
                    return "D1";
                case PokemonType.Dark:
                    return "D2";
                case PokemonType.Steel:
                    return "S";
                case PokemonType.Fairy:
                    return "F4";
                default:
                    return "U";
            }
        }

        public static PokemonType FromString(string type)
        {
            switch (type)
            {
                case "N":
                    return PokemonType.Normal;
                case "F1":
                    return PokemonType.Fire;
                case "W":
                    return PokemonType.Water;
                case "E":
                    return PokemonType.Electric;
                case "G1":
                    return PokemonType.Grass;
                case "I":
                    return PokemonType.Ice;
                case "F2":
                    return PokemonType.Fighting;
                case "P1":
                    return PokemonType.Poison;
                case "G2":
                    return PokemonType.Ground;
                case "F3":
                    return PokemonType.Flying;
                case "P2":
                    return PokemonType.Psychic;
                case "B":
                    return PokemonType.Bug;
                case "R":
                    return PokemonType.Rock;
                case "G3":
                    return PokemonType.Ghost;
                case "D1":
                    return PokemonType.Dragon;
                case "D2":
                    return PokemonType.Dark;
                case "S":
                    return PokemonType.Dark;
                case "F4":
                    return PokemonType.Fairy;
                default:
                    return PokemonType.Unknown;
            }
        }

        #endregion
    }
}

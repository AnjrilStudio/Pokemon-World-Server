using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Utils
{
    public static class NatureTypeUtils
    {
        #region nmod list

        private static List<NatureType> ATTACK_INC = new List<NatureType> { NatureType.Lonely, NatureType.Adamant, NatureType.Naughty, NatureType.Brave };
        private static List<NatureType> ATTACK_DEC = new List<NatureType> { NatureType.Bold, NatureType.Modest, NatureType.Calm, NatureType.Timid };

        private static List<NatureType> DEFENSE_INC = new List<NatureType> { NatureType.Bold, NatureType.Impish, NatureType.Lax, NatureType.Relaxed };
        private static List<NatureType> DEFENSE_DEC = new List<NatureType> { NatureType.Lonely, NatureType.Mild, NatureType.Gentle, NatureType.Hasty };

        private static List<NatureType> SPE_ATTACK_INC = new List<NatureType> { NatureType.Modest, NatureType.Mild, NatureType.Rash, NatureType.Quiet };
        private static List<NatureType> SPE_ATTACK_DEC = new List<NatureType> { NatureType.Adamant, NatureType.Impish, NatureType.Carefull, NatureType.Jolly };

        private static List<NatureType> SPE_DEFENSE_INC = new List<NatureType> { NatureType.Calm, NatureType.Gentle, NatureType.Carefull, NatureType.Sassy };
        private static List<NatureType> SPE_DEFENSE_DEC = new List<NatureType> { NatureType.Naughty, NatureType.Lax, NatureType.Rash, NatureType.Naive };

        private static List<NatureType> VELOCITY_INC = new List<NatureType> { NatureType.Timid, NatureType.Hasty, NatureType.Jolly, NatureType.Naive };
        private static List<NatureType> VELOCITY_DEC = new List<NatureType> { NatureType.Brave, NatureType.Relaxed, NatureType.Quiet, NatureType.Sassy };


        #endregion

        #region nmod

        public static bool IncreaseAttack(this NatureType nature)
        {
            return ATTACK_INC.Contains(nature);
        }

        public static bool DecreaseAttack(this NatureType nature)
        {
            return ATTACK_DEC.Contains(nature);
        }

        public static double GetAttackModifier(this NatureType nature)
        {
            return nature.IncreaseAttack() ? 1.1 : nature.DecreaseAttack() ? 0.9 : 1;
        }

        public static bool IncreaseDefense(this NatureType nature)
        {
            return DEFENSE_INC.Contains(nature);
        }

        public static bool DecreaseDefense(this NatureType nature)
        {
            return DEFENSE_DEC.Contains(nature);
        }

        public static double GetDefenseModifier(this NatureType nature)
        {
            return nature.IncreaseDefense() ? 1.1 : nature.DecreaseDefense() ? 0.9 : 1;
        }

        public static bool IncreaseSpeAttack(this NatureType nature)
        {
            return SPE_ATTACK_INC.Contains(nature);
        }

        public static bool DecreaseSpeAttack(this NatureType nature)
        {
            return SPE_ATTACK_DEC.Contains(nature);
        }

        public static double GetSpeAttackModifier(this NatureType nature)
        {
            return nature.IncreaseSpeAttack() ? 1.1 : nature.DecreaseSpeAttack() ? 0.9 : 1;
        }

        public static bool IncreaseSpeDefense(this NatureType nature)
        {
            return SPE_DEFENSE_INC.Contains(nature);
        }

        public static bool DecreaseSpeDefense(this NatureType nature)
        {
            return SPE_DEFENSE_DEC.Contains(nature);
        }

        public static double GetSpeDefenseModifier(this NatureType nature)
        {
            return nature.IncreaseSpeDefense() ? 1.1 : nature.DecreaseSpeDefense() ? 0.9 : 1;
        }

        public static bool IncreaseVelocity(this NatureType nature)
        {
            return VELOCITY_INC.Contains(nature);
        }

        public static bool DecreaseVelocity(this NatureType nature)
        {
            return VELOCITY_DEC.Contains(nature);
        }

        public static double GetVelocityModifier(this NatureType nature)
        {
            return nature.IncreaseVelocity() ? 1.1 : nature.DecreaseVelocity() ? 0.9 : 1;
        }

        #endregion

        #region serialization methods

        public static string ToString(NatureType nature)
        {
            return nature.ToString();
        }

        public static NatureType FromString(string nature)
        {
            return (NatureType)Enum.Parse(typeof(NatureType), nature);
        }

        #endregion
    }
}
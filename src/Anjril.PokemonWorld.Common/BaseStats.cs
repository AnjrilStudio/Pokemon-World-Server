using Anjril.PokemonWorld.Common.Index;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common
{
    public class BaseStats
    {
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpeAttack { get; set; }
        public int SpeDefense { get; set; }
        public int Speed { get; set; }

        public BaseStats()
        {

        }

        public BaseStats(int pokedexId, int level, BaseStats iv, BaseStats ev, NatureType nature)
        {
            var pokedexSheet = Pokedex.GetPokemonSheetByNationalId(pokedexId);

            HP = (pokedexSheet.BaseCharacteristic.HP * 2 + iv.HP + (ev.HP / 4)) * level / 100 + 10 + level;
            Attack = (int)(((pokedexSheet.BaseCharacteristic.Attack * 2 + iv.Attack + (ev.Attack / 4)) * level / 100 + 5) * nature.GetAttackModifier());
            Defense = (int)(((pokedexSheet.BaseCharacteristic.Defense * 2 + iv.Defense + (ev.Defense / 4)) * level / 100 + 5) * nature.GetDefenseModifier());
            SpeAttack = (int)(((pokedexSheet.BaseCharacteristic.SpeAttack * 2 + iv.SpeAttack + (ev.SpeAttack / 4)) * level / 100 + 5) * nature.GetSpeAttackModifier());
            SpeDefense = (int)(((pokedexSheet.BaseCharacteristic.SpeDefense * 2 + iv.SpeDefense + (ev.SpeDefense / 4)) * level / 100 + 5) * nature.GetSpeDefenseModifier());
            Speed = (int)(((pokedexSheet.BaseCharacteristic.Speed * 2 + iv.Speed + (ev.Speed / 4)) * level / 100 + 5) * nature.GetVelocityModifier());
        }

    }
}
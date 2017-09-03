using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using Anjril.PokemonWorld.Common.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Server.Model.Persistence.Dto;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class Pokemon : WorldEntity
    {
        #region public properties

        public Position HiddenPosition { get; set; }
        public bool IsVisible { get { return Position != null; } }

        public int NoRepTime { get; set; }

        public PokemonSheet PokedexSheet { get; private set; }
        public int Age { get; set; }
        public int Xp { get; set; }
        public Gender Gender { get; private set; }
        public NatureType Nature { get; private set; }

        public int Level { get; set; }
        public BaseStats EffortValues { get; set; }
        public BaseStats IndividualValues { get; set; }
        public BaseStats Characteristics
        {
            get
            {
                return new BaseStats(PokedexSheet.NationalId, Level, IndividualValues, EffortValues, Nature);
            }
        }

        public override bool CanSwim { get { return PokedexSheet.CanSwim; } }
        public override bool CanWalk { get { return PokedexSheet.CanWalk; } }
        public override bool CanFly { get { return PokedexSheet.CanFly; } }
        public override bool CanBeRidden { get { return PokedexSheet.CanBeRidden; } }

        #endregion

        #region constructor

        public Pokemon(int nationalId, Position hiddenPosition)
            : base(EntityType.Pokemon)
        {
            PokedexSheet = Pokedex.GetPokemonSheetByNationalId(nationalId);

            Level = 1;
            Xp = 0;
            Age = 1;
            Gender = RandomUtils.RandomDouble() > PokedexSheet.MaleDistribution ? Gender.Female : Gender.Male;
            Nature = (NatureType)RandomUtils.RandomInt(Enum.GetValues(typeof(NatureType)).Length);

            NoRepTime = 0;

            HiddenPosition = hiddenPosition;

            EffortValues = new BaseStats();

            IndividualValues = new BaseStats();

            IndividualValues.HP = RandomUtils.RandomInt(32);
            IndividualValues.Attack = RandomUtils.RandomInt(32);
            IndividualValues.Defense = RandomUtils.RandomInt(32);
            IndividualValues.SpeAttack = RandomUtils.RandomInt(32);
            IndividualValues.SpeDefense = RandomUtils.RandomInt(32);
            IndividualValues.Speed = RandomUtils.RandomInt(32);
        }

        public Pokemon(PopulationPokemonDto pokemonDto)
            : base(EntityType.Pokemon)
        {

            PokedexSheet = Pokedex.GetPokemonSheetByNationalId(pokemonDto.PokedexId);

            Level = pokemonDto.Level;
            Xp = pokemonDto.TotalXp;
            Age = pokemonDto.Age;
            Gender = pokemonDto.Gender;
            //TODO
            Nature = (NatureType)RandomUtils.RandomInt(Enum.GetValues(typeof(NatureType)).Length);

            NoRepTime = pokemonDto.NoRepTime;

            HiddenPosition = new Position(pokemonDto.Position.X, pokemonDto.Position.Y);


            //TODO
            EffortValues = new BaseStats();

            IndividualValues = new BaseStats();

            IndividualValues.HP = RandomUtils.RandomInt(32);
            IndividualValues.Attack = RandomUtils.RandomInt(32);
            IndividualValues.Defense = RandomUtils.RandomInt(32);
            IndividualValues.SpeAttack = RandomUtils.RandomInt(32);
            IndividualValues.SpeDefense = RandomUtils.RandomInt(32);
            IndividualValues.Speed = RandomUtils.RandomInt(32);
        }

        #endregion

        public bool AddXp(int xpGain)
        {
            bool levelUp = false;
            Xp += xpGain;
            var nextLevelXp = XpUtils.getXpForLevel(Level + 1, XpFormula.Medium_Fast);
            while (Xp > nextLevelXp)
            {
                Level++;
                nextLevelXp = XpUtils.getXpForLevel(Level + 1, XpFormula.Medium_Fast);
                levelUp = true;
            }

            return levelUp;
        }
    }
}
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using Anjril.PokemonWorld.Common.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Characteristics EffortValues { get; set; }
        public Characteristics IndividualValues { get; set; }
        public Characteristics Characteristics
        {
            get
            {
                return new Characteristics
                {
                    LifePoint = (PokedexSheet.BaseCharacteristic.LifePoint * 2 + IndividualValues.LifePoint + (EffortValues.LifePoint / 4)) * Level / 100 + 10 + Level,
                    Attack = (int)(((PokedexSheet.BaseCharacteristic.Attack * 2 + IndividualValues.Attack + (EffortValues.Attack / 4)) * Level / 100 + 5) * Nature.GetAttackModifier()),
                    Defense = (int)(((PokedexSheet.BaseCharacteristic.Defense * 2 + IndividualValues.Defense + (EffortValues.Defense / 4)) * Level / 100 + 5) * Nature.GetDefenseModifier()),
                    SpeAttack = (int)(((PokedexSheet.BaseCharacteristic.SpeAttack * 2 + IndividualValues.SpeAttack + (EffortValues.SpeAttack / 4)) * Level / 100 + 5) * Nature.GetSpeAttackModifier()),
                    SpeDefense = (int)(((PokedexSheet.BaseCharacteristic.SpeDefense * 2 + IndividualValues.SpeDefense + (EffortValues.SpeDefense / 4)) * Level / 100 + 5) * Nature.GetSpeDefenseModifier()),
                    Velocity = (int)(((PokedexSheet.BaseCharacteristic.Velocity * 2 + IndividualValues.Velocity + (EffortValues.Velocity / 4)) * Level / 100 + 5) * Nature.GetVelocityModifier())
                };
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

            EffortValues = new Characteristics();

            IndividualValues = new Characteristics();

            IndividualValues.LifePoint = RandomUtils.RandomInt(32);
            IndividualValues.Attack = RandomUtils.RandomInt(32);
            IndividualValues.Defense = RandomUtils.RandomInt(32);
            IndividualValues.SpeAttack = RandomUtils.RandomInt(32);
            IndividualValues.SpeDefense = RandomUtils.RandomInt(32);
            IndividualValues.Velocity = RandomUtils.RandomInt(32);
        }

        #endregion
    }
}
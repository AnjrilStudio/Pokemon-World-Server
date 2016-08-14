using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
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

        public int PokedexId { get; private set; }
        public int Age { get; set; }
        public Gender Sex { get; private set; }
        public int Level { get; set; }
        public int Xp { get; set; }

        #endregion

        #region constructor

        public Pokemon(int pokemonId, Position hiddenPosition)
            : base()
        {
            PokedexId = pokemonId;
            Type = EntityType.Pokemon;
            Level = 1;
            Xp = 0;
            Age = 1;
            Sex = RandomUtils.CoinToss() ? Gender.Female : Gender.Male;

            NoRepTime = 0;

            HiddenPosition = hiddenPosition;
        }

        #endregion
    }
}

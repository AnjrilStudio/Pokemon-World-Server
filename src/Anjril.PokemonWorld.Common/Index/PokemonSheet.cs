using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Index.Attack;
using Anjril.PokemonWorld.Common.Index.Evolution;
using Anjril.PokemonWorld.Common.Index.Talent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Index
{
    public class PokemonSheet
    {
        #region private fields

        [JsonProperty]
        private int? _preEvolution;
        [JsonProperty]
        private List<int> _evolutions;

        #endregion

        #region public properties

        public int NationalId { get; set; }

        public int CatchRate { get; set; }
        public double MaleDistribution { get; set; }
        [JsonIgnore]
        public double FemaleDistribution { get { return 1 - MaleDistribution; } }
        public EggGroup EggGroup1 { get; set; }
        public EggGroup EggGroup2 { get; set; }

        public string Name { get; set; }
        public PokemonType Type1 { get; set; }
        public PokemonType Type2 { get; set; }
        public double AverageSize { get; set; }
        public double AverageWeight { get; set; }
        public string Description { get; set; }

        public int XpGain { get; set; }

        public bool CanSwim { get; set; }
        public bool CanWalk { get; set; }
        public bool CanFly { get; set; }
        public bool CanBeRidden { get; set; }

        public int BaseAP { get; set; }
        public int BaseMP { get; set; }
        public BaseStats BaseCharacteristic { get; set; }
        public IList<ITalent> AvailableTalents { get; set; }
        public string LevelingMoves { get; set; }
        public string LearnableMoves { get; set; }

        public IEvolutionCondition EvolutionCondition { get; set; }
        [JsonIgnore]
        public PokemonSheet PreEvolution { get { return Pokedex.GetPokemonSheetByNationalId(_preEvolution.Value); } }
        [JsonIgnore]
        public IList<PokemonSheet> Evolutions { get { return _evolutions.Select(id => Pokedex.GetPokemonSheetByNationalId(id)).ToList(); } }

        #endregion

        #region constructor

        internal PokemonSheet()
        {
            _preEvolution = -1;
            _evolutions = new List<int>();
        }

        #endregion
    }
}

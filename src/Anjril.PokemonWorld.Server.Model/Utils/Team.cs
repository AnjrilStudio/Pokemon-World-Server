using Anjril.PokemonWorld.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Utils
{
    public class Team
    {
        #region private fields

        private BattleEntity[] _pokemons;

        #endregion

        #region public properties

        public int NbPokemon { get { return _pokemons.Count(poke => poke != null); } }
        public bool IsFull { get { return NbPokemon == 6; } }
        public IReadOnlyList<BattleEntity> Pokemons { get { return _pokemons.ToList().AsReadOnly(); } }

        #endregion

        #region constructor

        public Team()
        {
            _pokemons = new BattleEntity[6];
        }

        #endregion

        #region public methods

        public bool AddPokemon(BattleEntity pokemon)
        {
            if (IsFull)
            {
                return false;
            }

            int i = 0;
            while (_pokemons[i] != null) { i++; }

            _pokemons[i] = pokemon;

            return true;
        }

        public bool RemovePokemon(int idx)
        {
            if (idx >= _pokemons.Length || _pokemons[idx] == null)
            {
                return false;
            }

            int currentSize = NbPokemon;

            for (int i = idx + 1; i < currentSize; i++)
            {
                _pokemons[i - 1] = _pokemons[i];
            }

            _pokemons[currentSize - 1] = null;

            return true;
        }

        public bool SwitchPokemon(int idxA, int idxB)
        {
            if (idxA >= _pokemons.Length || idxB >= _pokemons.Length || _pokemons[idxA] == null || _pokemons[idxB] == null)
            {
                return false;
            }

            var tmp = _pokemons[idxA];

            _pokemons[idxA] = _pokemons[idxB];
            _pokemons[idxB] = tmp;

            return true;
        }

        #endregion
    }
}

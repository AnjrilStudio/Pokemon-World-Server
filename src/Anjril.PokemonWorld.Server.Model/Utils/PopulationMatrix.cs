using Anjril.PokemonWorld.Server.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Server.Model.Utils
{
    public class PopulationMatrix : IEnumerable<Pokemon>
    {
        #region private fields

        private readonly object _stackCreation = new object();

        private Dictionary<int, Pokemon> _population;
        private Dictionary<int, Pokemon>[,] _matrix;

        private PositionMatrix _positionMatrix;

        #endregion

        #region public properties

        public int Size { get; private set; }

        #endregion

        #region indexors

        public Pokemon this[int id] { get { return _population[id]; } }

        public IReadOnlyCollection<Pokemon> this[Position pos] { get { return _matrix[pos.X, pos.Y].Values; } }

        public IReadOnlyCollection<Pokemon> this[int x, int y] { get { return _matrix[x, y].Values; } }

        #endregion

        #region constructor

        public PopulationMatrix(int size, PositionMatrix positionMatrix)
        {
            this.Size = size;

            this._matrix = new Dictionary<int, Pokemon>[size, size];
            this._population = new Dictionary<int, Pokemon>();

            this._positionMatrix = positionMatrix;
        }

        #endregion

        #region public methods

        public bool Add(Pokemon pokemon)
        {
            var id = pokemon.Id;

            if (!_population.ContainsKey(id))
            {
                var pos = pokemon.HiddenPosition;

                lock (_stackCreation)
                {
                    if (_matrix[pos.X, pos.Y] == null)
                    {
                        _matrix[pos.X, pos.Y] = new Dictionary<int, Pokemon>();
                    }
                }

                _matrix[pos.X, pos.Y].Add(id, pokemon);
                _population.Add(id, pokemon);

                if (pokemon.IsVisible && !_positionMatrix.Add(pokemon, pokemon.Position))
                {
                    pokemon.Position = null;
                }

                return true;
            }

            return false;
        }

        public bool Remove(int id)
        {
            if (_population.ContainsKey(id))
            {
                var pokemon = _population[id];

                var pos = pokemon.HiddenPosition;

                _matrix[pos.X, pos.Y].Remove(id);
                _population.Remove(id);
                _positionMatrix.Remove(id);

                return true;
            }

            return false;
        }

        public bool Move(Pokemon pokemon, Position dest)
        {
            return Move(pokemon.Id, dest);
        }

        public bool Move(int id, Position dest)
        {
            if (_population.ContainsKey(id) && dest.IsInMap(Size))
            {
                var pokemon = _population[id];

                _matrix[pokemon.HiddenPosition.X, pokemon.HiddenPosition.Y].Remove(id);

                lock (_stackCreation)
                {
                    if (_matrix[dest.X, dest.Y] == null)
                    {
                        _matrix[dest.X, dest.Y] = new Dictionary<int, Pokemon>();
                    }
                }

                _matrix[dest.X, dest.Y].Add(id, pokemon);
                pokemon.HiddenPosition = dest;

                return true;
            }

            return false;
        }

        #endregion

        #region iterator methods

        public IEnumerator<Pokemon> GetEnumerator()
        {
            return _population.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _population.Values.GetEnumerator();
        }

        #endregion
    }
}

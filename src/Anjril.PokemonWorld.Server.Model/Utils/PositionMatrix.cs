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
    public class PositionMatrix : IEnumerable<WorldEntity>
    {
        #region private fields

        private readonly object _moveLock = new object();

        private Dictionary<int, WorldEntity> _entities;
        private WorldEntity[,] _matrix;

        #endregion

        #region public properties

        public int Size { get; private set; }

        #endregion

        #region indexors

        public WorldEntity this[int id] { get { return _entities[id]; } }

        public WorldEntity this[Position pos] { get { return _matrix[pos.X, pos.Y]; } }

        public WorldEntity this[int x, int y] { get { return _matrix[x, y]; } }

        #endregion

        #region constructor

        public PositionMatrix(int size)
        {
            this.Size = size;

            this._matrix = new WorldEntity[size, size];
            this._entities = new Dictionary<int, WorldEntity>();
        }

        #endregion

        #region public methods

        public bool Add(WorldEntity entity, Position dest)
        {
            if (!_entities.ContainsKey(entity.Id))
            {
                lock (_moveLock)
                {
                    if (this[dest] == null)
                    {
                        _matrix[dest.X, dest.Y] = entity;
                        _entities.Add(entity.Id, entity);
                        entity.Position = dest;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool Remove(int id)
        {
            if (_entities.ContainsKey(id))
            {
                var entity = _entities[id];

                var pos = entity.Position;

                _matrix[pos.X, pos.Y] = null;
                _entities.Remove(id);
                entity.Position = null;

                return true;
            }

            return false;
        }

        public bool Move(WorldEntity entity, Position dest)
        {
            return Move(entity.Id, dest);
        }

        public bool Move(int id, Position dest)
        {
            if (_entities.ContainsKey(id) && dest.IsInMap(Size, Size))
            {
                var entity = _entities[id];

                lock (_moveLock)
                {
                    if (this[dest] == null)
                    {
                        var currentPos = entity.Position;

                        _matrix[dest.X, dest.Y] = entity;
                        _matrix[currentPos.X, currentPos.Y] = null;

                        entity.Position = dest;

                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region iterator methods

        public IEnumerator<WorldEntity> GetEnumerator()
        {
            return _entities.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.Values.GetEnumerator();
        }

        #endregion
    }
}
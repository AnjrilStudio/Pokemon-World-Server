using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public abstract class WorldEntity
    {
        private static int sequenceId = 0;

        #region public properties

        public int Id { get; private set; }
        public Direction Direction { get; set; }
        public Position Position { get; set; }
        public EntityType Type { get; protected set; }
        public float MoveDuration { get; protected set; }

        #endregion

        #region constructor

        protected WorldEntity()
        {
            Id = sequenceId++;
            Direction = Direction.Down;
            MoveDuration = 0.6f;
        }

        #endregion

        #region serialization

        public override string ToString()
        {
            return Type.ToString() + "-" + Id + "=" + Position.ToString() + ":" + Utils.DirectionToString(Direction);
        }

        #endregion
    }
}

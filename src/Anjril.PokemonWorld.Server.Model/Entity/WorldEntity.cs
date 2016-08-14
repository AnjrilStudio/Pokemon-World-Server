using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
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

        #region overriden methods

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is WorldEntity))
                return false;

            WorldEntity other = (WorldEntity)obj;

            return other.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}

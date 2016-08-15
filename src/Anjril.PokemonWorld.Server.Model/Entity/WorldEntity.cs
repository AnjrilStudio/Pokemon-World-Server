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
        public EntityType Type { get; protected set; }

        public Position Position { get; set; }
        public Direction Direction { get; set; }

        public bool CanSwim { get; private set; }
        public bool CanWalk { get; private set; }
        public bool CanFly { get; private set; }

        public EntityState State { get; set; }

        public float MoveDuration { get; protected set; }

        #endregion

        #region constructor

        protected WorldEntity(EntityType type, bool canSwim, bool canWalk, bool canFly)
        {
            Id = sequenceId++;
            Type = type;

            CanSwim = canSwim;
            CanWalk = canWalk;
            CanFly = canFly;

            Direction = Direction.Down;
            MoveDuration = 0.6f;

            State = EntityState.Undefined;
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

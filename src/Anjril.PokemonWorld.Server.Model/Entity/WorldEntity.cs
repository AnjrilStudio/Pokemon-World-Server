using Anjril.PokemonWorld.Server.Model.State;
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

        public int Id { get; private set; }
        public Direction Direction { get; set; }
        public Position Position { get; set; }
        public EntityType Type { get; protected set; }
        public float MoveTime { get; protected set; }


        protected WorldEntity()
        {
            Id = sequenceId++;
            Direction = Direction.DOWN;
            MoveTime = 0.6f;
        }

        public override string ToString()
        {
            return Type.ToString() + "-" + Id + "=" + Position.ToString() + ":" + Utils.DirectionToString(Direction);
        }

        
    }
}

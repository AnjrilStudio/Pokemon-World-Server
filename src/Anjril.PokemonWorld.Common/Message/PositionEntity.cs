using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public class PositionEntity
    {
        #region public properties

        public int Id { get; private set; }
        public Position Position { get; private set; }
        public Direction Orientation { get; private set; }
        public EntityType Type { get; private set; }

        #endregion

        #region constructor

        public PositionEntity(int id, Position position, EntityType type, Direction orientation)
        {
            Id = id;
            Position = position;
            Type = type;
            Orientation = orientation;
        }

        public PositionEntity(string args)
        {
            var argsSplit = args.Split('=');
            var entityInfos = argsSplit[0].Split('-');
            var entityValues = argsSplit[1].Split(':');

            var id = entityInfos[1];
            var type = entityInfos[0];
            var position = entityValues[0];
            var dir = entityValues[1];

            Id = Int32.Parse(id);
            Position = new Position(position);
            Type = EntityTypeUtils.FromString(type);
            Orientation = DirectionUtils.FromString(dir);
        }

        #endregion

        #region serialization

        public override string ToString()
        {
            return String.Format("{0}-{1}={2}:{3}", EntityTypeUtils.ToString(Type), Id, Position, DirectionUtils.ToString(Orientation));
        }

        #endregion
    }
}

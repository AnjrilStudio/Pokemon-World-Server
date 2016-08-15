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
        public int PokedexId { get; private set; }
        public EntityState State { get; private set; }

        #endregion

        #region constructor

        public PositionEntity(int id, Position position, EntityType type, Direction orientation, int pokedexId, EntityState state)
        {
            Id = id;
            Position = position;
            Type = type;
            Orientation = orientation;
            PokedexId = pokedexId;
            State = state;
        }

        public PositionEntity(string args)
        {
            var argsSplit = args.Split('=');
            var entityInfos = argsSplit[0].Split('-');
            var entityValues = argsSplit[1].Split(':');

            var id = entityInfos[1];
            var type = entityInfos[0];
            var pokedexId = entityInfos[2];
            var position = String.Format("{0}:{1}", entityValues[0], entityValues[1]);
            var dir = entityValues[2];
            var state = entityValues[3];

            Id = Int32.Parse(id);
            Position = new Position(position);
            Type = EntityTypeUtils.FromString(type);
            Orientation = DirectionUtils.FromString(dir);
            PokedexId = Int32.Parse(pokedexId);
            State = StateUtils.FromString(state);
        }

        #endregion

        #region serialization

        public override string ToString()
        {
            return String.Format("{0}-{1}-{4}={2}:{3}:{5}", EntityTypeUtils.ToString(Type), Id, Position, DirectionUtils.ToString(Orientation), PokedexId, StateUtils.ToString(State));
        }

        #endregion
    }
}

using Anjril.PokemonWorld.Common.Message;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Core.Properties;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Module
{
    public class NotificationModule : BaseModule
    {
        public NotificationModule()
            : base(200)
        { }

        public override void Update(TimeSpan elapsed)
        {
            foreach (var player in World.Instance.Players)
            {
                BaseMessage message = new PositionMessage(GetVisibleEntities(player));
                SendMessage(player, message);

                if (player.MapToUpdate)
                {
                    message = GetMapUpdate(player);
                    SendMessage(player, message);

                    player.MapToUpdate = false;
                }

                if (player.TeamToUpdate)
                {
                    player.RemoteConnection.Send(GetTeamUpdate(player));
                    player.TeamToUpdate = false;
                }
            }

            base.Update(elapsed);
        }

        #region private methods

        private List<PositionEntity> GetVisibleEntities(Player player)
        {
            var lineOfSight = Settings.Default.LineOfSight;

            var visibleEntites = new List<PositionEntity>();

            for (int x = Math.Max(player.Position.X - lineOfSight, 0); x < Math.Min(player.Position.X + lineOfSight, World.Instance.Map.Size); x++)
            {
                for (int y = Math.Max(player.Position.Y - lineOfSight, 0); y < Math.Min(player.Position.Y + lineOfSight, World.Instance.Map.Size); y++)
                {
                    var entity = World.Instance.VisibleEntities[x, y];

                    if (entity != null)
                    {
                        if (entity.Type == EntityType.Player)
                        {
                            visibleEntites.Add(new PositionEntity(entity.Id, entity.Position, entity.Type, entity.Direction, 0, entity.State));
                        }
                        else if (entity.Type == EntityType.Pokemon)
                        {
                            var pokedexId = (entity as Pokemon).PokedexId;
                            visibleEntites.Add(new PositionEntity(entity.Id, entity.Position, entity.Type, entity.Direction, pokedexId, entity.State));
                        }
                    }
                }
            }

            return visibleEntites;
        }

        private MapMessage GetMapUpdate(Player player)
        {
            Position segment = player.Position.GetSegment(Settings.Default.ChunkSize);

            var startx = (segment.X - 1) * 20;
            var starty = (segment.Y - 1) * 20;
            if (startx < 0) startx = 0;
            if (starty < 0) starty = 0;

            Position startPos = new Position(startx, starty);
            string message = String.Empty;

            var endx = (segment.X + 2) * 20;
            var endy = (segment.Y + 2) * 20;

            for (int y = starty; y < endy - 1; y++)
            {
                for (int x = startx; x < endx - 1; x++)
                {
                    Position pos = new Position(x, y);
                    message += (int)World.Instance.Map.GetTile(pos);
                    message += ".";
                    message += (int)World.Instance.Map.GetObject(pos);
                    message += ",";
                }
            }

            message = message.Remove(message.Length - 1, 1);

            return new MapMessage(startPos, message);
        }

        private string GetTeamUpdate(Player player)
        {
            string message = "team:";

            foreach (BattleEntity pokemon in player.Team)
            {
                message += pokemon.PokedexId;
                message += ".";
                message += pokemon.Level;
                message += ",";
            }

            message = message.Remove(message.Length - 1, 1);

            return message;
        }

        #endregion
    }
}

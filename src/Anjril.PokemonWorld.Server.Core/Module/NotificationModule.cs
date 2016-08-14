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
                var visibleEntites = GetVisibleEntities(player);

                string message = String.Format("entities:{0}", String.Join(";", visibleEntites));

                Console.WriteLine("send(" + player.Id + ") :" + message);

                player.RemoteConnection.Send(message);

                if (player.MapToUpdate)
                {
                    message = GetMapUpdate(player);

                    Console.WriteLine("send(" + player.Id + ") :" + message);

                    player.RemoteConnection.Send(message);
                    player.MapToUpdate = false;
                }
            }

            base.Update(elapsed);
        }

        #region private methods

        private List<WorldEntity> GetVisibleEntities(Player player)
        {
            var lineOfSight = Settings.Default.LineOfSight;

            var visibleEntites = new List<WorldEntity>();

            for (int x = Math.Max(player.Position.X - lineOfSight, 0); x < Math.Min(player.Position.X + lineOfSight, World.Instance.Size); x++)
            {
                for (int y = Math.Max(player.Position.Y - lineOfSight, 0); y < Math.Min(player.Position.Y + lineOfSight, World.Instance.Size); y++)
                {
                    var entity = World.Instance.GetEntity(x, y);
                    if (entity != null)
                    {
                        visibleEntites.Add(entity);
                    }
                }
            }

            return visibleEntites;
        }

        private string GetMapUpdate(Player player)
        {
            string message = "map:";
            int mapsize = World.Instance.Size;

            Position segment = player.Position.GetSegment(Settings.Default.ChunkSize);

            var startx = (segment.X - 1) * 20;
            var starty = (segment.Y - 1) * 20;
            if (startx < 0) startx = 0;
            if (starty < 0) starty = 0;

            Position startPos = new Position(startx, starty);
            message += startPos.ToString() + "+";

            var endx = (segment.X + 2) * 20;
            var endy = (segment.Y + 2) * 20;

            Position endPos = new Position(endx, endy);

            for (int y = starty; y < endy - 1; y++)
            {
                for (int x = startx; x < endx - 1; x++)
                {
                    Position pos = new Position(x, y);
                    message += (int)World.Instance.GetTile(pos);
                    message += ".";
                    message += (int)World.Instance.GetObject(pos);
                    message += ",";
                }
            }

            message = message.Remove(message.Length - 1, 1);

            return message;
        }

        #endregion
    }
}

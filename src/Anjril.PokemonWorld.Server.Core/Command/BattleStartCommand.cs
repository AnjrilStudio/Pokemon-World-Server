using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.Entity;
using System.ComponentModel;
using Anjril.PokemonWorld.Common.Parameter;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    [Description("btl")]
    class BattleStartCommand : BaseCommand<BattleStartParam>
    {
        public override void RunWithCast(Player player, BattleStartParam param)
        {
            var dirPos = PositionUtils.GetDirPosition(player.Direction);
            var otherPos = new Position(player.Position.X + dirPos.X, player.Position.Y - dirPos.Y);
            if (World.Instance.VisibleEntities[otherPos] != null)
            {
                var entity = World.Instance.VisibleEntities[otherPos];

                List<int> entitiesList = new List<int>();
                entitiesList.Add(player.Id);
                entitiesList.Add(entity.Id);
                var battle = GlobalServer.Instance.NewBattle(entitiesList);

                string startmessage = "battlestart:";

                foreach (int entityId in entitiesList)
                {
                    startmessage += entityId;
                    startmessage += ";";
                }

                foreach (int id in entitiesList)
                {
                    string battlemessage = battle.ToNoActionMessage(id);
                    GlobalServer.Instance.SendMessage(id, startmessage);
                    GlobalServer.Instance.SendMessage(id, battlemessage);
                }
            }
        }
    }
}

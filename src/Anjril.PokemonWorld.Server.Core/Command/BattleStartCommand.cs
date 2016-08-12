using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public class BattleStartCommand : ICommand
    {
        private int entityId;
        public bool CanRun { get { return true; } }

        public BattleStartCommand(string arg)
        {
            entityId = Int32.Parse(arg);
        }

        public void Run()
        {
            var entity = World.Instance.GetEntity(entityId);

            var dirPos = Utils.GetDirPosition(entity.Direction);
            var otherPos = new Position(entity.Position.X + dirPos.X, entity.Position.Y - dirPos.Y);
            if (World.Instance.GetEntity(otherPos) != null)
            {
                List<int> entitiesList = new List<int>();
                entitiesList.Add(entity.Id);
                entitiesList.Add(World.Instance.GetEntity(otherPos).Id);
                var battle = GlobalServer.Instance.NewBattle(entitiesList);

                
                string startmessage = World.Instance.BattleStartToMessage(entitiesList);
                string battlemessage = battle.ToNoActionMessage();
                foreach (int id in entitiesList)
                {
                    GlobalServer.Instance.SendMessage(id, startmessage);
                    GlobalServer.Instance.SendMessage(id, battlemessage);
                }
            }
        }
    }
}

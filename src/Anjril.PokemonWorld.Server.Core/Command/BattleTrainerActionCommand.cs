using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Server.Core.Battle;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public class BattleTrainerActionCommand : ICommand
    {
        private int playerId;
        private int turn;
        private Position target;
        private Common.Action action;
        public bool CanRun { get { return true; } }

        public BattleTrainerActionCommand(string arg)
        {
            var playerStr = arg.Split(',')[0];
            var turnStr = arg.Split(',')[1];
            var targetStr = arg.Split(',')[2];
            var actionStr = arg.Split(',')[3];

            playerId = Int32.Parse(playerStr);
            turn = Int32.Parse(turnStr);
            target = new Position(targetStr);
            action = TrainerActions.Get((TrainerAction)Int32.Parse(actionStr));
        }

        public void Run()
        {
            BattleState battle = GlobalServer.Instance.GetBattle(playerId);
            if (battle.CurrentPlayer() == playerId)
            {
                battle.PlayTrainerAction(target, action);
            }
            else
            {
                GlobalServer.Instance.SendMessage(playerId, battle.ToNoActionMessage());
            }
        }
    }
}

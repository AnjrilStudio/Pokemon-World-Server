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
    public class BattleActionCommand : ICommand
    {
        private int playerId;
        private int turn;
        private Position target;
        private Common.Action action;
        private Direction direction;
        public bool CanRun { get { return true; } }

        public BattleActionCommand(string arg)
        {
            var playerStr = arg.Split(',')[0];
            var turnStr = arg.Split(',')[1];
            var targetStr = arg.Split(',')[2];
            var actionStr = arg.Split(',')[3];
            var dirStr = arg.Split(',')[4];

            playerId = Int32.Parse(playerStr);
            turn = Int32.Parse(turnStr);
            direction = Utils.DirectionFromString(dirStr);
            target = new Position(targetStr);
            action = Moves.Get((Move)Int32.Parse(actionStr));
        }

        public void Run()
        {
            BattleState battle = GlobalServer.Instance.GetBattle(playerId);
            bool result = false;
            if (battle.CurrentPlayer() == playerId)
            {
                result = battle.PlayAction(target, action, direction);
            }
            
            if (!result)
            {
                GlobalServer.Instance.SendMessage(playerId, battle.ToNoActionMessage());
            }
        }
    }
}

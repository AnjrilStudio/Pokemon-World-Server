using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Server.Core.Battle;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Common.Parameter;
using System.ComponentModel;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    [Description("act")]
    class BattleActionCommand : BaseCommand<BattleActionParam>
    {
        public override void RunWithCast(Player player, BattleActionParam param)
        {
            BattleState battle = GlobalServer.Instance.GetBattle(player.Id);
            
            if (battle.CurrentPlayer() == player.Id)
            {
                battle.PlayAction(param.Target, param.Action, param.Direction);
            }
        }
    }
}

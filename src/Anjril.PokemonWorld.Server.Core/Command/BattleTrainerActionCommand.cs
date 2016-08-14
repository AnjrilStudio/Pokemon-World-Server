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
using Anjril.PokemonWorld.Server.Core.Command;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    [Description("tra")]
    class BattleTrainerActionCommand : BaseCommand<BattleTrainerActionParam>
    {
        public override void RunWithCast(Player player, BattleTrainerActionParam param)
        {
            BattleState battle = GlobalServer.Instance.GetBattle(player.Id);
            if (battle.WaitingPokemonGo || battle.CurrentPlayer() == player.Id)
            {
                battle.PlayTrainerAction(player, param.Target, param.Action, param.Index);
            }
        }
    }
}

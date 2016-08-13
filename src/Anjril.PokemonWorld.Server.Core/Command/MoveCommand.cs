using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Common.Parameter;
using System.ComponentModel;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    [Description("mov")]
    class MoveCommand : BaseCommand<MoveParam>
    {
        public override void RunWithCast(Player player, MoveParam param)
        {
            player.DoMove(param.Direction);
        }
    }
}

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
using Anjril.PokemonWorld.Server.Core.Properties;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    [Description("mov")]
    class MoveCommand : BaseCommand<MoveParam>
    {
        public override bool CanRun(Player player, string args, out object param)
        {
            if (base.CanRun(player, args, out param))
            {
                var nextMoveInputTime = player.LastMove.AddSeconds(player.MoveDuration - player.MoveInputDelay);

                return DateTime.Now >= nextMoveInputTime;
            }

            return false;
        }

        public override void RunWithCast(Player player, MoveParam param)
        {
            var dest = new Position(player.Position, (param as MoveParam).Direction);

            var result = World.Instance.MoveEntity(player.Id, dest);

            if (result)
            {
                var nextMoveTime = player.LastMove.AddSeconds(player.MoveDuration);

                if (DateTime.Now > nextMoveTime)
                {
                    player.LastMove = DateTime.Now;
                }
                else
                {
                    player.LastMove = nextMoveTime;
                }

                var oldSegment = player.Position.GetSegment(Settings.Default.ChunkSize);
                var newSegment = dest.GetSegment(Settings.Default.ChunkSize);

                if (!oldSegment.Equals(newSegment))
                {
                    player.MapToUpdate = true;
                }

            }
        }
    }
}

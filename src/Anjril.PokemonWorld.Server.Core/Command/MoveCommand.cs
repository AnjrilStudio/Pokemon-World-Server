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
        public override void RunWithCast(Player player, MoveParam param)
        {
            lock (player)
            {
                var nextMoveInputTime = player.LastMove.AddSeconds(player.MoveDuration - player.MoveInputDelay);

                if (DateTime.Now >= nextMoveInputTime)
                {
                    var oldSegment = player.Position.GetSegment(Settings.Default.ChunkSize);

                    var dest = new Position(player.Position, (param as MoveParam).Direction);
                    var newSegment = dest.GetSegment(Settings.Default.ChunkSize);

                    var moveResult = false;
                    EntityState newState;
                    if (World.Instance.Map.CanGo(player, dest, out newState))
                    {
                        moveResult = World.Instance.VisibleEntities.Move(player.Id, dest);
                        player.State = newState;
                    }

                    if (moveResult)
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

                        if (!oldSegment.Equals(newSegment))
                        {
                            player.MapToUpdate = true;
                        }
                    }
                }
            }
        }
    }
}

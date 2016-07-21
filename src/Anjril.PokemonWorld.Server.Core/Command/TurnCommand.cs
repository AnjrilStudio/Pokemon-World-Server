using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Server.Model.State;
using Anjril.PokemonWorld.Server.Model;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public class TurnCommand : ICommand
    {
        private Direction dir;
        private int entityId;
        public bool CanRun { get { return true; } }

        public TurnCommand(string arg)
        {
            var arg1 = arg.Split(',')[0];
            var arg2 = arg.Split(',')[1];

            entityId = Int32.Parse(arg1);

            switch (arg2)
            {
                case "Left":
                    dir = Direction.LEFT;
                    break;
                case "Down":
                    dir = Direction.DOWN;
                    break;
                case "Right":
                    dir = Direction.RIGHT;
                    break;
                case "Up":
                    dir = Direction.UP;
                    break;
                default:
                    dir = Direction.NONE;
                    break;

            }
        }

        public void Run()
        {
            var entity = World.Instance.GetEntity(entityId);
            entity.Direction = dir;
        }
    }
}

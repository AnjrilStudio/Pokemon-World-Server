using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public class MoveCommand : ICommand
    {
        public bool CanRun { get { return true; } }

        public void Run()
        {
            // TODO : move the player
        }
    }
}

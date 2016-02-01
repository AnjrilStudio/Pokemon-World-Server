using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public static class CommandFactory
    {
        public static ICommand GetCommand(string player, string argument)
        {
            var splitedArgument = argument.Split('|');

            var cmd = splitedArgument[0];
            var arg = splitedArgument[1];

            switch(cmd)
            {
                // TODO : map to all existing commands
                default: return null;
            }
        }
    }
}

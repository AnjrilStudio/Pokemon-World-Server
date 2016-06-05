using Anjril.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public class ConnectionCommand : ICommand
    {
        private string jsonMap;
        private IRemoteConnection sender;

        public bool CanRun { get { return true; } }

        public ConnectionCommand(string jsonMap, IRemoteConnection sender)
        {
            this.jsonMap = jsonMap;
            this.sender = sender;
        }

        public void Run()
        {
            sender.Send(jsonMap);
        }
    }
}

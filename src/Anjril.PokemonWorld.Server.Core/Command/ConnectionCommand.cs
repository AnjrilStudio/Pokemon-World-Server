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
        private Socket socket;
        private RemoteConnection sender;

        public bool CanRun { get { return true; } }

        public ConnectionCommand(string jsonMap, Socket socket, RemoteConnection sender)
        {
            this.jsonMap = jsonMap;
            this.socket = socket;
            this.sender = sender;
        }

        public void Run()
        {
            var mapLength = this.jsonMap.Length;
            int buffer = 10000;
            var nbMessage = mapLength / buffer + 1;
            int i = 0;

            while (this.jsonMap.Length > 0)
            {
                var subMap = this.jsonMap.Take(buffer);
                this.jsonMap = String.Join("", this.jsonMap.Skip(buffer));

                var response = String.Format("{0}:{1}|{2}", i, nbMessage, String.Join("", subMap));

                socket.Send(response, this.sender);

                Thread.Sleep(100);

                i++;
            }
        }
    }
}

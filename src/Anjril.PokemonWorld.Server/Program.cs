using Anjril.Common.Network;
using Anjril.Common.Network.UdpImpl;
using Anjril.PokemonWorld.Generator;
using Anjril.PokemonWorld.Server.Core.Command;
using Anjril.PokemonWorld.Server.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server
{
    class Program
    {
        private static string jsonMap;
        private static Common.Network.Socket socket;

        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("------ Pokemon World Server ------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");

            #region get the map

            var mapLocation = Settings.Default.MapLocation;

            var fi = new FileInfo(mapLocation);

            if(!fi.Exists)
            {
                Console.Write("Generating map");

                var generator = new WorldGen(400, 400, "World", false, false);

                var generationResult = Task.Run(() =>
                {
                    var chrono = Stopwatch.StartNew();

                    generator.GenerateMap();

                    chrono.Stop();

                    return chrono.Elapsed;
                });

                int nbLoop = 14;
                while (!generationResult.IsCompleted)
                {
                    Console.Write(".");
                    nbLoop++;

                    if (nbLoop == 23)
                    {
                        nbLoop = 0;
                        Console.WriteLine();
                    }

                    Thread.Sleep(5000);
                }

                Console.WriteLine("Map generated in {0:#.##}s!", generationResult.Result.TotalSeconds);
            }
            else
            {
                Console.WriteLine("The server has loaded the old map (delete/rename the file under Map\\World.json to generate a new one).");
            }

            jsonMap = File.ReadAllText(mapLocation);

            #endregion

            #region set network on

            var port = Settings.Default.ListeningPort;

            var udpClient = new UdpClient(port);
            var receiver = new UdpReceiver(udpClient);
            var sender = new UdpSender(udpClient);

            socket = new Common.Network.Socket(receiver, sender, MessageReceived);

            socket.StartListening();

            Console.WriteLine();
            Console.WriteLine("Server listening on port " + port);
            Console.WriteLine();

            #endregion

            Console.WriteLine("Press any key to stop the server...");

            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();

            #region set network off

            udpClient.Close();

            #endregion

            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("------ Pokemon World Server ------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");

            Console.WriteLine();
            Console.WriteLine("Server stopped. Press any key to exit...");
            Console.WriteLine();

            Console.ReadKey();
        }

        private static void MessageReceived(RemoteConnection sender, string message)
        {
            Console.WriteLine("Message received from {0}:{1} : {2}.", sender.IPAddress, sender.Port, message);

            ICommand command;
            
            if(message.ToUpper().StartsWith("LOGIN"))
            {
                command = new ConnectionCommand(jsonMap, socket, sender);
            }
            else
            {
                command = CommandFactory.GetCommand(sender.IPAddress, message);
            }

            // TODO : trace command

            try
            {
                if (command.CanRun)
                    Task.Run(() => command.Run());
            }
            finally { }
        }
    }
}

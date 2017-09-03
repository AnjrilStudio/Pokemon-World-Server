using Anjril.Common.Network;
using Anjril.Common.Network.TcpImpl;
using Anjril.PokemonWorld.Generator;
using Anjril.PokemonWorld.Server.Core.Command;
using Anjril.PokemonWorld.Server.Properties;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using Anjril.PokemonWorld.Server.Core.Module;
using Anjril.PokemonWorld.Server.Model.Persistence;
using Anjril.PokemonWorld.Server.Model.Persistence.Dto;

namespace Anjril.PokemonWorld.Server
{
    class Program
    {
        private static bool STOP = false;
        private static Dictionary<IRemoteConnection, Player> PLAYERS = new Dictionary<IRemoteConnection, Player>();

        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("------ Pokemon World Server ------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");

            World.Instance.Init(GetMap());
            World.Instance.LoadPopulation();

            using (var socket = new TcpSocket())
            {
                #region start module loop

                var updateLoop = new Thread(new ThreadStart(UpdateModules));
                updateLoop.Name = "UpdateLoop";
                updateLoop.Start();

                #endregion

                #region set network on

                socket.StartListening(ConnectionRequested, MessageReceived, Disconnected);

                Console.WriteLine();
                Console.WriteLine("Server listening on port " + socket.Port);
                Console.WriteLine();

                #endregion

                Console.WriteLine("Press any key to stop the server...");

                Console.WriteLine();
                var key = Console.ReadKey();
                Console.WriteLine();

                #region stop module loop

                STOP = true;

                while (updateLoop.IsAlive)
                {
                    Thread.Sleep(10);
                }

                #endregion

                #region set network off

                socket.StopListening();

                #endregion
            }

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

        #region socket management

        private static bool ConnectionRequested(IRemoteConnection sender, string request, out string response)
        {
            Player player = null;

            PlayerDto playerDto = PlayerDaoImpl.Instance.LoadPlayer(request);
            if (playerDto == null)
            {
                //le jouer n'existe pas
                player = new Player(request, sender);
                PlayerDaoImpl.Instance.SavePlayer(new PlayerDto(player));
            }
            else
            {
                player = new Player(playerDto, sender);
            }


            
            //player.Position = new Position(200, 150);
            //player.Position = new Position(200, 175);
            player.Position = new Position(35, 35);

            PLAYERS.Add(sender, player);
            World.Instance.AddPlayer(player);
            GlobalServer.Instance.AddPlayer(player.Id, sender);

            response = "OK:" + player.Id;

            return true;
        }

        private static void MessageReceived(IRemoteConnection sender, string message)
        {
            Console.WriteLine("Message received from {0}:{1} : {2}.", sender.IPAddress, sender.Port, message);

            var player = PLAYERS[sender];

            var splitedArgument = message.Split('/');

            var cmd = splitedArgument[0];
            var args = splitedArgument[1];

            ICommand command = CommandFactory.GetCommand(cmd);

            Object param;
            if (command.CanRun(player, args, out param))
            {
                Task.Run(() => command.Run(player, param));
            }
            else
            {
                Console.WriteLine("Can't run the command {0} with the arguments {1}", cmd, args);
            }
        }

        private static void Disconnected(IRemoteConnection remote, string justification)
        {
            var player = PLAYERS[remote];

            World.Instance.RemovePlayer(player.Id);
            GlobalServer.Instance.RemoveBattle(player.Id);
            GlobalServer.Instance.RemovePlayer(player.Id);
            PLAYERS.Remove(remote);

            Console.WriteLine("disconnected ({0} - {1})", player.Id, player.Name);
        }

        #endregion

        #region private methods

        private static void UpdateModules()
        {
            var modules = new List<IModule>();

            modules.Add(new NotificationModule());
            modules.Add(new WildModule());
            modules.Add(new PopulationModule());

            while (!STOP)
            {
                var now = DateTime.Now;

                foreach (IModule mod in modules)
                {
                    var elapsed = now - mod.LastUpdate;

                    if (elapsed.TotalMilliseconds >= mod.Interval)
                    {
                        mod.Update(elapsed);
                    }
                }

                Thread.Sleep(Settings.Default.UpdateTick);
            }
        }

        private static string GetMap()
        {
            var mapLocation = Settings.Default.MapLocation;

            var fi = new FileInfo(mapLocation);

            if (!fi.Exists)
            {
                var di = fi.Directory;

                if (!di.Exists)
                {
                    di.Create();
                }

                Console.Write("Generating map");

                var generator = new WorldGen(400, 400, "World", false, false);

                var generationResult = Task.Run(() =>
                {
                    var chrono = Stopwatch.StartNew();

                    generator.Generate();

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

            return File.ReadAllText(mapLocation);
        }

        #endregion
    }
}

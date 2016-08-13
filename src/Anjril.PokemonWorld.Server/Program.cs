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

namespace Anjril.PokemonWorld.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("------ Pokemon World Server ------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");

            World.Instance.LoadMap(GetMap());

            using (var socket = new TcpSocket())
            {
                #region set network on

                socket.StartListening(ConnectionRequested, MessageReceived, Disconnected);

                Console.WriteLine();
                Console.WriteLine("Server listening on port " + socket.Port);
                Console.WriteLine();

                #endregion

                InitWorld();

                Task.Run(() => Tick());

                Console.WriteLine("Press any key to stop the server...");

                Console.WriteLine();
                var key = Console.ReadKey();
                Console.WriteLine();

                while (key.Key == ConsoleKey.Spacebar)
                {
                    socket.Broadcast("test");
                    key = Console.ReadKey();
                }

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

            return File.ReadAllText(mapLocation);
        }

        #region socket management

        private static bool ConnectionRequested(IRemoteConnection sender, string request, out string response)
        {
            Player player = new Player(request);
            //player.Position = new Position(200, 150);
            player.Position = new Position(30, 175);
            //player.Position = new Position(35, 35);
            World.Instance.AddEntity(player);

            GlobalServer.Instance.AddPlayer(player.Id, sender);

            response = "OK:" + player.Id;

            return true;
        }

        private static void MessageReceived(IRemoteConnection sender, string message)
        {
            Console.WriteLine("Message received from {0}:{1} : {2}.", sender.IPAddress, sender.Port, message);

            ICommand command = CommandFactory.GetCommand(sender.IPAddress, message);

            try
            {
                if (command.CanRun)
                    Task.Run(() => command.Run());
            }
            finally { }
        }

        private static void Disconnected(IRemoteConnection remote, string justification)
        {
            var id = Int32.Parse(justification);
            World.Instance.RemoveEntity(id);
            GlobalServer.Instance.RemoveBattle(id);
            GlobalServer.Instance.RemovePlayer(id);

            Console.WriteLine("disconnected (" + justification + ")");
        }

        #endregion

        private static void Tick()
        {
            var random = new Random();

            while (true)
            {
                List<WorldEntity> entities = World.Instance.Entities;
                if (entities.Count > 0)
                {
                    //test
                    foreach (WorldEntity entity in entities)
                    {
                        if (entity.Type == EntityType.Pokemon && random.NextDouble() < 0.1)
                        {
                            var dest = new Position(entity.Position, Utils.RandomDirection());

                            //World.Instance.MoveEntity(entity.Id, dest);
                        }
                    }

                    //message
                    foreach (int id in GlobalServer.Instance.GetPlayers())
                    {
                        var conn = GlobalServer.Instance.GetConnection(id);
                        try
                        {
                            string message = World.Instance.EntitiesToMessage;
                            Console.WriteLine("send(" + id + ") :" + message);

                            GlobalServer.Instance.SendMessage(id, message);

                            var player = World.Instance.GetEntity(id) as Player;
                            if (player.MapToUpdate)
                            {
                                GlobalServer.Instance.SendMessage(id, player.MapMessage);
                                player.MapToUpdate = false;
                            }
                        }
                        catch (SocketException e)
                        {
                            Disconnected(conn, "" + id);
                        }

                    }
                    //socket.Broadcast(message);
                }

                Thread.Sleep(200);
            }
        }

        private static void InitWorld()
        {
            Pokemon pokemon = new Pokemon(1);
            pokemon.Position = new Position(200, 180);
            World.Instance.AddEntity(pokemon);

            Player other = new Player("other");
            other.Position = new Position(205, 175);
            World.Instance.AddEntity(other);
        }

    }
}

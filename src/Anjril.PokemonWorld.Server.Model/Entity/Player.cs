using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class Player : WorldEntity
    {
        private object movelock = new object();
        private static int chunksize = 20;

        public string Name { get; set; }

        public float MoveInputDelay { get; protected set; }
        public long LastMoveTime { get; set; }
        public bool MapToUpdate { get; set; }

        public List<Pokemon> Pokemons;

        public Player(string name) : base()
        {
            Name = name;
            Type = EntityType.Player;
            LastMoveTime = DateTime.Now.Ticks;
            MoveInputDelay = 0.30f;
            MapToUpdate = true;
            Pokemons = new List<Pokemon>();
            Pokemons.Add(new Pokemon(1));
        }

        public void DoMove(Direction dir)
        {
            lock (movelock)
            {
                var dest = new Position(Position, dir);
                var nextMoveTime = LastMoveTime + (long)(MoveTime * 1000 * 10000);
                var nextMoveInputTime = LastMoveTime + (long)((MoveTime - MoveInputDelay) * 1000 * 10000);

                if (DateTime.Now.Ticks > nextMoveInputTime)
                {
                    var oldSegment = Position.GetSegment(chunksize);
                    var newSegment = dest.GetSegment(chunksize);

                    var result = World.Instance.MoveEntity(Id, dest);
                    if (result)
                    {
                        if (DateTime.Now.Ticks > nextMoveTime)
                        {
                            LastMoveTime = DateTime.Now.Ticks;
                        }
                        else
                        {
                            LastMoveTime = nextMoveTime;
                        }

                        if (!oldSegment.Equals(newSegment))
                        {
                            MapToUpdate = true;
                        }
                    }
                }
            }
        }

        public string MapMessage
        {
            get
            {
                string message = "map:";
                int mapsize = World.Instance.Size;

                Position segment = Position.GetSegment(chunksize);

                var startx = (segment.X - 1) * 20;
                var starty = (segment.Y - 1) * 20;
                if (startx < 0) startx = 0;
                if (starty < 0) starty = 0;

                Position startPos = new Position(startx, starty);
                message += startPos.ToString() + "+";

                var endx = (segment.X + 2) * 20;
                var endy = (segment.Y + 2) * 20;

                Position endPos = new Position(endx, endy);


                for (int y = starty; y < endy - 1; y++)
                {
                    for (int x = startx; x < endx - 1; x++)
                    {
                        Position pos = new Position(x, y);
                        message += (int)World.Instance.GetTile(pos);
                        message += ".";
                        message += (int)World.Instance.GetObject(pos);
                        message += ",";
                        
                    }
                }

                message = message.Remove(message.Length - 1, 1);

                return message;
            }
        }
    }
}

using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class Player : WorldEntity
    {
        #region private fields

        private object movelock = new object();

        private float _moveInputDelay;
        private DateTime _lastMove;

        #endregion

        #region public properties

        public string Name { get; set; }
        public bool MapToUpdate { get; set; }
        public List<Pokemon> Team { get; private set; }

        #endregion

        #region constructor

        public Player(string name) : base()
        {
            Name = name;
            Team = new List<Pokemon>();
            Team.Add(new Pokemon(1));

            Type = EntityType.Player;
            MapToUpdate = true;

            _lastMove = DateTime.Now;
            _moveInputDelay = 0.30f;
        }

        #endregion

        #region public methods

        public void DoMove(Direction dir)
        {
            lock (movelock)
            {
                var dest = new Position(Position, dir);
                var nextMoveTime = _lastMove.AddSeconds(MoveDuration);
                var nextMoveInputTime = _lastMove.AddSeconds(MoveDuration - _moveInputDelay);

                if (DateTime.Now > nextMoveInputTime)
                {
                    var oldSegment = Position.GetSegment(Settings.Default.ChunkSize);
                    var newSegment = dest.GetSegment(Settings.Default.ChunkSize);

                    var result = World.Instance.MoveEntity(Id, dest);
                    if (result)
                    {
                        if (DateTime.Now > nextMoveTime)
                        {
                            _lastMove = DateTime.Now;
                        }
                        else
                        {
                            _lastMove = nextMoveTime;
                        }

                        if (!oldSegment.Equals(newSegment))
                        {
                            MapToUpdate = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region serialization

        public string MapMessage
        {
            get
            {
                string message = "map:";
                int mapsize = World.Instance.Size;

                Position segment = Position.GetSegment(Settings.Default.ChunkSize);

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

        #endregion
    }
}

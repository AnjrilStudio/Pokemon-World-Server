using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.State
{
    public class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }
        
        public Position(Position pos, Direction dir)
        {
            X = pos.X;
            Y = pos.Y;

            switch (dir)
            {
                case Direction.DOWN: Y++; break;
                case Direction.RIGHT: X++; break;
                case Direction.UP: Y--; break;
                case Direction.LEFT: X--; break;
            }
        }

        public Position GetSegment()
        {
            var x = X / 20; //TODO paramètres ?
            var y = Y / 20;

            return new Position(x, y);
        }

        public override string ToString()
        {
            return X + ":" + Y;
        }

        public bool Equals(Position p)
        {
            return X == p.X && Y == p.Y;
        }
    }
}

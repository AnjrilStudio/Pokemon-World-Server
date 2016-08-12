using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.State
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
                case Direction.Down: Y++; break;
                case Direction.Right: X++; break;
                case Direction.Up: Y--; break;
                case Direction.Left: X--; break;
            }
        }

        public Position(string pos)
        {
            var x = pos.Split(':')[0];
            var y = pos.Split(':')[1];
            X = Int32.Parse(x);
            Y = Int32.Parse(y);
        }

        public Position GetSegment(int chunksize)
        {
            var x = X / chunksize;
            var y = Y / chunksize;

            return new Position(x, y);
        }

        public override string ToString()
        {
            return X + ":" + Y;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Position;

            if (p == null)
            {
                return false;
            }

            return X == p.X && Y == p.Y;
        }

        public override int GetHashCode()
        {
            return X * 62482673 + Y;
        }

        public void NormalizePos(int mapsize)
        {
            if (X >= mapsize)
            {
                X = mapsize - 1;
            }

            if (X < 0)
            {
                X = 0;
            }

            if (Y >= mapsize)
            {
                Y = mapsize - 1;
            }

            if (Y < 0)
            {
                Y = 0;
            }
        }

        public static int NormalizedPos(int pos, int mapsize)
        {
            if (pos >= mapsize)
            {
                return mapsize - 1;
            }

            if (pos < 0)
            {
                return 0;
            }

            return pos;
        }

        public static bool isInMap(int x, int y, int mapsize)
        {
            return x >= 0 && y >= 0 && x < mapsize && y < mapsize;
        }

        public static int Distance(Position p1, Position p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public static Position GetSegment(int x, int y, int chunksize)
        {
            var px = x / chunksize;
            var py = y / chunksize;

            return new Position(px, py);
        }
    }
}

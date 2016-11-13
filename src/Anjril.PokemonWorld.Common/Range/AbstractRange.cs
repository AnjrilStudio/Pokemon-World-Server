using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public abstract class AbstractRange
    {
        private bool initialized = false;

        public abstract bool InRange(Arena arena, BattleEntity self, Position target, Direction dir);

        public virtual bool InRange(Arena arena, BattleEntity self, Position target)
        {
            if (!initialized)
            {
                Init(arena, self);
            }
            return InRange(arena, self, target, Direction.None);
        }

        public List<Position> InRangeTiles(BattleEntity self, Direction dir, Arena arena)
        {
            var result = new List<Position>();
            Init(arena, self);

            for (int x = 0; x < arena.Width; x++)
            {
                for (int y = 0; y < arena.Height; y++)
                {
                    Position target = new Position(x, y);
                    if (dir == Direction.None)
                    {
                        if (InRange(arena, self, target))
                        {
                            result.Add(target);
                        }
                    }
                    else
                    {
                        if (InRange(arena, self, target, dir))
                        {
                            result.Add(target);
                        }
                    }
                }
            }

            return result;
        }

        public virtual void Init(Arena arena, BattleEntity self)
        {
            initialized = true;
        }

        public void Reset()
        {
            initialized = false;
        }
    }
}
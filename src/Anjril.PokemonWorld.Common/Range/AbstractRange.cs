using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.Range
{
    public abstract class AbstractRange
    {
        public abstract bool InRange(BattleArena arena, BattleEntity self, Position target, Direction dir);

        public virtual bool InRange(BattleArena arena, BattleEntity self, Position target)
        {
            return InRange(arena, self, target, Direction.None);
        }

        public List<Position> InRangeTiles(BattleEntity self, Direction dir, BattleArena arena)
        {
            var result = new List<Position>();

            for (int x = 0; x < arena.ArenaSize; x++)
            {
                for (int y = 0; y < arena.ArenaSize; y++)
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
    }
}
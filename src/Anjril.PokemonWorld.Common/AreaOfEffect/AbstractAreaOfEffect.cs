using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common.AreaOfEffect
{
    public abstract class AbstractAreaOfEffect
    {

        public abstract bool InArea(Arena arena, Position origin, Position target, Position actionOrigin, Direction dir);

        public bool InArea(Arena arena, Position origin, Position target, Position actionOrigin)
        {
            return InArea(arena, origin, target, actionOrigin, Direction.None);
        }

        public virtual void Init(Arena arena, Position origin, Position actionOrigin)
        {
            //Do nothing
        }

        public List<Position> AoeTiles(Arena arena, Position target, Position actionOrigin, Direction dir, TargetType targetType)
        {
            var result = new List<Position>();

            Init(arena, target, actionOrigin);

            for (int x = 0; x < arena.Width; x++)
            {
                for (int y = 0; y < arena.Height; y++)
                {
                    Position aoe = new Position(x, y);
                    var inArea = false;

                    if (targetType == TargetType.Position)
                    {

                        if (InArea(arena, target, aoe, actionOrigin))
                        {
                            inArea = true;
                        }
                    }

                    if (targetType == TargetType.Directional)
                    {
                        if (InArea(arena, target, aoe, actionOrigin, dir))
                        {
                            inArea = true;
                        }
                    }

                    if (inArea)
                    {
                        result.Add(aoe);
                    }
                }
            }

            return result;
        }
    }
}

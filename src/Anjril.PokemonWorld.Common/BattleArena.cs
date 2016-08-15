
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common
{
    public class BattleArena
    {
        public int ArenaSize { get; private set; }
        public ArenaTile[,] arenaTiles { get; private set; }
        public ArenaObject[,] arenaObjects { get; private set; }

        public BattleArena(int size)
        {
            ArenaSize = size;
            arenaTiles = new ArenaTile[size, size];
            arenaObjects = new ArenaObject[size, size];
        }

        public void MoveBattleEntity(BattleEntity entity, Position target)
        {
            entity.CurrentPos = new Position(target);
        }
    }
    
}

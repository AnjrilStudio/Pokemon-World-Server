
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
        public ArenaTile[,] ArenaTiles { get; private set; }
        public ArenaObject[,] ArenaObjects { get; private set; }
        public BattleEntity[,] Pokemons { get; private set; }

        public BattleArena(int size)
        {
            ArenaSize = size;
            ArenaTiles = new ArenaTile[size, size];
            ArenaObjects = new ArenaObject[size, size];
            Pokemons = new BattleEntity[size, size];
        }

        public void MoveBattleEntity(BattleEntity entity, Position target)
        {
            if (entity.CurrentPos != null)
            {
                Pokemons[entity.CurrentPos.X, entity.CurrentPos.Y] = null;
            }
            entity.CurrentPos = new Position(target);
            Pokemons[entity.CurrentPos.X, entity.CurrentPos.Y] = entity;
        }

        public void RemoveBattleEntity(BattleEntity entity)
        {
            if (entity.CurrentPos != null)
            {
                Pokemons[entity.CurrentPos.X, entity.CurrentPos.Y] = null;
                entity.CurrentPos = null;
            }
        }
    }
    
}

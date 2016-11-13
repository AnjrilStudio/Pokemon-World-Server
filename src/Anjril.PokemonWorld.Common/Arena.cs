
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Effect;

namespace Anjril.PokemonWorld.Common
{
    public abstract class Arena
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ArenaTile[,] ArenaTiles { get; set; }
        public ArenaObject[,] ArenaObjects { get; set; }
        public BattleEntity[,] Pokemons { get; private set; }

        public Arena(int size)
        {
            Width = size;
            Height = size;
            ArenaTiles = new ArenaTile[size, size];
            ArenaObjects = new ArenaObject[size, size];
            Pokemons = new BattleEntity[size, size];
        }

        public Arena(ArenaTile[,] arena)
        {
            Width = arena.GetLength(0);
            Height = arena.GetLength(1);
            ArenaTiles = arena;
            ArenaObjects = new ArenaObject[Width, Height];
            Pokemons = new BattleEntity[Width, Height];
        }

        public virtual bool MoveBattleEntity(BattleEntity entity, Position target)
        {
            if (entity.CurrentPos != null)
            {
                Pokemons[entity.CurrentPos.X, entity.CurrentPos.Y] = null;
            }
            entity.CurrentPos = new Position(target);
            Pokemons[entity.CurrentPos.X, entity.CurrentPos.Y] = entity;

            return true;
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

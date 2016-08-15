using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common
{
    public class BattleEntity
    {
        public int BattleId { get; set; }
        public List<Action> Actions { get; private set; }
        public int PlayerId { get; private set; }
        public int WorldId { get; private set; }
        public int PokedexId { get; private set; }
        public Position CurrentPos { get; set; }
        public bool InBattle { get { return CurrentPos != null; } }
        public bool Ready { get; set; }
        public bool ComingBack { get; set; }

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Level { get; set; }

        public int Atk { get; set; }
        public int Def { get; set; }
        public int AtkSpe { get; set; }
        public int DefSpe { get; set; }
        public int Vit { get; set; }

        public int AP { get; set; }
        public int MaxAP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }


        public BattleEntity(int id, int pokedexId)
        {
            BattleId = id;
            PokedexId = pokedexId;
            PlayerId = -1;
            WorldId = -1;
            Ready = false;
            ComingBack = false;

            Actions = new List<Action>();
            CurrentPos = null;

            HP = 20;
            MaxHP = HP;
            Level = 5;
            Atk = 15;
            Def = 15;
            AtkSpe = 15;
            DefSpe = 15;
            Vit = 15;

            MaxAP = 4;
            AP = MaxAP;
            MaxMP = 3;
            MP = MaxMP;

            Actions.Add(Moves.Get(Move.Move));
            Actions.Add(Moves.Get(Move.Tackle));
            Actions.Add(Moves.Get(Move.Gust));
            Actions.Add(Moves.Get(Move.Bubble));
            Actions.Add(Moves.Get(Move.Water_Gun));
            Actions.Add(Moves.Get(Move.Thunder_Shock));
        }

        public BattleEntity(int id, int pokedexId, int playerId) : this (id, pokedexId)
        {
            PlayerId = playerId;
        }

        public BattleEntity(int id, int pokedexId, int playerId, int worldId) : this(id, pokedexId, playerId)
        {
            WorldId = worldId;
        }
    }
}

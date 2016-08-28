using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Effect;

namespace Anjril.PokemonWorld.Common
{
    public class BattleEntity
    {
        public int BattleId { get; set; }
        public List<Action> Moves { get; private set; }
        public int PlayerId { get; private set; }
        public int WorldId { get; private set; }
        public int PokedexId { get; private set; }
        public Position CurrentPos { get; set; }
        public bool InBattle { get { return CurrentPos != null; } }
        public bool Ready { get; set; }
        public bool ComingBack { get; set; }

        private List<OverTimeEffect> overTimeEffects;

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Level { get; set; }

        public int Atk { get; set; }
        public int Def { get; set; }
        public int AtkSpe { get; set; }
        public int DefSpe { get; set; }
        public int Vit { get; set; }

        public int BaseMaxAP { get; private set; }
        public int MaxAP { get; set; }
        public int AP { get; set; }
        public int BaseMaxMP { get; private set; }
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

            Moves = new List<Action>();
            CurrentPos = null;
            overTimeEffects = new List<OverTimeEffect>();

            HP = 20;
            MaxHP = HP;
            Level = 5;
            Atk = 15;
            Def = 15;
            AtkSpe = 15;
            DefSpe = 15;
            Vit = 15;

            BaseMaxAP = 6;
            MaxAP = BaseMaxAP;
            AP = MaxAP;
            BaseMaxMP = 3;
            MaxMP = BaseMaxMP;
            MP = MaxMP;

            Moves.Add(Common.Moves.Get(Move.Move));
            Moves.Add(Common.Moves.Get(Move.Tackle));
            Moves.Add(Common.Moves.Get(Move.Gust));
            Moves.Add(Common.Moves.Get(Move.Bubble));
            Moves.Add(Common.Moves.Get(Move.Water_Gun));
            Moves.Add(Common.Moves.Get(Move.Thunder_Shock));
            Moves.Add(Common.Moves.Get(Move.Tail_Whip));
            Moves.Add(Common.Moves.Get(Move.Pound));
        }

        public BattleEntity(int id, int pokedexId, int playerId) : this (id, pokedexId)
        {
            PlayerId = playerId;
        }

        public BattleEntity(int id, int pokedexId, int playerId, int worldId) : this(id, pokedexId, playerId)
        {
            WorldId = worldId;
        }

        public void addOverTimeEffect(BattleEntity origin, HitEffectOverTime effect, int duration)
        {
            overTimeEffects.Add(new OverTimeEffect(origin, effect, duration));
        }

        public void applyOverTimeEffect(BattleArena arena)
        {
            List<OverTimeEffect> toRemove = new List<OverTimeEffect>();
            foreach(OverTimeEffect effect in overTimeEffects)
            {
                effect.Duration -= 1;
                effect.Effect.applyOverTime(effect.Origin, this, arena);
                if (effect.Duration == 0)
                {
                    toRemove.Add(effect);
                }
            }

            foreach (OverTimeEffect effect in toRemove)
            {
                overTimeEffects.Remove(effect);
            }

        }
    }
}

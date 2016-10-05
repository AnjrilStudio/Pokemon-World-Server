using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Effect;
using Anjril.PokemonWorld.Common.Index;

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

        public int BaseAtk { get; private set; }
        public int AtkStage { get; set; }
        public int Atk { get { return ComputeStat(BaseAtk, AtkStage); } }

        public int BaseDef { get; private set; }
        public int DefStage { get; set; }
        public int Def { get { return ComputeStat(BaseDef, DefStage); } }

        public int BaseAtkSpe { get; private set; }
        public int AtkSpeStage { get; set; }
        public int AtkSpe { get { return ComputeStat(BaseAtkSpe, AtkSpeStage); } }

        public int BaseDefSpe { get; private set; }
        public int DefSpeStage { get; set; }
        public int DefSpe { get { return ComputeStat(BaseDefSpe, DefSpeStage); } }

        public int BaseSpeed { get; private set; }
        public int SpeedStage { get; set; }
        public int Speed { get { return ComputeStat(BaseSpeed, SpeedStage); } }

        public int BaseMaxAP { get; private set; }
        public int MaxAP { get; set; }
        public int AP { get; set; }
        public int BaseMaxMP { get; private set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }


        public BattleEntity(int id, int pokedexId, int playerId)
        {
            BattleId = id;
            PokedexId = pokedexId;
            PlayerId = playerId;
            WorldId = -1;
            Ready = false;
            ComingBack = false;

            Moves = new List<Action>();
            CurrentPos = null;
            overTimeEffects = new List<OverTimeEffect>();

            MaxHP = 20;
            HP = MaxHP;
            Level = 5;

            BaseAtk = 15;
            AtkStage = 0;
            BaseDef = 15;
            DefStage = 0;
            BaseAtkSpe = 15;
            AtkSpeStage = 0;
            BaseDefSpe = 15;
            DefSpeStage = 0;
            BaseSpeed = 15;
            SpeedStage = 0;

            BaseMaxAP = 6;
            MaxAP = BaseMaxAP;
            AP = MaxAP;
            BaseMaxMP = 3;
            MaxMP = BaseMaxMP;
            MP = MaxMP;

            initMoves();
        }

        public BattleEntity(int id, int pokedexId, int playerId, int worldId) : this(id, pokedexId, playerId)
        {
            WorldId = worldId;
        }

        private void initMoves()
        {
            Moves.Add(Common.Moves.Get(Move.Move));

            /*Moves.Add(Common.Moves.Get(Move.Tackle));
            //Moves.Add(Common.Moves.Get(Move.Gust));
            Moves.Add(Common.Moves.Get(Move.Bubble));
            Moves.Add(Common.Moves.Get(Move.Water_Gun));
            Moves.Add(Common.Moves.Get(Move.Thunder_Shock));
            Moves.Add(Common.Moves.Get(Move.Tail_Whip));
            //Moves.Add(Common.Moves.Get(Move.Pound));
            Moves.Add(Common.Moves.Get(Move.Peck));
            Moves.Add(Common.Moves.Get(Move.Quick_Attack));*/

            PokemonSheet sheet = Pokedex.GetPokemonSheetByNationalId(PokedexId);
            var moves = sheet.LevelingMoves.Split(';');
            foreach(string move in moves)
            {
                var level = move.Split(',')[0];
                var name = move.Split(',')[1];
                Moves.Add(Common.Moves.Get((Move)Enum.Parse(typeof(Move), name)));
            }

        }

        private int ComputeStat(int baseStat, int stage)
        {
            int x = 2;
            int y = 2;
            if (stage > 0)
            {
                x += stage;
            } else
            {
                y -= stage;
            }

            return baseStat * x/y;
        }

        public void resetStatStages()
        {
            AtkStage = 0;
            DefStage = 0;
            AtkSpeStage = 0;
            DefSpeStage = 0;
            SpeedStage = 0;
        }

        public void addOverTimeEffect(BattleEntity origin, HitEffectOverTime effect, int duration)
        {
            overTimeEffects.Add(new OverTimeEffect(origin, effect, duration));
        }

        public void addOverTimeEffect(BattleEntity origin, HitEffectOverTime effect, int duration, Status status)
        {
            overTimeEffects.Add(new OverTimeEffect(origin, effect, duration, status));
        }

        public void applyOverTimeEffect(BattleArena arena)
        {
            List<OverTimeEffect> toRemove = new List<OverTimeEffect>();
            foreach(OverTimeEffect effect in overTimeEffects)
            {
                if (effect.Duration <= 0)
                {
                    toRemove.Add(effect);
                } else
                {
                    effect.Duration -= 1;
                    effect.Effect.applyOverTime(effect.Origin, this, arena);
                }
            }

            foreach (OverTimeEffect effect in toRemove)
            {
                overTimeEffects.Remove(effect);
            }

        }
    }
}

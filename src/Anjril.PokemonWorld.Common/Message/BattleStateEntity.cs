using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public class BattleStateEntity
    {
        public int Id { get; private set; }
        public int PokemonId { get; private set; }
        public int Level { get; private set; }
        public int PlayerId { get; private set; }
        public Position CurrentPos { get; private set; }
        public bool ComingBack { get; private set; }
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int AP { get; private set; }
        public int MaxAP { get; private set; }
        public int MP { get; private set; }
        public int MaxMP { get; private set; }

        public BattleStateEntity(string entityStr)
        {
            var i = 0;
            Id = Int32.Parse(entityStr.Split(',')[i++]);
            PokemonId = Int32.Parse(entityStr.Split(',')[i++]);
            Level = Int32.Parse(entityStr.Split(',')[i++]);
            PlayerId = Int32.Parse(entityStr.Split(',')[i++]);
            CurrentPos = new Position(entityStr.Split(',')[i++]);
            ComingBack = "1" == entityStr.Split(',')[i++];
            HP = Int32.Parse(entityStr.Split(',')[i++]);
            MaxHP = Int32.Parse(entityStr.Split(',')[i++]);
            AP = Int32.Parse(entityStr.Split(',')[i++]);
            MaxAP = Int32.Parse(entityStr.Split(',')[i++]);
            MP = Int32.Parse(entityStr.Split(',')[i++]);
            MaxMP = Int32.Parse(entityStr.Split(',')[i++]);
        }
    }
}
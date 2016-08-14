﻿using Anjril.PokemonWorld.Common.State;
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
        public int PlayerId { get; private set; }
        public Position CurrentPos { get; private set; }
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int AP { get; private set; }
        public int MaxAP { get; private set; }
        public int MP { get; private set; }
        public int MaxMP { get; private set; }

        public BattleStateEntity(string entityStr)
        {
            Id = Int32.Parse(entityStr.Split(',')[0]);
            PokemonId = Int32.Parse(entityStr.Split(',')[1]);
            PlayerId = Int32.Parse(entityStr.Split(',')[2]);
            CurrentPos = new Position(entityStr.Split(',')[3]);
            HP = Int32.Parse(entityStr.Split(',')[4]);
            MaxHP = Int32.Parse(entityStr.Split(',')[5]);
            AP = Int32.Parse(entityStr.Split(',')[6]);
            MaxAP = Int32.Parse(entityStr.Split(',')[7]);
            MP = Int32.Parse(entityStr.Split(',')[8]);
            MaxMP = Int32.Parse(entityStr.Split(',')[9]);
        }
    }
}
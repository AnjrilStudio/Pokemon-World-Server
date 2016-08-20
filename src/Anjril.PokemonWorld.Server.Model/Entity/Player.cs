﻿using Anjril.Common.Network;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model.Index;
using Anjril.PokemonWorld.Server.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class Player : WorldEntity
    {
        #region public properties

        public string Name { get; private set; }
        public Team Team { get; private set; }

        public bool MapToUpdate { get; set; }
        public bool TeamToUpdate { get; set; }
        public DateTime LastMove { get; set; }
        public float MoveInputDelay { get; private set; }
        public IRemoteConnection RemoteConnection { get; private set; }

        public override bool CanSwim { get { return false; } }
        public override bool CanWalk { get { return true; } }
        public override bool CanFly { get { return false; } }
        public override bool CanBeRidden { get { return false; } }

        public Pokedex Pokedex { get; private set; }

        #endregion

        #region constructor

        public Player(string name, IRemoteConnection remote)
            : base(EntityType.Player)
        {
            Name = name;
            Team = new Team();

            Pokedex = new Pokedex();

            MapToUpdate = true;
            TeamToUpdate = true;
            LastMove = DateTime.Now;
            MoveInputDelay = 0.30f;
            RemoteConnection = remote;

            // DEBUG
            Team.AddPokemon(new BattleEntity(-1, 16, Id));
            Team.AddPokemon(new BattleEntity(-1, 19, Id));
        }

        #endregion
    }
}

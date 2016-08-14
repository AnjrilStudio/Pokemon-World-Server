using Anjril.Common.Network;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
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

        public string Name { get; set; }
        public List<BattleEntity> Team { get; private set; }

        public bool MapToUpdate { get; set; }
        public bool TeamToUpdate { get; set; }
        public DateTime LastMove { get; set; }
        public float MoveInputDelay { get; private set; }
        public IRemoteConnection RemoteConnection { get; private set; }

        #endregion

        #region constructor

        public Player(string name, IRemoteConnection remote)
            : base()
        {
            Name = name;
            Team = new List<BattleEntity>();
            Type = EntityType.Player;

            MapToUpdate = true;
            TeamToUpdate = true;
            LastMove = DateTime.Now;
            MoveInputDelay = 0.30f;
            RemoteConnection = remote;

            // DEBUG
            Team.Add(new BattleEntity(-1, 1, Id));
            Team.Add(new BattleEntity(-1, 2, Id));
        }

        #endregion
    }
}

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
        public List<Pokemon> Team { get; private set; }

        public bool MapToUpdate { get; set; }
        public DateTime LastMove { get; set; }
        public float MoveInputDelay { get; private set; }

        #endregion

        #region constructor

        public Player(string name) : base()
        {
            Name = name;
            Team = new List<Pokemon>();
            Type = EntityType.Player;

            MapToUpdate = true;
            LastMove = DateTime.Now;
            MoveInputDelay = 0.30f;

            // DEBUG
            Team.Add(new Pokemon(1));
        }

        #endregion
    }
}

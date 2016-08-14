using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Module
{
    public class WildModule : BaseModule
    {
        #region private fields

        private const double SPAWN_RATE = 0.01d;
        private const double HIDE_RATE = 0.05d;
        private const double MOVE_RATE = 0.5d;

        #endregion

        public WildModule()
            : base(1000)
        { }

        public override void Update(TimeSpan elapsed)
        {
            foreach (var pokemon in World.Instance.Population)
            {
                if (!pokemon.IsVisible && RandomUtils.RandomDouble() < SPAWN_RATE && World.Instance.GetEntity(pokemon.HiddenPosition.X, pokemon.HiddenPosition.Y) == null)
                {
                    pokemon.Position = new Position(pokemon.HiddenPosition);
                    World.Instance.AddEntity(pokemon);
                }
                else if (pokemon.IsVisible && RandomUtils.RandomDouble() < HIDE_RATE)
                {
                    World.Instance.RemoveEntity(pokemon.Id);
                    pokemon.Position = null;
                }
                else if (pokemon.IsVisible)
                {
                    var dest = new Position(pokemon.Position, DirectionUtils.RandomDirection());
                    World.Instance.MoveEntity(pokemon.Id, dest);
                }
            }

            base.Update(elapsed);
        }
    }
}

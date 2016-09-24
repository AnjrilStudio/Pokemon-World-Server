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

        private const double SPAWN_RATE = 0.002d;
        private const double HIDE_RATE = 0.01d;
        private const double MOVE_RATE = 0.1d;

        #endregion

        public WildModule()
            : base(200)
        { }

        public override void Update(TimeSpan elapsed)
        {
            foreach (var pokemon in World.Instance.Population)
            {
                if (!pokemon.IsVisible && RandomUtils.RandomDouble() < SPAWN_RATE)
                {
                    World.Instance.VisibleEntities.Add(pokemon, pokemon.HiddenPosition);
                }
                else if (pokemon.IsVisible && GlobalServer.Instance.GetBattle(pokemon.Id) == null && RandomUtils.RandomDouble() < HIDE_RATE)
                {
                    World.Instance.VisibleEntities.Remove(pokemon.Id);
                }
                else if (pokemon.IsVisible && GlobalServer.Instance.GetBattle(pokemon.Id) == null && RandomUtils.RandomDouble() < MOVE_RATE)
                {
                    var dir = DirectionUtils.RandomDirection();
                    var dest = new Position(pokemon.Position, dir);
                    pokemon.Direction = dir;

                    EntityState newState;
                    if (World.Instance.Map.CanGo(pokemon, dest, out newState))
                    {
                        World.Instance.VisibleEntities.Move(pokemon.Id, dest);
                        pokemon.State = newState;
                    }
                }
            }

            base.Update(elapsed);
        }
    }
}

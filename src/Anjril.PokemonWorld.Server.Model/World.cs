using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Server.Model.WorldMap;
using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Common.Utils;
using Anjril.PokemonWorld.Server.Model.Utils;
using Anjril.PokemonWorld.Server.Model.Persistence;
using Anjril.PokemonWorld.Server.Model.Persistence.Dto;

namespace Anjril.PokemonWorld.Server.Model
{
    public class World
    {
        #region singleton 

        private static World instance;
        public static World Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new World();
                }

                return instance;
            }
        }

        #endregion

        #region private fields

        private Dictionary<int, Player> _players;

        #endregion

        #region public properties

        public IReadOnlyCollection<Player> Players { get; private set; }

        public PositionMatrix VisibleEntities { get; private set; }
        public PopulationMatrix Population { get; private set; }
        public Map Map { get; private set; }

        #endregion

        #region constructor

        private World()
        {
            _players = new Dictionary<int, Player>();
            Players = _players.Values;
        }

        #endregion

        #region players managment

        public bool AddPlayer(Player player)
        {
            if (!_players.ContainsKey(player.Id))
            {
                _players.Add(player.Id, player);

                while (!VisibleEntities.Add(player, player.Position))
                {
                    player.Position = new Position(player.Position, DirectionUtils.RandomDirection());
                }

                return true;
            }

            return false;
        }

        public bool RemovePlayer(int id)
        {
            if (_players.ContainsKey(id))
            {
                _players.Remove(id);
                VisibleEntities.Remove(id);

                return true;
            }

            return false;
        }

        #endregion

        #region initialization

        public void Init(string jsonMap)
        {
            Map = new Map(jsonMap);

            VisibleEntities = new PositionMatrix(Map.Size);
            Population = new PopulationMatrix(Map.Size, VisibleEntities);

            foreach (var player in Players)
            {
                while (!VisibleEntities.Add(player, player.Position))
                {
                    player.Position = new Position(player.Position, DirectionUtils.RandomDirection());
                }
            }
        }

        public void LoadPopulation()
        {
            var population = PopulationDaoImpl.Instance.LoadPopulation();

            if (population != null)
            {
                foreach (PopulationPokemonDto pokemonDto in population.WorldPopulation)
                {
                    Population.Add(new Pokemon(pokemonDto));
                }
            } else
            {
                //population par défaut
                for (int i = 0; i < 15000; i++)
                {
                    var randX = RandomUtils.RandomInt(Map.Size);
                    var randY = RandomUtils.RandomInt(Map.Size);

                    var dest = new Position(randX, randY);

                    // DEBUG
                    var id = RandomUtils.RandomInt(3);
                    id = id == 0 ? 19 : id == 1 ? 16 : 60;

                    var pokemon = new Pokemon(id, dest);
                    EntityState state;

                    while (!Map.CanGo(pokemon, dest, out state))
                    {
                        randX = RandomUtils.RandomInt(Map.Size);
                        randY = RandomUtils.RandomInt(Map.Size);

                        dest = new Position(randX, randY);
                    }

                    pokemon.HiddenPosition = dest;
                    pokemon.State = state;
                    pokemon.Age = RandomUtils.RandomInt(80);

                    Population.Add(pokemon);
                }
            }
        }

        #endregion
    }
}

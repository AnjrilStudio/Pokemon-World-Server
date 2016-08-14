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

        private List<Player> _players;

        private WorldEntity[,] _worldEntities;
        private Dictionary<int, WorldEntity> _mapEntities;

        private Dictionary<int, Pokemon> _mapPopulation;
        private List<Pokemon>[,] _populationEntities;

        private WorldTile[,] _worldTiles;
        private WorldObject[,] _worldObjects;

        #endregion

        #region public properties

        public int Size { get; private set; }

        public IReadOnlyCollection<Player> Players { get; private set; }
        public IReadOnlyCollection<WorldEntity> Entities { get; private set; }
        public IReadOnlyCollection<Pokemon> Population { get; private set; }

        #endregion

        #region constructor

        private World()
        {
            _players = new List<Player>();
            _mapEntities = new Dictionary<int, WorldEntity>();
            _mapPopulation = new Dictionary<int, Pokemon>();

            Players = _players.AsReadOnly();
            Entities = _mapEntities.Values;
            Population = _mapPopulation.Values;
        }

        #endregion

        #region visible entity management

        public WorldEntity GetEntity(int id)
        {
            return _mapEntities[id];
        }

        public WorldEntity GetEntity(Position position)
        {
            return GetEntity(position.X, position.Y);
        }

        public WorldEntity GetEntity(int x, int y)
        {
            return _worldEntities[x, y];
        }

        public void AddEntity(WorldEntity entity)
        {
            _worldEntities[entity.Position.X, entity.Position.Y] = entity;
            _mapEntities.Add(entity.Id, entity);

            if (entity is Player)
            {
                _players.Add(entity as Player);
            }
        }

        public void RemoveEntity(int id)
        {
            var entity = _mapEntities[id];

            _worldEntities[entity.Position.X, entity.Position.Y] = null;
            _mapEntities.Remove(id);

            if (entity is Player)
            {
                _players.Remove(entity as Player);
            }
        }

        public bool MoveEntity(int id, Position pos)
        {
            var entity = _mapEntities[id];

            if (Position.isInMap(pos.X, pos.Y, Size) && _worldEntities[pos.X, pos.Y] == null && World.Instance.GetObject(pos) != WorldObject.Rock) //TODO collision
            {
                _worldEntities[entity.Position.X, entity.Position.Y] = null;
                entity.Position = pos;
                _worldEntities[entity.Position.X, entity.Position.Y] = entity;

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region population management

        public void AddIndividual(Pokemon pokemon)
        {
            var compartment = this._populationEntities[pokemon.HiddenPosition.X, pokemon.HiddenPosition.Y];

            if (compartment == null)
            {
                compartment = new List<Pokemon>();
                this._populationEntities[pokemon.HiddenPosition.X, pokemon.HiddenPosition.Y] = compartment;
            }

            compartment.Add(pokemon);
            _mapPopulation.Add(pokemon.Id, pokemon);
        }

        #endregion

        #region world management

        public WorldTile GetTile(Position position)
        {
            if (Position.isInMap(position.X, position.Y, Size))
            {
                return _worldTiles[position.X, position.Y];
            }
            else
            {
                return WorldTile.Undefined;
            }
        }

        public WorldObject GetObject(Position position)
        {
            if (Position.isInMap(position.X, position.Y, Size))
            {
                return _worldObjects[position.X, position.Y];
            }
            else
            {
                return WorldObject.None;
            }
        }

        #endregion

        #region serialization

        public string BattleStartToMessage(List<int> entitiesList)
        {
            string message = "battlestart:";

            foreach (int entityId in entitiesList)
            {
                message += entityId;
                message += ";";
            }

            return message;
        }

        #endregion

        #region intialization

        public void LoadMap(string jsonMap)
        {
            jsonMap = jsonMap.Substring(1);
            jsonMap = jsonMap.Remove(jsonMap.Length - 3); //caractères en trop ?

            var mapArray = jsonMap.Split(',');
            int x = 0, y = 0;

            Size = (int)Math.Sqrt(mapArray.Length);

            _worldEntities = new WorldEntity[Size, Size];
            _worldTiles = new WorldTile[Size, Size];
            _worldObjects = new WorldObject[Size, Size];
            _populationEntities = new List<Pokemon>[Size, Size];

            foreach (string s in mapArray)
            {
                var tmp = s.Split('.');
                int ground = int.Parse(tmp[0]) - 1;

                switch (ground)
                {
                    case 1:
                    case 2:
                        _worldTiles[x, y] = WorldTile.Sea;
                        break;
                    case 3:
                    case 4:
                    default:
                        _worldTiles[x, y] = WorldTile.Ground;
                        break;
                    case 5:
                        _worldTiles[x, y] = WorldTile.Grass;
                        break;
                    case 6:
                        _worldTiles[x, y] = WorldTile.Sand;
                        break;
                    case 7:
                    case 8:
                        _worldTiles[x, y] = WorldTile.Road;
                        break;
                }

                int mapObj = int.Parse(tmp[1]) - 1;
                switch (mapObj)
                {
                    case 1:
                        _worldObjects[x, y] = WorldObject.Tree;
                        break;
                    case 2:
                        _worldObjects[x, y] = WorldObject.Rock;
                        break;
                    case 3:
                        _worldObjects[x, y] = WorldObject.HighGrass;
                        break;
                    case 4:
                        _worldObjects[x, y] = WorldObject.Bush;
                        break;
                    default:
                        _worldObjects[x, y] = WorldObject.None;
                        break;
                }

                x++;
                if (x == Size)
                {
                    x = 0;
                    y++;
                }
            }
        }

        public void LoadPopulation()
        {
            for (int i = 0; i < 15000; i++)
            {
                var randX = RandomUtils.RandomInt(Size);
                var randY = RandomUtils.RandomInt(Size);

                AddIndividual(new Pokemon(1, new Position(randX, randY)));
            }
        }

        public void Reset()
        {
            _worldEntities = new WorldEntity[Size, Size];
            _mapEntities = new Dictionary<int, WorldEntity>();
        }

        #endregion
    }
}

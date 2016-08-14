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

        private Dictionary<int, WorldEntity> mapEntities;

        private WorldEntity[,] worldEntities;
        private WorldTile[,] worldTiles;
        private WorldObject[,] worldObjects;

        #endregion

        #region public properties

        public int Size { get; private set; }
        public List<WorldEntity> Entities
        {
            get
            {
                return mapEntities.Values.ToList<WorldEntity>();
            }
        }

        #endregion

        #region constructor

        private World()
        {
            Size = 400;
            worldEntities = new WorldEntity[Size, Size];
            mapEntities = new Dictionary<int, WorldEntity>();

            worldTiles = new WorldTile[Size, Size];
            worldObjects = new WorldObject[Size, Size];
        }

        #endregion

        #region entity management

        public WorldEntity GetEntity(int id)
        {
            return mapEntities[id];
        }

        public WorldEntity GetEntity(Position position)
        {
            return GetEntity(position.X, position.Y);
        }

        public WorldEntity GetEntity(int x, int y)
        {
            return worldEntities[x, y];
        }

        public void AddEntity(WorldEntity entity)
        {
            worldEntities[entity.Position.X, entity.Position.Y] = entity;
            mapEntities.Add(entity.Id, entity);
        }

        public void RemoveEntity(int id)
        {
            var entity = mapEntities[id];
            worldEntities[entity.Position.X, entity.Position.Y] = null;
            mapEntities.Remove(id);
        }

        public bool MoveEntity(int id, Position pos)
        {
            var entity = mapEntities[id];

            if (Position.isInMap(pos.X, pos.Y, Size) && worldEntities[pos.X, pos.Y] == null && World.Instance.GetObject(pos) != WorldObject.Rock) //TODO collision
            {
                worldEntities[entity.Position.X, entity.Position.Y] = null;
                entity.Position = pos;
                worldEntities[entity.Position.X, entity.Position.Y] = entity;

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region world management

        public WorldTile GetTile(Position position)
        {
            if (Position.isInMap(position.X, position.Y, Size))
            {
                return worldTiles[position.X, position.Y];
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
                return worldObjects[position.X, position.Y];
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
            jsonMap = jsonMap.Remove(jsonMap.Length - 3); //charactères en trop ?

            var mapArray = jsonMap.Split(',');
            int x = 0, y = 0;

            foreach (string s in mapArray)
            {
                var tmp = s.Split('.');
                int ground = int.Parse(tmp[0]) - 1;

                switch (ground)
                {
                    case 1:
                    case 2:
                        worldTiles[x, y] = WorldTile.Sea;
                        break;
                    case 3:
                    case 4:
                    default:
                        worldTiles[x, y] = WorldTile.Ground;
                        break;
                    case 5:
                        worldTiles[x, y] = WorldTile.Grass;
                        break;
                    case 6:
                        worldTiles[x, y] = WorldTile.Sand;
                        break;
                    case 7:
                    case 8:
                        worldTiles[x, y] = WorldTile.Road;
                        break;
                }

                int mapObj = int.Parse(tmp[1]) - 1;
                switch (mapObj)
                {
                    case 1:
                        worldObjects[x, y] = WorldObject.Tree;
                        break;
                    case 2:
                        worldObjects[x, y] = WorldObject.Rock;
                        break;
                    case 3:
                        worldObjects[x, y] = WorldObject.HighGrass;
                        break;
                    case 4:
                        worldObjects[x, y] = WorldObject.Bush;
                        break;
                    default:
                        worldObjects[x, y] = WorldObject.None;
                        break;
                }

                x++;
                if (x == Size)
                {
                    x = 0;
                    y++;
                }
            }

            // DEBUG
            Pokemon pokemon = new Pokemon(1);
            pokemon.Position = new Position(200, 180);
            AddEntity(pokemon);

            Player other = new Player("other");
            other.Position = new Position(205, 175);
            AddEntity(other);
        }

        public void Reset()
        {
            worldEntities = new WorldEntity[Size, Size];
            mapEntities = new Dictionary<int, WorldEntity>();
        }

        #endregion
    }
}

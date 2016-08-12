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
        public int Mapsize { get; private set; }
        private static World instance;

        private WorldEntity[,] entitiesGrid; 
        private Dictionary<int, WorldEntity> entitiesMap;

        private WorldTile[,] worldTiles;
        private WorldObject[,] worldObjects;

        private World()
        {
            Mapsize = 400;
            entitiesGrid = new WorldEntity[Mapsize, Mapsize];
            entitiesMap = new Dictionary<int, WorldEntity>();

            worldTiles = new WorldTile[Mapsize, Mapsize];
            worldObjects = new WorldObject[Mapsize, Mapsize];
        }

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

        public void AddEntity(WorldEntity entity)
        {
            entitiesGrid[entity.Position.X, entity.Position.Y] = entity;
            entitiesMap.Add(entity.Id, entity);
        }

        public void RemoveEntity(int id)
        {
            var entity = entitiesMap[id];
            entitiesGrid[entity.Position.X, entity.Position.Y] = null;
            entitiesMap.Remove(id);
        }

        public bool MoveEntity(int id, Position pos)
        {
            var entity = entitiesMap[id];

            if (entitiesGrid[pos.X, pos.Y] == null && World.Instance.GetWorldObject(pos) != WorldObject.Rock)
            {
                entitiesGrid[entity.Position.X, entity.Position.Y] = null;
                entity.Position = pos;
                entitiesGrid[entity.Position.X, entity.Position.Y] = entity;

                return true;
            } else
            {
                return false;
            }
        }

        public void Reset()
        {
            entitiesGrid = new WorldEntity[Mapsize, Mapsize];
            entitiesMap = new Dictionary<int, WorldEntity>();
        }

        public List<WorldEntity> EntitiesList
        {
            get
            {
                return entitiesMap.Values.ToList<WorldEntity>();
            }
        }

        public WorldEntity GetEntity(int id)
        {
            return entitiesMap[id];
        }

        public WorldEntity GetEntity(Position position)
        {
            return entitiesGrid[position.X, position.Y];
        }

        public String EntitiesToMessage
        {
            get
            {
                string message = "entities:";

                foreach (WorldEntity entity in EntitiesList)
                {
                    message += entity.ToString();
                    message += ";";
                }

                return message;
            }
        }

        public WorldTile GetWorldTile(Position position)
        {
            return worldTiles[position.X, position.Y];
        }

        public WorldObject GetWorldObject(Position position)
        {
            return worldObjects[position.X, position.Y];
        }

        public void LoadMap(string jsonMap)
        {
            jsonMap = jsonMap.Substring(1);
            jsonMap = jsonMap.Remove(jsonMap.Length - 3); //charactères en trop ?

            var mapArray = jsonMap.Split(',');
            int i = 0, j = 0;

            foreach (string s in mapArray)
            {
                var tmp = s.Split('.');
                int ground = int.Parse(tmp[0]) - 1;

                switch (ground)
                {
                    case 1:
                    case 2:
                        worldTiles[i, j] = WorldTile.Sea;
                        break;
                    case 3:
                    case 4:
                    default:
                        worldTiles[i, j] = WorldTile.Ground;
                        break;
                    case 5:
                        worldTiles[i, j] = WorldTile.Grass;
                        break;
                    case 6:
                        worldTiles[i, j] = WorldTile.Sand;
                        break;
                    case 7:
                    case 8:
                        worldTiles[i, j] = WorldTile.Road;
                        break;
                }

                int mapObj = int.Parse(tmp[1]) - 1;
                switch (mapObj)
                {
                    case 1:
                        worldObjects[i, j] = WorldObject.Tree;
                        break;
                    case 2:
                        worldObjects[i, j] = WorldObject.Rock;
                        break;
                    case 3:
                        worldObjects[i, j] = WorldObject.HighGrass;
                        break;
                    case 4:
                        worldObjects[i, j] = WorldObject.Bush;
                        break;
                    default:
                        worldObjects[i, j] = WorldObject.None;
                        break;
                }

                i++;
                if (i == Mapsize)
                {
                    i = 0;
                    j++;
                }
            }
        }

        public String BattleStartToMessage(List<int> entitiesList)
        {
            string message = "battlestart:";

            foreach (WorldEntity entity in EntitiesList)
            {
                message += entity.Id;
                message += ";";
            }

            return message;
        }
    }
}

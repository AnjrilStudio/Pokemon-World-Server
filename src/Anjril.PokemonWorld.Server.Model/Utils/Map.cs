using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Server.Model.WorldMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Utils
{
    public class Map
    {
        #region private fields

        private WorldTile[,] _worldTiles;
        private WorldObject[,] _worldObjects;

        #endregion

        #region public properties

        public int Size { get; set; }

        #endregion

        #region constructor

        public Map(string jsonMap)
        {
            jsonMap = jsonMap.Substring(1);
            jsonMap = jsonMap.Remove(jsonMap.Length - 3);

            var mapArray = jsonMap.Split(',');

            Size = (int)Math.Sqrt(mapArray.Length);

            _worldTiles = new WorldTile[Size, Size];
            _worldObjects = new WorldObject[Size, Size];

            int x = 0, y = 0;

            foreach (string s in mapArray)
            {
                var tmp = s.Split('.');
                int landType = int.Parse(tmp[0]) - 1;

                switch (landType)
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

        #endregion

        #region getters

        public WorldTile GetTile(Position position)
        {
            if (Position.IsInMap(position.X, position.Y, Size, Size))
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
            if (Position.IsInMap(position.X, position.Y, Size, Size))
            {
                return _worldObjects[position.X, position.Y];
            }
            else
            {
                return WorldObject.None;
            }
        }

        #endregion

        #region collision helpers

        public bool IsBlocked(Position position)
        {
            var obj = GetObject(position);

            return obj != WorldObject.HighGrass && obj != WorldObject.None;
        }

        public bool CanGo(WorldEntity entity, Position position, out EntityState newState)
        {
            bool canGo = false;

            switch (GetTile(position))
            {
                case WorldTile.Sea:
                    canGo = entity.CanSwim && !IsBlocked(position);
                    newState = EntityState.Swimming;
                    break;
                case WorldTile.Ground:
                case WorldTile.Grass:
                case WorldTile.Sand:
                case WorldTile.Road:
                    canGo = entity.CanWalk && !IsBlocked(position);
                    newState = EntityState.Walking;
                    break;
                default:
                    newState = EntityState.Undefined;
                    return false;
            }

            if (entity.CanFly && !canGo)
            {
                newState = EntityState.Flying;
                canGo = true;
            }

            return canGo;
        }

        #endregion
    }
}

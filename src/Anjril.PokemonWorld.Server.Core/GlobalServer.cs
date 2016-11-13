using Anjril.Common.Network;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Core.Battle;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.WorldMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core
{
    public class GlobalServer
    {
        #region singleton

        private static GlobalServer instance;
        public static GlobalServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalServer();
                }

                return instance;
            }
        }

        #endregion

        #region private fields

        private Dictionary<int, IRemoteConnection> playerConnectionMap = new Dictionary<int, IRemoteConnection>();
        private Dictionary<int, BattleState> battles;

        #endregion

        #region constructor

        private GlobalServer()
        {
            playerConnectionMap = new Dictionary<int, IRemoteConnection>();

            battles = new Dictionary<int, BattleState>();
        }

        #endregion

        #region player management

        public void AddPlayer(int playerId, IRemoteConnection remote)
        {
            playerConnectionMap.Add(playerId, remote);
        }

        public void RemovePlayer(int playerId)
        {
            playerConnectionMap.Remove(playerId);
        }

        public List<int> GetPlayers()
        {
            return playerConnectionMap.Keys.ToList();
        }

        #endregion

        #region network management

        public IRemoteConnection GetConnection(int playerId)
        {
            if (playerConnectionMap.ContainsKey(playerId))
            {
                return playerConnectionMap[playerId];
            }
            else
            {
                return null;
            }
        }

        public void SendMessage(int playerId, string message)
        {
            var conn = GetConnection(playerId);
            if (conn != null)
            {
                conn.Send(message);
            }
        }

        #endregion

        #region battle management

        public BattleState NewBattle(List<int> entities)
        {
            var wildDir = GetWildDir(entities);
            BattleState battle = new BattleState(entities, GetArenaPattern(entities), wildDir);
            foreach (int entity in entities)
            {
                battles.Add(entity, battle);
            }
            return battle;
        }

        private ArenaTile[,] GetArenaPattern(List<int> entitiesList)
        {
            ArenaTile[,] pattern = new ArenaTile[4, 3];
            //default
            for (int i = 0; i < pattern.GetLength(0); i++)
            {
                for (int j = 0; j < pattern.GetLength(1); j++)
                {
                    pattern[i, j] = ArenaTile.Ground;
                }
            }

            var pos1 = World.Instance.VisibleEntities[entitiesList[0]].Position;
            var pos2 = World.Instance.VisibleEntities[entitiesList[1]].Position;
            var startpos = new Position(Math.Min(pos1.X, pos2.X) - 1, Math.Min(pos1.Y, pos2.Y) - 1);
            if (Position.Distance(pos1, pos2) == 1)
            {
                if (pos1.X == pos2.X)
                {
                    pattern = new ArenaTile[3, 4];
                }
                else
                {
                    pattern = new ArenaTile[4, 3];
                }
            }

            for (int i = 0; i < pattern.GetLength(0); i++)
            {
                for (int j = 0; j < pattern.GetLength(1); j++)
                {
                    var worldtile = World.Instance.Map.GetTile(new Position(startpos.X + i, startpos.Y + j));
                    switch (worldtile)
                    {
                        case WorldTile.Ground:
                            pattern[i, j] = ArenaTile.Ground;
                            break;
                        case WorldTile.Grass:
                            pattern[i, j] = ArenaTile.Grass;
                            break;
                        case WorldTile.Sea:
                            pattern[i, j] = ArenaTile.Water;
                            break;
                        case WorldTile.Sand:
                            pattern[i, j] = ArenaTile.Sand;
                            break;
                        default:
                            pattern[i, j] = ArenaTile.Ground;
                            break;
                    }
                }
            }

            return pattern;
        }

        private Direction GetWildDir(List<int> entitiesList)
        {
            var ent1 = World.Instance.VisibleEntities[entitiesList[0]];
            var ent2 = World.Instance.VisibleEntities[entitiesList[1]];

            var pos = World.Instance.VisibleEntities[entitiesList[0]].Position;
            var pos2 = World.Instance.VisibleEntities[entitiesList[1]].Position;
            if (ent2.Type == EntityType.Pokemon)
            {
                pos2 = World.Instance.VisibleEntities[entitiesList[0]].Position;
                pos = World.Instance.VisibleEntities[entitiesList[1]].Position;
            }

            if (pos.X == pos2.X)
            {
                if (pos.Y > pos2.Y)
                {
                    return Direction.Down;
                }
                else
                {
                    return Direction.Up;
                }
            }
            else
            {
                if (pos.X > pos2.X)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }
        }

        public void AddBattlePlayer(int playerId, BattleState battle)
        {
            battles.Add(playerId, battle);
        }

        public void RemoveBattle(int entity)
        {
            if (battles.ContainsKey(entity))
            {
                var battle = battles[entity];
                var entities = battle.Entities;
                foreach (int id in entities)
                {
                    battles.Remove(id);
                }
            }
        }

        public BattleState GetBattle(int entity)
        {
            if (battles.ContainsKey(entity))
            {
                return battles[entity];
            }
            else
            {
                return null;
            }
        }

        public void RemoveBattleEntity(int entity)
        {
            var battle = battles[entity];
            battles.Remove(entity);
        }

        #endregion
    }
}

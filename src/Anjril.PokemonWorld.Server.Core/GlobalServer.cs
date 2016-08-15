using Anjril.Common.Network;
using Anjril.PokemonWorld.Server.Core.Battle;
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
            BattleState battle = new BattleState(entities);
            foreach (int entity in entities)
            {
                battles.Add(entity, battle);
            }
            return battle;
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

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
        private static GlobalServer instance;

        private Dictionary<int, IRemoteConnection> playerConnectionMap = new Dictionary<int, IRemoteConnection>();

        private Dictionary<int, BattleState> battles;

        private GlobalServer()
        {
            playerConnectionMap = new Dictionary<int, IRemoteConnection>();

            battles = new Dictionary<int, BattleState>();
        }

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

        public void AddPlayer(int playerId, IRemoteConnection remote)
        {
            playerConnectionMap.Add(playerId, remote);
        }

        public void RemovePlayer(int playerId)
        {
            playerConnectionMap.Remove(playerId);
        }

        public IRemoteConnection GetConnection(int playerId)
        {
            if (playerConnectionMap.ContainsKey(playerId))
            {
                return playerConnectionMap[playerId];
            } else
            {
                return null;
            }
        }

        public List<int> GetPlayers()
        {
            return playerConnectionMap.Keys.ToList();
        }

        public void SendMessage(int playerId, string message)
        {
            var conn = GetConnection(playerId);
            if (conn != null)
            {
                conn.Send(message);
            }
        }

        public BattleState NewBattle(List<int> entities)
        {
            BattleState battle = new BattleState(entities);
            foreach (int entity in entities)
            {
                battles.Add(entity, battle);
            }
            return battle;
        }

        public BattleState GetBattle(int entity)
        {
            if (battles.ContainsKey(entity))
            {
                return battles[entity];
            } else
            {
                return null;
            }
        }

        public void RemoveBattle(int entity)
        {
            if (battles.ContainsKey(entity)){
                var battle = battles[entity];
                var entities = battle.Entities;
                foreach (int id in entities)
                {
                    battles.Remove(id);
                }
            }
        }

        public void RemoveBattleEntity(int entity)
        {
            var battle = battles[entity];
            battles.Remove(entity);
        }
    }
}

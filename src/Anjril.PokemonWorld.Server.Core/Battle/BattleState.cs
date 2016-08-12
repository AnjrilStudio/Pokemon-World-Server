using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Range;
using Anjril.PokemonWorld.Common.AreaOfEffect;
using Anjril.PokemonWorld.Common.Effect;
using Anjril.PokemonWorld.Common.ActionCost;
using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Server.Model;

namespace Anjril.PokemonWorld.Server.Core.Battle
{
    public class BattleState
    {
        private static int defaultsize = 10;
        private System.Random random = new System.Random();

        private List<int> players;
        private List<int> entities;
        private List<BattleEntity> turns;
        private int currentTurn;
        private BattleArena arena;
        private int actionId;
        private int entityIdSequence;

        public BattleState(List<int> entitiesList)
        {
            entityIdSequence = 0;
            actionId = 0;
            turns = new List<BattleEntity>();
            players = new List<int>();
            entities = entitiesList;
            arena = new BattleArena(defaultsize);
            currentTurn = 0;

            foreach(int entityId in entitiesList){
                var entity = World.Instance.GetEntity(entityId);
                if (entity.Type == EntityType.Pokemon)
                {
                    BattleEntity battleEntity = new BattleEntity(entityIdSequence++, (entity as Pokemon).PokemonId, -1);
                    battleEntity.CurrentPos = new Position(2, 5);//TODO
                    turns.Add(battleEntity);
                } else if (entity.Type == EntityType.Player)
                {
                    //TODO
                    var player = entity as Player;
                    players.Add(player.Id);
                    BattleEntity battleEntity = new BattleEntity(entityIdSequence++, (entity as Player).Pokemons[0].PokemonId, player.Id);
                    battleEntity.CurrentPos = new Position(8, 5);//TODO
                    turns.Add(battleEntity);
                }
            }
        }


        public bool PlayAction(Position target, Action action, Direction dir)
        {
            var entity = turns[currentTurn];

            bool inRange = action.Range.InRange(arena, entity, target);
            if (action.Range2 != null && action.Range2.InRange(arena, entity, target))
            {
                inRange = true;
            }

            if (inRange)
            {
                if (action.ActionCost != null)
                {
                    action.ActionCost.ApplyCost(entity, target);
                }

                foreach (GroundEffect effect in action.GroundEffects)
                {
                    effect.apply(entity, target, dir, arena);
                }

                foreach (Position aoe in action.AoeTiles(entity, target, dir, arena))
                {
                    foreach (BattleEntity pokemon in turns)
                    {
                        //todo ne pas toucher soi-même
                        if (aoe.Equals(pokemon.CurrentPos))
                        {
                            foreach (HitEffect effect in action.HitEffects)
                            {
                                effect.apply(entity, pokemon, dir, arena);
                            }
                        }
                    }
                }

                actionId++;

                if (action.NextTurn)
                {
                    NextTurn();
                }

                var message = ToActionMessage(target, action, dir);

                foreach (int id in players)
                {
                    GlobalServer.Instance.SendMessage(id, message);
                }

                while (turns[currentTurn].PlayerId < 0)
                {
                    PlayIA();
                }

                return true;
            }

            return false;
        }

        public bool PlayTrainerAction(Position target, Action action)
        {
            if (action.Id == (int)TrainerAction.End_Battle) //TODO
            {
                actionId++;

                EndPlayerBattle(turns[currentTurn].PlayerId);

                //Todo mutualiser
                if (action.NextTurn)
                {
                    NextTurn();
                }

                var message = ToNoActionMessage();

                foreach (int id in players)
                {
                    GlobalServer.Instance.SendMessage(id, message);
                }

                while (turns[currentTurn].PlayerId < 0 && players.Count > 0)
                {
                    PlayIA();
                }

                return true;
            }

            return false;
        }

        public int CurrentPlayer()
        {
            return turns[currentTurn].PlayerId;
        }

        public void NextTurn()
        {
            currentTurn++;
            if (currentTurn == turns.Count)
            {
                currentTurn = 0;
            }

            turns[currentTurn].AP = turns[currentTurn].MaxAP;
            turns[currentTurn].MP = turns[currentTurn].MaxMP;
        }

        public void EndPlayerBattle(int playerId)
        {
            //TODO vérifier que les pokemon sont rappelés
            
            players.Remove(playerId);
            if (players.Count == 0)
            {
                GlobalServer.Instance.RemoveBattle(playerId);
            } else
            {
                GlobalServer.Instance.RemoveBattleEntity(playerId);
            }
            
            GlobalServer.Instance.SendMessage(playerId, ToEndMessage());
            var player = World.Instance.GetEntity(playerId) as Player;
            player.MapToUpdate = true;
        }

        private void PlayIA()
        {
            var turn = turns[currentTurn];
            Action actionAI = turn.Actions[random.Next(0, turn.Actions.Count)];
            Position targetPos = null;
            var dir = Direction.None;
            while (targetPos == null)
            { // attention boucle infinie potentielle, mais ne devrait jamais arriver
                if (actionAI.TargetType == TargetType.Position)
                {
                    List<Position> targets = actionAI.InRangeTiles(turn, arena);
                    if (targets.Count != 0)
                    {
                        targetPos = targets[random.Next(0, targets.Count)];
                    }

                }

                if (actionAI.TargetType == TargetType.Directional)
                {
                    dir = (Direction)random.Next(1, 5);
                    List<Position> targets = actionAI.InRangeTiles(turn, dir, arena);
                    if (targets.Count != 0)
                    {
                        targetPos = targets[random.Next(0, targets.Count)];
                    }
                }
            }
            PlayAction(targetPos, actionAI, dir);
        }

        public List<int> Players
        {
            get
            {
                return players.ToList();
            }
        }

        public List<int> Entities
        {
            get
            {
                return entities.ToList();
            }
        }

        public string StateMessage()
        {
            string message = currentTurn + "@";

            foreach (BattleEntity entity in turns)
            {
                message += entity.Id + ",";
                message += entity.PokemonId + ",";
                message += entity.CurrentPos + ",";
                message += entity.HP + ",";
                message += entity.MaxHP + ",";
                message += entity.AP + ",";
                message += entity.MaxAP + ",";
                message += entity.MP + ",";
                message += entity.MaxMP;
                message += ";" ;
            }

            return message;
        }

        public string ActionMessage(Position target, Action action, Direction dir)
        {
            string message = "" ;

            message += target.ToString() + ",";
            message += action.Id + ",";
            message += dir.ToString();

            return message;
        }

        public string ToActionMessage(Position target, Action action, Direction dir)
        {
            var message = "battleaction:";
            message += actionId;
            message += "=";
            message += ActionMessage(target, action, dir);
            message += "=";
            message += StateMessage();

            return message;
        }

        public string ToNoActionMessage()
        {
            string message = "battleaction:";
            message += actionId;
            message += "=";
            message += "0";
            message += "=";
            message += StateMessage();

            return message;
        }

        public string ToEndMessage()
        {
            string message = "battleaction:";
            message += actionId;
            message += "=";
            message += "0";
            message += "=";
            message += "0";

            return message;
        }
    }
}

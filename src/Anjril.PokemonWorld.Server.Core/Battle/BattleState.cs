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
using Anjril.PokemonWorld.Common.Utils;

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
        public bool WaitingPokemonGo { get; private set; }

        public BattleState(List<int> entitiesList)
        {
            entityIdSequence = 0;
            actionId = 0;
            turns = new List<BattleEntity>();
            players = new List<int>();
            entities = entitiesList;
            arena = new BattleArena(defaultsize);
            currentTurn = 0;
            WaitingPokemonGo = true;

            foreach (int entityId in entitiesList)
            {
                var entity = World.Instance.GetEntity(entityId);
                if (entity.Type == EntityType.Pokemon)
                {
                    BattleEntity battleEntity = new BattleEntity(entityIdSequence++, (entity as Pokemon).PokedexId, -1);
                    battleEntity.CurrentPos = GetRandomStartPosition(Direction.Left);//TODO
                    turns.Add(battleEntity);
                }
                else if (entity.Type == EntityType.Player)
                {
                    //TODO
                    var player = entity as Player;
                    players.Add(player.Id);

                    //BattleEntity battleEntity = new BattleEntity(entityIdSequence++, (entity as Player).Team[0].PokedexId, player.Id);
                    //battleEntity.CurrentPos = GetRandomStartPosition(Direction.Right);//TODO
                    //turns.Add(battleEntity);
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

                var message = ToActionMessage(turns[currentTurn].PlayerId, target, action, dir);

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

        public bool PlayTrainerAction(Player player, Position target, Action action)
        {
            if (action.Id == (int)TrainerAction.End_Battle && GetPlayerNbPokemons(player.Id) == 0)
            {
                actionId++;

                EndPlayerBattle(turns[currentTurn].PlayerId);

                if (action.NextTurn)
                {
                    NextTurn();
                }

                foreach (int id in players)
                {
                    var message = ToNoActionMessage(id);
                    GlobalServer.Instance.SendMessage(id, message);
                }

                while (turns[currentTurn].PlayerId < 0)
                {
                    PlayIA();
                }

                return true;
            }
            else if (action.Id == (int)TrainerAction.Pokemon_Go)
            {
                BattleEntity battleEntity = new BattleEntity(entityIdSequence++, player.Team[0].PokedexId, player.Id);
                battleEntity.CurrentPos = target;
                turns.Add(battleEntity);

                bool ok = true;
                foreach (int id in players)
                {
                    if (GetPlayerNbPokemons(id) == 0)
                    {
                        ok = false;
                    }
                }

                if (ok)
                {
                    actionId++;
                    WaitingPokemonGo = false;

                    NextTurn();

                    foreach (int id in players)
                    {
                        var message = ToNoActionMessage(id);
                        GlobalServer.Instance.SendMessage(id, message);
                    }

                    while (turns[currentTurn].PlayerId < 0)
                    {
                        PlayIA();
                    }
                }

                return true;
            }
            else if (action.Id == (int)TrainerAction.Pokemon_Come_Back)
            {

                var entity = GetEntity(target);
                if (entity != null && entity.PlayerId == player.Id)
                {
                    var indexof = turns.IndexOf(entity);
                    turns.Remove(entity);
                    if (indexof <= currentTurn)
                    {
                        currentTurn--;
                        if (currentTurn < 0)
                        {
                            currentTurn += turns.Count;
                        }
                    }

                    actionId++;
                    if (GetPlayerNbPokemons(player.Id) == 0)
                    {
                        WaitingPokemonGo = true;
                    }

                    foreach (int id in players)
                    {
                        var message = ToNoActionMessage(id);
                        GlobalServer.Instance.SendMessage(id, message);
                    }

                    return true;
                }


            }

            return false;
        }

        private BattleEntity GetEntity(Position pos)
        {
            foreach (BattleEntity entity in turns)
            {
                if (entity.CurrentPos.Equals(pos))
                {
                    return entity;
                }
            }
            return null;
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
            }
            else
            {
                GlobalServer.Instance.RemoveBattleEntity(playerId);
            }

            GlobalServer.Instance.SendMessage(playerId, ToEndMessage(playerId));
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

        private Position GetRandomStartPosition(Direction dir)
        {
            int x;
            int y;
            do
            {
                x = 0;
                y = 0;
                switch (dir)
                {
                    case Direction.Down:
                        x = random.Next(arena.ArenaSize);
                        y = arena.ArenaSize - 1 - random.Next(arena.ArenaSize) / 2;
                        break;
                    case Direction.Up:
                        x = random.Next(arena.ArenaSize);
                        y = random.Next(arena.ArenaSize) / 2;
                        break;
                    case Direction.Right:
                        x = arena.ArenaSize - 1 - random.Next(arena.ArenaSize) / 2;
                        y = random.Next(arena.ArenaSize);
                        break;
                    case Direction.Left:
                        x = random.Next(arena.ArenaSize) / 2;
                        y = random.Next(arena.ArenaSize);
                        break;
                    default:
                        break;
                }

            } while (!IsTileFree(x, y));

            return new Position(x, y);
        }

        private bool IsTileFree(int x, int y)
        {
            var result = true;
            foreach (BattleEntity entity in turns)
            {
                if (entity.CurrentPos.X == x && entity.CurrentPos.Y == y)
                {
                    result = false;
                }
            }
            return result;
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

        public string CurrentAvailableActionsMessage(int player)
        {
            string message = "";
            if (GetPlayerNbPokemons(player) == 0) //TODO condition aucun ennemi ?
            {
                message += (int)TrainerAction.End_Battle + ",";
            }
            if (GetPlayerNbPokemons(player) < 1)
            { //TODO 6 pokemons
                message += (int)TrainerAction.Pokemon_Go + ",";
            }
            if (GetPlayerNbPokemons(player) > 0)
            {
                message += (int)TrainerAction.Pokemon_Come_Back + ",";
            }
            message += (int)TrainerAction.Pokeball;

            return message;
        }

        public int GetPlayerNbPokemons(int playerId)
        {
            int result = 0;
            foreach (BattleEntity entity in turns)
            {
                if (entity.PlayerId == playerId)
                {
                    result++;
                }
            }
            return result;
        }

        public string StateMessage()
        {
            string message = currentTurn + "@";

            foreach (BattleEntity entity in turns)
            {
                message += entity.Id + ",";
                message += entity.PokemonId + ",";
                message += entity.PlayerId + ",";
                message += entity.CurrentPos + ",";
                message += entity.HP + ",";
                message += entity.MaxHP + ",";
                message += entity.AP + ",";
                message += entity.MaxAP + ",";
                message += entity.MP + ",";
                message += entity.MaxMP;
                message += ";";
            }

            return message;
        }

        public string ActionMessage(Position target, Action action, Direction dir)
        {
            string message = "";

            message += target.ToString() + ",";
            message += action.Id + ",";
            message += DirectionUtils.ToString(dir);

            return message;
        }

        public string ToActionMessage(int player, Position target, Action action, Direction dir)
        {
            var message = "battleaction:";
            message += actionId;
            message += "=";
            message += CurrentAvailableActionsMessage(player);
            message += "=";
            message += ActionMessage(target, action, dir);
            message += "=";
            message += StateMessage();

            return message;
        }

        public string ToNoActionMessage(int player)
        {
            string message = "battleaction:";
            message += actionId;
            message += "=";
            message += CurrentAvailableActionsMessage(player);
            message += "=";
            message += "0";
            message += "=";
            message += StateMessage();

            return message;
        }

        public string ToEndMessage(int player)
        {
            string message = "battleaction:";
            message += actionId;
            message += "=";
            message += CurrentAvailableActionsMessage(player);
            message += "=";
            message += "0";
            message += "=";
            message += "0";

            return message;
        }
    }
}

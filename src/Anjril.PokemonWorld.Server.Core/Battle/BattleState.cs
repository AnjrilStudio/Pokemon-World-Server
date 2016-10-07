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
using Anjril.PokemonWorld.Server.Core.Module;

namespace Anjril.PokemonWorld.Server.Core.Battle
{
    public class BattleState
    {
        private static int defaultsize = 10;
        private System.Random random = new System.Random();

        private List<int> players;
        private List<int> spectators;
        private List<int> entities;
        private List<BattleEntity> turns;
        private int currentTurn;
        private BattleArena arena;
        private int actionId;
        private int entityIdSequence;
        public bool WaitingPokemonGo { get; private set; }
        public bool HasAttacked { get; private set; }
        public bool HasMoved { get; private set; }
        public bool CanAttack { get; private set; }
        public bool CanMove { get; private set; }
        public bool CannotAttack { get; private set; } //priorité sur le can

        public BattleState(List<int> entitiesList)
        {
            entityIdSequence = 0;
            actionId = 0;
            turns = new List<BattleEntity>();
            players = new List<int>();
            spectators = new List<int>();
            entities = entitiesList;
            arena = new BattleArena(defaultsize);
            currentTurn = 0;
            WaitingPokemonGo = true;
            HasAttacked = false;
            HasMoved = false;
            CanAttack = true;
            CanMove = true;
            CannotAttack = false;

            foreach (int entityId in entitiesList)
            {
                var entity = World.Instance.VisibleEntities[entityId];
                if (entity.Type == EntityType.Pokemon)
                {
                    BattleEntity battleEntity = new BattleEntity(entityIdSequence++, (entity as Pokemon).PokedexSheet.NationalId, -1, (entity as Pokemon).Level, entityId);
                    arena.MoveBattleEntity(battleEntity, GetRandomStartPosition(Direction.Left));
                    turns.Add(battleEntity);
                }
                else if (entity.Type == EntityType.Player)
                {
                    //TODO bord
                    var player = entity as Player;
                    players.Add(player.Id);
                    foreach (BattleEntity pokemon in player.Team)
                    {
                        arena.RemoveBattleEntity(pokemon);
                    }
                }
            }
        }

        private bool CanPlayAction(BattleEntity entity, Action action, Position target, Direction dir)
        {
            if (entity.HP == 0) return false;
            if (entity.ComingBack) return false;
            if (!action.ActionCost.CheckCost(arena, entity, target)) return false;
            if (action.Id == (int)Move.Move && !CanMove) return false;
            if (!action.CanMoveBefore && HasMoved) return false;
            if (!action.CanAttackBefore && HasAttacked && action.Id != (int)Move.Move && !CanAttack) return false;
            
            //priorite
            if (action.Id != (int)Move.Move && CannotAttack) return false;
            if (action.CannotAttackBefore && HasAttacked) return false;

            return true;
        }


        public bool PlayAction(Position target, Action action, Direction dir)
        {
            var entity = turns[currentTurn];
            var oldPos = entity.CurrentPos;
            if (CanPlayAction(entity, action, target, dir))
            {
                action.Range.Reset();
                if (action.Range2 != null) action.Range2.Reset();

                bool inRange = action.Range.InRange(arena, entity, target);
                if (action.Range2 != null && action.Range2.InRange(arena, entity, target))
                {
                    inRange = true;
                }

                if (inRange)
                {
                    if (action.ActionCost != null)
                    {
                        action.ActionCost.ApplyCost(arena, entity, target);
                    }

                    var aoeTiles = action.AoeTiles(entity, target, dir, arena);
                    foreach (Position aoe in aoeTiles)
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

                    foreach (HitEffect effect in action.SelfEffects)
                    {
                        effect.apply(entity, entity, dir, arena);
                    }

                    foreach (GroundEffect effect in action.GroundEffects)
                    {
                        effect.apply(entity, target, dir, arena, currentTurn);
                    }

                    actionId++;
                    if (action.Id == (int)Move.Move)
                    {
                        HasMoved = true;
                    } else
                    {
                        HasAttacked = true;
                    }
                    
                    CanMove = action.CanMoveAfter;
                    CanAttack = action.CanAttackAfter || HasComboAttack();
                    CannotAttack = action.CannotAttackAfter;

                    if (!CanMove && (!CanAttack || CannotAttack))
                    {
                        NextTurn();
                    }

                    foreach (int id in players)
                    {
                        if (action.Id == (int) Move.Move) //voir d'autres moves avec ce comportement
                        {
                            var currentPos = oldPos;
                            while (aoeTiles.Count > 0)
                            {
                                foreach (Position pos in aoeTiles)
                                {
                                    if (Position.Distance(pos, oldPos) == 1)
                                    {
                                        currentPos = pos;
                                        break;
                                    }
                                }
                                var moveDir = DirectionUtils.FromPosition(oldPos, currentPos);
                                if (moveDir == Direction.None) break;

                                arena.MoveBattleEntity(entity, currentPos);
                                var message = ToActionMessage(id, target, action, moveDir);
                                GlobalServer.Instance.SendMessage(id, message);

                                aoeTiles.Remove(currentPos);
                                oldPos = currentPos;
                                if (aoeTiles.Count > 0) actionId++;
                            }
                        } else
                        {
                            var message = ToActionMessage(id, target, action, dir);
                            GlobalServer.Instance.SendMessage(id, message);
                        }
                        
                    }

                    playIATurns();

                    return true;
                }
            }

            return false;
        }

        public bool PlayTrainerAction(Player player, Position target, Action action, int index)
        {
            //TODO check range

            if (action.Id == (int)TrainerAction.End_Battle && GetPlayerNbPokemons(player.Id) == 0)
            {
                if (spectators.Contains(player.Id))
                {
                    spectators.Remove(player.Id);
                    EndPlayerBattle(player.Id, true);
                } else
                {
                    actionId++;

                    EndPlayerBattle(player.Id, false);
                    
                    NextTurn();

                    foreach (int id in players)
                    {
                        var message = ToNoActionMessage(id);//TODO
                        GlobalServer.Instance.SendMessage(id, message);
                    }

                    playIATurns();
                }

                return true;
            }
            else if (action.Id == (int)TrainerAction.Pokemon_Go)
            {
                if (spectators.Contains(player.Id))
                {
                    BattleEntity battleEntity = player.Team[index];
                    battleEntity.BattleId = entityIdSequence++;
                    arena.MoveBattleEntity(battleEntity, target);
                    battleEntity.Ready = false;
                    turns.Add(battleEntity);

                    WaitingPokemonGo = true;
                    spectators.Remove(player.Id);
                }
                else if (!player.Team[index].InBattle)
                {
                    BattleEntity battleEntity = player.Team[index];
                    battleEntity.BattleId = entityIdSequence++;
                    arena.MoveBattleEntity(battleEntity, target);
                    battleEntity.Ready = false;
                    turns.Add(battleEntity);

                    bool battleok = true;
                    foreach (int id in players)
                    {
                        if (!spectators.Contains(id))
                        {
                            if (GetPlayerNbPokemons(id) == 0)
                            {
                                battleok = false;
                            }
                        }
                    }

                    if (battleok)
                    {
                        actionId++;
                        if (!WaitingPokemonGo)
                        {
                            NextTurn();
                        } else
                        {
                            CheckTurn();
                        }

                        foreach (int id in players)
                        {
                            var message = ToNoActionMessage(id);//TODO
                            GlobalServer.Instance.SendMessage(id, message);
                        }
                        
                        playIATurns();
                        WaitingPokemonGo = false;
                    }

                    return true;
                }
            }
            else if (action.Id == (int)TrainerAction.Pokemon_Come_Back)
            {

                var entity = GetEntity(target);
                if (entity != null && entity.PlayerId == player.Id)
                {
                    entity.ComingBack = true;

                    actionId++;
                    if (GetPlayerNbPokemons(player.Id) == 0)
                    {
                        WaitingPokemonGo = true;
                    }

                    foreach (int id in players)
                    {
                        var message = ToNoActionMessage(id);//TODO
                        GlobalServer.Instance.SendMessage(id, message);
                    }

                    return true;
                }
            }
            else if (action.Id == (int)TrainerAction.Pokeball && !spectators.Contains(player.Id))
            {
                var entity = GetEntity(target);
                if (entity != null && entity.PlayerId < 0)
                {
                    var indexof = turns.IndexOf(entity);
                    turns.Remove(entity);
                    if (indexof <= currentTurn)
                    {
                        currentTurn--;
                        if (currentTurn < 0)
                        {
                            currentTurn = 0;
                        }
                    }

                    player.Team.AddPokemon(new BattleEntity(-1, entity.PokedexId, player.Id, entity.Level)); //TODO boîte
                    player.TeamToUpdate = true;

                    World.Instance.Population.Remove(entity.WorldId);
                }

                actionId++;

                NextTurn();

                foreach (int id in players)
                {
                    var message = ToNoActionMessage(id);//TODO
                    GlobalServer.Instance.SendMessage(id, message);
                }

                playIATurns();
            }
            else if (action.Id == (int)TrainerAction.End_Turn)
            {
                actionId++;

                NextTurn();

                foreach (int id in players)
                {
                    var message = ToNoActionMessage(id);
                    GlobalServer.Instance.SendMessage(id, message);
                }

                playIATurns();
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

        private bool HasComboAttack()
        {
            //cherche si une attaque CanAttackBefore est dispo
            foreach (Action move in turns[currentTurn].Moves)
            {
                if (move.Id != (int) Move.Move && move.CanAttackBefore) return true;
            }

            return false;
        }

        //tour terminé
        public void NextTurn()
        {
            if (turns.Count > 1)
            {
                do
                {
                    updateTurns();
                } while (turns.Count > 1 && (!turns[currentTurn].Ready || turns[currentTurn].ComingBack));
                
                UpdateStats(turns[currentTurn]);
            }
        }

        //on check si le tour est à jouer
        public void CheckTurn()
        {
            bool next = false;
            while (turns.Count > 1 && (!turns[currentTurn].Ready || turns[currentTurn].ComingBack))
            {
                updateTurns();
                next = true;
            }

            if (next)
            {
                UpdateStats(turns[currentTurn]);
            }
        }

        //debut d'un cycle
        private void updateTurns()
        {
            currentTurn++;

            if (currentTurn >= turns.Count)
            {
                //application des effets de terrain qui aurait un index plus élevé (cas où il y avait plus de pokemons dans l'arène avant)
                var maxTurnIndex = arena.GetGroundEffectMaxTurnIndex();
                for(int t = currentTurn; t <= maxTurnIndex; t++)
                {
                    arena.applyOverTimeGroundEffects(t);
                }

                //retour à 0
                currentTurn = 0;

                //suppression des pokemons rappelés
                var turnsToRemove = new List<BattleEntity>();
                foreach (BattleEntity turn in turns)
                {
                    turn.Ready = true;

                    if (turn.ComingBack)
                    {
                        turnsToRemove.Add(turn);
                    }
                }
                foreach (BattleEntity turn in turnsToRemove)
                {
                    turns.Remove(turn);
                    arena.RemoveBattleEntity(turn);
                    turn.ComingBack = false;
                }

                //tri par vitesse
                turns.Sort(delegate (BattleEntity b1, BattleEntity b2)
                {
                    return b1.PokedexId.CompareTo(b2.Speed);
                });
            }

            arena.applyOverTimeGroundEffects(currentTurn);
        }

        private void UpdateStats(BattleEntity entity)
        {
            entity.MaxAP = entity.BaseMaxAP;
            entity.MaxMP = entity.BaseMaxMP;
            entity.resetStatStages();
            entity.applyOverTimeEffects(arena);
            if (entity.MaxAP < 0) entity.MaxAP = 0;
            if (entity.MaxMP < 0) entity.MaxMP = 0;
            entity.AP = entity.MaxAP;
            entity.MP = entity.MaxMP;
            
            HasAttacked = false;
            HasMoved = false;
            CanAttack = true;
            CanMove = true;
        }

        private void playIATurns()
        {
            if (turns.Count > 1)
            {
                while (turns[currentTurn].PlayerId < 0)
                {
                    var result = PlayIA();

                    if (!result)
                    {
                        actionId++;

                        NextTurn();

                        foreach (int id in players)
                        {
                            var message = ToNoActionMessage(id);
                            GlobalServer.Instance.SendMessage(id, message);
                        }
                    }
                }
            }
        }

        public void AddSpectator(int playerId)
        {
            players.Add(playerId);
            spectators.Add(playerId);
            actionId++; //todo message ici
            GlobalServer.Instance.AddBattlePlayer(playerId, this);
        }

        public bool IsSpectator(int playerId)
        {
            return spectators.Contains(playerId);
        }

        public void EndPlayerBattle(int playerId, bool spectator)
        {
            players.Remove(playerId);
            if (players.Count == 0)
            {
                GlobalServer.Instance.RemoveBattle(playerId);
            }
            else
            {
                GlobalServer.Instance.RemoveBattleEntity(playerId);
            }

            GlobalServer.Instance.SendMessage(playerId, ToEndMessage(playerId, spectator));
            var player = World.Instance.VisibleEntities[playerId] as Player;
            player.MapToUpdate = true;
        }

        private bool PlayIA()
        {
            var turn = turns[currentTurn];
            Action actionAI = turn.Moves[random.Next(0, turn.Moves.Count)];
            Position targetPos = null;
            var dir = Direction.None;
            while (targetPos == null)
            { // attention boucle infinie potentielle, mais ne devrait jamais arriver
                bool invalidAction = false;
                if (actionAI.TargetType == TargetType.Position)
                {
                    List<Position> targets = actionAI.InRangeTiles(turn, arena);
                    if (actionAI.Range2 != null)
                    {
                        targets.AddRange(actionAI.InRange2Tiles(turn, arena));
                    }
                    if (targets.Count != 0)
                    {
                        targetPos = targets[random.Next(0, targets.Count)];
                    }
                    else
                    {
                        invalidAction = true;
                    }

                }

                if (actionAI.TargetType == TargetType.Directional)
                {
                    dir = (Direction)random.Next(1, 5);
                    List<Position> targets = actionAI.InRangeTiles(turn, dir, arena);
                    if (actionAI.Range2 != null)
                    {
                        targets.AddRange(actionAI.InRange2Tiles(turn, dir, arena));
                    }
                    if (targets.Count != 0)
                    {
                        targetPos = targets[random.Next(0, targets.Count)];
                    }
                    else
                    {
                        invalidAction = true;
                    }
                }
                if (invalidAction)
                {
                    actionAI = turn.Moves[random.Next(0, turn.Moves.Count)];
                }
            }
            return PlayAction(targetPos, actionAI, dir);
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
            return arena.Pokemons[x, y] == null;
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

            if (GetPlayerNbPokemons(player) < 2) // 2 pokemons max en jeu
            { //TODO 6 pokemons
                message += (int)TrainerAction.Pokemon_Go + ",";
            }
            if (GetPlayerNbPokemons(player) > 0)
            {
                message += (int)TrainerAction.Pokemon_Come_Back + ",";
            }
            message += (int)TrainerAction.Pokeball;
            if (GetPlayerNbPokemons(player) > 0)
            {
                message += ",";
                message += (int)TrainerAction.End_Turn;
            }
            if (GetPlayerNbPokemons(player) == 0)
            {
                message += ",";
                message += (int)TrainerAction.End_Battle;
            }

            return message;
        }

        public int GetPlayerNbPokemons(int playerId)
        {
            int result = 0;
            foreach (BattleEntity entity in turns)
            {
                if (entity.PlayerId == playerId && !entity.ComingBack)
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
                message += entity.BattleId + ",";
                message += entity.PokedexId + ",";
                message += entity.Level + ",";
                message += entity.PlayerId + ",";
                message += entity.CurrentPos + ",";
                message += (entity.ComingBack ? "1" : "0") + ",";
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

        public string ToEndMessage(int player, bool spectator)
        {
            string message = "battleaction:";
            message += spectator?actionId+1:actionId;
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

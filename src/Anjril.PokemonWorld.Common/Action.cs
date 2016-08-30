using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Range;
using Anjril.PokemonWorld.Common.AreaOfEffect;
using Anjril.PokemonWorld.Common.Effect;
using Anjril.PokemonWorld.Common.ActionCost;

namespace Anjril.PokemonWorld.Common
{
    public class Action
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        public TargetType TargetType;
        public AbstractRange Range { get; set; }
        public AbstractRange Range2 { get; set; }
        public AbstractAreaOfEffect AreaOfEffect;
        public List<HitEffect> HitEffects { get; private set; }
        public List<GroundEffect> GroundEffects { get; private set; }
        public AbstractActionCost ActionCost { get; set; }
        
        public bool IsTrainer { get; private set; }
        public PokemonType MoveType { get; set; }

        public bool CanMoveBefore { get; set; }
        public bool CanMoveAfter { get; set; }

        public bool CanAttackBefore { get; set; } //priorité sur le false
        public bool CanAttackAfter { get; set; } //priorité sur le false
        public bool CannotAttackBefore { get; set; } //priorité sur le can
        public bool CannotAttackAfter { get; set; } //priorité sur le can

        public Action(Move move)
        {
            Id = (int)move;
            Name = move.ToString();
            TargetType = TargetType.None;
            HitEffects = new List<HitEffect>();
            GroundEffects = new List<GroundEffect>();
            IsTrainer = false;

            CanMoveBefore = true;
            CanMoveAfter = false;

            CanAttackBefore = false;
            CanAttackAfter = false;
            CannotAttackBefore = false;
            CannotAttackAfter = false;
        }

        public Action(TrainerAction action)
        {
            Id = (int)action;
            Name = action.ToString();
            TargetType = TargetType.None;
            HitEffects = new List<HitEffect>();
            GroundEffects = new List<GroundEffect>();
            IsTrainer = true;
        }

        public List<Position> InRangeTiles(BattleEntity self, Direction dir, BattleArena arena)
        {
            if (Range == null)
            {
                return new List<Position>();
            }
            return Range.InRangeTiles(self, dir, arena);
        }

        public List<Position> InRange2Tiles(BattleEntity self, Direction dir, BattleArena arena)
        {
            if (Range2 == null)
            {
                return new List<Position>();
            }
            return Range2.InRangeTiles(self, dir, arena).Except(Range.InRangeTiles(self, dir, arena)).ToList();
        }

        public List<Position> InRangeTiles(BattleEntity self, BattleArena arena)
        {
            return InRangeTiles(self, Direction.None, arena);
        }

        public List<Position> InRange2Tiles(BattleEntity self, BattleArena arena)
        {
            return InRange2Tiles(self, Direction.None, arena);
        }

        public List<Position> AoeTiles(BattleEntity self, Position target, Direction dir, BattleArena arena)
        {
            var result = new List<Position>();
            if (AreaOfEffect == null)
            {
                result.Add(target);
                return result;
            }

            int startX = Position.NormalizedPos(target.X - AreaOfEffect.MaxArea, arena.ArenaSize);
            int endX = Position.NormalizedPos(target.X + AreaOfEffect.MaxArea, arena.ArenaSize);
            int startY = Position.NormalizedPos(target.Y - AreaOfEffect.MaxArea, arena.ArenaSize);
            int endY = Position.NormalizedPos(target.Y + AreaOfEffect.MaxArea, arena.ArenaSize);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    Position origin = target;
                    Position aoe = new Position(x, y);
                    bool inArea = false;

                    if (TargetType == TargetType.Position)
                    {

                        if (AreaOfEffect.InArea(origin, aoe))
                        {
                            inArea = true;
                        }
                    }

                    if (TargetType == TargetType.Directional)
                    {
                        if (AreaOfEffect.InArea(origin, aoe, dir))
                        {
                            inArea = true;
                        }
                    }

                    if (inArea)
                    {
                        result.Add(aoe);
                    }
                }
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;

namespace Anjril.PokemonWorld.Common.Effect
{
    public class RandomPushEffect : HitEffect
    {
        public int Dist { get; private set; }

        public RandomPushEffect(int dist)
        {
            Dist = dist;
        }

        public override void apply(BattleEntity self, BattleEntity target, Direction dir, BattleArena arena)
        {
            var randomdir = DirectionUtils.RandomDirection();

            Position newPos = new Position(target.CurrentPos.X + PositionUtils.GetDirPosition(dir, true).X * Dist, target.CurrentPos.Y + PositionUtils.GetDirPosition(randomdir, true).Y * Dist);
            newPos.NormalizePos(arena.Width, arena.Height);
            arena.MoveBattleEntity(target, newPos);
        }
    }
}

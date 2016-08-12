using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.Range;
using Anjril.PokemonWorld.Common.AreaOfEffect;
using Anjril.PokemonWorld.Common.Effect;
using Anjril.PokemonWorld.Common.ActionCost;

namespace Anjril.PokemonWorld.Common
{
    public class Moves
    {
        private static Action[] moves = new Action[10];

        public static Action Get(Move move)
        {
            if (moves[(int)move] == null)
            {
                init(move);
            }
            return moves[(int)move];
        }

        private static void init(Move move)
        {
            Action action = new Action(move);

            switch (move)
            {
                case Move.Move:
                    action.TargetType = TargetType.Position;
                    action.Range = new DistanceMPRange(1);
                    action.Range2 = new DistanceMPAPRange(1);
                    action.GroundEffects.Add(new MoveEffect());
                    action.ActionCost = new MPAPDistanceActionCost(1);
                    action.NextTurn = false;
                    break;

                case Move.Tackle:
                    action.TargetType = TargetType.Position;
                    action.Range = new DistanceRange(3);
                    action.AreaOfEffect = new DistanceAreaOfEffect(1);
                    action.HitEffects.Add(new DamageEffect(40));
                    break;

                case Move.Gust:
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(2);
                    action.AreaOfEffect = new LineAreaOfEffect(4);
                    action.HitEffects.Add(new PushEffect(1));
                    action.HitEffects.Add(new DamageEffect(50));
                    break;
                case Move.Bubble:
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(2);
                    action.AreaOfEffect = new LineAreaOfEffect(4);
                    action.HitEffects.Add(new PushEffect(1));
                    action.HitEffects.Add(new DamageEffect(50));
                    break;
                case Move.Water_Gun:
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(1);
                    action.AreaOfEffect = new LineAreaOfEffect(5);
                    action.HitEffects.Add(new DamageEffect(50));
                    break;
                case Move.Thunder_Shock:
                    action.TargetType = TargetType.Position;
                    action.Range = new DistanceRange(5);
                    action.HitEffects.Add(new DamageEffect(40));
                    break;
                default:
                    break;
            }

            moves[(int)move] = action;
        }
    }
}

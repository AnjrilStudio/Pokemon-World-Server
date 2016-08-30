using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anjril.PokemonWorld.Common.Range;
using Anjril.PokemonWorld.Common.AreaOfEffect;
using Anjril.PokemonWorld.Common.Effect;
using Anjril.PokemonWorld.Common.ActionCost;
using Anjril.PokemonWorld.Common.State;

namespace Anjril.PokemonWorld.Common
{
    public class Moves
    {
        private static Action[] moves = new Action[50];

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
                    action.Range2 = new DistanceMPAPRange(1, 1);
                    action.GroundEffects.Add(new MoveEffect());
                    action.ActionCost = new MPAPDistanceActionCost(1);
                    action.CanAttackBefore = true;
                    action.CanMoveBefore = true;
                    action.CanAttackAfter = true;
                    action.CanMoveAfter = true;
                    break;
                case Move.Ember:
                    action.MoveType = PokemonType.Fire;
                    action.TargetType = TargetType.Position;
                    action.Range = new DistanceRange(3);
                    action.AreaOfEffect = new DistanceAreaOfEffect(1);
                    action.HitEffects.Add(new DamageEffect(40));
                    action.ActionCost = new APActionCost(4);
                    break;
                case Move.Gust:
                    action.MoveType = PokemonType.Flying;
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(2);
                    action.AreaOfEffect = new LineAreaOfEffect(4);
                    action.HitEffects.Add(new PushEffect(1));
                    action.HitEffects.Add(new DamageEffect(50));
                    action.ActionCost = new APActionCost(4);
                    //TODO le vrai
                    break;
                case Move.Bubble:
                    action.MoveType = PokemonType.Water;
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(2);
                    action.AreaOfEffect = new LineAreaOfEffect(4);
                    action.HitEffects.Add(new DamageEffect(50));
                    action.ActionCost = new APActionCost(4);
                    action.HitEffects.Add(new StatEffect(Stat.Speed, -1, 1));
                    break;
                case Move.Water_Gun:
                    action.MoveType = PokemonType.Water;
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(1);
                    action.AreaOfEffect = new LineAreaOfEffect(5);
                    action.HitEffects.Add(new DamageEffect(50));
                    action.ActionCost = new APActionCost(4);
                    break;
                case Move.Thunder_Shock:
                    action.MoveType = PokemonType.Electric;
                    action.TargetType = TargetType.Position;
                    action.Range = new DistanceRange(5);
                    action.HitEffects.Add(new DamageEffect(40));
                    action.ActionCost = new APActionCost(4);
                    break;
                case Move.Tail_Whip:
                    action.MoveType = PokemonType.Normal;
                    action.Range = new SelfRange();
                    action.TargetType = TargetType.Position;
                    action.AreaOfEffect = new DistanceAreaOfEffect(3);
                    action.ActionCost = new APActionCost(3);
                    break;
                case Move.Scratch:
                    action.MoveType = PokemonType.Normal;
                    action.Range = new LineRange(1);
                    action.TargetType = TargetType.Directional;
                    action.AreaOfEffect = new RightAreaOfEffect(1);
                    action.ActionCost = new APActionCost(4);
                    break;
                case Move.Pound:
                    action.MoveType = PokemonType.Normal;
                    action.Range = new LineRange(1);
                    action.TargetType = TargetType.Position;
                    action.HitEffects.Add(new DamageEffect(40));
                    action.ActionCost = new APActionCost(4);
                    action.HitEffects.Add(new MaxAPEffect(-1, 2));
                    break;
                case Move.Peck:
                    action.MoveType = PokemonType.Flying;
                    action.Range = new LineRange(1);
                    action.TargetType = TargetType.Position;
                    action.HitEffects.Add(new DamageEffect(35));
                    action.ActionCost = new APActionCost(3);
                    action.CanAttackAfter = true;
                    break;
                case Move.Growl:
                    action.MoveType = PokemonType.Normal;
                    action.Range = new SelfRange();
                    action.TargetType = TargetType.Position;
                    action.AreaOfEffect = new DistanceAreaOfEffect(3);
                    action.ActionCost = new APActionCost(3);
                    action.HitEffects.Add(new StatEffect(Stat.Attack, -1, 3));
                    break;
                case Move.Leer:
                    action.MoveType = PokemonType.Normal;
                    action.Range = new DistanceRange(6);
                    action.TargetType = TargetType.Position;
                    action.ActionCost = new APActionCost(3);
                    action.HitEffects.Add(new StatEffect(Stat.Attack, -1, 5));
                    break;
                case Move.Tackle:
                    action.MoveType = PokemonType.Normal;
                    action.TargetType = TargetType.Directional;
                    action.Range = new LineRange(1);
                    action.HitEffects.Add(new DamageEffect(50));
                    action.HitEffects.Add(new PushEffect(1));
                    action.ActionCost = new APActionCost(4);
                    break;
                case Move.Poison_Sting:
                    action.MoveType = PokemonType.Poison;
                    action.TargetType = TargetType.Position;
                    action.Range = new LineRange(2);
                    action.ActionCost = new APActionCost(4);
                    action.HitEffects.Add(new StatusEffect(0.5, Status.Poison, 10));
                    break;
                case Move.String_Shot:
                    action.MoveType = PokemonType.Bug;
                    action.TargetType = TargetType.Position;
                    action.Range = new DistanceRange(3);
                    action.ActionCost = new APActionCost(4);
                    action.HitEffects.Add(new StatEffect(Stat.Speed, -1, 3));
                    action.HitEffects.Add(new MaxMPEffect(-2, 3));
                    break;
                default:
                    break;
            }

            moves[(int)move] = action;
        }
    }
}

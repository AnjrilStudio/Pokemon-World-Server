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
    public class TrainerActions
    {
        private static Action[] trainerActions = new Action[10];

        public static Action Get(TrainerAction trainerAction)
        {
            if (trainerActions[(int)trainerAction] == null)
            {
                init(trainerAction);
            }
            return trainerActions[(int)trainerAction];
        }

        private static void init(TrainerAction trainerAction)
        {
            Action action = new Action(trainerAction);

            switch (trainerAction)
            {
                case TrainerAction.End_Turn:
                case TrainerAction.End_Battle:
                    break;
                case TrainerAction.Pokeball:
                    action.TargetType = TargetType.Position;
                    action.Range = new ArenaRange();
                    action.NextTurn = true;
                    break;
                case TrainerAction.Pokemon_Come_Back:
                    action.TargetType = TargetType.Position;
                    action.Range = new ArenaRange();
                    action.NextTurn = false;
                    break;
                case TrainerAction.Pokemon_Go:
                    action.TargetType = TargetType.Position;
                    action.Range = new ArenaRange();
                    action.NextTurn = true;
                    break;
                default:
                    break;
            }

            trainerActions[(int)trainerAction] = action;
        }
    }
}

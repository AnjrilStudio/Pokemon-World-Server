using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public class BattleActionMessage
    {
        public int ActionId { get; private set; }
        public List<TrainerAction> ActionsAvailable { get; private set; }
        public Position Target { get; private set; }
        public Action Action { get; private set; }
        public Direction Dir { get; private set; }
        public BattleStateMessage State { get; private set; }
        public ArenaTile[,] Arena { get; private set; }

        public BattleActionMessage(string battleStr)
        {
            var actionId = battleStr.Split('=')[0];
            ActionId = System.Int32.Parse(actionId);

            ActionsAvailable = new List<TrainerAction>();
            var actionAvailStr = battleStr.Split('=')[1];
            for (int i = 0; i < actionAvailStr.Split(',').Count(); i++)
            {
                ActionsAvailable.Add((TrainerAction)(System.Int32.Parse(actionAvailStr.Split(',')[i])));
            }

            var actionStr = battleStr.Split('=')[2];
            if (actionStr != "0")
            {
                Target = new Position(actionStr.Split(',')[0]);
                Action = Moves.Get((Move)(System.Int32.Parse(actionStr.Split(',')[1])));
                Dir = DirectionUtils.FromString(actionStr.Split(',')[2]);
            }

            var stateStr = battleStr.Split('=')[3];
            if (stateStr != "0")
            {
                State = new BattleStateMessage(stateStr);
            }

            var arenaStr = battleStr.Split('=')[4];
            if (arenaStr != "0")
            {
                var height = arenaStr.Split(';').Count();
                var width = arenaStr.Split(';')[0].Split(',').Count();
                Arena = new ArenaTile[width, height];
                for (int j = 0; j < height; j++)
                {
                    var lineStr = arenaStr.Split(';')[j];
                    for (int i = 0; i < width; i++)
                    {
                        Arena[i, j] = (ArenaTile)System.Int32.Parse(lineStr.Split(',')[i]);
                    }
                }
            }
        }
    }
}
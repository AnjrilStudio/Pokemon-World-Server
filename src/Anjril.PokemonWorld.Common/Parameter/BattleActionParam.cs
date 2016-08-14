using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Parameter
{
    public class BattleActionParam : BaseParam
    {
        #region public properties

        public Direction Direction { get; private set; }
        public Position Target { get; private set; }
        public Action Action { get; private set; }

        #endregion

        #region constructors

        public BattleActionParam()
            : base("act")
        { }

        public BattleActionParam(Direction dir, Position target, Action action)
            : this()
        {
            this.Direction = dir;
            this.Target = target;
            this.Action = action;
        }

        #endregion

        #region serialization

        public override void DeserializeArguments(string args)
        {
            var splitedArg = args.Split(',');

            if (splitedArg.Length < 3)
            {
                this.IsValid = false;
            }
            else
            {
                try
                {
                    Target = new Position(splitedArg[0]);
                    Action = Moves.Get((Move)Int32.Parse(splitedArg[1]));
                    Direction = DirectionUtils.FromString(splitedArg[2]);

                    this.IsValid = true;
                }
                catch (Exception)
                {
                    this.IsValid = false;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("{0},{1},{2}", Target, Action.Id, DirectionUtils.ToString(Direction));
        }

        #endregion
    }
}

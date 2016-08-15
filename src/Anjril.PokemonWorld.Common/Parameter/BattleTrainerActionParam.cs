using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Parameter
{
    public class BattleTrainerActionParam : BaseParam
    {
        #region public properties

        public Position Target { get; set; }
        public Action Action { get; set; }
        public int Index { get; set; }

        #endregion

        #region constructors

        public BattleTrainerActionParam()
            : base("tra")
        { }

        public BattleTrainerActionParam(Position target, Action action, int index)
            : this()
        {
            this.Index = index;
            this.Target = target;
            this.Action = action;
        }

        #endregion

        #region serialization

        public override void DeserializeArguments(string args)
        {
            var splitedArgs = args.Split(',');

            if (splitedArgs.Length < 3)
            {
                IsValid = false;
            }
            else
            {
                try
                {
                    Target = new Position(splitedArgs[0]);
                    Action = TrainerActions.Get((TrainerAction)Int32.Parse(splitedArgs[1]));
                    Index = Int32.Parse(splitedArgs[2]);

                    IsValid = true;
                }
                catch (Exception)
                {
                    IsValid = false;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("{0},{1},{2}", Target, Action.Id, Index);
        }

        #endregion
    }
}

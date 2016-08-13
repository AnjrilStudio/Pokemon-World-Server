using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Parameter
{
    public class BattleStartParam : BaseParam
    {
        #region constructors

        public BattleStartParam()
            : base("act")
        { }

        #endregion

        #region serialization

        public override void DeserializeArguments(string args)
        {
            IsValid = String.IsNullOrEmpty(args);
        }

        #endregion
    }
}

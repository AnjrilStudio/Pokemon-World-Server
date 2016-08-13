using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common
{
    public enum CommandType
    {
        [Description("mov")]
        Move,
        [Description("trn")]
        Turn,
        [Description("btl")]
        BattleStart,
        [Description("tra")]
        BattleTrainerAction,
        [Description("act")]
        BattleAction
    }
}

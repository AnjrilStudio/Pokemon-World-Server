using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Parameter
{
    public abstract class BaseParam
    {
        #region public properties

        public string CommandTrigram { get; private set; }
        public bool IsValid { get; protected set; }

        #endregion

        #region constructor

        public BaseParam(string trigram)
        {
            this.CommandTrigram = trigram;
        }

        #endregion

        #region serialization

        public abstract void DeserializeArguments(string args);

        public override string ToString()
        {
            return CommandTrigram + "/";
        }

        #endregion
    }
}

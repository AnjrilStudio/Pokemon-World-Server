using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public abstract class BaseMessage
    {
        #region public properties

        public string MessageTrigram { get; private set; }
        public bool IsValid { get; protected set; }

        #endregion

        #region constructor

        public BaseMessage(string trigram)
        {
            this.MessageTrigram = trigram;
        }

        #endregion

        #region serialization

        public abstract void DeserializeArguments(string args);

        public override string ToString()
        {
            return MessageTrigram + ":";
        }

        #endregion
    }
}

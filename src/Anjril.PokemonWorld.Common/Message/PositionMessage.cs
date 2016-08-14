using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public class PositionMessage : BaseMessage
    {
        #region public properties

        public IList<PositionEntity> Entities { get; private set; }

        #endregion

        #region constructor

        public PositionMessage(IList<PositionEntity> entities)
            : this()
        {
            Entities = entities;
        }

        public PositionMessage()
            : base("entities")
        { }

        #endregion

        #region serialization

        public override void DeserializeArguments(string args)
        {
            try
            {
                Entities = args.Split(';').Select(e => new PositionEntity(e)).ToList();

                IsValid = true;
            }
            catch (Exception)
            {
                IsValid = false;
            }
        }

        public override string ToString()
        {
            return base.ToString() + String.Join(";", Entities.Select(e => e.ToString()).ToArray());
        }

        #endregion
    }
}
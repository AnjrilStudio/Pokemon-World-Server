using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Message
{
    public class MapMessage : BaseMessage
    {
        #region public properties

        public Position Origin { get; private set; }
        public string Segments { get; private set; }

        #endregion

        #region constructors

        public MapMessage(Position origin, string segments)
            : this()
        {
            Origin = origin;
            Segments = segments;
        }

        public MapMessage()
            : base("map")
        { }

        #endregion

        #region serialization

        public override void DeserializeArguments(string args)
        {
            try
            {
                var splitArgs = args.Split('+');

                Origin = new Position(splitArgs[0]);
                Segments = splitArgs[1];

                IsValid = true;
            }
            catch (Exception)
            {
                IsValid = false;
            }
        }

        public override string ToString()
        {
            return base.ToString() + String.Format("{0}+{1}", Origin, Segments);
        }

        #endregion
    }
}
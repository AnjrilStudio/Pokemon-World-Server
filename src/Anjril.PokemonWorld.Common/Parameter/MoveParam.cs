using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Parameter
{
    public class MoveParam : BaseParam
    {
        #region public properties

        public Direction Direction { get; set; }

        #endregion

        #region constructors

        public MoveParam()
            : base("mov")
        { }

        public MoveParam(Direction dir)
            : this()
        {
            this.Direction = dir;
        }

        #endregion

        #region serialization

        public override void DeserializeArguments(string args)
        {
            Direction = DirectionUtils.FromString(args);
            IsValid = Direction != Direction.None;
        }

        public override string ToString()
        {
            return base.ToString() + DirectionUtils.ToString(Direction);
        }

        #endregion
    }
}

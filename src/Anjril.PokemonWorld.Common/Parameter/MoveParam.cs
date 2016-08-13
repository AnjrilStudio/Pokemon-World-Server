using Anjril.PokemonWorld.Common.State;
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
            IsValid = true;

            switch (args)
            {
                case "Left":
                    Direction = Direction.Left;
                    break;
                case "Down":
                    Direction = Direction.Down;
                    break;
                case "Right":
                    Direction = Direction.Right;
                    break;
                case "Up":
                    Direction = Direction.Up;
                    break;
                default:
                    IsValid = false;
                    break;
            }
        }

        public override string ToString()
        {
            return base.ToString() + Direction;
        }

        #endregion
    }
}

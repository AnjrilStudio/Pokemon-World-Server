using Anjril.PokemonWorld.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anjril.PokemonWorld.Common.Parameter
{
    public class TurnParam : BaseParam
    {
        #region public properties

        public Direction Direction { get; set; }

        #endregion

        #region constructors

        public TurnParam()
            : base("mov")
        { }

        public TurnParam(Direction dir)
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

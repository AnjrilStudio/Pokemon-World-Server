using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Command
{
    public interface ICommand
    {
        #region properties

        bool CanRun { get; }

        #endregion

        #region methods

        void Run();

        #endregion
    }
}

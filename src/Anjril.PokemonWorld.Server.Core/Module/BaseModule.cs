using System;

namespace Anjril.PokemonWorld.Server.Core.Module
{
    public abstract class BaseModule : IModule
    {
        #region public properties

        public long Interval { get; private set; }
        public DateTime LastUpdate { get; private set; }

        #endregion

        #region constructor

        public BaseModule(long interval)
        {
            Interval = interval;
            LastUpdate = DateTime.Now;
        }

        #endregion

        #region public methods

        public virtual void Update(TimeSpan elapsed)
        {
            this.LastUpdate = DateTime.Now;
        }

        #endregion
    }
}
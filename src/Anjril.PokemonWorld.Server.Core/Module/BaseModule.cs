using Anjril.PokemonWorld.Common.Message;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.Entity;
using System;
using System.Collections.Generic;

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

        #region protected methods

        protected void SendMessage(Player player, BaseMessage message)
        {
            player.RemoteConnection.Send(message.ToString());
        }

        protected List<Pokemon> FindPopulationInArea(int x, int y, int size)
        {
            var result = new List<Pokemon>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var mapsize = World.Instance.Map.Size;
                    var tmpPos = new Position(i + x - size / 2, j + y - size / 2);
                    tmpPos.NormalizePos(mapsize, mapsize);

                    if (Position.IsInMap(tmpPos.X, tmpPos.Y, mapsize, mapsize))
                    {
                        if (World.Instance.Population[tmpPos.X, tmpPos.Y] != null)
                        {
                            foreach (Pokemon pokemon in World.Instance.Population[tmpPos.X, tmpPos.Y])
                            {
                                result.Add(pokemon);
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
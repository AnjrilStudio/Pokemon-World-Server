﻿using Anjril.PokemonWorld.Server.Model.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Entity
{
    public class WorldEntity
    {
        public int Id { get; private set; }
        public Orientation Orientation { get; set; }
    }
}

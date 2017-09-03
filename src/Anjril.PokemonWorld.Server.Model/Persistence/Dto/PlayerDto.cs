using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Server.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Model.Persistence.Dto
{
    public class PlayerDto
    {
        public string Name { get; set; }
        public PokemonDto[] Team { get; set; }

        public PlayerDto(Player player)
        {
            Name = player.Name;
            Team = new PokemonDto[6];
            int teamindex = 0;
            foreach (BattleEntity pokemon in player.Team.Pokemons)
            {
                Team[teamindex] = new PokemonDto();
                Team[teamindex].PokedexId = pokemon.PokedexId;
                Team[teamindex].Level = pokemon.Level;
                Team[teamindex].TotalXp = pokemon.TotalXp;
                teamindex++;
            }
        }

        public PlayerDto()
        {
            Team = new PokemonDto[6];
        }
    }
}

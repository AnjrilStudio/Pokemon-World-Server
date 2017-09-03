using Anjril.PokemonWorld.Common;
using Anjril.PokemonWorld.Common.State;
using Anjril.PokemonWorld.Common.Utils;
using Anjril.PokemonWorld.Server.Model;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Server.Model.Persistence;
using Anjril.PokemonWorld.Server.Model.Persistence.Dto;
using Anjril.PokemonWorld.Server.Model.WorldMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjril.PokemonWorld.Server.Core.Module
{
    public class PopulationModule : BaseModule
    {
        #region private fields
        private float timeSpeed = 0.1f; //debug 10f

        //reproduction
        private float reproductionTimer = 1.5f;
        private float reproductionTime = 20f;
        private int repArea = 8;
        private int repAge = 10;
        private Dictionary<int, Dictionary<WorldTile, double>> reproductionChanceMatrix;
        private int breedNoRepTime = 3;
        private float maleFactorPerMale = 0.18f;
        private float maleFactorPerSameGroupMale = 0.12f;
        private float maleFactorPerOtherMale = 0.03f;

        private float moveTimer = 0.5f;
        private float moveTime = 2f;
        private float moveChance = 0.6f;
        private float moveChanceMalusPerLevel = 0.003f;

        //age
        private float ageTimer = 0;
        private float ageTime = 5f;
        private int ageValue = 1;
        private int ageLimit = 100;
        private float ageDeathChance = 0.05f;

        //xp
        private float xpTimer = 0;
        private float xpTime = 5f;
        private float xpGainFactor = 0.3f;
        private int[,] xpMatrix;

        private float debugTimer = 5f;
        private float debugTime = 5f;

        private float saveTimer = 0f;
        private float saveTime = 30f;

        private Random random = new Random();

        #endregion

        public PopulationModule()
            : base(200)
        {

            InitReproductionMatrix();
            xpMatrix = UpdateXpMatrix();
        }

        private void InitReproductionMatrix()
        {
            reproductionChanceMatrix = new Dictionary<int, Dictionary<WorldTile, double>>();
            //roucool
            reproductionChanceMatrix.Add(16, new Dictionary<WorldTile, double>());
            reproductionChanceMatrix[16].Add(WorldTile.Grass, 0.40);
            reproductionChanceMatrix[16].Add(WorldTile.Ground, 0.20);

            //rattata
            reproductionChanceMatrix.Add(19, new Dictionary<WorldTile, double>());
            reproductionChanceMatrix[19].Add(WorldTile.Grass, 0.30);
            reproductionChanceMatrix[19].Add(WorldTile.Ground, 0.50);

            //ptitard
            reproductionChanceMatrix.Add(60, new Dictionary<WorldTile, double>());
            reproductionChanceMatrix[60].Add(WorldTile.Grass, 0.10);
            reproductionChanceMatrix[60].Add(WorldTile.Sea, 0.50);
        }

        public override void Update(TimeSpan elapsed)
        {
            reproductionTimer += elapsed.Milliseconds * timeSpeed / 1000f;
            moveTimer += elapsed.Milliseconds * timeSpeed / 1000f;
            ageTimer += elapsed.Milliseconds * timeSpeed / 1000f;
            xpTimer += elapsed.Milliseconds * timeSpeed / 1000f;

            debugTimer += elapsed.Milliseconds / 1000f;
            saveTimer += elapsed.Milliseconds / 1000f;
            
            if (moveTimer > moveTime)
            {
                movePhase();
                moveTimer -= moveTime;
            }


            if (reproductionTimer > reproductionTime)
            {
                int repCount = reproductionPhase();
                reproductionTimer -= reproductionTime;
                Console.WriteLine("Births : " + repCount);
            }

            if (ageTimer > ageTime)
            {
                int deathCount = agePhase();
                ageTimer -= ageTime;
                Console.WriteLine("Deaths : " + deathCount);
            }

            if (xpTimer > xpTime)
            {
                int levelUpCount = xpPhase();
                xpTimer -= xpTime;
                Console.WriteLine("LevelUp : " + levelUpCount);
            }

            if (saveTimer > saveTime)
            {
                Console.WriteLine("Saving population ....");
                PopulationDaoImpl.Instance.SavePopulation(new PopulationDto());
                Console.WriteLine("Population saved");
                saveTimer -= saveTime;
            }

            /*if (debugTimer > debugTime)
            {
                int[] total = new int[1000];
                foreach (var pokemon in World.Instance.Population)
                {
                    total[pokemon.PokedexSheet.NationalId]++;
                }
                Console.WriteLine("------------- Total : " + total[16] + " | " + total[19] + " | " + total[60]);
                debugTimer -= debugTime;
            }*/

            base.Update(elapsed);
        }

        private void movePhase()
        {
            var tmpList = new List<Pokemon>();
            foreach (var pokemon in World.Instance.Population)
            {
                if (random.NextDouble() < moveChance - pokemon.Level * moveChanceMalusPerLevel)
                {
                    tmpList.Add(pokemon);
                }
            }


            foreach (Pokemon pokemon in tmpList)
            {
                movePokemon(pokemon, DirectionUtils.RandomDirection());
            }
        }


        private int reproductionPhase()
        {
            int repCount = 0;
            var tmpList = new List<Pokemon>();

            foreach (var male in World.Instance.Population)
            {
                if (male.Gender == Gender.Male && male.Age >= repAge)
                {
                    var females = FindGenderInArea(male.HiddenPosition.X, male.HiddenPosition.Y, repArea, Gender.Female);

                    foreach (var female in females)
                    {
                        if (GetSameEggGroup(male, female) != EggGroup.None)
                        {
                            WorldTile tile = World.Instance.Map.GetTile(female.HiddenPosition);
                            double repChance = 0;
                            if (reproductionChanceMatrix[female.PokedexSheet.NationalId].Keys.Contains(tile))
                            {
                                repChance = reproductionChanceMatrix[female.PokedexSheet.NationalId][tile];
                            }

                            var maleFactor = FindMaleFactor(male, female);

                            repChance = repChance - maleFactor;

                            if (repChance > 0 && random.NextDouble() < repChance)
                            {
                                tmpList.Add(male);//todo mémoriser un couple M/F pour la génétique
                            }
                        }
                    }
                }
            }

            foreach (Pokemon pokemon in tmpList)
            {
                pokemon.NoRepTime = breedNoRepTime;
                newPokemon(pokemon.HiddenPosition.X, pokemon.HiddenPosition.Y, pokemon.PokedexSheet.NationalId);
                repCount++;
            }

            return repCount;
        }

        private int agePhase()
        {
            int deathCount = 0;
            var tmpList = new List<Pokemon>();

            foreach (var pokemon in World.Instance.Population)
            {
                tmpList.Add(pokemon);
            }

            foreach (var pokemon in tmpList)
            {
                var death = agePokemon(pokemon, ageValue);
                if (death) deathCount++;
            }

            return deathCount;
        }

        private int xpPhase()
        {
            var levelUpCount = 0;

            foreach (var pokemon in World.Instance.Population)
            {
                var xpFactor = xpMatrix[pokemon.HiddenPosition.X, pokemon.HiddenPosition.Y] * 0.01f * xpGainFactor;
                var xpGain = GetXpGain(pokemon, xpFactor);
                bool levelUp = pokemon.AddXp(xpGain);
                if (levelUp)
                {
                    levelUpCount++;
                }
            }

            return levelUpCount;
        }

        private int[,] UpdateXpMatrix()
        {
            var mapsize = World.Instance.Map.Size;
            int[,] xpMatrix = new int[mapsize, mapsize];

            for (int x = 0; x < mapsize; x++)
            {
                for (int y = 0; y < mapsize; y++)
                {
                    var tile = World.Instance.Map.GetTile(new Position(x, y));
                    if (tile == WorldTile.Road)
                    {
                        xpMatrix[x, y] = 0;
                    }
                    else
                    {
                        xpMatrix[x, y] = 99;
                    }
                }
            }
            var updateCount = 99999;
            while (updateCount > 0){
                updateCount = 0;
                for (int x = 0; x < mapsize; x++)
                {
                    for (int y = 0; y < mapsize; y++)
                    {
                        bool update = UpdateXpFactor(xpMatrix, x, y, mapsize);
                        if (update)
                        {
                            updateCount++;
                        }
                    }
                }
            }

            return xpMatrix;
        }

        private bool UpdateXpFactor(int[,] xpMatrix, int x, int y, int mapsize)
        {
            
            var minValue = xpMatrix[x, y] - 1;
            if (Position.IsInMap(x + 1, y, mapsize, mapsize) && xpMatrix[x + 1, y] < minValue)
            {
                minValue = xpMatrix[x + 1, y];
            }
            if (Position.IsInMap(x, y + 1, mapsize, mapsize) && xpMatrix[x, y + 1] < minValue)
            {
                minValue = xpMatrix[x, y + 1];
            }
            if (Position.IsInMap(x - 1, y, mapsize, mapsize) && xpMatrix[x - 1, y] < minValue)
            {
                minValue = xpMatrix[x - 1, y];
            }
            if (Position.IsInMap(x, y - 1, mapsize, mapsize) && xpMatrix[x, y - 1] < minValue)
            {
                minValue = xpMatrix[x, y - 1];
            }
            if (minValue < xpMatrix[x, y] - 1)
            {
                xpMatrix[x, y] = minValue + 1;
                return true;
                /*if (Position.IsInMap(x + 1, y, mapsize, mapsize) && xpMatrix[x + 1, y] > minValue + 2)
                {
                    UpdateXpFactor(xpMatrix, x + 1, y, mapsize);
                }
                if (Position.IsInMap(x, y + 1, mapsize, mapsize) && xpMatrix[x, y + 1] > minValue + 2)
                {
                    UpdateXpFactor(xpMatrix, x, y + 1, mapsize);
                }
                if (Position.IsInMap(x - 1, y, mapsize, mapsize) && xpMatrix[x - 1, y] > minValue + 2)
                {
                    UpdateXpFactor(xpMatrix, x - 1, y, mapsize);
                }
                if (Position.IsInMap(x, y - 1, mapsize, mapsize) && xpMatrix[x, y - 1] > minValue + 2)
                {
                    UpdateXpFactor(xpMatrix, x, y - 1, mapsize);
                }*/
            }
            return false;
        }

        private void movePokemon(Pokemon pokemon, Direction dir)
        {
            Position p = PositionUtils.GetDirPosition(dir, true);
            Position newPos = new Position(pokemon.HiddenPosition.X + p.X, pokemon.HiddenPosition.Y + p.Y);

            var mapsize = World.Instance.Map.Size;
            newPos.NormalizePos(mapsize, mapsize);

            World.Instance.Population.Move(pokemon, newPos);
        }

        private bool agePokemon(Pokemon pokemon, int value)
        {
            bool death = false;
            pokemon.Age += value;

            if (pokemon.NoRepTime > 0)
            {
                pokemon.NoRepTime -= value;
            }

            if (pokemon.Age > ageLimit)
            {
                if (random.NextDouble() < ageDeathChance)
                {
                    World.Instance.Population.Remove(pokemon.Id);
                    death = true;
                }
            }

            return death;
        }

        private Pokemon newPokemon(int x, int y, int id)
        {
            Pokemon newPokemon = new Pokemon(id, new Position(x,y));
            
            World.Instance.Population.Add(newPokemon);

            return newPokemon;
        }

        private int GetXpGain(Pokemon pokemon, float xpFactor)
        {
            var currentLevelXp = XpUtils.getXpForLevel(pokemon.Level, XpFormula.Medium_Fast);
            var nextLevelXp = XpUtils.getXpForLevel(pokemon.Level + 1, XpFormula.Medium_Fast);
            var levelXp = nextLevelXp - currentLevelXp;

            return (int) Math.Floor(levelXp * xpFactor);
        }

        private float FindMaleFactor(Pokemon male, Pokemon female)
        {
            var eggGroup = GetSameEggGroup(male, female);
            float maleFactor = 0;

            var males = FindGenderInArea(female.HiddenPosition.X, female.HiddenPosition.Y, repArea, Gender.Male);

            foreach (Pokemon otherMale in males)
            {
                if (otherMale.PokedexSheet.NationalId == male.PokedexSheet.NationalId)
                {
                    maleFactor += maleFactorPerMale;
                } else if (GetSameEggGroup(otherMale, female) != EggGroup.None)
                {
                    maleFactor += maleFactorPerSameGroupMale;
                } else
                {
                    maleFactor += maleFactorPerOtherMale;
                }
            }

            return maleFactor;
        }

        private List<Pokemon> FindGenderInArea(int x, int y, int size, Gender gender)
        {
            var result = new List<Pokemon>();

            var pokemons = FindPopulationInArea(x, y, size);
            foreach(Pokemon pokemon in pokemons)
            {
                if (pokemon.Gender == gender)
                {
                    result.Add(pokemon);
                }
            }

            return result;
        }

        private EggGroup GetSameEggGroup(Pokemon pokemon1, Pokemon pokemon2)
        {
            if (pokemon1.PokedexSheet.EggGroup1 == pokemon2.PokedexSheet.EggGroup1)
            {
                return pokemon1.PokedexSheet.EggGroup1;
            }
            if (pokemon1.PokedexSheet.EggGroup2 == pokemon2.PokedexSheet.EggGroup1)
            {
                return pokemon1.PokedexSheet.EggGroup2;
            }
            if (pokemon1.PokedexSheet.EggGroup1 == pokemon2.PokedexSheet.EggGroup2)
            {
                return pokemon1.PokedexSheet.EggGroup1;
            }
            if (pokemon1.PokedexSheet.EggGroup2 == pokemon2.PokedexSheet.EggGroup2)
            {
                return pokemon1.PokedexSheet.EggGroup2;
            }

            return EggGroup.None;
        }
    }
}

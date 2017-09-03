using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anjril.PokemonWorld.Server.Model.Entity;
using Anjril.PokemonWorld.Server.Model.Persistence.Dto;
using System.Runtime.Serialization;
using System.IO;
using Anjril.PokemonWorld.Server.Model.Properties;
using Newtonsoft.Json;

namespace Anjril.PokemonWorld.Server.Model.Persistence
{
    public class PopulationDaoImpl : IPopulationDao
    {
        #region singleton

        private static PopulationDaoImpl instance;
        public static PopulationDaoImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PopulationDaoImpl();
                }

                return instance;
            }
        }

        #endregion


        public PopulationDto LoadPopulation()
        {
            var binder = new TypeNameSerializationBinder();

            var directory = new DirectoryInfo(Settings.Default.PopulationDataFolder);
            string fileContent = "";
            try
            {
                fileContent = File.ReadAllText(directory.FullName + "\\\\population.json", Encoding.UTF8);
            } catch (FileNotFoundException)
            {
                // pas de fichier de sauvegarde
                return null;
            }
            

            var populationDto = JsonConvert.DeserializeObject<PopulationDto>(fileContent, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });

            return populationDto;
        }

        public void SavePopulation(PopulationDto populationDto)
        {
            var binder = new TypeNameSerializationBinder();

            string fileContent = JsonConvert.SerializeObject(populationDto, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });
            
            var directory = new DirectoryInfo(Settings.Default.PopulationDataFolder);
            if (!directory.Exists)
            {
                directory.Create();
            }
            File.WriteAllText(directory.FullName + "\\\\population.json", fileContent);

        }



        private class TypeNameSerializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                return Type.GetType(typeName, true);
            }
        }

    }
}

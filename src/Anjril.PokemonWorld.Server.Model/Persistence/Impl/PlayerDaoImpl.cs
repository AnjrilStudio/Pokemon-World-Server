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
    public class PlayerDaoImpl : IPlayerDao
    {
        #region singleton

        private static PlayerDaoImpl instance;
        public static PlayerDaoImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerDaoImpl();
                }

                return instance;
            }
        }

        #endregion


        public PlayerDto LoadPlayer(string playerName)
        {
            var binder = new TypeNameSerializationBinder();

            var directory = new DirectoryInfo(Settings.Default.PlayerDataFolder);
            string fileContent = "";
            try
            {
                fileContent = File.ReadAllText(directory.FullName + "\\\\" + playerName + ".json", Encoding.UTF8);
            } catch (FileNotFoundException)
            {
                // le joueur n'existe pas
                return null;
            }
            

            var playerDto = JsonConvert.DeserializeObject<PlayerDto>(fileContent, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });

            return playerDto;
        }

        public void SavePlayer(PlayerDto playerDto)
        {
            var binder = new TypeNameSerializationBinder();

            string fileContent = JsonConvert.SerializeObject(playerDto, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            });
            
            var directory = new DirectoryInfo(Settings.Default.PlayerDataFolder);
            if (!directory.Exists)
            {
                directory.Create();
            }
            File.WriteAllText(directory.FullName + "\\\\" + playerDto.Name + ".json", fileContent);

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

using Anjril.PokemonWorld.Common.Properties;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Anjril.PokemonWorld.Common.Index
{
    public class Pokedex : IEnumerable<PokemonSheet>
    {
        #region private static fields

        private static Dictionary<int, PokemonSheet> POKEMONS;

        static Pokedex()
        {
            POKEMONS = new Dictionary<int, PokemonSheet>();

            var binder = new TypeNameSerializationBinder();

            var files = new DirectoryInfo(Settings.Default.PokedexFolder).GetFiles();
            var fileSContent = files.Select(sheet => File.ReadAllText(sheet.FullName, Encoding.UTF8));
            var pokemonSheets = fileSContent.Select(sheet => JsonConvert.DeserializeObject<PokemonSheet>(sheet, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Binder = binder
            })).ToList();

            foreach (var sheet in pokemonSheets)
            {
                POKEMONS.Add(sheet.NationalId, sheet);
            }
        }

        private class TypeNameSerializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                return Type.GetType(typeName, true);
            }
        }

        #endregion

        #region static methods

        public static PokemonSheet GetPokemonSheetByNationalId(int nationalId)
        {
            if (POKEMONS.ContainsKey(nationalId))
            {
                return POKEMONS[nationalId];
            }

            return null;
        }

        #endregion

        #region private fields

        private Dictionary<int, bool> _entries;

        #endregion

        #region constructors

        public Pokedex()
        {
            _entries = new Dictionary<int, bool>();
        }

        #endregion

        #region public methods

        public void NewSeen(int nationalId)
        {
            if (!IsAlreadySeen(nationalId))
            {
                _entries.Add(nationalId, false);
            }
        }

        public void NewCaught(int nationalId)
        {
            if (!IsAlreadyCaught(nationalId))
            {
                _entries[nationalId] = true;
            }
        }

        public bool IsAlreadySeen(int nationalId)
        {
            return _entries.ContainsKey(nationalId);
        }

        public bool IsAlreadyCaught(int nationalId)
        {
            if (IsAlreadySeen(nationalId))
            {
                return _entries[nationalId];
            }

            return false;
        }

        #endregion

        #region enumerator methods

        public IEnumerator<PokemonSheet> GetEnumerator()
        {
            return POKEMONS.Where(pair => _entries.Keys.Contains(pair.Key)).Select(pair => pair.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

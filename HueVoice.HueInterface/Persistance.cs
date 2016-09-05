using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueVoice.HueInterface.Interfaces;
using HueVoice.HueInterface.Lights;
using Newtonsoft.Json;

namespace HueVoice.HueInterface
{
    internal interface IPersistance
    {
        Task<IDictionary<string, HuePreference>> GetPreferencesAsync();
        Task<bool> SavePreferencesAsync(IDictionary<string, HuePreference> preferences);
    }


    internal class Persistance : IPersistance
    {
        private const string FileName = @"SavedLightCombinations.json";
        private readonly ITextSaver _textSaver;
        
        public Persistance(ITextSaver textSaver)
        {
            _textSaver = textSaver;
        }


        public async Task<IDictionary<string, HuePreference>> GetPreferencesAsync()
        {
            var fileText = await _textSaver.ReadFileAsync(FileName);
            var unconverted = JsonConvert.DeserializeObject<HuePreferenceCollection>(fileText);

            return unconverted?.Preferences?.ToDictionary(k => k.Name, v => v);
        }

        public async Task<bool> SavePreferencesAsync(IDictionary<string, HuePreference> preferences)
        {
            var obj = new HuePreferenceCollection
            {
                CollectionName = "SavedCollection",
                Preferences = preferences.Values
            };
            var serialized = JsonConvert.SerializeObject(obj);
            return await _textSaver.SaveFileAsync(FileName, serialized);
        }



    }
}

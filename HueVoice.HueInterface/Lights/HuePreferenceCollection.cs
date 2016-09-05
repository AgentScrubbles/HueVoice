using System.Collections.Generic;

namespace HueVoice.HueInterface.Lights
{
    public class HuePreferenceCollection
    {
        public string CollectionName { get; set; }
        public ICollection<HuePreference> Preferences { get; set; } 
    }
}

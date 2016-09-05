using System.Collections.Generic;

namespace HueVoice.HueInterface.Lights
{
    public class HuePreference
    {
        public string Name { get; set; }
        public ICollection<HueLight> Lights { get; set; }  
    }
}

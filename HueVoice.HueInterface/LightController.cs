using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueVoice.HueInterface
{
    public class LightController
    {
        private static LightController _instance;

        private LightController()
        {

        }

        public static LightController Instance { get { return _instance ?? (_instance = new LightController()); } }



    }
}

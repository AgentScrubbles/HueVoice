using System;

namespace HueVoice.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class VoiceCommandAttribute : Attribute
    {
        public string CommandName { get; private set; }
        public string[] Parameters { get; private set; }

        public VoiceCommandAttribute(string commandName, string[] parameters = null)
        {
            CommandName = commandName;
            Parameters = parameters;
        }
    }

}

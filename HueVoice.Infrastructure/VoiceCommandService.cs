using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HueVoice.Infrastructure
{
    public class VoiceCommandService
    {
        private readonly IDictionary<string, VoiceCommandContainer> _commands;

        public VoiceCommandService(Type[] registeredTypes)
        {
            _commands = BuildCommands(registeredTypes);
        }

        private IDictionary<string, VoiceCommandContainer> BuildCommands(Type[] types)
        {
            var dict = types.Select(k => new
            {
                ValidMethods =
                    k.GetMethods().Where(j => j.GetCustomAttributes(typeof(VoiceCommandAttribute), true).Any())
            }).Select(k => new
            {
                ValidMethods = k.ValidMethods.Select(j => new
                {
                    VoiceCommands =
                        j.GetCustomAttributes(typeof(VoiceCommandAttribute), true)
                            .Cast<VoiceCommandAttribute>()
                            .ToList(),
                    Method = j
                })
            }).SelectMany(k => k.ValidMethods)
                .Select(k => new
                {
                    Commands = k.VoiceCommands.Select(j => new VoiceCommandContainer
                    {
                        CommandName = j.CommandName,
                        Attribute = j,
                        Method = k.Method
                    })
                }).SelectMany(k => k.Commands)
                .GroupBy(k => k.CommandName)
                .ToDictionary(k => k.Key, v => v.First());

            //var nonStaticTypes = dict.Values.Where(k => !k.Method.IsStatic);
            //if(nonStaticTypes.Any())
            //{
            //    throw new Exception("All uses of VoiceCommandAttribute must be static!");
            //}

            return dict;
        } 


        public void Call(string commandName, object instance = null, IDictionary<string, object> parameters = null)
        {
            if (!_commands.ContainsKey(commandName))
            {
                throw new Exception($"Command {commandName} not found");
            }
            var command = _commands[commandName];
            var hasParams = command.Attribute.Parameters != null && command.Attribute.Parameters.Any() && parameters != null;

            object[] combinedParameters = null;

            if (hasParams)
            {
                combinedParameters = command.Attribute.Parameters.ToArray().Select(k => new
                {
                    Name = k,
                    HasKey = parameters.ContainsKey(k)
                }).Where(k => k.HasKey)
                    .Select(k => new
                    {
                        k.Name,
                        Value = parameters[k.Name]
                    }).Select(k => k.Value).ToArray();
            }


            command.Method.Invoke(instance, combinedParameters);
        }

        private class VoiceCommandContainer
        {
            public MethodInfo Method { get; set; }
            public VoiceCommandAttribute Attribute { get; set; }
            public string CommandName { get; set; }
        }
    }
}

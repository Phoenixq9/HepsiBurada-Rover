using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MarsRover
{
    public class CommandCenter 
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandCenter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public void SendCommand(string command)
        {
            var commandExecuters = GetCommandExecutersInAssembly();

            var executer = commandExecuters.FirstOrDefault(c => c.MatchCommand(command));
            executer?.ExecuteCommand(command);
        }

        private List<CommandExecuter> GetCommandExecutersInAssembly()
        {
            return Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(type => type.IsSubclassOf(typeof(CommandExecuter)) && !type.IsAbstract)
                .Select(x => Activator.CreateInstance(x, _serviceProvider) as CommandExecuter)
                .ToList();
        }
    }
}
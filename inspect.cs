using System;
using System.Reflection;
using NetCord.Services.ApplicationCommands;

class Program {
    static void Main() {
        foreach (var c in typeof(ApplicationCommandServiceManager).GetConstructors()) {
            Console.WriteLine("Ctor: " + string.Join(", ", Array.ConvertAll(c.GetParameters(), p => p.ParameterType.Name)));
        }
        foreach (var m in typeof(ApplicationCommandServiceManager).GetMethods()) {
            if (m.Name.Contains("CommandsAsync") || m.Name.Contains("Register")) {
                Console.WriteLine("Method: " + m.Name + " - " + string.Join(", ", Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name)));
            }
        }
    }
}

using System;
using System.Reflection;
using NetCord.Services.ApplicationCommands;
using System.Linq;

class Program {
    static void Main() {
        var types = typeof(SlashCommandAttribute).Assembly.GetTypes()
            .Where(t => t.Name.Contains("CommandAttribute"))
            .Select(t => t.Name);
        Console.WriteLine("Types: " + string.Join(", ", types));
    }
}

using System;
using System.Reflection;
using NetCord.Services.ApplicationCommands;

class Program {
    static void Main() {
        foreach (var p in typeof(SlashCommandAttribute).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
            Console.WriteLine("Prop: " + p.Name + " (" + p.PropertyType.Name + ")");
        }
    }
}

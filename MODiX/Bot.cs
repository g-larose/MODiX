using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Guilded;
using Guilded.Client;
using Guilded.Commands;
using MODiX.Commands.Commands;
using MODiX.Config;

namespace MODiX
{
    public class Bot
    {
        private static string? json   = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json"));
        private static string? token  = JsonSerializer.Deserialize<ConfigJson>(json!)!.Token!;
        private static string? prefix = JsonSerializer.Deserialize<ConfigJson>(json!).Prefix!;
       
        public async Task RunAsync()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            await using var client = new GuildedBotClient(token!)
                .AddCommands(new ModCommands(), prefix!);

            client.Prepared
                .Subscribe(async me =>
                {
                    var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                    var date = DateTime.Now.ToShortDateString();
                    Console.WriteLine($"[{date}] [{time}] [{me.ParentClient.Name}] talking to gateway...");
                });


            await client.ConnectAsync();
            await client.SetStatusAsync("Watching Everything", 90002579);
            var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
            var date = DateTime.Now.ToShortDateString();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"[{date}] [{time}] [MODiX] connected...");
            Console.WriteLine($"[{date}] [{time}] [MODiX] registering command modules...");
            await Task.Delay(200);
            Console.WriteLine($"[{date}] [{time}] [MODiX] listening for events");
            await Task.Delay(-1);
        }
    }
}

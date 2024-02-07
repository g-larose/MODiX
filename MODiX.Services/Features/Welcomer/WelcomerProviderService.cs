using MODiX.Data.Models;
using MODiX.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MODiX.Services.Features.Welcomer
{
    public class WelcomerProviderService : IWelcomerProvider
    {
        public async Task<WelcomeMessage> GetRandomWelcomeMessageAsync()
        {
            var jFile = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Features", "Welcomer", "welcomer_messages.json"));
            var json = JsonSerializer.Deserialize<WelcomerRoot>(jFile);
            var rnd = new Random();
            var index = rnd.Next(1, json.Messages.Count);

            var message = json.Messages[index].Message;
            var emoji = json.Messages[index].Emoji;

            var welcomeMessage = new WelcomeMessage();
            welcomeMessage.Emoji = emoji;
            welcomeMessage.Message = message;

            return welcomeMessage;

        }
    }
}

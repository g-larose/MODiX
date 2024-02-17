using Guilded.Base.Embeds;
using Guilded.Commands;
using MODiX.Services.Features.Blackjack;
using MODiX.Services.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Commands.Commands
{
    public class BlackjackCommands : CommandModule
    {
        [Command(Aliases = new string[] { "blackjack" })]
        public async Task Blackjack(CommandEvent invokator)
        {
            var blackJack = new BlackjackSystem();
            var userCard = blackJack.UserSelectedCard;
            var botCard = blackJack.BotSelectedCard;

            var embed = new Embed()
            .AddField(new EmbedField("User Card", userCard!, true))
            .AddField(new EmbedField("Bot Card", botCard!, true));

            embed.Color = EmbedColorService.GetColor("darkgray", Color.DarkGray);
            embed.Description = "react to hit or stand";
            var message = await invokator.ReplyAsync(embed);
            var reactions = new uint[] { 90001164, 90002171 };

            foreach (var r in reactions)
            {
                 await message.AddReactionAsync(r);
            }
                      
            var reaction = message.ReactionAdded;

        }
    }
}

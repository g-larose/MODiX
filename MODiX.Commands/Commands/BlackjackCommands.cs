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
            var userCard1 = blackJack.UserSelectedCards![0];
            var botCard1 = blackJack.BotSelectedCards![0];

            var userCard2 = blackJack.UserSelectedCards![1];
            var botCard2 = blackJack.BotSelectedCards![1];

            var userCard3 = blackJack.UserSelectedCards![2];
            var botCard3 = blackJack.BotSelectedCards![2];

            var userCard4 = blackJack.UserSelectedCards![3];
            var botCard4 = blackJack.BotSelectedCards![3];

            var userCard5 = blackJack.UserSelectedCards![4];
            var botCard5 = blackJack.BotSelectedCards![4];

            var embed = new Embed();
            embed.SetTitle("User draws");
            embed.SetDescription($"{userCard1.Card}\nreact with :point_down:  to hit or :white_check_mark: to stand");

            embed.Color = EmbedColorService.GetColor("darkgray", Color.DarkGray);
            var message = await invokator.ReplyAsync(embed);
            var reactions = new uint[] { 90001110, 90002171 };

            foreach (var r in reactions)
            {
                 await message.AddReactionAsync(r);
            }

            invokator.ParentClient.MessageReactionAdded
                     .Where(e => e.CreatedBy == invokator.CreatedBy)
                     .Subscribe(async reaction =>
                     {

                         if (reaction.Name == "point_down")
                         {
                             embed.SetDescription($"Player Cards {userCard1.Card} {userCard2.Card} total [{userCard1.Value + userCard2.Value}]");
                             await message.UpdateAsync(embed);
                         }
                         else
                         {
                             embed.SetDescription($"Player Cards {userCard1.Card} {userCard2.Card} total [{userCard1.Value + userCard2.Value}]\n" +
                                 $"Dealer Cards {botCard1.Card} {botCard2.Card} total [{botCard1.Value + botCard2.Value}]");
                             await message.UpdateAsync(embed);
                         }
                     });

        }
    }
}

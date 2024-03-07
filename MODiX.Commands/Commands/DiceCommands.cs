using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using MODiX.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Commands.Commands
{
    public class DiceCommands: CommandModule
    {
        [Command(Aliases = [ "dice", "roll" ])]
        [Description("roll's a set of dice | min is 1 | max is 6")]
        public async Task RollDice(CommandEvent invokator, int die = 6, int sides = 6)
        {
            var diceRoller = new DiceRollerProviderService();
            var embed = new Embed();
            var authorId = invokator.Message.CreatedBy;
            var serverId = invokator.ServerId;
            var author = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, authorId);
            var rollResult = diceRoller.Roll(author, die, sides);

            if (!rollResult.IsValid)
            {
                embed.SetTitle($"<@{authorId}> Dice Roller return an invalid roll, please try again");
                embed.AddField("Id", rollResult.Id, true);
                embed.AddField("Rolled At", rollResult.RolledAt!, true);
                embed.SetFooter($"MODiX watching everyone ");
                embed.SetTimestamp(DateTime.Now);
                await invokator.ReplyAsync(embed);
            }
            else
            {
                var rollDie = string.Join(",", rollResult.Die);
                embed.SetTitle($"Dice Roller");
                embed.SetDescription($"<@{author.Id}> rolled [{rollDie}] {rollResult.Die.Count} , {rollResult.Sides} sided dice\r\nfor a total of [{rollResult.Die.Sum()}]");
                embed.AddField("Id", rollResult.Id, true);
                embed.AddField("Rolled At", rollResult.RolledAt!, true);
                embed.SetFooter($"MODiX watching everyone ");
                embed.SetTimestamp(DateTime.Now);
                await invokator.ReplyAsync(embed);
            }

        }
    }
}

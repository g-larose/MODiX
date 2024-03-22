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

            if (!rollResult.IsOk)
            {
                embed.SetTitle($"<@{rollResult.Value.Member!.Id}> Dice Roller max die amount is 12 and max sides is 12, you tried to exceed that amount\r\nERROR!");
                embed.AddField("Id", rollResult.Value.Id, true);
                embed.AddField("Rolled At", rollResult.Value.RolledAt!, true);
                embed.SetFooter($"MODiX watching everyone ");
                embed.SetTimestamp(DateTime.Now);
                await invokator.ReplyAsync(embed);
            }
            else
            {
                var rollDie = string.Join(",", rollResult.Value.Die);
                embed.SetTitle($"Dice Roller");
                embed.SetThumbnail(new EmbedMedia("https://cdn.gilcdn.com/MediaChannelUpload/e615752d38a881e44543348ae8bda8c2-Full.webp?w=1500&h=1500"));
                embed.SetDescription($"<@{author.Id}> rolled [{rollDie}] {rollResult.Value.Die.Count} , {rollResult.Value.Sides} sided dice\r\nfor a total of [{rollResult.Value.Die.Sum()}]");
                embed.AddField("Id", rollResult.Value.Id, true);
                embed.AddField("Rolled At", rollResult.Value.RolledAt!, true);
                embed.SetFooter($"MODiX watching everyone ");
                embed.SetTimestamp(DateTime.Now);
                await invokator.ReplyAsync(embed);
            }

        }
    }
}

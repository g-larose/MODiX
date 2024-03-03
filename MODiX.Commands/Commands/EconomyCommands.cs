using Guilded.Base.Embeds;
using Guilded.Commands;
using Humanizer;
using MODiX.Data.Models;
using MODiX.Services.Features.Economy;
using MODiX.Services.Interfaces;
using MODiX.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Commands.Commands
{
    public class EconomyCommands : CommandModule
    {
        private Wallet wallet { get; set; } = new();
        private BotTimerService timer = new();
        [Command(Aliases = [ "daily" ] )]
        [Description("get member's daily economy points")]
        public async Task Daily(CommandEvent invokator)
        {
            var interval = DateTime.Parse(BotTimerService.GetStartTime()) - DateTime.Now;

            using var economy = new EconomyProviderService();
            var points = economy.GetDaily();
            wallet.Points += points;
            await invokator.ReplyAsync($"you finished daily task and received ⭐{points}⭐ points => {interval}");

            
        }

        [Command(Aliases = [ "balance" ])]
        [Description("get the member's bank balance")]
        public async Task Balance(CommandEvent invokator, string member = "")
        {
            if (wallet is null)
                await invokator.ReplyAsync($"user has 0 points in wallet!");
            else
                await invokator.ReplyAsync($"user has {wallet!.Points} points in wallet!");
        }

        [Command(Aliases = [ "deposit" ])]
        [Description("deposit wallet points into the bank account")]
        public async Task Deposit(CommandEvent invokator, string args = "")
        {
            if (args is null || args == "")
            {
                var embed = new Embed();
                embed.SetTitle($"﻿:x: invalid command argument :x: ");
                embed.SetDescription("please specify a command argument!");
                embed.SetFooter($"{invokator.ParentClient.Name} watching everything ");
                embed.SetTimestamp(DateTime.Now);
                await invokator.ReplyAsync(embed);
            }
            else
            {
                switch (args)
                {
                    case "all":
                        await invokator.ReplyAsync($"you set args as [**all**]");
                        break;
                    default:
                        await invokator.ReplyAsync("I didn't recognise the command argument, command ignored!");
                        break;
                }
            }
        }
    }
}

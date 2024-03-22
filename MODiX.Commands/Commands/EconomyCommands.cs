﻿using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using MODiX.Data;
using MODiX.Data.Factories;
using MODiX.Data.Models;
using MODiX.Services.Features.Economy;
using MODiX.Services.Interfaces;
using MODiX.Services.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Commands.Commands
{
    public class EconomyCommands : CommandModule
    {
        private BotTimerService timer = new();
        private ModixDbContextFactory _dbFactory = new();
        private EconomyGameService gameService = new();

        #region DAILY
        [Command(Aliases = [ "daily" ] )]
        [Description("get member's daily economy points")]
        public async Task Daily(CommandEvent invokator)
        {
            var interval = DateTime.Parse(BotTimerService.GetStartTime()) - DateTime.Now;

            using var economy = new EconomyGameService();
            var points = economy.GetDaily();
            await invokator.ReplyAsync($"you finished daily task and received ⭐{points}⭐ points => {interval}");

            
        }
        #endregion

        #region WORK
        [Command(Aliases = [ "work" ])]
        [Description("get's members work points")]
        public async Task Work(CommandEvent invokator)
        {

        }

        #endregion

        #region CHORES
        [Command(Aliases = [ "chores" ])]
        [Description("get the chores value")]
        public async Task Chores(CommandEvent invokator)
        {
            using EconomyGameService gameService = new();
            var chores = new string[] { "washing the dishes", "sweeping", "taking the garbage out", "cleaning your room", "feeding the animals", "mowing the grass" };
            var result = gameService.GetChores();
            if (result.IsOk)
            {
                try
                {
                    var rnd = new Random();
                    var index = rnd.Next(0, chores.Length - 1);
                    var chore = chores[index];
                    var serverId = invokator.Message.ServerId;
                    var memberId = invokator.Message.CreatedBy;
                    var member = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, memberId);
                    await invokator.ReplyAsync($"you finished {chore} and received 💰{result.Value}💰 gold coins");

                    using var db = _dbFactory.CreateDbContext();
                    var localMember = db.ServerMembers?.Where(x => x.UserId!.Equals(member.Id.ToString()))
                        .Include(w => w.Wallet)
                        .FirstOrDefault();

                    localMember!.Wallet!.Points += result.Value;
                    db.Update(localMember);
                    await db.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    Console.WriteLine("I encountered an error while fetching the chores, command reset!");
                    var error = new SystemError()
                    {
                        ErrorCode = Guid.NewGuid(),
                        ErrorMessage = $"{e.Message}"
                    };
                    using var db = _dbFactory.CreateDbContext();
                    db.Add(error);
                    await db.SaveChangesAsync();
                }
                
            }
            else
            {
                await invokator.ReplyAsync($"{result.Error.ErrorMessage}\r\nplease refer this code to a developer: {result.Error.ErrorCode}");
                var error = new SystemError()
                {
                    ErrorCode = Guid.NewGuid(),
                    ErrorMessage = $"{result.Error.ErrorMessage}"
                };
                using var db = _dbFactory.CreateDbContext();
                db.Add(error);
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region BALANCE
        [Command(Aliases = [ "balance", "bal" ])]
        [Description("get the member's bank balance")]
        public async Task Balance(CommandEvent invokator, string member = "")
        {
            var serverId = invokator.Message.ServerId;
            var memberId = invokator.Message.CreatedBy;
            var _member = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, memberId);
            //using var db = _dbFactory.CreateDbContext();
            //var localMember = db.ServerMembers?.Where(x => x.UserId!.Equals(_member.Id.ToString()))
            //    .Include(a => a.Wallet)
            //    .Include(b => b.Bank)
            //    .FirstOrDefault();
            var walletBal = gameService.GetMemberWalletBalance(memberId.ToString());
            var bankBal = gameService.GetMemberBankBalance(memberId.ToString());
            var embed = new Embed();
            embed.SetTitle($"<@{memberId}>'s Balance");
            embed.SetThumbnail(new EmbedMedia($"{_member?.Avatar?.AbsoluteUri}"));
            embed.SetDescription($"member has {walletBal.Value} gold in their wallet.\r\nmember has {bankBal.Value} gold in the bank");
            embed.SetFooter("MODiX watching everything ");
            embed.SetTimestamp(DateTime.Now);
            await invokator.ReplyAsync(embed);
            
           
        }
        #endregion

        #region DEPOSIT
        [Command(Aliases = [ "deposit" ])]
        [Description("deposit wallet points into the bank account")]
        public async Task Deposit(CommandEvent invokator, string args = "")
        {
            if (args is null || args == "")
            {
                var memberId = invokator.Message.CreatedBy;
                var serverId = invokator.ServerId;
                var member = await invokator.ParentClient.GetMemberAsync((HashId)serverId!, memberId);

                using var db = _dbFactory.CreateDbContext();
                var localMember = db.ServerMembers?.Where(x => x.UserId!.Equals(member.Id.ToString()))
                    .Include(w => w.Wallet)
                    .Include(b => b.Bank)
                    .FirstOrDefault();
                if (localMember is null)
                {
                    await invokator.ReplyAsync("unable to deposit gold, try again later");
                }
                else
                {
                    var embed = new Embed();
                    var walletPoints = localMember.Wallet.Points;
                    var bankTotal = localMember.Bank.AccountTotal + walletPoints;
                    localMember.Bank.AccountTotal += walletPoints;
                    localMember.Wallet.Points = 0;
                    db.Update(localMember);
                    await db.SaveChangesAsync();
                    
                    embed.SetTitle($"﻿<@{memberId}> deposit successful");
                    embed.SetDescription($"{localMember.Wallet.Points} was successfully deposited into your bank account.");
                    embed.SetFooter($"{invokator.ParentClient.Name} watching everything ");
                    embed.SetTimestamp(DateTime.Now);
                    await invokator.ReplyAsync(embed);
                }
                
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
        #endregion
    }
}

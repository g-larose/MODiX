﻿using Guilded.Base;
using Guilded.Base.Embeds;
using Guilded.Commands;

namespace MODiX.Commands.Commands
{
    public class ModCommands : CommandModule
    {
        [Command(Aliases= new string[] { "warn", "w" })]
        [Description("warns another server member with a reason for the warning.")]
        public async Task Warn(CommandEvent invokator, string reason)
        {
            
        }

        [Command(Aliases = new string[] { "mute", "m" })]
        [Description("mutes a server member with a reason why they were muted.")]
        public async Task Mute(CommandEvent invokator, string reason)
        {

        }

        [Command(Aliases = new string[] { "kick", "k" })]
        [Description("kicks another server member from the server with a reason for the kick.")]
        public async Task Kick(CommandEvent invokator, string reason)
        {

        }

        [Command(Aliases = new string[] { "ban", "b" })]
        [Description("ban another server member from the server with a reason for the ban.")]
        public async Task BAn(CommandEvent invokator, string reason)
        {

        }

        [Command(Aliases = new string[] { "promote", "pm" })]
        [Description("promotes another server member")]
        public async Task Promote(CommandEvent invokator, string roleId)
        {

        }

        [Command(Aliases = new string[] { "demote", "dm" })]
        [Description("demotes another server member")]
        public async Task Demote(CommandEvent invokator, string roleId)
        {

        }
    }
}

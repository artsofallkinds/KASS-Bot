using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace KASS_Bot.Core.Commands
{
    public class SimpleCommands : ModuleBase<SocketCommandContext>
    {
        // Responds when someone greets the bot
        [Command("hello"), Summary("Hello World command")]
        public async Task HelloWorld()
        {
            await Context.Channel.SendMessageAsync("Hello, " + Context.User.Username + "!");
        }

        // Test for the Embed Builder
        [Command("embed"), Summary("Embed test command")]
        public async Task Embed([Remainder]string Input = "None")
        {
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor("Test Embed", Context.User.GetAvatarUrl());
            Embed.WithColor(255,255,0);
            Embed.WithFooter("The footer", Context.Guild.Owner.GetAvatarUrl());
            Embed.WithDescription("This is a **dummy** description. Have a link: \n[__KASS OwlLife Page__](https://owllife.kennesaw.edu/organization/kass)");
            Embed.AddField("User input:", Input, true);


            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }

        /* Template for making new commands: 
        [Command(""), Alias(""), Summary("")]
        public async Task Template()
        {
            // Code goes here
        }
        */
    }
}

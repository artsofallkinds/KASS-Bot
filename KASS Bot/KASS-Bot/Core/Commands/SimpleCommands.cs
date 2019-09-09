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
        [Command("help"), Summary("Basic Help command.")]
        public async Task HelpCommand()
        {
            await Context.Channel.SendMessageAsync("Hello, " + Context.User.Username + "! I am Kass and I am here to help however I can!\n" +
                                                   "Commands:\n" +
                                                   "\"Squack\" - I react to a message with a bird when I see my name in it. Can also be triggered manually with `k!squack`.\n" +
                                                   "\"Hello\" - I respond to a message that has only \"Hello\" in it by greeting that user. Can also be triggered manually with `k!hello`.");
        }

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

        // Reacts to a message by adding a bird emoji
        [Command("squack"), Summary("Mari-Bot detects when its' name is spoken and does something silly.")]
        public async Task Squack()
        {
            var birdemoji = new Emoji("\uD83D\uDC26");                  // Can be replaced with a custom server emoji
            await Context.Message.AddReactionAsync(birdemoji);          // Reacts to the identified message with an emoji reaction
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

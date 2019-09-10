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
            await Context.Channel.SendMessageAsync("Hello, " + Context.User.Username + "! I am KASSBot and I am here to help however I can!\n" +
                                                   "```ml\n" +
                                                   "Automatic Commands:\n" +
                                                   "\'Squack\' = - i React to a message if i see my Name. can also be triggered manually using \"k!squack\".\n" +
                                                   "\'Hello\' - i Respond to a message that has only Hello by greeting that user. can also be triggered manually using \"k!hello\".\n" +
                                                   "\nManual Commands:\n" +
                                                   "\"k!kennesaw\" - i give you the Kennesaw role, stating that Kennesaw Campus is your main campus.\n" +
                                                   "\"k!marrietta\" - i give you the Marietta role, stating that Marietta Campus is your main campus." +
                                                   "```");
        }

        // Responds when someone greets the bot
        [Command("hello"), Summary("KASSBot detects when just the word hello is spoken, and responds to the user who sent it.")]
        public async Task HelloWorld()
        {
            await Context.Channel.SendMessageAsync("Hello, " + Context.User.Username + "!");

            Console.WriteLine($"{DateTime.Now} at Commands] Hello given to {Context.User.Username}.");
        }

        // Reacts to a message by adding a bird emoji
        [Command("squack"), Summary("KASSBot detects when its' name is spoken and reacts to the message with a bird emoji.")]
        public async Task Squack()
        {
            var birdemoji = new Emoji("\uD83D\uDC26");                  // Can be replaced with a custom server emoji
            await Context.Message.AddReactionAsync(birdemoji);          // Reacts to the identified message with an emoji reaction

            Console.WriteLine($"{DateTime.Now} at Commands] Squack!");
        }

        // Gives someone the Kennesaw role
        [Command("kennesaw"), Alias("giveKennesaw"), Summary("Gives the user the Kennesaw role")]
        public async Task GiveKennesaw()
        {
            ulong roleid = 613097833507323915;

            IUser user = Context.User;
            IRole role = Context.Guild.GetRole(roleid);

            IGuildUser guildUser = user as IGuildUser;
            List<ulong> userRoles = new List<ulong>(guildUser.RoleIds);
            bool hasRole = false;

            foreach (ulong x in userRoles)
            {
                if (x == roleid)
                {
                    hasRole = true;
                }
            }

            if (!hasRole)
            {
                await guildUser.AddRoleAsync(role);

                var check = new Emoji("\u2705");                        
                await Context.Message.AddReactionAsync(check);

                Console.WriteLine($"{DateTime.Now} at Commands] {role.Name} given to {user.Username}.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("" + user.Username + ", you already have that role!");

                Console.WriteLine($"{DateTime.Now} at Commands] {user.Username} already has {role.Name}.");
            }
        }

        // Gives someone the Marietta role
        [Command("marietta"), Alias("giveMarietta"), Summary("Gives the user the Marietta role")]
        public async Task GiveMarietta()
        {
            ulong roleid = 613097920777945091;

            IUser user = Context.User;
            IRole role = Context.Guild.GetRole(roleid);

            IGuildUser guildUser = user as IGuildUser;
            List<ulong> userRoles = new List<ulong>(guildUser.RoleIds);
            bool hasRole = false;

            foreach (ulong x in userRoles)
            {
                if (x == roleid)
                {
                    hasRole = true;
                }
            }

            if (!hasRole)
            {
                await guildUser.AddRoleAsync(role);

                var check = new Emoji("\u2705");
                await Context.Message.AddReactionAsync(check);

                Console.WriteLine($"{DateTime.Now} at Commands] {role.Name} given to {user.Username}.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("" + user.Username + ", you already have that role!");

                Console.WriteLine($"{DateTime.Now} at Commands] {user.Username} already has {role.Name}.");
            }
        }

        // Test for the Embed Builder
        /*
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

        */
        /* Template for making new commands: 
        [Command(""), Alias(""), Summary("")]
        public async Task Template()
        {
            // Code goes here
        }
        */

    }
}

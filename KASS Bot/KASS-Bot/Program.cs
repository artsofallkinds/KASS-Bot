using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace KASS_Bot
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            // The core Discord connection object
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug   // Change this before going live
            });

            // Commands - Different key phrases used to tell the bot what to do
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug   // Change this before going live
            });

            Client.MessageReceived += Client_MessageRecieved;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);     // Very Necessary for commands to work

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            // Using the Token to log in
            string token = "";
            using (var Stream = new FileStream(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.2", @"Data\Token.txt"), FileMode.Open, FileAccess.Read))
            using (var ReadToken = new StreamReader(Stream))
            {
                token = ReadToken.ReadToEnd();
            }
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
        
        // Logging method - What is logged is controlled by LogLevel 
        private async Task Client_Log(LogMessage Message)
        {
            Console.WriteLine($"{DateTime.Now} at {Message.Source}] {Message.Message}");
        }

        // Sets the Bot's "Playing" message
        private async Task Client_Ready()
        {
            await Client.SetGameAsync("🛠 Under Construction 🛠", "", ActivityType.Playing);
        }

        // Runs when the bot detects a new message sent in the server
        private async Task Client_MessageRecieved(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);
            var birdemoji = new Emoji("\uD83D\uDC26");

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;

            // Checks to see if the message has prefix triggers
            if (Message.HasStringPrefix("k!", ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))
            {
                var Result = await Commands.ExecuteAsync(Context, ArgPos, null);
                if (!Result.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text: {Context.Message.Content} | Error: {Result.ErrorReason}");
                }
            }
            /*  Attempting to make the bot respond to all messages it's mentioned in that are not commands by adding the bird emoji reaction to said messages.
            else if (Message.MentionedUsers.Equals(Client.CurrentUser))
            {
                Console.WriteLine($"{DateTime.Now} at Commands] Non-command mention noticed. Text: {Context.Message.Content}");
                await Message.AddReactionAsync(birdemoji);
            }
            */
            else return;
        }
    }
}

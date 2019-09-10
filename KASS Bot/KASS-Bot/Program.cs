using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            Client.UserJoined += Client_UserJoined;
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
        
// =====================================================================================================================================================

        // Runs when the bot detects a new user joining a server
        private async Task Client_UserJoined(SocketGuildUser User)
        {
            Console.WriteLine($"{DateTime.Now} at UserJoined] {User.Username} joined the server.");

            try
            {
                IDMChannel newDM = await User.GetOrCreateDMChannelAsync();
                await newDM.SendMessageAsync("Welcome to the KASS Server! I'm KASSBot, and I can help you get accepted as a member of the Kennesaw Anthropomorphic Student Society. " +
                                             "If you ever need to know what I can do, say `k!help` wherever you can see me, such as here or in the KASS server!\n\n" +
                                             "At the moment, your view of the KASS Discord Server is probably fairly limited, perhaps only to a handfull of channels. "+
                                             "In order to be accepted into KASS, you need to join us on OwlLife! It's Kennesaw's official online catalogue for student organizations.\n" +
                                             "https://owllife.kennesaw.edu/organization/kass \n\n" +
                                             "Alternatively, you can meet with one of our admins in real life to be verified. Our Admins are Alpheron#1950 (President), Ramble#5637 (Vice President), " +
                                             "and Vaktavious#6070 (Treasurer).\n\n" +
                                             "Once you've done either verification method, message an Admin in a DM to verify your identity and receive access to the rest of the Discord server. " +
                                             "We're glad to have you, and we hope you enjoy your stay!");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine($"{DateTime.Now} at UserJoined] Null Reference Exception occured.");

            }
        }

        // Runs when the bot detects a new message
        private async Task Client_MessageRecieved(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);
            var birdemoji = new Emoji("\uD83D\uDC26");

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;
            
            // Checks to see if the message has prefix triggers, runs the correct command
            if (Message.HasStringPrefix("k!", ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))
            {
                var Result = await Commands.ExecuteAsync(Context, ArgPos, null);
                if (!Result.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text: {Context.Message.Content} | Error: {Result.ErrorReason}");
                }
            }

            // Checks to see if the message contains the bot's name, if so runs the Squack command
            else if (FormatString(Message.ToString()).Contains("kassbot"))
            {
                Console.WriteLine($"{DateTime.Now} ] Name detected! | From: {Context.User} : {Context.Message.Content} ");      // Debugging purposes, remove for live
                var Result = await Commands.ExecuteAsync(Context, "squack", null);
                if (!Result.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text: {Context.Message.Content} | Error: {Result.ErrorReason}");
                }
            }

            // Checks to see if the message is only "hello", if so runs the Hello command
            else if (FormatString(Message.ToString()).Equals("hello"))
            {
                Console.WriteLine($"{DateTime.Now} ] Hello detected! | From: {Context.User} : {Context.Message.Content} ");      // Debugging purposes, remove for live
                var Result = await Commands.ExecuteAsync(Context, "hello", null);
                if (!Result.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text: {Context.Message.Content} | Error: {Result.ErrorReason}");
                }
            }

            else return;
        }

        public string FormatString(string x)
        {
            return x.ToLower().Trim().Replace(".", "").Replace("!", "").Replace("?", "");
        }
    }
}

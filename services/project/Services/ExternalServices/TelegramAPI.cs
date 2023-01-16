using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Models.Configs;
using Serilog;

namespace Services.ExternalServices
{
    public class TelegramAPI
    {
        private static TelegramConfig _telegramConfig;
        
        public static void Configure(TelegramConfig telegramConfig)
        {
            _telegramConfig = telegramConfig;
        }

        private static ConcurrentQueue<string> _messages = new();
        private static Timer _timer = new(async _ => { await TryFlushQueue();}, null, 5000, 10000);

        public static async Task Send(string message)
        {
            if (message.Length > 200)
            {
                _messages.Enqueue(message);
                await ForceFlushQueue();
            }
            else
            {
                _messages.Enqueue(message);
            }
        }

        private static int _emptyLogsCount = 0;

        private static async Task TryFlushQueue()
        {
            if (!_messages.IsEmpty)
            {
                _emptyLogsCount = 0;
                await ForceFlushQueue();
            }
            else
            {
                _emptyLogsCount++;
                if (_emptyLogsCount == 10)
                {
                    _emptyLogsCount = 0;
                    const string content = "Log Queue Flush Empty 10 times";
                    await DoSend(content);
                }
            }
        }

        private static async Task ForceFlushQueue()
        {
            var content = "Log Queue Flush\n" + string.Join("\n", _messages);
            _messages.Clear();
            await DoSend(content);
        }

        private static async Task DoSend(string content)
        {
            using var client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            try
            {
                var parameters = new 
                {
                    chat_id = $"-{_telegramConfig.DevelopmentChatId}",
                    parse_mode = "Markdown",
                    text = content.Length <= 4096 ? content : content[..4096]
                };

                var response = await client.PostAsync($"https://api.telegram.org/bot{_telegramConfig.Token}/sendMessage", JsonContent.Create(parameters));

                var respMessage = await response.Content.ReadAsStringAsync();
                
                Log.Logger.ForContext<TelegramAPI>()
                    .Information("received telegram response: {response}", respMessage);
            }
            catch (Exception ex)
            {
                Log.Logger.ForContext<TelegramAPI>()
                    .Information(ex, "failed to send to telegram");
            }
        }
    }
}
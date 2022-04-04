using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using DiscordStats.ViewModel;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace DiscordStats.DAL.Concrete
{
    public class DiscordServicesForChannels : IDiscordServicesForChannels
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IChannelRepository _channelRepository;

        public DiscordServicesForChannels(IHttpClientFactory httpClientFactory, IChannelRepository channelRepository)
        {
            _httpClientFactory = httpClientFactory;
            _channelRepository = channelRepository;
        }


        public async Task<string> GetWebHooksInfoInFormOfJsonStringFromEndpoint(string botToken, string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                        { HeaderNames.Authorization, "Bot " + botToken},
                        { HeaderNames.UserAgent, "DiscordStat" }
                    }
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            // Note this is the blocking version.  Would be better to use the Async version
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            // This is only a minimal version; make sure to cover all your bases here
            if (response.IsSuccessStatusCode)
            {
                // same here, this is blocking; use ReadAsStreamAsync instead
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                // What to do if failure? Should throw specific exceptions that explain what happened
                throw new HttpRequestException();
            }
        }

        public async Task<string> PostToDiscordCreateWebhook(string botToken, string uri, string webhookName)
        {
            var bodyAsJSON = $"{{\"name\": \"{webhookName}\"}}";
            HttpContent body = new StringContent(bodyAsJSON);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken},
                    { HeaderNames.UserAgent, "DiscordStat" }
                },
                Content = body
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                throw new HttpRequestException();
            }
        }

        public async Task<string> PostToDiscordSendMessageThroughWebhook(string botToken, string uri, string message)
        {
            var bodyAsJSON = $"{{\"content\": \"{message}\"}}";
            HttpContent body = new StringContent(bodyAsJSON);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken},
                    { HeaderNames.UserAgent, "DiscordStat" }
                },
                Content = body
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                throw new HttpRequestException();
            }
        }

        public async Task<string> DeleteToDiscordDeleteWebhook(string botToken, string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken},
                    { HeaderNames.UserAgent, "DiscordStat" }
                }

            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                throw new HttpRequestException();
            }
        }


        public async Task<List<WebhookUsageVM>?> GetChannelWebHooks(string botToken, string channelId)
        {
            string uri = "https://discord.com/api/channels/" + channelId + "/webhooks";
            // Remember to handle errors here
            string response = await GetWebHooksInfoInFormOfJsonStringFromEndpoint(botToken, uri);
            // And here
            List<WebhookUsageVM>? webhooks = JsonConvert.DeserializeObject<List<WebhookUsageVM>>(response);
            return webhooks;
        }

        public async Task<string?> CreateWebhook(string botToken, string channelId, string webhookName)
        {
            string uri = "https://discord.com/api/channels/" + channelId + "/webhooks";
            string response = await PostToDiscordCreateWebhook(botToken, uri, webhookName);
            return response;
        }

        public async Task<string?> SendMessageThroughWebhook(string botToken, string webhookId, string webhookToken, string message)
        {
            string uri = "https://discord.com/api/webhooks/" + webhookId + "/" + webhookToken;
            string response = await PostToDiscordSendMessageThroughWebhook(botToken, uri, message);
            return response;
        }

        public async Task<string?> DeleteWebhook(string botToken, string webhookId)
        {
            string uri = "https://discord.com/api/webhooks/" + webhookId;
            string response = await DeleteToDiscordDeleteWebhook(botToken, uri);
            return response;
        }

        public async Task<string?> ChannelEntryAndUpdateDbCheck(Channel[] channels)
        {
            foreach (var channel in channels)
            {
                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allChannels = _channelRepository.GetAll().ToList();
                    var duplicateChannel = new Channel();
                    for (int i = 0; i < allChannels.Count(); i++)
                    {
                        if (channel.Id == allChannels[i].Id)
                        {
                            duplicate = true;
                            duplicateChannel = allChannels[i];
                        }
                    }
                    if (!duplicate)
                    {
                        _channelRepository.AddOrUpdate(channel);
                    }
                    if (duplicate)
                    {

                        _channelRepository.AddOrUpdate(duplicateChannel);
                    }
                });
            }
            return "It Worked";
        }
    }
}

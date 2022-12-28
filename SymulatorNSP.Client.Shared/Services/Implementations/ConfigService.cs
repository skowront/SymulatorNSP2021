using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using SymulatorNSP.Client.Shared.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace SymulatorNSP.Client.Shared.Services.Implementations
{
    public class ConfigService:IConfigService
    {
        private readonly HttpClient httpClient;
        private string? cachedMessage = null;

        public ConfigService(HttpClient httpClient) 
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetRemoteAddresss()
        {
            if (this.cachedMessage == null)
            {
                this.cachedMessage = await this.httpClient.GetStringAsync("/_content/remote-config.json");
            }
            var data = JObject.Parse(cachedMessage);
            return data["remoteServer"]?.Value<string>() ?? string.Empty;
        }
    }
}

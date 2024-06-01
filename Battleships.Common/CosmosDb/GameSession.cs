using Newtonsoft.Json;

namespace Battleships.Common.CosmosDb
{
    public class GameSession
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("gameState")]
        public string GameStateJson { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }
    }
}

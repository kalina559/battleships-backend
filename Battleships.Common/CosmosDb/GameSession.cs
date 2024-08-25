using Newtonsoft.Json;

namespace Battleships.Common.CosmosDb
{
    public class GameSession
    {
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("sessionId")]
        public required string SessionId { get; set; }

        [JsonProperty("gameState")]
        public required string GameStateJson { get; set; }

        [JsonProperty("dateCreated")]
        public required DateTime DateCreated { get; set; }

        [JsonProperty("playerWon")]
        public required bool PlayerWon { get; set; }

        [JsonProperty("playerMovesCount")]
        public required int PlayerMovesCount { get; set; }

        [JsonProperty("opponentMovesCount")]
        public required int OpponentMovesCount { get; set; }

        [JsonProperty("playerAiType")]
        public int? PlayerAiType { get; set; }

        [JsonProperty("opponentAiType")]
        public required int OpponentAiType { get; set; }

        [JsonProperty("shipsCanTouch")]
        public required bool ShipsCanTouch { get; set; }

        [JsonProperty("playersShipsSunk")]
        public required int PlayersShipsSunk { get; set; }

        [JsonProperty("opponentsShipsSunk")]
        public required int OpponentShipsSunk { get; set; }

        [JsonProperty("testType")]
        public required string TestType { get; set; }
    }
}

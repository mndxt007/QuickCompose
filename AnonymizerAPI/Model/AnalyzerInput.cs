using System.Text.Json.Serialization;

namespace AnonymizerAPI.Model
{
    public class AnalyzerInput
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; } = "en";

        [JsonPropertyName("score_threshold")]
        public double ScoreThreshold { get; set; } = 0.2;
    }
}
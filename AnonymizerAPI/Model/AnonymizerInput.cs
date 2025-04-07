using System.Text.Json.Serialization;

namespace AnonymizerAPI.Model
{
    public class AnonymizerInput
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("anonymizers")]
        public Dictionary<string, AnonymizerOptions> AnonymizerOptions { get; set; } = 
            new Dictionary<string, AnonymizerOptions>() { { "DEFAULT", new AnonymizerOptions() } };

        [JsonPropertyName("analyzer_results")]
        public List<AnalyzerResult> AnalyzerResults { get; set; }
    }
}
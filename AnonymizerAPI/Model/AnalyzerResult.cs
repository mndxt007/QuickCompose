using System.Text.Json.Serialization;

namespace AnonymizerAPI.Model
{
    public class AnalyzerResult
    {
        [JsonPropertyName("analysis_explanation")]
        [JsonIgnore]
        public string AnalysisExplanation { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }

        [JsonPropertyName("entity_type")]
        public string EntityType { get; set; }

        [JsonPropertyName("recognition_metadata")]
        [JsonIgnore]
        public RecognitionMetadata RecognitionMetadata { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }
    }
}
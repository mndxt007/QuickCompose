using System.Text.Json.Serialization;

namespace AnonymizerAPI.Model
{
    public class RecognitionMetadata
    {
        [JsonPropertyName("recognizer_identifier")]
        public string RecognizerIdentifier { get; set; }

        [JsonPropertyName("recognizer_name")]
        public string RecognizerName { get; set; }
    }
}
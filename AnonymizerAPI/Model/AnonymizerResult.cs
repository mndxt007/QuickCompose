using System.Text.Json.Serialization;

namespace AnonymizerAPI.Model
{
    public class AnonymizerResult
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }

        [JsonPropertyName("entity_type")]
        public string EntityType { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("operator")]
        public string Operator { get; set; }
    }
}

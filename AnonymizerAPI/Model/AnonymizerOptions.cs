using System.Text.Json.Serialization;

public class AnonymizerOptions
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "replace";

    [JsonPropertyName("new_value")]
    public string NewValue { get; set; } = "***********";
}

namespace ChangeSet.Models.Changers;

[JsonConverter(typeof(ChangerBaseModelConverter))]
internal abstract class ChangerBaseModel
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }
}

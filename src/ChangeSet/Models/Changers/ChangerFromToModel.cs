namespace ChangeSet.Models.Changers;

internal sealed class ChangerFromToModel : ChangerBaseModel
{
    public const string ChangerType = "from-to";

    [JsonPropertyName("from")]
    public required string From { get; set; }

    [JsonPropertyName("to")]
    public required string To { get; set; }
}
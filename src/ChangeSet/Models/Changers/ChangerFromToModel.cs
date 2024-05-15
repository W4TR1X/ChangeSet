namespace ChangeSet.Models.Changers;

internal sealed class ChangerFromToModel : ChangerBaseModel
{
    public const string ChangerType = "from-to";

    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }
}
namespace ChangeSet.Models.Changers;

internal sealed class ChangerReplaceModel : ChangerBaseModel
{
    public const string ChangerType = "replace";

    [JsonPropertyName("find")]
    public required string Find { get; set; }
}

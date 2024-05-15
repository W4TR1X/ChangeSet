namespace ChangeSet.Models.Changers;

internal sealed class ChangerReplaceModel : ChangerBaseModel
{
    public const string ChangerType = "replace";

    [JsonPropertyName("find")]
    public string Find { get; set; }
}

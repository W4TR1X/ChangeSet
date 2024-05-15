using ChangeSet.JsonConverters;

namespace ChangeSet.Models.Changers;

[JsonConverter(typeof(ChangerBaseModelConverter))]
internal abstract class ChangerBaseModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

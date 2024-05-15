namespace ChangeSet.Models;

[JsonSourceGenerationOptions(
    AllowTrailingCommas = true,
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip)]
[JsonSerializable(typeof(InputModel[]))]
internal sealed partial class InputModelArrayContext : JsonSerializerContext { }

internal interface IInputModel
{
    string Parameter { get; }

    public ChangerBaseModel Changer { get; }

    public string[] Filters { get; }
}

internal sealed class InputModel : IInputModel
{
    [JsonPropertyName("parameter")]
    public string Parameter { get; set; }

    [JsonPropertyName("changer")]
    public ChangerBaseModel Changer { get; set; }

    [JsonPropertyName("filters")]
    public string[] Filters { get; set; }
}

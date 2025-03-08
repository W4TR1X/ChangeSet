namespace ChangeSet.JsonConverters;

internal sealed class ChangerBaseModelConverter : JsonConverter<ChangerBaseModel>
{
    public override ChangerBaseModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? type = null;
        string? from = null;
        string? to = null;
        string? find = null;

        string? keyName = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                keyName = reader.GetString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                switch (keyName)
                {
                    case nameof(type):
                        type = reader.GetString();
                        break;

                    case nameof(from) when type == ChangerFromToModel.ChangerType:
                        from = reader.GetString();
                        break;
                    case nameof(to) when type == ChangerFromToModel.ChangerType:
                        to = reader.GetString();
                        break;

                    case nameof(find) when type == ChangerReplaceModel.ChangerType:
                        find = reader.GetString();
                        break;

                    default:
                        throw new NotImplementedException(keyName);
                }

                keyName = null;
            }
            else if (reader.TokenType == JsonTokenType.EndObject)
            {
                return type switch
                {
                    ChangerFromToModel.ChangerType => new ChangerFromToModel()
                    {
                        Type = ChangerFromToModel.ChangerType,
                        From = from!,
                        To = to!
                    },
                    ChangerReplaceModel.ChangerType => new ChangerReplaceModel()
                    {
                        Type = ChangerReplaceModel.ChangerType,
                        Find = find!
                    },
                    _ => throw new NotSupportedException(type)
                };
            }
            else
            {
                throw new NotSupportedException(reader.TokenType.ToString());
            }
        }

        throw new NotImplementedException(keyName);
    }

    public override void Write(Utf8JsonWriter writer, ChangerBaseModel value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
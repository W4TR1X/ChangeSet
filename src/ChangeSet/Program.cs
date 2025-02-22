namespace ChangeSet;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var path = parseParameter(args.FirstOrDefault(x => x.StartsWith("-path="))).Value
            ?? Environment.ProcessPath;

        ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));

        var configPath = parseParameter(args.FirstOrDefault(x => x.StartsWith("-config="))).Value
            ?? "changeset.config.json";

        var parameterArgs = args
            .Where(x => !x.StartsWith("-config=") && !x.StartsWith("-path="))
            .ToArray();

        if (parameterArgs.Length == 0)
            throw new ArgumentNullException("args", "No parameters found");

        var configContent = await File.ReadAllTextAsync(configPath);

        var serializerOptions = new JsonSerializerOptions(InputModelArrayContext.Default.Options)
        {
            PropertyNameCaseInsensitive = false,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        var config = JsonSerializer.Deserialize<InputModel[]>(configContent, InputModelArrayContext.Default.Options);

        var parameters = parameterArgs
            .Select(x => parseParameter(x))
            .ToDictionary(x => x.Key, x => x.Value);

        var paramKeys = parameters.Keys;
        var configKeys = config!.Select(x => x.Parameter).Distinct().ToArray();

        if (paramKeys.Except(configKeys).Any())
            throw new Exception($"Unknown parameters: {string.Join(',', paramKeys.Except(configKeys))}");

        foreach (var parameter in config)
        {
            Console.WriteLine($"Searching for parameter: '{parameter.Parameter}'");

            var parameterValue = parameters.First(x => x.Key == parameter.Parameter).Value;

            IFileProcessor? fileProcessor = null;
            if (parameter.Changer is ChangerFromToModel)
            {
                fileProcessor = new ChangerFromToFileProcessor(parameter, parameterValue, path);
            }
            else if (parameter.Changer is ChangerReplaceModel)
            {
                fileProcessor = new ChangerReplaceFileProcessor(parameter, parameterValue, path);
            }

            await fileProcessor!.ProcessAsync();
        }

        static (string Key, string Value) parseParameter(string? parameter)
        {
            if (parameter is null) return (null, null);

            var tuple = parameter.Split("=");
            var paramName = tuple[0].TrimStart('-');
            var paramValue = tuple[1].TrimStart('\"').TrimEnd('\"');
            return (paramName, paramValue);
        }
    }
}
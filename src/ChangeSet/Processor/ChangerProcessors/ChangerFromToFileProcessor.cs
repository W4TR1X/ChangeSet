namespace ChangeSet.Processor.ChangerProcessors;

internal sealed class ChangerFromToFileProcessor : FileProcessor<ChangerFromToModel>
{
    public ChangerFromToFileProcessor(InputModel inputModel, string parameterValue, string path)
        : base(inputModel, parameterValue, path) { }

    int lineIndex = 0;

    protected override async Task DoProcessAsync(string filePath)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        bool fileChanged = false;

        using (StreamReader reader = new StreamReader(filePath))
        await using (StreamWriter writer = new StreamWriter(tempFilePath))
        {
            bool findCompleted = false;

            string? line;
            while ((line = await reader.ReadLineAsync()) is not null)
            {
                lineIndex = 0;

                while (lineIndex > -1 && lineIndex < line.Length)
                {
                    if (!findCompleted)
                    {
                        var result = await Find(line, writer, Changer.From);

                        if (result == ParamStatus.Success)
                        {
                            findCompleted = true;
                        }
                        else if (result == ParamStatus.Continue)
                        {
                            continue;
                        }
                        else
                        {
                            findCompleted = false;
                        }
                    }
                    else
                    {
                        var result = await Complete(line, writer, Changer.To, ParameterValue);

                        if (result == ParamStatus.Success)
                        {
                            fileChanged = true;
                            findCompleted = false;

                            Console.WriteLine($"Value changed in '{filePath}'");
                        }
                    }
                }
            }
        }

        if (fileChanged)
        {
            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }
    }

    async Task<ParamStatus> Find(string line, StreamWriter writer, string parameter)
    {
        lineIndex = line.IndexOf(parameter, lineIndex);
        if (lineIndex == -1)
        {
            await writer.WriteLineAsync(line);
            return ParamStatus.Failure;
        }

        lineIndex += parameter.Length;

        await writer.WriteAsync(line[0..lineIndex]);
        return ParamStatus.Success;
    }

    async Task<ParamStatus> Complete(string line, StreamWriter writer, string parameter, string value)
    {
        var endIndex = line.IndexOf(parameter, lineIndex);
        if (endIndex == -1)
        {
            lineIndex = line.Length;
            return ParamStatus.Failure;
        }

        await writer.WriteAsync(ParameterValue);
        await writer.WriteLineAsync(line[endIndex..]);

        lineIndex = line.Length;
        return ParamStatus.Success;
    }

    enum ParamStatus
    {
        Continue,
        Success,
        Failure,
    }
}
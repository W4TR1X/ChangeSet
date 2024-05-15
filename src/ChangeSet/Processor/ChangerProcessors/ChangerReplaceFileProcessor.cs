namespace ChangeSet.Processor.ChangerProcessors;

internal sealed class ChangerReplaceFileProcessor : FileProcessor<ChangerReplaceModel>
{
    public ChangerReplaceFileProcessor(InputModel inputModel, string parameterValue, string path)
        : base(inputModel, parameterValue, path) { }

    protected override async Task DoProcessAsync(string filePath)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        bool fileChanged = false;

        using (StreamReader reader = new StreamReader(filePath))
        await using (StreamWriter writer = new StreamWriter(tempFilePath))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) is not null)
            {
                var lineIndex = line.IndexOf(Changer.Find, 0);
                if (lineIndex != -1)
                {
                    line = line.Replace(Changer.Find, ParameterValue);
                    fileChanged = true;

                    Console.WriteLine($"Value changed in '{filePath}'");
                }

                await writer.WriteLineAsync(line);
            }
        }

        if (fileChanged)
        {
            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }
    }
}

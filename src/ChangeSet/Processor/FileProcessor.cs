namespace ChangeSet.Processor;

internal interface IFileProcessor
{
    Task ProcessAsync();
}

internal abstract class FileProcessor<T> : IFileProcessor where T : ChangerBaseModel
{
    public InputModel InputModel { get; }
    public string ParameterValue { get; }
    public string RootPath { get; }

    protected T Changer { get; }

    protected FileProcessor(InputModel inputModel, string parameterValue, string path)
    {
        Changer = (T)inputModel.Changer;
        InputModel = inputModel;
        ParameterValue = parameterValue;
        RootPath = path;
    }

    public Task ProcessAsync()
    {
        return ProcessDirectory(RootPath);
    }

    async Task ProcessDirectory(string path)
    {
        var di = new DirectoryInfo(path);

        List<FileInfo> files = new();

        foreach (var filter in InputModel.Filters)
        {
            files.AddRange(di.GetFiles(filter));
        }

        foreach (var file in files)
        {
            await DoProcessAsync(file.FullName);
        }

        var directories = di.GetDirectories();
        foreach (var dir in directories)
        {
            await ProcessDirectory(dir.FullName);
        }
    }

    protected abstract Task DoProcessAsync(string filePath);
}

namespace ChangeSet.Tests;

[TestClass]
public sealed class UnitTest
{
    [TestMethod]
    public async Task TestWithArgsShouldSuccess()
    {
        var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(path);

        var configPath = Path.Combine(path, "config.json");
        const string configFileContent =
        """
        [
          {
            "parameter": "version",
            "changer": {
              "type": "from-to",
              "from": "<from-to-test>",
              "to": "</from-to-test>"
            },
            "filters": [
              "*.xyz"
            ]
          },
          {
            "parameter": "buildNumber",
            "changer": {
              "type": "replace",
              "find": "replace-this-value"
            },
            "filters": [
              "*.xyz"
            ]
          }
        ]
        """;

        await File.WriteAllTextAsync(configPath, configFileContent);

        var sampleFilePath = Path.Combine(path, "sample.xyz");
        const string sampleFileContent =
        """
        <from-to-test></from-to-test>
        ReplaceTest: replace-this-value
        """;

        await File.WriteAllTextAsync(sampleFilePath, sampleFileContent);

        string[] args = [
            $"-path=\"{path}\"",
            $"-config=\"{configPath}\"",
            "-version=1.2.3",
            "-buildNumber=123321123"
       ];

        await ChangeSet.Program.Main(args);

        var changedSampleFileContent = await File.ReadAllLinesAsync(sampleFilePath);

        Assert.AreEqual("<from-to-test>1.2.3</from-to-test>", changedSampleFileContent[0]);
        Assert.AreEqual("ReplaceTest: 123321123", changedSampleFileContent[1]);

        Directory.Delete(path, true);
    }
}

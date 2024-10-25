namespace RandomNameGeneratorLibrary;

using System.Reflection;

public abstract class BaseNameGenerator
{
    private const string ResourcePathPrefix = "VehicleRegistrationService.RandomNameGeneratorLibrary.Resources.";
    protected readonly Random RandGen;

    protected BaseNameGenerator()
    {
        RandGen = new Random();
    }

    protected BaseNameGenerator(Random randGen)
    {
        RandGen = randGen;
    }

    private static Stream ReadResourceStreamForFileName(string resourceFileName)
    {
        return typeof(BaseNameGenerator)!.GetTypeInfo()!.Assembly!
            .GetManifestResourceStream(ResourcePathPrefix + resourceFileName)!;
    }

    protected static string[] ReadResourceByLine(string resourceFileName)
    {
        var stream = ReadResourceStreamForFileName(resourceFileName);

        List<string> list = [];

        var streamReader = new StreamReader(stream);

        while (streamReader.ReadLine() is { } str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                list.Add(str);
            }
        }

        return list.ToArray();
    }

    protected static string[] ReadResourceByLineSpan(string resourceFileName)
    {
        var stream = ReadResourceStreamForFileName(resourceFileName);
        var buffer = new char[4096].AsSpan();
        int numberRead;
        List<string> list = [];

        var streamReader = new StreamReader(stream);
        while ((numberRead = streamReader.ReadBlock(buffer)) > 0)
        {
            list.Add(buffer[..numberRead].ToString() );
        }

        return list.ToArray();
    }
}
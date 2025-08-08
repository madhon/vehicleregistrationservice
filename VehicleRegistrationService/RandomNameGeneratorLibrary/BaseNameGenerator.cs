namespace RandomNameGeneratorLibrary;

using System.Reflection;

internal abstract class BaseNameGenerator
{
    private const string ResourcePathPrefix = "VehicleRegistrationService.RandomNameGeneratorLibrary.Resources.";

    protected BaseNameGenerator()
    {
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

        using var streamReader = new StreamReader(stream);

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

        using var streamReader = new StreamReader(stream);
        while ((numberRead = streamReader.ReadBlock(buffer)) > 0)
        {
            list.Add(buffer[..numberRead].ToString() );
        }

        return list.ToArray();
    }
}
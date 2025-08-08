using System.Text.RegularExpressions;

namespace RandomNameGeneratorLibrary;

using System.Globalization;

internal sealed partial class CensusListStripper
{
    public static void StripStatisticsFromPersonNameFile(string nameFilePath, string nameStrippedFilePath)
    {
        StripStatisticsAndSaveFile(nameFilePath, nameStrippedFilePath, ExtractPersonNameStrings);
    }

    private static void StripStatisticsAndSaveFile(string nameFilePath, string nameStrippedFilePath,
        Func<string[], StringBuilder> stripFunction)
    {
        var strArray = File.ReadAllLines(nameFilePath);
        var stringBuilder = stripFunction(strArray);
        File.WriteAllText(nameStrippedFilePath, stringBuilder.ToString());
    }

    private static readonly char[] separator =
    [
        ' ',
    ];

    private static StringBuilder ExtractPersonNameStrings(IEnumerable<string> names)
    {
        var stringBuilder = new StringBuilder();
        foreach (var str1 in names)
        {
            var str2 = ConvertToStandardCasing(str1.Split(separator)[0]);
            stringBuilder.AppendLine(str2);
        }
        return stringBuilder;
    }

    private static string ConvertToStandardCasing(string uppercaseName)
    {
#pragma warning disable CA1308
        var str = uppercaseName.ToLower(CultureInfo.InvariantCulture);
#pragma warning restore CA1308

        return str[0].ToString().ToUpper(CultureInfo.InvariantCulture) + str[1..];
    }

    [GeneratedRegex("\\d", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex RemoveDigitsRegex();

    public static string RemoveDigits(string key)
    {
        return RemoveDigitsRegex().Replace(key, replacement: "");
    }

    private static StringBuilder ExtractPlaceNameStrings(IEnumerable<string> names)
    {
        var stringBuilder = new StringBuilder();
        foreach (var key in names)
        {
            var str = RemoveTrailingTextOnPlaceName(RemoveDigits(key)[2..]).Trim();
            stringBuilder.AppendLine(str);
        }
        return stringBuilder;
    }

    private static string RemoveTrailingTextOnPlaceName(string minusState)
    {
        ArgumentOutOfRangeException.ThrowIfNullOrWhiteSpace(minusState);

        var townClassification = GetTownClassification(minusState, throwExceptionOnStrangePlaceName: false);

        var startIndex = minusState.IndexOf(townClassification, StringComparison.OrdinalIgnoreCase);

        return minusState[..startIndex];
    }

    private static string GetTownClassification(string source, bool throwExceptionOnStrangePlaceName)
    {
        if (source.Contains("town", StringComparison.OrdinalIgnoreCase))
            return "town";
        if (source.Contains("city", StringComparison.OrdinalIgnoreCase))
            return "city";
        if (source.Contains("CDP", StringComparison.OrdinalIgnoreCase))
            return "CDP";
        if (source.Contains("village", StringComparison.OrdinalIgnoreCase))
            return "village";
        if (source.Contains("municipality", StringComparison.OrdinalIgnoreCase))
            return "municipality";
        if (source.Contains("borough", StringComparison.OrdinalIgnoreCase))
            return "borough";
        if (source.Contains("(balance)", StringComparison.OrdinalIgnoreCase))
            return "(balance)";
        if (source.Contains("Lexington-Fayette", StringComparison.OrdinalIgnoreCase) || source.Contains("Anaconda-Deer Lodge County", StringComparison.OrdinalIgnoreCase) ||
            (source.Contains("Carson City", StringComparison.OrdinalIgnoreCase) || source.Contains("Lynchburg, Moore County", StringComparison.OrdinalIgnoreCase)) ||
            (source.Contains("comunidad", StringComparison.OrdinalIgnoreCase) || source.Contains("urbana", StringComparison.OrdinalIgnoreCase) || !throwExceptionOnStrangePlaceName))
            return ".";
        throw new ArgumentOutOfRangeException("cannot find town classification in " + source);
    }

    public static void StripStatisticsFromPlaceNameFile(string placeFilePath, string placeStrippedFilePath)
    {
        StripStatisticsAndSaveFile(placeFilePath, placeStrippedFilePath, ExtractPlaceNameStrings);
    }
}
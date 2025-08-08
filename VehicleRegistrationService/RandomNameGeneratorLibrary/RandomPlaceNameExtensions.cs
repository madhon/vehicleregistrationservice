namespace RandomNameGeneratorLibrary;

internal static class RandomPlaceNameExtensions
{
    public static string GenerateRandomPlaceName(this Random rand)
    {
        return new PlaceNameGenerator().GenerateRandomPlaceName();
    }

    public static IEnumerable<string> GenerateMultiplePlaceNames(this Random rand, int numberOfNames)
    {
        return new PlaceNameGenerator().GenerateMultiplePlaceNames(numberOfNames);
    }
}
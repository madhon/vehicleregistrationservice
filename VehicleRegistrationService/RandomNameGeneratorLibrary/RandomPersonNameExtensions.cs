namespace RandomNameGeneratorLibrary;

internal static class RandomPersonNameExtensions
{
    public static string GenerateRandomFirstName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);

        return new PersonNameGenerator().GenerateRandomFirstName();
    }

    public static string GenerateRandomLastName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);

        return new PersonNameGenerator().GenerateRandomLastName();
    }

    public static string GenerateRandomFemaleFirstName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);

        return new PersonNameGenerator().GenerateRandomFemaleFirstName();
    }

    public static string GenerateRandomMaleFirstName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);


        return new PersonNameGenerator().GenerateRandomMaleFirstName();
    }

    public static string GenerateRandomFemaleFirstAndLastName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);


        return new PersonNameGenerator().GenerateRandomFemaleFirstAndLastName();
    }


    public static string GenerateRandomMaleFirstAndLastName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);


        return new PersonNameGenerator().GenerateRandomMaleFirstAndLastName();
    }


    public static string GenerateRandomFirstAndLastName(this Random rand)
    {
        ArgumentNullException.ThrowIfNull(rand);


        return new PersonNameGenerator().GenerateRandomFirstAndLastName();
    }

    public static IEnumerable<string> GenerateMultipleFirstAndLastNames(this Random rand, int numberOfNames)
    {
        ArgumentNullException.ThrowIfNull(rand);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

        return new PersonNameGenerator().GenerateMultipleFirstAndLastNames(numberOfNames);
    }

    public static IEnumerable<string> GenerateMultipleLastNames(this Random rand, int numberOfNames)
    {
        ArgumentNullException.ThrowIfNull(rand);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

        return new PersonNameGenerator().GenerateMultipleLastNames(numberOfNames);
    }

    public static IEnumerable<string> GenerateMultipleFemaleFirstAndLastNames(this Random rand, int numberOfNames)
    {
        ArgumentNullException.ThrowIfNull(rand);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

        return new PersonNameGenerator().GenerateMultipleFemaleFirstAndLastNames(numberOfNames);
    }

    public static IEnumerable<string> GenerateMultipleMaleFirstAndLastNames(this Random rand, int numberOfNames)
    {
        ArgumentNullException.ThrowIfNull(rand);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

        return new PersonNameGenerator().GenerateMultipleMaleFirstAndLastNames(numberOfNames);
    }

    public static IEnumerable<string> GenerateMultipleFemaleFirstNames(this Random rand, int numberOfNames)
    {
        ArgumentNullException.ThrowIfNull(rand);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

        return new PersonNameGenerator().GenerateMultipleFemaleFirstNames(numberOfNames);
    }

    public static IEnumerable<string> GenerateMultipleMaleFirstNames(this Random rand, int numberOfNames)
    {
        ArgumentNullException.ThrowIfNull(rand);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

        return new PersonNameGenerator().GenerateMultipleMaleFirstNames(numberOfNames);
    }
}
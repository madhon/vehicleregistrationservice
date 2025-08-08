using System.Collections.Generic;

namespace RandomNameGeneratorLibrary
{
    internal interface IPlaceNameGenerator
    {
        string GenerateRandomPlaceName();

        IEnumerable<string> GenerateMultiplePlaceNames(int numberOfNames);
    }
}
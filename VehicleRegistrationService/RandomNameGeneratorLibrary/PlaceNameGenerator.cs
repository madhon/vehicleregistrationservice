namespace RandomNameGeneratorLibrary
{
    using System.Security.Cryptography;

    internal sealed class PlaceNameGenerator : BaseNameGenerator, IPlaceNameGenerator
    {
        private const string PlaceNameFile = "places2k.txt.stripped";
        private static string[] _placeNames = null!;

        public PlaceNameGenerator()
        {
            InitPlaceNames();
        }

        public string GenerateRandomPlaceName()
        {
            var index = RandomNumberGenerator.GetInt32(0, _placeNames.Length);
            return _placeNames.AsSpan()[index];
        }

        public IEnumerable<string> GenerateMultiplePlaceNames(int numberOfNames)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfNames);

            var list = new List<string>();

            for (var index = 0; index < numberOfNames; ++index)
            {
                list.Add(GenerateRandomPlaceName());
            }

            return list;
        }

        private static void InitPlaceNames()
        {
            if (_placeNames != null)
            {
                return;
            }

            _placeNames = ReadResourceByLine(PlaceNameFile);
        }
    }
}
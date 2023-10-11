namespace RandomNameGeneratorLibrary
{
    public class PlaceNameGenerator : BaseNameGenerator, IPlaceNameGenerator
    {
        private const string PlaceNameFile = "places2k.txt.stripped";
        private static string[] _placeNames = null!;

        public PlaceNameGenerator()
        {
            InitPlaceNames();
        }

        public PlaceNameGenerator(Random randGen) : base(randGen)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(randGen));
            InitPlaceNames();
        }

        public string GenerateRandomPlaceName()
        {
            var index = RandGen.Next(0, _placeNames.Length);

            return _placeNames[index];
        }

        public IEnumerable<string> GenerateMultiplePlaceNames(int numberOfNames)
        {
            if (numberOfNames < 0) throw new ArgumentOutOfRangeException(nameof(numberOfNames));

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
                return;

            _placeNames = ReadResourceByLine(PlaceNameFile);
        }
    }
}
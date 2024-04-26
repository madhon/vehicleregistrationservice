using System.Reflection;

namespace RandomNameGeneratorLibrary
{
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

            var list = new List<string>();

            var streamReader = new StreamReader(stream);
            string str;

            while ((str = streamReader.ReadLine()) != null)
            {
                if (str != string.Empty)
                    list.Add(str);
            }

            return list.ToArray();
        }
    }
}
namespace VehicleRegistrationService.Repositories
{
    using RandomNameGeneratorLibrary;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using VehicleRegistrationService.Model;

    public class InMemoryVehicleInfoRepository : IVehicleInfoRepository
    {
        private readonly Random _rnd;

        private readonly PersonNameGenerator nameGenerator;

        private readonly string[] _vehicleBrands = {
            "Mercedes", "Toyota", "Audi", "Volkswagen", "Seat", "Renault", "Skoda",
            "Kia", "Citroën", "Suzuki", "Mitsubishi", "Fiat", "Toyota", "Opel" };

        private readonly Dictionary<string, string[]> models = new()
        {
            { "Mercedes", new[] { "A Class", "B Class", "C Class", "E Class", "SLS", "SLK" } },
            { "Toyota", new[] { "Yaris", "CHR", "Rav 4", "Prius", "Celica" } },
            { "Audi", new[] { "A3", "A4", "A6", "A8", "Q5", "Q7" } },
            { "Volkswagen", new[] { "Golf", "Pasat", "Tiguan", "Caddy" } },
            { "Seat", new[] { "Leon", "Arona", "Ibiza", "Alhambra" } },
            { "Renault", new[] { "Megane", "Clio", "Twingo", "Scenic", "Captur" } },
            { "Skoda", new[] { "Octavia", "Fabia", "Superb", "Karoq", "Kodiaq" } },
            { "Kia", new[] { "Picanto", "Rio", "Ceed", "XCeed", "Niro", "Sportage" } },
            { "Citroën", new[] { "C1", "C2", "C3", "C4", "C4 Cactus", "Berlingo" } },
            { "Suzuki", new[] { "Ignis", "Swift", "Vitara", "S-Cross", "Swace", "Jimny" } },
            { "Mitsubishi", new[] { "Space Star", "ASX", "Eclipse Cross", "Outlander PHEV" } },
            { "Ford", new[] { "Focus", "Ka", "C-Max", "Fusion", "Fiesta", "Mondeo", "Kuga" } },
            { "BMW", new[] { "1 Series", "2 Series", "3 Series", "5 Series", "7 Series", "X5" } },
            { "Fiat", new[] { "500", "Panda", "Punto", "Tipo", "Multipla" } },
            { "Opel", new[] { "Karl", "Corsa", "Astra", "Crossland X", "Insignia" } }
        };

        public InMemoryVehicleInfoRepository()
        {
            _rnd = new Random();
            nameGenerator = new PersonNameGenerator(_rnd);
        }

        public VehicleInfo GetVehicleInfo(string licenseNumber)
        {
            // simulate slow IO
            Thread.Sleep(_rnd.Next(5, 200));


            // get random vehicle info
            var brand = GetRandomBrand();
            var model = GetRandomModel(brand);

            // get random owner info
            var ownerName = nameGenerator.GenerateRandomFirstAndLastName();
            var ownerEmail = $"{ownerName.ToLowerInvariant().Replace(' ', '.')}@outlook.com";

            if (licenseNumber.Equals("K27JSD", StringComparison.OrdinalIgnoreCase))
            {
                brand = "Audi";
                model = "A3";
            }

            if (licenseNumber.Equals("K27ASD", StringComparison.OrdinalIgnoreCase))
            {
                brand = "Toyota";
                model = "CHR";
            }

            // return info
            return new VehicleInfo
            {
                VehicleId = licenseNumber,
                Brand = brand,
                Model = model,
                OwnerName = ownerName,
                OwnerEmail = ownerEmail
            };
        }

        private string GetRandomBrand()
        {
            return _vehicleBrands[_rnd.Next(_vehicleBrands.Length)];
        }

        private string GetRandomModel(string brand)
        {
            var models = this.models[brand];
            return models[_rnd.Next(models.Length)];
        }
    }
}

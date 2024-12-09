namespace VehicleRegistrationService.Repositories;

using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
using VehicleRegistrationService.Model;

internal sealed class InMemoryVehicleInfoRepository : IVehicleInfoRepository
{
    private readonly Random _rnd;

    private readonly PersonNameGenerator nameGenerator;

    private readonly string[] vehicleBrands =
    [
        "Mercedes", "Toyota", "Audi", "Volkswagen", "Seat", "Renault", "Skoda",
        "Kia", "Citroën", "Suzuki", "Mitsubishi", "Fiat", "Toyota", "Opel"
    ];

    private readonly Dictionary<string, string[]> models = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Mercedes", ["A Class", "B Class", "C Class", "E Class", "SLS", "SLK"] },
        { "Toyota", ["Yaris", "CHR", "Rav 4", "Prius", "Celica"] },
        { "Audi", ["A3", "A4", "A6", "A8", "Q5", "Q7"] },
        { "Volkswagen", ["Golf", "Pasat", "Tiguan", "Caddy"] },
        { "Seat", ["Leon", "Arona", "Ibiza", "Alhambra"] },
        { "Renault", ["Megane", "Clio", "Twingo", "Scenic", "Captur"] },
        { "Skoda", ["Octavia", "Fabia", "Superb", "Karoq", "Kodiaq"] },
        { "Kia", ["Picanto", "Rio", "Ceed", "XCeed", "Niro", "Sportage"] },
        { "Citroën", ["C1", "C2", "C3", "C4", "C4 Cactus", "Berlingo"] },
        { "Suzuki", ["Ignis", "Swift", "Vitara", "S-Cross", "Swace", "Jimny"] },
        { "Mitsubishi", ["Space Star", "ASX", "Eclipse Cross", "Outlander PHEV"] },
        { "Ford", ["Focus", "Ka", "C-Max", "Fusion", "Fiesta", "Mondeo", "Kuga"] },
        { "BMW", ["1 Series", "2 Series", "3 Series", "5 Series", "7 Series", "X5"] },
        { "Fiat", ["500", "Panda", "Punto", "Tipo", "Multipla"] },
        { "Opel", ["Karl", "Corsa", "Astra", "Crossland X", "Insignia"] },
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

        return new VehicleInfo(licenseNumber, brand, model, ownerName, ownerEmail);
    }

    private string GetRandomBrand()
    {
        return vehicleBrands[_rnd.Next(vehicleBrands.Length)];
    }

    private string GetRandomModel(string brand)
    {
        var rndModels = this.models[brand];
        return rndModels[_rnd.Next(rndModels.Length)];
    }
}
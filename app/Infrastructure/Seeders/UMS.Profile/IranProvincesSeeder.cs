using Microsoft.EntityFrameworkCore;
using UMS.Profile.Domain.Entities;

namespace Infrastructure.Seeders.UMS.Profile;

public class IranProvincesSeeder : BaseSeeder
{
    private Country _iran;

    public IranProvincesSeeder(ModelBuilder modelBuilder, Country iran, int count = 1) : base(modelBuilder,
        count)
    {
        _iran = iran;
    }

    public override void Seed()
    {
        ModelBuilder.Entity<Province>().HasData(
            CreateIranProvince("EA", "East Azarbayejan"),
            CreateIranProvince("WA", "West Azarbayejan"),
            CreateIranProvince("AR", "Ardabil"),
            CreateIranProvince("ES", "Esfahan"),
            CreateIranProvince("AL", "Alborz"),
            CreateIranProvince("IL", "Ilam"),
            CreateIranProvince("BU", "Bushehr"),
            CreateIranProvince("TE", "Tehran"),
            CreateIranProvince("CB", "Chaharmahal va Bakhtiari"),
            CreateIranProvince("KJ", "Khorasan Jonubi"),
            CreateIranProvince("KR", "Khorasan Razavi"),
            CreateIranProvince("KS", "Khorasan Shomal"),
            CreateIranProvince("KH", "Khuzestan"),
            CreateIranProvince("ZA", "Zanjan"),
            CreateIranProvince("SE", "Semnan"),
            CreateIranProvince("SB", "Sistan va Baluchestan"),
            CreateIranProvince("FA", "Fars"),
            CreateIranProvince("QA", "Qazvin"),
            CreateIranProvince("KU", "Kurdestan"),
            CreateIranProvince("KE", "Kerman"),
            CreateIranProvince("KM", "Kermanshah"),
            CreateIranProvince("KB", "Kohgelooye va Booyerahmad"),
            CreateIranProvince("GO", "Golestan"),
            CreateIranProvince("GI", "Gilan"),
            CreateIranProvince("LU", "Lurestan"),
            CreateIranProvince("MZ", "Mazandaran"),
            CreateIranProvince("MA", "Markazi"),
            CreateIranProvince("HO", "Hormozgan"),
            CreateIranProvince("HA", "Hamadan"),
            CreateIranProvince("YA", "Yazd")
        );
    }

    private object CreateIranProvince(string key, string name)
    {
        return new
        {
            Id = Guid.NewGuid(),
            CountryId = _iran.Id,
            Key = key,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
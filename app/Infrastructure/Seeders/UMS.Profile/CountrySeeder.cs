using Microsoft.EntityFrameworkCore;
using UMS.Profile.Domain.Entities;

namespace Infrastructure.Seeders.UMS.Profile;

public class CountrySeeder : BaseSeeder
{
    public CountrySeeder(ModelBuilder modelBuilder, int count = 1) : base(modelBuilder, count)
    {
    }

    public override void Seed()
    {
    }
}
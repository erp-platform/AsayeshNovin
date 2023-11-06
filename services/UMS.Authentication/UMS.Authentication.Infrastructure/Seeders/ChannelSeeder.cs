using UMS.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UMS.Authentication.Infrastructure.Seeders;

public class ChannelSeeder : Seeder
{
    public ChannelSeeder(ModelBuilder modelBuilder, int count = 1) : base(modelBuilder, count)
    {
    }

    public override void Seed()
    {
        foreach(int i in Enum.GetValues(typeof(Domain.Enums.Channel)))
        {
            ModelBuilder.Entity<Channel>().HasData(
                new Channel { Id = i, Name = Enum.GetName(typeof(Domain.Enums.Channel), i) }
            );
        }
    }
}
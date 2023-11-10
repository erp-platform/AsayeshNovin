using Microsoft.EntityFrameworkCore;
using UMS.Authentication.Domain.Entities;
using Enums = UMS.Authentication.Domain.Enums;

namespace Infrastructure.Seeders.UMS.Authentication;

public class ChannelSeeder : BaseSeeder
{
    public ChannelSeeder(ModelBuilder modelBuilder, int count = 1) : base(modelBuilder, count)
    {
    }

    public override void Seed()
    {
        foreach (int i in Enum.GetValues(typeof(Enums.Channel)))
        {
            ModelBuilder.Entity<Channel>().HasData(
                new Channel
                {
                    Id = Guid.NewGuid(),
                    AId = i,
                    Name = Enum.GetName(typeof(Enums.Channel), i),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
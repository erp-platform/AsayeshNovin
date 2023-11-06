using UMS.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public class ChannelSeeder : BaseSeeder
{
    public ChannelSeeder(ModelBuilder modelBuilder, int count = 1) : base(modelBuilder, count)
    {
    }

    public override void Seed()
    {
        foreach (int i in Enum.GetValues(typeof(UMS.Authentication.Domain.Enums.Channel)))
        {
            ModelBuilder.Entity<Channel>().HasData(
                new Channel
                {
                    Id = Guid.NewGuid(),
                    AId = i,
                    Name = Enum.GetName(typeof(UMS.Authentication.Domain.Enums.Channel), i),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
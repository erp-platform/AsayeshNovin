using Microsoft.EntityFrameworkCore;

namespace UMS.Authentication.Infrastructure.Seeders;

public abstract class Seeder
{
    protected readonly ModelBuilder ModelBuilder;
    protected readonly int Count;

    protected Seeder(ModelBuilder modelBuilder, int count = 1)
    {
        ModelBuilder = modelBuilder;
        Count = count;
    }

    public abstract void Seed();
}
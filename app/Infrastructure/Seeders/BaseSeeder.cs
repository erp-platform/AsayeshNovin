using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public abstract class BaseSeeder
{
    protected readonly ModelBuilder ModelBuilder;
    protected readonly int Count;

    protected BaseSeeder(ModelBuilder modelBuilder, int count = 1)
    {
        ModelBuilder = modelBuilder;
        Count = count;
    }

    public abstract void Seed();
}
using MAUI4Maui.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace MAUI4Maui.Services;

public sealed class DataContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<TimeValue> Prices { get; set; }

    public DataContext()
    {
        // needed in the constructor to initiate SQLite on iOS
        SQLitePCL.Batteries_V2.Init();

        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "stocks.db");

        optionsBuilder.UseSqlite($"Filename={dbPath}");

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimeValue>().HasKey(x => new
        {
            x.StockId,
            x.Time
        });

        base.OnModelCreating(modelBuilder);
    }
}
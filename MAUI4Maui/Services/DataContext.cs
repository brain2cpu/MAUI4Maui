using MAUI4Maui.Models.DB;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text;

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

        Database.GetDbConnection().StateChange += OnStateChange;
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

        modelBuilder.Entity<Stock>().Property(c => c.Name)
            .UseCollation("NOCASE");

        base.OnModelCreating(modelBuilder);
    }

    private void OnStateChange(object sender, StateChangeEventArgs e)
    {
        if (e.CurrentState != ConnectionState.Open)
            return;

        //CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = new CultureInfo("ro-RO", false);
        ((SqliteConnection)sender).CreateCollation("NOCASE",
            (x, y) => string.Compare(x, y, StringComparison.CurrentCultureIgnoreCase));
            //(x, y) => string.Compare(RemoveDiacritics(x), RemoveDiacritics(y), StringComparison.CurrentCultureIgnoreCase));
    }

    private static string RemoveDiacritics(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        var stFormD = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char t in stFormD)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(t) != UnicodeCategory.NonSpacingMark)
                sb.Append(t);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
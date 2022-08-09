using CryptoPassive.Infrastructure.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CryptoPassive.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = new SqliteConnectionStringBuilder(@"Data Source=pass.db;") {Password = "123_Incendia_123"}
            .ToString();

        optionsBuilder.UseSqlite(connection);
    }

    // public ApplicationDbContext()
    // {
    //     Database.EnsureCreated();
    //     for (int x = 0; x < 100; x++)
    //     {
    //         string password = Guid.NewGuid().ToString();
    //         Passwords.Add(new Password
    //             {PasswordString = password, Type = (CryptoPassive.Infrastructure.Enums.Type) 2});
    //         Console.WriteLine(password);
    //     }
    //     
    //     SaveChanges();
    // }

    public DbSet<Password> Passwords { get; set; } = null!;
}
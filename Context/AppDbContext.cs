using System;
using Expensify.Models;
using Microsoft.EntityFrameworkCore;

namespace Expensify.Context;

public class AppDbContext : DbContext
{
    public string DbPath { get; }
    public DbSet<Transaction> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        DbPath = "expenses.sqlite";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
        base.OnConfiguring(optionsBuilder);
    }
}

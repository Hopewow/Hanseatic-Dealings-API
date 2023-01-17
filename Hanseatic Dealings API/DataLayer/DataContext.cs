using Hanseatic_Dealings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hanseatic_Dealings_API.DataLayer;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<ShipModel> Players { get; set; }
    public DbSet<CityModel> Cities { get; set; }
    public DbSet<CityStorageModel> CitiesStorage { get; set; }
    public DbSet<ShipStorageModel> PlayersStorage { get; set; }
}

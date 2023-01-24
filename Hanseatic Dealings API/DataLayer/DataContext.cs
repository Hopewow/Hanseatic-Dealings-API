using Hanseatic_Dealings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hanseatic_Dealings_API.DataLayer;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<ShipModel> Ships { get; set; }
    public DbSet<CityModel> Cities { get; set; }
    public DbSet<CityStorageModel> CitiesStorages { get; set; }
    public DbSet<ShipStorageModel> ShipStorages { get; set; }
    public DbSet<UserModel> Users { get; set; }
}

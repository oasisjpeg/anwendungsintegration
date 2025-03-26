using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.NewFolder;

namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlDbContext: DbContext
{
    public DbSet<ConsumptionRecordModel> ConsumptionRecords { get; set; }
    
    public DbSet<UserModel> Users { get; set; }

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }
}
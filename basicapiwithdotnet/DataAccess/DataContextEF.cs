
using basicapiwithdotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace basicapiwithdotnet.DataAccess;

public class DataContextEF : DbContext
{
    
    private readonly IConfiguration _config;
    public DataContextEF(IConfiguration config)
    {
        _config = config;
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

    public virtual DbSet<UserSalary> UserSalary { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.EnableRetryOnFailure()
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ComputerStoreAppSchema");

        // map to users table on schema
        modelBuilder.Entity<User>()
            .ToTable("Users", "ComputerStoreAppSchema")
            .HasKey(u => u.UserId);
        
        modelBuilder.Entity<UserJobInfo>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<UserSalary>()
           .HasKey(u => u.UserId);
    }

}
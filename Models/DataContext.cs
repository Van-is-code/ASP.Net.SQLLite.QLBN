namespace Asp.Net_MvcWeb_Pj3.Aptech.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class DataContext : DbContext
    {
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Patient> Patient { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // Chuỗi kết nối đến SQL Azure
                string connectionString = "Server=tcp:qlbn.database.windows.net,1433;Initial Catalog=qlbn;Persist Security Info=False;User ID=qlbn;Password=@T2307atest;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                
                // Sử dụng SQL Server thay vì SQLite
                options.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publisher>().ToTable("Publisher");
            modelBuilder.Entity<Patient>().ToTable("Patient");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RapidPay.Domain;

namespace RapidPay.Infrastructure.Database
{
    public class RapidPayDbContext : DbContext
    {
        private readonly string _dbPath;

        public DbSet<Card>? Cards { get; set; }
        public DbSet<Payment>? Transactions { get; set; }
        public DbSet<User>? Users { get; set; }


        public RapidPayDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _dbPath = Path.Join(path, "rapidPayDb.db");
        }

        /// <summary>
        /// OnConfiguring overriding to create a Sqlite database file in the specified folder.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={_dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Card entity configuraton */
            modelBuilder.Entity<Card>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Card>()
                .HasAlternateKey(x => x.Number);

            /* Payment entity configuration */
            modelBuilder.Entity<Payment>(payment =>
            {
                payment.HasKey(x => x.Id);
            });
        }
    }
}
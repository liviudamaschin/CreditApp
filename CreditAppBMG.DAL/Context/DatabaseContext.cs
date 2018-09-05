using CreditAppBMG.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace CreditAppBMG.DAL.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            //Database.SetInitializer<DatabaseContext>(null);
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=CreditApp;Trusted_Connection=True;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        #region Db Sets
        public DbSet<StatesEntity> States { get; set; }
        #endregion
    }
}

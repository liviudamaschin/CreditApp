using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CreditAppBMG.ViewModels;
using Microsoft.Extensions.Configuration;

namespace CreditAppBMG.Entities
{
    public partial class CreditAppContext : DbContext
    {

        public CreditAppContext()
        {

        }

        public CreditAppContext(DbContextOptions<CreditAppContext> options)
            : base(options)
        {
        }

        public DbSet<CreditDataEntity> CreditData { get; set; }
        public DbSet<CreditDataFiles> CreditDataFiles { get; set; }
        public DbSet<DbVersion> DbVersion { get; set; }
        public DbSet<StatesEntity> States { get; set; }
        public DbSet<ZipCodesEntity> ZipCodes { get; set; }
        public DbSet<ZipCodesUSEntity> USZipCodes { get; set; }
        public DbSet<DistributorEntity> Distributors { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();

                var config = builder.Build();

                var connstr = config.GetConnectionString("DefaultConnection");
              
                optionsBuilder.UseSqlServer(connstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CreditDataEntity>(entity =>
            //{
            //    entity.HasIndex(e => e.BankReferenceState);

            //    entity.HasIndex(e => e.BillingContactState);

            //    entity.HasIndex(e => e.PrincipalState);

            //    entity.HasIndex(e => e.PriorBusinessState);

            //    entity.HasIndex(e => e.PropertyState);

            //    entity.HasIndex(e => e.State);

            //    entity.HasIndex(e => e.Token);

            //    entity.HasIndex(e => e.TradeReference1State);

            //    entity.HasIndex(e => e.TradeReference2State);

            //    entity.Property(e => e.Address1)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.Address2).HasMaxLength(100);

            //    entity.Property(e => e.BankReferenceAccountNumber)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.BankReferenceAccountType)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.BankReferenceAddress1)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BankReferenceAddress2).HasMaxLength(100);

            //    entity.Property(e => e.BankReferenceCity)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BankReferenceName)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BankReferencePhone)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.BankReferenceRoutingNumber)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.BankReferenceState)
            //        .HasMaxLength(2);

            //    entity.Property(e => e.BankReferenceZipCode)
            //        .HasMaxLength(5);

            //    entity.Property(e => e.BillingContactAddress1)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BillingContactAddress2).HasMaxLength(100);

            //    entity.Property(e => e.BillingContactCity)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BillingContactEmail)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BillingContactName)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.BillingContactPhone)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.BillingContactState)
            //        .HasMaxLength(2);

            //    entity.Property(e => e.BillingContactZipCode)
            //        .HasMaxLength(5);

            //    entity.Property(e => e.BusinessName)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.City)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.CompanyType)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.CreatedDate)
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.DeliveryTime).HasMaxLength(50);

            //    entity.Property(e => e.Ein)
            //        .HasColumnName("EIN")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.LastUpdate)
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.LicenseExpirationDate).HasColumnType("date");

            //    entity.Property(e => e.LicenseNumber)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.NystateTaxId)
            //        .HasColumnName("NYStateTaxId")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.Phone)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.PrincipalAddress1)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.PrincipalAddress2).HasMaxLength(100);

            //    entity.Property(e => e.PrincipalCity)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.PrincipalEmail)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.PrincipalName)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.PrincipalPhone)
            //        .HasMaxLength(50);

            //    entity.Property(e => e.PrincipalSsn)
            //        .HasColumnName("PrincipalSSN")
            //        .HasMaxLength(50);

            //    entity.Property(e => e.PrincipalState)
            //        .HasMaxLength(2);

            //    entity.Property(e => e.PrincipalTitle)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.PrincipalZipCode)
            //        .HasMaxLength(5);

            //    entity.Property(e => e.PriorBusiness).HasDefaultValueSql("((0))");

            //    entity.Property(e => e.PriorBusinessAddress1).HasMaxLength(100);

            //    entity.Property(e => e.PriorBusinessAddress2).HasMaxLength(100);

            //    entity.Property(e => e.PriorBusinessCity).HasMaxLength(100);

            //    entity.Property(e => e.PriorBusinessState).HasMaxLength(2);

            //    entity.Property(e => e.PriorBusinessZipCode).HasMaxLength(5);

            //    entity.Property(e => e.PropertyAddress1).HasMaxLength(100);

            //    entity.Property(e => e.PropertyAddress2).HasMaxLength(100);

            //    entity.Property(e => e.PropertyCity).HasMaxLength(100);

            //    entity.Property(e => e.PropertyState).HasMaxLength(2);

            //    entity.Property(e => e.PropertyType).HasMaxLength(50);

            //    entity.Property(e => e.PropertyZipCode).HasMaxLength(5);

            //    entity.Property(e => e.State)
            //        .HasMaxLength(2);

            //    entity.Property(e => e.Token)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.TradeName)
            //        .HasMaxLength(100);

            //    entity.Property(e => e.TradeReference1AccountNumber).HasMaxLength(50);

            //    entity.Property(e => e.TradeReference1Address1).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference1Address2).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference1City).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference1Name).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference1Phone).HasMaxLength(50);

            //    entity.Property(e => e.TradeReference1State).HasMaxLength(2);

            //    entity.Property(e => e.TradeReference1ZipCode).HasMaxLength(5);

            //    entity.Property(e => e.TradeReference2AccountNumber).HasMaxLength(50);

            //    entity.Property(e => e.TradeReference2Address1).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference2Address2).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference2City).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference2Name).HasMaxLength(100);

            //    entity.Property(e => e.TradeReference2Phone).HasMaxLength(50);

            //    entity.Property(e => e.TradeReference2State).HasMaxLength(2);

            //    entity.Property(e => e.TradeReference2ZipCode).HasMaxLength(5);

            //    entity.Property(e => e.ZipCode)
            //        .HasMaxLength(5);
            //});

            //modelBuilder.Entity<CreditDataFiles>(entity =>
            //{
            //    entity.HasIndex(e => e.CreditDataId);

            //    entity.Property(e => e.CreatedDate)
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.LastUpdate)
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.LicenseFileName).HasMaxLength(100);

            //    entity.Property(e => e.TaxCertificateFileName).HasMaxLength(100);

            //    //entity.HasOne(d => d.CreditData)
            //        //.WithMany(p => p.CreditDataFiles)
            //        //.HasForeignKey(d => d.CreditDataId)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //      //  .HasConstraintName("FK_CreditDataFiles_CreditData");
            //});

            //modelBuilder.Entity<DbVersion>(entity =>
            //{
            //    entity.HasKey(e => new { e.Product, e.ReleaseDate })
            //        .HasName("PK_DbVersion");

            //    entity.ToTable("_DbVersion");

            //    entity.Property(e => e.Product).HasMaxLength(100);

            //    entity.Property(e => e.ReleaseDate)
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.IntVersion).HasComputedColumnSql("(([Major]*(1000000)+[Minor]*(1000))+[Build])");
            //});

            //modelBuilder.Entity<StatesEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Abbreviation)
            //        .HasName("PK_States_1");

            //    entity.Property(e => e.Abbreviation)
            //        .HasMaxLength(2)
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.State)
            //        .IsRequired()
            //        .HasMaxLength(100);
            //});

            //modelBuilder.Entity<ZipCodesEntity>(entity =>
            //{
            //    entity.HasIndex(e => e.Abbreviation);

            //    entity.Property(e => e.Abbreviation)
            //        .IsRequired()
            //        .HasMaxLength(2);

            //    entity.Property(e => e.ZipCodeHigh)
            //        .IsRequired()
            //        .HasMaxLength(5)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ZipCodeLow)
            //        .IsRequired()
            //        .HasMaxLength(5)
            //        .IsUnicode(false);
            //});
        }

    }
}

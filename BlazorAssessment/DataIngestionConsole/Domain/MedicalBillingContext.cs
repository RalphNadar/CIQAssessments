using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIngestionConsole.Domain
{
    public class MedicalBillingContext : DbContext
    {
        public DbSet<Provider> Providers { get; set; }
        public DbSet<BillingRecord> BillingRecords { get; set; }

        public MedicalBillingContext(DbContextOptions<MedicalBillingContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MedicalBillingContext");

            modelBuilder.Entity<Provider>()
                 .HasKey(p => p.NPI); // Set NPI as the primary key

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.NPI)
                .IsUnique();

            modelBuilder.Entity<BillingRecord>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<BillingRecord>()
                .HasOne(b => b.Provider)
                .WithMany(p => p.BillingRecords)
                .HasForeignKey(b => b.NPI)
                .HasPrincipalKey(p => p.NPI); // Explicitly specify NPI as the principal key
        }
    }

    public class Provider
    {
        [Key]
        public string NPI { get; set; }
        public string ProviderName { get; set; }
        public string Specialty { get; set; }
        public string State { get; set; }
        public List<BillingRecord> BillingRecords { get; set; }
    }

    public class BillingRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string NPI { get; set; }
        public string HCPCSCode { get; set; }
        public string HCPCSDescription { get; set; }
        public string PlaceOfService { get; set; }
        public int NumberOfServices { get; set; }
        public decimal TotalMedicarePayment { get; set; }
        [ForeignKey("NPI")]
        public Provider Provider { get; set; }
    }
}


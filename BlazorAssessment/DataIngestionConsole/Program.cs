using System;
using System.Data;
using System.Formats.Asn1;
using LumenWorks.Framework.IO.Csv;
using Microsoft.Data.Sqlite;
using DataIngestionConsole.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataIngestionConsole
{
    public static class Program
    {
        private static readonly string connectionString = "Data Source=medical_billing.sqlite";
        private static MedicalBillingContext context;

        static async Task Main(string[] args)
        {
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSV", "Reduced_MUP_PHT_R25_P05_V20_D23_Prov_Svc.csv");

                if (File.Exists(filePath))
                {
                    var providers = await GetProviders(filePath);
                    var billingRecords = await GetBillingRecords(filePath);

                    var optionsBuilder = new DbContextOptionsBuilder<MedicalBillingContext>();
                    optionsBuilder.UseSqlite(connectionString);

                    using (context = new MedicalBillingContext(optionsBuilder.Options))
                    {
                        //Delete previous db if exists
                        context.Database.EnsureDeleted();
                        Console.WriteLine("Database deleted successfully!");

                        // Create the database if it doesn't exist
                        context.Database.EnsureCreated();
                        Console.WriteLine("Database created successfully!");

                        // Insert data
                        await InsertProviders(context, providers);
                        await InsertBillingRecords(context, billingRecords);

                        Console.WriteLine("Data Available!");
                        Console.ReadLine();

                    }
                }
                else
                {
                    Console.WriteLine("Error locating CSV file paths.");
                }

            }
            catch (Exception ex)
            {
                var error = ex.Message;
                Console.WriteLine(error);
            }
        }


        private static async Task InsertProviders(MedicalBillingContext context, List<Domain.Provider> providers)
        {
            try
            {
                if (providers.Count > 0)
                {
                    foreach (var item in providers)
                    {
                        await context.Providers.AddAsync(item);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    Console.WriteLine("Providers Empty");
                }

                Console.WriteLine("Providers Populated");

            }
            catch (Exception ex)
            {
                var error = ex;
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task InsertBillingRecords(MedicalBillingContext context, List<Domain.BillingRecord> records)
        {
            try
            {
                if (records.Count > 0)
                {
                    foreach (var item in records)
                    {
                        await context.BillingRecords.AddAsync(item);
                        await context.SaveChangesAsync();
                    }

                }
                else
                {
                    Console.WriteLine("Billing Records Empty");
                }

                Console.WriteLine("Billing Records Populated");
            }
            catch (Exception ex)
            {
                var error = ex;
                Console.WriteLine(ex.Message);
            }
        }


        private static async Task<List<Domain.Provider>> GetProviders(string filePath)
        {
            try
            {
                var csvTable = new DataTable();
                using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
                {
                    csvTable.Load(csvReader);
                }

                var models = new List<Domain.Provider>();

                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    var model = new Domain.Provider();
                    model.NPI = csvTable.Rows[i][0].ToString();
                    model.ProviderName = csvTable.Rows[i][2].ToString() + " " + csvTable.Rows[i][1].ToString();
                    model.Specialty = csvTable.Rows[i][4].ToString();
                    model.State = csvTable.Rows[i][9].ToString();

                    models.Add(model);
                }

                var distinctProviders = models.GroupBy(x => x.NPI).Select(y => y.First());
                return await Task.FromResult(distinctProviders.ToList());

            }
            catch (Exception ex)
            {
                var error = ex;
                return null;
            }
        }

        private static async Task<List<Domain.BillingRecord>> GetBillingRecords(string filePath)
        {
            try
            {
                var csvTable = new DataTable();
                using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
                {
                    csvTable.Load(csvReader);
                }

                var models = new List<Domain.BillingRecord>();

                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    var model = new Domain.BillingRecord();
                    model.NPI = csvTable.Rows[i][0].ToString();
                    model.HCPCSCode = csvTable.Rows[i][17].ToString();
                    model.HCPCSDescription = csvTable.Rows[i][18].ToString();
                    model.PlaceOfService = csvTable.Rows[i][20].ToString();
                    model.NumberOfServices = Convert.ToInt32(csvTable.Rows[i][22].ToString());
                    model.TotalMedicarePayment = Convert.ToDecimal(csvTable.Rows[i][26].ToString());

                    models.Add(model);
                }
                return await Task.FromResult(models);
            }
            catch (Exception ex)
            {
                var error = ex;
                return null;
            }
        }


    }
}
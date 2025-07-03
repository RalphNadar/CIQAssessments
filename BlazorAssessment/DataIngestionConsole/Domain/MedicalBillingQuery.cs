using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIngestionConsole.Domain
{
    public interface IMedicalBillingQuery
    {
        Task<ProviderBilling.Sdk.Models.Provider> GetProvider(string NPI);
        Task<List<ProviderBilling.Sdk.Models.Provider>> GetAllProviders();
        Task<ProviderBilling.Sdk.Models.BillingRecord> GetBillingRecord(string NPI);
        Task<List<ProviderBilling.Sdk.Models.BillingRecord>> GetAllBillingRecords();

    }

    public class MedicalBillingQuery:IMedicalBillingQuery
    {
        private MedicalBillingContext _db;
        private IQueryable<Provider> _provider;
        private IQueryable<BillingRecord> _records;

        public MedicalBillingQuery(MedicalBillingContext db)
        {
            _db = db;
            _provider = db.Providers;
            _records = db.BillingRecords;
        }

        public async Task<ProviderBilling.Sdk.Models.Provider> GetProvider(string NPI)
        {
            var dbProvider = await _provider.Where(x => x.NPI.ToLower() == NPI.ToLower()).Include(y => y.BillingRecords).SingleOrDefaultAsync();
            if (dbProvider != null)
            {

                var records = new List<ProviderBilling.Sdk.Models.BillingRecord>();
                if (dbProvider.BillingRecords != null && dbProvider.BillingRecords.Count > 0)
                {
                    foreach (var item in dbProvider.BillingRecords)
                    {
                        records.Add(new ProviderBilling.Sdk.Models.BillingRecord
                        {
                            NPI = item.NPI,
                            HCPCSCode = item.HCPCSCode,
                            HCPCSDescription = item.HCPCSDescription,
                            PlaceOfService = item.PlaceOfService,
                            NumberOfServices = item.NumberOfServices,
                            TotalMedicarePayment = item.TotalMedicarePayment
                        });
                    }
                }

                var provider = new ProviderBilling.Sdk.Models.Provider
                {
                    NPI = dbProvider.NPI,
                    ProviderName = dbProvider.ProviderName,
                    Specialty = dbProvider.Specialty,
                    State = dbProvider.State,
                    BillingRecords = records
                };
                return provider;
            }
            return null;
        }

        public async Task<List<ProviderBilling.Sdk.Models.Provider>> GetAllProviders()
        {
            var dbProviders = await _provider.ToListAsync();
            if (dbProviders.Count > 0)
            {
                var providers = new List<ProviderBilling.Sdk.Models.Provider>();
                foreach (var item in dbProviders)
                {
                    var records = new List<ProviderBilling.Sdk.Models.BillingRecord>();

                    if (item.BillingRecords != null && item.BillingRecords.Count > 0)
                    {
                        foreach (var record in item.BillingRecords)
                        {
                            records.Add(new ProviderBilling.Sdk.Models.BillingRecord
                            {
                                NPI = record.NPI,
                                HCPCSCode = record.HCPCSCode,
                                HCPCSDescription = record.HCPCSDescription,
                                PlaceOfService = record.PlaceOfService,
                                NumberOfServices = record.NumberOfServices,
                                TotalMedicarePayment = record.TotalMedicarePayment
                            });
                        }
                    }

                    providers.Add(new ProviderBilling.Sdk.Models.Provider
                    {
                        NPI = item.NPI,
                        ProviderName = item.ProviderName,
                        Specialty = item.Specialty,
                        State = item.State,
                        BillingRecords = records
                    });
                }
                return providers;
            }
            return null;
        }

        public async Task<ProviderBilling.Sdk.Models.BillingRecord> GetBillingRecord(string NPI)
        {
            var dbRecord = await _records.Where(x => x.NPI.ToLower() == NPI.ToLower()).SingleOrDefaultAsync();
            if (dbRecord != null)
            {
                var record = new ProviderBilling.Sdk.Models.BillingRecord
                {
                    NPI = dbRecord.NPI,
                    HCPCSCode = dbRecord.HCPCSCode,
                    HCPCSDescription = dbRecord.HCPCSDescription,
                    PlaceOfService = dbRecord.PlaceOfService,
                    NumberOfServices = dbRecord.NumberOfServices,
                    TotalMedicarePayment = dbRecord.TotalMedicarePayment,
                    Provider = dbRecord.Provider != null ? new ProviderBilling.Sdk.Models.Provider
                    {
                        NPI = dbRecord.Provider.NPI,
                        ProviderName = dbRecord.Provider.ProviderName,
                        Specialty = dbRecord.Provider.Specialty,
                        State = dbRecord.Provider.State,

                    } : null
                };
                return record;
            }
            return null;
        }

        public async Task<List<ProviderBilling.Sdk.Models.BillingRecord>> GetAllBillingRecords()
        {
            var dbRecords = await _records.ToListAsync();
            if (dbRecords.Count > 0)
            {
                var records = new List<ProviderBilling.Sdk.Models.BillingRecord>();
                foreach (var item in dbRecords)
                {
                    records.Add(new ProviderBilling.Sdk.Models.BillingRecord
                    {
                        NPI = item.NPI,
                        HCPCSCode = item.HCPCSCode,
                        HCPCSDescription = item.HCPCSDescription,
                        PlaceOfService = item.PlaceOfService,
                        NumberOfServices = item.NumberOfServices,
                        TotalMedicarePayment = item.TotalMedicarePayment,
                        Provider = item.Provider != null ? new ProviderBilling.Sdk.Models.Provider
                        {
                            NPI = item.Provider.NPI,
                            ProviderName = item.Provider.ProviderName,
                            Specialty = item.Provider.Specialty,
                            State = item.Provider.State,

                        } : null
                    });
                }
                return records;
            }
            return null;
        }

    }
}

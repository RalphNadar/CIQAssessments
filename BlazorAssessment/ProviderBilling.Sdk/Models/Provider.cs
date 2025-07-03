using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderBilling.Sdk.Models
{
    public class Provider
    {
        public string NPI { get; set; }
        public string ProviderName { get; set; }
        public string Specialty { get; set; }
        public string State { get; set; }
        public List<BillingRecord>? BillingRecords { get; set; }
    }
}

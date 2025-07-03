using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderBilling.Sdk.Models
{
    public class BillingRecord
    {
        public string NPI { get; set; }
        public string HCPCSCode { get; set; }
        public string HCPCSDescription { get; set; }
        public string PlaceOfService { get; set; }
        public int NumberOfServices { get; set; }
        public decimal TotalMedicarePayment { get; set; }
        public Provider? Provider { get; set; }
    }
}

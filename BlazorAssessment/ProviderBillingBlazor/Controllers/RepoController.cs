using ProviderBilling.Sdk.Models;
using Microsoft.AspNetCore.Mvc;
using DataIngestionConsole.Domain;

namespace ProviderBillingBlazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepoController : Controller
    {
        private readonly IMedicalBillingQuery _query;

        public RepoController(IMedicalBillingQuery query, MedicalBillingContext context)
        {
            _query = query;
        }


        [HttpGet("GetProviders")]
        public async Task<ActionResult<List<ProviderBilling.Sdk.Models.Provider>>> GetProviders()
        {
            try
            {
                var providers = await _query.GetAllProviders();
                return providers;
            }
            catch (Exception ex)
            {
                var error = ex;
                return BadRequest();
            }
        }

        [HttpGet("GetRecords")]
        public async Task<ActionResult<List<ProviderBilling.Sdk.Models.BillingRecord>>> GetRecords()
        {
            try
            {
                var records = await _query.GetAllBillingRecords();
                return records;
            }
            catch (Exception ex)
            {
                var error = ex;
                return BadRequest();
            }
        }

        [HttpGet("GetProvNPI/{NPI}")]
        public async Task<ActionResult<ProviderBilling.Sdk.Models.Provider>> GetProvNPI(string NPI)
        {
            try
            {
                var provider = await _query.GetProvider(NPI);
                return Ok(provider);
            }
            catch (Exception ex)
            {
                var error = ex;
                return BadRequest();
            }
        }

        [HttpGet("GetRecNPI/{NPI}")]
        public async Task<ActionResult<ProviderBilling.Sdk.Models.BillingRecord>> GetRecNPI(string NPI)
        {
            try
            {
                var record = await _query.GetBillingRecord(NPI);
                return Ok(record);
            }
            catch (Exception ex)
            {
                var error = ex;
                return BadRequest();
            }
        }
    }
}

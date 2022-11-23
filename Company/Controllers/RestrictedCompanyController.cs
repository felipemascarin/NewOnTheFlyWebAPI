using DomainAPI.Models.Company;
using DomainAPI.Services.Company;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestrictedCompanyController : ControllerBase
    {
        private readonly RestrictedCompanyServices _restrictedCompanyServices;
        private readonly CompanyServices _companyServices;
        private readonly AircraftService _aircraftServices;

        public RestrictedCompanyController(RestrictedCompanyServices restrictedCompanyServices, CompanyServices companyServices, AircraftService aircraftService)
        {
            _restrictedCompanyServices = restrictedCompanyServices;
            _companyServices = companyServices;
            _aircraftServices = aircraftService;
        }

        //Endpoint para obter todas companhias restritas cadastradas
        [HttpGet]
        public async Task<ActionResult<List<RestrictedCompany>>> Get() => await _restrictedCompanyServices.Get();

        //Endpoint para obter uma companhia restrita especifica
        [HttpGet("{cnpj}", Name = "GetRestrict")]
        public async Task<ActionResult<RestrictedCompany>> Get(string cnpj) => await _restrictedCompanyServices.Get(cnpj);

        //Endpoint para adionar um cnpj de companhia restrita
        [HttpPost]
        public async Task<ActionResult<RestrictedCompany>> Create(RestrictedCompany restrict)
        {
            var cnpj = restrict.CNPJ.Replace(".", "").Replace("-", "").Replace("/", "").Replace("%2F", "");

            restrict.CNPJ = cnpj;

            var restrictIn = await _restrictedCompanyServices.Get(cnpj);

            if (restrictIn is not null) return BadRequest("CNPJ já cadastrado!");

            var company = await _companyServices.Get(cnpj);

            if (company is not null)
            {
                company.Status = false;
            }

            await _companyServices.Put(cnpj, company);

            var listAircraft = await _aircraftServices.GetAircraft(cnpj);

            foreach (var airctafft in listAircraft)
            {
                if(airctafft.Company.CNPJ == cnpj)
                {
                    airctafft.Company = company;
                    await _aircraftServices.UpdateAircraft(airctafft);
                }
            }            

            await _restrictedCompanyServices.Create(restrict);

            return Ok(restrict);
        }

        //Endpoint para alteração do cnpj retrito
        [HttpPut]
        public async Task<IActionResult> Put(string cnpj)
        {
            var restricted = await _restrictedCompanyServices.Get(cnpj);

            if (restricted is null) return BadRequest();

            await _restrictedCompanyServices.Put(cnpj, restricted);
            
            return Ok();
        }

        //Endpoint para deletar um cnpj dos restritos
        [HttpDelete("{cnpj}")]
        public async Task<IActionResult> Remove(string cnpj)
        {
            var cnpjIn = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("%2F", "");
                        
            var restricted = await _restrictedCompanyServices.Get(cnpjIn);

            if(restricted is null) return NotFound();

            var company = await _companyServices.Get(cnpj);

            if (company is not null) company.Status = true;
                       
            await _companyServices.Put(cnpj, company);

            var listAircraft = await _aircraftServices.GetAircraft(cnpj);

            foreach (var airctafft in listAircraft)
            {
                if (airctafft.Company.CNPJ == cnpj)
                {
                    airctafft.Company = company;
                    await _aircraftServices.UpdateAircraft(airctafft);
                }
            }

            await _restrictedCompanyServices.Remove(cnpj);

            return NoContent();
        } 
    }
}
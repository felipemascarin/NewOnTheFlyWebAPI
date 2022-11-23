using DomainAPI.Models.Company;
using DomainAPI.Services.Company;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeadfileCompanyController : ControllerBase
    {
        private readonly DeadfileCompanyServices _deadfileCompanyServices;

        public DeadfileCompanyController(DeadfileCompanyServices deadfileCompanyServices)
        {
            _deadfileCompanyServices = deadfileCompanyServices;
        }


        //endpoint para obter todos arquivos mortos/excluidos de companhia
        [HttpGet]
        public async Task<ActionResult<List<DeadfileCompany>>> Get() => await _deadfileCompanyServices.Get();

        //endpoint para obter um morto/excluido especifico
        [HttpGet("{cnpj}")]
        public async Task<ActionResult<DeadfileCompany>> Get(string cnpj)
        {
            var deadfileCompany = await _deadfileCompanyServices.Get(cnpj);

            if (deadfileCompany is null) return NotFound();

            return Ok(deadfileCompany);
        }

        //Cria um arquivo morto no banco de dados
        [HttpPost]
        public async Task Create(DeadfileCompany deadfile)
        {
            await _deadfileCompanyServices.Create(deadfile);            
        }

        //Atualiza um arquivo morto, porém não está disponivel para o usuário
        [HttpPut]
        public async Task Put(string cnpj, DeadfileCompany deadfile) => await _deadfileCompanyServices.Put(cnpj, deadfile);

        //Excluir um arquivo morto, porém não está disponivel para o usuário
        [HttpDelete("{cnpj}")]
        public async Task Remove(string cnpj) => await _deadfileCompanyServices.Remove(cnpj);
    }
}

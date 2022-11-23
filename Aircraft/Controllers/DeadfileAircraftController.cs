using DomainAPI.Models.Aircraft;
using DomainAPI.Services.Aircraft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aircraft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeadfileAircraftController : ControllerBase
    {
        private readonly DeadfileAircraftServices _deadfileAircraftServices;

        public DeadfileAircraftController(DeadfileAircraftServices deadfileAircraftServices)
        {
            _deadfileAircraftServices = deadfileAircraftServices;
        }

        //Chamada do endpoint para obter todos os arquivos excluidos/mortos
        [HttpGet]
        public async Task<ActionResult<List<DeadfileAircrafts>>> Get() => await _deadfileAircraftServices.Get();

        //Chamada do endpoint para obter um arquivo excluido/morto especifico
        [HttpGet("{rab}", Name = "GetFile")]
        public async Task<ActionResult<DeadfileAircrafts>> Get(string rab) => await _deadfileAircraftServices.Get(rab);

        //Criação do arquivo morto, após ser solicitado um delete de qlqr classe
        [HttpPost]
        public async Task Create(DeadfileAircrafts deadfile)
        {
            await _deadfileAircraftServices.Create(deadfile);
        }

        //Endpoint de atualização do arquivo morto, porém o mesmo não pode ser usado.
        [HttpPut]
        public async Task<IActionResult> Put(string rab)
        {
            var file = await _deadfileAircraftServices.Get(rab);

            if (file is null) return NotFound();

            return Ok("Objeto encontrado, porém não pode ser editado!");
        }

        //Endpoint de delete do arquivo morto, porém o mesmo não pode ser usado!
        [HttpDelete]
        public async Task<IActionResult> Remove(string rab)
        {
            var file = await _deadfileAircraftServices.Get(rab);

            if (file is null) return NotFound();

            return Ok("Objeto encontrado, porém não pode ser excluido!");
        }
    }
}

using DomainAPI.Models.Aircraft;
using DomainAPI.Models.Flight;
using DomainAPI.Models.Passenger;
using DomainAPI.Models.Sale;
using DomainAPI.Services.Flight;
using DomainAPI.Services.Passenger;
using DomainAPI.Services.Sale;
using DomainAPI.Utils.Passenger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Saler.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase {
        private readonly SalesService _salesService;
        Sales sales = new();
        public SaleController(SalesService salesServices) {
            _salesService = salesServices;
        }
        //Get para buscar todas as vendas
        [HttpGet]
        public ActionResult<List<Sales>> Get() => _salesService.Get();

        //Post para cadastrar uma venda de passagens
        [HttpPost("Sold/cpf")]
        public ActionResult<Sales> CreateSold(string cpf, DateTime date, string rab) {
          
            string[] list = cpf.Split(',');
            
            //Lista das pessoas de uma venda
            List<Passengers> peoples = new List<Passengers>();
           
            //Laço para verificar se os CPF estão cadastrados e retorna o objeto Passenger
            for (int i = 0; i < list.Length; i++) {
                var passenger = new ConsumerController().GetPassengerAsync(list[i]);

                //Verificação se o primeiro da lista de Passengers(titular da venda) não é menor de 18 anos.
                int age = DateTime.Now.Year - passenger.Result.DtBirth.Year;
                if (i == 0 && age < 18) {
                    return BadRequest("Precisa ser Maior de 18 Anos para Comprar a Passagem!");
                } else {
                   
                    //Adiciona o Passenger na lista
                    peoples.Add(passenger.Result);

                    //Verificação se para não haver o cadastro do mesmo CPF no mesmo voo
                    var sale = _salesService.GetSpecificSale(passenger.Result,date,rab);
                    if (sale == null) {
                        sales.Passengers = peoples;
                    } else {
                        return BadRequest("Venda já foi Cadastrada com esse CPF!");
                    }
                }
            }
            
            //Busca um voo específico
            var fligth = new ConsumerController().GetFlightAsync(date, rab);
            sales.Flight = fligth.Result;

            //Verifica se a data da venda é a mesma do voo
            if (sales.Flight.Departure == date && sales.Flight.Plane.RAB.Equals(rab)) {
                
                //Verifica se a quantidade de vendas é menor que a capacidade da aeronave
                if (sales.Flight.Sales + sales.Passengers.Count <= sales.Flight.Plane.Capacity) {
                    sales.Sold = true;
                    sales.Reserved = false;
                    sales.Flight.Sales += sales.Passengers.Count;
                    
                } else {
                    return BadRequest("Capacidade de Assentos da Aeronave está Esgotado!");
                }
            } else {
                return BadRequest("Não existe Voo Marcado para essa Data!");
            }

            //Altera a quantidade de vendas realizadas no voo
            var result = new ConsumerController().PutFlightAsync(sales.Flight);

            _salesService.Create(sales);
            
            return Ok(sales);
        }
        //Post para cadastrar uma reserva de passagens
        [HttpPost("Reserverd/cpf")]
        public ActionResult<Sales> CreateReserved(string cpf, DateTime date, string rab)
        {

            string[] list = cpf.Split(',');

            //Lista das pessoas de uma venda
            List<Passengers> peoples = new List<Passengers>();

            //Laço para verificar se os CPF estão cadastrados e retorna o objeto Passenger
            for (int i = 0; i < list.Length; i++)
            {
                var passenger = new ConsumerController().GetPassengerAsync(list[i]);

                //Verificação se o primeiro da lista de Passengers(titular da venda) não é menor de 18 anos.
                int age = DateTime.Now.Year - passenger.Result.DtBirth.Year;
                if (i == 0 && age < 18)
                {
                    return BadRequest("Precisa ser Maior de 18 Anos para Comprar a Passagem!");
                }
                else
                {
                    //Adiciona o Passenger na lista
                    peoples.Add(passenger.Result);

                    //Verificação se para não haver o cadastro do mesmo CPF no mesmo voo
                    var sale = _salesService.GetSpecificSale(passenger.Result, date, rab);
                    if (sale == null)
                    {
                        sales.Passengers = peoples;
                    }
                    else
                    {
                        return BadRequest("Venda já foi Cadastrada com esse CPF!");
                    }
                }
            }
            //Busca um voo específico
            var fligth = new ConsumerController().GetFlightAsync(date, rab);
            sales.Flight = fligth.Result;

            //Verifica se a data da venda é a mesma do voo
            if (sales.Flight.Departure == date && sales.Flight.Plane.RAB.Equals(rab))
            {
                //Verifica se a quantidade de vendas é menor que a capacidade da aeronave
                if (sales.Flight.Sales + sales.Passengers.Count <= sales.Flight.Plane.Capacity)
                {
                    sales.Sold = false;
                    sales.Reserved = true;
                    sales.Flight.Sales += sales.Passengers.Count;
                }
                else
                {
                    return BadRequest("Capacidade de Assentos da Aeronave está Esgotado!");
                }
            }
            else
            {
                return BadRequest("Não exite Voo Marcado para essa Data!");
            }
            //Altera a quantidade de vendas realizadas no voo
            var result = new ConsumerController().PutFlightAsync(sales.Flight);
            _salesService.Create(sales);

            return Ok(sales);
        }

        //GET retorna uma venda especifica
        [HttpGet("Sale")]
        public ActionResult<Sales> GetsSpecificSale(string cpf, DateTime date, string rab)
        {

            var passenger = new ConsumerController().GetPassengerAsync(cpf);
            sales.Flight = new ConsumerController().GetFlightAsync(date, rab).Result;

            if (passenger == null && sales.Flight == null)
            {
                return BadRequest("Passageiro ou Voo não foi Encntrado!");

            }
            else
            {
                var sale = _salesService.GetSpecificSale(passenger.Result, date, rab);
                if (sale == null)
                {
                    return BadRequest("Venda não Encontrada!");
                }
                else
                {
                    return Ok(sale);
                }
            }
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Servico;
using System;

namespace ProjetoFinalPromocoes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromocoesController : ControllerBase
    {
        [HttpGet]
        public object Get(int mes)
        {
            var repo = new RepositorioPromocao();

            return new { ProdutoMaisVendido = repo.GetProdutoMaisVendido(mes) };
        }

        [HttpGet("PorMes")]
        public object GetPromocaoPorMes(string produto, int mes)
        {
            if (mes > 12 || mes < 1)
            {
                throw new Exception("Mês deve estar entre 1 e 12.");
            }

            var repo = new RepositorioPromocao();

            return new
            {
                Produto = produto,
                Sugestao = repo.GetSugestaoDePromocao(produto, mes)
            };
        }

        [HttpGet("PorDia")]
        public object GetPromocaoPorDia(string produto, int mes, int dia)
        {

            if (mes > 12 || mes < 1)
            {
                throw new Exception("Mês deve estar entre 1 e 12.");
            }

            if (dia > 31 || dia < 1)
            {
                throw new Exception("Dia deve estar entre 1 e 31.");
            }

            var repo = new RepositorioPromocao();

            return new
            {
                Produto = produto,
                Sugestao = repo.GetSugestaoDePromocao(produto, mes, dia)
            };
        }
    }
}

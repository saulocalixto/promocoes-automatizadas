using Microsoft.AspNetCore.Mvc;
using Servico;

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
            var repo = new RepositorioPromocao();
            return new
            {
                Produto = produto,
                Sugestao = repo.GetSugestaoDePromocao(produto, mes)
            };
        }
    }
}

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
            var repo = new Repositorio();

            return new { ProdutoMaisVendido = repo.Get(mes) };
        }
    }
}

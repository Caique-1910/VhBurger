using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VhBurger.Applications.Services;
using VhBurger.Exceptions;

namespace VhBurger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAlteracaoProdutoController : ControllerBase
    {
        private readonly LogAlteracaoProdutoService _service;

        public LogAlteracaoProdutoController(LogAlteracaoProdutoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Listar()
        {
            return Ok(_service.Listar());
        }

        [HttpGet("produto/{id}")]
        public ActionResult ListarProduto(int id)
        {
            try
            {
                return Ok(_service.ListarPorProduto(id));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

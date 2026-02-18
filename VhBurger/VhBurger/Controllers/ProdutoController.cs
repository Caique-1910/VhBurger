using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VhBurger.Applications.Services;
using VhBurger.DTOs.ProdutoDto;

namespace VhBurger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerProdutoDTO>> Listar() 
        {
            List<LerProdutoDTO> produtos = _service.ListarProdutos();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerProdutoDTO> ObterPorId(int id) 
        {
            LerProdutoDTO produto = _service.ObterPorID(id);

            if (produto == null) 
            { 
                return NotFound();
            }

            return Ok(produto);
        }


    }
}

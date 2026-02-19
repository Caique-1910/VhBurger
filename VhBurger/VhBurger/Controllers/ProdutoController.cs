using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VhBurger.Applications.Services;
using VhBurger.DTOs.ProdutoDto;
using VhBurger.Exceptions;

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

        //autenticação do usuario
        private int ObterUsuarioIdLogado() 
        {
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier); //busca no token o valor armazenado como id usuario

            if (string.IsNullOrWhiteSpace(idTexto)) 
            {
                throw new DomainException("Usuário não autenticado.");
            }

            return int.Parse(idTexto); // converte o valor do id para inteiro e retorna
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

        [HttpGet("{id}/imagens")]
        public ActionResult ObterImagem(int id) 
        {
            try
            { 
                var imagem = _service.ObterImagem(id);
                return File(imagem, "image/jpeg"); // Retorna a imagem como um arquivo JPEG para o navegador
            }
            catch (DomainException ex) 
            {
                return NotFound(ex.Message);
            }
        }
    }
}

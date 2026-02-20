using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Consumes("multipart/form-data")] //Indica que recebe dados no formato multipart/form-data necessario quando enviamos arquivos
        [Authorize] //Exige login para adiocionar produtos
        public ActionResult Adicionar([FromForm] CriarProdutoDTO produtoDto) //Diz que os dados vem do formulario da requisição (multipart/form-data)
        {
            try
            {
                int usuarioId = ObterUsuarioIdLogado(); //Obtem o id do usuario logado
                _service.Adicionar(produtoDto, usuarioId); //Chama o service para adicionar o produto, passando os dados do produto e o id do usuario logado
                return StatusCode(201); //Retorna status 201 Created indicando que o produto foi criado com sucesso

            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public ActionResult Atualizar(int id, [FromForm] AtualizarProdutoDTO produtoDto) 
        {
            try 
            {
                _service.Atualizar(id, produtoDto);
                return NoContent(); //Retorna status 204 No Content indicando que a atualização foi bem sucedida, mas não há conteúdo para retornar
            }
            catch (DomainException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Remover (int id) 
        {
            try
            {
                _service.Remover(id);
                return NoContent(); //Retorna status 204 No Content indicando que a remoção foi bem sucedida, mas não há conteúdo para retornar
            }
            catch (DomainException ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

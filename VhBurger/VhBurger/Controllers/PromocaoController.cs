using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VhBurger.Applications.Services;
using VhBurger.DTOs.PromocaoDTO;
using VhBurger.Exceptions;

namespace VhBurger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocaoController : ControllerBase
    {
        private readonly PromocaoService _service;

        public PromocaoController(PromocaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerPromocaoDTO>> Listar()
        {
            List<LerPromocaoDTO> promocoes = _service.Listar();
            return Ok(promocoes);
        }

        [HttpGet("{id}")]
        public ActionResult<LerPromocaoDTO> ObterPorId(int id)
        {
            try
            {
                LerPromocaoDTO promocao = _service.ObterPorId(id);
                return Ok(promocao);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Adicionar(CriarPromocaoDTO promoDto)
        {
            try
            {
                _service.Adicionar(promoDto);
                return Ok("Promoção adicionada com sucesso.");

            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Atualizar(int id, CriarPromocaoDTO promoDto)
        {
            try
            {
                _service.Atualizar(id, promoDto);
                return NoContent();

            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

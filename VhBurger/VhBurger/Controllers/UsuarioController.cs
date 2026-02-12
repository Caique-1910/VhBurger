using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VhBurger.Applications.Services;
using VhBurger.DTOs;
using VhBurger.Exceptions;

namespace VhBurger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet] //Listar informações
        public ActionResult<List<LerUsuarioDTO>> Listar() 
        {
            List<LerUsuarioDTO> usuarios = _service.Listar();
            return Ok(usuarios); //OK-200 deu bão
        }

        [HttpGet("{id}")]
        public ActionResult<LerUsuarioDTO>ObterPorId(int id)
        {
           LerUsuarioDTO usuario = _service.ObterPorId(id);

           if(usuario == null) 
            {
               return NotFound(); //NotFound-404 não encontrado
            }

            return Ok(usuario);
        }

        [HttpGet("email/{email}")]
        public ActionResult<LerUsuarioDTO> ObterPorEmail(string email) 
        {
            LerUsuarioDTO usuario = _service.ObterPorEmail(email);

            if(usuario == null) 
            {
                return NotFound();
            }

            return Ok(usuario);
        }


        [HttpPost] //Envia dados
        public ActionResult<LerUsuarioDTO> Adicionar(CriarUsuarioDTO usuarioDTO) 
        {
            try 
            {
                LerUsuarioDTO usuarioCriado = _service.Adicionar(usuarioDTO);
                return StatusCode(201, usuarioCriado); //Created-201 criado
            }
            catch (DomainException ex) 
            {
                return BadRequest(ex.Message); //BadRequest-400 deu ruim
            }
        }

        [HttpPut("{id}")] //Atualiza dados
        public ActionResult<LerUsuarioDTO> Atualizar(int id, CriarUsuarioDTO usuarioDTO) 
        {
            try
            {
                LerUsuarioDTO usuarioAtualizado = _service.Atualizar(id, usuarioDTO);
                return StatusCode(200, usuarioAtualizado);
            }
            catch (DomainException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")] //Deleta dados (soft delete, ou seja, somente irá inativa-lo)
        public ActionResult<LerUsuarioDTO> Remover(int id)
        {
            try 
            {
                _service.Remover(id);
                return NoContent(); //NoContent-204 sem conteúdo
            }
            catch (DomainException ex) 
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

using System.Security.Cryptography;
using System.Text;
using VhBurger.Domains;
using VhBurger.DTOs;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    //service concetra o "como fazer"
    public class UsuarioService
    {
        // _repository é o canal para acessar os dados do banco como camada de segurança.
        private readonly IUsuarioRepository _repository;

        // injeção de dependencia
        // implementamos o repositorio e o service so depende da interface
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        private static LerUsuarioDTO LerDTO(Usuario usuario)  //Pega a entidade e gera um DTO
        {
            LerUsuarioDTO lerUsuario = new LerUsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true
            };

            return lerUsuario;
        }

        public List<LerUsuarioDTO> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();

            List<LerUsuarioDTO> usuariosDto = usuarios.Select(usuarioBanco => LerDTO(usuarioBanco)).ToList(); //Select que percorre cada usuaripo e lerdto

            return usuariosDto;
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatória");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDTO ObterPorId(int id)
        {
            Usuario usuario = _repository.ObterPorId(id);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe");
            }

            return LerDTO(usuario);
        }

        public LerUsuarioDTO ObterPorEmail(string email)
        {
            Usuario usuario = _repository.ObterPorEmail(email);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe");
            }

            return LerDTO(usuario);
        }

        public LerUsuarioDTO Adicionar(CriarUsuarioDTO usuarioDTO)
        {
            ValidarEmail(usuarioDTO.Email);
            if (_repository.EmailExiste(usuarioDTO.Email))
            {
                throw new DomainException("Já exite um usuário com este e-mail");
            }

            Usuario usuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Email = usuarioDTO.Email,
                Senha = HashSenha(usuarioDTO.Senha),
                StatusUsuario = true
            };

            _repository.Adicionar(usuario);

            return LerDTO(usuario);
        }

        public LerUsuarioDTO Atualizar(int id, CriarUsuarioDTO usuarioDTO)
        {

            Usuario usuarioBanco = _repository.ObterPorId(id);

            if (usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            ValidarEmail(usuarioDTO.Email);

            Usuario usuarioComMesmoEmail = _repository.ObterPorEmail(usuarioDTO.Email);

            if (usuarioComMesmoEmail != null && usuarioComMesmoEmail.UsuarioID != id)
            {
                throw new DomainException("Já existe um usuario com este email");
            }

            usuarioBanco.Nome = usuarioDTO.Nome;
            usuarioBanco.Email = usuarioDTO.Email;
            usuarioBanco.Senha = HashSenha(usuarioDTO.Senha);

            _repository.Atualizar(usuarioBanco);

            return LerDTO(usuarioBanco);
        }


        public void Remover (int id) 
        { 
            Usuario usuario = _repository.ObterPorId(id);

            if (usuario == null ) 
            {
                throw new DomainException("Usuário não encontrado");
            }

            _repository.Remover(id);
        }

    }
}

using VhBurger.Applications.Autentificacao;
using VhBurger.Domains;
using VhBurger.DTOs.AutenticacaoDTO;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco) // compara a hash SHA256
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDTO Login(LoginDTO loginDto)
        {
            Usuario usuario = _repository.ObterPorEmail(loginDto.Email);

            if (usuario == null)
            {
                throw new DomainException("E-mail ou senha inválidos.");
            }

            if (!VerificarSenha(loginDto.Senha, usuario.Senha))  //comparar a senha digitada com a senha armazenada
            {
                throw new DomainException("E-mail ou senha inválidos.");
            }

            var token = _tokenJwt.GerarToken(usuario); //Gerando o token JWT

            TokenDTO novoToken = new TokenDTO { Token = token };

            return novoToken;
        }

    }
}

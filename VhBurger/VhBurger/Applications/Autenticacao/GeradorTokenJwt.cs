using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VhBurger.Domains;
using VhBurger.Exceptions;

namespace VhBurger.Applications.Autentificacao
{
    public class GeradorTokenJwt
    {
        private readonly IConfiguration _config;

        public GeradorTokenJwt(IConfiguration config) // Recebe as configuracoes do appsettigns.json
        {
            _config = config;
        }

        public string GerarToken(Usuario usuario) 
        {
            var chave = _config["Jwt:Key"]!; // Chave secreta para assinatura do token e garante que o token nao foi alterado

            var issuer = _config["Jwt:Issuer"]!; // Quem gerou o token (nome da api/sistema) a api valida se o token veio do emissor correto

            var audience = _config["Jwt:Audience"]!; // Para quem foi criado o token (nome da api/sistema) define qual sistema pode utilizar o token

            var expiraEmMinutos = int.Parse(_config["Jwt:ExpiraEmMinutos"]); //Tempo de expiracao define quantos minutos sera valido e depois disso o usuario tem logar novamente

            var keyBytes = Encoding.UTF8.GetBytes(chave); //Converte a chave para bytes necessario para criar a assinatura

            if (keyBytes.Length < 32) 
            {
                throw new DomainException("Jwt: Key precisa ter pelo menos 32 caracteres (256 bits).");
            }

            var securityKey = new SymmetricSecurityKey(keyBytes); //Cria a chave de segurança usada para assinar o token

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Define o algoritmo de assinatura do token

            var claims = new List<Claim> //Claims informaçoes do usuario que vao dentro do token e essas informacoes podem ser recuperadas na api para identificar quem esta logando
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()), // Id do usuario para saber quem fez acao

                new Claim(ClaimTypes.Name, usuario.Nome), // Nome do usuario

                new Claim(ClaimTypes.Email, usuario.Email) // Email do usuario
            };

            var token = new JwtSecurityToken(                           // Cria o token jwt com todas as informacoes 
                    issuer: issuer,                                     // quem gerou o tokenm
                    audience: audience,                                 // quem pode usar o token
                    claims: claims,                                     // dados do usuario
                    expires: DateTime.Now.AddMinutes(expiraEmMinutos),  // validade do token
                    signingCredentials: credentials                    //  assinatura de segurança
                );

            return new JwtSecurityTokenHandler().WriteToken(token); // Converte o token para string e essa string é enviada para o cliente
        }


    }
}

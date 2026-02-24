using Microsoft.AspNetCore.Http.HttpResults;
using VhBurger.Applications.Regras;
using VhBurger.Domains;
using VhBurger.DTOs.PromocaoDTO;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    public class PromocaoService
    {
        private readonly IPromocaoRepository _repository;

        public PromocaoService(IPromocaoRepository repository)
        {
            _repository = repository;
        }

        public List<LerPromocaoDTO> Listar()
        {
            List<Promocao> promocoes = _repository.Listar();

            List<LerPromocaoDTO> promocoesDto = promocoes.Select(promocao => new LerPromocaoDTO
            {
                PromocaoID = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao
            }).ToList();

            return promocoesDto;
        }

        public LerPromocaoDTO ObterPorId(int id)
        {
            Promocao promocao = _repository.ObterPorId(id);

            if (promocao == null)
            {
                throw new DomainException("Promoção não encontrada");
            }

            LerPromocaoDTO promocaoDto = new LerPromocaoDTO
            {
                PromocaoID = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao
            };

            return promocaoDto;
        }

        private static void ValidarNome(string nome)
        { 
            if(string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException("Nome é obrigatório.");
            }
        }

     

        public void Adicionar(CriarPromocaoDTO promocaoDto) 
        {
            ValidarNome(promocaoDto.Nome);
            ValidarDataExpiracaoPromocao.ValidarDataExpiracao(promocaoDto.DataExpiracao);

            if (_repository.NomeExiste(promocaoDto.Nome))
            {
                throw new DomainException("Promocao já existente");
            }

            Promocao promocao = new Promocao
            {
                Nome = promocaoDto.Nome,
                DataExpiracao = promocaoDto.DataExpiracao,
                StatusPromocao = promocaoDto.StatusPromocao
            };

            _repository.Adicionar(promocao);
        }

        public void Atualizar(CriarPromocaoDTO promocaoDto)
        {
            ValidarNome(promocaoDto.Nome);
            ValidarDataExpiracaoPromocao.ValidarDataExpiracao(promocaoDto.DataExpiracao);

            if (_repository.NomeExiste(promocaoDto.Nome))
            {
                throw new DomainException("Promocao já existente");
            }

            Promocao promocao = new Promocao
            {
                Nome = promocaoDto.Nome,
                DataExpiracao = promocaoDto.DataExpiracao,
                StatusPromocao = promocaoDto.StatusPromocao
            };

            _repository.Atualizar(promocao);
        }

        public void Remover(int id)
        {

        }
    }
}

using VhBurger.Domains;
using VhBurger.DTOs.CategoriaDTO;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<LerCategoriaDTO> Listar() 
        {
            List<Categoria> categorias = _repository.Listar();

            List<LerCategoriaDTO> categoriasDTO = categorias.Select(categoria => new LerCategoriaDTO
            {
                CategoriaID = categoria.CategoriaID,
                Nome = categoria.Nome
            }).ToList();

            return categoriasDTO;
        }

        public LerCategoriaDTO ObterPorId(int id) 
        {
            Categoria categoria = _repository.ObterPorId(id);

            if(categoria == null) 
            {
                throw new DomainException("Categoria não encontrada.");
            }

            LerCategoriaDTO categoriaDTO = new LerCategoriaDTO
            {
                CategoriaID = categoria.CategoriaID,
                Nome = categoria.Nome
            };

            return categoriaDTO;
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException("Nome é obrigatório.");
            }
        }

        public void Adicionar(CriarCategoriaDTO criarDto) 
        {
            ValidarNome(criarDto.Nome);

            if (_repository.NomeExiste(criarDto.Nome))
            {
                throw new DomainException("Categoria já existente");
            }


            Categoria categoria = new Categoria
            {
                Nome = criarDto.Nome
            };

            _repository.Adicionar(categoria);
        }

        public void Atualizar(int id, CriarCategoriaDTO criarDto) 
        {
            ValidarNome(criarDto.Nome);

            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco == null) 
            {
                throw new DomainException("Categoria não encontrada.");
            }

            if (_repository.NomeExiste(criarDto.Nome, categoriaIdAtual: id))
            {
                throw new DomainException("Categoria já existente");
            }

            categoriaBanco.Nome = criarDto.Nome;
            _repository.Atualizar(categoriaBanco);
        }

        public void Remover(int id) 
        {
            Categoria categoriaBanco = _repository.ObterPorId(id);

            if (categoriaBanco == null) 
            {
                throw new DomainException("Categoria não encontrada.");
            }

            _repository.Remover(id);
        }
    }
}

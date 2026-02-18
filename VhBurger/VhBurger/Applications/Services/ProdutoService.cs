using VhBurger.Applications.Conversoes;
using VhBurger.Applications.Regras;
using VhBurger.Domains;
using VhBurger.DTOs.ProdutoDto;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public List<LerProdutoDTO> ListarProdutos()
        {
            List<Produto> produtos = _repository.Listar();

            List<LerProdutoDTO> produtosDTO = produtos.Select(ProdutoParaDTO.ConverterParaDto).ToList();

            return produtosDTO;
        }

        public LerProdutoDTO ObterPorID(int id)
        {
            Produto produto = _repository.ObterPorId(id);

            if (produto == null)
            {
                throw new Exception("Produto não encontrado.");
            }


            return ProdutoParaDTO.ConverterParaDto(produto);
        }

        private static void ValidarCadastro(CriarProdutoDTO produtoDto)
        {
            if (string.IsNullOrWhiteSpace(produtoDto.Nome))
            {
                throw new DomainException("Nome é obrigatório.");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior que zero.");
            }

            if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
            {
                throw new DomainException("Descrição é obrigatório.");
            }

            if (produtoDto.Imagem == null || produtoDto.Imagem.Length == 0)
            {
                throw new DomainException("Imagem é obrigatória.");
            }

            if (produtoDto.CategoriaIDs == null || produtoDto.CategoriaIDs.Count == 0)
            {
                throw new DomainException("Produto deve ter ao menos uma categoria.");
            }
        }

        public byte[] ObterImagem(int id)
        {
            byte[] imagem = _repository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem não encontrada.");
            }

            return imagem;
        }

        public LerProdutoDTO Adicionar(CriarProdutoDTO produtoDTO, int usuarioID)
        {
            ValidarCadastro(produtoDTO);

            if (_repository.NomeExiste(produtoDTO.Nome))
            {
                throw new DomainException("Já existe um produto com esse nome.");
            }

            Produto produto = new Produto
            {
                Nome = produtoDTO.Nome,
                Preco = produtoDTO.Preco,
                Descricao = produtoDTO.Descricao,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDTO.Imagem),
                StatusProduto = true,
                UsuarioID = usuarioID
            };

            _repository.Adicionar(produto, produtoDTO.CategoriaIDs);

            return ProdutoParaDTO.ConverterParaDto(produto);
        }

        public LerProdutoDTO Atualizar(int id, AtualizarProdutoDTO produtoDto)
        {
            HorarioAlteracaoProduto.ValidarHorario();


            Produto produtoBanco = _repository.ObterPorId(id);

            if (produtoBanco == null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            // : serve para passar o valor do parametro
            if (_repository.NomeExiste(produtoDto.Nome, produtoIdAtual: id))
            {
                throw new DomainException("Já existe outro produto com esse nome.");
            }


            if (produtoDto.CategoriaIDs == null || produtoDto.CategoriaIDs.Count == 0)
            {
                throw new DomainException("Produto deve ter ao menos uma categoria.");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior que zero.");
            }

            produtoBanco.Nome = produtoDto.Nome;
            produtoBanco.Preco = produtoDto.Preco;
            produtoBanco.Descricao = produtoDto.Descricao;

            if (produtoDto.Imagem != null && produtoDto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem);
            }

            if (produtoDto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produtoDto.StatusProduto.Value;
            }

            _repository.Atualizar(produtoBanco, produtoDto.CategoriaIDs);

            return ProdutoParaDTO.ConverterParaDto(produtoBanco);
        }

        public void Remover(int id)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produto = _repository.ObterPorId(id);

            if (produto == null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            _repository.Remover(id);
        }

    }
}

using VhBurger.Domains;
using VhBurger.DTOs.LogProdutoDTO;
using VhBurger.Exceptions;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    public class LogAlteracaoProdutoService
    {
        private readonly ILogAlterecaoProdutoRepository _repository;
        private readonly IProdutoRepository _repositoryP;

        public LogAlteracaoProdutoService(ILogAlterecaoProdutoRepository repository, IProdutoRepository repositoryP)
        {
            _repository = repository;
            _repositoryP = repositoryP;
        }

        public List<LerLogProdutoDTO> Listar() 
        { 
            List<Log_AlteracaoProduto> logs = _repository.Listar();

            List<LerLogProdutoDTO> listLogProduto = logs.Select(log => new LerLogProdutoDTO
            {
                LogID = log.Log_AlteracaoProdutoID,
                ProdutoID = log.ProdutoID,
                NomeAnterior = log.NomeAnterior,
                PrecoAnterior = log.PrecoAnterior,
                DataAlteracao = log.DataAlteracao
            }).ToList();
            
            return listLogProduto;
        }

        public List<LerLogProdutoDTO> ListarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> logs = _repository.ListarPorProduto(produtoId);

            List<LerLogProdutoDTO>? listaLogProduto = logs.Select(log => new LerLogProdutoDTO { LogID = log.Log_AlteracaoProdutoID, ProdutoID = log.ProdutoID, NomeAnterior = log.NomeAnterior, PrecoAnterior = log.PrecoAnterior, DataAlteracao = log.DataAlteracao }).ToList();

            if (listaLogProduto.Count == 0)
            {
                listaLogProduto = null;
                throw new DomainException("não teve alteração");
            }

            return listaLogProduto;
        }

    }
}

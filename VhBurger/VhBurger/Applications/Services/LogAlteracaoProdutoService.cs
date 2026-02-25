using VhBurger.Domains;
using VhBurger.DTOs.LogProdutoDTO;
using VhBurger.Interfaces;

namespace VhBurger.Applications.Services
{
    public class LogAlteracaoProdutoService
    {
        private readonly ILogAlterecaoProdutoRepository _repository;

        public LogAlteracaoProdutoService(ILogAlterecaoProdutoRepository repository)
        {
            _repository = repository;
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

            List<LerLogProdutoDTO> listaLogProduto = logs.Select(log => new LerLogProdutoDTO
            {
                LogID = log.Log_AlteracaoProdutoID,
                ProdutoID = log.ProdutoID,
                NomeAnterior = log.NomeAnterior,
                PrecoAnterior = log.PrecoAnterior,
                DataAlteracao = log.DataAlteracao
            }).ToList();

            return listaLogProduto;
        }
    }
}

using VhBurger.Domains;

namespace VhBurger.Interfaces
{
    public interface ILogAlterecaoProdutoRepository
    {
        List<Log_AlteracaoProduto> Listar();

        List<Log_AlteracaoProduto> ListarPorProduto(int produtoId);
    }
}

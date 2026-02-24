using VhBurger.Domains;

namespace VhBurger.Interfaces
{
    public interface IPromocaoRepository
    {
        List<Promocao> Listar();

        Promocao ObterPorId(int id );

        bool NomeExiste(string nome, int? promocaoIdAtual = null);

        void Adicionar(Promocao promocao);

        void Atualizar(Promocao promocao);

        void Remover(int id );
    }
}

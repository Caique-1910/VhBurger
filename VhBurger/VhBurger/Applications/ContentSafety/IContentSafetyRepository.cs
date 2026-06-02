namespace VhBurger.Applications.ContentSafety
{
    public interface IContentSafetyRepository
    {
        Task<(bool aprovado, string msg)> ValidarConteudo(string texto);
    }
}

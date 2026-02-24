using VhBurger.Exceptions;

namespace VhBurger.Applications.Regras
{
    public class ValidarDataExpiracaoPromocao
    {
        public static void ValidarDataExpiracao(DateTime dataExpiracao)
        {
            if (dataExpiracao <= DateTime.Now)
            {
                throw new DomainException("Data expiração deve ser futura.");
            }
        }
    }
}

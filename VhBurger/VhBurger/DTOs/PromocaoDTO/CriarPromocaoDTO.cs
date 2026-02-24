namespace VhBurger.DTOs.PromocaoDTO
{
    public class CriarPromocaoDTO
    {
        public string Nome { get; set; } = null!;

        public DateTime DataExpiracao { get; set; }

        public bool StatusPromocao { get; set; }
    }
}

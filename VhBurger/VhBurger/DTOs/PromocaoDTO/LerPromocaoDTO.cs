namespace VhBurger.DTOs.PromocaoDTO
{
    public class LerPromocaoDTO
    {
        public int PromocaoID { get; set; }

        public string Nome { get; set; } = null!;

        public DateTime DataExpiracao { get; set; }

        public bool StatusPromocao { get; set; }
    }
}

namespace VhBurger.DTOs.ProdutoDto
{
    public class CriarProdutoDTO
    {
        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public string Descricao { get; set; } = null!;

        public IFormFile Imagem { get; set; } = null!; //A imagem vem via multipart/form-data, ideal para upload de arquivo

        public List<int> CategoriaIDs { get; set; } = new();
    }
}

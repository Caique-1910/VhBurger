using Microsoft.EntityFrameworkCore;
using VhBurger.Contexts;
using VhBurger.Domains;
using VhBurger.Interfaces;

namespace VhBurger.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly VH_BurguerContext _context;

        public ProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Produto> Listar()
        {
            List<Produto> produtos = _context.Produto.Include(produto => produto.Categoria).Include(produto => produto.Usuario).ToList();
            return produtos;
        }

        public Produto ObterPorId(int id)
        {
            Produto? produto = _context.Produto.Include(produtoDb => produtoDb.Categoria).Include(produtoDb => produtoDb.Usuario).FirstOrDefault(produtoDb => produtoDb.ProdutoID == id);
            return produto;
        }

        public bool NomeExiste(string nome, int? produtoIdAtual = null)
        {
            //AsQueryable monta a consulta para executar passo a passo, monta a consulta na tabela produto e não executa nada no banco ainda
            var produtoConsultado = _context.Produto.AsQueryable();

            if (produtoIdAtual.HasValue)
            {
                produtoConsultado = produtoConsultado.Where(produto => produto.ProdutoID != produtoIdAtual.Value);
            }

            return produtoConsultado.Any(produto => produto.Nome == nome);
        }

        public byte[] ObterImagem( int id)
        {
            var produto = _context.Produto.Where(produto => produto.ProdutoID == id).Select(produto => produto.Imagem).FirstOrDefault();
            return produto;
        }

        public void Adicionar(Produto produto, List<int> categoriaIds) 
        {
            List<Categoria> categorias = _context.Categoria.Where(categoria => categoriaIds.Contains(categoria.CategoriaID) ).ToList();
            produto.Categoria = categorias;
            _context.Produto.Add(produto);
            _context.SaveChanges();
        }

        public void Atualizar(Produto produto, List<int> categoriaIds) 
        {
            Produto? produtoBanco = _context.Produto.Include(produto => produto.Categoria).FirstOrDefault(produtoAux => produtoAux.ProdutoID == produto.ProdutoID);

            if (produtoBanco == null) 
            {
                return;
            }
            
            produtoBanco.Nome = produto.Nome;
            produtoBanco.Preco = produto.Preco;
            produtoBanco.Descricao = produto.Descricao;

            if (produto.Imagem != null && produto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = produto.Imagem;
            }

            if (produto.StatusProduto.HasValue) 
            {
                produtoBanco.StatusProduto = produto.StatusProduto;
            }

            var categorias = _context.Categoria.Where(categoria => categoriaIds.Contains(categoria.CategoriaID)).ToList();

            produtoBanco.Categoria.Clear(); // Limpa as categorias atuais do produto e não apaga as categorias do banco, só remove o vinculo entre produto e categoria

            foreach (var categoria in categorias)
            {
                produtoBanco.Categoria.Add(categoria);
            }

            _context.SaveChanges();
        }

        public void Remover(int id) 
        {
            Produto? produto = _context.Produto.FirstOrDefault(produtoAux => produtoAux.ProdutoID == id);

            if (produto == null) 
            {
                return;
            }

            _context.Produto.Remove(produto);
            _context.SaveChanges(); 
        }
    }
}

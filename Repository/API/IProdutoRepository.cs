using System.Collections.Generic;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.API
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(QueryStringParameters produtosParameters);
        IEnumerable<Produto> GetProdutosPorPreco();        
    }
}
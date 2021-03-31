using System.Collections.Generic;
using System.Threading.Tasks;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.API
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(QueryStringParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorPreco();        
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.API
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriaPaginacao(QueryStringParameters parameters);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
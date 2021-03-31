using System.Collections.Generic;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.API
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoriaPaginacao(QueryStringParameters parameters);
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
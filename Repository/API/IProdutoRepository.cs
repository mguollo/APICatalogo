using System.Collections.Generic;
using APICatalogo.Models;

namespace APICatalogo.Repository.API
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();        
    }
}
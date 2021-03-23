using System.Collections.Generic;
using APICatalogo.Models;

namespace APICatalogo.Repository.API
{
    interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();        
    }
}
using System.Collections.Generic;
using APICatalogo.Models;

namespace APICatalogo.Repository.API
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
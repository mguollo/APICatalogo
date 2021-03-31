using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.API;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository.Impl
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {}

        public async Task<PagedList<Categoria>> GetCategoriaPaginacao(QueryStringParameters parameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get().OrderBy(x => x.CategoriaId), 
                parameters.PageNumber, 
                parameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }
    }
}
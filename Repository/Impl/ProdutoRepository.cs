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
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {      
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {            
        }

        public async Task<PagedList<Produto>> GetProdutos(QueryStringParameters produtosParameters)
        {
            return await PagedList<Produto>.ToPagedList(Get().OrderBy(no => no.ProdutoId), 
                produtosParameters.PageNumber, 
                produtosParameters.PageSize);
            
            /*return Get()
                .OrderBy(no => no.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize)
                .ToList();*/
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
    }
}
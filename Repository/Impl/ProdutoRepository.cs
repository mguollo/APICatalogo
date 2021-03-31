using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.API;

namespace APICatalogo.Repository.Impl
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {      
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {            
        }

        public PagedList<Produto> GetProdutos(QueryStringParameters produtosParameters)
        {
            return PagedList<Produto>.ToPagedList(Get().OrderBy(no => no.ProdutoId), 
                produtosParameters.PageNumber, 
                produtosParameters.PageSize);
            
            /*return Get()
                .OrderBy(no => no.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize)
                .ToList();*/
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
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

        public PagedList<Categoria> GetCategoriaPaginacao(QueryStringParameters parameters)
        {
            return PagedList<Categoria>.ToPagedList(
                Get().OrderBy(x => x.CategoriaId), 
                parameters.PageNumber, 
                parameters.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
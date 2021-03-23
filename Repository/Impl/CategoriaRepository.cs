using System.Collections.Generic;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository.API;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository.Impl
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {}

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
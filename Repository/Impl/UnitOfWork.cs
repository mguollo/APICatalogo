using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Repository.API;

namespace APICatalogo.Repository.Impl
{
    public class UnitOfWork : IUnitOfWork
    {

        private ProdutoRepository _produtoRepo;
        private CategoriaRepository _categoriaRepo;
        private AppDbContext _contexto;

        public UnitOfWork(AppDbContext contexto)
        {
            _contexto = contexto;
        }
        public IProdutoRepository ProdutoRepository 
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_contexto);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_contexto);
            }
        }

        public async Task Commit()
        {
            await _contexto.SaveChangesAsync();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }
    }
}
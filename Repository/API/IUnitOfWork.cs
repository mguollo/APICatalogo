using System.Threading.Tasks;

namespace APICatalogo.Repository.API
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task Commit();

    }
}
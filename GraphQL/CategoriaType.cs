using APICatalogo.Models;
using GraphQL.Types;

namespace APICatalogo.GraphQL
{
    //Estamos definindo qual entidade será mapeada para o nosso Type
    public class CategoriaType : ObjectGraphType<Categoria>
    {
        public CategoriaType()
        {
            //campos do Type
            Field(x => x.CategoriaId);
            Field(x => x.Nome);
            Field(x => x.ImageUrl);

            Field<ListGraphType<CategoriaType>>("categorias");
        }
    }
}
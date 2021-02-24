using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _contexto;
        public ProdutosController(AppDbContext contexto)
        {
            _contexto = contexto;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _contexto.Produtos
            .AsNoTracking()
            .ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _contexto.Produtos
                .AsNoTracking()
                .FirstOrDefault(c => c.ProdutoId.Equals(id));

            if  (produto == null)
            {
                return NotFound();
            }
            return produto;
        }
    }
}
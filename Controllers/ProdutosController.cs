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

        [HttpGet("{id}", Name="ObterProduto")]
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

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            _contexto.Produtos.Add(produto);
            _contexto.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", 
                new {id = produto.ProdutoId}, produto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
                //return BadRequest("Código do produto diferente do objeto enviado.");
                return BadRequest(produto);

            _contexto.Entry(produto).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var produto = _contexto.Produtos.FirstOrDefault(c => c.ProdutoId == id);
            var produto = _contexto.Produtos.Find(id);

            if (produto == null)
                return NotFound();

            _contexto.Produtos.Remove(produto);
            _contexto.SaveChanges();
            return Ok();
        }

    }
}
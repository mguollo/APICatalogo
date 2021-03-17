using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Filters;
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
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            return await _contexto.Produtos
            .AsNoTracking()
            .ToListAsync();
        }

        [ServiceFilter(typeof(ApiLoggingFilter2))]
        [HttpGet("{id}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            throw new Exception("Expcetion ao retornar produto pelo ID");
            
            var produto = await _contexto.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProdutoId.Equals(id));

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
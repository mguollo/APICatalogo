using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly AppDbContext _contexto;
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("produtospreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPreco()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _uof.ProdutoRepository.Get().ToList();
        }

        [ServiceFilter(typeof(ApiLoggingFilter2))]
        [HttpGet("{id}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            //throw new Exception("Expcetion ao retornar produto pelo ID");
            
            var produto = _uof.ProdutoRepository.GetById(c => c.ProdutoId.Equals(id));

            if  (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", 
                new {id = produto.ProdutoId}, produto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
                //return BadRequest("Código do produto diferente do objeto enviado.");
                return BadRequest(produto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var produto = _contexto.Produtos.FirstOrDefault(c => c.ProdutoId == id);
            var produto = _uof.ProdutoRepository.GetById(c => c.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return Ok();
        }

    }
}
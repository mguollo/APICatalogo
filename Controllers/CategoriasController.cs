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
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _contexto;

        public CategoriasController(AppDbContext contexto)        
        {
            _contexto = contexto;
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _contexto.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _contexto.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId.Equals(id));
            if (categoria == null)
                return NotFound();

            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria) 
        {
            _contexto.Categorias.Add(categoria);
            _contexto.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new {id = categoria.CategoriaId}, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] Categoria categoria)
        {            
            if (id != categoria.CategoriaId)            
                return BadRequest(categoria);            
            
            _contexto.Entry(categoria).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok();           
        }      

        [HttpDelete("{id}")]     
        public IActionResult Delete(int id)
        {
            var categoria = _contexto.Categorias.FirstOrDefault(c => c.CategoriaId.Equals(id));
            if (categoria == null)
                return NotFound();

            _contexto.Remove(categoria);
            _contexto.SaveChanges();
            return Ok();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _contexto;
        private readonly IConfiguration _configuration;

        public CategoriasController(AppDbContext contexto, IConfiguration config)        
        {
            _contexto = contexto;
            _configuration = config;            
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
           var autor = _configuration["autor"];
           var conexao = _configuration["ConnectionStrings:DefaultConnection"];

           return $"Autor: {autor} Conexão : {conexao}";
        }

        [HttpGet("/TaxaJuros")]
        public ActionResult GetTaxaJuros()
        {   
            var resultado = new {TaxaJuros = 0.01};
            return Ok(resultado);
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico, string nome)
        {
            return meuservico.Saudacao(nome);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _contexto.Categorias.AsNoTracking().Include(x => x.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try{
                return _contexto.Categorias.AsNoTracking().ToList();
            }
            catch (Exception){
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados.");
            }            
        }

        [HttpGet("{id}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _contexto.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId.Equals(id));
                if (categoria == null)
                    return NotFound($"A categoria com id ={id} não foi encontrada");

                return categoria;
            }
            catch (Exception)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados.");
            }            
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
            try
            {
                if (id != categoria.CategoriaId)            
                    return BadRequest($"Não foi possível atualizar a categoria com o Id={id}");
            
                _contexto.Entry(categoria).State = EntityState.Modified;
                _contexto.SaveChanges();
                return Ok($"Categoria com Id={id} foi atualizada com sucesso"); 
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar a categoria do banco de dados.");    
            } 
                      
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
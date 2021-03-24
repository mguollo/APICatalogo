using System;
using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository.API;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //private readonly AppDbContext _contexto;
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork unitOfWork, /*AppDbContext contexto,*/ IConfiguration config, ILogger<CategoriasController> logger)        
        {
            //_contexto = contexto;
            _uof = unitOfWork;
            _configuration = config;            
            _logger = logger;
        }

        [HttpGet("categoriasProduto")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProduto()
        {
            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet("autor")]
        public string GetAutor()        
        {
            _logger.LogInformation("=======================GET API AUTOR=====================================");
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
            _logger.LogInformation("=======================GET api/categorias/produtos=====================================");
            return _uof.CategoriaRepository.Get().Include(x => x.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try{
                //return _contexto.Categorias.AsNoTracking().ToList();
                return _uof.CategoriaRepository.Get().ToList();
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
                var categoria =  _uof.CategoriaRepository.GetById(c => c.CategoriaId.Equals(id));
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
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

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
            
                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();
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
            var categoria =  _uof.CategoriaRepository.GetById(c => c.CategoriaId.Equals(id));
            if (categoria == null)
                return NotFound();

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return Ok();
        }
    }
}
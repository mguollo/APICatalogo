using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.API;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //private readonly AppDbContext _contexto;
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork unitOfWork, IMapper mapper, /*AppDbContext contexto,*/ IConfiguration config, ILogger<CategoriasController> logger)        
        {
            //_contexto = contexto;
            _uof = unitOfWork;
            _mapper = mapper;
            _configuration = config;            
            _logger = logger;
        }

        [HttpGet("categoriasProduto")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProduto()
        {
            var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos();
            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);
            return categoriaDTO;
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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            _logger.LogInformation("=======================GET api/categorias/produtos=====================================");
            var categoria = await _uof.CategoriaRepository.Get().Include(x => x.Produtos).ToListAsync();            
            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);
            return categoriaDTO;
        }

        [HttpGet("categoriaPaginacao")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaPaginacao([FromQuery] QueryStringParameters parameters)
        {
            var categoria = await _uof.CategoriaRepository.GetCategoriaPaginacao(parameters);

            var metadata = new
            {
                categoria.TotalCount,
                categoria.PageSize,
                categoria.CurrentPage,
                categoria.TotalPages,
                categoria.HasNext,
                categoria.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);
            return categoriaDTO;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            try{
                //return _contexto.Categorias.AsNoTracking().ToList();
                var categoria = await _uof.CategoriaRepository.Get().ToListAsync();
                var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);
                return categoriaDTO;
            }
            catch (Exception){
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados.");
            }            
        }

        [HttpGet("{id}", Name="ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId.Equals(id));
                if (categoria == null)
                    return NotFound($"A categoria com id ={id} não foi encontrada");

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDTO;
            }
            catch (Exception)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados.");
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDTO) 
        {
            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            _uof.CategoriaRepository.Add(categoria);
            await _uof.Commit();

            categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new {id = categoria.CategoriaId}, categoriaDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody] CategoriaDTO categoriaDTO)
        {           
            try
            {
                if (id != categoriaDTO.CategoriaId)            
                    return BadRequest($"Não foi possível atualizar a categoria com o Id={id}");
            
                var categoria = _mapper.Map<Categoria>(categoriaDTO);            
                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();
                return Ok($"Categoria com Id={id} foi atualizada com sucesso"); 
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar a categoria do banco de dados.");    
            } 
                      
        }      

        [HttpDelete("{id}")]     
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId.Equals(id));
            if (categoria == null)
                return NotFound();

            _uof.CategoriaRepository.Delete(categoria);
            await _uof.Commit();
            return Ok();
        }
    }
}
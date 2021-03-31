using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.API;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly AppDbContext _contexto;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtospreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPreco()
        {            
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        [HttpGet("paginacao")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPaginacao([FromQuery] QueryStringParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        [ServiceFilter(typeof(ApiLoggingFilter2))]
        [HttpGet("{id}", Name="ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            //throw new Exception("Expcetion ao retornar produto pelo ID");
            
            var produto = _uof.ProdutoRepository.GetById(c => c.ProdutoId.Equals(id));

            if  (produto == null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtosDTO;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtosDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", 
                new {id = produtosDTO.ProdutoId}, produtosDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
                //return BadRequest("Código do produto diferente do objeto enviado.");
                return BadRequest(produtoDTO);


            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            //var produto = _contexto.Produtos.FirstOrDefault(c => c.ProdutoId == id);
            var produto = _uof.ProdutoRepository.GetById(c => c.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDTO);
        }

    }
}
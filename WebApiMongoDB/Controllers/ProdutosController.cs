using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApiMongoDB.Model;
using WebApiMongoDB.Services;

namespace WebApiMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoServices _produtoServices;

        public ProdutosController(ProdutoServices produtoServices)
        {
            _produtoServices = produtoServices;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<Produto>> GetProdutosAsync()
            => await _produtoServices.GetListAsync();

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Produto> GetProdutoByIdAsync(string id)
           => await _produtoServices.GetAsync(id);

        [HttpPost]                
        public async Task<IActionResult> PostProdutoAsync(Produto produto)
        {
            if (produto == null)
                return BadRequest();

            await _produtoServices.CreateAsync(produto);

            return Ok(produto);
        }

        [HttpPut("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProdutoAsync(string id, Produto produto)
        {
            var response = await _produtoServices.GetAsync(id);

            if (response == null)
                return NotFound();

            produto.Id = response.Id;

            var produtoUptade = await _produtoServices.UpdateAsync(id, produto);

            return Ok(produtoUptade);
        }

        [HttpDelete("delete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProdutoAsync(string id)
        {
            if (id == null)
                return BadRequest();

            var responseDeleteProduto = await _produtoServices.GetAsync(id);

            await _produtoServices.RemoveAsync(id);


            return Ok(responseDeleteProduto);
        }

    }
}

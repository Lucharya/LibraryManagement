using LibraryManagement.Core.Entidades;
using LibraryManagement.Service.Communication.Request.Livro;
using LibraryManagement.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LivrosController(ILivroService service) : ControllerBase
    {
        private readonly ILivroService _livroService = service;

        [HttpPost]
        public async Task<ActionResult<Livro>> CriaLivro([FromBody] CriarLivroRequest request)
        {
            var result = await _livroService.CriaLivro(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Livro>>> ListarLivros()
        {
            var result = _livroService.ListarLivros();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> ObterLivroPorId(Guid id)
        {
            var result = _livroService.ObterLivroPorId(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoverLivro(Guid id)
        {
            _livroService.RemoverLivro(id);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Livro>> AtualizarLivro([FromBody] AtualizarLivroRequest request)
        {
            var result = _livroService.AtualizarLivro(request);
            return Ok(result);
        }

    }
}

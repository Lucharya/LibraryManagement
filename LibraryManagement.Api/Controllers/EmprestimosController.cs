using LibraryManagement.Core.Entidades;
using LibraryManagement.Service.Communication.Request.Emprestimo;
using LibraryManagement.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmprestimosController(IEmprestimoService service) : ControllerBase
    {
        private readonly IEmprestimoService _emprestimoService = service;

        [HttpPost]
        public async Task<ActionResult<Emprestimo>> CriaEmprestimo([FromBody] CriarEmprestimoRequest request)
        {
            var result = _emprestimoService.FazEmprestimo(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Emprestimo>>> ListarEmprestimos()
        {
            var result = _emprestimoService.ListaEmprestimos();
            return Ok(result);
        }

        [HttpPost("{id}/devolver")]
        public async Task<ActionResult<Emprestimo>> DevolveUmEmprestimo(Guid id)
        {
            var result = _emprestimoService.DevolveUmEmprestimo(id);
            return Ok(result);
        }

        [HttpGet("membros/list/{membroId}")]
        public async Task<ActionResult<List<Emprestimo>>> ListaEmprestimosPorMembro(Guid membroId)
        {
            var result = _emprestimoService.ListaEmprestimosPorMembro(membroId);
            return Ok(result);
        }

        [HttpGet("livros/list/{livroId}")]
        public async Task<ActionResult<List<Emprestimo>>> ListaEmprestimosPorLivro(Guid livroId)
        {
            var result = _emprestimoService.ListaEmprestimosPorLivro(livroId);
            return Ok(result);
        }

        [HttpGet("atraso")]
        public async Task<ActionResult<List<Emprestimo>>> ListaEmprestimosEmAtraso()
        {
            var result = _emprestimoService.ListaEmprestimosEmAtraso();
            return Ok(result);
        }

    }

}

using LibraryManagement.Core.Entidades;
using LibraryManagement.Service.Communication.Request.Membro;
using LibraryManagement.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MembrosController(IMembroService service) : ControllerBase
    {
        private readonly IMembroService _membroService = service;

        [HttpPost]
        public async Task<ActionResult<Membro>> CriaMembro([FromBody] CriarMembroRequest request)
        {
            var result = _membroService.CriarMembro(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Membro>>> ListarMembros()
        {
            var result = _membroService.ListarMembros();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Membro>> ObterMembroPorId(Guid id)
        {
            var result = _membroService.ObterMembroPorId(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> InativarMembro(Guid id)
        {
            _membroService.InativarMembro(id);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Membro>> AtualizarMembro([FromBody] AtualizarMembroRequest request)
        {
            var result = _membroService.AtualizarMembro(request);
            return Ok(result);
        }

    }
}

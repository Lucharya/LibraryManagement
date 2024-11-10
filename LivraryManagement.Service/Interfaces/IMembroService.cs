using LibraryManagement.Core.Entidades;
using LibraryManagement.Service.Communication.Request.Membro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Interfaces
{
    public interface IMembroService
    {
        Membro CriarMembro(CriarMembroRequest request);
        List<Membro> ListarMembros();
        Membro ObterMembroPorId(Guid id);
        void InativarMembro(Guid id);
        Membro AtualizarMembro(AtualizarMembroRequest request);
    }
}

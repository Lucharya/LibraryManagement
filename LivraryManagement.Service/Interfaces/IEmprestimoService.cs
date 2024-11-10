using LibraryManagement.Core.Entidades;
using LibraryManagement.Service.Communication.Request.Emprestimo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Interfaces
{
    public interface IEmprestimoService
    {
        Emprestimo FazEmprestimo(CriarEmprestimoRequest request);
        List<Emprestimo> ListaEmprestimos();
        Task<Emprestimo> DevolveUmEmprestimo(Guid id);
        List<Emprestimo> ListaEmprestimosPorMembro(Guid membroId);
        List<Emprestimo> ListaEmprestimosPorLivro(Guid livroId);
        List <Emprestimo> ListaEmprestimosEmAtraso();
    }
}

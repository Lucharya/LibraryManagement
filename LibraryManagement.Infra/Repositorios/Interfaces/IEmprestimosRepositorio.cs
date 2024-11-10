using LibraryManagement.Core.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios.Interfaces
{
    public interface IEmprestimosRepositorio : IRepositorioGenerico<Emprestimo>
    {
        List<Emprestimo> ListaEmprestimosEmAtraso();
    }
}

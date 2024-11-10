using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Context;
using LibraryManagement.Infra.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios.Implementacoes
{
    public class EmprestimosRepositorio : RepositorioGenerico<Emprestimo>, IEmprestimosRepositorio
    {
        public EmprestimosRepositorio(LibraryManagementContext dbContext) : base(dbContext) { }
        
        public List<Emprestimo> ListaEmprestimosEmAtraso()
        {
           return _dbContext.Emprestimos.Where(e => e.DataDevolucao < DateTime.Now).ToList();
        }
    }
}

using LibraryManagement.Infra.Context;
using LibraryManagement.Infra.Repositorios.Implementacoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios
{
    public class LibraryManagementRepositorio : ILibraryManagementRepositorio, IDisposable
    {
        private LibraryManagementContext _context { get; set; }
        private readonly IDbContextTransaction _transaction;

        public LibraryManagementRepositorio(LibraryManagementContext context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task CommitTran()
        {
            _context.SaveChangesAsync();
            _transaction.CommitAsync();
        }
        public async Task RollbackTran()
        {
            _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction.Dispose();
        }

        private LivrosRepositorio _livro;
        public LivrosRepositorio Livros
        {
            get { return _livro ?? (_livro = new LivrosRepositorio(_context)); }
        }

        private EmprestimosRepositorio _emprestimo;
        public EmprestimosRepositorio Emprestimos
        {
            get { return _emprestimo ?? (_emprestimo = new EmprestimosRepositorio(_context)); }
        }

        private MembrosRepositorio _membro;
        public MembrosRepositorio Membros
        {
            get { return _membro ?? (_membro = new MembrosRepositorio(_context)); }
        }
    }
}

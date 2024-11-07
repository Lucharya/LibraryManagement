using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Context;
using LibraryManagement.Infra.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios.Implementacoes
{
    public class LivrosRepositorio : RepositorioGenerico<Livro>, ILivrosRepositorio
    {
        public LivrosRepositorio(LibraryManagementContext dbContext) : base(dbContext) { }

        public bool LivroEstaDisponivel(int livroId)
        {
            var livro = GetById(livroId).Result;
            return livro != null && livro.Disponivel;
        }
    }
}

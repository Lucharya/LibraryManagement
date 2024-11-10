using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios.Implementacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios.Interfaces
{
    public interface ILivrosRepositorio : IRepositorioGenerico<Livro>
    {
        bool LivroEstaDisponivel(int livroId);
    }
}

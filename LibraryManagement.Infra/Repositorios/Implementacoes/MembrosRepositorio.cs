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
    public class MembrosRepositorio : RepositorioGenerico<Membro>, IMembrosRepositorio
    {
        public MembrosRepositorio(LibraryManagementContext dbContext) : base(dbContext) { }

        public bool MembroEstaAtivo(Guid membroId)
        {
            return GetById(membroId).Result.Ativo;
        }
    }
}

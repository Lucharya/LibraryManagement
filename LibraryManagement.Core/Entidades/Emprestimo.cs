using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Entidades
{
    public class Emprestimo
    {
        public Guid Id { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public Guid LivroId { get; set; }
        public Livro Livro { get; set; }
        public Guid MembroId { get; set; }
        public Membro Membro { get; set; }
        public bool Devolvido { get; set; }
    }
}

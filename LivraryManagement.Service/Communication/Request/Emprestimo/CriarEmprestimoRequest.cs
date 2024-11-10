using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Communication.Request.Emprestimo
{
    public class CriarEmprestimoRequest
    {
        public Guid LivroId { get; set; }
        public Guid MembroId { get; set; }
        public DateTime DataDevolucao { get; set; }
    }
}

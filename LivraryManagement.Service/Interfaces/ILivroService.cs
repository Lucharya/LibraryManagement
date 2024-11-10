using LibraryManagement.Core.Entidades;
using LibraryManagement.Service.Communication.Request.Livro;

namespace LibraryManagement.Service.Interfaces
{
    public interface ILivroService
    {
        Task<List<Livro>> ListarLivros();
        Task<Livro> CriaLivro(CriarLivroRequest request);
        Task<Livro> ObterLivroPorId(Guid id);
        void RemoverLivro(Guid id);
        Task<Livro> AtualizarLivro(AtualizarLivroRequest request);
    }
}

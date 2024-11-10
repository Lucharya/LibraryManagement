namespace LibraryManagement.Core.Entidades
{
    public class Livro
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public bool Disponivel { get; set; }

        public ICollection<Emprestimo> Emprestimos { get; set; }
    }
}

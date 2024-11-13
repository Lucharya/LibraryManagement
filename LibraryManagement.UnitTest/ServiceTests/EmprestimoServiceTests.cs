using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Service.Communication.Request.Emprestimo;
using LibraryManagement.Service.Interfaces;
using LibraryManagement.Service.Services;
using Moq;

public class EmprestimoServiceTests
{
    private readonly Mock<IEmprestimosRepositorio> _emprestimoRepositorioMock;
    private readonly Mock<ILivrosRepositorio> _livrosRepositorioMock;
    private readonly Mock<IMembrosRepositorio> _membrosRepositorioMock;
    private readonly EmprestimoService _emprestimoService;

    public EmprestimoServiceTests()
    {
        _emprestimoRepositorioMock = new Mock<IEmprestimosRepositorio>();
        _livrosRepositorioMock = new Mock<ILivrosRepositorio>();
        _membrosRepositorioMock = new Mock<IMembrosRepositorio>();

        _emprestimoService = new EmprestimoService(
            _emprestimoRepositorioMock.Object,
            _livrosRepositorioMock.Object,
            _membrosRepositorioMock.Object
        );
    }

    [Fact]
    public async Task DevolveUmEmprestimo_ShouldReturnEmprestimo_WhenEmprestimoAndLivroExist()
    {
        // Arrange
        var emprestimoId = Guid.NewGuid();
        var livroId = Guid.NewGuid();

        var emprestimo = new Emprestimo { Id = emprestimoId, LivroId = livroId, Devolvido = false };
        var livro = new Livro { Id = livroId, Disponivel = false };

        _emprestimoRepositorioMock.Setup(repo => repo.GetById(emprestimoId)).ReturnsAsync(emprestimo);
        _livrosRepositorioMock.Setup(repo => repo.GetById(livroId)).ReturnsAsync(livro);
        _emprestimoRepositorioMock.Setup(repo => repo.Update(It.IsAny<Emprestimo>())).ReturnsAsync(emprestimo);

        // Act
        var result = await _emprestimoService.DevolveUmEmprestimo(emprestimoId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Devolvido);
        Assert.True(livro.Disponivel);

        _livrosRepositorioMock.Verify(repo => repo.Update(livro), Times.Once);
        _emprestimoRepositorioMock.Verify(repo => repo.Update(emprestimo), Times.Once);
        _emprestimoRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Exactly(2));
    }

    [Fact]
    public async Task FazEmprestimo_sholdReturnError_whenLivroEmprestado()
    {

        var expecteLivro= new Livro { Disponivel = false };

        _livrosRepositorioMock.Setup(repo => repo.GetById(It.IsAny<Guid>())).Throws(new Exception("unespected error"));

        var request = new CriarEmprestimoRequest
        {
            LivroId = Guid.NewGuid(),
            MembroId = Guid.NewGuid()
        };

        var exception = Assert.Throws<Exception>(() => _emprestimoService.FazEmprestimo(request));
        Assert.Equal("unespected error", exception.Message);


    }
}

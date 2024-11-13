using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Service.Communication.Request.Livro;
using LibraryManagement.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class LivroServiceTests
{
    private readonly Mock<ILivrosRepositorio> _livrosRepositorioMock;
    private readonly Mock<IEmprestimosRepositorio> _emprestimosRepositorioMock;
    private readonly LivroService _livroService;

    public LivroServiceTests()
    {
        _livrosRepositorioMock = new Mock<ILivrosRepositorio>();
        _emprestimosRepositorioMock = new Mock<IEmprestimosRepositorio>();

        _livroService = new LivroService(
            _livrosRepositorioMock.Object,
            _emprestimosRepositorioMock.Object
        );
    }

    [Fact]
    public async Task CriaLivro_ShouldReturnLivro_WhenLivroIsCreatedSuccessfully()
    {
        // Arrange
        var request = new CriarLivroRequest { Titulo = "Livro novo", Autor = "Nome do Autor" };
        var livro = new Livro { Id = Guid.NewGuid(), Titulo = request.Titulo, Autor = request.Autor, Disponivel = true };

        _livrosRepositorioMock.Setup(repo => repo.Insert(It.IsAny<Livro>())).ReturnsAsync(livro);

        // Act
        var result = await _livroService.CriaLivro(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Titulo, result.Titulo);
        Assert.Equal(request.Autor, result.Autor);
        Assert.True(result.Disponivel);
        _livrosRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarLivros_ShouldReturnListOfLivros()
    {
        // Arrange
        var livros = new List<Livro> { new Livro { Titulo = "Livro 1" }, new Livro { Titulo = "Livro 2" } };
        _livrosRepositorioMock.Setup(repo => repo.GetAll()).Returns(livros);

        // Act
        var result = await _livroService.ListarLivros();

        // Assert
        Assert.Equal(2, result.Count);
        _livrosRepositorioMock.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public async Task ObterLivroPorId_ShouldReturnLivro_WhenLivroExists()
    {
        // Arrange
        var livroId = Guid.NewGuid();
        var livro = new Livro { Id = livroId, Titulo = "Livro Existente" };
        _livrosRepositorioMock.Setup(repo => repo.GetById(livroId)).ReturnsAsync(livro);

        // Act
        var result = await _livroService.ObterLivroPorId(livroId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Livro Existente", result.Titulo);
        _livrosRepositorioMock.Verify(repo => repo.GetById(livroId), Times.Once);
    }

    [Fact]
    public async Task RemoverLivro_ShouldCallRemoveAndCommitTransaction()
    {
        // Arrange
        var livroId = Guid.NewGuid();

        // Act
        _livroService.RemoverLivro(livroId);

        // Assert
        _livrosRepositorioMock.Verify(repo => repo.RemoveAsync(livroId), Times.Once);
        _livrosRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task AtualizarLivro_ShouldReturnUpdatedLivro_WhenLivroIsUpdatedSuccessfully()
    {
        // Arrange
        var livroId = Guid.NewGuid();
        var livro = new Livro { Id = livroId, Titulo = "Titulo Antigo", Autor = "Autor Antigo", Disponivel = false };
        var request = new AtualizarLivroRequest { Id = livroId, Titulo = "Titulo Novo", Autor = "Autor Novo", Disponivel = true };

        _livrosRepositorioMock.Setup(repo => repo.GetById(livroId)).ReturnsAsync(livro);
        _livrosRepositorioMock.Setup(repo => repo.Update(It.IsAny<Livro>())).ReturnsAsync(livro);

        // Act
        var result = await _livroService.AtualizarLivro(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Titulo Novo", result.Titulo);
        Assert.Equal("Autor Novo", result.Autor);
        Assert.True(result.Disponivel);
        _livrosRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Once);
    }

}

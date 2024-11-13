using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Service.Communication.Request.Membro;
using LibraryManagement.Service.Services;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MembroServiceTests
{
    private readonly Mock<IMembrosRepositorio> _membroRepositorioMock;
    private readonly MembroService _membroService;

    public MembroServiceTests()
    {
        _membroRepositorioMock = new Mock<IMembrosRepositorio>();
        _membroService = new MembroService(_membroRepositorioMock.Object);
    }

    [Fact]
    public void CriarMembro_ShouldReturnMembro_WhenMembroIsCreated()
    {
        // Arrange
        var request = new CriarMembroRequest
        {
            Nome = "oreia da rocinha",
            Email = "oreia@gmail.com"
        };

        var membro = new Membro { Nome = request.Nome, Email = request.Email, Ativo = true };

        _membroRepositorioMock.Setup(repo => repo.Insert(It.IsAny<Membro>())).ReturnsAsync(membro);

        // Act
        var result = _membroService.CriarMembro(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("oreia da rocinha", result.Nome);
        Assert.Equal("oreia@gmail.com", result.Email);
        Assert.True(result.Ativo);
        _membroRepositorioMock.Verify(repo => repo.Insert(It.IsAny<Membro>()), Times.Once);
        _membroRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public void ListarMembros_ShouldReturnListOfMembros()
    {
        // Arrange
        var membros = new List<Membro> { new Membro { Nome = "oreia da rocinha" }, new Membro { Nome = "oreia do p.o" } };
        _membroRepositorioMock.Setup(repo => repo.GetAll()).Returns(membros);

        // Act
        var result = _membroService.ListarMembros();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void AtualizarMembro_ShouldReturnUpdatedMembro_WhenMembroExists()
    {
        // Arrange
        var request = new AtualizarMembroRequest { Id = Guid.NewGuid(), Nome = "Oreia de p.o", Email = "oreiapo@gmail.com" };
        var membro = new Membro { Id = request.Id, Nome = "Oreia da rocinha", Email = "oreia@gmail.com" };

        _membroRepositorioMock.Setup(repo => repo.GetById(request.Id)).ReturnsAsync(membro);
        _membroRepositorioMock.Setup(repo => repo.Update(It.IsAny<Membro>())).ReturnsAsync(membro);

        // Act
        var result = _membroService.AtualizarMembro(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Oreia de p.o", result.Nome);
        Assert.Equal("oreiapo@gmail.com", result.Email); 
        _membroRepositorioMock.Verify(repo => repo.Update(It.IsAny<Membro>()), Times.Once);
        _membroRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public void InativarMembro_ShouldSetMembroAtivoToFalse_WhenMembroExists()
    {
        // Arrange
        var membroId = Guid.NewGuid();
        var membro = new Membro { Id = membroId, Nome = "oreia da rocinha", Ativo = true };

        _membroRepositorioMock.Setup(repo => repo.GetById(membroId)).ReturnsAsync(membro);
        _membroRepositorioMock.Setup(repo => repo.Update(membro)).ReturnsAsync(membro);

        // Act
        _membroService.InativarMembro(membroId);

        // Assert
        Assert.False(membro.Ativo);
        _membroRepositorioMock.Verify(repo => repo.Update(It.IsAny<Membro>()), Times.Once);
        _membroRepositorioMock.Verify(repo => repo.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public void ObterMembroPorId_ShouldReturnMembro_WhenMembroExists()
    {
        // Arrange
        var membroId = Guid.NewGuid();
        var membro = new Membro { Id = membroId, Nome = "oreia da rocinha" };

        _membroRepositorioMock.Setup(repo => repo.GetById(membroId)).ReturnsAsync(membro);

        // Act
        var result = _membroService.ObterMembroPorId(membroId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(membroId, result.Id);
        Assert.Equal("oreia da rocinha", result.Nome);
    }
}

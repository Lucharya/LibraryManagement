using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Service.Communication.Request.Livro;
using LibraryManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivrosRepositorio _livroRepositorio;
        private readonly IEmprestimosRepositorio _emprestimosRepositorio;
        public LivroService(ILivrosRepositorio livroRepositorio, IEmprestimosRepositorio emprestimosRepositorio)
        {
            _livroRepositorio = livroRepositorio;
            this._emprestimosRepositorio = emprestimosRepositorio;
        }

        public async Task<Livro> CriaLivro(CriarLivroRequest request)
        {
            try
            {
                var result = _livroRepositorio.Insert(new Livro
                {
                    Titulo = request.Titulo,
                    Autor = request.Autor,
                    Disponivel = true
                }).Result;
                await _livroRepositorio.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Livro>> ListarLivros()
        {
            try
            {
                return _livroRepositorio.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Livro> ObterLivroPorId(Guid id)
        {
            try
            {
                return _livroRepositorio.GetById(id).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async void RemoverLivro(Guid id)
        {
            try
            {
                await _livroRepositorio.RemoveAsync(id);
                await _livroRepositorio.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Livro> AtualizarLivro(AtualizarLivroRequest request)
        {
            try
            {
                var livro = await ObterLivroPorId(request.Id);
                livro.Titulo = request.Titulo;
                livro.Autor = request.Autor;
                livro.Disponivel = request.Disponivel;
                var result = _livroRepositorio.Update(livro).Result;
                await _livroRepositorio.CommitTransactionAsync();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

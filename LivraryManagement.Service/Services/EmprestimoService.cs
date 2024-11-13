using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Service.Communication.Request.Emprestimo;
using LibraryManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private readonly IEmprestimosRepositorio _emprestimoRepositorio;
        private readonly ILivrosRepositorio _livrosRepositorio;
        private readonly IMembrosRepositorio _membrosRepositorio;
        public EmprestimoService(IEmprestimosRepositorio repositorio, ILivrosRepositorio livrosRepositorio, IMembrosRepositorio membrosRepositorio)
        {
            _emprestimoRepositorio = repositorio;
            _livrosRepositorio = livrosRepositorio;
            _membrosRepositorio = membrosRepositorio;
        }

        public async Task<Emprestimo> DevolveUmEmprestimo(Guid id)
        {
            try
            {
                var emprestimo = _emprestimoRepositorio.GetById(id).Result;
                var livroEmprestado = _livrosRepositorio.GetById(emprestimo.LivroId).Result;
                if (emprestimo is not null && livroEmprestado is not null)
                {
                    livroEmprestado.Disponivel = true;
                    _livrosRepositorio.Update(livroEmprestado);
                    _emprestimoRepositorio.CommitTransactionAsync();

                    emprestimo.Devolvido = true;
                    var result = _emprestimoRepositorio.Update(emprestimo).Result;
                    _emprestimoRepositorio.CommitTransactionAsync();
                    return result;
                }
                else
                {
                    await _emprestimoRepositorio.RollbackTransactionAsync();
                    throw new Exception("Emprestimo/Livro não encontrado");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao devolver emprestimo: {ex.Message}");
            }
        }

        public Emprestimo FazEmprestimo(CriarEmprestimoRequest request)
        {
            try
            {
                var livro = _livrosRepositorio.GetById(request.LivroId).Result;
                if (livro is null)
                {
                    throw new Exception("Livro não encontrado");
                }
                if (!livro.Disponivel)
                {
                    throw new Exception("Livro já emprestado");
                }
                var membro = _membrosRepositorio.GetById(request.MembroId).Result;
                if (membro is not null && membro.Ativo)
                {
                    livro.Disponivel = false;
                    _livrosRepositorio.Update(livro);
                    _emprestimoRepositorio.CommitTransactionAsync();

                    var result = _emprestimoRepositorio.Insert(new Emprestimo
                    {
                        LivroId = request.LivroId,
                        MembroId = request.MembroId,
                        DataEmprestimo = DateTime.Now,
                        DataDevolucao = request.DataDevolucao,
                        Devolvido = false
                    }).Result;
                    _emprestimoRepositorio.CommitTransactionAsync();
                    return result;
                }
                else
                {
                    throw new Exception("Membro não encontrado ou inativo");
                }
            }
            catch (Exception ex)
            {
                _emprestimoRepositorio.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }


        public List<Emprestimo> ListaEmprestimos()
        {
            try
            {
                return _emprestimoRepositorio.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Emprestimo> ListaEmprestimosEmAtraso()
        {
            try
            {
                return _emprestimoRepositorio.GetAll().Where(e => e.DataDevolucao < DateTime.Now).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Emprestimo> ListaEmprestimosPorLivro(Guid livroId)
        {
            try
            {
                return _emprestimoRepositorio.GetAll().Where(e => e.LivroId == livroId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Emprestimo> ListaEmprestimosPorMembro(Guid membroId)
        {
            try
            {
                return _emprestimoRepositorio.GetAll().Where(e => e.MembroId == membroId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using LibraryManagement.Core.Entidades;
using LibraryManagement.Infra.Repositorios;
using LibraryManagement.Infra.Repositorios.Interfaces;
using LibraryManagement.Service.Communication.Request.Membro;
using LibraryManagement.Service.Interfaces;

namespace LibraryManagement.Service.Services
{
    public class MembroService : IMembroService
    {
        private readonly IMembrosRepositorio _membroRepositorio;
        public MembroService(IMembrosRepositorio membroRepositorio)
        {
            _membroRepositorio = membroRepositorio;
        }

        public Membro CriarMembro(CriarMembroRequest request)
        {
            try
            {
                var membro = new Membro
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    Ativo = true,
                };

                var result = _membroRepositorio.Insert(membro).Result;
                _membroRepositorio.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Membro> ListarMembros()
        {
            try
            {
                return _membroRepositorio.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Membro AtualizarMembro(AtualizarMembroRequest request)
        {
            try
            {
                var membro = _membroRepositorio.GetById(request.Id).Result;

                if (membro != null)
                {
                    membro.Nome = request.Nome;
                    membro.Email = request.Email;

                    var result = _membroRepositorio.Update(membro).Result;
                    _membroRepositorio.CommitTransactionAsync();
                    return result;
                }
                else throw new Exception("Membro não encontrado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void InativarMembro(Guid id)
        {
            try
            {
                var membro = _membroRepositorio.GetById(id).Result;

                if (membro != null)
                {
                    membro.Ativo = false;
                    _membroRepositorio.Update(membro);
                    _membroRepositorio.CommitTransactionAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Membro ObterMembroPorId(Guid id)
        {
            try
            {
                return _membroRepositorio.GetById(id).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

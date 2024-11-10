using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios.Interfaces
{
    public interface IRepositorioGenerico<T> where T : class
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        IEnumerable<T> GetPage(
               Expression<Func<T, bool>> filter = null,
               Expression<Func<T, object>> orderByDescending = null,
               Expression<Func<T, object>> orderBy = null,
               string includeProperties = "",
               int page = 1,
               int take = 50);
        List<T> GetAll();
        IQueryable<T> GetAllQueryable();
        List<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetById(int id);
        Task<T> GetById(Guid id);
        T GetByID(object id, string[] includeProperties);
        Task<T> Insert(T entidade);
        Task Insert(List<T> entidades);
        void Remove(int id);
        Task RemoveAsync(int id);
        Task RemoveAsync(Guid id);
        Task Remove(T entidade);
        Task<T> Update(T entidade);
        Task<T> Update(int id, T entidade);
        Task<T> Update(Guid id, T entidade);
        Task Update(List<T> entidades);
        int Count();
        int Count(Expression<Func<T, bool>> filter);
        IQueryable<T> ObterPorExpression(Expression<Func<T, bool>> filter);
        IEnumerable<TResult> GetPageLazy<TResult, TEntity>(
           Expression<Func<T, TResult>> selector,
           Expression<Func<T, bool>> filter = null,
           Expression<Func<T, T>> orderBy = null,
           string includeProperties = "",
           int page = 1,
           int take = 50);
    }  
}

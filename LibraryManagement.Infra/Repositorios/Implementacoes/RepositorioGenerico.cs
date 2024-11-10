using LibraryManagement.Infra.Context;
using LibraryManagement.Infra.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Repositorios.Implementacoes
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        public LibraryManagementContext _dbContext { get; set; }
        private IDbContextTransaction _transaction;
        public RepositorioGenerico(LibraryManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
        public virtual IEnumerable<T> GetPage(
                       Expression<Func<T, bool>> filter = null,
                       Expression<Func<T, object>> orderByDescending = null,
                       Expression<Func<T, object>> orderBy = null,
                       string includeProperties = "",
        int page = 1,
                       int take = 50)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var skip = page <= 1 ? 0 : (page - 1) * take;
            if (orderByDescending != null)
            {
                query = query.OrderByDescending(orderByDescending).Skip(skip).Take(take);
                return query.AsNoTracking().ToList();
            }
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).Skip(skip).Take(take);
                return query.AsNoTracking().ToList();
            }
            else
            {
                query = query.Skip(skip).Take(take);
                return query.AsNoTracking().ToList();
            }
        }
        public List<T> GetAll()
        {
            IQueryable<T> result = _dbContext.Set<T>();
            return result.ToList();
        }
        public IQueryable<T> GetAllQueryable()
        {
            IQueryable<T> result = _dbContext.Set<T>();
            return result;
        }
        public List<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }
        public virtual async Task<T> GetById(int id)
        {
            return await _dbContext
                .Set<T>()
                .FindAsync(id);
        }
        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbContext
                .Set<T>()
                .FindAsync(id);
        }
        public virtual T GetByID(object id, string[] includeProperties)
        {
            var keyProperty = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            var parameter = Expression.Parameter(typeof(T), "e");
            var key = Expression.PropertyOrField(parameter, keyProperty?.Name ?? "Id");
            var value = Convert.ChangeType(id, keyProperty?.ClrType ?? typeof(int));
            var predicate = Expression.Lambda<Func<T, bool>>(Expression.Equal(key, Expression.Constant(value)), parameter);



            var query = _dbContext.Set<T>().AsQueryable();



            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }



            return query.FirstOrDefault(predicate);
        }

        public async Task<T> Insert(T entidade)
        {
            await _dbContext.Set<T>().AddAsync(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }

        public async Task Insert(List<T> entidades)
        {
            entidades.ForEach(async e => { await _dbContext.Set<T>().AddAsync(e); });
            await _dbContext.SaveChangesAsync();
        }

        public async void Remove(int id)
        {
            var entidade = await this.GetById(id);
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entidade = await this.GetById(id);
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveAsync(Guid id)
        {
            var entidade = await this.GetById(id);
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(T entidade)
        {
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Update(T entidade)
        {
            _dbContext.Update(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }

        public async Task<T> Update(int id, T entidade)
        {
            var entidadeExistente = await _dbContext
            .Set<T>()
            .FindAsync(id);

            _dbContext.Entry(entidadeExistente).CurrentValues.SetValues(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }
        public async Task<T> Update(Guid id, T entidade)
        {
            var entidadeExistente = await _dbContext
            .Set<T>()
            .FindAsync(id);

            _dbContext.Entry(entidadeExistente).CurrentValues.SetValues(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade;
        }

        public async Task Update(List<T> entidades)
        {
            entidades.ForEach(e => { _dbContext.Update(e); });
            await _dbContext.SaveChangesAsync();
        }
        public int Count()
        {
            var quantidade = _dbContext.Set<T>().Count();
            return quantidade;
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            var quantidade = _dbContext.Set<T>().Count(filter);
            return quantidade;
        }

        public IQueryable<T> ObterPorExpression(Expression<Func<T, bool>> filter)
        {
            var entidadeExistente = _dbContext
                .Set<T>()
                .Where(filter);

            return entidadeExistente;
        }

        public virtual IEnumerable<TResult> GetPageLazy<TResult, TEntity>(
           Expression<Func<T, TResult>> selector,
           Expression<Func<T, bool>> filter = null,
           Expression<Func<T, T>> orderBy = null,
           string includeProperties = "",
           int page = 1,
           int take = 50)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var skip = page <= 1 ? 0 : (page - 1) * take;
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy).Skip(skip).Take(take);
                return query.AsNoTracking().Select(selector);
            }
            else
            {
                query = query.Skip(skip).Take(take);
                return query.AsNoTracking().Select(selector);
            }
        }
    }
}

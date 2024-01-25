using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Create(TEntity entity);
    IEnumerable<TEntity> GetAll();
    TEntity GetOne(Expression<Func<TEntity, bool>> predication);
    TEntity Update(TEntity entity);
    bool Delete(Expression<Func<TEntity, bool>> predicate);
    bool Exists(Expression<Func<TEntity, bool>> predicate);
}

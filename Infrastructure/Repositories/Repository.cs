using Infrastructure.Contexts;
using Infrastructure.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class Repository<TEntity, TContext> :IRepository<TEntity> where TEntity : class where TContext : class
{
    private readonly ContactContext _contactContext;

    protected Repository(ContactContext contactContext)
    {
        _contactContext = contactContext;
    }

    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            _contactContext.Set<TEntity>().Add(entity);
            _contactContext.SaveChanges();
            return entity;
        }
        catch(Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        try
        {
            var result = _contactContext.Set<TEntity>().ToList();
            if (result != null)
                return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = _contactContext.Set<TEntity>().FirstOrDefault(predicate);
            if (result != null)
                return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public virtual TEntity Update(TEntity entity)
    {
        try
        {
            var updatedEntity = _contactContext.Set<TEntity>().Find(entity);
            if (updatedEntity != null)
            {
                _contactContext.Set<TEntity>().Update(updatedEntity);
                _contactContext.SaveChanges();

                return updatedEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _contactContext.Set<TEntity>().FirstOrDefault(predicate);
            if (entity != null)
            {
                _contactContext.Set<TEntity>().Remove(entity);
                _contactContext.SaveChanges();

                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = _contactContext.Set<TEntity>().Any(predicate);
            return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}

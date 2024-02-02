using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Created a new entity in the repository
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <returns>The created entity</returns>
    TEntity Create(TEntity entity);

    /// <summary>
    /// Gets all entities in the repository
    /// </summary>
    /// <returns>Returns all entities</returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>
    /// Retrieves one entity from the repository based on the provided predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>The first entity that satisfies the condition specified by the predicate</returns>
    TEntity GetOne(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <returns>A boolean indicating whether the update was successful</returns>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Deletes one entity from the repository based on the provided predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>A boolean indicating whether the deletion was successful</returns>
    bool Delete(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Checks if the entity exists in the repository based on the provided predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>A boolean indicating whether the deletion was successful</returns>
    bool Exists(Expression<Func<TEntity, bool>> predicate);
}

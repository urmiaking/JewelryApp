using System.Linq.Expressions;
using JewelryApp.Core.DomainModels;

namespace JewelryApp.Core.Interfaces.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> Get(bool asNoTracking = true, bool retrieveDeletedRecords = false, bool useAuthentication = true);
    void Add(TEntity entity, bool saveNow = true, bool useAuthentication = true);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true);
    void AddRange(IEnumerable<TEntity> entities, bool saveNow = true, bool useAuthentication = true);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true);
    void Attach(TEntity entity);
    void Delete(TEntity entity, bool saveNow = true, bool deletePermanently = false);
    void UndoDelete(TEntity entity, bool saveNow = true);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true, bool deletePermanently = false);
    Task UndoDeleteAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true);
    void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true, bool deletePermanently = false);
    void UndoDeleteRange(IEnumerable<TEntity> entities, bool saveNow = true);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true, bool deletePermanently = false);
    Task UndoDeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true);
    void Detach(TEntity entity);
    TEntity? GetById(params object[] ids);
    ValueTask<TEntity?> GetByIdAsync(object? id, CancellationToken cancellationToken = default);
    void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty) where TProperty : class;
    Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken) where TProperty : class;
    void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty) where TProperty : class;
    Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken = default) where TProperty : class;
    void Update(TEntity entity, bool saveNow = true, bool useAuthentication = true);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true, bool useAuthentication = true);
    void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true, bool useAuthentication = true);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true);
}

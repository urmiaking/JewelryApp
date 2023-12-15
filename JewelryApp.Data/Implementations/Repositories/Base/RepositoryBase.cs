﻿using System.Linq.Expressions;
using System.Threading;
using JewelryApp.Core.Constants;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces.Repositories.Base;
using JewelryApp.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories.Base;

public class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly AppDbContext DbContext;
    public DbSet<TEntity> Entities { get; }
    public virtual IQueryable<TEntity> Table => Entities;
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;

    public RepositoryBase(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        DbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        Entities = DbContext.Set<TEntity>();
    }

    #region Async Method
    public virtual ValueTask<TEntity?> GetByIdAsync(object? id, CancellationToken cancellationToken = default)
    {
        return Entities.FindAsync(id, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteModelBase)
        {
            softDeleteModelBase.CreatedAt = DateTime.Now;
            softDeleteModelBase.ModifiedUserId = useAuthentication ? await GetUserIdAsync() : null;
        }

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));

        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteModelBase)
            {
                softDeleteModelBase.CreatedAt = DateTime.Now;
                softDeleteModelBase.ModifiedUserId = useAuthentication ? await GetUserIdAsync() : null;
            }
        }

        await Entities.AddRangeAsync(entitiesList, cancellationToken).ConfigureAwait(false);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteModelBase)
        {
            softDeleteModelBase.CreatedAt = DateTime.Now;
            softDeleteModelBase.ModifiedUserId = useAuthentication ? await GetUserIdAsync() : null;
        }

        Entities.Update(entity);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true, bool useAuthentication = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));

        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteModelBase)
            {
                softDeleteModelBase.CreatedAt = DateTime.Now;
                softDeleteModelBase.ModifiedUserId = useAuthentication ? await GetUserIdAsync() : null;
            }
        }

        Entities.UpdateRange(entitiesList);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteEntity)
        {
            softDeleteEntity.Deleted = true;
            Entities.Update(entity);
        }
        else
        {
            Entities.Remove(entity);
        }

        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UndoDeleteAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteEntity)
        {
            softDeleteEntity.Deleted = false;
            Entities.Update(entity);
        }

        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));

        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteEntity)
            {
                softDeleteEntity.Deleted = true;
                Entities.Update(entity);
            }
            else
            {
                Entities.Remove(entity);
            }
        }

        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UndoDeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));

        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteEntity)
            {
                softDeleteEntity.Deleted = false;
                Entities.Update(entity);
            }
        }

        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Sync Methods
    public virtual TEntity? GetById(params object[] ids)
    {
        return Entities.Find(ids);
    }

    public virtual void Add(TEntity entity, bool saveNow = true, bool useAuthentication = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteModelBase)
        {
            softDeleteModelBase.CreatedAt = DateTime.Now;
            softDeleteModelBase.ModifiedUserId = useAuthentication ? GetUserId() : null;
        }

        Entities.Add(entity);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true, bool useAuthentication = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));

        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteModelBase)
            {
                softDeleteModelBase.CreatedAt = DateTime.Now;
                softDeleteModelBase.ModifiedUserId = useAuthentication ? GetUserId() : null;
            }
        }

        Entities.AddRange(entitiesList);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Update(TEntity entity, bool saveNow = true, bool useAuthentication = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteModelBase)
        {
            softDeleteModelBase.CreatedAt = DateTime.Now;
            softDeleteModelBase.ModifiedUserId = useAuthentication ? GetUserId() : null;
        }

        Entities.Update(entity);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true, bool useAuthentication = true)
    {
        var entitiesList = entities as TEntity[] ?? entities.ToArray();
        Assert.NotNull(entitiesList, nameof(entities));

        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteModelBase)
            {
                softDeleteModelBase.CreatedAt = DateTime.Now;
                softDeleteModelBase.ModifiedUserId = useAuthentication ? GetUserId() : null;
            }
        }

        Entities.UpdateRange(entitiesList);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Delete(TEntity entity, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));
            
        if (entity is SoftDeleteModelBase softDeleteEntity)
        {
            softDeleteEntity.Deleted = true;
            Entities.Update(entity);
        }
        else
        {
            Entities.Remove(entity);
        }

        if (saveNow)
            DbContext.SaveChanges();
    }

    public void UndoDelete(TEntity entity, bool saveNow = true)
    {
        Assert.NotNull(entity, nameof(entity));

        if (entity is SoftDeleteModelBase softDeleteEntity)
        {
            softDeleteEntity.Deleted = false;
            Entities.Update(entity);
        }

        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));
        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteEntity)
            {
                softDeleteEntity.Deleted = true;
                Entities.Update(entity);
            }
            else
            {
                Entities.Remove(entity);
            }
        }

        //Entities.RemoveRange(entitiesList);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public void UndoDeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
    {
        var entitiesList = entities.ToList();
        Assert.NotNull(entitiesList, nameof(entities));
        foreach (var entity in entitiesList)
        {
            if (entity is SoftDeleteModelBase softDeleteEntity)
            {
                softDeleteEntity.Deleted = false;
                Entities.Update(entity);
            }
        }

        if (saveNow)
            DbContext.SaveChanges();
    }
    #endregion

    #region Attach & Detach
    public virtual void Detach(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        var entry = DbContext.Entry(entity);
        entry.State = EntityState.Detached;
    }

    public virtual void Attach(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        if (DbContext.Entry(entity).State == EntityState.Detached)
            Entities.Attach(entity);
    }
    #endregion

    #region Explicit Loading
    public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);

        var collection = DbContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class
    {
        Attach(entity);
        var collection = DbContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            collection.Load();
    }

    public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);
        var reference = DbContext.Entry(entity).Reference(referenceProperty!);
        if (!reference.IsLoaded)
            await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
        where TProperty : class
    {
        Attach(entity);
        var reference = DbContext.Entry(entity).Reference(referenceProperty!);
        if (!reference.IsLoaded)
            reference.Load();
    }
    #endregion

    #region Common Methods

    protected async Task<Guid?> GetUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

        return user?.Id;
    }

    protected Guid? GetUserId()
    {
        var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User).GetAwaiter().GetResult();

        return user?.Id;
    }

    protected async Task<bool> IsAdminAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
        if (user is null)
            return false;
        
        return await _userManager.IsInRoleAsync(user, Data.Identity.AdminRole);
    }

    #endregion
}

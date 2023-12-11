using JewelryApp.Core.DomainModels.Identity;

namespace JewelryApp.Core.DomainModels;

public interface IEntity
{

}

public abstract class ModelBase : IEntity
{
    public DateTime? CreatedAt { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public AppUser? ModifiedUser { get; set; }
}

public abstract class ModelBase<TId> : ModelBase
{
    public virtual TId Id { get; set; } = default!;
}

public abstract class SoftDeleteModelBase<TId> : ModelBase<TId>
{
    public virtual bool Deleted { get; set; }
}

public abstract class SoftDeleteModelBase : SoftDeleteModelBase<int>
{

}
namespace JewelryApp.Data.Models;

public abstract class ModelBase
{

}

public abstract class ModelBase<TId> : ModelBase
{
    public TId Id { get; set; } = default!;
}
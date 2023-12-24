using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<bool> CheckCustomerExistsAsync(Customer customer, CancellationToken cancellationToken = default);
}

using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<ICustomerRepository>]
public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext dbContext, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager) 
        : base(dbContext, elevatedAccessService, userManager)
    {
    }

    public async Task<bool> CheckCustomerExistsAsync(Customer customer, CancellationToken cancellationToken = default)
        => await Get().AnyAsync(x => x.PhoneNumber == customer.PhoneNumber, cancellationToken);
}

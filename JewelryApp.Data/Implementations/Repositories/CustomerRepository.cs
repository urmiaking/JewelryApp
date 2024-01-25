using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using JewelryApp.Shared.Attributes;
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
        => await Get().AnyAsync(x => x.NationalCode == customer.NationalCode, cancellationToken);

    public async Task<Customer?> GetByPhoneNumber(string? phoneNumber, CancellationToken cancellationToken = default)
        => await Get().FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);

    public async Task<Customer?> GetByNationalCode(string? nationalCode, CancellationToken cancellationToken = default)
        => await Get().FirstOrDefaultAsync(x => x.NationalCode == nationalCode, cancellationToken);
}

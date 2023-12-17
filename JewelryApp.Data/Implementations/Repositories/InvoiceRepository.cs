using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IInvoiceRepository>]
public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager, AppDbContext context)
        : base(context, elevatedAccessService, userManager)
    {
    }
}

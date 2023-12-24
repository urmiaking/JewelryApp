using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IInvoiceRepository>]
public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager, AppDbContext context)
        : base(context, elevatedAccessService, userManager)
    {
    }

    public async Task<bool> CheckInvoiceExistsAsync(int invoiceNumber, CancellationToken cancellationToken = default)
        => await Get().AnyAsync(x => x.InvoiceNumber == invoiceNumber, cancellationToken);
}

using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using JewelryApp.Shared.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IInvoiceRepository>]
public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
{
    private readonly IElevatedAccessService _elevatedAccessService;
    public InvoiceRepository(IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager, AppDbContext context)
        : base(context, elevatedAccessService, userManager)
    {
        _elevatedAccessService = elevatedAccessService;
    }

    public async Task<bool> CheckInvoiceExistsAsync(int invoiceNumber, CancellationToken cancellationToken = default)
        => await Get().AnyAsync(x => x.InvoiceNumber == invoiceNumber &&
            x.ModifiedUserId == _elevatedAccessService.GetUserId(), cancellationToken);

    public async Task<int> GetLastSavedInvoiceNumberAsync(CancellationToken cancellationToken = default)
    {
        var invoice = await Get().OrderByDescending(x => x.InvoiceNumber)
            .FirstOrDefaultAsync(x => x.ModifiedUserId == _elevatedAccessService.GetUserId(), cancellationToken);

        return invoice?.InvoiceNumber ?? 1;
    }
}

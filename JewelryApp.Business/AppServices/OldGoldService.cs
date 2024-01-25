using AutoMapper;
using ErrorOr;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Core.Interfaces.Repositories.Base;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Errors;
using JewelryApp.Shared.Requests.OldGolds;
using JewelryApp.Shared.Responses.OldGolds;

namespace JewelryApp.Application.AppServices;

[ScopedService<IOldGoldService>]
public class OldGoldService : IOldGoldService
{
    private readonly IRepository<OldGold> _repository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public OldGoldService(IRepository<OldGold> repository, IInvoiceRepository invoiceRepository, IMapper mapper)
    {
        _repository = repository;
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AddOldGoldResponse>> AddOldGoldAsync(AddOldGoldRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        var oldGold = _mapper.Map<OldGold>(request);

        await _repository.AddAsync(oldGold, cancellationToken);

        return new AddOldGoldResponse(oldGold.Id);
    }

    public async Task<ErrorOr<RemoveOldGoldResponse>> RemoveOldGoldAsync(int id, bool deletePermanently = false, CancellationToken cancellationToken = default)
    {
        var oldGold = await _repository.GetByIdAsync(id, cancellationToken);

        if (oldGold is null)
            return Errors.OldGolds.NotFound;

        if (oldGold.Deleted && !deletePermanently)
            return Errors.OldGolds.Deleted;
        
        await _repository.DeleteAsync(oldGold, cancellationToken, deletePermanently: deletePermanently);

        return new RemoveOldGoldResponse(id);
    }
}
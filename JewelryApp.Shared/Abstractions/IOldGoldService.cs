using ErrorOr;
using JewelryApp.Shared.Requests.OldGolds;
using JewelryApp.Shared.Responses.OldGolds;

namespace JewelryApp.Shared.Abstractions;

public interface IOldGoldService
{
    Task<ErrorOr<AddOldGoldResponse>> AddOldGoldAsync(AddOldGoldRequest request, CancellationToken cancellationToken = default);
}

using Wasm.Models;

namespace Wasm.Services;

public interface IPurchaseOrderService
{
    public Task<List<PurchaseOrder>> GetPurchaseOrdersAsync();
}
using Wasm.Models;

namespace Wasm.Services;

public interface IPurchaseOrderService
{
    public Task<PurchaseOrder> GetPurchaseOrderAsync(int poNumber);

    public Task<List<PurchaseOrder>> GetPurchaseOrdersAsync();
}
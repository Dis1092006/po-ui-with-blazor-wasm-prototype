using Newtonsoft.Json;
using Wasm.Models;

namespace Wasm.Services;

public class PurchaseOrdersDto
{
    public string PoDate { get; set; }
    public string SupplierId { get; set; }
}

public class PurchaseOrderData
{
    public string Type { get; set; }
    public string Id { get; set; }
    public PurchaseOrdersDto Attributes { get; set; }
}

public class PurchaseOrdersResponse
{
    public PurchaseOrderData[] Data { get; set; }
}

class PurchaseOrderService : IPurchaseOrderService
{
    private readonly HttpClient _client;

    public PurchaseOrderService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<PurchaseOrder>> GetPurchaseOrdersAsync()
    {
        var result = new List<PurchaseOrder>();
        
        var response = await _client.GetAsync("purchaseorders");
        var json = await response.Content.ReadAsStringAsync();
        
        var content = JsonConvert.DeserializeObject<PurchaseOrdersResponse>(json);
        
        foreach (var purchaseOrderData in content.Data)
        {
            var po = new PurchaseOrder
            {
                PoNumber = int.Parse(purchaseOrderData.Id),
                SupplierId = int.Parse(purchaseOrderData.Attributes.SupplierId),
                PoDate = DateTime.Parse(purchaseOrderData.Attributes.PoDate),
                Items = new List<PurchaseOrderItem>()
            };
            result.Add(po);
        }

        return result;
    }
}
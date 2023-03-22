using Newtonsoft.Json;
using Wasm.Models;

namespace Wasm.Services;

public class PurchaseOrdersDto
{
    public string PoDate { get; set; }
    public string SupplierId { get; set; }
    public string Terms { get; set; }
}

public class PoHistoryDto
{
    public string PoNumber { get; set; }
    public int ItemNumber { get; set; }
    public string LocalSku { get; set; }
    public int Quantity { get; set; }
    public float Cost { get; set; }
}

public class PurchaseOrderData
{
    public string Type { get; set; }
    public string Id { get; set; }
    public PurchaseOrdersDto Attributes { get; set; }
}

public class PurchaseOrderIncluded
{
    public string Type { get; set; }
    public string Id { get; set; }
    public PoHistoryDto Attributes { get; set; }
}

public class PurchaseOrderResponse
{
    public PurchaseOrderData Data { get; set; }
    public PurchaseOrderIncluded[] Included { get; set; }
}

public class PoHistoryData
{
    public string Type { get; set; }
    public string Id { get; set; }
    public PoHistoryDto Attributes { get; set; }
}

public class PoHistoryResponse
{
    public PoHistoryData[] Data { get; set; }
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

    public async Task<PurchaseOrder> GetPurchaseOrderAsync(int poNumber)
    {
        var result = new List<PurchaseOrder>();
        
        var poResponse = await _client.GetAsync($"purchaseOrders/{poNumber}");
        var jsonPoResponse = await poResponse.Content.ReadAsStringAsync();
        var contentPoResponse = JsonConvert.DeserializeObject<PurchaseOrderResponse>(jsonPoResponse);
        
        var po = new PurchaseOrder
        {
            PoNumber = int.Parse(contentPoResponse.Data.Id),
            SupplierId = int.Parse(contentPoResponse.Data.Attributes.SupplierId),
            PoDate = DateTime.Parse(contentPoResponse.Data.Attributes.PoDate),
            Terms = contentPoResponse.Data.Attributes.Terms,
            Items = new List<PurchaseOrderItem>()
        };

        var poHistoryResponse = await _client.GetAsync($"poHistory?filter=equals(poNumber,'{poNumber}')");
        var jsonPoHistoryResponse = await poHistoryResponse.Content.ReadAsStringAsync();
        var contentPoHistoryResponse = JsonConvert.DeserializeObject<PoHistoryResponse>(jsonPoHistoryResponse);

        if (contentPoHistoryResponse?.Data != null)
            foreach (var item in contentPoHistoryResponse.Data)
            {
                po.Items.Add(new PurchaseOrderItem
                {
                    PoNumber = int.Parse(item.Id),
                    ItemNumber = item.Attributes.ItemNumber,
                    LocalSku = item.Attributes.LocalSku,
                    Quantity = item.Attributes.Quantity,
                    Cost = item.Attributes.Cost
                });
            }

        return po;
    }

    public async Task<List<PurchaseOrder>> GetPurchaseOrdersAsync()
    {
        var result = new List<PurchaseOrder>();
        
        var response = await _client.GetAsync("purchaseorders?sort=-id&page[size]=10");
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
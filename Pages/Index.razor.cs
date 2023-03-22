using Microsoft.AspNetCore.Components;
using Wasm.Models;
using Wasm.Services;

namespace Wasm.Pages;

public partial class Index
{
    [Inject]
    public IPurchaseOrderService PurchaseOrderService { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    
    public List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    protected override async Task OnInitializedAsync()
    {
        PurchaseOrders = await PurchaseOrderService.GetPurchaseOrdersAsync();
    }
    
    private void ViewPoClicked(PurchaseOrder purchaseOrder)
    {
        NavigationManager.NavigateTo($"/purchase-order/{purchaseOrder.PoNumber}");
    }
}
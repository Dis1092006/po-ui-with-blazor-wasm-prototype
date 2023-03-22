using Microsoft.AspNetCore.Components;
using Wasm.Models;
using Wasm.Services;

namespace Wasm.Pages;

public partial class PurchaseOrderPage
{
    private int PoNumber { get; set; }
    
    private PurchaseOrder? PurchaseOrder { get; set; }

    [Parameter]
    public string Id
    {
        get => PoNumber.ToString();
        set => PoNumber = int.Parse(value);
    }

    [Inject]
    public IPurchaseOrderService PurchaseOrderService { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        PurchaseOrder = await PurchaseOrderService.GetPurchaseOrderAsync(PoNumber);
    }
}
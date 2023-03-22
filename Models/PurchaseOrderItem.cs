namespace Wasm.Models;

public class PurchaseOrderItem
{
    public int PoNumber { get; set; }

    public int ItemNumber { get; set; }

    public string LocalSku { get; set; }

    public int Quantity { get; set; }

    public double Cost { get; set; }
}
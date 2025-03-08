using Microsoft.AspNetCore.Components.Forms;

namespace PricingTool_Front_end.Models;

public class UserAdd
{
    public string Title { get; set; } = string.Empty;

    public string Descritpiton { get; set; } = string.Empty;

    public int? Price { get; set; } = 0;

    public IBrowserFile? Image { get; set; } = null;
}

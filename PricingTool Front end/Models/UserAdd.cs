using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace PricingTool_Front_end.Models;

public class UserAdd
{
    [Required(ErrorMessage = "Název je povinný")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage ="Popis je povinný")]
    [MinLength(50, ErrorMessage = "Popis musí být dlouhý alespoň 50 znaků")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Obrázek je povinný")]
    public IBrowserFile? Image { get; set; } = null;
    
    public int? Price { get; set; } = 0;

}

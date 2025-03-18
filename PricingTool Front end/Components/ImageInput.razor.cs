using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace PricingTool_Front_end.Components;

public partial class ImageInput
{
    [Parameter]
    public EventCallback<IBrowserFile> OnImageSelected { get; set; }
    private string? ImagePreviewUrl { get; set; }

    [Parameter]
    public string Height { get; set; } = "auto";

    [Parameter]
    public string Width { get; set; } = "auto";

    private async Task HandleImageUpload(InputFileChangeEventArgs e)
    {
        var file = e.File;

        if (file != null)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            ImagePreviewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";

            await OnImageSelected.InvokeAsync(file);
        }
    }
}

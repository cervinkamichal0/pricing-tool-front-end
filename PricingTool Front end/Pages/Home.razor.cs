using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using PricingTool_Front_end.Models;

namespace PricingTool_Front_end.Pages;

public partial class Home
{
    [Inject][NotNull] private HttpClient? Http { get; set; }

    protected UserAdd UsersAdd { get; set; } = new();
    protected SimilarAdsResponse SimilarAdsResponse { get; set; } = new();

    protected async Task FetchSimilarAds()
    {
        if (UsersAdd.Image != null)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();

            // Přidání textových dat (title, description)
            content.Add(new StringContent(UsersAdd.Title), "title");
            content.Add(new StringContent(UsersAdd.Descritpiton), "description");

            // Přidání souboru (obrázku)
            var fileContent = new StreamContent(UsersAdd.Image.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)); // max 10 MB
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(UsersAdd.Image.ContentType);
            content.Add(fileContent, "file", UsersAdd.Image.Name);

            // Odeslání dat na API
            var response = await Http.PostAsync("/similar_ads", content);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                SimilarAdsResponse = System.Text.Json.JsonSerializer.Deserialize<SimilarAdsResponse>(json) ?? new SimilarAdsResponse();
                UsersAdd.Price = SimilarAdsResponse.EstimatedPrice;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }

    private void HandleImageUpload(IBrowserFile file)
    {
        UsersAdd.Image = file;
    }
}

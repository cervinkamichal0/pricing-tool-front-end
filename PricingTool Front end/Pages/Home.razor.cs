using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using PricingTool_Front_end.Models;

namespace PricingTool_Front_end.Pages;

public partial class Home
{
    [Inject][NotNull] private HttpClient? Http { get; set; }

    protected UserAdd? UsersAdd { get; set; } = new();
    protected SimilarAdsResponse SimilarAdsResponse { get; set; } = new();

    protected async Task FetchSimilarAds()
    {
        var data = new Dictionary<string, string>
        {
            { "title", UsersAdd.Title },
            { "description", UsersAdd.Descritpiton}
        };

        var response = await Http.PostAsJsonAsync("/similar_ads", data);

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();  

            SimilarAdsResponse = System.Text.Json.JsonSerializer.Deserialize<SimilarAdsResponse>(json) ?? new SimilarAdsResponse();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"Chyba: {response.StatusCode}");
        }
    }

    private async Task HandleImageUpload(IBrowserFile file)
    {
        //TODO: implement
        Console.WriteLine($"Nahrán obrázek: {file.Name}");
    }
}

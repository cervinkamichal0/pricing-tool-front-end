using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using PricingTool_Front_end.Models;
using System.Text.Json;

namespace PricingTool_Front_end.Pages;

public partial class Home
{
    [Inject][NotNull] private HttpClient? Http { get; set; }

    protected UserAdd FormUsersAdd { get; set; } = new();
    protected SimilarAdsResponse SimilarAdsResponse { get; set; } = new();

    private bool PriceNotCalculated { get; set; } = true;

    private string ErrorMessage { get; set; } = string.Empty;

    private bool isErrorHidden { get; set; } = true;

    private bool IsPriceLoading { get; set; } = false;

    private bool IsDescriptionLoading { get; set; } = false;

    /// <summary>
    /// Naplní <see cref="SimilarAdsResponse"/> daty z similar_ads api endpointu a <see cref="FormUsersAdd.Price"/> vypočítanou cenou
    /// </summary>
    protected async Task FetchSimilarAds()
    {
        //Zobazí v tlačítku indikaci čekání na data
        IsPriceLoading = true;

        if (FormUsersAdd.Image is not null && FormUsersAdd.Title is not null && FormUsersAdd.Description is not null)
        {
            isErrorHidden = true;
            MultipartFormDataContent content = new MultipartFormDataContent();

            // Přidání textových dat (title, description)
            content.Add(new StringContent(FormUsersAdd.Title), "title");
            content.Add(new StringContent(FormUsersAdd.Description), "description");

            try
            {
                // Přidání souboru (obrázku)
                var fileContent = new StreamContent(FormUsersAdd.Image.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)); // max 10 MB
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(FormUsersAdd.Image.ContentType);
                content.Add(fileContent, "file", FormUsersAdd.Image.Name);
            }
            catch
            {
                ErrorMessage = "Nepodařilo se nahrát obrázek";
                isErrorHidden = false;
                IsPriceLoading = false;
            }

            try
            {
                // Odeslání dat na API
                var response = await Http.PostAsync("/similar_ads", content);

                if (response.IsSuccessStatusCode)
                {
                    //Deserializace dat z api
                    string json = await response.Content.ReadAsStringAsync();
                    SimilarAdsResponse = JsonSerializer.Deserialize<SimilarAdsResponse>(json) ?? new SimilarAdsResponse();
                    FormUsersAdd.Price = SimilarAdsResponse.EstimatedPrice;
                    PriceNotCalculated = false;
                }
                else
                {
                    ErrorMessage = "Na serveru došlo k chybě.";
                    IsPriceLoading = false;
                    isErrorHidden = false;
                }
            }
            catch
            {
                ErrorMessage = "Na serveru došlo k chybě.";
                isErrorHidden = false;
            }
            finally
            {
                IsPriceLoading = false;
            }
        }
    }

    /// <summary>
    /// Naplní <see cref="FormUsersAdd.Description"/> strukturou popisu z /generate_description api endpointu
    /// </summary>
    private async Task suggest_description()
    {
        if (FormUsersAdd.Title is not null && FormUsersAdd.Title.Length > 0)
        {
            IsDescriptionLoading = true;
            try
            {
                MultipartFormDataContent content = new MultipartFormDataContent();

                // Přidání textových dat (title)
                content.Add(new StringContent(FormUsersAdd.Title), "title");

                var response = await Http.PostAsync("/generate_description", content);

                if (response.IsSuccessStatusCode)
                {
                    //Deserrializace dat z api
                    string jsonString = await response.Content.ReadAsStringAsync();
                    FormUsersAdd.Description = JsonSerializer.Deserialize<string>(jsonString) ?? "";
                }
            }
            catch
            {
                ErrorMessage = "Na serveru došlo k chybě.";
                isErrorHidden = false;
            }
            finally
            {
                IsDescriptionLoading = false;
            }
        }
    }

    /// <summary>
    /// Naplní <see cref="FormUsersAdd.Image"/> obrázkem, který uživatel nahrál.
    /// </summary>
    /// <param name="file">Obrázek, který se má uložit do <see cref="FormUsersAdd.Image"/></param>
    private void HandleImageUpload(IBrowserFile file)
    {
        FormUsersAdd.Image = file;
    }
}

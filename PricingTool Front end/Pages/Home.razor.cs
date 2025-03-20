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

    private bool PriceNotCalculated { get; set; } = true;

    private string ErrorMessage { get; set; } = string.Empty;

    private bool isErrorHidden { get; set; } = true;

    private bool IsLoading { get; set; } = false;

    private EditContext? editContext;
    private ValidationMessageStore? messageStore;


    protected override void OnInitialized()
    {
        editContext = new EditContext(UsersAdd);
        messageStore = new ValidationMessageStore(editContext);
    }
    protected async Task FetchSimilarAds()
    {
        IsLoading = true;
        if (editContext is not null && !editContext.Validate())
        {
            IsLoading = false;
            return; // Zastaví odeslání, pokud validace selže
        }

        if (UsersAdd.Image is not null && UsersAdd.Title is not null && UsersAdd.Description is not null)
        {
            isErrorHidden = true;
            MultipartFormDataContent content = new MultipartFormDataContent();

            // Přidání textových dat (title, description)
            content.Add(new StringContent(UsersAdd.Title), "title");
            content.Add(new StringContent(UsersAdd.Description), "description");

            try
            {
                // Přidání souboru (obrázku)
                var fileContent = new StreamContent(UsersAdd.Image.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)); // max 10 MB
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(UsersAdd.Image.ContentType);
                content.Add(fileContent, "file", UsersAdd.Image.Name);
            }
            catch
            {
                ErrorMessage = "Nepodařilo se nahrát obrázek";
                isErrorHidden = false;
                IsLoading = false;
            }

            try
            {
                // Odeslání dat na API
                var response = await Http.PostAsync("/similar_ads", content);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    SimilarAdsResponse = System.Text.Json.JsonSerializer.Deserialize<SimilarAdsResponse>(json) ?? new SimilarAdsResponse();
                    UsersAdd.Price = SimilarAdsResponse.EstimatedPrice;
                    PriceNotCalculated = false;
                }
                else
                {
                    ErrorMessage = "Na serveru došlo k chybě.";
                    IsLoading = false;
                    isErrorHidden = false;
                }
            }
            catch ()
            {
                ErrorMessage = "Na serveru došlo k chybě.";
                isErrorHidden = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }



    private void HandleImageUpload(IBrowserFile file)
    {
        UsersAdd.Image = file;
    }
}

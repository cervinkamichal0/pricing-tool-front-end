using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using PricingTool_Front_end.Models;

namespace PricingTool_Front_end.Pages
{
    partial class Home
    {
        [Inject] [NotNull] HttpClient? Http { get; set; }
        private UserAdd UsersAdd { get; set; } = new();
        private List<AdResponse> SimilarAds { get; set; } = new();

        private async Task FetchSimilarAds()
        {
            var data = new Dictionary<string, string>
            {
                { "title", UsersAdd.Title },
                { "description", UsersAdd.Descritpiton }
            };

            var response = await Http.PostAsJsonAsync("/similar_ads", data);

            if (response.IsSuccessStatusCode)
            {
                SimilarAds = await response.Content.ReadFromJsonAsync<List<AdResponse>>() ?? new();
            }
            else
            {
                Console.WriteLine($"Chyba: {response.StatusCode}");
            }
        }
    }

    public class AdResponse
    {
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Url { get; set; } = string.Empty;
        public double SimilarityScore { get; set; }
    }
}
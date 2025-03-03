using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace PricingTool_Front_end.Pages
{
    partial class Home
    {
        [Inject]
        [NotNull]
        HttpClient Http { get; set; }
        private string? Title { get; set; }

        private string? Description { get; set; }

        private async Task FetchSimilarAds()
        {
            HttpClient client = new HttpClient();

            var data = new Dictionary<string, string>
            {
                { "title", Title },
                { "description", Description }
            };

            var response = await Http.PostAsJsonAsync("/similar_ads", data);


        }
    }
}

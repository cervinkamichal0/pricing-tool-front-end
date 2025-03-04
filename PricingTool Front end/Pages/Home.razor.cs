using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Xml.Linq;
using PricingTool_Front_end.Models;

namespace PricingTool_Front_end.Pages
{
    partial class Home
    {
        [Inject] [NotNull] HttpClient? Http { get; set; }
        private UserAdd UsersAdd { get; set; } = new();

        private async Task FetchSimilarAds()
        {
            var data = new Dictionary<string, string>
            {
                { "title", UsersAdd.Title },
                { "description", UsersAdd.Descritpiton }
            };

            var response = await Http.PostAsJsonAsync("/similar_ads", data);
            
            Console.WriteLine((response));
        }
    }
}

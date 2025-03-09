using System.Text.Json.Serialization;

public class SimilarAdsResponse
{
    [JsonPropertyName("estimated_price")]
    public int? EstimatedPrice { get; set; }

    [JsonPropertyName("estimated_quick_sale_price")]
    public int QuickPrice { get; set; }

    [JsonPropertyName("similar_ads")]
    public List<AdResponse> SimilarAds { get; set; } = new();
}

public class AdResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("similarity_score")]
    public double SimilarityScore { get; set; }
}

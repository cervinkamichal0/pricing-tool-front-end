using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace PricingTool_Front_end.Components;

public partial class AdsDisplay
{
    [Parameter, EditorRequired, NotNull]
    public SimilarAdsResponse? AdsResponse { get; set; }
}

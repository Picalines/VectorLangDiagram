using Microsoft.AspNetCore.Components;

namespace VectorLangDocs.Shared.Extensions;

internal static class NavigationManagerExtensions
{
    public static string UriOfElement(this NavigationManager navigationManager, string anchorId)
    {
        var currentUri = navigationManager.Uri;

        if (currentUri.IndexOf('#') is var idIndex and not -1)
        {
            currentUri = currentUri[..idIndex];
        }

        return currentUri + '#' + anchorId;
    }

    public static string UriWithoutId(this NavigationManager navigationManager)
    {
        var endIndex = navigationManager.Uri.IndexOf('#') switch
        {
            -1 => Index.End,
            var index => new Index(index),
        };

        return navigationManager.Uri[..endIndex];
    }
}

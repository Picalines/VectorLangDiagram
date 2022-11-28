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
}

using Microsoft.AspNetCore.Components;
using VectorLangDocs.Shared;

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

    public static RenderFragment RenderHeader(this NavigationManager navigationManager,
        int headerLevel, string headerId, string headerNavTitle, Func<string, RenderFragment> headerInnerContent)
    {
        var headerAttributes = NavigationItemElementAttributes("anchor", headerId, headerNavTitle);

        var uri = UriOfElement(navigationManager, headerId);

        return MarkupUtils.Header(headerLevel, headerAttributes, headerInnerContent(uri));
    }

    public static RenderFragment RenderSectionHeader(this NavigationManager navigationManager,
        int headerLevel, string headerId, string headerNavTitle, Func<string, RenderFragment> headerInnerContent)
    {
        var headerAttributes = NavigationItemElementAttributes("section", headerId, headerNavTitle);

        var uri = UriOfElement(navigationManager, headerId);

        return MarkupUtils.Header(headerLevel, headerAttributes, headerInnerContent(uri));
    }

    public static RenderFragment RenderHeader(this NavigationManager navigationManager,
        int headerLevel, string headerId, string headerNavTitle, RenderFragment headerInnerContent)
    {
        return RenderHeader(navigationManager, headerLevel, headerId, headerNavTitle, _ => headerInnerContent);
    }

    public static RenderFragment RenderSectionHeader(this NavigationManager navigationManager,
        int headerLevel, string headerId, string headerNavTitle, RenderFragment headerInnerContent)
    {
        return RenderSectionHeader(navigationManager, headerLevel, headerId, headerNavTitle, _ => headerInnerContent);
    }

    public static RenderFragment RenderHeader(this NavigationManager navigationManager, int headerLevel, string headerId, string headerTitle, bool copyButton = false)
    {
        return RenderHeader(navigationManager, headerLevel, headerId, headerTitle, uri => __builder =>
        {
            int sequence = 0;
            __builder.AddContent(sequence++, headerTitle);

            if (copyButton)
            {
                __builder.OpenComponent<CopyButton>(sequence++);
                __builder.AddAttribute(sequence++, nameof(CopyButton.TextToCopy), uri);
                __builder.AddAttribute(sequence++, nameof(CopyButton.ButtonTitle), "Copy link");
                __builder.AddAttribute(sequence++, nameof(CopyButton.Position), CopyButton.PositionType.AfterInlineParent);
                __builder.AddAttribute(sequence++, nameof(CopyButton.IconUrl), "./svg/copy-link-icon.svg");
                __builder.CloseComponent();
            }
        });
    }

    public static RenderFragment RenderSectionHeader(this NavigationManager navigationManager, int headerLevel, string headerId, string headerTitle, bool copyButton = false)
    {
        return RenderSectionHeader(navigationManager, headerLevel, headerId, headerTitle, uri => __builder =>
        {
            int sequence = 0;
            __builder.AddContent(sequence++, headerTitle);

            if (copyButton)
            {
                __builder.OpenComponent<CopyButton>(sequence++);
                __builder.AddAttribute(sequence++, nameof(CopyButton.TextToCopy), uri);
                __builder.AddAttribute(sequence++, nameof(CopyButton.ButtonTitle), "Copy link");
                __builder.AddAttribute(sequence++, nameof(CopyButton.Position), CopyButton.PositionType.AfterInlineParent);
                __builder.AddAttribute(sequence++, nameof(CopyButton.IconUrl), "./svg/copy-link-icon.svg");
                __builder.CloseComponent();
            }
        });
    }

    private static Dictionary<string, object> NavigationItemElementAttributes(string elementType, string headerId, string headerNavTitle)
    {
        return new()
        {
            ["tabindex"] = -1,
            ["id"] = headerId,
            [PageNavigation.BaseDataAttribute + elementType] = headerNavTitle,
        };
    }
}

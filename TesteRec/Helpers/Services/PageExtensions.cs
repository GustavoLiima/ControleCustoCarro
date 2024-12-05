namespace TesteRec.Helpers.Services
{
    public static class PageExtensions
    {
        public static void AddOverlay(this Page page, View overlay)
        {
            if (page is ContentPage contentPage && contentPage.Content is Layout layout)
            {
                layout.Children.Add(overlay);
            }
            else
            {
                throw new InvalidOperationException("The page must be a ContentPage with a layout-based content.");
            }
        }
    }
}

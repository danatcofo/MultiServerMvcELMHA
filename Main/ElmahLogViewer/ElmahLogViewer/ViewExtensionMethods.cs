namespace System.Web.Mvc
{
    public static class ViewExtensionMethods
    {
        public static string GetRouteValue(this ViewContext view, string id)
        {
            return view.RouteData.GetRequiredString(id);
        }
    }
}
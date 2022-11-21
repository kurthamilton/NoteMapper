using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace NoteMapper.Web.Blazor.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static void SetQueryStringValues(this NavigationManager navigationManager, IDictionary<string, string?> values)
        {
            Uri uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            IDictionary<string, StringValues> queryString = QueryHelpers.ParseQuery(uri.Query);
            foreach (string key in values.Keys)
            {
                queryString[key] = values[key];
            }

            string path = uri.GetLeftPart(UriPartial.Path);            
            string updatedUrl = QueryHelpers.AddQueryString(path, queryString);

            navigationManager.NavigateTo(updatedUrl);
        }

        public static bool TryGetQueryStringValue(this NavigationManager navigationManager, string key, out string value)
        {            
            Uri uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out StringValues queryValue))
            {
                value = queryValue.ToString();
                return true;
            }

            value = "";
            return false;
        }
    }
}
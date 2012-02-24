using System.Linq;
using System.Linq.Dynamic;
using System.Web.Helpers;

namespace ElmahLogViewer.Elmah.Models
{
    public static class ControllerExtensionMethods
    {
        public static IQueryable<TResult> GetQueryResults<T, TResult>(this IQueryable<T> v,
            int startIndex,
            int perPage,
            string sort,
            SortDirection sortDir,
            System.Linq.Expressions.Expression<System.Func<T, TResult>> selector)
        {
            return v
                .OrderBy(string.Format("{0} {1}", sort, sortDir))
                .Skip(startIndex).Take(perPage)
                .Select(selector);
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElmahLogViewer.Models
{
    public class ResultSet<T>
    {
        public IEnumerable<T> Results { get; set; }

        public SetConstraints Constraints { get; set; }
    }

    public interface IFilter<T> { T Filter { get; set; } }

    public class SetConstraints
    {
        public string Sort { get; set; }

        public SortDir SortDir { get; set; }

        public int StartIndex { get; set; }

        [Range(15, 200)]
        public int PerPage { get; set; }

        public int Page { get; set; }

        public int Total { get; set; }
    }

    public class SetConstraints<T> : SetConstraints, IFilter<T>
    {
        public T Filter { get; set; }
    }

    public enum SortDir
    {
        ASC,
        DESC,
    }
}
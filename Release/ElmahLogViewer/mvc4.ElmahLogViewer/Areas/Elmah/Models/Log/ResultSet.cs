using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mvc4.ElmahLogViewer.Areas.Elmah.Models
{
    public class ResultSet<T>
    {
        public IEnumerable<T> Results { get; set; }

        public SetConstraints Constraints { get; set; }
    }

    public class SetConstraints
    {
        public string Sort { get; set; }

        public SortDirection SortDir { get; set; }

        public int StartIndex { get; set; }

        [Range(15, 200)]
        public int PerPage { get; set; }

        public int Page { get; set; }

        public int Total { get; set; }
    }

    public class SortSelectListModel
    {
        public SortSelectListModel() { }

        public SortSelectListModel(string property, IEnumerable<string> values)
            : this(property, values, values) { }

        public SortSelectListModel(string property, IEnumerable<string> values, IEnumerable<string> labels)
        {
            Property = property;
            Values = values;
            Labels = labels;
        }

        public string Property { get; set; }

        public IEnumerable<string> Values { get; set; }

        public IEnumerable<string> Labels { get; set; }

        public SortDirection SortDir { get; set; }

        public IEnumerable<SortDirection> SortDirValue { get { return new List<SortDirection> { SortDirection.Ascending, SortDirection.Descending }; } }

        public IEnumerable<SelectListItem> GetItems()
        {
            if (Labels == null) Labels = Values;
            if (Values == null) Values = Labels;
            if (Values == null || Values.Count() == 0)
                return new List<SelectListItem> { new SelectListItem { Value = Property, Text = Property } };

            return GetItems(Values.ToArray(), Labels.ToArray());
        }

        private IEnumerable<SelectListItem> GetItems(string[] v1, string[] v2)
        {
            for (int i = 0; i < v1.Length; ++i)
                yield return new SelectListItem { Value = v1[i], Text = v2[i] };
        }
    }

    public class SortModel
    {
        public string Property { get; set; }

        public string Label { get; set; }
    }
}
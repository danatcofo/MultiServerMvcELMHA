using System;
using mvc4.ElmahLogViewer.Areas.Elmah.Data;

namespace mvc4.ElmahLogViewer.Areas.Elmah.Models
{
    public class ElmahError : IELMAH_Error
    {
        public Guid ServerId { get; set; }

        public string AllXml { get; set; }

        public string Application { get; set; }

        public Guid ErrorId { get; set; }

        public string Host { get; set; }

        public string Message { get; set; }

        public int Sequence { get; set; }

        public string Source { get; set; }

        public int StatusCode { get; set; }

        public DateTime TimeUtc { get; set; }

        public string Type { get; set; }

        public string User { get; set; }
    }
}
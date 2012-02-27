using System;

namespace mvc4.ElmahLogViewer.Areas.Elmah.Data
{
    public interface IELMAH_Error
    {
        string AllXml { get; set; }

        string Application { get; set; }

        Guid ErrorId { get; set; }

        string Host { get; set; }

        string Message { get; set; }

        int Sequence { get; set; }

        string Source { get; set; }

        int StatusCode { get; set; }

        DateTime TimeUtc { get; set; }

        string Type { get; set; }

        string User { get; set; }
    }
}
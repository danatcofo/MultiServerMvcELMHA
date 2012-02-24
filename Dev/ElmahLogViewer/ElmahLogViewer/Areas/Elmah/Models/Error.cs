using error = Elmah.Error;

namespace ElmahLogViewer.Areas.Elmah.Models
{
    public class Error
    {
        public error ElmahError { get; set; }

        public Data.IELMAH_Error ErrorDetails { get; set; }

        public Data.IServer ServerDetails { get; set; }
    }
}
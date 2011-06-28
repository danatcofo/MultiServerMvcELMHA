namespace ElmahLogViewer.Models.ELMAH
{
    public class Error
    {
        public Elmah.Error ElmahError { get; set; }

        public Data.IELMAH_Error ErrorDetails { get; set; }

        public Data.IServer ServerDetails { get; set; }
    }
}

using ElmahLogViewer.Data;

namespace ElmahLogViewer.Models.ELMAH
{
    public class ServerList : ResultSet<IELMAH_Server> { }

    public class ErrorList : ResultSet<IELMAH_Error>
    {
        public IELMAH_Server Server { get; set; }
    }
}
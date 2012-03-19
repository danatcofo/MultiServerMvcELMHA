using ElmahLogViewer.Areas.Elmah.Data;

namespace ElmahLogViewer.Areas.Elmah.Models
{
    public class ServerList : ResultSet<IServer> { }

    public class ErrorList : ResultSet<IELMAH_Error>
    {
        public IServer Server { get; set; }
    }
}
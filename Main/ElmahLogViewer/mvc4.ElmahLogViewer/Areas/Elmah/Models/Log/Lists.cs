using mvc4.ElmahLogViewer.Areas.Elmah.Data;

namespace mvc4.ElmahLogViewer.Areas.Elmah.Models
{
    public class ServerList : ResultSet<IServer> { }

    public class ErrorList : ResultSet<IELMAH_Error>
    {
        public IServer Server { get; set; }
    }
}
using ElmahLogViewer.Data;

namespace ElmahLogViewer.Models.ELMAH
{
    public class ServerList : ResultSet<IServer> { }

    public class ErrorList : ResultSet<IELMAH_Error>
    {
        public IServer Server { get; set; }
    }
}

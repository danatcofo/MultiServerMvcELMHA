using System;

namespace ElmahLogViewer.Data
{
    public interface IELMAH_Server
    {
        string ConnectionString { get; set; }

        string Environment { get; set; }

        string Name { get; set; }

        Guid ServerId { get; set; }
    }
}
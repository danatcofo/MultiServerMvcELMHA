﻿using System;

namespace ElmahLogViewer.Areas.Elmah.Data
{
    public interface IServer
    {
        string ConnectionString { get; set; }

        string Environment { get; set; }

        string Name { get; set; }

        Guid ServerId { get; set; }
    }
}
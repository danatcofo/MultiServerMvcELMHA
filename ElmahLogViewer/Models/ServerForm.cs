using System;
using System.ComponentModel.DataAnnotations;
using ElmahLogViewer.Areas.Elmah.Data;

namespace ElmahLogViewer.Areas.Elmah.Models
{
    public class ServerForm : IServer
    {
        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string Environment { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid ServerId { get; set; }
    }
}
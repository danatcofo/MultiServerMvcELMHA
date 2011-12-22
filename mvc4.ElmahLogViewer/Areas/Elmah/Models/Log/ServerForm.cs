using System;
using System.ComponentModel.DataAnnotations;
using mvc4.ElmahLogViewer.Areas.Elmah.Data;

namespace mvc4.ElmahLogViewer.Areas.Elmah.Models
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
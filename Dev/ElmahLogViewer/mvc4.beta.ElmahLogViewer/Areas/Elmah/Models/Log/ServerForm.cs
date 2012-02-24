using System;
using System.ComponentModel.DataAnnotations;
using ElmahLogViewer.Elmah.Data;

namespace ElmahLogViewer.Elmah.Models
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
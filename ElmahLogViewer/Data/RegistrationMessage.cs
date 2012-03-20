using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib.PortableAreas;

namespace ElmahLogViewer.Areas.Elmah.Data
{
    public class RegistrationMessage : IEventMessage
    {
        public RegistrationMessage(string message)
        {
            _Message = message;
        }

        private readonly string _Message;

        public override string ToString()
        {
            return _Message;
        }
    }
}
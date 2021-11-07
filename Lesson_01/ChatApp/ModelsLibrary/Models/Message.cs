using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models
{
    public class SimpleMessage
    {
        public string Text { get; set; }
        public string From { get; set; }
        public string RemoteIP { get; set; }
    }

    public class ClientMessage : SimpleMessage
    {
        public string To { get; set; }
    } 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookMAUI8.Model
{
    public class EmailContext
    {
        public string? EmailBody { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? SenderEmail { get; set; }

        public DateTime? RecievedTime { get; set; }

        public string? Subject { get; set; }

        public string? Error { get; set; }
    }
}

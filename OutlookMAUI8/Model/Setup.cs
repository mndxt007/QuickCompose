using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookMAUI8.Model
{
    public class Setup
    {
        [RegularExpression("^\\S+$", ErrorMessage = "Specify in action in single word (without spaces, use _-)")]
        public string? Category1 { get; set; }
        [RegularExpression("^\\S+$", ErrorMessage = "Specify in action in single word (without spaces, use _-)")]
        public string? Category2 { get; set; }
        [RegularExpression("^\\S+$", ErrorMessage = "Specify in action in single word (without spaces, use _-)")]
        public string? Category3 { get; set; }
        [RegularExpression("^\\S+$", ErrorMessage = "Specify in action in single word (without spaces, use _-)")]
        public string? Category4 { get; set; }
        public string? Folder { get; set; }
        public string? AccountIndex { get; set; } = "1";


        public Setup()
        {
            Category1 = "Follow-up";
            Category2 = "Attention-Needed";
            Category3 = "Acknowledgement";
            Category4 = "Case-Closure";
            Folder = "IIS Discussions";
        }
    }
}

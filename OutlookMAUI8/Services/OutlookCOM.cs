using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddInMAUI8.Services
{
    internal class OutlookCOM
    {
        public Microsoft.Office.Interop.Outlook.Application outlookApp;

        public OutlookCOM()
        {
            outlookApp = new Microsoft.Office.Interop.Outlook.Application();
        }
    }
}

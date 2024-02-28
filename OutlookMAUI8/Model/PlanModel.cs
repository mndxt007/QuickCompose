using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookMAUI8.Model
{
	public class PlanModel
	{
		public EmailContext Message { get; set; } = default!;
		public string Action { get; set; } = "Demo_Action";
		public string Response { get; set; } = "Take Action on the email";
		public string Sentiment { get; set; } = "Default Sentiment";
		public int Priority { get; set; } = 5;
	}
}

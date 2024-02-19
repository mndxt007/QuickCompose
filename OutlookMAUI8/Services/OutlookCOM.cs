using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookMAUI8.Services
{
    internal class OutlookCOM
    {
        public Microsoft.Office.Interop.Outlook.Application outlookApp;

        public OutlookCOM()
        {
            outlookApp = new Microsoft.Office.Interop.Outlook.Application();
        }

        //public async Task<string> GetEmailDataAsync(bool includeFullConversation)
        //{
        //    //return await _jsRuntime.InvokeAsync<string>("getEmailData", includeFullConversation);
        //}

        //public async Task<string> GetUserAsync()
        //{
        //    //return await _jsRuntime.InvokeAsync<string>("getUser");
        //}

        //public async Task<string> Html2TextAsync(string html, bool includeFullConversation)
        //{
        //   // return await _jsRuntime.InvokeAsync<string>("html2text", html, includeFullConversation);
        //}

        //public async Task ReplyAllAsync(string chatGPTResponse)
        //{
        //    //await _jsRuntime.InvokeVoidAsync("replyAll", chatGPTResponse);
        //}
    }
}

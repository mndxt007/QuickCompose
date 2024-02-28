using Microsoft.Office.Interop.Outlook;
using OutlookMAUI8.Components.Pages;
using OutlookMAUI8.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;

namespace OutlookMAUI8.Services
{
    internal class OfficeService
    {
        public Microsoft.Office.Interop.Outlook.Application outlookApp;
        Selection selection;
        public Explorer explorer;
        MailItem mailItem;

        public OfficeService()
        {
            outlookApp = new Microsoft.Office.Interop.Outlook.Application();
        }

        public EmailContext GetEmailDataAsync(bool includeFullConversation)
        {
            var emailContext = new EmailContext();
            try
            {
                explorer = outlookApp.ActiveExplorer();
                
                //explorer.SelectionChange += Explorer_SelectionChange;
                selection = explorer.Selection;
                if (selection.Count > 0)
                {
                    mailItem = selection[1] as MailItem;

                }
                if (mailItem != null)
                {
                    // print the subject of the email
                    emailContext.EntryID = mailItem.EntryID;
                    emailContext.EmailBody = mailItem.Body;
                    emailContext.RecievedTime = mailItem.ReceivedTime;
                    emailContext.Subject = mailItem.Subject;
                    emailContext.UserName = mailItem.UserProperties.Session.CurrentUser.Name;
                    emailContext.SenderEmail = mailItem.SenderName;
                    AddressEntry currentUserAddressEntry = mailItem.UserProperties.Session.CurrentUser.AddressEntry;

                    if (currentUserAddressEntry.Type == "EX")
                    {
                        // This is an Exchange user. Use the ExchangeUser object to get the SMTP address.
                        ExchangeUser exchangeUser = currentUserAddressEntry.GetExchangeUser();
                        if (exchangeUser != null)
                        {
                            emailContext.UserEmail = exchangeUser.PrimarySmtpAddress;
                        }
                    }
                    else
                    {
                        // This is not an Exchange user. Just use the address.
                        emailContext.UserEmail = currentUserAddressEntry.Address;
                    }


                    return emailContext;

                }

                else
                {
                    emailContext.Error = "No email found";
                    return emailContext;
                }
            }
            catch (System.Exception ex) 
            {
                emailContext.Error = "No email found";
                return emailContext;
            }
            //return await _jsRuntime.InvokeAsync<string>("getEmailData", includeFullConversation);
           

        }

        public EmailContext GetEmailDataAsync(MailItem mailItem, bool includeFullConversation)
        {
            var emailContext = new EmailContext();
            try
            {
                
                if (mailItem != null)
                {
                    // print the subject of the email
                    emailContext.EntryID = mailItem.EntryID;    
                    emailContext.EmailBody = mailItem.Body;
                    emailContext.RecievedTime = mailItem.ReceivedTime;
                    emailContext.Subject = mailItem.Subject;
                    emailContext.UserName = mailItem.UserProperties.Session.CurrentUser.Name;
                    emailContext.SenderEmail = mailItem.SenderName;
                    AddressEntry currentUserAddressEntry = mailItem.UserProperties.Session.CurrentUser.AddressEntry;

                    if (currentUserAddressEntry.Type == "EX")
                    {
                        // This is an Exchange user. Use the ExchangeUser object to get the SMTP address.
                        ExchangeUser exchangeUser = currentUserAddressEntry.GetExchangeUser();
                        if (exchangeUser != null)
                        {
                            emailContext.UserEmail = exchangeUser.PrimarySmtpAddress;
                        }
                    }
                    else
                    {
                        // This is not an Exchange user. Just use the address.
                        emailContext.UserEmail = currentUserAddressEntry.Address;
                    }


                    return emailContext;

                }

                else
                {
                    emailContext.Error = "No email found";
                    return emailContext;
                }
            }
            catch (System.Exception ex)
            {
                emailContext.Error = "No email found";
                return emailContext;
            }
            //return await _jsRuntime.InvokeAsync<string>("getEmailData", includeFullConversation);


        }


        public async Task<string> GetUserAsync()
        {
            var currentUser = outlookApp.Session.CurrentUser;

            // Return the user's email
            return currentUser.AddressEntry.Address;
        }

        //public async Task<string> Html2TextAsync(string html, bool includeFullConversation)
        //{
        //    // return await _jsRuntime.InvokeAsync<string>("html2text", html, includeFullConversation);
        //}

        public async Task ReplyAllAsync(string entryID, string chatGPTResponse)
        {
            MailItem mailItem = outlookApp.GetNamespace("MAPI").GetItemFromID(entryID) as MailItem;
            var reply = mailItem.ReplyAll();

            // Set the body of the reply

            // Set the body of the reply and add the signature
            //reply.HTMLBody = chatGPTResponse + "\n\n" + reply.Body;
            reply.Display();
        }
    }
}

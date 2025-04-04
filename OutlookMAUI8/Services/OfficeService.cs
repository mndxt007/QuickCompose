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
using Blazored.LocalStorage;
using System.Diagnostics;

namespace OutlookMAUI8.Services
{
    internal class OfficeService
    {
        public Microsoft.Office.Interop.Outlook.Application outlookApp;
        Selection selection;
        public Explorer explorer;
        MailItem mailItem;
        private static EmailContext emailContext = new();
        public int accountIndex = 1;

        public OfficeService(ILocalStorageService LocalStorage)
        {
            outlookApp = new Microsoft.Office.Interop.Outlook.Application();
        }


        public EmailContext GetEmailDataAsync(bool includeFullConversation)
        {
            try
            {
                explorer = outlookApp.ActiveExplorer();
                selection = explorer.Selection;
                if (selection.Count > 0)
                {
                    mailItem = selection[1] as MailItem;
                }

                if (mailItem != null)
                {
                    return PopulateEmailContext(mailItem);
                }
                else
                {
                    emailContext.Error = "No email found";
                    return emailContext;
                }
            }
            catch (System.Exception)
            {
                emailContext.Error = "No email found";
                return emailContext;
            }
        }

        public EmailContext GetEmailDataAsync(MailItem mailItem, bool includeFullConversation)
        {
            try
            {
                if (mailItem != null)
                {
                    return PopulateEmailContext(mailItem);
                }
                else
                {
                    var emailContext = new EmailContext
                    {
                        Error = "No email found"
                    };
                    return emailContext;
                }
            }
            catch (System.Exception)
            {
                var emailContext = new EmailContext
                {
                    Error = "No email found"
                };
                return emailContext;
            }
        }

        private EmailContext PopulateEmailContext(MailItem mailItem)
        {
            var emailContext = new EmailContext
            {
                EntryID = mailItem.EntryID,
                EmailBody = mailItem.Body,
                RecievedTime = mailItem.ReceivedTime,
                Subject = mailItem.Subject,
                UserName = mailItem.UserProperties.Session.Accounts[accountIndex].UserName,
                SenderEmail = mailItem.SenderName,
                UserEmail = mailItem.UserProperties.Session.Accounts[accountIndex].SmtpAddress
            };

            return emailContext;
        }


        public async Task<string> GetUserAsync()
        {
            var currentUser = outlookApp.Session.Accounts[accountIndex];

            // Return the user's email
            return currentUser.SmtpAddress;
        }

        //public async Task<string> Html2TextAsync(string html, bool includeFullConversation)
        //{
        //    // return await _jsRuntime.InvokeAsync<string>("html2text", html, includeFullConversation);
        //}
        public Folders ReturnFolders()
        {
            NameSpace outlookNamespace = outlookApp.GetNamespace("MAPI");
            Accounts accounts = outlookNamespace.Accounts;
            Account selectedAccount = null;
            // Select the first account or prompt the user to select an account
            if (accounts.Count > 0)
            {
                selectedAccount = accounts[accountIndex]; // Select the first account for simplicity
                // Alternatively, you can prompt the user to select an account
                // selectedAccount = PromptUserToSelectAccount(accounts);
            }

            if (selectedAccount != null)
            {
                // Get the inbox folder for the selected account
                MAPIFolder inboxFolder = selectedAccount.DeliveryStore.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
                return inboxFolder.Folders;
            }
            else
            {
                throw new System.Exception("No account found.");
            }
        }

        public List<string> GetAccounts()
        {
            List<string> accounts = new List<string>();
            foreach (Account account in outlookApp.Session.Accounts)
            {
                accounts.Add(account.SmtpAddress);
            }
            return accounts;
        }

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

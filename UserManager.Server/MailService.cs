using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;

namespace UserManager
{
    public class MailService
    {
        public bool Send(string to, string name, string text)
        {
            try
            {
                UserCredential credential;
                string[] scopes = { GmailService.Scope.GmailSend };
                // Load client secrets.
                using (var stream = new FileStream(
                           "client_secret.json", 
                           FileMode.Open, 
                           FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                // Create Gmail API service.
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "UserManager"
                });

                //create message
                MimeMessage message = new MimeMessage();
                message.To.Add(new MailboxAddress(name, to));
                message.Body = new TextPart("plain")
                {
                    Text = text
                };

                MemoryStream memoryStream = new MemoryStream();
                message.WriteTo(memoryStream);
                memoryStream.Position = 0;
                StreamReader streamReader = new StreamReader(memoryStream);
                string rawString = streamReader.ReadToEnd();

                byte[] raw = System.Text.Encoding.UTF8.GetBytes(rawString);
                
                Message toSend = new Message()
                {
                    Raw = Convert.ToBase64String(raw)
                };

                //Send message
                Message response = service.Users.Messages.Send(toSend, "me").Execute();

                if (response != null) return true;
                
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
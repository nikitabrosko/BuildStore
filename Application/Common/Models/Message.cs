using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace Application.Common.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public byte[] Attachment { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>(); 
            To.AddRange(to.Select(x => new MailboxAddress(x, x)));

            Subject = subject;
            Content = content;
        }

        public Message(IEnumerable<string> to, string subject, string content, byte[] attachment)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x, x)));

            Subject = subject;
            Content = content;
            Attachment = attachment;
        }
    }
}

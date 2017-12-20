using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain;

namespace lm.Comol.Core.Mail
{
    [Serializable]
    public class dtoMailMessage
    {
        public virtual String UserSubject { get; set; }
        public virtual String Body { get; set; }
        public virtual List<Attachment> Attachments { get; set; }

        public virtual MailAddress FromUser { get; set; }
        public virtual MailAddress ReplyTo { get; set; }
        public virtual List<MailAddress> To { get; set; }
        public virtual List<MailAddress> CC { get; set; }
        public virtual List<MailAddress> BCC { get; set; }

        public dtoMailMessage() {
            Body = "";
            UserSubject = "";
            To = new List<MailAddress>();
            CC = new List<MailAddress>();
            BCC = new List<MailAddress>();
            Attachments = new List<Attachment>();
        }
        public dtoMailMessage(dtoMailContent mailContent) 
            : this()
        {
            Body = mailContent.Body;
            UserSubject = mailContent.Subject;
        }
        public dtoMailMessage(String subject,String body) 
            : this()
        {
            Body = body;
            UserSubject = subject;
        }
        public dtoMailMessage(String userMail) : this()
		{
            FromUser = new MailAddress(userMail);
            ReplyTo = FromUser;
		}
        public dtoMailMessage(MailAddress mailAddress) : this()
		{
            FromUser = mailAddress;
            ReplyTo = FromUser;
		}
        public dtoMailMessage(Person person)
            : this()
		{
            FromUser =  new MailAddress(person.Mail, person.SurnameAndName);
            ReplyTo = FromUser;
		}
        public MailException AddAddresses(String addresses, RecipientType recipientType)
        {
            Char splitChar = ' ';
            if (addresses.Contains(";"))
                splitChar = ';';
            else if (addresses.Contains(","))
                splitChar = ',';

            List<String> recipients = new List<String>();
            if (splitChar == ' ')
                recipients.Add(addresses);
            else
                recipients.AddRange(addresses.Split(splitChar).Where(s=> !string.IsNullOrEmpty(s)).ToList());
            try
            {
                switch (recipientType) { 
                    case RecipientType.To:
                        recipients.ForEach(r=> To.Add(new MailAddress(r)));
                        break;
                    case RecipientType.CC:
                        recipients.ForEach(r=> CC.Add(new MailAddress(r)));
                        break;
                    case RecipientType.BCC:
                        recipients.ForEach(r=> BCC.Add(new MailAddress(r)));
                        break;
                }
                return MailException.None;
            }
            catch (Exception ex)
            {
                return MailException.InvalidAddress;
            }
        }
        public MailException AddAddresses(List<dtoRecipient> recipients, RecipientType recipientType)
        {
            try
            {
                switch (recipientType)
                {
                    case RecipientType.To:
                        recipients.Where(r=> String.IsNullOrEmpty(r.DisplayName)).ToList().ForEach(r => To.Add(new MailAddress(r.MailAddress)));
                        recipients.Where(r => !String.IsNullOrEmpty(r.DisplayName)).ToList().ForEach(r => To.Add(new MailAddress(r.MailAddress, r.DisplayName)));
                        break;
                    case RecipientType.CC:
                        recipients.Where(r => String.IsNullOrEmpty(r.DisplayName)).ToList().ForEach(r => CC.Add(new MailAddress(r.MailAddress)));
                        recipients.Where(r => !String.IsNullOrEmpty(r.DisplayName)).ToList().ForEach(r => CC.Add(new MailAddress(r.MailAddress, r.DisplayName)));
                        break;
                    case RecipientType.BCC:
                        recipients.Where(r => String.IsNullOrEmpty(r.DisplayName)).ToList().ForEach(r => BCC.Add(new MailAddress(r.MailAddress)));
                        recipients.Where(r => !String.IsNullOrEmpty(r.DisplayName)).ToList().ForEach(r => BCC.Add(new MailAddress(r.MailAddress, r.DisplayName)));
                        break;
                }
                return MailException.None;
            }
            catch (Exception ex)
            {
                return MailException.InvalidAddress;
            }
        }
    }
}
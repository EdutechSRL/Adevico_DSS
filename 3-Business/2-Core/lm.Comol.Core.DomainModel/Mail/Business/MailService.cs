using System;
using System.Net.Mail;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.MailCommons.Domain.Configurations;
using lm.Comol.Core.MailCommons.Domain;
namespace lm.Comol.Core.Mail
{
	public class MailService
	{
        #region "Person"
            private SmtpServiceConfig SMTPconfig { get; set; }
            private lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings MailSettings { get; set; }
        #endregion

        public MailService(SmtpServiceConfig smtp, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings settings) 
		{
            SMTPconfig = smtp;
            MailSettings = settings;
		}
        public MailException SendMail(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, Boolean replaceCRLF = false)
		{
            return InternalSendMail(idUserLanguage, dLanguage,dtoMessage, replaceCRLF);
		}
        public MailException SendMail(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, String ToAdresses, RecipientType type, Boolean replaceCRLF = false)
		{
            return (dtoMessage.AddAddresses(ToAdresses, type) == MailException.None) ? InternalSendMail(idUserLanguage, dLanguage, dtoMessage, replaceCRLF) : MailException.InvalidAddress;
		}
        private MailException InternalSendMail(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, Boolean replaceCRLF = false)
		{
            try{
                MailMessage message = CreateMessage(idUserLanguage, dLanguage,dtoMessage, replaceCRLF);
                SmtpClient smtpClient = CreateSmtpClient();
                List<dtoRecipients> recipients = new List<dtoRecipients>();
                dtoMessage.To.ForEach(t => recipients.Add(new dtoRecipients() { Mail = t, Type = RecipientType.To }));
                dtoMessage.CC.ForEach(t => recipients.Add(new dtoRecipients() { Mail = t, Type = RecipientType.CC }));
                dtoMessage.BCC.ForEach(t => recipients.Add(new dtoRecipients() { Mail = t, Type = RecipientType.BCC }));
                Int32 maxRecipients = SMTPconfig.MaxRecipients;
                int Total = recipients.Count;
                int PageNumber = 0;
                if (maxRecipients > 0)
                {
                    if (recipients.Count / maxRecipients < 1)
                        PageNumber = 0;
                    else
                        PageNumber = (recipients.Count % maxRecipients == 0 ? recipients.Count / maxRecipients - 1 : (recipients.Count / maxRecipients));
                }

                for (int i = 0; i <= PageNumber; i++)
                {
                    List<dtoRecipients> paged = null;
                    if (SMTPconfig.MaxRecipients > 0)
                        paged = recipients.Skip(maxRecipients * i).Take(maxRecipients).ToList();
                    else
                        paged = recipients;

                    if (paged != null && paged.Count > 0)
                    {
                        message.To.Clear();
                        message.CC.Clear();
                        message.Bcc.Clear();
                        paged.Where(p=>p.Type==  RecipientType.To).ToList().ForEach(p => message.To.Add(p.Mail));
                        paged.Where(p => p.Type == RecipientType.CC).ToList().ForEach(p => message.CC.Add(p.Mail));
                        paged.Where(p => p.Type == RecipientType.BCC).ToList().ForEach(p => message.Bcc.Add(p.Mail));

                        smtpClient.Send(message);
                    }
                }
                if (MailSettings.CopyToSender) {
                    message.To.Clear();
                    message.CC.Clear();
                    message.Bcc.Clear();
                    message.Subject = SubjectForCopy(idUserLanguage, dLanguage,dtoMessage);
                    message.To.Add(new MailAddress(dtoMessage.FromUser.DisplayName, dtoMessage.FromUser.Address));
                    smtpClient.Send(message);
                }
                return MailException.MailSent;
            }
            catch (ArgumentOutOfRangeException outOfRangeException)
            {
                return MailException.NoDestinationAddress;
            }
            catch (SmtpException smtpException)
            {
                return MailException.SMTPunavailable;    
                }
            catch( Exception ex){
                return MailException.UnknownError;
            }
		}
        private SmtpClient CreateSmtpClient(){
            SmtpClient smtpClient = new SmtpClient(SMTPconfig.Host,SMTPconfig.Port);
            smtpClient.EnableSsl = SMTPconfig.Authentication.EnableSsl;
            if (SMTPconfig.Authentication.Enabled) {
                smtpClient.Credentials = new System.Net.NetworkCredential(SMTPconfig.Authentication.AccountName, SMTPconfig.Authentication.AccountPassword);
                
                //smtpClient.UseDefaultCredentials = SMTPsettings.Authentication.UseDefaultCredentials;
                
            }
            return smtpClient;
        }
        private MailMessage CreateMessage(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage, Boolean replaceCRLF = false)
        {
            MailMessage message = new System.Net.Mail.MailMessage();
            //message.BodyEncoding = System.Text.Encoding.Unicode;
            //message.SubjectEncoding = System.Text.Encoding.Unicode;
            message.Sender = Sender(dtoMessage);
            message.From = From(dtoMessage);
            if (dtoMessage.ReplyTo !=null)
                message.ReplyToList.Add(dtoMessage.ReplyTo);
            if (!SMTPconfig.RelayAllowed && !message.ReplyToList.Any())
                message.ReplyToList.Add(message.From);


            if (MailSettings.NotifyToSender && message.ReplyToList.Any())
            {
            	try {
                    if (!string.IsNullOrEmpty(dtoMessage.ReplyTo.DisplayName))
                    {
                        message.Headers.Add("Disposition-Notification-To", dtoMessage.ReplyTo.DisplayName + " <" + dtoMessage.ReplyTo.Address + ">");
					} else {
                        message.Headers.Add("Disposition-Notification-To", "<" + dtoMessage.ReplyTo.Address + ">");
					}

				} catch (Exception ex) {
				}
            }


            message.Subject = Subject(idUserLanguage, dLanguage,dtoMessage);
            message.IsBodyHtml = MailSettings.IsBodyHtml;
            if (dtoMessage.Attachments != null && dtoMessage.Attachments.Count > 0)
                dtoMessage.Attachments.ForEach(a => message.Attachments.Add(a));
            message.Body = (message.IsBodyHtml && replaceCRLF ? dtoMessage.Body.Replace("\r\n", "<br>") : dtoMessage.Body);
            switch (MailSettings.Signature)
            {
                case Signature.FromConfigurationSettings:
                    message.Body += ((message.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + SMTPconfig.GetSignature(idUserLanguage, dLanguage.Id);
                    break;
                case Signature.FromNoReplySettings:
                    message.Body += ((message.IsBodyHtml) ? "<br/><br/>" : "\r\n\r\n\r\n") + SMTPconfig.GetNoReplySignature(idUserLanguage,  dLanguage.Id);
                    break;
            }
            return message;
        }
        private String Subject(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage)
        {
            return (MailSettings.PrefixType == MailCommons.Domain.SubjectPrefixType.None) ? dtoMessage.UserSubject : SMTPconfig.GetSubjectPrefix(idUserLanguage, dLanguage.Id ) + dtoMessage.UserSubject;
        }
        private String SubjectForCopy(Int32 idUserLanguage, Language dLanguage, dtoMailMessage dtoMessage)
        {
            return (MailSettings.PrefixType ==  MailCommons.Domain.SubjectPrefixType.None) ? SMTPconfig.GetSubjectForSenderCopy(idUserLanguage, dLanguage.Id) + dtoMessage.UserSubject : SMTPconfig.GetSubjectPrefix(idUserLanguage, dLanguage.Id) + SMTPconfig.GetSubjectForSenderCopy(idUserLanguage, dLanguage.Id ) + dtoMessage.UserSubject;
        }
        private MailAddress From(dtoMailMessage dtoMessage)
        {
            return (MailSettings.SenderType ==  MailCommons.Domain.SenderUserType.System) ? SMTPconfig.GetSystemSender() : dtoMessage.FromUser;
            //return (SMTPconfig.RelayAllowed) ? ((MailSettings.Sender == SenderType.System) ? SMTPconfig.SystemSender : dtoMessage.FromUser) : ((SMTPconfig.SystemSender.Address ==dtoMessage.FromUser.Address) ?  SMTPconfig.SystemSender : new MailAddress( SMTPconfig.SystemSender.Address, dtoMessage.FromUser.Address)) ;
        }
        private MailAddress Sender(dtoMailMessage dtoMessage)
        {
            return (SMTPconfig.RelayAllowed) ? ((MailSettings.SenderType == SenderUserType.System || dtoMessage.FromUser == null) ? SMTPconfig.GetSystemSender() : dtoMessage.FromUser) : SMTPconfig.GetSystemSender();
        }

      
    }
}

using System;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.DataContract
{
    //[ServiceContract]
    //public interface iServiceMailSender
    //{
    //    /// <summary>
    //    /// Send direct mail
    //    /// </summary>
    //    /// <param name="istanceIdentifier">Istance unique ID</param>
    //    /// <param name="idUser">Sender user</param>
    //    /// <param name="moduleCode">Sender module code</param>
    //    /// <param name="subject">Mail subject</param>
    //    /// <param name="body">Mail body</param>
    //    /// <param name="attachments">Mail attachments list (file names)</param>
    //    /// <param name="attachmentPath">Attachment path (used only if !="" otherwise system uses istance configuration settings)</param>
    //    /// <param name="saveMessage">Message sent MUST be saved ?</param>
    //    /// <param name="attachmentSavedPath">If not found in configuration where attachments must be saved IF mail MUST be saved !</param>
        
    //    [OperationContract(IsOneWay = true)]
    //    void SendMail(String istanceIdentifier, Int32 idUser, String moduleCode, String subject, String body, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = false, String attachmentSavedPath = "");

    //    //[OperationContract(IsOneWay = true)]
    //    //void SendDirectMailWithSettings(Int32 idUser, String moduleCode, String subject, String body, SmtpConfig smtp, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = false);

    //    //[OperationContract(IsOneWay = true)]
    //    //void SendMail(Int32 idUser, String moduleCode, String subject, String body, dtoMailSettings settings, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = false);

    //    //[OperationContract(IsOneWay = true)]
    //    //void SendDirectMailMessage(Int32 idUser, String moduleCode, String subject, String body, SenderType sndType = SenderType.System, SubjectType sType = SubjectType.System, Boolean isHtml = true, SignatureType signature = SignatureType.FromConfigurationSettings, Boolean notifyToSender = false, Boolean copyToSender = false, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = false);
       
    //    //[OperationContract(IsOneWay = true)]
    //    //void SendMailMessagesFromTemplate(Int32 idUser, String moduleCode, lm.Comol.Core.Mail.dtoMailSettings mailSettings, List<lm.Comol.Core.Mail.Messages.dtoMailTranslatedMessage> messages, lm.Comol.Core.Mail.Messages.dtoBaseMailTemplate template = null, ModuleObject obj = null, Int32 idCommunity = -1, Boolean isPortal = false, List<String> attachments = null, String attachmentPath = "", Boolean saveMessage = true);
    //}
}
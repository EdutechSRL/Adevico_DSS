using System;
namespace lm.Comol.Core.Mail
{
    [Serializable]
    public enum MailException
    {
        None = 0,
        MailSent = 1,
        InvalidAddress = 2,
        SMTPunavailable = 3,
        AuthenticationError = 4,
        NoDestinationAddress = 5,
        UnknownError = 6
    }
}

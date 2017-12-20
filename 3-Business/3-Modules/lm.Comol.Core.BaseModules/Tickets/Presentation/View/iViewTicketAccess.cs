using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewTicketAccess : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        void DisplayLogin(Domain.Enums.LoginStatus LoginStat); //, Boolean HasToken);

        void RedirectToList();
        void RedirectToTicket(Int64 TicketID);
        void RedirectToCreate();

        void ShowAccessError(Domain.Enums.ExternalUserValidateError Error);
        //void ShowRegistrationError( Domain.DTO.DTO_ExtUserAddResult Result);
        void ShowRecoverError(Domain.Enums.AccessRecoverError Error);

        void ShowView(Domain.Enums.AccessView currentView);
        void ShowRegistered(Domain.DTO.DTO_ExtUserAddResult Result);

        //void InitDDL(IList<Domain.DTO.DTO_TkLanguage> Languages);

        String GetLanguageCode { get; }

        void SetCurrentUser(Domain.DTO.DTO_User User);

        String UrlToken { get; }

        void ShowValidationError(Domain.Enums.TokenValidationResult Result);

        Domain.DTO.DTO_NotificationSettings Settings { get; }

        void SendAction(
            ModuleTicket.ActionType Action,
            Int32 idCommunity,
            ModuleTicket.InteractionType Type,
            IList<KeyValuePair<Int32, String>> Objects = null);

        void ShowServiceDisabled();
    }
}

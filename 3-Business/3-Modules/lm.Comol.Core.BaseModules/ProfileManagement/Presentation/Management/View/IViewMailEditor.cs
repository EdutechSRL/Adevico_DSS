using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewMailEditor : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
       
        Int32 IdProfile { get; set; }
        long IdPendingRequest { get; set; }
        Boolean IsInitialized { get; set; }

        void InitializeControl(Int32 idProfile);

        void DisplayMailEditor();
        void DisplayWaitingCode(DateTime createdOn, String mailAddress);
        void DisplayProfileUnknown();
        void DisplaySessionTimeout();
        void DisplayError(ErrorMessages error);
        void DisplayActivationComplete(String mail);

       
    }

    public enum ErrorMessages
    {
        None = 0,
        InvalidCode = 1,
        InvalidMailAddress = 2,
        AlreadyActivated = 3,
        NoPendingRequest = 4,
        UnsavedRequest = 5,
        MailAlreadyExist = 6
    }
}
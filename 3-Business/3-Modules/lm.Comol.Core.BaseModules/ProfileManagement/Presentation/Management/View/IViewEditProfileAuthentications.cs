using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewEditProfileAuthentications : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 PreloadedIdProfile { get; }
        Int32 PreloadedIdProfileType { get; }

        Boolean AllowEditProfile { set; }
        Boolean AllowAddprovider {  set; }
        Boolean AllowManagement { set; }
        Int32 IdProfile { get; set; }
        Int32 IdProfileType { get; set; }


        void InitializeControl(Int32 idProfile);
        void SetTitle(String displayName);
        void DisplayNoPermission();
        void DisplaySessionTimeout();
        void DisplayProfileUnknown();
        void DisplayAddAuthentication();
    }
}
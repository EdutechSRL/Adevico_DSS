using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewEditProfileType : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 PreloadedIdProfile { get;  }
        Int32 PreloadedIdProfileType { get; }

        Boolean AllowManagement { set; }
        Boolean AllowEdit { get; set; }
        Int32 IdProfile { get; set; }
        Int32 IdProfileType { get; set; }
        Int32 SelectedIdProfileType { get; set; }
        
        void LoadProfileTypes(Int32 idCurrent, List<Int32> exceptItems);
        void LoadProfileName(string displayName);
        void LoadProfile(Int32 idProfile, Int32 idProfileType);
        
        //void LoadProfileData(dtoCompany companyUser, PersonInfo userData);

        //void InitializeControl(Int32 idProfile, Int32 idProfileType);

        void DisplayErrorSaving();
        void DisplaySessionTimeout();
        void DisplayProfileUnknown();
        void DisplayNoPermission();
        Boolean ValidateContent();
        void GotoManagement();
    }
}
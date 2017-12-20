using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation.View
{
    public interface iViewUcUserSettings : iViewBase
    {
        //void setParameters(
        Domain.DTO.DTO_UserSettings Settings { get; set; }
        //);
        
        //Domain.Enums.MailSettings usrSysSettings,
        //Domain.Enums.MailSettings manSysSettings,

        void ShowError(
            Domain.Enums.ViewSettingsUserError setError);


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    public class DTO_UserSettings
    {
        public bool isUserSysNotificationOn { get; set; }
        public bool isManagerSysNotificationOn { get; set; }

        public bool isUserNotificationOn { get; set; }
        public bool isManagerNotificationOn { get; set; }
        public bool isManager { get; set; }
        public bool isBehalfer { get; set; }
        public Domain.Enums.MailSettings Settings { get; set; }
        //public Domain.Enums.MailSettings manSettings { get; set; }

    }

    //public class DTO_UserSEttings_Set
    //{
    //    public bool isNotificationOn { get; set; }
    //    public bool isUserNotificationOn { get; set; }
    //    public bool isManagerNotificationOn { get; set; }
    //    public Domain.Enums.MailSettings Settings { get; set; }
    //}
}

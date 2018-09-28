using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    public class DTO_UserListFilters
    {
        public DTO_UserListFilters()
        {
            OrderBy = UserListOrderBy.SureName;
            OrderDir = true;

            SearchBy = UserListSearchBy.Surename;
            SearchString = "";

            UserType = UserListUsertype.all;
        }

        public UserListOrderBy OrderBy { get; set; }
        public Boolean OrderDir { get; set; }

        public UserListSearchBy SearchBy { get; set; }
        public String SearchString { get; set; }

        public UserListUsertype UserType { get; set; }
    }
}

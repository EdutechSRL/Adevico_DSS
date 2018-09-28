using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// X ora non usata....
namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoUserRole
    {
        public Int32 IdUser { get; set; }
        public Int32 IdRole { get; set; }

        public dtoUserRole() 
        { }
    }
}
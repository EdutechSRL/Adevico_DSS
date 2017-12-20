using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lm.Comol.Core.DomainModel
{
    [Serializable(), CLSCompliant(true)]
    public class dtoModulePermission
    {
        public Int32 IdModule { get; set; }
        public String ModuleCode { get; set; }
        public long Permissions { get; set; }

        public dtoModulePermission()
        {

        }

        public dtoModulePermission(String permission)
        {
            Permissions = Convert.ToInt64(new String(permission.Reverse().ToArray()), 2);
        }
    }
}
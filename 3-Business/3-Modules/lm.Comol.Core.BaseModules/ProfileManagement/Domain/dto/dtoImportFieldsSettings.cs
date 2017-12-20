using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable]
    public class dtoImportFieldsSettings : dtoImportSettings
    {
        public virtual List<ProfileColumnComparer<String>> Fields { get; set; }

        public dtoImportFieldsSettings(){
            Fields = new List<ProfileColumnComparer<String>>();
        }

    }
}
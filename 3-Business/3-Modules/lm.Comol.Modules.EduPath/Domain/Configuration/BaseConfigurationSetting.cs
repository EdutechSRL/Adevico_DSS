using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class BaseConfigurationSetting : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual Int32 IdCommunity { get; set; }
        public virtual Int32 IdOrganization { get; set; }
        public virtual Path Path { get; set; }
        
        public virtual Boolean IsEnabled { get; set; }
        public virtual Boolean ForAllPath { get; set; }
        public virtual ConfigurationType ConfigType { get; set; }
        
        public BaseConfigurationSetting()
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.EduPath.Domain.DTO
{
    [Serializable]
    public class dtoSubActivityLink
    {
        public virtual Int64 Id { get; set; }

        public virtual Int64 IdObject { get; set; }
        public virtual Int64 IdModule { get; set; }
        public virtual Int64 IdSubActivity { get; set; }

        public virtual String Name { get; set; }

        public virtual SubActivityLinkType ContentType { get; set; }

        public virtual Boolean Mandatory { get; set; }

        public virtual Boolean Visible { get; set; }

        public dtoSubActivityLink()
        {
        }

        public dtoSubActivityLink(SubActivityLink sal)
        {
            this.Id = sal.Id;
            this.IdObject = sal.IdObject;
            this.IdModule = sal.IdModule;

            if (sal.SubActivity != null)
            {
                this.IdSubActivity = sal.SubActivity.Id;
            }
            this.ContentType = sal.ContentType;
            this.Mandatory = sal.Mandatory;
            this.Visible = sal.Visible;
            
            //try
            //{
            //    if (sal.SubActivity != null)
            //    {
            //        this.IdSubActivity = sal.SubActivity.Id;
            //    }
            //}catch(Exception ex)
            //{
            //}
            

        }

        public dtoSubActivityLink(SubActivityLink sal, Int64 subactid)
            :this(sal)
        {
            this.IdSubActivity = subactid;
        }
        
    }
}

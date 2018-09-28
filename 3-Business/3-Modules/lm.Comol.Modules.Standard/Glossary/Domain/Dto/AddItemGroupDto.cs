using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.Glossary.Domain.Dto
{
    public class AddItemGroupDto
    {
        public Int64 Id { get; set; }

        public String Name { get; set; }
        public Boolean IsDefault { get; set; }


        public DefaultView DefaultView { get; set; }
        public Boolean IsPaged { get; set; }
        public Int32 ItemPerPage { get; set; }

        ///// <summary>
        ///// Owner ID: Actually CommunityID
        ///// </summary>
        //public virtual Int64 OwnerId { get; set; }

        ///// <summary>
        ///// Owner Text: for future. Actually only community. (Text = 0)
        ///// </summary>
        //public virtual Int32 OwnerType { get; set; }
    }
}

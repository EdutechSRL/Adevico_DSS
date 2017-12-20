
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
    public class PersonInfo
	{
        public virtual String RemoteUniqueID { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual String Note { get; set; }
        public virtual Int32 IdIstitution { get; set; }
        public virtual Int32 IdProvince { get; set; }
        public virtual Int32 IdNation { get; set; }
        public virtual String SecondaryMail { get; set; }
        public virtual Boolean DefaultShowMailAddress { get; set; }
        public virtual Boolean isInternalProfile { get; set; }
        //public virtual DateTime CreatedOn { get; set; }


        public virtual String BirthPlace { get; set; }
        public virtual Boolean IsMale { get; set; }
        public virtual String Address { get; set; }
        public virtual String PostCode { get; set; }
        public virtual String City { get; set; }
        public virtual String OfficePhone { get; set; }
        public virtual String HomePhone { get; set; }
        public virtual String Mobile { get; set; }
        public virtual String Fax { get; set; }
        public virtual String Homepage { get; set; }


        public PersonInfo()
		{
            isInternalProfile = true;
            IsMale = true;
            BirthPlace = "";
            Address = "";
            PostCode = "";
            OfficePhone = "";
            Mobile = "";
            Fax = "";
            Note = "";
            RemoteUniqueID = "";
		}
	}
}
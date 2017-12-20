
using System;
using System.Collections.Generic;
using System.Linq;

namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoMemberContact : DomainObject<int>, IComparable
	{
		public string Name {get;set;}
		public string Surname {get;set;}
		public string Mail{get;set;}
        public string DisplayName
        {
			get { return Surname + " " + Name; }
		}
		public string Login {get;set;}
		public string Other{get;set;}
		public Dictionary<int, string> Roles {get;set;}

		

		public List<string> AssignedRolesName()
		{
			return (from r in Roles select r.Value).ToList();
		}
		public List<int> AssignedRolesID()
		{
			return (from r in Roles select r.Key).ToList();
		}

		public override string ToString()
		{
			return DisplayName;
		}

        public int CompareTo(object obj)
		{
            if (typeof(dtoMemberContact) == obj.GetType())
                return this.Id.CompareTo(((dtoMemberContact)obj).Id);
            else
                return -1;

            //   Dim objContatto As dtoMemberContact
            //objContatto = CType(obj, dtoMemberContact)
            //Me.Id.CompareTo(objContatto.Id)

    	}
    }
}
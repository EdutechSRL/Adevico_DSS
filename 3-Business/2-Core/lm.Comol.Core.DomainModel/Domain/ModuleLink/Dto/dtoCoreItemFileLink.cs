using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable()]
	public class dtoCoreItemFileLink<T> : iCoreItemFileLink<T>
	{
     
	    public T  ItemFileLinkId {get;set;}
        public Person  Owner {get;set;}
        public Person  CreatedBy {get;set;}
        public DateTime?  CreatedOn {get;set;}
        public Person  ModifiedBy {get;set;}
        public DateTime?  ModifiedOn {get;set;}
        public BaseStatusDeleted  Deleted {get;set;}
        public ModuleLink  Link {get;set;}
        public BaseCommunityFile  File {get;set;}
        public bool  isVisible {get;set;}
        public long  StatusId {get;set;}
        public bool  isModified()
        {
 	       return !ModifiedBy.Equals(CreatedBy) && ! CreatedOn.Equals(ModifiedOn);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain.toRemove
{
    [Serializable]
    public class OldAssignments
    {
        public virtual long Id { get; set; }
        public virtual long IdCommunityFile { get; set; }
        public virtual Int32 Permission {get; set;}
        public virtual Int32 Discriminator {get; set;}
        public virtual Int32 IdCommunity {get; set;}
        public virtual Int32 IdRole {get; set;}
        public virtual Int32 IdPerson {get; set;}
        public virtual Int32 IdPersonType {get; set;}
        public virtual Boolean Denyed {get; set;}
        public virtual Boolean Inherited {get; set;}
        public virtual DateTime _CreatedOn {get; set;}
        public virtual Int32 _CreatedBy {get; set;}
        public virtual DateTime? _ModifiedOn {get; set;}
        public virtual Int32 _ModifiedBy {get; set;}

        public virtual Boolean Transferred { get; set; } 
    }
    [Serializable]
    public class NewAssignments
    {

            public virtual long Id { get; set; }
            public virtual long IdItem { get; set; }
        public virtual long Permissions { get; set; }
        public virtual Int32 Type {get; set;}
        public virtual Int32? IdCommunity {get; set;}
        public virtual Int32? IdRole {get; set;}
        public virtual Int32? IdPerson {get; set;}
        public virtual Boolean Denyed {get; set;}
        public virtual Boolean Inherited {get; set;}
        public virtual DateTime _CreatedOn {get; set;}
        public virtual Int32 _CreatedBy {get; set;}
        public virtual DateTime _ModifiedOn {get; set;}
        public virtual Int32 _ModifiedBy {get; set;}

        public virtual String _CreatedProxyIPaddress {get; set;}
        public virtual String _CreatedIPaddress {get; set;}
        public virtual String _ModifiedIPaddress {get; set;}
        public virtual String _ModifiedProxyIPaddress {get; set;}

        public virtual RepositoryIdentifier Repository { get; set; }
        

    }
}
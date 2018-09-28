using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable()]
    public class ProjectResource : DomainBaseObjectLiteMetaInfo<long>
    {
        public virtual Project Project { get; set; }
        public virtual litePerson Person { get; set; }
        public virtual String LongName { get; set; }
        public virtual String ShortName { get; set; }
        public virtual String Mail { get; set; }
        public virtual ResourceType Type { get; set; }
        public virtual ActivityRole ProjectRole { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }

        public virtual long ConfirmedActivities { get; set; }
        public virtual long CompletedActivities { get; set; }
        public virtual long LateActivities { get; set; }
        public virtual long AssignedActivities { get; set; }
        public virtual long StartedActivities { get; set; }
        public virtual Guid UniqueIdentifier { get; set; }
        public virtual long Number { get; set; }
        public virtual Boolean DefaultForActivity { get; set; }
        public ProjectResource() 
        {
        }

        public virtual String GetShortName()
        {
            switch (Type) { 
                case ResourceType.Internal:
                    return (Person == null) ? "R." + Id.ToString() : (String.IsNullOrEmpty(ShortName) ? GetPersonShortName() : ShortName);
                default:
                    return (String.IsNullOrEmpty(ShortName) ? "R." + Id.ToString() : ShortName);
            }
        }

        protected internal virtual String GetPersonShortName()
        {
            String name = Person.Name.Trim();
            return ((name.Length > 0) ? name[0].ToString() : Id.ToString()) + Person.FirstLetter;
        }

        public virtual String GetLongName()
        {
            switch (Type)
            {
                case ResourceType.Internal:
                    return (Person == null) ? GetShortName() : Person.SurnameAndName;
                default:
                    return (String.IsNullOrEmpty(LongName) ? GetShortName() : LongName);
            }
        }
    } 
}
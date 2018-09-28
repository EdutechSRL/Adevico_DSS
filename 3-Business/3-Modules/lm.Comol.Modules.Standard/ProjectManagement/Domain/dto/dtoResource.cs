using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoResource 
    {
        public virtual Int32 IdPerson { get; set; }
        public virtual long IdResource { get; set; }
        public virtual String LongName { get; set; }
        public virtual String ShortName { get; set; }
        public virtual Boolean DefaultForActivity { get; set; }
        public virtual ActivityRole ProjectRole { get; set; }
        public dtoResource() { }

        public dtoResource(ProjectResource resource, String unknownUser)
        {
            IdResource = resource.Id;
            switch (resource.Type)
            {
                case Domain.ResourceType.Internal:
                    if (resource.Person != null)
                    {
                        IdPerson = resource.Person.Id;
                        LongName = resource.Person.SurnameAndName;
                        ShortName = resource.GetShortName();
                    }
                    else
                        LongName = unknownUser;
                    break;
                case Domain.ResourceType.External:
                case Domain.ResourceType.Removed:
                    LongName = resource.LongName;
                    ShortName = resource.ShortName;
                    break;
            }

            DefaultForActivity = resource.DefaultForActivity;
            ProjectRole = resource.ProjectRole;
        }
        public dtoResource(liteResource resource, String unknownUser)
        {
            IdResource = resource.Id;
            switch (resource.Type)
            {
                case Domain.ResourceType.Internal:
                    if (resource.Person != null)
                    {
                        IdPerson = resource.Person.Id;
                        LongName = resource.Person.SurnameAndName;
                        ShortName = resource.GetShortName();
                    }
                    else
                        LongName = unknownUser;
                    break;
                case Domain.ResourceType.External:
                case Domain.ResourceType.Removed:
                    LongName = resource.LongName;
                    ShortName = resource.ShortName;
                    break;
            }

            DefaultForActivity = resource.DefaultForActivity;
            ProjectRole = resource.ProjectRole;
        }
        
        public dtoResource(dtoProjectResource resource)
        {
            IdResource = resource.IdResource;
            IdPerson = resource.IdPerson;
            LongName = resource.LongName;
            ShortName = resource.ShortName;
            DefaultForActivity = resource.DefaultForActivity;
            ProjectRole = resource.ProjectRole;
        }
    }
}
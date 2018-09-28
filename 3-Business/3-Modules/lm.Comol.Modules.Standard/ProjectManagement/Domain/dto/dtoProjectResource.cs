using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    [Serializable]
    public class dtoProjectResource : dtoResource
    {
        public virtual String Mail { get; set; }
        public virtual ResourceType ResourceType { get; set; }
        public virtual ProjectVisibility Visibility { get; set; }
        public virtual long ConfirmedActivities { get; set; }
        public virtual long CompletedActivities { get; set; }
        public virtual long LateActivities { get; set; }
        public virtual long AssignedActivities { get; set; }
        public virtual Guid UniqueIdentifier { get; set; }

        public virtual Boolean isOwner { get { return ProjectRole == ActivityRole.ProjectOwner; } }
        public virtual Boolean isManager { get; set; } //verifica che la risorsa sia manager o meno e ne imposta il TAssignment
        public virtual Boolean isValid { get; set; }  //verifico che la persona non sia stata cancellata e il TAssignment invece ancora esista
        public virtual Boolean AllowDelete { get; set; } //gestisce se si puo cancellare una risorsa
        public virtual Boolean AllowEdit { get; set; }
        public virtual EditingErrors DisplayErrors { get; set; }
        public virtual long Number { get; set; }
        public dtoProjectResource(){ }

        public dtoProjectResource(ProjectResource resource, String unknownUser) {
            DisplayErrors = EditingErrors.None;
            AllowEdit = true;
            if (resource != null)
            {
                IdResource = resource.Id;
                ProjectRole = resource.ProjectRole;
                Visibility = resource.Visibility;
                ResourceType = resource.Type;
                AssignedActivities = resource.AssignedActivities;
                LateActivities = resource.LateActivities;
                CompletedActivities = resource.CompletedActivities;
                ConfirmedActivities = resource.ConfirmedActivities;
                UniqueIdentifier = resource.UniqueIdentifier;
                Number = resource.Number;
                switch (resource.Type)
                {
                    case Domain.ResourceType.Internal:
                        if (resource.Person != null)
                        {
                            IdPerson = resource.Person.Id;
                            LongName = resource.Person.SurnameAndName;
                            ShortName = resource.GetShortName();
                            Mail = resource.Person.Mail;
                        }
                        else
                        {
                            ResourceType = Domain.ResourceType.Removed;
                            LongName = unknownUser;
                            Mail = "";
                        }
                        isValid = true;
                        break;
                    case  Domain.ResourceType.External:
                    case Domain.ResourceType.Removed:
                        LongName = resource.LongName;
                        ShortName = resource.ShortName;
                        Mail = resource.Mail;
                        isValid = true;
                        break;
                    default:
                        isValid=false;
                        break;
                }
                isManager = (ProjectRole == ActivityRole.Manager || ProjectRole ==ActivityRole.ProjectOwner ); 
            }
            else
                isValid = false;
        }
    }
}
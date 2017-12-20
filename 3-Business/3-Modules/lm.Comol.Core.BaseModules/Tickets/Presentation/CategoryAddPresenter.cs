using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Tickets;

namespace lm.Comol.Core.BaseModules.Tickets.Presentation
{
    public class CategoryAddPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        
            private TicketService service;

            protected virtual new View.iViewCategoryAdd View
            {
                get { return (View.iViewCategoryAdd)base.View; }
            }

            public CategoryAddPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.service = new TicketService(oContext);
            }
            public CategoryAddPresenter(iApplicationContext oContext, View.iViewCategoryAdd view)
                : base(oContext, view)
            {
                this.service = new TicketService(oContext);
            }

            private lm.Comol.Core.BaseModules.Tickets.ModuleTicket _module;
            private lm.Comol.Core.BaseModules.Tickets.ModuleTicket Module
            {
                get
                {
                    if ((_module == null))
                    {
                        Int32 idUser = UserContext.CurrentUserID;
                        _module = service.PermissionGetService(idUser, CurrentCommunityId);
                    }
                    return _module;
                }
            }

        #endregion

            public void InitView()
            {
                if (!CheckSessionAccess())
                    return;

                if (!(Module.ManageCategory || Module.Administration))
                {
                    View.SendAction(ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);

                    View.ShowNoPermission();
                    return;
                }

                //Begin Action
                List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));

                View.SendAction(ModuleTicket.ActionType.CategoryLoadManage, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                //End Action
                //int comTypeId = (from Community com in Man this.CurrentCommunityId

                this.View.Initialize(service.CategoryTypeGetPermission(this.CurrentCommunityId));

                this.UpdateManager();
            }

            public void Create()
            {
                if (!CheckSessionAccess())
                    return;

                if (!(Module.ManageCategory || Module.Administration))
                {
                    View.SendAction(ModuleTicket.ActionType.NoPermission, this.CurrentCommunityId, ModuleTicket.InteractionType.None);
                    View.ShowNoPermission();
                    return;
                }

                Int64 newCatId = service.CategoryCreateSimple(View.Name, View.Description, View.Type);


                if (newCatId > 0)
                {
                    IList<Domain.DTO.DTO_CategoryRole> Roles = new List<Domain.DTO.DTO_CategoryRole>();

                    Int32 USerId = View.SelectedManagerID;

                    //View.Roles;  //new List<Domain.DTO.DTO_CategoryRole>();
                    if (USerId <= 0)
                    {
                        Domain.DTO.DTO_CategoryRole ManRole = new Domain.DTO.DTO_CategoryRole();
                        ManRole.IsManager = true;
                        ManRole.PersonId = UserContext.CurrentUserID;
                        Roles.Add(ManRole);
                    } else
                    {
                        Domain.DTO.DTO_CategoryRole ManRole = new Domain.DTO.DTO_CategoryRole();
                        ManRole.IsManager = true;
                        ManRole.PersonId = USerId;
                        Roles.Add(ManRole);
                    }
                    
                    Domain.DTO.DTO_AddRolesResponse roleResponse = service.CategoryRolesAdd(newCatId, Roles);

                    //Begin Action
                    List<KeyValuePair<int, String>> Objects = new List<KeyValuePair<int, string>>();
                    Objects.Add(ModuleTicket.KVPgetPerson(this.UserContext.CurrentUserID));
                    Objects.Add(ModuleTicket.KVPgetCategory(newCatId));

                    View.SendAction(ModuleTicket.ActionType.CategoryCreate, this.CurrentCommunityId, ModuleTicket.InteractionType.UserWithLearningObject, Objects);
                    //End Action

                    //Gestire eventuali errori nell'aggiunta, tramite roleResponse...

                    //View.CurrentCategoryId = newCatId;
                    View.NavigateToEdit(newCatId);
                    //InitView();
                }
                else
                {
                    //Manage Error
                }
            }

            /// <summary>
            /// Comunità corrente. Da view (URL) se presente, altrimenti dalla sessione utente
            /// </summary>
            public Int32 CurrentCommunityId
            {
                get
                {
                    Int32 VComId = View.ViewCommunityId;
                    if (VComId > 0)
                    {
                        return VComId;
                    }
                    else
                    {
                        Int32 SysComId = UserContext.CurrentCommunityID;
                        View.ViewCommunityId = SysComId;
                        return SysComId;
                    }
                }
            }

            public bool CheckSessionAccess()
            {
                if (UserContext.isAnonymous)
                {
                    View.DisplaySessionTimeout(CurrentCommunityId);
                    return false;
                }

                Domain.DTO.DTO_Access Access = service.SettingsAccessGet(true);
                if (!(Access.IsActive && Access.CanManageCategory))
                {
                    View.ShowNoAccess();
                    return false;
                }

                return true;
            }

        public void UpdateManager()
        {
            Int32 UserId = View.SelectedManagerID;
            String UserName = "";
            Boolean isCurrent = false;

            if(UserId <= 0)
            {
                UserId = this.UserContext.CurrentUserID;
                isCurrent = true;
            }

            Person Person = service.PersonGet(UserId);
            if(Person != null)
            {
                UserName = Person.NameAndSurname;
            }
            
            View.UpdateUserName(UserName, isCurrent);
        }
    }
}

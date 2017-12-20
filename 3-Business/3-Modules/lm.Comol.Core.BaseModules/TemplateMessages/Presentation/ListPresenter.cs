using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class ListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewListTemplates View
            {
                get { return (IViewListTemplates)base.View; }
            }
            private TemplateMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new TemplateMessagesService(AppContext);
                    return service;
                }
            }
            public ListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ListPresenter(iApplicationContext oContext, IViewListTemplates view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout(RootObject.List(View.PreloadIdCommunity, View.PreloadTemplateType, View.PreloadOwnership, View.PreloadFromCookies, View.PreloadModuleCode, View.PreloadModulePermissions, View.PreloadModulePermissions));
            }
            else
            {
                Int32 idCommunity = View.PreloadIdCommunity;
                if (idCommunity < 0)
                    idCommunity = UserContext.CurrentCommunityID;
                View.IdManagerCommunity = idCommunity;
                TemplateType type = View.PreloadTemplateType;
                if (type == TemplateType.None)
                    type = TemplateType.User;
                dtoBaseFilters filters = (View.PreloadFromCookies) ? View.GetFromCookies() : null;
                if (filters == null)
                {
                    filters = new dtoBaseFilters();
                    filters.Ascending = true;
                    filters.SearchForName = "";
                    filters.Status = TemplateStatus.Active;
                    filters.TemplateType = type;
                    //filters.SetLoadingType(type); 
                    switch (type)
                    {
                        case TemplateType.Module:
                            filters.ModuleCode = View.PreloadModuleCode;
                            break;
                    }
                    filters.Ascending = true;
                    filters.OrderBy = TemplateOrder.ByName;
                    filters.TranslationsStatus = View.GetTranslationsStatus();
                    filters.TranslationsType = View.GetTranslationsTypes();
                    filters.TemplateDisplay = TemplateDisplay.OnlyVisible;
                    View.SaveToCookies(filters);
                }
                Int32 idUser = UserContext.CurrentUserID;
                Boolean allowView = false;
                Boolean allowAdd = false;
                String addUrl = "";

                dtoModuleContext context = new dtoModuleContext();
                context.ModuleCode = View.PreloadModuleCode;
                context.ModulePermissions = View.PreloadModulePermissions;
                context.IdCommunity = idCommunity;

                dtoBaseTemplateOwner ownerInfo = null;
                ModuleTemplateMessages module = null;

                switch (type) { 
                    case TemplateType.System:
                        module = ModuleTemplateMessages.CreatePortalmodule(UserContext.UserTypeID, OwnerType.System);
                        allowView = module.Administration || module.List;
                        allowAdd = module.Administration || module.Add;
                        if (allowAdd) {
                            ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = 0, IsPortal = true, Type = OwnerType.System, ModuleCode = View.PreloadModuleCode, IdModule= (String.IsNullOrEmpty(View.PreloadModuleCode) ? 0 : CurrentManager.GetModuleID(View.PreloadModuleCode)) };
                            addUrl = RootObject.Add(TemplateType.System, ownerInfo, idCommunity, View.PreloadModuleCode, View.PreloadModulePermissions);
                        }
                        context.LoaderType = TemplateLoaderType.System;
                        context.IdModule = Service.ServiceModuleID();
                        context.Permissions= new ModuleGenericTemplateMessages(module);
                        break;
                    case TemplateType.Module:
                        if (context.ModuleCode == ModuleTemplateMessages.UniqueCode)
                        {
                            context.IdModule = Service.ServiceModuleID();
                            module = Service.GetPermission(idCommunity, OwnerType.Module);
                            context.ModulePermissions = module.GetPermissions();
                            allowView = module.Administration || module.List;
                            allowAdd = module.Administration || module.Add;
                            if (allowAdd)
                            {
                                ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Module, ModuleCode = context.ModuleCode, IdModule = context.IdModule, ModulePermission = context.ModulePermissions };
                                addUrl = RootObject.Add(TemplateType.Module, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                            }
                            
                            context.Permissions = new ModuleGenericTemplateMessages(module);
                            context.LoaderType = TemplateLoaderType.Module;
                        }
                        else if (View.PreloadModulePermissions > 0)
                        {
                            context.IdModule = CurrentManager.GetModuleID(context.ModuleCode);
                            long dbPermissions = CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, context.IdModule);

                            context.Permissions = View.GetModulePermissions(context.ModuleCode, context.IdModule, dbPermissions, idCommunity, UserContext.UserTypeID);

                            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(dbPermissions, context.ModulePermissions))
                            {
                                allowView = true;
                                allowAdd = true;
                                if (allowAdd)
                                {
                                    ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Module, ModuleCode = context.ModuleCode, IdModule = context.IdModule, ModulePermission = context.ModulePermissions };
                                    addUrl = RootObject.Add(TemplateType.Module, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                                }
                            }
                            context.LoaderType = TemplateLoaderType.OtherModule;
                        }
                        else {
                            context.IdModule = CurrentManager.GetModuleID(context.ModuleCode);
                            context.Permissions = View.GetModulePermissions(context.ModuleCode, context.IdModule, GetModulePermission(idCommunity,context.IdModule), idCommunity, UserContext.UserTypeID);
                            context.LoaderType = TemplateLoaderType.OtherModule;

                            allowView = context.Permissions.Administration || context.Permissions.List;
                            allowAdd = context.Permissions.Administration || context.Permissions.Add;
                            if (allowAdd)
                            {
                                ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Module, ModuleCode = context.ModuleCode, IdModule = context.IdModule, ModulePermission = context.ModulePermissions };
                                addUrl = RootObject.Add(TemplateType.Module, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                            }
                        }
                        break;
                    case TemplateType.User:
                        Person p = GetCurrentUser(ref idUser);
                        allowView = (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser);
                        allowAdd = allowView;
                        if (allowAdd)
                        {
                            ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity==0), Type = OwnerType.Person, IdPerson= UserContext.CurrentUserID, ModuleCode = View.PreloadModuleCode, IdModule = (String.IsNullOrEmpty(View.PreloadModuleCode) ? 0 : CurrentManager.GetModuleID(View.PreloadModuleCode)) };
                            addUrl = RootObject.Add(TemplateType.User, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                        }
                        context.LoaderType = TemplateLoaderType.User;
                        context.Permissions = new ModuleGenericTemplateMessages("personal");
                        context.Permissions.Add = allowView;
                        context.Permissions.Administration = allowView;
                        context.Permissions.Clone = allowView;
                        context.Permissions.DeleteMyTemplates = allowView;
                        context.Permissions.Edit = allowView;
                        context.Permissions.List = allowView;
                        break;
                }
                if (allowView)
                {
                    if (allowAdd)
                        View.SetAddUrl(addUrl);
                    switch (type)
                    {
                        case TemplateType.User:
                        case TemplateType.System:
                            View.InitializeList(context, filters, GetAvailableTypes(type), GetAvailableDisplay(type));
                            break;
                        case  TemplateType.Module:
                            View.InitializeList(context, filters, GetAvailableTypes(type), GetAvailableDisplay(type));
                            break;
                    }
                }
                else
                    View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                
            }
        }

        private List<TemplateType> GetAvailableTypes(TemplateType type) {
            Int32 idCommunity = View.IdManagerCommunity;
            List<TemplateType> items = new List<TemplateType>();
            items.Add(type);

            if (idCommunity != 0 && type == TemplateType.Module) {
                items.Add(TemplateType.User);
                items.Add(TemplateType.System);
            }

            return items;
        }
        private List<TemplateDisplay> GetAvailableDisplay(TemplateType type)
        {
            Int32 idCommunity = View.IdManagerCommunity;
            List<TemplateDisplay> items = new List<TemplateDisplay>();
            switch (type) { 
                case TemplateType.System:
                    items.Add(TemplateDisplay.OnlyVisible);
                    items.Add(TemplateDisplay.Deleted);
                    items.Add(TemplateDisplay.All);
                    break;
                case TemplateType.Module:
                    items.Add(TemplateDisplay.OnlyVisible);
                    items.Add(TemplateDisplay.Deleted);
                    items.Add(TemplateDisplay.All);
                    break;
                case TemplateType.User:
                    items.Add(TemplateDisplay.OnlyVisible);
                    items.Add(TemplateDisplay.Deleted);
                    break;
            }
            return items;
        }

        public void SessionTimeout() {
            View.DisplaySessionTimeout(RootObject.List(View.PreloadIdCommunity, View.PreloadTemplateType, View.PreloadOwnership, View.PreloadFromCookies, View.PreloadModuleCode, View.PreloadModulePermissions, View.PreloadModulePermissions));
        }
        public long GetModulePermission(Int32 idCommunity, Int32 idModule)
        {
            return CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, idModule);
        }
        private Person GetCurrentUser(ref Int32 idUser)
        {
            Person person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetPerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetPerson(idUser);
            return person;
        }
    }
}
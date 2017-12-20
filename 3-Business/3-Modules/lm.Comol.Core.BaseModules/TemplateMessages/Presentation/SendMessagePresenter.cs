using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Business;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class SendMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            private lm.Comol.Core.Mail.Messages.MailMessagesService mailService;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSendMessage View
            {
                get { return (IViewSendMessage)base.View; }
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
            private lm.Comol.Core.Mail.Messages.MailMessagesService MailService
            {
                get
                {
                    if (mailService == null)
                        mailService = new lm.Comol.Core.Mail.Messages.MailMessagesService(AppContext);
                    return mailService;
                }
            }
            public SendMessagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SendMessagePresenter(iApplicationContext oContext, IViewSendMessage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Int32 idCommunity = View.PreloadIdCommunity;
            if (idCommunity < 0)
                idCommunity = UserContext.CurrentCommunityID;
            View.CurrentIdCommunity = idCommunity;
            if (UserContext.isAnonymous)
                SessionTimeout(idCommunity,View.PreloadSelectedTab);
            else
            {
                DisplayTab tabs = View.PreloadTabs;
                DisplayTab tab = View.PreloadSelectedTab;
                if (tab == DisplayTab.Sent && tabs !=  DisplayTab.None)
                    tab = DisplayTab.Send;
             
                TemplateType type = View.PreloadTemplateType;
                if (type == TemplateType.None)
                    type = TemplateType.Module;

                SetCurrentItems(type, idCommunity, tab);
                
               
                
                String moduleCode = View.PreloadModuleCode;
                long permissions = View.PreloadFromModulePermissions;
                Int32 idOtherModule = CurrentManager.GetModuleID(moduleCode);
                Boolean otherModule = (moduleCode != ModuleTemplateMessages.UniqueCode);

               
               
                dtoModuleContext context = GetModuleContext(moduleCode,permissions, idCommunity, type);
                LoadTabs(tabs, tab, context);
                Boolean allowView  = context.Permissions.Administration || context.Permissions.List;
                switch (tab) { 
                    case DisplayTab.List:
                        if (allowView)
                        {
                            InitializeList(context, idCommunity, type, permissions);
                            View.DisplayList();
                        }
                        else
                            View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        break;
                    case DisplayTab.Send:
                        LoadTab(DisplayTab.Send);
                        break;
                    case DisplayTab.Sent:
                        break;
                    default:
                        if (otherModule)
                            View.DisplayNoPermission(idCommunity, idOtherModule, moduleCode);
                        else
                            View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                        break;
                }
            }
        }

#region "Initializers"
    private void SetCurrentItems(TemplateType type, Int32 idCommunity, DisplayTab current)
    {
        View.CurrentModuleCode = View.PreloadModuleCode;
        View.AvailableTabs = View.PreloadTabs;
        View.SelectionMode = View.PreloadSelectionMode;
        View.AllowSelectTemplate = View.PreloadAllowSelectTemplate;
        View.CurrentIdTemplate = View.PreloadIdTemplate;
        View.CurrentIdVersion = View.PreloadIdVersion;
        View.CurrentIdAction = View.PreloadIdAction;
        View.AlsoWithEmptyActions = View.PreloadWithEmptyActions;
        View.CurrentIdCommunity = idCommunity;
        View.CurrentIdModule = View.PreloadIdModule;
        View.CurrentTemplateType = type;
        View.CurrentModuleObject = View.PreloadModuleObject;
        System.Guid sessionId = Guid.NewGuid();
        View.CurrentSessionId = sessionId;
        View.SetBackUrl(View.PreloadBackUrl, sessionId, GetCurrentUrl(current));
    }
    private void LoadTabs(DisplayTab tabs, DisplayTab tab, dtoModuleContext context)
    {
        List<DisplayTab> items = new List<DisplayTab>();
        if (tabs != DisplayTab.None) {
            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)tabs, (long)DisplayTab.List))
                items.Add(DisplayTab.List);
            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)tabs, (long)DisplayTab.Send))
                items.Add(DisplayTab.Send);
            if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft((long)tabs, (long)DisplayTab.Sent) && MailService.HasMessages(context.ModuleObject))
                items.Add(DisplayTab.Sent);
            if (!items.Contains(tab))
                tab = items.Where(t => t != DisplayTab.Sent).FirstOrDefault();
        }
        
        View.LoadTabs(items, tab);
    }

    private List<TemplateType> GetAvailableTypes(TemplateType type) {
        Int32 idCommunity = View.CurrentIdCommunity;
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
        Int32 idCommunity = View.CurrentIdCommunity;
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

    public void LoadTab(DisplayTab current) {
        if (UserContext.isAnonymous)
            SessionTimeout(View.CurrentIdCommunity,current);
        else {
            switch (current) { 
                case DisplayTab.List:
                    if (!View.IsInitializedList)
                        InitializeList(null, View.CurrentIdCommunity, View.CurrentTemplateType, View.ModulePermissions);
                    View.DisplayList();
                    break;
                case DisplayTab.Send:
                    if (!View.IsInitializedSender)
                    {
                        //GetAvailableSaveAs(),
                        View.InitializeSendMessage(false, View.CurrentIdCommunity, View.CurrentModuleCode);
                    }
                    View.DisplaySendMessage();
                    break;
            }
            View.SelectedTab = current;
        }    
    }
#endregion

    #region "Templates List"
        private void InitializeList(dtoModuleContext context, Int32 idCommunity, TemplateType type,long permissions)
        {
            dtoBaseTemplateOwner ownerInfo = null;
            Boolean allowAdd = false;
            String addUrl = "";
            String addPersonalUrl = "";
            String addObjectUrl = "";
            View.IsInitializedList = true;
            if (context == null)
                context = GetModuleContext(View.CurrentModuleCode, permissions, View.CurrentIdCommunity, type);

            #region "Permissions"
            allowAdd = context.Permissions.Administration || context.Permissions.Add;
            System.Guid sessionId = View.CurrentSessionId;
            switch (type)
            {
                case TemplateType.System:
                    if (allowAdd)
                    {
                        ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Module, ModuleCode = context.ModuleCode, IdModule = context.IdModule, ModulePermission = context.ModulePermissions };
                        addUrl = RootObject.Add(sessionId,TemplateType.System, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                    }
                    break;
                case TemplateType.Module:
                    allowAdd = context.Permissions.Administration || context.Permissions.Add;

                    // DA RIVEDERE
                    if (allowAdd)
                    {
                        ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Module, ModuleCode = context.ModuleCode, IdModule = context.IdModule, ModulePermission = context.ModulePermissions };
                        addUrl = RootObject.Add(sessionId, TemplateType.Module, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);

                        ModuleObject obj = View.CurrentModuleObject;
                        if (obj != null) { 
                            ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Object, IdObject=obj.ObjectLongID, IdObjectType= obj.ObjectTypeID, IdObjectCommunity= obj.CommunityID , IdObjectModule= obj.ServiceID, ModuleCode = context.ModuleCode, IdModule = context.IdModule, ModulePermission = context.ModulePermissions };
                            addObjectUrl = RootObject.Add(sessionId, TemplateType.Module, ownerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                        }
                    }
                    dtoBaseTemplateOwner personalOwnerInfo = new dtoBaseTemplateOwner() { IdCommunity = idCommunity, IsPortal = (idCommunity == 0), Type = OwnerType.Person, IdPerson = UserContext.CurrentUserID, ModuleCode = View.PreloadModuleCode, IdModule = (String.IsNullOrEmpty(View.PreloadModuleCode) ? 0 : CurrentManager.GetModuleID(View.PreloadModuleCode)) };
                    addPersonalUrl = RootObject.Add(sessionId, TemplateType.User, personalOwnerInfo, idCommunity, context.ModuleCode, context.ModulePermissions);
                    break;
                case TemplateType.User:
                    if (allowAdd)
                    {
                        ownerInfo = new dtoBaseTemplateOwner() { IdCommunity = 0, IsPortal = true, Type = OwnerType.Person, IdPerson = UserContext.CurrentUserID, ModuleCode = View.PreloadModuleCode, IdModule = (String.IsNullOrEmpty(View.PreloadModuleCode) ? 0 : CurrentManager.GetModuleID(View.PreloadModuleCode)) };
                        addPersonalUrl = RootObject.Add(sessionId, TemplateType.User, ownerInfo, 0, context.ModuleCode, context.ModulePermissions);
                    }
                    break;
            }
            #endregion
            #region "Filters"
            dtoBaseFilters filters = (View.PreloadFromCookies) ? View.GetFromCookies() : null;
            if (filters == null)
            {
                filters = new dtoBaseFilters();
                filters.Ascending = true;
                filters.SearchForName = "";
                filters.Status = TemplateStatus.Active;
                filters.TemplateType = type;
                //filters.LoaderType = context.LoaderType;
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
            #endregion
            View.InitializeList(context, filters, GetAvailableTypes(type), GetAvailableDisplay(type), allowAdd, addUrl, addPersonalUrl,addObjectUrl);
        }
    #endregion

    #region "Send Messages"
        public void LoadSendMessage(DisplayTab tab, long idTemplate, long idVersion) {
            View.SelectedTab = tab;
            View.IsInitializedSender = true;
            //GetAvailableSaveAs(),
            View.InitializeSendMessage(false, View.CurrentIdCommunity, View.CurrentModuleCode,idTemplate, idVersion);
            View.DisplaySendMessage();
        }
    #endregion

    #region "Permissions"
        private dtoModuleContext GetModuleContext(String moduleCode,long permissions, Int32 idCommunity, TemplateType type)
        {
            Int32 idUser = UserContext.CurrentUserID;
            dtoModuleContext context = new dtoModuleContext();
            context.ModuleCode = moduleCode;
            //context.ModulePermissions = View.PreloadModulePermissions;
            context.IdCommunity = idCommunity;
            context.IdAction = View.CurrentIdAction;
            context.AlsoEmptyActions = View.AlsoWithEmptyActions || (context.IdAction == 0);
            switch (type)
            {
                case TemplateType.Module:
                    if (context.ModuleCode == ModuleTemplateMessages.UniqueCode)
                    {
                        ModuleTemplateMessages module = null;
                        context.IdModule = Service.ServiceModuleID();
                        context.ModulePermissions = module.GetPermissions();
                        module = Service.GetPermission(idCommunity, OwnerType.Module);
                        context.Permissions = new ModuleGenericTemplateMessages(module);
                        context.LoaderType = TemplateLoaderType.Module;
                    }
                    else
                    {
                        context.IdModule = CurrentManager.GetModuleID(context.ModuleCode);
                        ModuleObject obj = View.CurrentModuleObject;
                        if (obj != null && obj.ServiceID == 0 && !String.IsNullOrEmpty(obj.ServiceCode))
                        {
                            obj.ServiceID = CurrentManager.GetModuleID(obj.ServiceCode);
                            View.CurrentModuleObject = obj;
                        }
                        else if (obj != null && obj.ServiceID > 0 && String.IsNullOrEmpty(obj.ServiceCode)){
                            obj.ServiceCode = CurrentManager.GetModuleCode(obj.ServiceID);
                            View.CurrentModuleObject = obj;
                        }
                        context.ModuleObject = obj;
                        //long dbPermissions = CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, context.IdModule);
                        //if (lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(dbPermissions, context.ModulePermissions))
                        //{
                        //    
                        //}
                        context.LoaderType = TemplateLoaderType.OtherModule;
                        if (permissions > 0)
                            context.Permissions = View.GetModulePermissions(context.ModuleCode, context.IdModule, CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, context.IdModule), idCommunity, UserContext.UserTypeID, obj);
                        else
                            context.Permissions = View.GetModulePermissions(context.ModuleCode, context.IdModule, GetModulePermission(idCommunity, context.IdModule), idCommunity, UserContext.UserTypeID, obj);
                    }
                    break;
                case TemplateType.User:
                    Person p = GetCurrentUser(ref idUser);
                    Boolean allowView = (p != null && p.TypeID != (Int32)UserTypeStandard.Guest && p.TypeID != (Int32)UserTypeStandard.PublicUser);
                   
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
            return context;
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
    #endregion     
   

        private List<OwnerType> GetAvailableSaveAs() {
            List<OwnerType> items = new List<OwnerType>();
            dtoModuleContext context = GetModuleContext(View.CurrentModuleCode, View.ModulePermissions, View.CurrentIdCommunity, View.CurrentTemplateType);
            ModuleTemplateMessages m = ModuleTemplateMessages.CreatePortalmodule(UserContext.UserTypeID, OwnerType.Person);
            if (m.Add || m.Administration)
                items.Add(OwnerType.Person);
            if (context.Permissions.Add || context.Permissions.Administration) {
                items.Add(OwnerType.Module);
                if (View.CurrentModuleObject != null)
                    items.Add(OwnerType.Object);
            }
            return items;
        }
        public void SessionTimeout(Int32 idCommunity, DisplayTab current) {
            View.DisplaySessionTimeout(idCommunity,GetCurrentUrl(current));
        }
        private String GetCurrentUrl(DisplayTab current)
        {
            switch (current)
            {
                case DisplayTab.List:
                    return ExternalModuleRootObject.ListAvailableTemplates(View.PreloadModuleCode, View.PreloadTabs, View.PreloadSelectionMode, View.PreloadAllowSelectTemplate, View.GetEncodedBackUrl(View.PreloadBackUrl), View.PreloadIdAction, View.PreloadWithEmptyActions, View.PreloadIdTemplate, View.PreloadIdVersion, View.PreloadIdCommunity, View.PreloadIdModule, View.PreloadModuleObject);
                case DisplayTab.Send:
                    return ExternalModuleRootObject.SendMessage(View.PreloadModuleCode, View.PreloadTabs, View.PreloadSelectionMode, View.PreloadAllowSelectTemplate, View.GetEncodedBackUrl(View.PreloadBackUrl), View.PreloadIdAction, View.PreloadWithEmptyActions, View.PreloadIdTemplate, View.PreloadIdVersion, View.PreloadIdCommunity, View.PreloadIdModule, View.PreloadModuleObject);
                case DisplayTab.Sent:
                    return ExternalModuleRootObject.MessagesSent(View.PreloadModuleCode, View.PreloadTabs, View.PreloadSelectionMode, View.PreloadAllowSelectTemplate, View.GetEncodedBackUrl(View.PreloadBackUrl), View.PreloadIdAction, View.PreloadWithEmptyActions, View.PreloadIdTemplate, View.PreloadIdVersion, View.PreloadIdCommunity, View.PreloadIdModule, View.PreloadModuleObject);
                default:
                    return "";
            }
        }
     
    }
}
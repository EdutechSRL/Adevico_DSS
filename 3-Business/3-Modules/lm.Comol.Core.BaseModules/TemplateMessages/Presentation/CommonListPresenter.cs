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
    public class CommonListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private TemplateMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCommonTemplateList View
            {
                get { return (IViewCommonTemplateList)base.View; }
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
            public CommonListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CommonListPresenter(iApplicationContext oContext, IViewCommonTemplateList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        //public void InitView(dtoBaseFilters filter, List<TemplateType> availableType, List<TemplateDisplay> availableDisplay, Boolean displayAdd = false, String addUrl = "", String addPersonalUrl = "")
        //{
        //    if (UserContext.isAnonymous)
        //        View.DisplaySessionTimeout();
        //    else
        //    {
        //        //InitializeView
        //    }
        //}
        public void InitView(dtoModuleContext context, dtoBaseFilters filter, List<TemplateType> availableType, List<TemplateDisplay> availableDisplay, Boolean displayAdd = false, String addUrl = "", String addPersonalUrl = "", String addObjectUrl = "")
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                //if (module.List || module.Administration){
                if (displayAdd && !String.IsNullOrEmpty(addUrl) && String.IsNullOrEmpty(addPersonalUrl) && String.IsNullOrEmpty(addObjectUrl)) //&& module.Add
                    View.SetAddUrl(addUrl);
                else if (displayAdd && (!String.IsNullOrEmpty(addPersonalUrl) || !String.IsNullOrEmpty(addObjectUrl))) //&& module.Add
                    View.SetAddUrl(addUrl, addPersonalUrl, addObjectUrl);
                InitializeView(context, filter, availableType, availableDisplay);
                //}
                //else
                //    View.DisplayNoPermission();
            }
        }
        private void InitializeView(dtoModuleContext context, dtoBaseFilters filters, List<TemplateType> availableType, List<TemplateDisplay> availableDisplay)
        {
            View.AvailableDisplay = availableDisplay;
            View.AvailableTypes = availableType;
            View.ContainerContext = context;
            View.CurrentType = filters.TemplateType;
            View.CurrentFilters = filters;
            
            List<TemplateDisplay> items = Service.GetAvailableTemplateDisplay(context.GetStandardType(), context.IdCommunity, UserContext.CurrentUserID, context.ModuleCode);
            items = items.Where(i => availableDisplay.Contains(i)).ToList();
            if (!items.Contains(filters.TemplateDisplay))
                filters.TemplateDisplay = items[0];
            View.LoadTemplateDisplay(items, filters.TemplateDisplay);
            //filters.CallType = type;
            //View.LoadSubmissionStatus(items, filters.Status);

            InternalTemplates(context, filters, filters.PageIndex, (filters.PageSize == 0) ? View.CurrentPageSize : filters.PageSize);
        }
        public void LoadTemplates(dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            View.CurrentFilters = filters;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else if (context.Permissions == null || (context.Permissions.Administration || context.Permissions.List))
                InternalTemplates(context, filters, pageIndex, pageSize);
            else
                View.DisplayNoPermission(context.IdCommunity, context.IdModule);
        }
        private void InternalTemplates(dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            List<dtoDisplayTemplateDefinition> items = Service.GetTemplates(context, filters,View.UnknownUserName, UserContext.CurrentUserID, UserContext.Language.Id);
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;

            if (pageSize == 0)
                pageSize = 50;
            pager.Count = (items==null)? -1 : (int)items.Count - 1;
            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;

            View.CurrentOrderBy = filters.OrderBy;
            //View.CurrentFilterBy = filters.Status;
            View.CurrentAscending = filters.Ascending;
            View.CurrentPageSize = pageSize;
            if (pager.Count < 0)
                View.LoadNoTemplatesFound();
            else
                View.LoadTemplates(items.Skip(pageIndex*pageSize).Take(pageSize).ToList());
            if (View.SendTemplateActions)
                View.SendUserAction(context.IdCommunity, context.IdModule, ModuleTemplateMessages.ActionType.ListTemplates);
        }

        public void VirtualDeleteTemplate(long idTemplate,String name, Boolean delete, dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean completed = Service.VirtualDeleteTemplate(idTemplate, delete);
                View.DisplayMessage(name,(delete) ? Domain.ListAction.VirtualDelete : Domain.ListAction.VirtualUndelete, ModuleTemplateMessages.ObjectType.Template, completed);

                if (View.SendTemplateActions)
                    View.SendUserAction(context.IdCommunity, context.IdModule, (completed) ? ModuleTemplateMessages.ActionType.VirtualDeleteTemplate : ModuleTemplateMessages.ActionType.GenericError);
                ReloadPageAndFilters(context, filters, pageIndex, pageSize);

                ////switch (View.CallType)
                ////{
                ////    case CallForPaperType.CallForBids:
                ////        View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission, ModuleCallForPaper.ActionType.VirtualDeleteSubmission);
                ////        break;
                ////    case CallForPaperType.RequestForMembership:
                ////        View.SendUserAction(View.IdCallCommunity, View.IdCallModule, idSubmission, ModuleRequestForMembership.ActionType.VirtualDeleteSubmission);
                ////        break;
                ////}
                ////LoadSubmissions(View.IdCallCommunity, type, filters, pageIndex, pageSize);
                //View.GotoUrl(RootObject.ViewSubmissions(type, View.IdCall, 0, 0, View.PreloadView, filters.Status, filters.OrderBy, filters.Ascending, pageIndex, pageSize));
            }
        }
        public void PhisicalDeleteTemplate(long idTemplate, String name, dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Boolean completed = Service.PhisicalDeleteTemplate(idTemplate);
                View.DisplayMessage(name,Domain.ListAction.Delete, ModuleTemplateMessages.ObjectType.Template, completed);

                if (View.SendTemplateActions)
                    View.SendUserAction(context.IdCommunity, context.IdModule, (completed) ? ModuleTemplateMessages.ActionType.PhisicalDeleteTemplate : ModuleTemplateMessages.ActionType.GenericError);
                ReloadPageAndFilters(context, filters, pageIndex, pageSize);
            }
        }
        public void CloneTemplate(long idTemplate, String name, String clonePrefix, dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                TemplateDefinition t = Service.CloneTemplate(idTemplate, clonePrefix,context);
                View.OpenIdTemplate = (t!=null) ? t.Id :idTemplate;
                View.DisplayMessage(name, Domain.ListAction.Clone, ModuleTemplateMessages.ObjectType.Template, (t != null));

                if (View.SendTemplateActions)
                    View.SendUserAction(context.IdCommunity, context.IdModule, (t!=null) ? ModuleTemplateMessages.ActionType.CloneTemplate : ModuleTemplateMessages.ActionType.GenericError);
                ReloadPageAndFilters(context, filters, pageIndex, pageSize);
            }
        }
        public void AddNewVersion(long idTemplate, String name, dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                TemplateDefinitionVersion v = Service.AddTemplateVersion(idTemplate);
                View.OpenIdTemplate = idTemplate;
                View.DisplayMessage(name, Domain.ListAction.AddVersion, ModuleTemplateMessages.ObjectType.Template, (v != null));

                if (View.SendTemplateActions)
                    View.SendUserAction(context.IdCommunity, context.IdModule, (v != null) ? ModuleTemplateMessages.ActionType.AddNewTemplateVersion : ModuleTemplateMessages.ActionType.GenericError);
                ReloadPageAndFilters(context, filters, pageIndex, pageSize);
            }
        }
        public void VirtualDeleteVersion(long idTemplate, long idVersion, String name, Boolean delete, dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.OpenIdTemplate = idTemplate;
                Boolean completed = Service.VirtualDeleteVersion(idVersion, delete);
                View.DisplayMessage(name, (delete) ? Domain.ListAction.VirtualDelete : Domain.ListAction.VirtualUndelete, ModuleTemplateMessages.ObjectType.Version, completed);

                if (View.SendTemplateActions)
                    View.SendUserAction(context.IdCommunity, context.IdModule, (completed) ? ModuleTemplateMessages.ActionType.VirtualDeleteTemplateVersion : ModuleTemplateMessages.ActionType.GenericError);
                ReloadPageAndFilters(context, filters, pageIndex, pageSize);
            }
        }
        public void PhisicalDeleteVersion(long idTemplate, long idVersion, String name, dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.OpenIdTemplate = idTemplate;
                Boolean completed = Service.PhisicalDeleteVersion(idVersion);
                View.DisplayMessage(name, Domain.ListAction.Delete, ModuleTemplateMessages.ObjectType.Version, completed);

                if (View.SendTemplateActions)
                    View.SendUserAction(context.IdCommunity, context.IdModule, (completed) ? ModuleTemplateMessages.ActionType.PhisicalDeleteTemplateVersion : ModuleTemplateMessages.ActionType.GenericError);
                ReloadPageAndFilters(context, filters, pageIndex, pageSize);
            }
        }

        private void ReloadPageAndFilters(dtoModuleContext context, dtoBaseFilters filters, int pageIndex, int pageSize)
        {
            List<TemplateDisplay> items = Service.GetAvailableTemplateDisplay(context.GetStandardType(), context.IdCommunity, UserContext.CurrentUserID, context.ModuleCode);
            items = items.Where(i => View.AvailableDisplay.Contains(i)).ToList();
            if (!items.Contains(filters.TemplateDisplay)){
                filters.TemplateDisplay = items[0];
                filters.PageIndex=0;
                pageIndex=0;
            }

            View.ReloadPageAndFilters(filters, items, filters.TemplateDisplay);
            LoadTemplates(context, filters, pageIndex, pageSize);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.TemplateMessages;
namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class TemplateSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region initClass
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService service;
            private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService Service
            {
                get
                {
                    if (service == null)
                        service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(AppContext);
                    return service;
                }
            }
            protected virtual IViewTemplateSelector View
            {
                get { return (IViewTemplateSelector)base.View; }
            }

            public TemplateSelectorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            public TemplateSelectorPresenter(iApplicationContext oContext, IViewTemplateSelector view)
                : base(oContext, view)
            {
                service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(ModuleGenericTemplateMessages permissions, lm.Comol.Core.Notification.Domain.NotificationChannel channel, long idAction, Int32 idModule, String moduleCode, Int32 idCommunty, Int32 idOrganization = 0, Boolean forPortal = false, long idTemplate = 0, long idVersion = 0, lm.Comol.Core.DomainModel.ModuleObject obj = null, List<dtoTemplateItem> items = null)
        {
            dtoSelectorContext c = new dtoSelectorContext();
            c.IdAction=idAction;
            c.IdModule= (idModule > 0) ? idModule : (!String.IsNullOrEmpty(moduleCode) ? CurrentManager.GetModuleID(moduleCode) : 0);
            c.ModuleCode= (!String.IsNullOrEmpty(moduleCode)) ? moduleCode : (idModule > 0 ? CurrentManager.GetModuleCode(idModule) : "");
            c.IdCommunity = idCommunty;
            c.IdOrganization= idOrganization;
            if (idCommunty > 0 && idOrganization <= 0)
            {
                Community community = CurrentManager.GetCommunity(idCommunty);
                c.IdOrganizationCommunity = (community == null) ? 0 : ((community.IdFather == 0) ? community.Id : CurrentManager.GetIdCommunityFromOrganization(community.IdOrganization));
            }
            else if (idOrganization > 0)
                c.IdOrganizationCommunity = CurrentManager.GetIdCommunityFromOrganization(idOrganization);
            c.IsForPortal = (forPortal && idCommunty == 0 && idOrganization == 0);
            c.ObjectOwner = obj;
            InitView(permissions, channel, c, idTemplate, idVersion, true, items);
        }
        public void InitView(ModuleGenericTemplateMessages permissions, lm.Comol.Core.Notification.Domain.NotificationChannel channel, dtoSelectorContext context, long idTemplate, long idVersion, Boolean isVerifyed = false, List<dtoTemplateItem> items = null)
        {
            View.isInitialized = true;
            if (!isVerifyed) {
                if (context.IdModule <= 0 && !String.IsNullOrEmpty(context.ModuleCode))
                    context.IdModule = CurrentManager.GetModuleID(context.ModuleCode);
                if (context.IdModule > 0 && String.IsNullOrEmpty(context.ModuleCode))
                    context.ModuleCode = CurrentManager.GetModuleCode(context.IdModule);
                context.IsForPortal = (context.IsForPortal && context.IdCommunity == 0 && context.IdOrganization == 0);
                if (context.IdOrganizationCommunity <= 0)
                {
                    if (context.IdOrganization > 0)
                        context.IdOrganizationCommunity = CurrentManager.GetIdCommunityFromOrganization(context.IdOrganization);
                    else if (context.IdCommunity > 0 && context.IdOrganization <= 0)
                    {
                        Community community = CurrentManager.GetCommunity(context.IdCommunity);
                        context.IdOrganizationCommunity = (community == null) ? 0 : ((community.IdFather == 0) ? community.Id : CurrentManager.GetIdCommunityFromOrganization(community.IdOrganization));
                    }
                }
            }
            dtoTemplateItem current = Service.GetDefaultAutomaticTemplate(context,channel);
            if (UserContext.isAnonymous)
            {
                LoadCurrentTemplate(current);
                View.AllowPreview = false;
                View.AllowSelect = false;
            }
            else
                LoadItems(permissions, channel, current, context, idTemplate, idVersion, items);
        }
        private void LoadItems(ModuleGenericTemplateMessages permissions, lm.Comol.Core.Notification.Domain.NotificationChannel channel, dtoTemplateItem current, dtoSelectorContext context, long idTemplate, long idVersion, List<dtoTemplateItem> items = null)
        {
            if (items==null)
                items = Service.GetAvailableTemplates(permissions,context, idTemplate, idVersion, channel);
            if (items.Count == 0)
            {
                View.LoadEmptyTemplate();
                View.AllowPreview = false;
                View.AllowSelect = false;
            }
            else
                View.LoadTemplates(items);
        }
        private void LoadCurrentTemplate(dtoTemplateItem item)
        {
            List<dtoTemplateItem> items = new List<dtoTemplateItem>();
            if (item == null)
                View.LoadEmptyTemplate();
            else
            {
                items.Add(item);
                View.LoadTemplates(items);
            }
        }
        //public void SelectSkin(DtoDisplaySkin skin)
        //{
        //    View.AllowEditSelection = skin != null && (skin.Type == SkinDisplayType.Module) && View.AllowEdit;
        //    View.AllowPreview = skin != null && (skin.Type != SkinDisplayType.Empty);
        //    View.AllowDelete = View.AllowEdit && skin != null && skin.Type == SkinDisplayType.Module && !Service.SkinHasMultipleAssociations(skin.Id, View.Source);
        //}

        //public Boolean SaveSkinAssociation(DtoDisplaySkin skin, lm.Comol.Core.DomainModel.ModuleObject source)
        //{
        //    if (UserContext.isAnonymous)
        //        return false;
        //    else
        //        return Service.SaveSkinAssociation(skin, source.CommunityID, source);
        //}

        //public Boolean HasSkinAssociation(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName)
        //{
        //    lm.Comol.Core.DomainModel.ModuleObject source = new lm.Comol.Core.DomainModel.ModuleObject() { FQN = fullyQualifiedName, ObjectTypeID = idItemType, ObjectLongID = idModuleItem, ServiceID = idModule };
        //    return Service.ObjectHasSkinAssociation(idCommunity, source);

        //}
        //public Boolean DeleteSKin(DtoDisplaySkin skin, lm.Comol.Core.DomainModel.ModuleObject source, String basePath)
        //{
        //    Boolean result = Service.DeleteModuleSkin(skin.Id, true, basePath);
        //    if (result)
        //    {
        //        LoadSkins(source.ServiceID, source.CommunityID, source.ObjectLongID, source.ObjectTypeID, Service.GetDefaultSkinForModule(source.ServiceID, source.CommunityID, source.ObjectLongID, source.ObjectTypeID), View.LoadModuleSkinBy);

        //    }
        //    return result;
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.TemplateMessages.Domain;
namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public class TemplateAssociationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region initClass
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
            protected virtual IViewTemplateAssociation View
            {
                get { return (IViewTemplateAssociation)base.View; }
            }

            public TemplateAssociationPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            public TemplateAssociationPresenter(iApplicationContext oContext, IViewTemplateAssociation view)
                : base(oContext, view)
            {
                service = new lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView( long idAction,Int32 idCommunity, long idTemplate, long idVersion, Int32 idModule = 0, String moduleCode = "", lm.Comol.Core.DomainModel.ModuleObject source = null)
        {
            View.isInitialized = true;
            //View.Source = source;
            //View.CurrentIdModule = idModule;
            //View.CurrentModuleCode = moduleCode;
            //View.CurrentIdAction = idAction;

            dtoTemplateItem current = Service.GetDefaultTemplate(idAction, idCommunity, idModule, moduleCode, source, idTemplate, idVersion);
            if (UserContext.isAnonymous)
            {
                LoadCurrentTemplate(current);
                View.AllowPreview = false;
                View.AllowSelect = false;
            }
            else
                LoadItems(current, idAction,idCommunity, idTemplate, idVersion, idModule, moduleCode, source);
        }
        private void LoadItems(dtoTemplateItem current, long idAction,Int32 idCommunity, long idTemplate, long idVersion, Int32 idModule = 0, String moduleCode = "", lm.Comol.Core.DomainModel.ModuleObject source = null)
        {
            List<dtoTemplateItem> items = Service.GetAvailableTemplates(idAction, idCommunity, idModule, moduleCode, source, lm.Comol.Core.Notification.Domain.NotificationChannel.Mail, idTemplate, idVersion);
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
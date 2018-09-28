using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.Skin.Domain;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class ModuleSkinSelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initalize"
            private Business.ServiceSkin _Service;
            private Business.ServiceSkin Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new Business.ServiceSkin(AppContext);
                    return _Service;
                }
            }

            public ModuleSkinSelectorPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleSkinSelectorPresenter(iApplicationContext oContext, IViewModuleSkinSelector view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewModuleSkinSelector View
            {
                get { return (IViewModuleSkinSelector)base.View; }
            }
        #endregion

            public void InitView(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName, Boolean allowAdd, Boolean allowEdit, LoadItemsBy loadModuleSkinBy)
            {

                lm.Comol.Core.DomainModel.ModuleObject item = new lm.Comol.Core.DomainModel.ModuleObject();
                item.CommunityID = idCommunity;
                item.ObjectLongID = idModuleItem;
                item.ObjectTypeID = idItemType;
                item.ServiceID = idModule;
                item.FQN=fullyQualifiedName;
                item.ServiceCode = Service.GetModuleCode(idModule);
                View.Source = item;

                View.AllowAdd = allowAdd;
                View.AllowEdit = allowEdit;
                View.AllowEditSelection = false;

                DtoDisplaySkin currentSkin = Service.GetDefaultSkinForModule(idModule, idCommunity, idModuleItem, idItemType);
                if (UserContext.isAnonymous)
                {
                    LoadCurrentSkin(currentSkin);
                    View.DisplaySessionTimeout();
                }
                else
                    LoadSkins(idModule, idCommunity, idModuleItem, idItemType, currentSkin, loadModuleSkinBy);
                View.isInitialized = true;
            }
        private void LoadSkins(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, DtoDisplaySkin currentSkin,LoadItemsBy loadModuleSkinBy)
        {
            List<DtoDisplaySkin> items = Service.GetAvailableSkins(idModule, idCommunity, idModuleItem, idItemType, View.FullSkinManagement,loadModuleSkinBy);
            if (items.Count == 0)
                View.LoadEmptySkin();
            else
            {
                View.LoadSkins(items, currentSkin);
                SelectSkin(currentSkin);
            }
        }
        private void LoadCurrentSkin(DtoDisplaySkin skin)
        {
            List<DtoDisplaySkin> items = new List<DtoDisplaySkin>();
            items.Add(skin);
            View.LoadSkins(items, skin);
            SelectSkin(skin);
            
        }
        public void SelectSkin(DtoDisplaySkin skin) {
            View.AllowEditSelection = skin!=null && (skin.Type == SkinDisplayType.Module) && View.AllowEdit;
            View.AllowPreview = skin != null && (skin.Type != SkinDisplayType.Empty);
            View.AllowDelete = View.AllowEdit && skin != null && skin.Type == SkinDisplayType.Module && !Service.SkinHasMultipleAssociations(skin.Id, View.Source);
        }

        public Boolean SaveSkinAssociation(DtoDisplaySkin skin, lm.Comol.Core.DomainModel.ModuleObject source)
        {
            if (UserContext.isAnonymous)
                return false;
            else
                return Service.SaveSkinAssociation(skin, source.CommunityID, source);
        }

        public Boolean HasSkinAssociation(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName)
        {
            lm.Comol.Core.DomainModel.ModuleObject source = new lm.Comol.Core.DomainModel.ModuleObject() { FQN = fullyQualifiedName, ObjectTypeID = idItemType, ObjectLongID = idModuleItem, ServiceID = idModule };
            return Service.ObjectHasSkinAssociation(idCommunity, source);

        }
        public Boolean DeleteSKin(DtoDisplaySkin skin,lm.Comol.Core.DomainModel.ModuleObject source, String basePath)
        {
            Boolean result = Service.DeleteModuleSkin(skin.Id, true, basePath);
            if (result){
                LoadSkins(source.ServiceID, source.CommunityID, source.ObjectLongID, source.ObjectTypeID, Service.GetDefaultSkinForModule(source.ServiceID, source.CommunityID, source.ObjectLongID, source.ObjectTypeID), View.LoadModuleSkinBy);
                
            }
            return result;
        }
    }
 }
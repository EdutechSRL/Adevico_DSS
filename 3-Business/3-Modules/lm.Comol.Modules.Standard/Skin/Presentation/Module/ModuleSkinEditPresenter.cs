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
    public class ModuleSkinEditPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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

            public ModuleSkinEditPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleSkinEditPresenter(iApplicationContext oContext, IViewModuleSkinEdit view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewModuleSkinEdit View
            {
                get { return (IViewModuleSkinEdit)base.View; }
            }
        #endregion

            public void InitView(long idSkin, Int32 idModule, Int32 idCommunity, long idItem, Int32 idItemType)
            {
                View.isInitialized = true;
                View.IdSkin = idSkin;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else {
                    lm.Comol.Core.DomainModel.ModuleObject item = new lm.Comol.Core.DomainModel.ModuleObject();
                    item.CommunityID = idCommunity;
                    item.ObjectLongID = idItem;
                    item.ObjectTypeID = idItemType;
                    item.ServiceID = idModule;
                    item.ServiceCode = Service.GetModuleCode(idModule);
                    View.Source = item;
                    Domain.Skin skin = Service.GetItem<Domain.Skin>(idSkin);
                    if (skin==null)
                        View.DisplayUnknownItem();
                    else{
                        SkinType type = (skin.IsPortal) ? SkinType.Portal : (skin.IsModule) ? SkinType.Module : (skin.Communities.Count > 0) ? SkinType.Community : (skin.Organizations.Count > 0) ? SkinType.Organization : SkinType.NotAssigned;
                        View.SkinType = type;
                        View.AllowEdit = (type == SkinType.Module && View.HasPermissionForItem(item)) || (type != SkinType.Module && View.FullSkinManagement);
                        if ((type == SkinType.Module && View.HasPermissionForItem(item)) || (type != SkinType.Module && View.FullSkinManagement))
                        {
                            View.LoadAvailableViews(Service.GetAvailableViews(type));
                            View.LoadSkinInfo(skin.Id, skin.Name, true, skin.OverrideVoidFooterLogos);
                        }
                        else
                            View.DisplayNoPermission();

                    }
                    View.BackUrl = View.PreloadedBackUrl;
                }
            }

            public Boolean SaveSkinName(String name, bool OverrideFooterLogos) {
                if (View.IdSkin == 0)
                    return false;
                else{
                    Skin.Domain.Skin skin = Service.SaveSkin(View.IdSkin,name, OverrideFooterLogos);
                    Service.CleanCache();
                    return (skin != null);
                }
            }

    }
 }
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
    public class ModuleSkinDeletePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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

            public ModuleSkinDeletePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleSkinDeletePresenter(iApplicationContext oContext, IViewModuleSkinDelete view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewModuleSkinDelete View
            {
                get { return (IViewModuleSkinDelete)base.View; }
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
                        View.AllowDelete = (type == SkinType.Module && View.HasPermissionForItem(item)) || (type != SkinType.Module && View.FullSkinManagement);
                        if ((type == SkinType.Module && View.HasPermissionForItem(item)) || (type != SkinType.Module && View.FullSkinManagement))
                            View.LoadSkinInfo(skin.Id, skin.Name, true);
                        else
                            View.DisplayNoPermission();
                    }
                    View.BackUrl = View.PreloadedBackUrl;
                }
            }

            public Boolean DeleteSkin(long idSkin,String basePath) {
                if (idSkin == 0)
                    return false;
                else {
                    switch (View.SkinType)
                    {
                        case SkinType.Module:
                            return Service.DeleteModuleSkin(idSkin, true, basePath);
                        default:
                            return Service.DELETE_Skin(idSkin, basePath);
                    }
                }
            }
    }
 }
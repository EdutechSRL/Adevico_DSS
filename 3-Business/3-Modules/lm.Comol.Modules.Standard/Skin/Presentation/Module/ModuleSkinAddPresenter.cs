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
    public class ModuleSkinAddPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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

            public ModuleSkinAddPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleSkinAddPresenter(iApplicationContext oContext, IViewModuleSkinAdd view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            protected virtual IViewModuleSkinAdd View
            {
                get { return (IViewModuleSkinAdd)base.View; }
            }
        #endregion

            public void InitView(Int32 idModule,Int32 idCommunity, long idItem, Int32 idItemType, SkinType type)
            {
                View.isInitialized = true;
                View.SkinType = type;
                View.IdSkin = (long)0;
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else {
                    lm.Comol.Core.DomainModel.ModuleObject item = new lm.Comol.Core.DomainModel.ModuleObject();
                    item.CommunityID = idCommunity;
                    item.ObjectLongID = idItem;
                    item.ObjectTypeID = idItemType;
                    item.ServiceID = idModule;
                    item.ServiceCode = Service.GetModuleCode(idModule);
                    if (View.HasPermissionForItem(item))
                        View.DisplayForm(item,true);
                    else
                    {
                        View.AllowAdd = false;
                        View.Source = item;
                        View.DisplayNoPermission();
                    }
                    View.BackUrl = View.PreloadedBackUrl;
                }
            }

            public Boolean AddSkin(String name,String skinPath) {
                if(View.IdSkin!=0)
                    return true;
                else{
                    Skin.Domain.Skin skin = Service.AddSkin(name, skinPath, View.Source);
                    if (skin != null)
                        View.IdSkin = skin.Id;
                    return (skin != null);
                }
            }
    }
 }
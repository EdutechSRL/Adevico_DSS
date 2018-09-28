using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Modules.EduPath.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
namespace lm.Comol.Modules.EduPath.Presentation
{
    public class ModuleTextActionPresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private Service _Service;

            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewModuleTextAction View
            {
                get { return (IViewModuleTextAction)base.View; }
            }
            private Service Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new Service(AppContext);
                    return _Service;
                }
            }
            public ModuleTextActionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleTextActionPresenter(iApplicationContext oContext, IViewModuleTextAction view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(dtoInternalActionInitializer dto)
            {
                View.InsideOtherModule = true;
                InitializeControlByInternalItem(dto, (Display(dto.Display, DisplayActionMode.defaultAction) ? StandardActionType.Play : StandardActionType.None));
            }
            public void InitView(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.InsideOtherModule = true;
                InitializeControlByInternalItem(dto, actionsToDisplay);
            }
            public List<dtoModuleActionControl> InitRemoteControlView(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                View.InsideOtherModule = true;
                return InitializeControlByInternalItem(dto, actionsToDisplay);
            }
            private List<dtoModuleActionControl> InitializeControlByInternalItem(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (dto.SubActivity == null || UserContext.isAnonymous || dto.SubActivity.Id == 0)
                    View.DisplayEmptyAction();
                else{
                    View.ItemIdentifier = "subactivity_" + dto.SubActivity.Id.ToString();
                    actions = AnalyzeActions(dto, actionsToDisplay);
                }
                return actions;
            }

            private List<dtoModuleActionControl> AnalyzeActions(dtoInternalActionInitializer dto, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                View.ContainerCSS = dto.ContainerCSS;
                View.IconSize = dto.IconSize;

                if (dto.SubActivity == null || dto.SubActivity.Id == 0)
                    View.DisplayEmptyAction();
                else
                {
                    View.Display = dto.Display;
                    //if (Service.ex)
                    //    View.DisplayRemovedObject();
                    //else{
                        if (dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).Any() && (Display(dto.Display, DisplayActionMode.defaultAction) || Display(dto.Display, DisplayActionMode.text)))
                            View.DisplayPlaceHolders(dto.PlaceHolders.Where(p => !String.IsNullOrEmpty(p.Text)).ToList());
                        actions = AnalyzeItem(dto.SubActivity, dto.Display, actionsToDisplay);
                  //  }
                }
                return actions;
            }

            private List<dtoModuleActionControl> AnalyzeItem(dtoSubActivity item, DisplayActionMode display, StandardActionType actionsToDisplay)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();
                if (item != null) {
                    if (Display(display, DisplayActionMode.text))
                        DisplayTextInfo(item);
                    else if (Display(display, DisplayActionMode.defaultAction))
                        DisplayDefaultAction(item);
                    actions = GenerateActions(item);
                    if (Display(display, DisplayActionMode.actions) && actionsToDisplay != StandardActionType.None)
                        View.DisplayActions(actions.Where(a => ((int)a.ControlType & (int)actionsToDisplay) > 0).ToList());
                }
                return actions;
            }

            private void DisplayTextInfo(dtoSubActivity item) {
                View.DisplayItem(item.Description);
            }
            private void DisplayDefaultAction(dtoSubActivity item)
            {
                View.DisplayItem(item.Id, item.Description);
            }
            private List<dtoModuleActionControl> GenerateActions(dtoSubActivity item)
            {
                List<dtoModuleActionControl> actions = new List<dtoModuleActionControl>();

                // MANCANO LE AZIONI PER VEDERE LE STATISTICHE UTENTE E LE VALUTAZIONI;
                // DA AGGIUNGERE
                
                return actions;
            }
            
            private Boolean Display(DisplayActionMode current, DisplayActionMode required)
            {
                return ((long)current & (long)required) > 0;
            }


            public Boolean ExecuteAction(long idSubActivity) { 
                // SAREBBE DA INSERIRE UN GET COMMUNITY BY idSubactivity..

                Int32 idRole = CurrentManager.GetSubscriptionIdRole(UserContext.CurrentUserID,UserContext.CurrentCommunityID);
                return Service.ExecuteSubActivityInternal(idSubActivity, UserContext.CurrentUserID,idRole, UserContext.ProxyIpAddress,UserContext.IpAddress);           
            }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.BusinessLogic;

namespace lm.Comol.Modules.EduPath.Presentation

{
    public class SummaryUserPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private int _IdModule;
            private Service _Service;
            private int IdModule
            {
                get
                {
                    if (_IdModule <= 0)
                    {
                        _IdModule = this.PathService.ServiceModuleID();
                    }
                    return _IdModule;
                }
            }
            
            private Service PathService
            {
                get
                {
                    if (_Service == null)
                        _Service = new Service(AppContext);
                    return _Service;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSummaryUser View
            {
                get { return (IViewSummaryUser)base.View; }
            }
            public SummaryUserPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SummaryUserPresenter(iApplicationContext oContext, IViewSummaryUser view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Int32 idUser = View.PreloadIdUser;
                Int32 idCommunity = View.SummaryIdCommunity;
                SummaryType type = View.PreloadFromSummary;

                if (type != SummaryType.PortalIndex && idCommunity <= 0)
                    type = SummaryType.PortalIndex;
                else if (type != SummaryType.PortalIndex){
                    type= SummaryType.CommunityIndex;
                    if (idCommunity == 0)
                        idCommunity = UserContext.CurrentCommunityID;
                }
                View.FromSummary = type;
                View.SummaryIdCommunity = idCommunity;
                View.SummaryIdUser = idUser;
                if (idUser == 0){
                    View.DisplayNoUserSelected(RootObject.SummaryIndex(type, idCommunity));
                    View.SendUserAction(idCommunity, IdModule,  ModuleEduPath.ActionType.UnselectedUserForSummaryUser);
                }
                else {
                    // VERIFICA PERMESSI
                    litePerson currentPerson = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                    litePerson person = CurrentManager.GetLitePerson(idUser);
                    ModuleEduPath mEduPath = PathService.ServiceStat.GetPermissionForSummaryUser(type, idCommunity, UserContext.CurrentUserID, idUser);
                    if (mEduPath.Administration || mEduPath.ViewMyStatistics)
                    {
                        View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.ViewSummaryUser);
                        View.AllowOrganizationSelection = (currentPerson != null && (currentPerson.TypeID == (int)UserTypeStandard.Administrator || currentPerson.TypeID == (int)UserTypeStandard.SysAdmin));
                        View.LoadAvailableOrganizations(UserContext.CurrentUserID);
                        View.LoadData(idUser, person);
                    }
                    else
                    {
                        View.DisplayWrongPageAccess(RootObject.SummaryIndex(type, idCommunity));
                        View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.WrongPageAccess);
                    }
                }
            }
        }
        public void LoadData(){
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else {
                Int32 idUser = View.SummaryIdUser;
                litePerson person = CurrentManager.GetLitePerson(idUser);
                View.LoadData(View.SummaryIdUser, person);
                View.SendUserAction(View.SummaryIdCommunity, IdModule, ModuleEduPath.ActionType.ViewSummaryUser);
            }
        }
        public ModuleEduPath GetModulePermission(Int32 idCommunity) { 
            return new ModuleEduPath(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, IdModule));
        }
    //     Public Overrides Function HasPermessi() As Boolean
    //    Dim uts As UserTypeStandard = CurrentContext.UserContext.UserTypeID

    //    Select Case uts

    //        Case UserTypeStandard.SysAdmin
    //            Return True
    //        Case UserTypeStandard.Administrator
    //            Return True
    //        Case UserTypeStandard.Administrative
    //            DIVfilterorganization.Visible = False
    //            Return True
    //        Case Else
    //            DIVfilterorganization.Visible = False
    //            Return UserId = CurrentContext.UserContext.CurrentUserID
    //    End Select

    //End Function
    }
}
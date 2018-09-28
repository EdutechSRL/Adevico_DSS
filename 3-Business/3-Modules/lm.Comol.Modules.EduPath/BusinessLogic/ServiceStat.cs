using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using System.Diagnostics;
using lm.Comol.Modules.Base.BusinessLogic;
using NHibernate;
using System.Linq.Expressions;
using lm.Comol.Modules.EduPath.Domain.DTO;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public partial class ServiceStat
    {
        private const Int32 maxItemsInContainsQuery = 900;
        private ManagerEP manager { get; set; }
        private Service serviceEP { get; set; }
        private DateTime timeToDelete { get { return DateTime.Now; } }

        public ServiceStat()
        {
        }


        public ServiceStat(ManagerEP Manager, Service ServiceEP)
        {
            this.manager = Manager;
            this.serviceEP = ServiceEP;
        }
        
        #region "Summary Access"
            public List<SummaryType> GetAvailableSummaryViews(SummaryType type, Int32 idCommunity, Int32 idPerson)
            {
                List<SummaryType> items = new List<SummaryType>();
                litePerson person = manager.GetLitePerson(idPerson);
                switch (type)
                {
                    case SummaryType.PortalIndex:
                        if (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrative || person.TypeID == (int)UserTypeStandard.Administrator))
                        {
                            items.Add(SummaryType.Organization);
                            items.Add(SummaryType.Community);
                            items.Add(SummaryType.Path);
                            items.Add(SummaryType.User);
                        }
                        break;
                    case SummaryType.CommunityIndex:
                        lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mPermission = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(idPerson, idCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));

                        // QUI DOVREI METTERE ANCHE IL PERMESSO RELATIVO AL SERVIZIO PERCORSO FORMATIVO .. MA ....
                        //lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mPermission = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(idPerson, idCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                        if (mPermission.Manage || mPermission.Manage)
                        {
                            if ((from c in manager.GetIQ<lm.Comol.Core.Communities._OldCommunityRelation>() where c.IdSource == idCommunity select c.Id).Any())
                                items.Add(SummaryType.Community);
                            items.Add(SummaryType.Path);
                            items.Add(SummaryType.User);
                        }
                        break;
                }
                return items;
            }
            public ModuleEduPath GetPermissionForSummaryUser(SummaryType fromType, Int32 idCommunity, Int32 idPerson, Int32 forIduser)
            {
                ModuleEduPath module = new ModuleEduPath();
                litePerson person = manager.GetLitePerson(idPerson);
                module.ViewMyStatistics = (forIduser == idPerson && person != null);
                module.ManagePermission = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));

                if (!module.Administration) {
                    switch (fromType)
                    {
                        case SummaryType.PortalIndex:
                            module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.Administrative));
                            break;
                        case SummaryType.CommunityIndex:
                            lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mCommunityManagement = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(idPerson, idCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                            ModuleEduPath mEduPath = new ModuleEduPath(manager.GetModulePermission(idPerson, idCommunity, manager.GetModuleID(ModuleEduPath.UniqueCode)));

                            module.Administration = mCommunityManagement.Administration || mEduPath.Administration;

                            if (!module.Administration)
                            {
                                Int32 idFatherCommunity = manager.GetIdCommunityFromOrganization(manager.GetIdOrganizationFromCommunity(idCommunity));
                                mCommunityManagement = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(idPerson, idFatherCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                                module.Administration = mCommunityManagement.Administration;
                            }
                            break;


                    }
                }
                //switch(type){
                //    case SummaryType.PortalIndex:
                //        module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrative || person.TypeID == (int)UserTypeStandard.Administrator));
                //        module.ManagePermission = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));    
                //        module.ViewMyStatistics= 
                //        break;
                //    case SummaryType.CommunityIndex:
                //        
                       
                //            // QUI DOVREI METTERE ANCHE IL PERMESSO RELATIVO AL SERVIZIO PERCORSO FORMATIVO .. MA ....
                //        //lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mPermission = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(idPerson, idCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                //        if (mPermission.Manage || mPermission.Manage) {
                //            if ((from c in manager.GetIQ<lm.Comol.Core.Communities._OldCommunityRelation>() where c.IdSource== idCommunity select c.Id).Any())
                //                items.Add(SummaryType.Community);
                //            items.Add(SummaryType.Path);
                //            items.Add(SummaryType.User);
                //        }
                //        break;
                //}
                return module;
            }

            public ModuleEduPath GetPermissionForCertification(Int32 idCommunity, litePerson person, Int64 idPath)
            {
                ModuleEduPath module = new ModuleEduPath();
                module.ManagePermission = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                module.ViewMyStatistics = (person != null);
                if (!module.Administration) {
                    ModuleEduPath cPermissions = new ModuleEduPath(manager.GetModulePermission(person.Id, idCommunity, manager.GetModuleID(ModuleEduPath.UniqueCode)));
                    module.Administration = cPermissions.Administration;
                    module.ManagePermission = module.ManagePermission || cPermissions.ManagePermission;
                    module.ViewMyStatistics = module.ViewMyStatistics || cPermissions.ViewMyStatistics;
                    module.ViewPathList = module.ViewPathList || cPermissions.ViewPathList;
                    if (!module.Administration) {
                        RoleEP role= serviceEP.GetUserRole_ByPath(idPath, person.Id, manager.GetSubscriptionIdRole(person.Id, idCommunity));
                        module.Administration = (role == RoleEP.Manager);
                        module.ViewMyStatistics = module.ViewMyStatistics || (role == RoleEP.Manager) || (role == RoleEP.StatViewer);
                        module.ViewPathList = (role != RoleEP.None);
                        if (!module.Administration)
                        {
                            Int32 idFatherCommunity= manager.GetIdCommunityFromOrganization(manager.GetIdOrganizationFromCommunity(idCommunity));
                            lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mCommunityManagement = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(person.Id, idFatherCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                            module.Administration = mCommunityManagement.Administration;
                        }
                    }
                }
                return module;
            }

            public ModuleEduPath GetPermissionForSummary(SummaryType type, Int32 idCommunity, litePerson person)
            {
                ModuleEduPath module = new ModuleEduPath();
                module.ManagePermission = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                module.ViewMyStatistics = (person != null);

                if (!module.Administration && person !=null)
                {
                    switch (type)
                    {
                        case SummaryType.PortalIndex:
                            module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.Administrative));
                            break;
                        case SummaryType.CommunityIndex:
                        case SummaryType.Community:
                        case SummaryType.Organization:
                        case SummaryType.Path:
                            lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mCommunityManagement = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(person.Id , idCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                            ModuleEduPath mEduPath = new ModuleEduPath(manager.GetModulePermission(person.Id, idCommunity, manager.GetModuleID(ModuleEduPath.UniqueCode)));

                            module.Administration = mCommunityManagement.Administration || mEduPath.Administration;
                            break;


                    }
                }
                //switch(type){
                //    case SummaryType.PortalIndex:
                //        module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrative || person.TypeID == (int)UserTypeStandard.Administrator));
                //        module.ManagePermission = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));    
                //        module.ViewMyStatistics= 
                //        break;
                //    case SummaryType.CommunityIndex:
                //        

                //            // QUI DOVREI METTERE ANCHE IL PERMESSO RELATIVO AL SERVIZIO PERCORSO FORMATIVO .. MA ....
                //        //lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mPermission = new Core.DomainModel.Domain.ModuleCommunityManagement(manager.GetModulePermission(idPerson, idCommunity, manager.GetModuleID(Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID)));
                //        if (mPermission.Manage || mPermission.Manage) {
                //            if ((from c in manager.GetIQ<lm.Comol.Core.Communities._OldCommunityRelation>() where c.IdSource== idCommunity select c.Id).Any())
                //                items.Add(SummaryType.Community);
                //            items.Add(SummaryType.Path);
                //            items.Add(SummaryType.User);
                //        }
                //        break;
                //}
                return module;
            }
        #endregion


        #region completion Status

        private ModifyStatField UpdateCompletedStatus_byActStat_Insert(ActivityStatistic oActStat,IList<SubActivityStatistic> subActStats, Status participantStatus,ref ModifyStatField subActFieldChanged)
        {          
            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedMandatoryAdd))
            {
                oActStat.MandatoryCompletedSubActivityCount = (Int16)(from stat in subActStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && serviceEP.CheckStatus(stat.SubActivity.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryAdd | ModifyStatField.ComplPassMandatoryAdd))
                {
                    oActStat.MandatoryPassedCompletedSubActivityCount = (Int16)(from stat in subActStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.SubActivity.Status, Status.Mandatory) select stat).Count();
                    subActFieldChanged -= (ModifyStatField.ComplPassMandatoryAdd | ModifyStatField.ComplPassMandatoryAdd);
                }
            }
            else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedMandatoryRem))
            {
                oActStat.MandatoryCompletedSubActivityCount = (Int16)(from stat in subActStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && serviceEP.CheckStatus(stat.SubActivity.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                {
                    oActStat.MandatoryPassedCompletedSubActivityCount = (Int16)(from stat in subActStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.SubActivity.Status, Status.Mandatory) select stat).Count();
                    subActFieldChanged -= ModifyStatField.ComplPassMandatoryRem;
                    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                    { subActFieldChanged -= ModifyStatField.ComplPassMandatoryRem; }

                }
            }

            ModifyStatField actFieldChanged = ModifyStatField.None;

            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
            {             

                if (oActStat.Completion >= oActStat.Activity.MinCompletion)
                {
                    Int16 subActMandCount = serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0);
                    if (subActMandCount == oActStat.MandatoryCompletedSubActivityCount)
                    {
                        oActStat.Status |= (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started);
                        actFieldChanged |= ModifyStatField.CompletedAdd;

                        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                        {
                            actFieldChanged |= ModifyStatField.CompletedMandatoryAdd;
                            if(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed))
                            {
                                actFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                            }                         
                            
                        }
                        actFieldChanged |= ModifyStatField.CompletedAdd;
                    }
                }
            }
            else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedRem))
            {
         
                if (CheckStatusStatistic(oActStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
                {
                    
                    if (oActStat.Completion <= oActStat.Activity.MinCompletion)
                    {
                        actFieldChanged |= SubRemoveMandatoryActCount(oActStat, participantStatus);                  

                    }
                    else if (serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0) != oActStat.MandatoryCompletedSubActivityCount)
                    {

                        actFieldChanged |= SubRemoveMandatoryActCount(oActStat, participantStatus);
                    }

                }
            }
            return actFieldChanged;
        }

        private ModifyStatField SubRemoveMandatoryActCount(ActivityStatistic oActStat, Status participantStatus)
        {
            bool remMandatoryPassedCompletedActivityCount = CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed);
            ModifyStatField actFieldChanged = ModifyStatField.None;
           
            oActStat.Status -= StatusStatistic.Completed;
            actFieldChanged |= ModifyStatField.CompletedRem;
            if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
            {
                actFieldChanged |= ModifyStatField.CompletedMandatoryRem;

                if (remMandatoryPassedCompletedActivityCount)
                {
                    actFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                }
            }
            return actFieldChanged;
        }

        private ModifyStatField UpdateCompletedStatus_byUnitStat_Insert(UnitStatistic oUnitStat, IList<ActivityStatistic> actStats,Status participantStatus,ref ModifyStatField actFieldChanged)
        {

            if (CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryAdd | ModifyStatField.ComplPassMandatoryAdd))
            {
                oUnitStat.MandatoryCompletedActivityCount = (Int16)(from stat in actStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && serviceEP.CheckStatus(stat.Activity.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryAdd))
                {
                    oUnitStat.MandatoryPassedCompletedActivityCount = (Int16)(from stat in actStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.Activity.Status, Status.Mandatory) select stat).Count();
                    actFieldChanged -= (ModifyStatField.ComplPassMandatoryAdd | ModifyStatField.ComplPassMandatoryAdd);
                }
            }
            else if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedMandatoryRem))
            {
                oUnitStat.MandatoryCompletedActivityCount = (Int16)(from stat in actStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && serviceEP.CheckStatus(stat.Activity.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                {
                    oUnitStat.MandatoryPassedCompletedActivityCount = (Int16)(from stat in actStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.Activity.Status, Status.Mandatory) select stat).Count();
                    actFieldChanged -= ModifyStatField.ComplPassMandatoryRem;
                    if (CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                    { actFieldChanged -= ModifyStatField.ComplPassMandatoryRem; }
                }
            }

            ModifyStatField unitFieldChanged = ModifyStatField.None;

            if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
            {


                if (oUnitStat.Completion >= oUnitStat.Unit.MinCompletion)
                {
                    Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0);
                    if (actMandCount == oUnitStat.MandatoryCompletedActivityCount)
                    {
                        oUnitStat.Status |= (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started);
                        unitFieldChanged |= ModifyStatField.CompletedAdd;

                        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                        {
                            unitFieldChanged |= ModifyStatField.CompletedMandatoryAdd;
                            if (CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed))
                            {
                                unitFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                            } 
                        }
                        unitFieldChanged |= ModifyStatField.CompletedAdd;
                    }

                }
            }
            else if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedRem))
            {
                if (CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
                {
                    if (oUnitStat.Completion <= oUnitStat.Unit.MinCompletion)
                    {
                        unitFieldChanged |= SubRemoveMandatoryUnitCount(oUnitStat, participantStatus); 
                    }
                    else if (serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0) != oUnitStat.MandatoryCompletedActivityCount)
                    {
                        unitFieldChanged |= SubRemoveMandatoryUnitCount(oUnitStat, participantStatus); 
                    }
                }
            }
            return unitFieldChanged;
        }


        private ModifyStatField SubRemoveMandatoryUnitCount(UnitStatistic oUnitStat, Status participantStatus)
        {
            bool remMandatoryPassedCompletedActivityCount = CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed);
            oUnitStat.Status -= StatusStatistic.Completed;
            ModifyStatField unitFieldChanged = ModifyStatField.CompletedRem;

            if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
            {
                unitFieldChanged |= ModifyStatField.CompletedMandatoryRem;
                if (remMandatoryPassedCompletedActivityCount)
                {
                    unitFieldChanged |= ModifyStatField.ComplPassMandatoryRem;                    
                }
            }
            return unitFieldChanged;
        }

        private void UpdateCompletedStatus_byPathStat_Insert(PathStatistic oPathStat, IList<UnitStatistic> unitStats,Status participantStatus,ref ModifyStatField unitFieldChanged)
        {
            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.ComplPassMandatoryAdd | ModifyStatField.ComplPassMandatoryAdd))
            {
                oPathStat.MandatoryCompletedUnitCount = (Int16)(from stat in unitStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && serviceEP.CheckStatus(stat.Unit.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(unitFieldChanged, ModifyStatField.ComplPassMandatoryAdd))
                {
                    oPathStat.MandatoryPassedCompletedUnitCount = (Int16)(from stat in unitStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.Unit.Status, Status.Mandatory) select stat).Count();
                    unitFieldChanged -= ModifyStatField.ComplPassMandatoryAdd | ModifyStatField.ComplPassMandatoryAdd;
                }
            }
            else if (CheckFieldChanged(unitFieldChanged, ModifyStatField.CompletedMandatoryRem))
            {
                oPathStat.MandatoryCompletedUnitCount = (Int16)(from stat in unitStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && serviceEP.CheckStatus(stat.Unit.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(unitFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                {
                    oPathStat.MandatoryPassedCompletedUnitCount = (Int16)(from stat in unitStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.Unit.Status, Status.Mandatory) select stat).Count();
                    unitFieldChanged -= ModifyStatField.ComplPassMandatoryRem;
                    if (CheckFieldChanged(unitFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                    { unitFieldChanged -= ModifyStatField.ComplPassMandatoryRem; }
                }
            }

            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.CompletedAdd))
            {
                if (!CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
                {
                    if (oPathStat.Completion >= oPathStat.Path.MinCompletion)
                    {
                        Int16 unitMandCount = serviceEP.GetActiveMandatoryUnitCount(oPathStat.Path.Id, 0, 0);
                        if (unitMandCount == oPathStat.MandatoryCompletedUnitCount)
                        {
                            oPathStat.Status |= (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started);
                        }
                    }
                }
            }
            else if (CheckFieldChanged(unitFieldChanged, ModifyStatField.CompletedRem))
            {
                if (CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
                {

                    if (oPathStat.Completion <= oPathStat.Path.MinCompletion)
                    {
                        oPathStat.Status -= StatusStatistic.Completed;
                    }
                    else if (serviceEP.GetActiveMandatoryUnitCount(oPathStat.Path.Id, 0, 0) != oPathStat.MandatoryCompletedUnitCount)
                    {
                        oPathStat.Status -= StatusStatistic.Completed;
                    }
                }
            }
        }


        #endregion


        //public DateTime? GetLastCertifiedDate(long pathId, int userId)
        //{
        //    Path oPath = manager.Get<Path>(pathId);

        //    if (oPath.EndDate != null)
        //    {
        //        IList<DateTime> lastCertifiedDate = (from stat in manager.GetIQ<SubActivityStatistic>()
        //                                             where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn > oPath.EndDate && stat.CreatedOn <= oPath.EndDateOverflow
        //                                             orderby stat.CreatedOn ascending
        //                                             select (DateTime)stat.CreatedOn).Take(1).ToList();
        //        if (lastCertifiedDate.Count == 1)
        //        {
        //            return lastCertifiedDate[0];
        //        }
        //    }
        //    return oPath.EndDate;
        //}

        public StatusStatistic GetStatusStat_byPathStat(long pathId, int userId, DateTime viewStatBefore)
        {
            IList<StatusStatistic> status = (from stat in manager.GetIQ<PathStatistic>()
                                             where stat.Path.Id == pathId && stat.Person.Id == userId && stat.CreatedOn!=null && (DateTime)stat.CreatedOn<=viewStatBefore
                                             orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending 
                                             select stat.Status).Take(1).ToList();
            if (status.Count ==1)
            {
                return GetMainStatusStatistic(status[0]);
            }
            else
            {
                return StatusStatistic.Browsed;
            }
        }

        
        public StatusStatistic GetStatusStat_byUnitStat(long unitId, int userId, DateTime viewStatBefore)
        {
            IList<StatusStatistic> status = (from stat in manager.GetIQ<UnitStatistic>()
                                             where stat.Unit.Id == unitId && stat.Person.Id == userId && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
                                             orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending 
                                             select stat.Status).Take(1).ToList();
            if (status.Count == 1)
            {
                return GetMainStatusStatistic(status[0]);
            }
            else
            {
                return StatusStatistic.Browsed;
            }
        }

        public StatusStatistic GetStatusStat_byActivityStat(long actId, int userId, DateTime viewStatBefore)
        {
            IList<StatusStatistic> status = (from stat in manager.GetIQ<ActivityStatistic>()
                                             where stat.Activity.Id == actId && stat.Person.Id == userId && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
                                             orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending 
                                             select stat.Status).Take(1).ToList();
            if (status.Count == 1)
            {
                return GetMainStatusStatistic(status[0]);
            }
            else
            {
                return StatusStatistic.Browsed;
            }
        }
        #region completion weighted

        private bool UpdateActStatCompletionManual(ActivityStatistic oActStat, IList<SubActivityStatistic> ActiveSubActStats)
        {
           
            Int64 totWeight = (Int64)(from sub in manager.GetAll<SubActivity>(item => item.ParentActivity.Id == oActStat.Activity.Id && item.Deleted == BaseStatusDeleted.None)
                                      where !sub.CheckStatus(Status.Draft)
                                      select sub.Weight).Sum(s => s);           
            Int64 totCompletion;

            if (totWeight == 0)
            {
                totWeight = (Int64)(from sub in manager.GetAll<SubActivity>(item => item.ParentActivity.Id == oActStat.Activity.Id && item.Deleted == BaseStatusDeleted.None)
                                    where !sub.CheckStatus(Status.Draft)
                                    select sub.Weight).Count();
                totCompletion = (Int64)(from stat in ActiveSubActStats
                                        select stat.Completion).Sum(b => b);
            }
            else
            {
                totCompletion = (Int64)(from stat in ActiveSubActStats
                                        select stat).Sum(b => (b.Completion * b.SubActivity.Weight));
            }

            Int64 newCompletion = (Int64)(totCompletion / Math.Max(1, totWeight));
            if (newCompletion != oActStat.Completion)
            {
                oActStat.Completion = newCompletion;
                return true;
            }

            return false;
        }
         
        private bool UpdateUnitStatCompletionManual(UnitStatistic oUnitStat, IList<ActivityStatistic> ActiveActStats)
        {

            Int64 totWeight = (Int64)(from act in manager.GetAll<Activity>(item => item.ParentUnit.Id == oUnitStat.Unit.Id && item.Deleted == BaseStatusDeleted.None)
                                      where !act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
                                      select act.Weight).Sum(s => s);

            Int64 totCompletion;
            if (totWeight == 0)
            {
                totWeight = (Int64)(from act in manager.GetAll<Activity>(item => item.ParentUnit.Id == oUnitStat.Unit.Id && item.Deleted == BaseStatusDeleted.None)
                                    where !act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
                                    select act.Weight).Count();
                totCompletion = (Int64)(from stat in ActiveActStats
                                        select new
                                        {
                                            subTot = stat.Completion < stat.Activity.MinCompletion ? (stat.Completion ) : (100 )
                                        }).Sum(b => b.subTot);
            }
            else
            {
                totCompletion = (Int64)(from stat in ActiveActStats
                                        select new
                                        {
                                            subTot = stat.Completion < stat.Activity.MinCompletion ? (Int64)(stat.Completion * stat.Activity.Weight) : (Int64)(100 * stat.Activity.Weight)
                                        }).Sum(b => b.subTot);
            }

            Int64 newCompletion = (Int64)(totCompletion / Math.Max( totWeight,1));
            if (newCompletion != oUnitStat.Completion)
            {
                oUnitStat.Completion = newCompletion;
                return true;
            }
            return false;
        }
            
        private void UpdatePathStatCompletionManual(PathStatistic oPathStat, IList<UnitStatistic> ActiveUnitStats)
        {

            Int64 totWeight = (Int64)(from unit in manager.GetAll<Unit>(item => item.ParentPath.Id == oPathStat.Path.Id && item.Deleted == BaseStatusDeleted.None)
                                      where !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                                      select unit.Weight).Sum(s => s);

            Int64 totCompletion;
            if (totWeight == 0)
            {
                totWeight = (Int64)(from unit in manager.GetAll<Unit>(item => item.ParentPath.Id == oPathStat.Path.Id && item.Deleted == BaseStatusDeleted.None)
                                    where !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                                    select unit.Weight).Count();
                totCompletion = (Int64)(from stat in ActiveUnitStats
                                        select new
                                        {
                                            subTot = stat.Completion < stat.Unit.MinCompletion ? (Int64)(stat.Completion ) : (Int64)(100 )
                                        }).Sum(b => (b.subTot));
            }
            else
            {
                totCompletion = (Int64)(from stat in ActiveUnitStats
                                        select new
                                        {
                                            subTot = stat.Completion < stat.Unit.MinCompletion ? (Int64)(stat.Completion * stat.Unit.Weight) : (Int64)(100 * stat.Unit.Weight)
                                        }).Sum(b => (b.subTot));
            }

            Int64 newCompletion = (Int64)(totCompletion / Math.Max(1, totWeight));
            if (newCompletion != oPathStat.Completion)
            {
                oPathStat.Completion = newCompletion;                
            }
        }
        
        #endregion

        #region mark weighted

        private void UpdatePathStatMarkManual(PathStatistic oPathStat, IList<UnitStatistic> ActiveUnitStats)
        {
            Int64 totWeight = (Int64)(from item in manager.GetAll<Unit>(item => item.ParentPath.Id == oPathStat.Path.Id && item.Deleted == BaseStatusDeleted.None)
                                      where !item.CheckStatus(Status.Draft)
                                      select item.Weight).Sum(s => s);


            Int16 markSum,  newMark=0;
            Int64 partialWeight;

            if (totWeight == 0) //newMark= media non pesata delle statistiche presenti
            {
                markSum = (Int16)(from stat in ActiveUnitStats
                                  where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                  select stat.Mark).Sum(b => b);
                partialWeight = (Int64)(from stat in ActiveUnitStats
                                        where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                        select stat.Unit.Weight).Count();
                newMark = (Int16)(markSum / Math.Max(1,partialWeight));
            }
            else
            {
                partialWeight = (Int64)(from stat in ActiveUnitStats
                                        where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                        select stat.Unit.Weight).Sum(s => s);

                if (partialWeight > 0) //newMark= media pesata delle statistiche presenti
                {
                    markSum = (Int16)(from stat in ActiveUnitStats
                                      where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                      select stat).Sum(b => (b.Mark * b.Unit.Weight));
                    newMark = (Int16)(markSum / Math.Max(1,partialWeight));
                }
            } 
            if (newMark != oPathStat.Mark)
            {
                oPathStat.Mark = newMark;

            }
        }

        private ModifyStatField UpdateUnitStatMarkManual(UnitStatistic oUnitStat, IList<ActivityStatistic> ActiveActStats)
        {

            Int64 totWeight = (Int64)(from act in manager.GetAll<Activity>(item=> item.ParentUnit.Id == oUnitStat.Unit.Id && item.Deleted == BaseStatusDeleted.None )

                                      select act.Weight).Sum(s => s);
           
            Int16 markSum,  newMark = 0;

            Int64 partialWeight;

            if (totWeight == 0) //newMark= media non pesata delle statistiche presenti
            {
                markSum = (Int16)(from stat in ActiveActStats
                                  where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                  select stat.Mark).Sum(b => b);
                partialWeight = (Int64)(from stat in ActiveActStats
                                        where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                        select stat.Activity.Weight).Count();
                newMark = (Int16)(markSum / Math.Max(1,partialWeight));
            }
            else
            {
                partialWeight = (Int64)(from stat in ActiveActStats
                                        where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                        select stat.Activity.Weight).Sum(s => s);

                if (partialWeight > 0) //newMark= media pesata delle statistiche presenti
                {
                    markSum = (Int16)(from stat in ActiveActStats
                                      where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                      select stat).Sum(b => (b.Mark * b.Activity.Weight));
                    newMark = (Int16)(markSum / Math.Max(1,partialWeight));
                }
            }         

            if (newMark != oUnitStat.Mark)
            {
                oUnitStat.Mark = newMark;
                return ModifyStatField.Mark;
            }
            return ModifyStatField.None;
        }

        private ModifyStatField UpdateActStatMarkManual(ActivityStatistic oActStat, IList<SubActivityStatistic> ActiveSubActStats)
        {
            Int64 totWeight = (Int64)(short)(from item in manager.GetIQ<SubActivity>()
                                      where item.ParentActivity !=null && item.ParentActivity.Id == oActStat.Activity.Id && item.Deleted == BaseStatusDeleted.None
                                      select item.Weight).ToList().Sum(s=> s);

            
            Int16 markSum,  newMark=0;
            Int64 partialWeight;

            if (totWeight == 0) //newMark= media non pesata delle statistiche presenti
            {
                markSum = (Int16)(from stat in ActiveSubActStats
                                  where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                  select stat.Mark).Sum(b => b);
                 partialWeight = (Int64)(from stat in ActiveSubActStats
                                              where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                         select stat.SubActivity.Weight).Count();
                newMark = (Int16)(markSum / Math.Max(1,partialWeight));
            }
            else 
            {
                partialWeight = (Int64)(from stat in ActiveSubActStats
                                              where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                              select stat.SubActivity.Weight).Sum(s=>s);
                
                if(partialWeight>0) //newMark= media pesata delle statistiche presenti
                {
                markSum = (Int16)(from stat in ActiveSubActStats
                                  where CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                  select stat).Sum(b => (b.Mark * b.SubActivity.Weight));
                    newMark = (Int16)(markSum / Math.Max(1,partialWeight));
                }               
            }         

            
            if (newMark != oActStat.Mark)
            {
                oActStat.Mark = newMark;
                return ModifyStatField.Mark;
            }
            return ModifyStatField.None;
        }

        #endregion

        #region update status passed and mark

        //private void UpdateActStatMarkWeightedAndStatusPassed(long actStatId, int userId, int croleId, bool isMarkUpdated, StatusOperation StatPassedOperation, int evaluatorId, string evaluatorProxyIPaddress, string evaluatorIPaddress)
        //{
        //    ActivityStatistic oActStat = manager.Get<ActivityStatistic>(actStatId);
        //    IList<SubActivityStatistic> ActiveSubActStats = GetActiveSubActivityStatistic(oActStat.ChildrenStats, userId, croleId);

        //    if (isMarkUpdated)
        //    {
        //        isMarkUpdated = UpdateActStatMarkWeighted(oActStat, ActiveSubActStats, userId, croleId);
        //    }

        //    if (StatPassedOperation != StatusOperation.None)
        //    {
        //        StatPassedOperation = UpdatePassedStatus_byActStat(oActStat, ActiveSubActStats, StatPassedOperation, userId, croleId);
        //    }

        //    if (isMarkUpdated || StatPassedOperation != StatusOperation.None)
        //    {
        //        SetStatActivityModifyMetaInfo(oActStat, manager.Get<litePerson>(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);
        //        UpdateUnitStatMarkWeightedAndStatusPassed(oActStat.ParentStat.Id, userId, croleId, isMarkUpdated, StatPassedOperation, evaluatorId, evaluatorProxyIPaddress, evaluatorIPaddress);
        //    }
        //}

        //private void UpdateUnitStatMarkWeightedAndStatusPassed(long unitStatId, int userId, int croleId, bool isMarkUpdated, StatusOperation StatPassedOperation, int evaluatorId, string evaluatorProxyIPaddress, string evaluatorIPaddress)
        //{
        //    UnitStatistic oUnitStat = manager.Get<UnitStatistic>(unitStatId);
        //    IList<ActivityStatistic> ActiveActStats = GetActiveActivityStatistic(unitStatId);

        //    if (isMarkUpdated)
        //    {
        //        isMarkUpdated = UpdateUnitStatMarkWeighted(oUnitStat, ActiveActStats, userId, croleId);
        //    }

        //    if (StatPassedOperation != StatusOperation.None)
        //    {
        //        StatPassedOperation = UpdatePassedStatus_byUnitStat(oUnitStat, ActiveActStats, StatPassedOperation, userId, croleId);
        //    }

        //    if (isMarkUpdated || StatPassedOperation != StatusOperation.None)
        //    {
        //        SetStatUnitModifyMetaInfo(oUnitStat, manager.Get<litePerson>(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);

        //        UpdatePathStatMarkWeightedAndStatusPassed(oUnitStat.ParentStat.Id, userId, croleId, isMarkUpdated, StatPassedOperation, evaluatorId, evaluatorProxyIPaddress, evaluatorIPaddress);
        //    }
        //}

        //private void UpdatePathStatMarkWeightedAndStatusPassed(long pathStatId, int userId, int croleId, bool isMarkUpdated, StatusOperation StatPassedOperation, int evaluatorId, string evaluatorProxyIPaddress, string evaluatorIPaddress)
        //{
        //    PathStatistic oPathStat = manager.Get<PathStatistic>(pathStatId);
        //    IList<UnitStatistic> ActiveUnitStats = GetActiveUnitStatistic(oPathStat.ChildrenStats, userId, croleId);

        //    if (isMarkUpdated)
        //    {
        //        UpdatePathStatMarkWeighted(oPathStat, ActiveUnitStats, userId, croleId);
        //    }

        //    if (StatPassedOperation != StatusOperation.None)
        //    {
        //        UpdatePassedStatus_byPathStat(oPathStat, ActiveUnitStats, StatPassedOperation, userId, croleId);
        //    }

        //    if (isMarkUpdated || StatPassedOperation != StatusOperation.None)
        //    {
        //        SetStatPathModifyMetaInfo(oPathStat, manager.Get<litePerson>(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);
        //    }
        //}


        #endregion


        private StatusStatistic RemoveStatus(StatusStatistic CurrentStatus, StatusStatistic StatusToRemove)
        {
            if (CheckStatusStatistic(CurrentStatus, StatusToRemove))
            {
                CurrentStatus = CurrentStatus - (int)StatusToRemove;
            }
            return CurrentStatus;
        }

        #region MandatoryCount

        /// <summary>
        /// conta il numero di subactivityStatistic mandatory che contengono un determinato statusStatistic
        /// </summary>
        /// <param name="ActiveStatisticSubActivityList"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <param name="statusToCheck"></param>
        /// <returns></returns>
        private Int16 GetSubActStatMandatoryCount_byStatus(IList<SubActivityStatistic> ActiveStatisticSubActivityList, int userId, int croleId, StatusStatistic statusToCheck)
        {
            return (Int16)(from item in ActiveStatisticSubActivityList
                           where serviceEP.SubActivityIsMandatoryForParticipant(item.SubActivity.Id, userId, croleId)  && CheckStatusStatistic(item.Status, statusToCheck)
                           select item).Count();
        }

        //private Int16 GetSubActivityNotMandatoryPassedCount(IList<SubActivityStatistic> ActiveStatisticSubActivityList, int userId, int croleId)
        //{
        //    return (Int16)(from item in ActiveStatisticSubActivityList
        //                   where !serviceEP.SubActivityIsMandatoryForParticipant(item.SubActivity.Id, userId, croleId) && CheckStatusStatistic(item.Status, StatusStatistic.Passed)
        //                   select item).Count();
        //}

        private Int16 GetSubActivityMandatoryTotCount(IList<SubActivity> ActiveSubActivityList, int userId, int croleId)
        {
            return (Int16)(from item in ActiveSubActivityList
                           where serviceEP.SubActivityIsMandatoryForParticipant(item.Id, userId, croleId)
                           select item).Count();
        }

        private Int16 GetActivityMandatoryCount_byStatus(IList<ActivityStatistic> ActiveStatisticActivityList, int userId, int croleId, StatusStatistic statusToCheck)
        {
            return (Int16)(from item in ActiveStatisticActivityList
                           where serviceEP.ActivityIsMandatoryForParticipant(item.Activity.Id, userId, croleId)  && CheckStatusStatistic(item.Status, statusToCheck)
                           select item).Count();
        }

        private Int16 GetActivityNotMandatoryPassedCount(IList<ActivityStatistic> ActiveStatisticActivityList, int userId, int croleId)
        {
            return (Int16)(from item in ActiveStatisticActivityList
                           where !serviceEP.ActivityIsMandatoryForParticipant(item.Activity.Id, userId, croleId) && CheckStatusStatistic(item.Status, StatusStatistic.Passed)
                           select item).Count();
        }

        private Int16 GetActivityMandatoryTotCount(IList<Activity> ActiveActivityList, int userId, int croleId)
        {
            return (Int16)(from item in ActiveActivityList
                           where serviceEP.ActivityIsMandatoryForParticipant(item.Id, userId, croleId)
                           select item).Count();
        }

        private Int16 GetUnitMandatoryCount_byStatus(IList<UnitStatistic> ActiveStatisticUnitList, int userId, int croleId, StatusStatistic statusToCheck)
        {
            return (Int16)(from item in ActiveStatisticUnitList
                           where serviceEP.UnitIsMandatoryForParticipant(item.Unit.Id, userId, croleId)  && CheckStatusStatistic(item.Status, statusToCheck)
                           select item).Count();
        }

        private Int16 GetUnitNotMandatoryPassedCount(IList<UnitStatistic> ActiveStatisticUnitList, int userId, int croleId)
        {
            return (Int16)(from item in ActiveStatisticUnitList
                           where !serviceEP.UnitIsMandatoryForParticipant(item.Unit.Id, userId, croleId) && CheckStatusStatistic(item.Status, StatusStatistic.Passed)
                           select item).Count();
        }

        private Int16 GetUnitMandatoryTotCount(IList<Unit> ActiveUnitList, int userId, int croleId)
        {
            return (Int16)(from item in ActiveUnitList
                           where serviceEP.UnitIsMandatoryForParticipant(item.Id, userId, croleId)
                           select item).Count();
        }
        #endregion




        #region completion auto

        private bool UpdateActStatCompletionAuto(ActivityStatistic oActStat, IList<SubActivityStatistic> ActiveSubActStats)
        {

            Int64 totWeight = (Int64)(from sub in manager.GetAll<SubActivity>(item => item.ParentActivity.Id == oActStat.Activity.Id && item.Deleted == BaseStatusDeleted.None)
                                      where !sub.CheckStatus(Status.Draft)
                                      select sub.Weight).Count();

            Int64 WeightCompleted = (Int64)(from stat in ActiveSubActStats
                                            where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) || CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed)
                                            select stat).Count();

            Int64 newCompletion = (Int64)(WeightCompleted * oActStat.Activity.Weight / Math.Max(1,totWeight)); //completion reale sulle sottoattività          

            if (newCompletion != oActStat.Completion)
            {
                oActStat.Completion = newCompletion;
                return true;
            }

            return false;
        }


        private bool UpdatePathStatCompletionAuto(PathStatistic oPathStat, IList<ActivityStatistic> ActiveActStats)
        {

            Int64 WeightCompleted = (from stat in ActiveActStats
                                    where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) || CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed)
                                    select stat.Activity.Weight).Sum(a=>a);


            if (WeightCompleted != oPathStat.Completion)
            {
                oPathStat.Completion = WeightCompleted;
                return true;
            }

            return false;
        }

        #endregion

        #region Exist Stat Insert Mode
            private bool ExistSubAcStat_Insert(long subActId, int userId)
            {
                return ExistSubAcStat_Insert(subActId, userId, DateTime.Now.AddSeconds(1));
            }

            private bool ExistSubAcStat_Insert(long subActId, int userId, DateTime viewStatBefore)
            {
                return (from stat in manager.GetIQ<SubActivityStatistic>()
                        where stat.SubActivity.Id == subActId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                        select stat.Id).Count() > 0; 
            }

            private bool ExistActStat_Insert(long actId, int userId)
            {
                return ExistActStat_Insert(actId, userId, DateTime.Now.AddSeconds(1));
            }

            private bool ExistActStat_Insert(long actId, int userId, DateTime viewStatBefore)
            {
                return (from stat in manager.GetIQ<ActivityStatistic>()
                        where stat.Activity.Id == actId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                                    select stat.Id).Count() > 0;
        
            }

            private bool ExistUnitStat_Insert(long unitId, int userId)
            {
                return ExistUnitStat_Insert(unitId, userId, DateTime.Now.AddSeconds(1));
            }

            private bool ExistPathStat_Insert(long pathId, int userId, DateTime viewStatBefore)
            {
                return (from stat in manager.GetIQ<liteBaseStatistic>()
                                              where stat.IdPath == pathId && stat.IdPerson == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                              select stat.Id).Count() > 0;
           
            }

            private bool ExistPathStat_Insert(long pathId, int userId)
            {
                return ExistPathStat_Insert(pathId, userId, DateTime.Now.AddSeconds(1));
            }

            private bool ExistUnitStat_Insert(long unitId, int userId, DateTime viewStatBefore)
            {
                return (from stat in manager.GetIQ<UnitStatistic>()
                        where stat.Unit.Id == unitId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore

                        select stat.Id).Count() > 0;           
            }

        #endregion

        #region Get Stat Insert Mode
        private SubActivityStatistic GetSubAcStat_Insert(long subActId, int userId)
        {
            return GetSubAcStat_Insert(subActId, userId, DateTime.Now.AddSeconds(1));
        }

        private SubActivityStatistic GetSubAcStat_Insert(long subActId, int userId, DateTime viewStatBefore)
        {
            IList<SubActivityStatistic> subActStat = (from stat in manager.GetIQ<SubActivityStatistic>()
                                                      where stat.SubActivity.Id == subActId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                                      orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                                      select stat).Take(1).ToList();
            if (subActStat.Count == 1)
            {
                return subActStat[0];
            }
            return null;
        }
        

        private ActivityStatistic GetActStat_Insert(long actId, int userId)
        {
            return GetActStat_Insert(actId, userId, DateTime.Now.AddSeconds(1));
        }

        private ActivityStatistic GetActStat_Insert(long actId, int userId, DateTime viewStatBefore)
        {
            IList<ActivityStatistic> actStat = (from stat in manager.GetIQ<ActivityStatistic>()
                                                where stat.Activity.Id == actId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                                orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                                select stat).Take(1).ToList();
            if (actStat.Count == 1)
            {
                return actStat[0];
            }
            return null;
        }

        

        private UnitStatistic GetUnitStat_Insert(long unitId, int userId)
        {
            return GetUnitStat_Insert(unitId, userId, DateTime.Now.AddSeconds(1));
        }

        public PathStatistic GetPathStat(long pathId, int userId, DateTime viewStatBefore)
        {
            return GetPathStat_Insert(pathId, userId, viewStatBefore);
        }

        private IList<PathStatistic> GetPathsStat_Insert(int userId, DateTime viewStatBefore)
        {
            ////IList<PathStatistic> pStat = (from stat in manager.GetIQ<PathStatistic>()
            ////                              where stat.Path.Id == pathId && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
            ////                              orderby stat.CreatedOn descending
            ////                              select stat).Take(1).ToList();
            ////if (pStat.Count == 1)
            ////{
            ////    return pStat[0];
            ////}

            //var q = (from stat in manager.GetIQ<PathStatistic>() 
            //         where stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore 
            //         group stat by stat.Path.Id into grp
            //         select grp.OrderByDescending(a=>a.CreatedOn).Take(1).ToList()
            //             ).ToList();

            //IList<PathStatistic> pStat = new List<PathStatistic>();

            //foreach (var item in q)
            //{
            //    pStat.Add(item[0]);
            //}

            return null;
        }

        private IList<Int32> GetUsersWithStat(Int64 pathId, DateTime viewStatBefore, DateTime? viewStatAfter=null)
        {
            DateTime actualViewStatAfter = DateTimeExt.ValueOrMinDBTimeValue(viewStatAfter);

            var q = (from stat in manager.GetIQ<PathStatistic>()
                     where stat.Path.Id == pathId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn > actualViewStatAfter &&  stat.CreatedOn <= viewStatBefore
                     select stat.Person.Id).Distinct().ToList();

            return q;
        }

        private IList<Int64> GetPathsWithStat(int userId, DateTime viewStatBefore)
        {
            var q = (from stat in manager.GetIQ<PathStatistic>()
                     where stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                     select stat.Path.Id).Distinct().ToList();

            return q;
        }

        private PathStatistic GetPathStat_Insert(long pathId, int userId, DateTime viewStatBefore, DateTime? viewStatAfter=null)
        {
            DateTime actualViewStatAfter = DateTimeExt.ValueOrMinDBTimeValue(viewStatAfter);

            IList<PathStatistic> pStat = (from stat in manager.GetIQ<PathStatistic>()
                                          where stat.Path.Id == pathId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn > actualViewStatAfter && stat.CreatedOn <= viewStatBefore
                                          orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                          select stat).Take(1).ToList();
            if (pStat.Count == 1)
            {
                return pStat[0];
            }
            return null;
        }

        private PathStatistic GetPathStat_Insert_Started(long pathId, int userId, DateTime viewStatBefore, DateTime? viewStatAfter = null)
        {
            DateTime actualViewStatAfter = DateTimeExt.ValueOrMinDBTimeValue(viewStatAfter);

            IList<PathStatistic> pStat = (from stat in manager.GetIQ<PathStatistic>()
                                          where stat.Path.Id == pathId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn > actualViewStatAfter && stat.CreatedOn <= viewStatBefore
                                          orderby stat.CreatedOn ascending, stat.Completion descending, stat.Status descending
                                          select stat).ToList();

            var q = (from item in pStat where CheckStatusStatistic(item.Status, StatusStatistic.Started) select item);


            return q.FirstOrDefault();            
        }

        public PathStatistic GetPathStat(long pathId, int userId)
        {
            return GetPathStat_Insert(pathId, userId, DateTime.Now.AddSeconds(1));
        }

        private PathStatistic GetPathStat_Insert(long pathId, int userId)
        {
            return GetPathStat_Insert(pathId, userId, DateTime.Now.AddSeconds(1));
        }

        private UnitStatistic GetUnitStat_Insert(long unitId, int userId, DateTime viewStatBefore)
        {
            IList<UnitStatistic> unitStat = (from stat in manager.GetIQ<UnitStatistic>()
                                             where stat.Unit.Id == unitId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                             orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                             select stat).Take(1).ToList();
            if (unitStat.Count == 1)
            {
                return unitStat[0];
            }
            return null;
        }

        #endregion

        #region insert time auto stat

        private Int16 GetActivityStatMandatoryPassedComplCount_Insert(long actId, int userId)
        {
            IList<Int16> actStat = (from stat in manager.GetIQ<ActivityStatistic>()
                                    where stat.Activity.Id == actId && stat.Person != null && stat.Person.Id == userId && stat.Deleted == BaseStatusDeleted.None
                                    orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                    select stat.MandatoryPassedCompletedSubActivityCount).Take(1).ToList();

            if (actStat.Count == 1)
            {
                return actStat[0];
            }
            return 0;
        }

        private void UpdateSubActStatAuto_Insert(long subActId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, Int64 completion, bool isStarted, bool isCompleted)
        {
            /// LA sub sopra deve esse
            SubActivityStatistic oSubActStat = GetOrInitSubActStat_Insert(subActId, participantId,  evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            StatusStatistic previous = (oSubActStat == null) ? StatusStatistic.None : oSubActStat.Status;
            //NOTE: aggiornare prima la completion dello Status Completed
            ModifyStatField subActFieldChanged = ModifyStatField.None;
            Status participantStatus = serviceEP.SubActivityStatus(oSubActStat.SubActivity.Id, participantId, partecipatCRoleId, false);

            //SubActivityStatistic insertStat = InitSubActivityStatisticNoTransaction(subActId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

            if (isStarted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                subActFieldChanged |= ModifyStatField.StartedStat;
                oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            if (isCompleted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
            {
                subActFieldChanged |= ModifyStatField.CompletedAdd|ModifyStatField.Completion;
                oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.CompletedPassed);
                oSubActStat.EndDate = DateTime.Now;
                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory)) //controllo se la subact è obbligatoria
                {
                    subActFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                }

            }
            else if (!isCompleted && CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed))
            {

                subActFieldChanged |= ModifyStatField.CompletedRem | ModifyStatField.Completion;
                oSubActStat.Status -= StatusStatistic.CompletedPassed;
                oSubActStat.EndDate = DateTime.Now;
                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                {
                    subActFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                }
            }

            if (oSubActStat.Completion != completion) //verifico nuova completion
            {
                subActFieldChanged |= (oSubActStat.Completion> completion) ? ModifyStatField.CompletionDecreased : ModifyStatField.CompletionIncreased;
                oSubActStat.Completion = completion;
            }

            if (subActFieldChanged > ModifyStatField.None && (oSubActStat.SubActivity.ContentType != SubActivityType.File || (oSubActStat.SubActivity.ContentType == SubActivityType.File && IsAutoUpdateEnabled(oSubActStat.SubActivity, subActFieldChanged, previous,oSubActStat.Status))))
            {
               
                DateTime creationDate = DateTime.Now;
                oSubActStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreateSubActStatMetaInfo(oSubActStat,manager.GetPerson( evaluatorId), creationDate, evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.SaveOrUpdate<SubActivityStatistic>(oSubActStat);

                UpdateActStatAuto_Insert(oSubActStat.IdActivity, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, subActFieldChanged,creationDate);
            }

        }

        private Boolean IsAutoUpdateEnabled(SubActivity subActivity, ModifyStatField status, StatusStatistic previous, StatusStatistic actual)
        {
            return IsAutoUpdateEnabled(subActivity, subActivity.Path, status, previous, actual);
        }
        private Boolean IsAutoUpdateEnabled(SubActivity subActivity, Path path, ModifyStatField status, StatusStatistic previous, StatusStatistic actual)
        {
            if (path == null)
                return true;
            else {
                switch (subActivity.ContentType) { 
                    case SubActivityType.File:
                        return IsAutoUpdateEnabled(path.Policy.Statistics, status, previous, actual);
                    default:
                        return true;
                }
            }
        }
        private Boolean IsAutoUpdateEnabled(CompletionPolicy update,ModifyStatField status, StatusStatistic previous, StatusStatistic actual)
        {
            switch (update)
            {
                case CompletionPolicy.NoUpdateIfCompleted:
                    if (previous != StatusStatistic.CompletedPassed)
                        return true;
                    else
                        return (!CheckFieldChanged(status, ModifyStatField.CompletedMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.CompletedRem) && !CheckFieldChanged(status, ModifyStatField.ComplPassMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.PassedMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.PassedRem));

                case CompletionPolicy.UpdateOnlyIfBetter:
                    return (
                        (!CheckFieldChanged(status, ModifyStatField.CompletedMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.CompletedRem) && !CheckFieldChanged(status, ModifyStatField.CompletionDecreased) && !CheckFieldChanged(status, ModifyStatField.ComplPassMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.PassedMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.PassedRem))
                        );
                case CompletionPolicy.UpdateOnlyIfWorst:
                    return (
                         (!CheckFieldChanged(status, ModifyStatField.CompletedAdd) && !CheckFieldChanged(status, ModifyStatField.CompletedMandatoryAdd) && !CheckFieldChanged(status, ModifyStatField.ComplPassMandatoryAdd) && !CheckFieldChanged(status, ModifyStatField.PassedAdd) && !CheckFieldChanged(status, ModifyStatField.PassedMandatoryAdd) && !CheckFieldChanged(status, ModifyStatField.CompletionIncreased))
                         );
                default:
                    return true;
            }
            //return (update == CompletionPolicy.UpdateAlways || forInsert || (update == CompletionPolicy.UpdateOnlyIfBetter &&
            //    (!CheckFieldChanged(status, ModifyStatField.CompletedMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.CompletedRem) && !CheckFieldChanged(status, ModifyStatField.CompletionDecreased) && !CheckFieldChanged(status, ModifyStatField.ComplPassMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.PassedMandatoryRem) && !CheckFieldChanged(status, ModifyStatField.PassedRem))
            //    ));
        }
        private IList<SubActivityStatistic> GetActiveSubActStat_Insert(long actId, int userId, DateTime viewStatBefore)
        {
            if (true)
            {
                var subStat = (from stat in manager.GetIQ<SubActivityStatistic>()
                               where stat.IdActivity == actId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                               select new { idSubActivity = stat.SubActivity.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status = stat.Status, Completion = stat.Completion }).ToList();


                var statsIdAnonymus = (from s in subStat
                                       group s by s.idSubActivity into g
                                       let data = g.Max(s => s.CreatedOn)
                                       select new
                                       {
                                           Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault() //TODO: CHECK CREATEDON
                                       }).ToList();
                IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();
                            
                return (from stat in manager.GetIQ<SubActivityStatistic>()
                        where stat.IdActivity == actId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();

            }
            else
            {
                //con NHIBernate 3 dovrebbe andare
                var subStat = (from stat in manager.GetIQ<SubActivityStatistic>()
                               where stat.IdActivity == actId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                               group stat by stat.IdActivity into g
                               let maxDate = g.Max(stat => stat.CreatedOn)
                               select new { a = g.Where(stat => stat.CreatedOn == maxDate).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).First() }).ToList();
                return (from s in subStat select s.a).ToList();
            }
        }

        private IList<SubActivityStatistic> GetActiveSubActStat_Insert(long actId, int userId)
        {
            return GetActiveSubActStat_Insert(actId, userId, DateTime.Now.AddSeconds(1));
        }
        public IList<ActivityStatistic> GetActiveActStat_ByPathId_Insert(long pathId, int userId)
        {
            return GetActiveActStat_ByPathId_Insert(pathId, userId, DateTime.Now.AddSeconds(1));
        }

        public IList<ActivityStatistic> GetActiveActStat_ByPathId_Insert(long pathId, int userId, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<ActivityStatistic>()
                           where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idactivity = stat.Activity.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status = stat.Status, Completion = stat.Completion }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idactivity into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            /// maxItemsInContainsQuery
            if (statListId.Count > maxItemsInContainsQuery)
                return (from stat in manager.GetIQ<ActivityStatistic>()
                        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None select stat).ToList().Where(stat=> statListId.Contains(stat.Id)).ToList();
            else
                return (from stat in manager.GetIQ<ActivityStatistic>()
                        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        public IList<SubActivityStatistic> GetActiveSubActStat_ByActId_Insert(long actId, int userId, DateTime viewStatBefore)
        {
            var subStat = (from stat in manager.GetIQ<SubActivityStatistic>()
                           where stat.IdActivity == actId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                           select new { idactivity = stat.SubActivity.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status = stat.Status, Completion = stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idactivity into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            /// maxItemsInContainsQuery
            if (statListId.Count > maxItemsInContainsQuery)
                return (from stat in manager.GetIQ<SubActivityStatistic>()
                        where stat.IdActivity == actId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                        select stat).ToList().Where(stat => statListId.Contains(stat.Id)).ToList();
            else
                return (from stat in manager.GetIQ<SubActivityStatistic>()
                    where stat.IdActivity == actId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                    select stat).ToList();
        }

        public IList<SubActivityStatistic> GetActiveSubActStat_ByActId_Insert(long actId, int userId)
        {
            return GetActiveSubActStat_ByActId_Insert(actId, userId, DateTime.Now.AddSeconds(1));
        }

        public IList<ActivityStatistic> GetActiveActStat_ByUnitId_Insert(long unitId, int userId)
        {
            return GetActiveActStat_ByUnitId_Insert(unitId, userId, DateTime.Now.AddSeconds(1));
        }

        public IList<ActivityStatistic> GetActiveActStat_ByUnitId_Insert(long unitId, int userId, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<ActivityStatistic>()
                           where stat.IdUnit == unitId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idactivity = stat.Activity.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status=stat.Status,Completion=stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idactivity into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            return (from stat in manager.GetIQ<ActivityStatistic>()
                    where stat.IdUnit == unitId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                    select stat).ToList();


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        public IList<UnitStatistic> GetActiveUnitStat_ByPathId_Insert(long pathId, int userId)
        {
            return GetActiveUnitStat_ByPathId_Insert(pathId, userId, DateTime.Now.AddSeconds(1));
        }

        public IList<UnitStatistic> GetActiveUnitStat_ByPathId_Insert(long pathId, int userId, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<UnitStatistic>()
                           where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idunit = stat.Unit.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status=stat.Status, Completion=stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idunit into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            return (from stat in manager.GetIQ<UnitStatistic>()
                    where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                    select stat).ToList();


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        #region get users Stat Insert Mode
            private List<liteBaseStatistic> GetAllPathStatistics(long idPath, DateTime viewStatBefore)
            {
                var statistics = (from s in manager.GetIQ<liteBaseStatistic>()
                                  where s.IdPath == idPath && s.CreatedOn <= viewStatBefore && s.Deleted == BaseStatusDeleted.None
                               select new { IdPerson = s.IdPerson, Id = s.Id,IdPath= s.IdPath, IdUnit = s.IdUnit, IdActivity=s.IdActivity, IdSubActivity = s.IdSubActivity, Discriminator = s.Discriminator, CreatedOn = s.CreatedOn, Status = s.Status, Completion = s.Completion }).ToList();

                List<long> idStatistics = new List<long>();
                // adding path statistics
                #region "adding path statistics"
                var pathStatistics = (from s in statistics where s.Discriminator== StatisticDiscriminator.Path
                                      group s by s.IdPerson into g
                                       let data = g.Max(s => s.CreatedOn)
                                       select new
                                       {
                                           IdStatistic = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.Id).FirstOrDefault()
                                       }).ToList();
                idStatistics.AddRange(pathStatistics.Select(i => i.IdStatistic).ToList());
                #endregion
                // adding unit statistics
                #region "adding unit statistics"
                var unitStatistics = (from s in statistics
                                      where s.Discriminator == StatisticDiscriminator.Unit
                                       group s by s.IdUnit into g
                                       select new
                                       {
                                           IdUnit = g.Key,
                                           PersonGroup = (from p in g
                                                          group p by p.IdPerson into gp
                                                          let data = gp.Max(x => x.CreatedOn)
                                                          select new
                                                          {
                                                              IdStatistic = gp.Where(x => x.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(x => x.Id).FirstOrDefault()
                                                          }
                                                        )
                                       }).ToList();
                idStatistics.AddRange(unitStatistics.SelectMany(i => i.PersonGroup.Select(p => p.IdStatistic)).ToList());
                #endregion
                #region "adding activity statistics"
                var activityStatistics = (from s in statistics
                                      where s.Discriminator == StatisticDiscriminator.Activity
                                      group s by s.IdActivity into g
                                      select new
                                      {
                                          IdActivity = g.Key,
                                          PersonGroup = (from p in g
                                                         group p by p.IdPerson into gp
                                                         let data = gp.Max(x => x.CreatedOn)
                                                         select new
                                                         {
                                                             IdStatistic = gp.Where(x => x.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(x => x.Id).FirstOrDefault()
                                                         }
                                                       )
                                      }).ToList();
                idStatistics.AddRange(activityStatistics.SelectMany(i => i.PersonGroup.Select(p => p.IdStatistic)).ToList());
                #endregion
                #region "adding sub activity statistics"
                var subActivities = (from s in statistics
                                          where s.Discriminator == StatisticDiscriminator.SubActivity
                                          group s by s.IdSubActivity into g
                                          select new
                                          {
                                              IdSubActivity = g.Key,
                                              PersonGroup = (from p in g
                                                             group p by p.IdPerson into gp
                                                             let data = gp.Max(x => x.CreatedOn)
                                                             select new
                                                             {
                                                                 IdStatistic = gp.Where(x => x.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(x => x.Id).FirstOrDefault()
                                                             }
                                                           )
                                          }).ToList();
                idStatistics.AddRange(subActivities.SelectMany(i => i.PersonGroup.Select(p => p.IdStatistic)).ToList());
                #endregion
                if (idStatistics.Any())
                {
                    //int i = 0;
                    List<liteBaseStatistic> p;
                    //DateTime t1 = DateTime.Now;
                    //if (i == 0)
                    //    p = GetLiteStatistics(idStatistics);
                    //else
                        p = (from s in manager.GetIQ<liteBaseStatistic>() where s.IdPath == idPath && s.CreatedOn <= viewStatBefore && s.Deleted == BaseStatusDeleted.None select s).ToList().Where(s => idStatistics.Contains(s.Id)).ToList();
                    //double  t2 = (DateTime.Now - t1).TotalMilliseconds;
                    return p;
                }
                else
                    return new List<liteBaseStatistic>();
            }
            public List<liteBaseStatistic> GetLiteStatistics(List<long> idStatistics)
            {
                List<liteBaseStatistic> results = new List<liteBaseStatistic>();
                if (idStatistics.Count <= maxItemsInContainsQuery)
                    results = (from s in manager.GetIQ<liteBaseStatistic>() where idStatistics.Contains(s.Id) select s).ToList();
                else
                {
                    Int32 pageIndex = 0;
                    List<long> idPagedStatistics = idStatistics.Skip(pageIndex * maxItemsInContainsQuery).Take(maxItemsInContainsQuery).ToList();
                    while (idPagedStatistics.Any())
                    {
                        results.AddRange((from s in manager.GetIQ<liteBaseStatistic>() where idPagedStatistics.Contains(s.Id) select s).ToList());
                        pageIndex++;
                        idPagedStatistics = idStatistics.Skip(pageIndex * maxItemsInContainsQuery).Take(maxItemsInContainsQuery).ToList();
                    }
                }
                return results;
            }
        private IList<PathStatistic> GetUsersPathStat_Insert(long pathId, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<PathStatistic>()
                           where stat.Path.Id == pathId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status=stat.Status, Completion=stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idPerson into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();
            /// maxItemsInContainsQuery
            if (statListId.Count> maxItemsInContainsQuery)
                return (from stat in manager.GetAll<PathStatistic>(s=>s.Path.Id == pathId && s.CreatedOn <= viewStatBefore && s.Deleted == BaseStatusDeleted.None )
                    where  statListId.Contains(stat.Id)
                    select stat).ToList();
            else
                return (from stat in manager.GetIQ<PathStatistic>()
                        where stat.Path.Id == pathId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();

            //return (from stat in manager.GetIQ<PathStatistic>()
            //        where stat.Path.Id == pathId  && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
            //        select stat).ToList();


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }


        private IList<PathStatistic> GetUsersPathStat_Insert(long pathId, IList<litePerson> interestedPerson)
        {
            return GetUsersPathStat_Insert(pathId, interestedPerson, DateTime.Now.AddSeconds(1));
        }

        private IList<PathStatistic> GetUsersPathStat_Insert(long pathId, IList<litePerson> interestedPerson, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<PathStatistic>()
                           where stat.Path.Id == pathId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status = stat.Status, Completion = stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idPerson into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();
           
             ///maxItemsInContainsQuery
            return GetUsersPathStat_Insert(pathId, viewStatBefore, statListId, interestedPerson);
            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        private IList<UnitStatistic> GetUsersUnitStat_Insert(long unitId, IList<litePerson> interestedPerson)
        {
            return GetUsersUnitStat_Insert(unitId, interestedPerson, DateTime.Now.AddSeconds(1));
        }

        private IList<UnitStatistic> GetUsersUnitStat_Insert(long unitId, IList<litePerson> interestedPerson, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<UnitStatistic>()
                           where stat.Unit.Id == unitId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status = stat.Status, Completion=stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idPerson into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            /// maxItemsInContainsQuery
            if (statListId.Count + subStat.Count > maxItemsInContainsQuery)
                return (from stat in manager.GetIQ<UnitStatistic>()
                        where stat.Unit.Id == unitId  && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None select stat).ToList().Where( stat=> statListId.Contains(stat.Id)).ToList();
            else
                return (from stat in manager.GetIQ<UnitStatistic>()
                    where stat.Unit.Id == unitId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                    select stat).ToList();
            


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        private IList<UnitStatistic> GetUsersUnitsStat_Insert(long parentPathId,  DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<UnitStatistic>()
                           where stat.IdPath == parentPathId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, IdUnit=stat.Unit.Id, Status = stat.Status, Completion = stat.Completion }).ToList();

            var groupByUnit = (from s in subStat
                               group s by s.IdUnit into g
                               select new
                                {
                                    IdUnit = g.Key,
                                    PersonGroup = (from p in g
                                                   group p by p.idPerson into gp
                                                   let data = gp.Max(x => x.CreatedOn)
                                                   select new
                                                   {
                                                       Id = gp.Where(x => x.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(x => x.idStatistic).FirstOrDefault()
                                                   }
                                                    )
                                }).ToList();    

            IList<long> statListId = (from a in groupByUnit.SelectMany(x=>x.PersonGroup) select a.Id ).ToList();
            /// maxItemsInContainsQuery
            if (statListId.Count > maxItemsInContainsQuery)
                return (from stat in manager.GetIQ<UnitStatistic>()
                        where stat.IdPath == parentPathId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None select stat).ToList().Where(stat=> statListId.Contains(stat.Id)).ToList();      
            else
                return (from stat in manager.GetIQ<UnitStatistic>()
                    where stat.IdPath == parentPathId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                    select stat).ToList();                                    
        }

        private IList<ActivityStatistic> GetUsersActivityStat_Insert(long actId, IList<litePerson> interestedPerson)
        {
            return GetUsersActivityStat_Insert(actId, interestedPerson, DateTime.Now.AddSeconds(1));
        }

        private IList<ActivityStatistic> GetUsersActivityStat_Insert(long actId, IList<litePerson> interestedPerson, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<ActivityStatistic>()
                           where stat.Activity.Id == actId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status=stat.Status, Completion = stat.Completion  }).ToList();


            var statsIdAnonymus = (from s in subStat
                                   group s by s.idPerson into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            /// maxItemsInContainsQuery
            if (statListId.Count + interestedPerson.Count> maxItemsInContainsQuery)
                return (from stat in manager.GetIQ<ActivityStatistic>()
                        where stat.Activity.Id == actId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None select stat).ToList().Where(stat => interestedPerson.Contains(stat.Person) && statListId.Contains(stat.Id)).ToList();
            else
                return (from stat in manager.GetIQ<ActivityStatistic>()
                        where stat.Activity.Id == actId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        private IList<ActivityStatistic> GetUsersActivitiesStat_Insert(long parentUnitId, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<ActivityStatistic>()
                           where stat.IdUnit == parentUnitId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn,IdAct=stat.Activity.Id, Status = stat.Status, Completion=stat.Completion  }).ToList();

            var groupByAct = (from s in subStat
                               group s by s.IdAct into g
                               select new
                               {
                                   IdUnit = g.Key,
                                   PersonGroup = (from p in g
                                                  group p by p.idPerson into gp
                                                  let data = gp.Max(x => x.CreatedOn)
                                                  select new
                                                  {
                                                      Id = gp.Where(x => x.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(x => x.idStatistic).FirstOrDefault()
                                                  }
                                                   )
                               }).ToList();

            IList<long> statListId = (from a in groupByAct.SelectMany(x => x.PersonGroup) select a.Id).ToList();

            Int32 pagesize=100;

            if (statListId.Count() > pagesize)
            {
                IList<ActivityStatistic> temp = new List<ActivityStatistic>();

                Int32 counts = (Int32)Math.Ceiling((double)statListId.Count() / (double)pagesize);
                for (int i = 0; i < counts; i++)
                {
                    var q = (from stat in manager.GetIQ<ActivityStatistic>()
                             where stat.IdUnit == parentUnitId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Skip(i*pagesize).Take(pagesize).ToList().Contains(stat.Id)
                             select stat).ToList();

                    temp = temp.Concat(q).ToList();
                }

                return temp;
            }
            else
            {
                return (from stat in manager.GetIQ<ActivityStatistic>()
                        where stat.IdUnit == parentUnitId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();     
            }

            
        }

        private IList<SubActivityStatistic> GetUsersSubActivityStat_Insert(long subActId, IList<litePerson> interestedPerson)
        {
            return GetUsersSubActivityStat_Insert(subActId, interestedPerson, DateTime.Now.AddSeconds(1));
        }

        private IList<SubActivityStatistic> GetUsersSubActivityStat_Insert(long subActId, IList<litePerson> interestedPerson, DateTime viewStatBefore)
        {
            //var subStat = (from stat in manager.GetIQ<SubActivityStatistic>()
            //               where stat.SubActivity.Id == subActId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
            //               select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn }).ToList();

            var subStat = (from stat in manager.GetAll<SubActivityStatistic>(stat => stat.SubActivity.Id == subActId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None)
                            where interestedPerson.Contains(stat.Person)
                            select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, Status =stat.Status, Completion = stat.Completion }).ToList();
            var statsIdAnonymus = (from s in subStat
                                   group s by s.idPerson into g
                                   let data = g.Max(s => s.CreatedOn)
                                   select new
                                   {
                                       Id = g.Where(p => p.CreatedOn == data).OrderByDescending(p=>p.Status).ThenByDescending(p=>p.Completion).Select(p => p.idStatistic).FirstOrDefault()
                                   }).ToList();
            IList<long> statListId = (from a in statsIdAnonymus select a.Id).ToList();

            return (from stat in manager.GetAll<SubActivityStatistic>(stat=>stat.SubActivity.Id == subActId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None)
                    where  interestedPerson.Contains(stat.Person)  && statListId.Contains(stat.Id)
                    select stat).ToList();

            //return (from stat in manager.GetIQ<SubActivityStatistic>()
            //        where stat.SubActivity.Id == subActId && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
            //        select stat).ToList();


            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        private IList<SubActivityStatistic> GetUsersSubActivitiesStat_Insert(long parentActId, DateTime viewStatBefore)
        {

            var subStat = (from stat in manager.GetIQ<SubActivityStatistic>()
                           where stat.IdActivity == parentActId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, IdSubAct=stat.SubActivity.Id, Status=stat.Status, Completion = stat.Completion  }).ToList();


            var groupBySubAct = (from s in subStat
                              group s by s.IdSubAct into g
                              select new
                              {
                                  IdUnit = g.Key,
                                  PersonGroup = (from p in g
                                                 group p by p.idPerson into gp
                                                 let data = gp.Max(x => x.CreatedOn)
                                                 select new
                                                 {
                                                     Id = gp.Where(x => x.CreatedOn == data).OrderByDescending(p => p.Status).ThenByDescending(p => p.Completion).Select(x => x.idStatistic).FirstOrDefault()
                                                 }
                                                  )
                              }).ToList();

            IList<long> statListId = (from a in groupBySubAct.SelectMany(x => x.PersonGroup) select a.Id).ToList();


            return GetUsersSubActivitiesStat_Insert(parentActId, viewStatBefore, statListId);
                //return (from stat in manager.GetIQ<SubActivityStatistic>()
                //        where stat.IdActivity == parentActId && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)

            //VALIDA CON NH3
            //var actStats = (from stat in manager.GetIQ<ActivityStatistic>()
            //        where stat.IdPath == pathId && stat.Person.Id == userId && stat.CreatedOn <= viewStatBefore && stat.Deleted==BaseStatusDeleted.None
            //        group stat by stat.Activity.Id into g
            //        let maxDate = g.Max(stat => stat.CreatedOn)
            //        select new {a= g.Where(stat => stat.CreatedOn == maxDate).First()}).ToList();
            //return (from s in actStats select s.a).ToList();       
        }

        ///maxItemsInContainsQuery
        private IList<SubActivityStatistic> GetUsersSubActivitiesStat_Insert(long idActivity, DateTime viewStatBefore, IList<long> statListId)
        {

            ///maxItemsInContainsQuery
            if (statListId.Count() > maxItemsInContainsQuery)
            {
                IList<SubActivityStatistic> temp = new List<SubActivityStatistic>();

                Int32 counts = (Int32)Math.Ceiling((double)statListId.Count() / (double)maxItemsInContainsQuery);
                for (int i = 0; i < counts; i++)
                {
                    var q = (from stat in manager.GetIQ<SubActivityStatistic>()
                             where stat.IdActivity == idActivity && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Skip(i * maxItemsInContainsQuery).Take(maxItemsInContainsQuery).ToList().Contains(stat.Id)
                             select stat).ToList();

                    temp = temp.Concat(q).ToList();
                }

                return temp;
            }
            else
            {
                return (from stat in manager.GetIQ<SubActivityStatistic>()
                        where stat.IdActivity == idActivity && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();
            } 
        }
        private IList<PathStatistic> GetUsersPathStat_Insert(long idPath, DateTime viewStatBefore, IList<long> statListId, IList<litePerson> interestedPerson)
        {
            /// maxItemsInContainsQuery
            if (statListId.Count() + interestedPerson.Count() > maxItemsInContainsQuery)
            {

                IList<PathStatistic> temp = new List<PathStatistic>();

                Int32 counts = (Int32)Math.Ceiling((double)interestedPerson.Count() / (double)maxItemsInContainsQuery);
                for (int i = 0; i < counts; i++)
                {
                    var q = (from stat in manager.GetIQ<PathStatistic>()
                             where stat.Path.Id == idPath && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && interestedPerson.Skip(i * maxItemsInContainsQuery).Take(maxItemsInContainsQuery).ToList().Contains(stat.Person)
                             select stat).ToList();

                    temp = temp.Concat(q).ToList();
                }

                return temp.Where(s => statListId.Contains(s.Id)).ToList();
                //return (from stat in manager.GetIQ<PathStatistic>()
                //        where stat.Path.Id == idPath && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None
                //        select stat).ToList().Where(stat =>
                //        interestedPerson.Contains(stat.Person) && statListId.Contains(stat.Id)).ToList();
            }
            else
            {
                return (from stat in manager.GetIQ<PathStatistic>()
                        where stat.Path.Id == idPath && interestedPerson.Contains(stat.Person) && stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None && statListId.Contains(stat.Id)
                        select stat).ToList();
            }
        }
        #endregion





        #region GetOrInit Stat_Insert

        public UnitStatistic GetOrInitUnitStat_Insert(long unitId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            UnitStatistic lastStat = GetUnitStat_Insert(unitId, participantId);
            if (lastStat == null)
            {
                return InitUnitStatisticNoTrasaction(unitId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
            else
            {
                return CopyUnitStatistic(lastStat, participantId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
        }

        public ActivityStatistic GetOrInitActStat_Insert(long actId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            ActivityStatistic lastStat = GetActStat_Insert(actId, participantId);
            if (lastStat == null)
            {
                return InitActStatisticNoTrasaction(actId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
            else
            {
                return CopyActivityStatistic(lastStat, participantId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
        }

        public SubActivityStatistic GetOrInitSubActStat_Insert(long subActId, int participantId,  int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            SubActivityStatistic lastStat = GetSubAcStat_Insert(subActId, participantId);
            if (lastStat == null)
            {
                return InitSubActivityStatisticNoTransaction(subActId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
            else
            {
                return CopySubActivityStatistic(lastStat, participantId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
        }

        public PathStatistic GetOrInitPathStat_Insert(long pathId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            PathStatistic lastStat = GetPathStat_Insert(pathId, participantId);
            if (lastStat == null)
            {
                return InitPathStatisticNoTransaction(pathId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
            else
            {
                return CopyPathStatistic(lastStat, participantId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }
        }
        
        #endregion

        #region Init Stats Browsed

        /// <summary>
        /// pper ora inutile
        /// </summary>
        /// <param name="subActId"></param>
        /// <param name="participantId"></param>
        /// <param name="partecipatCRoleId"></param>
        /// <param name="evaluatorId"></param>
        /// <param name="evaluatorIPaddress"></param>
        /// <param name="evaluatorProxyIPaddress"></param>
        /// <returns></returns>
        public bool InitSubActBrowsed(long subActId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            if (ExistSubAcStat_Insert(subActId, participantId))
            {
                return true;
            }
            else
            {
                try
                {
                    manager.BeginTransaction();
                    SubActivityStatistic oStat = InitSubActivityStatisticNoTransaction(subActId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                    oStat.Status |= StatusStatistic.Browsed;
                    manager.SaveOrUpdate(oStat);
                    SubInitActBrowsed(oStat.IdActivity, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                    manager.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    manager.RollBack();
                    return false;
                }
               
            }        
        }

        private void SubInitActBrowsed(long actId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            if (!ExistActStat_Insert(actId, participantId))
            {
                ActivityStatistic oStat = InitActStatisticNoTrasaction(actId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                oStat.Status |= StatusStatistic.Browsed;
                manager.SaveOrUpdate(oStat);
                if (serviceEP.CheckEpType(serviceEP.GetEpType(oStat.IdPath, ItemType.Path), EPType.Auto))
                {
                    SubInitPathBrowsed(oStat.IdPath, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                }
                else
                {
                    SubInitUnitBrowsed(oStat.IdUnit, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                }
            }
        }

        public bool InitActBrowsed(long actId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            try
            {
                manager.BeginTransaction();
                SubInitActBrowsed(actId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                manager.RollBack();
                return false;
            }
        }

        private void SubInitUnitBrowsed(long unitId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            if (!ExistUnitStat_Insert(unitId, participantId))
            {                
                UnitStatistic oStat = InitUnitStatisticNoTrasaction(unitId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                oStat.Status |= StatusStatistic.Browsed;
                manager.SaveOrUpdate(oStat);
                SubInitPathBrowsed(oStat.IdPath, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            }  
        }

        public bool InitUnitBrowsed(long unitId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            try
            {
                manager.BeginTransaction();
                SubInitUnitBrowsed(unitId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                manager.RollBack();
                return false;
            }
        }

        private void SubInitPathBrowsed(long pathId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            if (!ExistPathStat_Insert(pathId, participantId))
            {
                PathStatistic oStat = InitPathStatisticNoTransaction(pathId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                oStat.Status |= StatusStatistic.Browsed;
                manager.SaveOrUpdate(oStat);
            }
        }

        public bool InitPathBrowsed(long pathId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            try
            {
                manager.BeginTransaction();
                SubInitPathBrowsed(pathId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                manager.RollBack();
                return false;
            }
        }

        #endregion


        private void UpdateActStatAuto_Insert(long actId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField subActFieldChanged,DateTime creationDate)
        {            
            //AGGIORNARE I MANDATORY PASS COMPL

            ActivityStatistic oActStat = GetOrInitActStat_Insert(actId, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            ModifyStatField actFieldChanged = ModifyStatField.None;
            Status participantStatus = serviceEP.ActivityStatus(actId, participantId, partecipatCRoleId, false);

        
            //STATUS.STARTED
            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                actFieldChanged |= ModifyStatField.StartedStat;
                oActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            //UPDATE COMPLETION DA STUDIARE COME MODIFICARE
            IList<SubActivityStatistic> ActiveSubActStats = GetActiveSubActStat_Insert(actId, participantId);
            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Completion) && UpdateActStatCompletionAuto(oActStat, ActiveSubActStats))
            {
                actFieldChanged |= ModifyStatField.Completion;
            }                   

           if (CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryAdd) || CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryRem))
           {
               oActStat.MandatoryPassedCompletedSubActivityCount = (Int16)(from a in ActiveSubActStats where CheckStatusStatistic(a.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(a.SubActivity.Status, Status.Mandatory) select a).Count();
               actFieldChanged |= ModifyStatField.Updated;
           }         

            //UPDATE STATUS.COMPLETED ADD 
            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
            {
             

                if (oActStat.Completion >= (oActStat.Activity.MinCompletion * oActStat.Activity.Weight / 100))
                {
                    Int16 subActMandCount = serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, participantId, partecipatCRoleId);
                    if (subActMandCount == oActStat.MandatoryPassedCompletedSubActivityCount)
                    {
                        oActStat.Status |= (StatusStatistic.CompletedPassed | StatusStatistic.Browsed | StatusStatistic.Started);
                        actFieldChanged |= ModifyStatField.CompletedAdd;

                        //UPDATE ACT MANDATORY COUNT ADD
                        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                        {
                            actFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                        }
                    }
                }

            }//UPDATE STATUS.COMPLETED REMOVE 
            else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedRem))
            {
               
                if (oActStat.Completion <= oActStat.Activity.MinCompletion)
                {
                    oActStat.Status -= StatusStatistic.CompletedPassed;
                    actFieldChanged |= ModifyStatField.CompletedRem;

                    //UPDATE ACT MANDATORY COUNT remove
                    if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                    {
                        actFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                    }
                }
                else if (serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, participantId, partecipatCRoleId) != oActStat.MandatoryPassedCompletedSubActivityCount)
                {
                    oActStat.Status -= StatusStatistic.CompletedPassed;
                    actFieldChanged |= ModifyStatField.CompletedRem;

                    //UPDATE ACT MANDATORY COUNT remove
                    if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                    {
                        actFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                    }
                }
            }

            if (actFieldChanged > ModifyStatField.None)
            {
                oActStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreateActStatMetaInfo(oActStat, manager.GetPerson(evaluatorId), creationDate, evaluatorIPaddress, evaluatorProxyIPaddress);
             
                manager.SaveOrUpdate(oActStat);
              
                UpdateEpStatAuto_Insert(oActStat.IdPath, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, actFieldChanged,creationDate);
            }

        }

        private void UpdateEpStatAuto_Insert(long pathId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField actFieldChanged,DateTime creationDate)
        {
            PathStatistic oPathStat = GetOrInitPathStat_Insert(pathId, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

            Status participantStatus = serviceEP.PathStatus(oPathStat.Path.Id, participantId, partecipatCRoleId, false);
            bool isUpdate = false;
           
            //STATUS.STARTED
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                isUpdate = true;
                oPathStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            //UPDATE COMPLETION
            IList<ActivityStatistic> ActiveActStats = GetActiveActStat_ByPathId_Insert(pathId, participantId);
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.Completion) && UpdatePathStatCompletionAuto(oPathStat, ActiveActStats))
            {
                isUpdate = true;
            }
           
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryAdd) || CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryRem))
            {
                oPathStat.MandatoryPassedCompletedUnitCount =(Int16) (from a in ActiveActStats where CheckStatusStatistic(a.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(a.Activity.Status,Status.Mandatory) select a).Count();
               
                isUpdate = true;
            }
         

            //UPDATE STATUS.COMPLETED ADD 
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
            {
            

                if (oPathStat.Completion >= (oPathStat.Path.MinCompletion * oPathStat.Path.Weight / 100))
                {
                    Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount_byPathId(oPathStat.Path.Id, participantId, partecipatCRoleId);
                    if (actMandCount == oPathStat.MandatoryPassedCompletedUnitCount)
                    {
                        oPathStat.Status |= (StatusStatistic.CompletedPassed | StatusStatistic.Browsed | StatusStatistic.Started);
                        isUpdate = true;

                    }
                }

            }//UPDATE STATUS.COMPLETED REMOVE 
            else if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedRem) && CheckStatusStatistic(oPathStat.Status, StatusStatistic.Completed))
            {
             
                if (oPathStat.Completion <= oPathStat.Path.MinCompletion)
                {
                    oPathStat.Status -= StatusStatistic.CompletedPassed;
                    isUpdate = true;
                }
                else if (serviceEP.GetActiveMandatoryActivitiesCount_byPathId(oPathStat.Path.Id, participantId, partecipatCRoleId) != oPathStat.MandatoryPassedCompletedUnitCount)
                {
                    oPathStat.Status -= StatusStatistic.CompletedPassed;
                    isUpdate = true;
                }
            }

            if (isUpdate)
            {
                oPathStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreatePathStatMetaInfo(oPathStat, manager.GetPerson(evaluatorId), creationDate, evaluatorIPaddress, evaluatorProxyIPaddress);

                manager.SaveOrUpdate(oPathStat);                
            }

        }

        #endregion
        
        #region Statistic Init




        public bool UnitIsCompleted(IList<dtoActivity> act)
        {
            return (from a in act where CheckStatusStatistic(a.statusStat, StatusStatistic.CompletedPassed) || serviceEP.CheckStatus(a.Status, Status.Text) select a).Count() == act.Count();
        }


        private void UpdateSubActManual_Insert(long subActId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, Int64 completion, Int16 Mark, bool isStarted, bool isCompleted, bool isPassed)
        {

            SubActivityStatistic oSubActStat = GetOrInitSubActStat_Insert(subActId, participantId,  evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

            //NOTE: aggiornare prima la completion dello Status Completed
            ModifyStatField subActFieldChanged = ModifyStatField.None;
            Status participantStatus = serviceEP.SubActivityStatus(oSubActStat.SubActivity.Id, participantId, partecipatCRoleId, false);


            if (isStarted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                subActFieldChanged |= ModifyStatField.StartedStat;
                oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            if (oSubActStat.Completion != completion)
            {
                subActFieldChanged |= ModifyStatField.Completion;
                oSubActStat.Completion = completion;
            }
           
            if (isCompleted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed)))
            {
                subActFieldChanged |= ModifyStatField.CompletedAdd;
                oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed);
                oSubActStat.EndDate = DateTime.Now;
                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                {
                    subActFieldChanged |= ModifyStatField.CompletedMandatoryAdd;
                    if (CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed))
                    {
                        subActFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                    }
                }
            }
            else if (!isCompleted && CheckStatusStatistic(oSubActStat.Status, StatusStatistic.Completed))
            {
                bool remMandatoryCompletedSubActivityCount = CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed);
                subActFieldChanged |= ModifyStatField.CompletedRem;
                oSubActStat.Status -= StatusStatistic.Completed;
                oSubActStat.EndDate = DateTime.Now;
                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
                {
                    subActFieldChanged |= ModifyStatField.CompletedMandatoryRem;
                    
                    if(remMandatoryCompletedSubActivityCount)
                    {
                        subActFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                    }
                }
            }

            subActFieldChanged |= SubUpdateSubActStaMarkAndStatusPassed_Insert(oSubActStat, Mark, isPassed, participantStatus);

            if (subActFieldChanged > ModifyStatField.None)
            {
                DateTime creationDate = DateTime.Now;
                oSubActStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreateSubActStatMetaInfo(oSubActStat, manager.GetPerson(evaluatorId),creationDate ,evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.SaveOrUpdate(oSubActStat);
                UpdateActStatManual_Insert(oSubActStat.IdActivity, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, subActFieldChanged,creationDate);
            }
        }

        private ModifyStatField SubUpdateSubActStaMarkAndStatusPassed_Insert(SubActivityStatistic oSubActStat, Int16 Mark, bool isPassed, Status participantStatus)
        {

            ModifyStatField subActFieldChanged = ModifyStatField.None;

            //  bool isMarkChanged = false, isUpdated=false;
            if (oSubActStat.Mark != Mark)
            {
                subActFieldChanged |= ModifyStatField.Mark;
                oSubActStat.Mark = Mark;
            }

            //    StatusOperation OperationStatPass = StatusOperation.None;
            if (isPassed && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Passed)))
            {
                subActFieldChanged |= ModifyStatField.PassedAdd;
                oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Passed);
                oSubActStat.EndDate = DateTime.Now;

                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory)  )
                {
                    subActFieldChanged |= ModifyStatField.PassedMandatoryAdd;
                    if (CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed))
                    {
                        subActFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                    }
                }
            }
            else if (!isPassed && CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Passed)))
            {
                bool remMandatoryPassedCompletedSubActivityCount= CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed);
                subActFieldChanged |= ModifyStatField.PassedRem;
                oSubActStat.Status -= StatusStatistic.Passed;

                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory) )
                {
                    subActFieldChanged |= ModifyStatField.PassedMandatoryRem;
                    if (remMandatoryPassedCompletedSubActivityCount)
                    {
                        subActFieldChanged |= ModifyStatField.ComplPassMandatoryRem;                
                    }
                }
            }
                  
            return subActFieldChanged;
        }

        private void UpdateActStatManual_Insert(long actId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField subActFieldChanged, DateTime creationDate)
        {
            ActivityStatistic oActStat = GetOrInitActStat_Insert(actId, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            ModifyStatField actFieldChanged = ModifyStatField.None;
            Status participantStatus = serviceEP.ActivityStatus(oActStat.Activity.Id, participantId, partecipatCRoleId, false);


            //STATUS.STARTED
            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                actFieldChanged |= ModifyStatField.StartedStat;
                oActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            //UPDATE COMPLETION
            IList<SubActivityStatistic> ActiveSubActStats = GetActiveSubActStat_Insert(actId, participantId);
            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Completion) && UpdateActStatCompletionManual(oActStat, ActiveSubActStats))
            {
                actFieldChanged |= ModifyStatField.Completion;
            }
    
            //UPDATE STATUS.COMPLETED ADD //REMOVE
            actFieldChanged |= UpdateCompletedStatus_byActStat_Insert(oActStat,ActiveSubActStats, participantStatus,ref subActFieldChanged);
          
       
            //MArk and Passed Status
            actFieldChanged |= SubUpdateActStaMarkAndStatusPassed_Insert(oActStat, ActiveSubActStats, subActFieldChanged);

            if (actFieldChanged > ModifyStatField.None)
            {
                oActStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreateActStatMetaInfo(oActStat, manager.GetPerson(evaluatorId), creationDate, evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.SaveOrUpdate(oActStat);
                UpdateUnitStatManual_Insert(oActStat.IdUnit, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, actFieldChanged,creationDate);
            }

        }


        private void UpdateUnitStatManual_Insert(long unitId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField actFieldChanged,DateTime creationDate)
        {
            UnitStatistic oUnitStat = GetOrInitUnitStat_Insert(unitId, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
           
            ModifyStatField unitFieldChanged = ModifyStatField.None;
            Status participantStatus = serviceEP.UnitStatus(unitId, participantId, partecipatCRoleId, false);

         
            //STATUS.STARTED
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                unitFieldChanged |= ModifyStatField.StartedStat;
                oUnitStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            //UPDATE COMPLETION
            IList<ActivityStatistic> ActiveActStats = GetActiveActStat_ByUnitId_Insert(unitId,participantId);
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.Completion) && UpdateUnitStatCompletionManual(oUnitStat, ActiveActStats))
            {
                unitFieldChanged |= ModifyStatField.Completion;
            }
          
            //UPDATE STATUS.COMPLETED ADD //REMOVE
            unitFieldChanged |= UpdateCompletedStatus_byUnitStat_Insert(oUnitStat, ActiveActStats,participantStatus,ref actFieldChanged);


            //MArk and Passed Status
            unitFieldChanged |= SubUpdateUnitStaMarkAndStatusPassed_Insert(oUnitStat, ActiveActStats, actFieldChanged);

            if (unitFieldChanged > ModifyStatField.None)
            {
                oUnitStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreateUnitStatMetaInfo(oUnitStat, manager.GetPerson(evaluatorId), creationDate,evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.SaveOrUpdate(oUnitStat);
                //update EP
                UpdateEpStatManual_Insert(oUnitStat.IdPath, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, unitFieldChanged, creationDate);
            }

        }


        private void UpdateEpStatManual_Insert(long pathId, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField unitFieldChanged, DateTime creationDate)
        {
            PathStatistic oPathStat = GetOrInitPathStat_Insert(pathId, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            Status participantStatus = serviceEP.PathStatus(oPathStat.Path.Id, participantId, partecipatCRoleId, false);                     

            //STATUS.STARTED
            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                oPathStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
            }

            //UPDATE COMPLETION   
            IList<UnitStatistic> ActiveUnitStats = GetActiveUnitStat_ByPathId_Insert(pathId, participantId);
            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.Completion))
            {
                UpdatePathStatCompletionManual(oPathStat, ActiveUnitStats);
            }

            //UPDATE STATUS.COMPLETED ADD //REMOVE
            UpdateCompletedStatus_byPathStat_Insert(oPathStat,ActiveUnitStats, participantStatus,ref unitFieldChanged);


            //MArk and Passed Status
            SubUpdateEpStaMarkAndStatusPassed_Insert(oPathStat, ActiveUnitStats, unitFieldChanged);

            if (unitFieldChanged > ModifyStatField.None)
            {
                oPathStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, creationDate);
                //CreatePathStatMetaInfo(oPathStat, manager.GetPerson(evaluatorId), creationDate,evaluatorIPaddress, evaluatorProxyIPaddress);
                manager.SaveOrUpdate(oPathStat);
            }

        }

          
        private ModifyStatField SubUpdateActStaMarkAndStatusPassed_Insert(ActivityStatistic oActStat, IList<SubActivityStatistic> activeSubActStats, ModifyStatField subActFieldChanged)
        {

            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedMandatoryAdd) || CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedMandatoryRem))
            {
                oActStat.MandatoryPassedSubActivityCount = (Int16)(from stat in activeSubActStats where CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && serviceEP.CheckStatus(stat.SubActivity.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryAdd) || CheckFieldChanged(subActFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                {
                    oActStat.MandatoryPassedCompletedSubActivityCount = (Int16)(from stat in activeSubActStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.SubActivity.Status, Status.Mandatory) select stat).Count();
                }
            }
           

            ModifyStatField actFieldChanged = ModifyStatField.None;
           

            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Mark))
            {               
                actFieldChanged |= UpdateActStatMarkManual(oActStat,activeSubActStats );
            }

            if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedAdd) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                if (oActStat.Mark >= oActStat.Activity.MinMark)
                {

                    Int16 subActMandCount = serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0);
                    if (subActMandCount == oActStat.MandatoryPassedSubActivityCount)
                    {                       
                     
                        Int16 PassedCompletion = (Int16)(100 * GetSubActWeightSum_byStatusStat(activeSubActStats, StatusStatistic.Passed) / Math.Max(1,serviceEP.GetActiveSubActivitiesWeightSum(oActStat.Activity.SubActivityList)));
                        if (PassedCompletion >= oActStat.Activity.MinCompletion)
                        {
                            oActStat.Status |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
                            actFieldChanged |= ModifyStatField.PassedAdd;
                        }

                        if (serviceEP.ActivityIsMandatoryForParticipant(oActStat.Activity.Id, 0, 0))
                        {                            
                            actFieldChanged |= ModifyStatField.PassedMandatoryAdd;
                            if (CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed))
                            {
                                actFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                            }
                        }

                    }
                }
            }
            else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedRem) && CheckStatusStatistic(oActStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
            {

                if (oActStat.Mark <= oActStat.Activity.MinMark)
                {
                    actFieldChanged |= SubRemoveStatusOperation_byActStat_Insert(oActStat);
                }
                else if (serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0) != oActStat.MandatoryPassedSubActivityCount)
                {
                    actFieldChanged |= SubRemoveStatusOperation_byActStat_Insert(oActStat);
                }
                else if ((Int64)(100 * GetSubActWeightSum_byStatusStat(GetActiveSubActStat_Insert(oActStat.Activity.Id,oActStat.Person.Id), StatusStatistic.Passed) / Math.Max(1, serviceEP.GetActiveSubActivitiesWeightSum(oActStat.Activity.SubActivityList))) < oActStat.Activity.MinCompletion)
                {
                    actFieldChanged |= SubRemoveStatusOperation_byActStat_Insert(oActStat);
                }
            }

            return actFieldChanged;
        }
                
        private ModifyStatField SubRemoveStatusOperation_byActStat_Insert(ActivityStatistic oActStat)
        {
            ModifyStatField actFieldChanged = ModifyStatField.None;
            bool remMandatoryPassedCompletedActivityCount = CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed);

            oActStat.Status -= StatusStatistic.Passed;
            actFieldChanged |= ModifyStatField.PassedRem;

            if (serviceEP.ActivityIsMandatoryForParticipant(oActStat.Activity.Id, 0, 0))
            {
                actFieldChanged |= ModifyStatField.PassedMandatoryRem;

                if (remMandatoryPassedCompletedActivityCount)
                {
                    actFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                }
            }
           
            return actFieldChanged;
        }


        private ModifyStatField SubUpdateUnitStaMarkAndStatusPassed_Insert(UnitStatistic oUnitStat,IList<ActivityStatistic> activeActStats, ModifyStatField actFieldChanged)
        {
            if (CheckFieldChanged(actFieldChanged, ModifyStatField.PassedMandatoryAdd) || CheckFieldChanged(actFieldChanged, ModifyStatField.PassedMandatoryRem))
            {
                oUnitStat.MandatoryPassedActivityCount = (Int16)(from stat in activeActStats where CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && serviceEP.CheckStatus(stat.Activity.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryAdd) || CheckFieldChanged(actFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                {
                    oUnitStat.MandatoryPassedCompletedActivityCount = (Int16)(from stat in activeActStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.Activity.Status, Status.Mandatory) select stat).Count();
                }
            }
          
            ModifyStatField unitFieldChanged = ModifyStatField.None;

            if (CheckFieldChanged(actFieldChanged, ModifyStatField.Mark))
            {
                unitFieldChanged |= UpdateUnitStatMarkManual(oUnitStat, activeActStats);
            }

            if (CheckFieldChanged(actFieldChanged, ModifyStatField.PassedAdd) && !CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
            {
                if (oUnitStat.Mark >= oUnitStat.Unit.MinMark)
                {
                    Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0);
                    if (actMandCount == oUnitStat.MandatoryPassedActivityCount)
                    {

                        Int16 PassedCompletion = (Int16)(100 * GetActWeightSum_byStatusStat(activeActStats, StatusStatistic.Passed) / Math.Max(1,serviceEP.GetActiveActivitiesWeightSum(oUnitStat.Unit.Id)));
                        if (PassedCompletion >= oUnitStat.Unit.MinCompletion)
                        {
                            oUnitStat.Status |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
                            unitFieldChanged |= ModifyStatField.PassedAdd;
                        }

                        if (serviceEP.UnitIsMandatoryForParticipant(oUnitStat.Unit.Id, 0, 0))
                        {
                            unitFieldChanged |= ModifyStatField.PassedMandatoryAdd;
                            if (CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed))
                            {
                                unitFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
                            }
                        }
                    }
                }
            }
            else if (CheckFieldChanged(actFieldChanged, ModifyStatField.PassedRem) && CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
            {

                if (oUnitStat.Mark <= oUnitStat.Unit.MinMark)
                {
                    unitFieldChanged |= SubRemoveStatusOperation_byUnitStat_Insert(oUnitStat);
                }
                else if (serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0) != oUnitStat.MandatoryPassedActivityCount)
                {
                    unitFieldChanged |= SubRemoveStatusOperation_byUnitStat_Insert(oUnitStat);
                }
                else if ((Int64)(100 * GetActWeightSum_byStatusStat(GetActiveActStat_ByUnitId_Insert(oUnitStat.Unit.Id,oUnitStat.Person.Id), StatusStatistic.Passed) / Math.Max(1, serviceEP.GetActiveActivitiesWeightSum(oUnitStat.Unit.Id))) < oUnitStat.Unit.MinCompletion)
                {
                    unitFieldChanged |= SubRemoveStatusOperation_byUnitStat_Insert(oUnitStat);
                }
            }

            return unitFieldChanged;
        }

        private ModifyStatField SubRemoveStatusOperation_byUnitStat_Insert(UnitStatistic oUnitStat)
        {
            bool remMandatoryPassedCompletedActivityCount = CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed);
            ModifyStatField unitFieldChanged = ModifyStatField.PassedRem;
            oUnitStat.Status -= StatusStatistic.Passed;

            if (serviceEP.UnitIsMandatoryForParticipant(oUnitStat.Unit.Id, 0, 0))
            {
                unitFieldChanged |= ModifyStatField.PassedMandatoryRem;

                if (remMandatoryPassedCompletedActivityCount)
                {
                    unitFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
                } 

            }
            return unitFieldChanged;
        }      

        private void SubUpdateEpStaMarkAndStatusPassed_Insert(PathStatistic oEpStat, IList<UnitStatistic> activeUnitStats, ModifyStatField unitFieldChanged)
        {

            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.PassedMandatoryAdd) || CheckFieldChanged(unitFieldChanged, ModifyStatField.PassedMandatoryRem))
            {
                oEpStat.MandatoryPassedUnitCount = (Int16)(from stat in activeUnitStats where CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && serviceEP.CheckStatus(stat.Unit.Status, Status.Mandatory) select stat).Count();
                if (CheckFieldChanged(unitFieldChanged, ModifyStatField.ComplPassMandatoryAdd) || CheckFieldChanged(unitFieldChanged, ModifyStatField.ComplPassMandatoryRem))
                {
                    oEpStat.MandatoryPassedCompletedUnitCount = (Int16)(from stat in activeUnitStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) && serviceEP.CheckStatus(stat.Unit.Status,Status.Mandatory) select stat).Count();
                }
            }
          

            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.Mark))
            {
                UpdatePathStatMarkManual(oEpStat,activeUnitStats);
            }

            if (CheckFieldChanged(unitFieldChanged, ModifyStatField.PassedAdd))
            {

                if (!CheckStatusStatistic(oEpStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
                {

                    if (oEpStat.Mark >= oEpStat.Path.MinMark)
                    {
                        Int16 unitMandCount = serviceEP.GetActiveMandatoryUnitCount(oEpStat.Path.Id, 0, 0);
                        if (unitMandCount == oEpStat.MandatoryPassedUnitCount)
                        {
                            Int64 PassedCompletion = (Int64)(100 * GetUnitStatWeightSum_byStatusStat(activeUnitStats, StatusStatistic.Passed) / Math.Max(1, serviceEP.GetActiveUnitsWeightSum(oEpStat.Path.Id)));
                            if (PassedCompletion >= oEpStat.Path.MinCompletion)
                            {
                                oEpStat.Status |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
                            }
                        }

                    }
                }

            }
            else if (CheckFieldChanged(unitFieldChanged, ModifyStatField.PassedRem))
            {
                if (CheckStatusStatistic(oEpStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
                {
                    if (oEpStat.Mark <= oEpStat.Path.MinMark)
                    {
                        oEpStat.Status -= StatusStatistic.Passed;
                    }
                    else if (serviceEP.GetActiveMandatoryUnitCount(oEpStat.Path.Id, 0, 0) != oEpStat.MandatoryPassedUnitCount)
                    {
                        oEpStat.Status -= StatusStatistic.Passed;
                    }
                    else if ((Int64)(100 * GetUnitStatWeightSum_byStatusStat(activeUnitStats, StatusStatistic.Passed) / Math.Max(1, serviceEP.GetActiveUnitsWeightSum(oEpStat.Path.Id))) < oEpStat.Path.MinCompletion)
                    {
                        oEpStat.Status -= StatusStatistic.Passed;
                    }
                }
            }
        }



        public void InitOrUpdateSubActivityNoTransaction(long SubActivityId, int participantId, int cRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, Int64 completion, Int16 Mark, bool isStarted, bool isCompleted, bool isPassed)
        {
       
            EPType epType = (EPType)manager.Get<SubActivity>(SubActivityId).Path.EPType;

            if (serviceEP.CheckEpType(epType,EPType.Manual) ) //percorsi con valutazione manuale
            {
               UpdateSubActManual_Insert(SubActivityId, participantId, cRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, completion, Mark, isStarted, isCompleted, isPassed);
            }
            else if (serviceEP.CheckEpType(epType,EPType.Auto)) //percorsi basati sulla partecipazione (no voto)
            {
                //UpdateSubActStatAuto(oSubActStat, participantId, cRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, completion, isStarted, isCompleted);
                UpdateSubActStatAuto_Insert(SubActivityId, participantId, cRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, completion, isStarted, isCompleted);
            }

        }


        public bool UpdateSubActsMark(long subActId, List<dtoUserStatToEvaluate> listOfVote, int communityId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress,DateTime dateToViewStat)
        {
            try
            {
                SubActivityStatistic subActStat;
                manager.BeginTransaction();             
               

                foreach (dtoUserStatToEvaluate item in listOfVote)
                {             
                    subActStat=manager.Get<SubActivityStatistic>(item.StatId);
                    int idRole = manager.GetActiveSubscriptionIdRole(item.UserId, communityId);
                    Status participantStatus = serviceEP.SubActivityStatus(subActId, item.UserId, idRole, false);
                    //Non essendoci un voto minimo assegno sempre passed a true
                    ModifyStatField subActMod = SubUpdateSubActStaMarkAndStatusPassed_Insert(subActStat, item.Mark, true, serviceEP.SubActivityStatus(subActId, item.UserId, idRole, false));
                    
                    if (subActMod!=ModifyStatField.None)
                    {
                        subActStat.UpdateMetaInfo(evaluatorId, evaluatorIPaddress,evaluatorProxyIPaddress);
                        manager.SaveOrUpdate(subActStat);
                    
                        ActivityStatistic oActStat=GetActStat_Insert(subActStat.IdActivity, item.UserId,dateToViewStat);
                        ModifyStatField actMod=SubUpdateActStaMarkAndStatusPassed_Insert(oActStat,GetActiveSubActStat_Insert(subActStat.IdActivity,item.UserId,dateToViewStat),subActMod);

                        if (actMod != ModifyStatField.None)
                        {
                            oActStat.UpdateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                            manager.SaveOrUpdate(oActStat);

                            UnitStatistic oUnitstat = GetUnitStat_Insert(subActStat.IdUnit, item.UserId, dateToViewStat);
                            ModifyStatField unitMod = SubUpdateUnitStaMarkAndStatusPassed_Insert(oUnitstat, GetActiveActStat_ByUnitId_Insert(subActStat.IdUnit, item.UserId, dateToViewStat), actMod);

                            if (unitMod != ModifyStatField.None)
                            {
                                oUnitstat.UpdateMetaInfo(evaluatorId,evaluatorIPaddress,evaluatorProxyIPaddress);
                                manager.SaveOrUpdate(oUnitstat);

                                PathStatistic oEpStat = GetPathStat_Insert(subActStat.IdPath, item.UserId, dateToViewStat);
                                SubUpdateEpStaMarkAndStatusPassed_Insert(oEpStat, GetActiveUnitStat_ByPathId_Insert(subActStat.IdPath, item.UserId, dateToViewStat), unitMod);

                                oEpStat.UpdateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
                                oEpStat.ModifiedIpAddress = evaluatorIPaddress;
                                oEpStat.ModifiedProxyIpAddress = evaluatorProxyIPaddress;
                                manager.SaveOrUpdate(oEpStat);
                            }

                        }                   
                    
                    }
                }
                
                manager.Commit();
            }
            catch (Exception ex) 
            {
                manager.RollBack();
                return false;
            }
            return true;
        }

        private SubActivityStatistic InitSubActivityStatisticNoTransaction(long SubActivityId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            SubActivity oSubAct=manager.Get<SubActivity>(SubActivityId);

            SubActivityStatistic oStatSub = new SubActivityStatistic();
            oStatSub.SubActivity = oSubAct;
        //   oStatSub.ParentStat = GetOrInitActivityStatisticNoTransaction(oStatSub.SubActivity.ParentActivity.Id, participantId,evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            DateTime CurrentTime = DateTime.Now;
            oStatSub.Person = manager.GetLitePerson(participantId);
            oStatSub.CreateMetaInfo(evaluatorId,evaluatorIPaddress,evaluatorProxyIPaddress,CurrentTime);
            oStatSub.StartDate = CurrentTime;
            oStatSub.IdActivity = oSubAct.ParentActivity.Id;
            oStatSub.IdUnit = serviceEP.GetUnitId_ByActivityId(oSubAct.ParentActivity.Id);
            oStatSub.IdPath = oSubAct.Path.Id;
            manager.SaveOrUpdate<SubActivityStatistic>(oStatSub);
            return oStatSub;
        }

        private SubActivityStatistic InitSubActivityStatisticNoTransaction(ModuleLink Link, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            SubActivity oSubAct = manager.Get<SubActivity>(Link.SourceItem.ObjectLongID);
            SubActivityStatistic oStatSub = new SubActivityStatistic();
            oStatSub.SubActivity = oSubAct;
           
     //     oStatSub.ParentStat = GetOrInitActivityStatisticNoTransaction(oStatSub.SubActivity.ParentActivity.Id, participantId,evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

            DateTime CurrentTime = DateTime.Now;
            oStatSub.Person = manager.GetLitePerson(participantId);
            oStatSub.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, CurrentTime);
            oStatSub.StartDate = CurrentTime;
            oStatSub.IdActivity = oSubAct.ParentActivity.Id;
            oStatSub.IdUnit = serviceEP.GetUnitId_ByActivityId(oSubAct.ParentActivity.Id);
            oStatSub.IdPath = oSubAct.Path.Id;
            manager.SaveOrUpdate<SubActivityStatistic>(oStatSub);
            return oStatSub;
        }

        public UnitStatistic GetUnitStatistic(long UnitId, int participantId, DateTime viewStatBefore)
        {
            IList<UnitStatistic> status = (from stat in manager.GetIQ<UnitStatistic>()
                                           where stat.Unit.Id == UnitId && stat.Person.Id == participantId && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
                                           orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                           select stat).Take(1).ToList();
            if (status.Count == 1)
            {
                return status[0];
            }
            return null;
        }

        private UnitStatistic InitUnitStatisticNoTransaction(long UnitId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            Unit oUnit = manager.Get<Unit>(UnitId);
            UnitStatistic oStat = new UnitStatistic();
            oStat.Unit = oUnit;
         //   oStat.ParentStat = GetOrInitPathStatisticNoTransaction(oStat.Unit.ParentPath.Id, participantId,evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            DateTime CurrentTime = DateTime.Now;
            oStat.Person = manager.GetLitePerson(participantId);
            oStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, CurrentTime);
            oStat.StartDate = CurrentTime;
            oStat.IdPath = oUnit.ParentPath.Id;
            manager.SaveOrUpdate<UnitStatistic>(oStat);
            return oStat;
        }

        private UnitStatistic InitUnitStatisticNoTrasaction(long unitId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            Unit oUnit = manager.Get<Unit>(unitId);
            UnitStatistic oStat = new UnitStatistic();
            oStat.Unit = oUnit;
            // oStat.ParentStat = GetOrInitUnitStatisticNoTransaction(oStat.Activity.ParentUnit.Id, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            DateTime CurrentTime = DateTime.Now;
            oStat.Person = manager.GetLitePerson(participantId);
            oStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, CurrentTime);
            oStat.StartDate = CurrentTime;
            oStat.IdPath = oUnit.ParentPath.Id;
            manager.SaveOrUpdate<UnitStatistic>(oStat);           
            return oStat;
        }

        private ActivityStatistic InitActStatisticNoTrasaction(long ActivityId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            Activity oAct = manager.Get<Activity>(ActivityId); 
            ActivityStatistic oStat = new ActivityStatistic();
            oStat.Activity = oAct;
           // oStat.ParentStat = GetOrInitUnitStatisticNoTransaction(oStat.Activity.ParentUnit.Id, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
            DateTime CurrentTime = DateTime.Now;
            oStat.Person = manager.GetLitePerson(participantId);
            oStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, CurrentTime);
            oStat.StartDate = CurrentTime;
            oStat.IdUnit = oAct.ParentUnit.Id;
            oStat.IdPath = oAct.Path.Id;
            manager.SaveOrUpdate<ActivityStatistic>(oStat);
            return oStat;
        }

        private PathStatistic InitPathStatisticNoTransaction(long PathId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        {
            PathStatistic oStat = new PathStatistic();
            oStat.Path = manager.Get<Path>(PathId);
            DateTime CurrentTime = DateTime.Now;
            oStat.Person = manager.GetLitePerson(participantId);
            oStat.CreateMetaInfo(evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, CurrentTime);
            oStat.StartDate = CurrentTime;
            manager.SaveOrUpdate<PathStatistic>(oStat);
            return oStat;
        }




        public PathStatistic GetPathStatistic(long PathId, int participantId, DateTime viewStatBefore)
        {
            IList<PathStatistic> ListOfStat = (from stat in manager.GetIQ<PathStatistic>()
                                               where stat.Path.Id == PathId && stat.Person.Id == participantId && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
                                               orderby stat.CreatedOn descending, stat.Id descending //TODO: CHECK CREATEDON
                                               select stat).Take(1).ToList();

            if (ListOfStat.Count == 1)
            {
                return ListOfStat[0];
            }
            return null;
                      
        }

        #endregion


        #region convert stat

        private List<long> getInteresteSubActId(long pathId)
        {
            return (from subAct in manager.GetIQ<SubActivity>()
                    where subAct.Path.Id == pathId && subAct.Deleted == BaseStatusDeleted.None
                    select subAct.Id).ToList();
        
        }

        /// <summary>
        /// COMMENTATO DA FRA IL 30/09/2015
        /// </summary>
        /// <param name="commId"></param>
        /// <param name="pathId"></param>
        /// <param name="userId"></param>
        /// <param name="adminID"></param>
        /// <param name="adminIPaddress"></param>
        /// <param name="adminProxyIPaddress"></param>
        //public void ConvertStat(int commId, long pathId, int userId,int adminID,string adminIPaddress, string adminProxyIPaddress)
        //{

        //    List<OldStat> statToConvert = new List<OldStat>();
        //    if (pathId > 0)
        //    {
        //        IList<long> subActsId = getInteresteSubActId(pathId);

        //        if (userId > 0)
        //        {
        //            statToConvert = (from stat in manager.GetIQ<OldStat>()
        //                             where subActsId.Contains(stat.SubActivity.Id) && stat.Person.Id == userId
        //                             select stat).ToList();
        //        }
        //        else
        //        {
        //            statToConvert = (from stat in manager.GetIQ<OldStat>()
        //                             where subActsId.Contains(stat.SubActivity.Id)
        //                             select stat).ToList();               
        //        }
        //    }
        //    List<OldStat> statnesadkjadskj = statToConvert.ToList();
        //    int b=14, a = 0;
        //    foreach (OldStat item in statnesadkjadskj)
        //    {
        //        try
        //        {
        //            manager.BeginTransaction();
        //            if (a == b)
        //            {
        //                Console.WriteLine("ddd");
        //            }
        //          //  ConvertSubActAuto_Insert(item.SubActivity.Id, item.Person.Id, adminID, adminIPaddress, adminProxyIPaddress, item.Completion, item.Mark, CheckStatusStatistic(item.Status, StatusStatistic.Started), CheckStatusStatistic(item.Status, StatusStatistic.Completed), CheckStatusStatistic(item.Status, StatusStatistic.Passed));
        //            UpdateSubActStatAuto_Insert(item.SubActivity.Id, item.Person.Id, 0, adminID, adminIPaddress, adminProxyIPaddress, item.Completion, CheckStatusStatistic(item.Status, StatusStatistic.Started), CheckStatusStatistic(item.Status, StatusStatistic.Completed));

        //        //    manager.DeleteGeneric(item);
                  
        //            a++;
                    
        //            manager.Commit();

        //            manager.Flush();
        //        }
        //        catch (Exception)
        //        {
        //            manager.RollBack(); 
        //            throw;
        //        }
        //    }
                
        //}

        #region "old"
        /// OLD 
        //private void ConvertSubActAuto_Insert(long subActId, int participantId,  int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, Int64 completion, Int16 Mark, bool isStarted, bool isCompleted, bool isPassed)
        //{

        //    /// LA sub sopra deve esse
        //    SubActivityStatistic oSubActStat = GetOrInitSubActStat_Insert(subActId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

        //    //NOTE: aggiornare prima la completion dello Status Completed
        //    ModifyStatField subActFieldChanged = ModifyStatField.None;
        //    Status participantStatus = serviceEP.SubActivityStatus(oSubActStat.SubActivity.Id, participantId, 0, false);

        //    //SubActivityStatistic insertStat = InitSubActivityStatisticNoTransaction(subActId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

        //    if (isStarted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        subActFieldChanged |= ModifyStatField.StartedStat;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    if (isCompleted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
        //    {
        //        subActFieldChanged |= ModifyStatField.CompletedAdd;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.CompletedPassed);
        //        oSubActStat.EndDate = DateTime.Now;
        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            subActFieldChanged |= ModifyStatField.ComplPassMandatoryAdd;
        //        }

        //    }
        //    else if (!isCompleted && CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed))
        //    {

        //        subActFieldChanged |= ModifyStatField.CompletedRem;
        //        oSubActStat.Status -= StatusStatistic.CompletedPassed;
        //        oSubActStat.EndDate = DateTime.Now;
        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            subActFieldChanged |= ModifyStatField.ComplPassMandatoryRem;
        //        }
        //    }

        //    if (oSubActStat.Completion != completion)
        //    {
        //        subActFieldChanged |= ModifyStatField.Completion;
        //        oSubActStat.Completion = completion;
        //    }


        //        subActFieldChanged |= ModifyStatField.Convert;
           
        //        DateTime creationDate = (DateTime)oSubActStat.CreatedOn;
        //        CreateSubActStatMetaInfo(oSubActStat, manager.GetPerson(evaluatorId), creationDate, evaluatorIPaddress, evaluatorProxyIPaddress);
                
                
        //        manager.SaveOrUpdate<SubActivityStatistic>(oSubActStat);

        //        UpdateActStatAuto_Insert(oSubActStat.IdActivity, participantId, 0, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, subActFieldChanged, creationDate);
            
        //}

         #endregion

        #endregion

        public dtoStatusCompletion GetStatusCompletion_bySubActivityStatistic(long SubActivityId, int UserId, DateTime viewStatBefore)
        {


        //    IList<SubActivityStatistic> StatList = manager.GetAll<SubActivityStatistic>(s => s.SubActivity.Id == SubActivityId && s.SubActivity.Deleted == BaseStatusDeleted.None && s.Person.Id == UserId && s.Deleted == BaseStatusDeleted.None).ToList();

            //IList<dtoStatusCompletion> StatList = (from stat in manager.GetIQ<SubActivityStatistic>()
            //                                        where stat.SubActivity.Id == SubActivityId && stat.Person.Id == UserId && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
            //                                        orderby stat.CreatedOn descending
            //                                        select new dtoStatusCompletion() { Status=GetMainStatusStatistic( stat.Status),Completion=stat.Completion}).Take(1).ToList();


                var StatList = (from stat in manager.GetIQ<SubActivityStatistic>()
                                                       where stat.SubActivity.Id == SubActivityId && stat.Person.Id == UserId && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
                                orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                                       select new{completion=stat.Completion, status=stat.Status}).Take(1).ToList();



                if (StatList.Count == 1)
                {
                    return new dtoStatusCompletion(GetMainStatusStatistic(StatList[0].status), StatList[0].completion);
                    //return StatList[0];
                }
          

            return new dtoStatusCompletion() { Status = StatusStatistic.None, Completion = (Int64)0 };
        }



        #region Update ParentStat

        //public void UpdateParentSubActivityStatisticNoTransaction(long subActivityId, int communityId, int updaterID, string updaterIPaddress, string updaterProxyIPaddress)
        //{
        //    int croleParticipant;
        //    long activityId = manager.Get<SubActivity>(subActivityId).ParentActivity.Id;
        //    IList<ActivityStatistic> ActivityStatistics = manager.GetAll<ActivityStatistic>(stat => stat.Activity.Id == activityId);
        //    foreach (ActivityStatistic stat in ActivityStatistics)
        //    {
        //        croleParticipant = manager.GetActiveSubscriptionRoleId(stat.Person.Id, communityId);
        //        UpdateStatActivityCompletionAndStatusNoTransaction(stat, stat.Person.Id, croleParticipant, updaterID, updaterProxyIPaddress, updaterIPaddress);
        //    }
        //}

        //public void UpdateParentActivityStatisticNoTransaction(long ActivityId, int communityId, int updaterID, string updaterIPaddress, string updaterProxyIPaddress)
        //{
        //    int croleParticipant;
        //    long UnitId = serviceEP.GetUnitId_ByActivityId(ActivityId);
        //    IList<UnitStatistic> UnitStatistics = manager.GetAll<UnitStatistic>(stat => stat.Unit.Id == UnitId);

        //    foreach (UnitStatistic stat in UnitStatistics)
        //    {
        //        croleParticipant = manager.GetActiveSubscriptionRoleId(stat.Person.Id, communityId);
        //        UpdateStatUnitCompletionAndStatusNoTransaction(stat, stat.Person.Id, croleParticipant, updaterID, updaterProxyIPaddress, updaterIPaddress);
        //    }
        //}

        //public void UpdateParentUnitStatisticNoTransaction(long unitId, int communityId, int updaterID, string updaterIPaddress, string updaterProxyIPaddress)
        //{
        //    int croleParticipant;
        //    long pathId = serviceEP.GetPathId_ByUnitId(unitId);
        //    IList<PathStatistic> PathStatistics = manager.GetAll<PathStatistic>(stat => stat.Path.Id == pathId);

        //    foreach (PathStatistic stat in PathStatistics)
        //    {
        //        croleParticipant = manager.GetActiveSubscriptionRoleId(stat.Person.Id, communityId);
        //        UpdateStatPathCompletionAndStatusNoTransaction(stat, stat.Person.Id, croleParticipant, updaterID, updaterProxyIPaddress, updaterIPaddress);
        //    }
        //}

        #endregion

        #region update statistics Da rivedere e riattivare se avverrà l'aggiornamento delle statistiche dovute alla modifica di path già iniziato

        //public void UpdatePathStatisticNoTransaction(long pathId, int communityId, int updaterID, string updaterIPaddress, string updaterProxyIPaddress)
        //{
        //    int croleParticipant;
        //    IList<PathStatistic> PathStatistics = manager.GetAll<PathStatistic>(stat => stat.Path.Id == pathId);

        //    foreach (PathStatistic stat in PathStatistics)
        //    {
        //        croleParticipant = manager.GetActiveSubscriptionRoleId(stat.Person.Id, communityId);
        //        UpdateStatPathCompletionAndStatusNoTransaction(stat, stat.Person.Id, croleParticipant, updaterID, updaterProxyIPaddress, updaterIPaddress);
        //    }
        //}

        //public void UpdateUnitStatisticNoTransaction(long unitId, int communityId, int updaterID, string updaterIPaddress, string updaterProxyIPaddress)
        //{
        //    int croleParticipant;
        //    IList<UnitStatistic> UnitStatistics = manager.GetAll<UnitStatistic>(stat => stat.Unit.Id == unitId);

        //    foreach (UnitStatistic stat in UnitStatistics)
        //    {
        //        croleParticipant = manager.GetActiveSubscriptionRoleId(stat.Person.Id, communityId);
        //        UpdateStatUnitCompletionAndStatusNoTransaction(stat, stat.Person.Id, croleParticipant, updaterID, updaterProxyIPaddress, updaterIPaddress);
        //    }
        //}

        //public void UpdateActivityStatisticNoTransaction(long activityId, int communityId, int updaterID, string updaterIPaddress, string updaterProxyIPaddress)
        //{
        //    int croleParticipant;
        //    IList<ActivityStatistic> ActivityStatistics = manager.GetAll<ActivityStatistic>(stat => stat.Activity.Id == activityId);

        //    foreach (ActivityStatistic stat in ActivityStatistics)
        //    {
        //        croleParticipant = manager.GetActiveSubscriptionRoleId(stat.Person.Id, communityId);
        //        UpdateStatActivityCompletionAndStatusNoTransaction(stat, stat.Person.Id, croleParticipant, updaterID, updaterProxyIPaddress, updaterIPaddress);
        //    }
        //}
        #endregion

                #region get statistic with not personalized min completion

        //public IList<ActivityStatistic> GetActivityStatisticNotPersonalized(long activityId, IList<int> PersonsIdWithPersonalMinCompletion, Int16 Min_MinCompletion, Int16 Max_MinCompletion)
        //{
        //  //  return manager.GetAll<ActivityStatistic>(stat=>stat.Activity.Id==activityId && Min_MinCompletion <= stat.Completion && stat.Completion >= Max_MinCompletion && ! PersonsIdWithPersonalMinCompletion.Contains(stat.Person.Id)  );

        //    Int16 subActivityCount = serviceEP.GetActiveSubActivitiesCount_NotPersonalizeProperties(activityId);
        //    Int16 subActivityMandatoryCount = serviceEP.GetActiveMandatorySubActivitiesCount_NotPersonalizeProperties(activityId);
        //    return manager.GetAll<ActivityStatistic>(stat => stat.Activity.Id == activityId && stat.MandatoryPassedSubActivityCount == subActivityMandatoryCount
        //        && Min_MinCompletion <= (subActivityCount / stat.PassedSubActivityCount) && (subActivityCount / stat.PassedSubActivityCount) >= Max_MinCompletion
        //        && !PersonsIdWithPersonalMinCompletion.Contains(stat.Person.Id));
        //}     

        //public IList<UnitStatistic> GetUnitStatisticNotPersonalized(long UnitId, IList<int> PersonsIdWithPersonalMinCompletion, Int16 Min_MinCompletion, Int16 Max_MinCompletion)
        //{
        //    Int16 activityCount = serviceEP.GetActiveActivitiesCount_NotPersonalizeProperties(UnitId); 
        //    Int16 activityMandatoryCount=serviceEP.GetActiveMandatoryActivitiesCount_NotPersonalizeProperties(UnitId);
        //    return manager.GetAll<UnitStatistic>(stat => stat.Unit.Id == UnitId && stat.MandatoryPassedActivityCount==activityMandatoryCount
        //        && Min_MinCompletion <= (activityCount / stat.PassedActivityCount) && (activityCount / stat.PassedActivityCount) >= Max_MinCompletion
        //        && !PersonsIdWithPersonalMinCompletion.Contains(stat.Person.Id));
        //}

        //public IList<PathStatistic> GetPathStatisticNotPersonalized(long pathId, IList<int> PersonsIdWithPersonalMinCompletion, Int16 Min_MinCompletion, Int16 Max_MinCompletion)
        //{
        //    Int16 unitCount = serviceEP.GetActiveUnitsCount_NotPersonalizeProperties(pathId);
        //    Int16 unitMandatoryCount = serviceEP.GetActiveMandatoryUnitsCount_NotPersonalizeProperties(pathId);
        //    return manager.GetAll<PathStatistic>(stat => stat.Path.Id == pathId && stat.MandatoryPassedUnitCount == unitMandatoryCount
        //        && Min_MinCompletion <= (unitCount / stat.PassedUnitCount) && (unitCount / stat.PassedUnitCount) >= Max_MinCompletion
        //        && !PersonsIdWithPersonalMinCompletion.Contains(stat.Person.Id));
        //}

        #endregion

        public bool CheckStatusStatistic(StatusStatistic Actual, StatusStatistic Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public bool CheckFieldChanged(ModifyStatField Actual, ModifyStatField Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public bool UsersStartedPath(long pathId)
        {
            return manager.GetAll<PathStatistic>(item => item.Path.Id == pathId && item.Path.Deleted == BaseStatusDeleted.None && item.Deleted == BaseStatusDeleted.None && item.Status >= StatusStatistic.Started).Count > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <param name="excludedPerson">Id of person to exclude from count</param>
        /// <returns></returns>
        public bool UsersStartedPath(long pathId, long excludedPersonId)
        {
            return manager.GetAll<PathStatistic>(item => item.Path.Id == pathId && item.Path.Deleted == BaseStatusDeleted.None && item.Deleted == BaseStatusDeleted.None && item.Status >= StatusStatistic.Started && item.Person.Id != excludedPersonId).Any();
        }



        public Dictionary<long,Boolean> UsersStartedPath(List<long> idPaths)
        {
            Dictionary<long, Boolean> results = (from s in manager.GetIQ<PathStatistic>()
                    where s.Deleted == BaseStatusDeleted.None && s.Status >= StatusStatistic.Started && s.Path != null && idPaths.Contains(s.Path.Id)
                    select s.Path.Id).Distinct().ToList().ToDictionary(i => i, s=> true);

            idPaths.Where(i => !results.ContainsKey(i)).ToList().ForEach(i => results.Add(i, false));
            return results;
        }


        public Activity GetActivity_ByLastModifyStatistic(long PathId, int UserId, int croleId, bool isAutoEp)
        {
            IList<Activity> act;
            if (isAutoEp)
            {
                IList<ActivityStatistic> actStat = GetActiveActStat_ByPathId_Insert(PathId, UserId);
                
                act = (from stat in actStat
                       where (CheckStatusStatistic(stat.Status, StatusStatistic.Started) || CheckStatusStatistic(stat.Status, StatusStatistic.Started | StatusStatistic.Browsed))
                            && (!CheckStatusStatistic(stat.Status, StatusStatistic.Completed) || !CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed))
                       orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                       select stat.Activity).Take(1).ToList();
         
          
                //act= (from stat in manager.GetAll<ActivityStatistic>(stat => stat.Person.Id == UserId && stat.Activity.Path.Id == PathId && stat.Activity.Deleted == BaseStatusDeleted.None && stat.Activity.Path.Deleted == BaseStatusDeleted.None).ToList()
                //        where CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed)
                //        orderby stat.CreatedOn descending
                //        select stat.Activity).Take(1).ToList();
            }
            else
            {
                act= (from stat in manager.GetAll<ActivityStatistic>(stat => stat.Person.Id == UserId && stat.IdPath == PathId  ).ToList()
                        where CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed)
                        orderby stat.ModifiedOn descending
                      select stat.Activity).Take(1).ToList();
            }

            if (act.Count == 1)
            {
                return act[0];
            }
            return null;
        }

        public IList<Activity> GetActivities_ByLastModifyStatistic(long PathId, int UserId, int croleId, bool isAutoEp)
        {
            IList<Activity> act;
            if (isAutoEp)
            {
                IList<ActivityStatistic> actStat = GetActiveActStat_ByPathId_Insert(PathId, UserId);

                act = (from stat in actStat
                       where (CheckStatusStatistic(stat.Status, StatusStatistic.Started) || CheckStatusStatistic(stat.Status, StatusStatistic.Started | StatusStatistic.Browsed))
                            && (!CheckStatusStatistic(stat.Status, StatusStatistic.Completed) || !CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed))
                       orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                       select stat.Activity).ToList();


                //act= (from stat in manager.GetAll<ActivityStatistic>(stat => stat.Person.Id == UserId && stat.Activity.Path.Id == PathId && stat.Activity.Deleted == BaseStatusDeleted.None && stat.Activity.Path.Deleted == BaseStatusDeleted.None).ToList()
                //        where CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed)
                //        orderby stat.CreatedOn descending
                //        select stat.Activity).Take(1).ToList();
            }
            else
            {
                act = (from stat in manager.GetAll<ActivityStatistic>(stat => stat.Person.Id == UserId && stat.IdPath == PathId).ToList()
                       where CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed)
                       orderby stat.ModifiedOn descending, stat.Completion descending, stat.Status descending
                       select stat.Activity).ToList();
            }

            //if (act.Count == 1)
            //{
            //    return act[0];
            //}

            return act;
        }

        public dtoStatWithWeight GetPassedCompletedWeight_byUnit(long pathId, int userId, DateTime viewStatBefore)
        {

            dtoStatWithWeight dtoStat = new dtoStatWithWeight();

            Path oPath = manager.Get<Path>(pathId);
            dtoStat.MinCompletion = oPath.MinCompletion;

            PathStatistic oPs = GetPathStat_Insert(pathId, userId, viewStatBefore);
            if (oPs != null)
            {
                dtoStat.Completion = oPs.Completion;
            }
            else
            {
                dtoStat.Completion = 0;
            }

            dtoStat.UserTotWeight = GetActiveUnitStat_ByPathId_Insert(pathId, userId,viewStatBefore).Where(zz => CheckStatusStatistic(zz.Status, StatusStatistic.CompletedPassed)).Sum(zz => zz.Unit.Weight);

            if (oPath.Weight != 0)
            {
                dtoStat.Completion = (Int64)(dtoStat.UserTotWeight * 100 / oPath.Weight);
            }

            return dtoStat;
        }


        public dtoStatWithWeight GetPassedCompletedWeight_byActivity(long pathId, int userId, DateTime viewStatBefore)
        {

            dtoStatWithWeight dtoStat = new dtoStatWithWeight();

            Path oPath = manager.Get<Path>(pathId);
            dtoStat.MinCompletion = oPath.MinCompletion;

            PathStatistic oPs = GetPathStat_Insert(pathId, userId, viewStatBefore);
            if (oPs != null)
            {
                dtoStat.Completion = oPs.Completion;
            }
            else
            {
                dtoStat.Completion = 0;
            }

            dtoStat.UserTotWeight = GetActiveActStat_ByPathId_Insert(pathId, userId,viewStatBefore).Where(zz => CheckStatusStatistic(zz.Status, StatusStatistic.CompletedPassed)).Sum(zz => zz.Activity.Weight);
                      
            if (oPath.Weight != 0)
            {
                dtoStat.Completion = (Int64)(dtoStat.UserTotWeight * 100 / oPath.Weight);
            }

            return dtoStat;
        }

        public Int64 GetActivityCompletion_Insert(long idActivity, long idUser)
        {
           
            Int64 retVal = 0;
            ActivityStatistic oStat= GetActStat_Insert(idActivity, (Int32)idUser);               
            if (oStat != null)
                   retVal = oStat.Completion;


            if (serviceEP.CheckEpType(manager.Get<Activity>(idActivity).Path.EPType, EPType.Auto))
            {
                Int64 weight = manager.Get<Activity>(idActivity).Weight;
                retVal = (Int64)(retVal * 100 /  (weight==0 ? 1:weight));
            }
            return retVal;
        }

        public string ToShortDate(DateTime? d)
        {
            if (d.HasValue)
            {
                return ToShortDate(d.Value);
            }
            else
            {
                return "";
            }
        }

        public string ToShortDate(DateTime d)
        {
            return String.Format("{0} {1}", d.ToShortDateString(), d.ToShortTimeString());
        }


        //+EP[Rob]
        public Int64 GetSubactivityIdByModuleLink(Int64 idLink, SubActivityType type)
        {
            try
            {
                var q = (from item in manager.GetIQ<SubActivity>() where item.ModuleLink.Id == idLink && item.ContentType == type select item.Id).FirstOrDefault();

                return q;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        //+EP[Rob]
        public StatusStatistic GetSubactivityStatusByModuleLink(Int64 idLink, SubActivityType type, Int32 idUser)
        {
            try
            {
                Int64 idSubActivity = GetSubactivityIdByModuleLink(idLink, type);

                if (idSubActivity > 0)
                {

                    SubActivityStatistic sas = GetSubAcStat_Insert(idSubActivity, (Int32)idUser);

                    return sas.Status;
                }
                else
                {
                    return StatusStatistic.None;
                }
            }
            catch (Exception ex)
            {
                return StatusStatistic.None;
            }
        }


        public ILookup<StatusStatistic, ActivityStatistic> GetStatCount_ByUser(Int64 idPath, Int32 idUser, DateTime viewStatBefore)
        {
            

            Path p = serviceEP.GetPath(idPath);

            var list = p.UnitList.SelectMany(x=>x.ActivityList).Select(x=>x.Id).ToList();

            IList<ActivityStatistic> actStat = new List<ActivityStatistic>();

            foreach (var item in list)
            {
                actStat = actStat.Concat((from stat in manager.GetIQ<ActivityStatistic>()
                                          where stat.IdPath == idPath &&
                                           item == stat.Activity.Id &&
                                           stat.Person.Id == idUser && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                          orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                          select stat).Take(1).ToList()).ToList();
            }



            return actStat.ToLookup(x => x.Status);
            //Dictionary(x => x.Status, g => 1);

            //return null;
        }

        public dtoCertificatesUserStat GetUserCertificatesStat(Int64 idPath, Int32 idUser, DateTime viewStatBefore, DateTime? startdate = null, DateTime? enddate = null)
        {
            dtoCertificatesUserStat dcus = new dtoCertificatesUserStat();

            Path path = manager.Get<Path>(idPath);

            dcus.IdPerson = idUser;
            dcus.Person = manager.GetLitePerson(idUser);

            dcus.PathId = idPath;
            dcus.PathName = path.Name;

            if (path != null)
            {

                Int32 idCommunity = 0;

                if (path.Community != null)
                {
                    idCommunity = path.Community.Id;
                }

                IList<SubActivity> subs = serviceEP.GetSubactiviesByPathIdAndTypes(idPath, SubActivityType.Certificate);

                dcus.Certificates = new List<dtoCertificateUserStat>();

                foreach (var sub in subs)
                {
                    dtoCertificateUserStat dcu = new dtoCertificateUserStat();

                    dcu.CommunityId = idCommunity;
                    dcu.PathId = idPath;
                    dcu.SubActivityId = sub.Id;
                    dcu.ActivityId = sub.ParentActivity.Id;
                    dcu.CertificateId = sub.IdCertificate;
                    dcu.CertificateName = sub.Name;
                    dcu.CertificateVersion = sub.IdCertificateVersion;
                    dcu.PathName = path.Name;


                    dcu.SubActivity = new dtoSubActivity(sub);


                    IList<SubActivityStatistic> saslist = GetActiveSubActStat_Insert(dcu.ActivityId, idUser);

                    var q = (from item in saslist where item.SubActivity.Id == sub.Id select item).FirstOrDefault();

                    if (q != null && (CheckStatusStatistic(q.Status, StatusStatistic.Completed) || CheckStatusStatistic(q.Status, StatusStatistic.CompletedPassed)))
                    {
                        Boolean afterStart = true;
                        Boolean beforeEnd = true;
                        Boolean selected = true;
                        if (startdate != null && startdate.HasValue == true)
                        {
                            afterStart = q.CreatedOn >= startdate.Value;
                        }
                        if (enddate != null && enddate.HasValue == true)
                        {
                            beforeEnd = q.CreatedOn <= enddate.Value;
                        }
                        
                        if (afterStart && beforeEnd && selected)
                        {


                            if (sub.IdCertificate > 0)
                            {
                                //dtoCertificateUsersStat dcus = new dtoCertificateUsersStat();
                                //dcus.Parent = dcs;
                                //dcus.Date = q.CreatedOn.Value;
                                //dcus.Person = q.Person;
                                //dcus.PersonId = q.Person.Id;

                                dcu.CompletedOn = q.CreatedOn.Value;

                                dcus.Certificates.Add(dcu);
                            }
                        }
                    }
                }

            }
            return dcus;
        }

        public IList<dtoCertificateStat> GetCertificatesStat(Int64 idPath, DateTime viewStatBefore, Boolean allStatistics = false, DateTime? startdate = null, DateTime? enddate = null, Int64 idCertificate = -1, Int32 idRole = -1, string user="")
        {
            IList<dtoCertificateStat> list = new List<dtoCertificateStat>();

            Path path = manager.Get<Path>(idPath);

            Int32 idCommunity = 0;

            if (path.Community != null)
            {
                idCommunity = path.Community.Id;
            }

            IList<SubActivity> subs = serviceEP.GetSubactiviesByPathIdAndTypes(idPath, SubActivityType.Certificate);

            var PersonAssignment = (from i in manager.GetIQ<PathPersonAssignment>() where i.IdPath == idPath select i).ToList();
            var RoleAssignment = (from i in manager.GetIQ<PathCRoleAssignment>() where i.IdPath == idPath select i).ToList();

            var roles = (from i in RoleAssignment select i.IdRole).Distinct().ToList();

            IList<Int32> idUsersPA = (from i in PersonAssignment select i.IdPerson).ToList();
            IList<Int32> idUsersRC = (from i in manager.GetIQ<LazySubscription>() where i.IdCommunity == idCommunity && roles.Contains(i.IdRole) select i.IdPerson).ToList();
            IList<Int32> idUsers = idUsersPA.Union(idUsersRC).ToList();

            if (allStatistics)
            {
                IList<Int32> idUsersAS = GetUsersWithStat(idPath, viewStatBefore);
                idUsers = idUsers.Union(idUsersAS).ToList();
            }

            if (idRole > 0)
            {
                IList<Int32> filteredUsers = (from i in manager.GetIQ<LazySubscription>() where i.IdCommunity == idCommunity && i.IdRole == idRole select i.IdPerson).ToList();

                idUsers = (from item in idUsers where filteredUsers.Contains(item) select item).ToList();
            }

            if (idCertificate > 0)
            {
                subs = (from item in subs where item.IdCertificate == idCertificate select item).ToList();
            }

            foreach (var sub in subs)
            {
                dtoCertificateStat dcs = new dtoCertificateStat();

                dcs.CertificateId = sub.IdCertificate;
                dcs.CertificateVersion = sub.IdCertificateVersion;
                dcs.CertificateName = sub.Name;
                dcs.PathId = idPath;
                dcs.SubActivityId = sub.Id;
                dcs.CommunityId = idCommunity;
                dcs.ActivityId = sub.ParentActivity.Id;

                dcs.dtoSubActivity = serviceEP.GetDtoSubActivity(sub.Id);

                dcs.Total = idUsers.Count();

                foreach (var iduser in idUsers)
                {
                    IList<SubActivityStatistic> saslist = GetActiveSubActStat_Insert(dcs.ActivityId, iduser);

                    var q = (from item in saslist where item.SubActivity.Id == sub.Id select item).FirstOrDefault();

                    if (q != null && (CheckStatusStatistic(q.Status, StatusStatistic.Completed) || CheckStatusStatistic(q.Status, StatusStatistic.CompletedPassed)))
                    {
                        Boolean afterStart = true;
                        Boolean beforeEnd = true;
                        Boolean selected = true;
                        if (startdate != null && startdate.HasValue == true)
                        {
                            afterStart = q.CreatedOn >= startdate.Value;
                        }
                        if (enddate != null && enddate.HasValue == true)
                        {
                            beforeEnd = q.CreatedOn <= enddate.Value;
                        }
                        if (!string.IsNullOrEmpty(user))
                        {
                            selected = q.Person.SurnameAndName.ToLower().Contains(user.ToLower());
                        }
                        if (afterStart && beforeEnd && selected )
                        {
                            dcs.Obtained += 1;

                            if (idCertificate > 0)
                            {
                                dtoCertificateUsersStat dcus = new dtoCertificateUsersStat();
                                dcus.Parent = dcs;
                                dcus.Date = q.CreatedOn.Value;
                                dcus.Person = q.Person;
                                dcus.PersonId = q.Person.Id;

                                dcs.Users.Add(dcus);
                            }
                        }
                    }


                }

                list.Add(dcs);
            }

            return list;
        }

        public dtoAllPaths GetPathsCount(DateTime viewStatBefore, Int32 pageindex = 0, Int32 pagesize = 0, string orderby = "", Boolean ascending = true, string deadline = "{0} {1} {2} {3}", string pathname = "", string communityname = "", string startdate = null, string enddate = null, StatusFilter status = StatusFilter.all, IEnumerable<Int32> idcommunities = null)
        {
            dtoAllPaths dap = new dtoAllPaths();

            var q = (from item in manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None select item);

            if (!string.IsNullOrEmpty(pathname))
            {
                q = (from item in q where item.Name.Contains(pathname) select item);
            }

            if (idcommunities != null && idcommunities.Count() > 0)
            {
                q = (from item in q where item.Community != null && idcommunities.Contains(item.Community.Id) select item);
            }

            if (!string.IsNullOrEmpty(communityname))
            {
                q = (from item in q where item.Community != null && item.Community.Name.Contains(communityname) select item);
            }

            if (!string.IsNullOrEmpty(startdate))
            {
                q = (from item in q where item.StartDate >= DateTime.Parse(startdate) select item);
            }

            if (!string.IsNullOrEmpty(enddate))
            {
                q = (from item in q where item.EndDate <= DateTime.Parse(enddate) select item);
            }

            IList<Path> AllPath = q.ToList<Path>();

            switch (status)
            {
                case StatusFilter.all:
                    break;
                case StatusFilter.unlocked:
                    AllPath = (from item in AllPath where serviceEP.CheckStatus(item.Status, Status.NotLocked) select item).ToList();
                    break;
                case StatusFilter.locked:
                    AllPath = (from item in AllPath where serviceEP.CheckStatus(item.Status, Status.Locked) select item).ToList();
                    break;
                default:
                    break;
            }
            dap.Paths = new List<dtoPathInfo>();

            dap.Started = 0;
            dap.NotStarted = 0;
            dap.Completed = 0;

            foreach (var path in AllPath)
            {
                dtoPathInfo dpi = new dtoPathInfo();
                dpi.IdPath = path.Id;
                dpi.PathName = path.Name;
                dpi.PathLocked = serviceEP.CheckStatus(path.Status, Status.Locked);
                dpi.PathType = path.EPType;
                dpi.IsMooc = path.IsMooc;
                if (path.FloatingDeadlines == true)
                {
                    dpi.Deadline = "floating";
                }
                else
                {

                    //if (currentpath.StartDate.HasValue)
                    //{
                    //    path.Deadline = currentpath.StartDate.Value.ToShortDateString();
                    //}

                    //if (currentpath.EndDate.HasValue)
                    //{
                    //    path.EndDate = currentpath.EndDate.Value.ToShortDateString();
                    //}

                    //path.Deadline = path.EndDate;

                    dpi.Deadline = String.Format(deadline, ToShortDate(path.StartDate), ToShortDate(path.EndDate), (path.StartDate.HasValue) ? "" : "empty", (path.EndDate.HasValue) ? "" : "empty");
                }
                
                //dpi.CanManage = serviceEP.CheckRoleEpNotStrict((from ass in PersonAssignment where ass.Path.Id == path.IdPath && ass.Person.Id == path.IdPerson select ass.RoleEP).FirstOrDefault(), RoleEP.Manager);
                //dpi.CanManage |= serviceEP.CheckRoleEpNotStrict((from ass in CurrentCommunitiesRolesAssignment where ass.Path != null && ass.Path.Id == path.IdPath && ass.Community != null && (from i in allSubscription where i.IdCommunity == ass.Community.Id select i.IdRole).Contains(ass.Role.Id) select ass.RoleEP).FirstOrDefault(), RoleEP.Manager);

                //dpi.CanStat = serviceEP.CheckRoleEpNotStrict((from ass in PersonAssignment where ass.Path.Id == path.IdPath && ass.Person.Id == path.IdPerson select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer);
                //dpi.CanStat |= serviceEP.CheckRoleEpNotStrict((from ass in CurrentCommunitiesRolesAssignment where ass.Path != null && ass.Path.Id == path.IdPath && ass.Community != null && (from i in allSubscription where i.IdCommunity == ass.Community.Id select i.IdRole).Contains(ass.Role.Id) select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer);

                if (path.Community != null)
                {


                    var globalstat = GetGlobalEpStatsForced(path.Id, path.Community.Id, serviceEP.Context.UserContext.CurrentUserID,  false, viewStatBefore);

                    dpi.IdCommunity = path.Community.Id;
                    dpi.CommunityName = manager.GetCommunityName(dpi.IdCommunity);

                    dpi.Started = Math.Max((long)0, globalstat.startedCount);
                    dpi.NotStarted = Math.Max((long)0, globalstat.notStartedCount);
                    dpi.Completed = Math.Max((long)0, Math.Max(globalstat.completedCount, globalstat.compPassedCount));                    

                    dap.Started += dpi.Started;
                    dap.Completed += dpi.Completed;
                    dap.NotStarted += dpi.NotStarted;


                    dap.Paths.Add(dpi);
                }
            }
            
            dap.Total = dap.Paths.Count();

            if (!string.IsNullOrEmpty(orderby))
            {
                
                switch (orderby)
                {
                    case "path":
                        if (ascending)
                        {
                            dap.Paths = dap.Paths.OrderBy(x => x.PathName).ToList();
                        }
                        else
                        {
                            dap.Paths = dap.Paths.OrderByDescending(x => x.PathName).ToList();
                        }
                        break;
                    case "timerange":
                        if (ascending)
                        {
                            dap.Paths = dap.Paths.OrderBy(x => x.Deadline ).ToList();
                        }
                        else
                        {
                            dap.Paths = dap.Paths.OrderByDescending(x => x.Deadline).ToList();
                        }
                        break;
                    default:
                        break;
                }


                
            }

            if (pagesize > 0)
            {
                dap.Paths = (from item in dap.Paths select item).Skip(pageindex * pagesize).Take(pagesize).ToList();
            }
            
            return dap;
        }

        public IList<dtoPathUsers> GetPathsUsersStat(DateTime viewStatBefore, Boolean allStatistics = false, Boolean onlyCertificateQuiz = false, DateTime? viewStatAfter = null)
        {
            //IList<Int32> communities = (from item in manager.GetIQ<Community>() select item.Id).ToList();

            //IList<dtoPathUsers> list = new List<dtoPathUsers>();

            //foreach (var id in communities)
            //{
            //    list = list.Concat(GetPathsUsersStat(id, viewStatBefore, allStatistics, onlyCertificateQuiz, viewStatAfter)).ToList();
            //}

            //return list;

            

            IList <Int64> idPaths = (from item in manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Status != Status.None && item.Community != null orderby item.Community.Id, item.Id  select item.Id).ToList();

            IList<dtoPathUsers> list = new List<dtoPathUsers>();

            foreach (var id in idPaths)
            {
                list.Add(GetPathUsersStat(id, viewStatBefore, 0, 0, allStatistics, onlyCertificateQuiz, viewStatAfter));
            }

            return list;
        }

        public IList<dtoPathUsers> GetPathsUsersStat(IEnumerable<Int32> idCommunities, DateTime viewStatBefore, Boolean allStatistics = false, Boolean onlyCertificateQuiz = false, DateTime? viewStatAfter = null)
        {
            IList<dtoPathUsers> list = new List<dtoPathUsers>();

            foreach (var id in idCommunities)
            {
                list = list.Concat(GetPathsUsersStat(id, viewStatBefore, allStatistics, onlyCertificateQuiz, viewStatAfter)).ToList();
            }

            return list;
        }

        public IList<dtoPathUsers> GetPathsUsersStat(Int32 idCommunity, DateTime viewStatBefore, Boolean allStatistics = false, Boolean onlyCertificateQuiz = false, DateTime? viewStatAfter = null)
        {
            IList<dtoPathUsers> dpu = new List<dtoPathUsers>();

            IList<Int64> idPaths = (from item in manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Status != Status.None && item.Community != null && item.Community.Id == idCommunity orderby item.Id select item.Id).ToList();

            foreach (var idPath in idPaths)
            {
                dpu.Add(GetPathUsersStat(idPath, viewStatBefore, 0, 0, allStatistics, onlyCertificateQuiz, viewStatAfter));
            }

            return dpu;
        }

        public dtoPathUsers GetPathUsersStat(Int64 idPath, DateTime viewStatBefore, Int32 pageindex = 0, Int32 pagesize = 0, Boolean allStatistics = false, Boolean onlyCertificateQuiz=false, DateTime? viewStatAfter=null)
        {
            dtoPathUsers dpu = new dtoPathUsers();

            Path path = manager.Get<Path>(idPath);
            dpu.PathInfo = new dtoFullPathItem(path, Status.None, RoleEP.Participant);
            dpu.PathLocked = (dpu.PathInfo!=null) ? serviceEP.CheckStatus(path.Status, Status.Locked) : false;

            if (path.Community != null)
            {
                dpu.IdCommunity = path.Community.Id;
                // ?????????
                dpu.CommunityName = manager.GetCommunityName(dpu.IdCommunity);
                dpu.IdOrganization = path.Community.IdOrganization;
                dpu.OrganizationName = manager.GetOrganizationName(path.Community.IdOrganization);
            }
            else
            {
                dpu.IdCommunity = 0;
                dpu.IdOrganization = 0;
                dpu.OrganizationName = "";
            }

            //var PersonAssignment = (from i in manager.GetIQ<PathPersonAssignment>() where AllPathIds.Contains(i.Path.Id) && i.Person.Id == userId select i).ToList();

            //var CurrentCommunitiesRolesAssignment = (from i in manager.GetIQ<PathCRoleAssignment>() where AllPathIds.Contains(i.Path.Id) select i).ToList();

            var PersonAssignment = (from i in manager.GetIQ<PathPersonAssignment>() where i.IdPath == idPath select i).ToList();
            var RoleAssignment = (from i in manager.GetIQ<PathCRoleAssignment>() where i.IdPath == idPath select i).ToList();

            var roles = (from i in RoleAssignment select i.IdRole).Distinct().ToList();

            IList<Int32> idUsersPA = (from i in PersonAssignment select i.IdPerson).ToList();
            IList<Int32> idUsersRC = (from i in manager.GetIQ<LazySubscription>() where i.IdCommunity == dpu.IdCommunity && roles.Contains(i.IdRole) select i.IdPerson).ToList();
            IList<Int32> idUsers = idUsersPA.Union(idUsersRC).ToList();

            if (allStatistics)
            {
                IList<Int32> idUsersAS = GetUsersWithStat(idPath, viewStatBefore);
                idUsers = idUsers.Union(idUsersAS).ToList();
            }

            dpu.Questionnaires = new List<dtoSubActivityQuestionnaire>();
            IList<SubActivity> subs = serviceEP.GetSubactiviesByPathIdAndTypes(path.Id, SubActivityType.Quiz);
            foreach (var sub in subs.Where(s=> s.IdObjectLong>0).ToList())
            {
                if (!onlyCertificateQuiz)
                    dpu.Questionnaires.Add(new dtoSubActivityQuestionnaire() { IdQuestionnaire = sub.IdObjectLong , IdSubActivity= sub.Id});
                else
                {
                    // VISIBILE SI, MANDATORY NON OBBLIGATORIO PER IL CERTIFICATO o SI ? 
                    // VERIFICARE DOMANI INSIEME
                    Boolean isCertificate = (from item in manager.GetIQ<SubActivityLink>()
                                             where item.IdObject == sub.IdObjectLong && item.ContentType == SubActivityLinkType.quiz && item.Deleted== BaseStatusDeleted.None &&
                                             item.SubActivity.Path.Id== idPath && item.Visible
                                             select item).Any();

                    // (item.Visible || item.Mandatory) select item).Any();
                    if (isCertificate)
                        dpu.Questionnaires.Add(new dtoSubActivityQuestionnaire() { IdQuestionnaire = sub.IdObjectLong, IdSubActivity = sub.Id });
                }
            }

            dpu.Users = new List<dtoPathUserInfo>();
            IList<litePerson> Users = manager.GetLitePersons(idUsers.ToList()).OrderBy(x => x.SurnameAndName).ToList();

            if (pagesize > 0)
                Users = Users.Skip(pageindex * pagesize).Take(pagesize).ToList();

            foreach (litePerson user in Users)
            {
                dtoPathUserInfo pui = new dtoPathUserInfo();
                pui.User = user;
                PathStatistic Ps = GetPathStat_Insert(path.Id, user.Id, viewStatBefore, viewStatAfter );

                PathStatistic ps1 = GetPathStat_Insert_Started(path.Id, user.Id, viewStatBefore,viewStatAfter );


                pui.ItemStatus = (Ps != null) ? Ps.Status : StatusStatistic.None;
                if (Ps != null)
                {
                    pui.StStartDate = Ps.StartDate;
                    pui.Completion = Ps.Completion;

                    if (ps1 != null) pui.StFirstActivity = ps1.CreatedOn;

                    pui.Mark = Ps.Mark;
                    if (CheckStatusStatistic(Ps.Status, StatusStatistic.Completed) || CheckStatusStatistic(Ps.Status, StatusStatistic.CompletedPassed))
                    {
                        pui.StEndDate = Ps.CreatedOn;
                        pui.ItemStatus = CheckStatusStatistic(Ps.Status, StatusStatistic.CompletedPassed) ? StatusStatistic.CompletedPassed : StatusStatistic.Completed;
                    }
                    else if (CheckStatusStatistic(Ps.Status, StatusStatistic.Browsed) && !CheckStatusStatistic(Ps.Status, StatusStatistic.Started))
                        pui.ItemStatus = StatusStatistic.None;
                    else
                        pui.ItemStatus = StatusStatistic.Started;

                }
                pui.Questionnaires = new List<dtoBaseUserPathQuiz>();
                if (!CheckStatusStatistic(pui.ItemStatus, StatusStatistic.None) || CheckStatusStatistic(pui.ItemStatus, StatusStatistic.CompletedPassed))
                {
                    foreach (var qItem in dpu.Questionnaires)
                    {
                        dtoBaseUserPathQuiz dupq = new dtoBaseUserPathQuiz();
                        dupq.IdQuestionnaire = qItem.IdQuestionnaire;
                        dupq.IdPerson = user.Id;
                        dupq.IdSub = qItem.IdSubActivity;
                        pui.Questionnaires.Add(dupq);
                    }
                }
                dpu.Users.Add(pui);
            }
            return dpu;
        }

        public dtoUserPaths GetSelectedUserPathsCount(Boolean isMooc, Int32 userId, DateTime viewStatBefore, Int32 pageindex = 0, Int32 pagesize = 0, string deadline = "{0} {1} {2} {3}", string order = "", Boolean ascending = true, string pathname = "", string communityname = "", string startdate = null, string enddate = null, StatusFilter status = StatusFilter.all, IEnumerable<Int32> idcommunities = null, Boolean onlyCertificateQuiz = false)
        {
            dtoUserPaths dup = new dtoUserPaths();
            try
            {

                var q = (from item in manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.IsMooc == isMooc select item);

                if (!string.IsNullOrEmpty(pathname))
                {
                    q = (from item in q where item.Name.Contains(pathname) select item);
                }

                if (idcommunities != null && idcommunities.Count() > 0)
                {
                    q = (from item in q where item.Community != null && idcommunities.Contains( item.Community.Id) select item);
                }

                if (!string.IsNullOrEmpty(communityname))
                {
                    q = (from item in q where item.Community!=null && item.Community.Name.Contains(communityname) select item);
                }

                if (!string.IsNullOrEmpty(startdate))
                {
                    q = (from item in q where   item.StartDate >= DateTime.Parse(startdate) select item);
                }

                if (!string.IsNullOrEmpty(enddate))
                {
                    q = (from item in q where   item.EndDate <= DateTime.Parse(enddate) select item);
                }

               

                IList<Path> AllPath = q.ToList<Path>();

                switch (status)
                {
                    case StatusFilter.all:
                        break;
                    case StatusFilter.unlocked:
                        AllPath = (from item in AllPath where serviceEP.CheckStatus(item.Status, Status.NotLocked) select item).ToList();
                        break;
                    case StatusFilter.locked:
                        AllPath = (from item in AllPath where serviceEP.CheckStatus(item.Status, Status.Locked) select item).ToList();
                        break;
                    default:
                        break;
                }


                //IList<Path> AllPath = manager.GetAll<Path>(ep => ep.Deleted == BaseStatusDeleted.None);

                //if (!string.IsNullOrEmpty(pathname))
                //{
                //    AllPath = (from item in AllPath where item.Name.Contains(pathname) select item).ToList();
                //}

                IList<Int64> AllPathIds = (from i in AllPath select i.Id).ToList();

                var allSubscription = new List<LazySubscription>() ;

                allSubscription = (from i in manager.GetIQ<LazySubscription>() where i.IdPerson == userId select i).ToList();

                //if (!string.IsNullOrEmpty(communityname))
                //{
                //    var qtemp = (from item in manager.GetIQ<Community>() where item.Name.Contains(communityname) select item.Id).ToList();
                //    allSubscription = (from i in manager.GetIQ<LazySubscription>() where i.IdPerson == userId && qtemp.Contains(i.IdCommunity) select i).ToList();

                //}
                //else
                //{
                    
                //}

                var allCommunitiesId = (from i in allSubscription select i.IdCommunity).ToList();

                var PersonAssignment = (from i in manager.GetIQ<PathPersonAssignment>() where AllPathIds.Contains(i.IdPath) && i.IdPerson == userId select i).ToList();

                var CurrentCommunitiesRolesAssignment = (from i in manager.GetIQ<PathCRoleAssignment>() where AllPathIds.Contains(i.IdPath) select i).ToList();

                CurrentCommunitiesRolesAssignment = (from i in CurrentCommunitiesRolesAssignment where allCommunitiesId.Contains(i.IdCommunity) select i).ToList();

                var PathWithStat = GetPathsWithStat(userId, viewStatBefore);

                litePerson p = manager.GetLitePerson(userId);
                dup.Id = p.Id;
                dup.Person = p;

                IList<dtoFullPathItem> paths = (from ep in AllPath
                                              where !((ep.Status & Status.Draft) == Status.Draft) && ep.Community!=null &&
                                              (
                                                  //   serviceEP.CheckRoleEp((from ass in q1 where ass.Path == ep && ass.Person == p select ass.RoleEP).FirstOrDefault(), RoleEP.Participant)
                                                  //||
                                                  //   serviceEP.CheckRoleEp((from ass in q1 where ass.Path == ep && ass.Person == p select ass.RoleEP).FirstOrDefault(), RoleEP.StatViewer)
                                                  //||

                                              //   serviceEP.CheckRoleEp((from ass in q2 where ass.Path == ep && ass.Role.Id == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.Participant)
                                                  //||
                                                  //   serviceEP.CheckRoleEp((from ass in q2 where ass.Path == ep && ass.Role.Id == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.StatViewer)

                                              serviceEP.CheckRoleEpNotStrict((from ass in PersonAssignment where ass.IdPath == ep.Id  && ass.IdPerson == p.Id select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer)

                                              ||
                                              serviceEP.CheckRoleEpNotStrict((from ass in CurrentCommunitiesRolesAssignment where ass.IdPath == ep.Id  &&  allSubscription.Any(s=> s.IdCommunity== ass.IdCommunity && s.IdRole== ass.IdRole) select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer)
                                              || PathWithStat.Contains(ep.Id)
                                              )

                                              select new dtoFullPathItem(ep, Status.None, RoleEP.Participant)
                                              {
                                                  Status =  (from i in allSubscription where i.IdCommunity == ep.Community.Id select i.IdRole).Count()>0 ? serviceEP.PathStatus(ep.Id, userId, (from i in allSubscription where i.IdCommunity == ep.Community.Id select i.IdRole).First(), false): Status.None ,
                                              }
                                ).ToList();

                //Int32 paths1 = (from ep in AllPath
                //                               where !((ep.Status & Status.Draft) == Status.Draft) && ep.Community == null
                //                               select ep).Count();

                //foreach (var ep in paths)
                //{
                //    ep.Status=                    serviceEP.PathStatus(ep.Id, userId, (from i in allSubscription where i.IdCommunity == ep.Community.Id select i.IdRole).First(), false)
                //}

                dup.Paths = (from path in paths select new dtoUserPathInfo() { PathInfo = path, IdPerson = userId, Ps = GetPathStat_Insert(path.Id, dup.Id), PsFirstActivity = GetPathStat_Insert_Started(path.Id, dup.Id, viewStatBefore) }).ToList(); //,viewStatBefore?

                

                foreach (var path in dup.Paths)
                {

                    path.IdCommunity = (from item in AllPath where item.Id == path.IdPath && item.Community != null select item.Community.Id).First();

                    Path currentpath = (from item in AllPath where item.Id == path.IdPath select item).First();                    

                    if (currentpath.FloatingDeadlines == true)
                    {
                        path.Deadline = "floating";
                        //path.StDeadline =;
                    }
                    else
                    {

                        //if (currentpath.StartDate.HasValue)
                        //{
                        //    path.Deadline = currentpath.StartDate.Value.ToShortDateString();
                        //}

                        //if (currentpath.EndDate.HasValue)
                        //{
                        //    path.EndDate = currentpath.EndDate.Value.ToShortDateString();
                        //}

                        //path.Deadline = path.EndDate;

                        path.Deadline = String.Format(deadline, ToShortDate(currentpath.StartDate), ToShortDate(currentpath.EndDate), (currentpath.StartDate.HasValue) ? "" : "empty", (currentpath.EndDate.HasValue) ? "" : "empty");
                    }
                    Community c = manager.GetCommunity(path.IdCommunity);
                    path.CommunityName = (c == null) ? "" : c.Name;
                    if (c!=null)
                    {
                        path.IdOrganization = c.IdOrganization;
                        path.OrganizationName = manager.GetOrganizationName(path.IdOrganization);
                    }
                    else {
                        path.IdOrganization = 0;
                        path.OrganizationName = "";
                    }

                    //path.PathType = currentpath.EPType;
                    if (serviceEP.Context.UserContext.CurrentUserID == userId)
                    {
                        path.PathLocked = serviceEP.CheckStatus(currentpath.Status, Status.Locked);

                        path.CanManage = serviceEP.CheckRoleEpNotStrict((from ass in PersonAssignment where ass.IdPath == path.IdPath && ass.IdPerson == path.IdPerson select ass.RoleEP).FirstOrDefault(), RoleEP.Manager);
                        path.CanManage |= serviceEP.CheckRoleEpNotStrict((from ass in CurrentCommunitiesRolesAssignment where ass.IdPath == path.IdPath && allSubscription.Any(s=> s.IdCommunity== ass.IdCommunity && s.IdRole== ass.IdRole) select ass.RoleEP).FirstOrDefault(), RoleEP.Manager);

                        path.CanPlay = serviceEP.CheckRoleEpNotStrict((from ass in PersonAssignment where ass.IdPath == path.IdPath && ass.IdPerson == path.IdPerson select ass.RoleEP).FirstOrDefault(), RoleEP.Participant);
                        path.CanPlay |= serviceEP.CheckRoleEpNotStrict((from ass in CurrentCommunitiesRolesAssignment where ass.IdPath == path.IdPath && allSubscription.Any(s => s.IdCommunity == ass.IdCommunity && s.IdRole == ass.IdRole) select ass.RoleEP).FirstOrDefault(), RoleEP.Participant);

                        path.CanPlay &= !path.PathLocked;

                        path.CanStat = serviceEP.CheckRoleEpNotStrict((from ass in PersonAssignment where ass.IdPath == path.IdPath && ass.IdPerson == path.IdPerson select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer);
                        path.CanStat |= serviceEP.CheckRoleEpNotStrict((from ass in CurrentCommunitiesRolesAssignment where ass.IdPath == path.IdPath && allSubscription.Any(s => s.IdCommunity == ass.IdCommunity && s.IdRole == ass.IdRole) select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer);
                    }
                    else
                    {
                        UserTypeStandard uts = (UserTypeStandard)serviceEP.Context.UserContext.UserTypeID;

                        path.CanPlay = false;

                        if (uts == UserTypeStandard.SysAdmin || uts == UserTypeStandard.Administrator)
                        {
                            path.CanManage = true;
                            path.CanStat = true;
                        }

                        if (uts == UserTypeStandard.Administrative )
                        {
                            path.CanManage = true;
                            path.CanStat = true;
                        }
                    }
                    path.ItemStatus = (path.Ps == null) ? StatusStatistic.None : path.Ps.Status;
                    if (path.Ps == null)
                        path.Status = "notstarted";
                    else
                    {
                        path.Completion = path.Ps.Completion.ToString();
                        if (path.Ps.StartDate.HasValue)
                        {
                            path.StartDate = path.Ps.StartDate.Value.ToShortDateString() + " " + path.Ps.StartDate.Value.ToShortTimeString();
                            path.StStartDate = path.Ps.StartDate;
                        }
                        if (path.PsFirstActivity != null)
                        {
                            path.FirstActivity = path.PsFirstActivity.CreatedOn.Value.ToShortDateString() + " " + path.PsFirstActivity.CreatedOn.Value.ToShortTimeString();
                            path.StFirstActivity = path.PsFirstActivity.CreatedOn;
                        }
                        if (CheckStatusStatistic(path.Ps.Status, StatusStatistic.Completed))
                        {
                            path.Status = "completed";
                            if (path.Ps.CreatedOn.HasValue)
                            {
                                path.EndDate = path.Ps.CreatedOn.Value.ToShortDateString() + " " + path.Ps.CreatedOn.Value.ToShortTimeString();
                                path.StEndDate = path.Ps.CreatedOn;
                            }
                        }
                        else
                        {
                            if (CheckStatusStatistic(path.Ps.Status, StatusStatistic.Browsed) && !CheckStatusStatistic(path.Ps.Status, StatusStatistic.Started))
                            {
                                path.Status = "notstarted";
                                path.ItemStatus = StatusStatistic.None;
                            }
                            else
                            {
                                path.Status = "started";
                                path.ItemStatus = StatusStatistic.Started;
                            }
                        }
                    }

                    path.Questionnaires = new List<dtoUserPathQuiz>();
                    if (path.Status != "notstarted")
                    {
                        IList<SubActivity> subs = serviceEP.GetSubactiviesByPathIdAndTypes(path.IdPath, SubActivityType.Quiz);
                        foreach (var sub in subs)
                        {
                            if (!onlyCertificateQuiz)
                                path.Questionnaires.Add(new dtoUserPathQuiz() { IdQuestionnaire = sub.IdObjectLong, IdSub = sub.Id, IdPerson=userId  });
                            else
                            {
                                // VISIBILE SI, MANDATORY NON OBBLIGATORIO PER IL CERTIFICATO o SI ? 
                                // VERIFICARE DOMANI INSIEME
                                Boolean isCertificate = (from item in manager.GetIQ<SubActivityLink>()
                                                         where item.IdObject == sub.IdObjectLong && item.ContentType == SubActivityLinkType.quiz && item.Deleted == BaseStatusDeleted.None &&
                                                         item.SubActivity.Path.Id == path.IdPath && item.Visible
                                                         select item).Any();

                                // (item.Visible || item.Mandatory) select item).Any();
                                if (isCertificate)
                                    path.Questionnaires.Add(new dtoUserPathQuiz() { IdQuestionnaire = sub.IdObjectLong, IdSub = sub.Id, IdPerson = userId });
                            }
                        }
                    }
                }

                dup.Total = paths.Count();

                dup.NotStarted = dup.Total;
                dup.Started = dup.Total;
                dup.Completed = dup.Total;

                //dup.Started = (from path in paths where GetPathStat_Insert(path.Id, dup.Id) != null && CheckStatusStatistic(GetPathStat_Insert(path.Id, dup.Id).Status, StatusStatistic.Started) select path.Id).Count();
                //dup.Completed = (from path in paths where GetPathStat_Insert(path.Id, dup.Id) != null && CheckStatusStatistic(GetPathStat_Insert(path.Id, dup.Id).Status, StatusStatistic.Completed) select path.Id).Count();

                dup.Started = (from path in dup.Paths where path.Ps != null && (CheckStatusStatistic(path.Ps.Status, StatusStatistic.BrowsedStarted) || CheckStatusStatistic(path.Ps.Status, StatusStatistic.Started)) select path.IdPath).Count();
                dup.Completed = (from path in dup.Paths where path.Ps != null && CheckStatusStatistic(path.Ps.Status, StatusStatistic.Completed) select path.IdPath).Count();

                dup.Started -= dup.Completed;

                dup.NotStarted = dup.Total - dup.Started - dup.Completed;


                if (!string.IsNullOrEmpty(order))
                {
                    if (ascending)
                    {
                        switch (order)
                        {
                            case "path":
                                dup.Paths = dup.Paths.OrderBy(x => x.PathName).ToList();
                                break;
                            case "timerange":
                                dup.Paths = dup.Paths.OrderBy(x => x.Deadline).ToList();
                                break;
                            case "community/path":
                                dup.Paths = (from item in dup.Paths orderby item.CommunityName, item.PathName select item).ToList();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (order)
                        {
                            case "path":
                                dup.Paths = dup.Paths.OrderByDescending(x => x.PathName).ToList();
                                break;
                            case "timerange":
                                dup.Paths = dup.Paths.OrderByDescending(x => x.Deadline).ToList();
                                break;
                            case "community/path":
                                dup.Paths = (from item in dup.Paths orderby item.CommunityName descending, item.PathName descending select item).ToList();
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (pagesize > 0)
                {
                    dup.Paths = (from item in dup.Paths select item).Skip(pageindex * pagesize).Take(pagesize).ToList();
                }

                return dup;

            }
            catch (Exception)
            {
                return dup;
            }
        }
        public dtoUserPaths GetSelectedUserPathsCount(Boolean isMooc, Int32 cmntId, Int32 userId)
        {
            try
            {
                IList<Path> CommunityPath = manager.GetAll<Path>(ep =>  ep.Community!=null && ep.Community.Id == cmntId && ep.IsMooc == isMooc && ep.Deleted == BaseStatusDeleted.None).ToList<Path>();

                IList<Int64> PathIds = (from i in CommunityPath select i.Id).ToList();

                var item = (from i in manager.GetIQ<LazySubscription>() where i.IdCommunity == cmntId && i.IdPerson == userId select i).FirstOrDefault();

                var q1 = (from i in manager.GetIQ<PathPersonAssignment>() where PathIds.Contains(i.IdPath) select i).ToList();
                var q2 = (from i in manager.GetIQ<PathCRoleAssignment>() where PathIds.Contains(i.IdPath) select i).ToList();

                dtoUserPaths dup = new dtoUserPaths();

                litePerson p = manager.GetLitePerson(item.IdPerson);
                dup.Id = p.Id;
                dup.Person = p;

                IList<dtoFullPathItem> paths = (from ep in CommunityPath
                                                where !((ep.Status & Status.Draft) == Status.Draft) &&
                                                (
                                                    //   serviceEP.CheckRoleEp((from ass in q1 where ass.Path == ep && ass.Person == p select ass.RoleEP).FirstOrDefault(), RoleEP.Participant)
                                                    //||
                                                    //   serviceEP.CheckRoleEp((from ass in q1 where ass.Path == ep && ass.Person == p select ass.RoleEP).FirstOrDefault(), RoleEP.StatViewer)
                                                    //||

                                                //   serviceEP.CheckRoleEp((from ass in q2 where ass.Path == ep && ass.Role.Id == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.Participant)
                                                    //||
                                                    //   serviceEP.CheckRoleEp((from ass in q2 where ass.Path == ep && ass.Role.Id == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.StatViewer)

                                                serviceEP.CheckRoleEpNotStrict((from ass in q1 where ass.IdPath == ep.Id && ass.IdPerson == p.Id select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer)
                                                ||
                                                serviceEP.CheckRoleEpNotStrict((from ass in q2 where ass.IdPath == ep.Id && ass.IdRole == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer)
                                                )
                                                select new dtoFullPathItem(ep, serviceEP.PathStatus(ep.Id, item.IdPerson, item.IdRole, false), RoleEP.Participant)
                                ).ToList();

                dup.Paths = (from path in paths select new dtoUserPathInfo() { IdCommunity = cmntId, PathInfo = path, IdPerson = item.IdPerson, Ps = GetPathStat_Insert(path.Id, dup.Id), PsFirstActivity = GetPathStat_Insert_Started(path.Id, dup.Id, DateTime.Now.AddSeconds(1)) }).ToList();


                foreach (var path in dup.Paths)
                {
                    path.ItemStatus = (path.Ps == null) ? StatusStatistic.None : path.Ps.Status;
                    if (path.Ps == null)
                        path.Status = "notstarted";
                    else
                    {
                        path.Completion = path.Ps.Completion.ToString();
                        path.ItemStatus = path.Ps.Status;
                        if (path.Ps.StartDate.HasValue)
                        {
                            path.StartDate = path.Ps.StartDate.Value.ToShortDateString() + " " + path.Ps.StartDate.Value.ToShortTimeString();
                            // MODIFICATO da path.StStartDate = path.Ps.CreatedOn;
                            path.StStartDate = path.Ps.StartDate;
                        }
                        if (path.PsFirstActivity != null)
                        {
                            path.FirstActivity = path.PsFirstActivity.CreatedOn.Value.ToShortDateString() + " " + path.PsFirstActivity.CreatedOn.Value.ToShortTimeString();
                            path.StFirstActivity = path.PsFirstActivity.CreatedOn;
                        }
                        if (CheckStatusStatistic(path.Ps.Status, StatusStatistic.Completed))
                        {
                            path.Status = "completed";
                            if (path.Ps.CreatedOn.HasValue)
                            {
                                path.EndDate = path.Ps.CreatedOn.Value.ToShortDateString() + " " + path.Ps.CreatedOn.Value.ToShortTimeString();
                                path.StEndDate = path.Ps.CreatedOn;
                            }
                        }
                        else
                        {
                            if (CheckStatusStatistic(path.Ps.Status, StatusStatistic.Browsed) && !CheckStatusStatistic(path.Ps.Status, StatusStatistic.Started))
                            {
                                path.Status = "notstarted";
                                path.ItemStatus = StatusStatistic.None;
                            }
                            else
                            {
                                path.Status = "started";
                                path.ItemStatus = StatusStatistic.Started;
                            }
                        }
                    }
                }

                dup.Total = paths.Count();

                dup.NotStarted = dup.Total;
                dup.Started = dup.Total;
                dup.Completed = dup.Total;

                //dup.Started = (from path in paths where GetPathStat_Insert(path.Id, dup.Id) != null && CheckStatusStatistic(GetPathStat_Insert(path.Id, dup.Id).Status, StatusStatistic.Started) select path.Id).Count();
                //dup.Completed = (from path in paths where GetPathStat_Insert(path.Id, dup.Id) != null && CheckStatusStatistic(GetPathStat_Insert(path.Id, dup.Id).Status, StatusStatistic.Completed) select path.Id).Count();

                dup.Started = (from path in dup.Paths where path.Ps != null && (CheckStatusStatistic(path.Ps.Status, StatusStatistic.BrowsedStarted) || CheckStatusStatistic(path.Ps.Status, StatusStatistic.Started)) select path.IdPath).Count();
                dup.Completed = (from path in dup.Paths where path.Ps != null && CheckStatusStatistic(path.Ps.Status, StatusStatistic.Completed) select path.IdPath).Count();

                dup.Started -= dup.Completed;

                dup.NotStarted = dup.Total - dup.Started - dup.Completed;



                return dup;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public IList<dtoUserPaths> GetUserPathsCount(Boolean isMooc, Int32 cmntId, Int32 roleId = 0, string username = "", Int32 pageIdx = 0, Int32 pageSize = 0, Boolean allStatistics = false)
        {
            try
            {
                IList<Path> CommunityPath = manager.GetAll<Path>(ep => ep.Community!=null && ep.Community.Id == cmntId && ep.Deleted == BaseStatusDeleted.None && ep.IsMooc == isMooc).ToList<Path>();


                IList<Int64> PathIds = (from item in CommunityPath select item.Id).ToList();

                IList<dtoUserPaths> ret = new List<dtoUserPaths>();

                //var q = (from item in manager.GetIQ<LazySubscription>()
                //         where item.IdCommunity == cmntId && item.Enabled == true
                //         //&& item.IdRole>0
                //         select item);

                var qt = (from item in manager.GetIQ<Subscription>()
                          where item.Community.Id == cmntId && item.Enabled == true
                          && item.Role.Id > 0
                          select item).ToList();

                qt = (from item in qt orderby item.Person.Surname, item.Person.Name select item).ToList();

                if (username != "")
                {
                    qt = (from item in qt where item.Person.Name.ToLower().Contains(username.ToLower()) || item.Person.Surname.ToLower().Contains(username.ToLower()) select item).ToList();
                }

                var q = (from item in qt select manager.Get<LazySubscription>(item.Id));

                if (roleId > 0)
                {
                    q = q.Where(x => x.IdRole == roleId);
                }

                if (pageSize > 0)
                {
                    q = q.Skip((pageIdx * pageSize)).Take(pageSize);
                }
                IList<LazySubscription> subscriptions = q.ToList();

                var q1 = (from i in manager.GetIQ<PathPersonAssignment>() where PathIds.Contains(i.IdPath) select i).ToList();
                var q2 = (from i in manager.GetIQ<PathCRoleAssignment>() where PathIds.Contains(i.IdPath) select i).ToList();




                foreach (var item in subscriptions)
                {
                    dtoUserPaths dup = new dtoUserPaths();

                    litePerson p = manager.GetLitePerson(item.IdPerson);
                    dup.Id = p.Id;
                    dup.Person = p;

                    // IList<dtoEPitemList> paths = this.serviceEP.GetMyEduPaths(p.Id, cmntId, item.IdRole, EpViewModeType.View);

                    //IList<dtoEPitemList> paths = (from ep in CommunityPath
                    //                              where !((ep.Status & Status.Draft) == Status.Draft) &&
                    //                              (
                    //                              serviceEP.CheckRoleEp(serviceEP.GetUserRole_ByPath(ep.Id, item.IdPerson, item.IdRole), RoleEP.Participant)
                    //                              ||
                    //                              serviceEP.CheckRoleEp(serviceEP.GetUserRole_ByPath(ep.Id, item.IdPerson, item.IdRole), RoleEP.StatViewer)
                    //                              )
                    //                              select new dtoEPitemList(ep, serviceEP.PathStatus(ep.Id, item.IdPerson, item.IdRole, false), RoleEP.Participant)
                    //                              {
                    //                                  type = ep.EPType
                    //                              }
                    //            ).ToList();

                    IList<dtoFullPathItem> paths = (from ep in CommunityPath
                                                  where !((ep.Status & Status.Draft) == Status.Draft) &&
                                                  (
                                                      //   serviceEP.CheckRoleEp((from ass in q1 where ass.Path == ep && ass.Person == p select ass.RoleEP).FirstOrDefault(), RoleEP.Participant)
                                                      //||
                                                      //   serviceEP.CheckRoleEp((from ass in q1 where ass.Path == ep && ass.Person == p select ass.RoleEP).FirstOrDefault(), RoleEP.StatViewer)
                                                      //||

                                                  //   serviceEP.CheckRoleEp((from ass in q2 where ass.Path == ep && ass.Role.Id == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.Participant)
                                                      //||
                                                      //   serviceEP.CheckRoleEp((from ass in q2 where ass.Path == ep && ass.Role.Id == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.StatViewer)

                                                  serviceEP.CheckRoleEpNotStrict((from ass in q1 where ass.IdPath == ep.Id && ass.IdPerson == p.Id select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer)
                                                  ||
                                                  serviceEP.CheckRoleEpNotStrict((from ass in q2 where ass.IdPath == ep.Id && ass.IdRole == item.IdRole select ass.RoleEP).FirstOrDefault(), RoleEP.Participant | RoleEP.StatViewer)
                                                  || 
                                                  allStatistics
                                                  )
                                                    select new dtoFullPathItem(ep, serviceEP.PathStatus(ep.Id, item.IdPerson, item.IdRole, false), RoleEP.Participant)
                                ).ToList();

                    //IList<dtoEPitemList> paths = new List<dtoEPitemList>();

                    dup.Paths = (from path in paths select new dtoUserPathInfo() { PathInfo = path, IdPerson = item.IdPerson, Ps = GetPathStat_Insert(path.Id, dup.Id), PsFirstActivity = GetPathStat_Insert_Started(path.Id, dup.Id, DateTime.Now.AddSeconds(1)) }).ToList();


                    foreach (var path in dup.Paths)
                    {
                        path.ItemStatus = (path.Ps == null) ? StatusStatistic.None : path.Ps.Status;
                        if (path.Ps == null)
                            path.Status = "notstarted";
                        else
                        {
                            path.Completion = path.Ps.Completion.ToString();
                            path.ItemStatus = path.Ps.Status;
                            if (path.Ps.StartDate.HasValue)
                            {
                                path.StartDate = path.Ps.StartDate.Value.ToShortDateString() + " " + path.Ps.StartDate.Value.ToShortTimeString();
                                path.StStartDate = path.Ps.StartDate;
                            }
                            if (path.PsFirstActivity != null)
                            {
                                path.FirstActivity = path.PsFirstActivity.CreatedOn.Value.ToShortDateString() + " " + path.PsFirstActivity.CreatedOn.Value.ToShortTimeString();
                                path.StFirstActivity = path.PsFirstActivity.CreatedOn;
                            }

                            if (CheckStatusStatistic(path.Ps.Status, StatusStatistic.Completed))
                            {
                                path.Status = "completed";
                                if (path.Ps.CreatedOn.HasValue)
                                {
                                    path.EndDate = path.Ps.CreatedOn.Value.ToShortDateString() + " " + path.Ps.CreatedOn.Value.ToShortTimeString();
                                    path.StEndDate = path.Ps.CreatedOn;
                                }
                            }
                            else
                            {
                                if (CheckStatusStatistic(path.Ps.Status, StatusStatistic.Browsed) && !CheckStatusStatistic(path.Ps.Status, StatusStatistic.Started))
                                {
                                    path.Status = "notstarted";
                                    path.ItemStatus=  StatusStatistic.None;
                                }
                                else
                                {
                                    path.Status = "started";
                                    path.ItemStatus = StatusStatistic.Started;
                                }
                            }
                        }
                    }

                    dup.Total = paths.Count();

                    dup.NotStarted = dup.Total;
                    dup.Started = dup.Total;
                    dup.Completed = dup.Total;

                    //dup.Started = (from path in paths where GetPathStat_Insert(path.Id, dup.Id) != null && CheckStatusStatistic(GetPathStat_Insert(path.Id, dup.Id).Status, StatusStatistic.Started) select path.Id).Count();
                    //dup.Completed = (from path in paths where GetPathStat_Insert(path.Id, dup.Id) != null && CheckStatusStatistic(GetPathStat_Insert(path.Id, dup.Id).Status, StatusStatistic.Completed) select path.Id).Count();

                    dup.Started = (from path in dup.Paths where path.Ps != null && (CheckStatusStatistic(path.Ps.Status, StatusStatistic.BrowsedStarted) || CheckStatusStatistic(path.Ps.Status, StatusStatistic.Started)) select path.IdPath).Count();
                    dup.Completed = (from path in dup.Paths where path.Ps != null && CheckStatusStatistic(path.Ps.Status, StatusStatistic.Completed) select path.IdPath).Count();

                    dup.Started -= dup.Completed;

                    dup.NotStarted = dup.Total - dup.Started - dup.Completed;
                    //(from path in dup.Paths where path.Ps == null || CheckStatusStatistic(path.Ps.Status, StatusStatistic.Browsed) select path.IdPath).Count();

                    //if (dup.Total > 0)
                    //{
                    ret.Add(dup);
                    //}


                }


                return ret;
            }
            catch (Exception)
            {
                return new List<dtoUserPaths>();
            }
        }

        public IList<SubActivityStatistic> GetSubActivitiesCertificateCompleted(DateTime viewStatBefore, Int32 userId = 0, Int32 commId = 0, Boolean excludeSaved = true)
        {
            //IList<SubActivityStatistic> list = null;

            var allPath = (from item in manager.GetIQ<Path>() select item);

            if (commId != 0)
            {
                allPath = (from item in allPath where item.Community != null && item.Community.Id == commId select item);
            }

            IList<Int64> extractedPath = (from item in allPath select item.Id).ToList();

            var subStat = (from stat in manager.GetIQ<SubActivityStatistic>()
                           where  stat.CreatedOn <= viewStatBefore && stat.Deleted == BaseStatusDeleted.None 
                           && (userId==0 || stat.Person.Id==userId)
                           && ( extractedPath.Count==0 || extractedPath.Contains(stat.IdPath) )
                           && stat.SubActivity.ContentType ==SubActivityType.Certificate 
                           && (stat.SubActivity.SaveCertificate == false || !excludeSaved)
                           && (stat.Status==StatusStatistic.Completed || stat.Status==StatusStatistic.CompletedPassed)
                           
                           //select new { idPerson = stat.Person.Id, idStatistic = stat.Id, CreatedOn = stat.CreatedOn, IdSubAct = stat.SubActivity.Id }).ToList();
                           select stat).ToList();

            return subStat;
        }

        public Int32 CountUserPathsCount(Int32 cmntId, Int32 roleId = 0, string username="")
        {
            //IList<dtoUserPaths> ret = new List<dtoUserPaths>();

            //var q = (from item in manager.GetIQ<LazySubscription>()
            //         where item.IdCommunity == cmntId && item.Enabled == true
            //         select item);
            //if (roleId > 0)
            //{
            //    q = q.Where(x => x.IdRole == roleId);
            //}


            var qt = (from item in manager.GetIQ<Subscription>()
                      where item.Community.Id == cmntId && item.Enabled == true
                      && item.Role.Id > 0
                      select item).ToList();           

            if (username != "")
            {
                qt = (from item in qt where item.Person.Name.ToLower().Contains(username.ToLower()) || item.Person.Surname.ToLower().Contains(username.ToLower()) select item).ToList();
            }

            var q = (from item in qt select manager.Get<LazySubscription>(item.Id));


            if (roleId > 0)
            {
                q = q.Where(x => x.IdRole == roleId);
            }  

            //IList<LazySubscription> subscriptions = q.ToList();

            return q.Count();
        }

        public Int32 GetStatCount_ByUserAndStatus(Int64 idPath, Int32 idUser, DateTime viewStatBefore, StatusStatistic status, Boolean not = false)
        {
            Path p = serviceEP.GetPath(idPath);
            var list = p.UnitList.SelectMany(x => x.ActivityList).Select(x => x.Id).ToList();

            IList<ActivityStatistic> actStat = new List<ActivityStatistic>();

            foreach (var item in list)
            {
                actStat = actStat.Concat((from stat in manager.GetIQ<ActivityStatistic>()
                                          where stat.IdPath == idPath &&
                                           item == stat.Activity.Id &&
                                           stat.Person.Id == idUser && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn <= viewStatBefore
                                          orderby stat.CreatedOn descending, stat.Status descending, stat.Completion descending
                                          select stat).Take(1).ToList()).ToList();
            }


            if (not)
            {
                //return actStat.Count() - (from item in actStat where CheckStatusStatistic(item.Status, status) select item).Count();
                return list.Count - (from item in actStat where CheckStatusStatistic(item.Status, status) select item).Count();
            }
            else
            {
                return (from item in actStat where CheckStatusStatistic(item.Status, status) select item).Count();
            }
            
        }

        #region get statistic for  page

        public dtoActivityStatistic GetDtoActivityStatistic_ByUser(long parentActivityId, int communityId, int participantId, int participantCrole, string evaluatorIPaddress, string evaluatorProxyIPaddress, DateTime viewStatBefore)
        {
            return GetDtoActivityStatistic_ByUser(parentActivityId, communityId, participantId, participantId, participantCrole, evaluatorIPaddress, evaluatorProxyIPaddress, viewStatBefore);
        }               
        
        public dtoActivityStatistic GetDtoActivityStatistic_ByUser(long parentActivityId, int communityId, int participantId, int supervisorId, int supervisorCrole, string supervisorIpAddress, string supervisorProxyIpAddress,DateTime viewStatBefore)
        {
            int participantCrole = manager.GetActiveSubscriptionIdRole(participantId, communityId);            
           
            Activity oAct = manager.Get<Activity>(parentActivityId);

            IList<SubActivityStatistic> ActiveSubActStat = GetActiveSubActStat_Insert(parentActivityId, participantId, viewStatBefore); 

            IList<long> ExistingStat_SubActId = (from item in ActiveSubActStat select item.SubActivity.Id).ToList();
            IList<long> ActiveSubActId = serviceEP.GetActiveSubActivitiesId(oAct.SubActivityList);

            IList<SubActivityStatistic> NotExistingStat = (from item in ActiveSubActId
                                                           where !ExistingStat_SubActId.Contains(item)
                                                           select new SubActivityStatistic()
                                                           {
                                                               SubActivity = manager.Get<SubActivity>(item)
                                                           }).ToList();

            IList<SubActivityStatistic> AllStat = ActiveSubActStat.Union<SubActivityStatistic>(NotExistingStat).ToList();
            return new dtoActivityStatistic()
            {
                Id = oAct.Id,
                Name = oAct.Name,
                SubActivities = (from item in AllStat
                                 orderby item.SubActivity.DisplayOrder
                                 select new dtoSubActivityStatistic()
                                 {
                                    // Id = item.SubActivity.Id,
                                 //    Name=item.SubActivity.Description,
                                     Completion = item.Completion,
                                     Mark=item.Mark,
                                 //    ContentType = item.SubActivity.ContentType,
                                     StatusStat =GetMainStatusStatistic( item.Status),
                                     SubActivity = new dtoSubActivity(item.SubActivity),
                                  //   Status=item.SubActivity.Status,
                                 //    ModuleLink = item.SubActivity.ModuleLink,
                                //     Weight=item.SubActivity.Weight,
                                     isMandatory=serviceEP.SubActivityIsMandatoryForParticipant(item.SubActivity.Id,participantId,participantCrole)
                                 }).ToList()
            };
        }

        public dtoItemWithStatistic GetOwnerEduPathStatisticStructure(long PathId, int userId, int CRoleId, DateTime viewStatBefore)
        {
           return SubGetOwnerEduPathStatisticStructure(PathId, userId, CRoleId, false,viewStatBefore);
        }


        public dtoPathUserStatistic GetEduPathUserStatistic(long idPath, DateTime viewStatBefore, Boolean allStatistics=false)
        {
            Path path = manager.Get<Path>(idPath);

            dtoPathUserStatistic dpu = new dtoPathUserStatistic();

            if (path.Community != null)
            {
                dpu.CommunityId = path.Community.Id;
                dpu.CommunityName = manager.GetCommunityName(dpu.CommunityId);
            }
            else
                dpu.CommunityId = 0;

            //var PersonAssignment = (from i in manager.GetIQ<PathPersonAssignment>() where AllPathIds.Contains(i.Path.Id) && i.Person.Id == userId select i).ToList();

            //var CurrentCommunitiesRolesAssignment = (from i in manager.GetIQ<PathCRoleAssignment>() where AllPathIds.Contains(i.Path.Id) select i).ToList();

            var PersonAssignment = (from i in manager.GetIQ<PathPersonAssignment>() where i.IdPath == idPath select i).ToList();
            var RoleAssignment = (from i in manager.GetIQ<PathCRoleAssignment>() where i.IdPath == idPath select i).ToList();

            var roles = (from i in RoleAssignment select i.IdRole).Distinct().ToList();

            IList<Int32> idUsersPA = (from i in PersonAssignment select i.IdPerson).ToList();
            IList<Int32> idUsersRC = (from i in manager.GetIQ<LazySubscription>() where i.IdCommunity == dpu.CommunityId && roles.Contains(i.IdRole) select i.IdPerson).ToList();
            IList<Int32> idUsers = idUsersPA.Union(idUsersRC).ToList();            

            if (allStatistics)
            {
                IList<Int32> idUsersAS = GetUsersWithStat(idPath, viewStatBefore);
                idUsers = idUsers.Union(idUsersAS).ToList();
            }

            IList<Activity> activities = path.UnitList.SelectMany(x => x.ActivityList).ToList();

            activities = (from item in activities where item.Deleted == BaseStatusDeleted.None select item).ToList();

            IList<Int64> activitiesId = (from item in activities select item.Id).ToList();

            Int32 currentUserId = serviceEP.Context.UserContext.CurrentUserID;
            Int32 currentRoleId = (from item in manager.GetIQ<LazySubscription>() where item.IdPerson == currentUserId && item.IdCommunity == dpu.CommunityId select item.IdRole).FirstOrDefault();
            String ipaddress = serviceEP.Context.UserContext.IpAddress;
            String proxyaddress = serviceEP.Context.UserContext.ProxyIpAddress;



            foreach (var iduser in idUsers)
            {
                dtoUserStatistic dus = new dtoUserStatistic();
                dus.UserId = iduser;
                dus.User = manager.GetLitePerson(iduser);
                dus.CRoleId = (from item in manager.GetIQ<LazySubscription>() where item.IdPerson == iduser && item.IdCommunity == dpu.CommunityId select item.IdRole).FirstOrDefault();

                foreach (var item in activitiesId)
                {

                    dtoActivityStatistic das = GetDtoActivityStatistic_ByUser(item, dpu.CommunityId, iduser, currentUserId, currentRoleId, ipaddress, proxyaddress, viewStatBefore);

                    if (das != null)
                    {

                        dtoUserActivityStatistic duas = new dtoUserActivityStatistic(das, iduser);

                        dus.Activities.Add(duas);
                    }
                }

            }
            

            return dpu;
        }

        private dtoItemWithStatistic SubGetOwnerEduPathStatisticStructure(long PathId, int userId, int CRoleId,bool getPermission, DateTime viewStatBefore)
        {
            dtoItemWithStatistic dtoEpStructure;

            PathStatistic oPathStat = GetPathStatistic(PathId, userId,viewStatBefore);
            Path oPath = manager.Get<Path>(PathId);

            if (oPathStat != null && oPathStat.Deleted == BaseStatusDeleted.None)
            {
                
                dtoEpStructure = new dtoItemWithStatistic()
                {
                    Id = PathId,
                    Name = oPath.Name,
                    Weight=oPath.Weight,
                    MinCompletion = oPath.MinCompletion,
                    MinMark = oPath.MinMark,
                    Completion = oPathStat.Completion,
                    Mark = oPathStat.Mark,  
                   Status=oPath.Status,
                   canUpdate= getPermission ? serviceEP.AdminCanUpdate(PathId,ItemType.Path,userId,CRoleId) : false,
                    TotMandatory =serviceEP.CheckEpType(oPath.EPType,EPType.Auto) ? serviceEP.GetActiveMandatoryActivitiesCount_byPathId(PathId,userId,CRoleId)  : serviceEP.GetActiveMandatoryUnitCount(PathId, userId, CRoleId),
                    OnlyCompletedMandatory = oPathStat.MandatoryCompletedOnlyUnitCount,
                    OnlyPassedMandatory = oPathStat.MandatoryPassedOnlyUnitCount,
                    CompletedPassedMandatory=oPathStat.MandatoryPassedCompletedUnitCount,
                    StatusStat =  GetMainStatusStatistic( oPathStat.Status) ,
                    Children = GetOwnerUnitsStatistic(PathId,  userId, CRoleId,getPermission,serviceEP.CheckEpType(oPath.EPType,EPType.Auto),viewStatBefore)
                };
            }
            else
            {
                dtoEpStructure = new dtoItemWithStatistic()
                {
                    Id = PathId,
                    Name = oPath.Name,
                    Weight=oPath.Weight,
                    MinCompletion = oPath.MinCompletion,
                    MinMark = oPath.MinMark,
                    TotMandatory = serviceEP.CheckEpType(oPath.EPType, EPType.Auto) ? serviceEP.GetActiveMandatoryActivitiesCount_byPathId(PathId, userId, CRoleId) : serviceEP.GetActiveMandatoryUnitCount(PathId, userId, CRoleId),
                    canUpdate = getPermission ? serviceEP.AdminCanUpdate(PathId, ItemType.Path, userId, CRoleId) : false,
                    Children = GetOwnerUnitsStatistic(PathId, userId, CRoleId, getPermission, serviceEP.CheckEpType(oPath.EPType, EPType.Auto),viewStatBefore)
                };
            }
            return dtoEpStructure;
        }

        /// <summary>
        /// Get single user stat
        /// </summary>
        /// <param name="PathId">parent path Id</param>
        /// <param name="pathStatId">parent pathstat Id->if pathstat don't exist set parameter=-1</param>
        /// <param name="userId"></param>
        /// <param name="CRoleId"></param>
        /// <returns></returns>
        public IList<dtoItemWithStatistic> GetOwnerUnitsStatistic(long PathId, int userId, int CRoleId, bool getPermission,bool isAutoEp,DateTime viewStatBefore)
        {
            IList<Unit> ActiveUnit = serviceEP.GetActiveUnits(manager.Get<Path>(PathId).UnitList, userId, CRoleId, false, false).Where(u=>!serviceEP.CheckStatus(u.Status,Status.Text)).ToList();
            IList<UnitStatistic> ExistUnitsStat;
            if (isAutoEp)
            {
                ExistUnitsStat = new List<UnitStatistic>();                   
            }
            else
            {
                ExistUnitsStat = GetActiveUnitStat_ByPathId_Insert(PathId, userId, viewStatBefore);
            }

            return (from unit in ActiveUnit
                    join stat in ExistUnitsStat
                    on unit.Id equals stat.Unit.Id into result
                    from stat in result.DefaultIfEmpty()
                    orderby unit.DisplayOrder
                    select new dtoItemWithStatistic()
                    {
                        Id = unit.Id,
                        Name = unit.Name,
                        MinCompletion = unit.MinCompletion,
                        MinMark = unit.MinMark,
                        Weight=unit.Weight,
                        Status = unit.Status,
                        canUpdate = getPermission ? serviceEP.AdminCanUpdate(unit.Id, ItemType.Unit, userId, CRoleId) : false,
                        Completion = stat == null ? (Int64)0 : stat.Completion,
                        Mark = stat == null ? (Int16)0 : stat.Mark,

                        TotMandatory = serviceEP.GetActiveMandatoryActivitiesCount(unit.Id, userId, CRoleId),
                        OnlyCompletedMandatory = stat == null ? (Int16)0 : stat.MandatoryCompletedOnlyActivityCount,
                        OnlyPassedMandatory = stat == null ? (Int16)0 : stat.MandatoryPassedOnlyActivityCount,
                        CompletedPassedMandatory = stat == null ? (Int16)0 : stat.MandatoryPassedCompletedActivityCount,
                        StatusStat = stat != null ? GetMainStatusStatistic(stat.Status) : StatusStatistic.None,
                        Children =  GetOwnerActivitiesStatistic(unit.Id, userId, CRoleId,getPermission,viewStatBefore)
                    }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitId">parent unit id</param>
        /// <param name="unitStatId">parent unitstat Id->if unitstat don't exist set parameter=-1</param>
        /// <param name="userId"></param>
        /// <param name="CRoleId"></param>
        /// <returns></returns>
        public IList<dtoItemWithStatistic> GetOwnerActivitiesStatistic(long unitId, int userId, int CRoleId, bool getPermission, DateTime viewStatBefore)
        {
            IList<Activity> ActiveActivities = serviceEP.GetActiveActivities(manager.Get<Unit>(unitId).ActivityList, userId, CRoleId, false, false).Where(a=>!serviceEP.CheckStatus(a.Status,Status.Text)).ToList();

            IList<ActivityStatistic> ExistActivitiesStat=GetActiveActStat_ByUnitId_Insert(unitId,userId,viewStatBefore);
            foreach (ActivityStatistic item in ExistActivitiesStat)
            {
                Console.WriteLine(item.Status);
            }

            return (from act in ActiveActivities
                    join stat in ExistActivitiesStat
                    on act.Id equals stat.Activity.Id into result
                    from stat in result.DefaultIfEmpty()
                    orderby act.DisplayOrder
                    select new dtoItemWithStatistic()
                    {
                        Id = act.Id,    
                        Name = act.Name,
                        MinCompletion = act.MinCompletion,
                        MinMark = act.MinMark,
                        Weight=act.Weight,
                        Status = act.Status,
                        canUpdate = getPermission ? serviceEP.AdminCanUpdate(act.Id, ItemType.Activity, userId, CRoleId) : false,
                        Completion = stat == null ? (Int64)0 : stat.Completion,
                        Mark = stat == null ? (Int16)0 : stat.Mark,
                        TotMandatory = serviceEP.GetActiveMandatorySubActivitiesCount(act.SubActivityList, userId, CRoleId),
                        OnlyCompletedMandatory = stat == null ? (Int16)0 : stat.MandatoryCompletedOnlySubActivityCount,
                        OnlyPassedMandatory = stat == null ? (Int16)0 : stat.MandatoryPassedOnlySubActivityCount,
                        CompletedPassedMandatory = stat == null ? (Int16)0 : stat.MandatoryPassedCompletedSubActivityCount,
                        StatusStat = stat != null ? GetMainStatusStatistic(stat.Status) : StatusStatistic.None,
                        //Children = null //GetOwnerSubActivitiesStatistic(act.Id,userId,CRoleId,getPermission,viewStatBefore)
                        Children = GetOwnerSubActivitiesStatistic(act.Id, userId, CRoleId, getPermission, viewStatBefore)
                    }).ToList();
        }

        public IList<dtoItemWithStatistic> GetOwnerSubActivitiesStatistic(long actId, int userId, int CRoleId, bool getPermission, DateTime viewStatBefore)
        {
            IList<SubActivity> ActiveSubActivities = serviceEP.GetActiveSubActivities(manager.Get<Activity>(actId).SubActivityList).Where(a => !serviceEP.CheckStatus(a.Status, Status.Text)).ToList();

            IList<SubActivityStatistic> ExistActivitiesStat = GetActiveSubActStat_ByActId_Insert(actId, userId, viewStatBefore);
            //foreach (SubActivityStatistic item in ExistActivitiesStat)
            //{
            //    Console.WriteLine(item.Status);
            //}

            return (from act in ActiveSubActivities
                    join stat in ExistActivitiesStat
                    on act.Id equals stat.SubActivity.Id into result
                    from stat in result.DefaultIfEmpty()
                    orderby act.DisplayOrder
                    select new dtoItemWithStatistic()
                    {
                        Id = act.Id,
                        Name = act.Name,
                        //MinCompletion = act.MinCompletion,
                        //MinMark = act.MinMark,
                        Weight = act.Weight,
                        Status = act.Status,
                        canUpdate = getPermission ? serviceEP.AdminCanUpdate(act.Id, ItemType.Activity, userId, CRoleId) : false,
                        Completion = stat == null ? (Int64)0 : stat.Completion,
                        Mark = stat == null ? (Int16)0 : stat.Mark,
                        //TotMandatory = serviceEP.GetActiveMandatorySubActivitiesCount(act.SubActivityList, userId, CRoleId),
                        //OnlyCompletedMandatory = stat == null ? (Int16)0 : stat.MandatoryCompletedOnlySubActivityCount,
                        //OnlyPassedMandatory = stat == null ? (Int16)0 : stat.MandatoryPassedOnlySubActivityCount,
                        //CompletedPassedMandatory = stat == null ? (Int16)0 : stat.MandatoryPassedCompletedSubActivityCount,
                        StatusStat = stat != null ? GetMainStatusStatistic(stat.Status) : StatusStatistic.None,
                        Children = null,
                    }).ToList();
        }

        public dtoItemWithStatistic GetParticipantEduPathStatisticStructure(long pathId, int supervisorId, int supervisorCRoleId, int participantId, DateTime viewStatBefore)
        {
            dtoItemWithStatistic dtoEpStructure = null;
            int participantCroleId = manager.GetActiveSubscriptionIdRole(participantId, serviceEP.GetPathIdCommunity(pathId));
            PermissionEP supervisorPermission;
            supervisorPermission = serviceEP.GetUserPermission_ByPath(pathId, supervisorId, supervisorCRoleId);
            if (supervisorPermission.ViewUserStat)
            {

                dtoEpStructure = GetOwnerEduPathStatisticStructure(pathId, participantId, participantCroleId,viewStatBefore);
            }
            else
            {
                Path oPath = manager.Get<Path>(pathId);
               
                dtoEpStructure = new dtoItemWithStatistic()
                                   {
                                       Id = pathId,
                                       Name = oPath.Name,
                                       MinCompletion = oPath.MinCompletion,
                                       MinMark = oPath.MinMark,
                                       Weight=oPath.Weight,
                                       Status=oPath.Status,
                                       canUpdate=serviceEP.AdminCanUpdate(pathId,ItemType.Path,supervisorId,supervisorCRoleId),
                                       Children = GetParticipantUnitStat(pathId, supervisorId, supervisorCRoleId, participantId, participantCroleId,viewStatBefore, serviceEP.CheckEpType(oPath.EPType,EPType.Auto))
                                   };
            }

            return dtoEpStructure;
        }


        #region Unit sub to verify if supervisor can view statistic

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="UnitsIdWithPermission">Id delle unit dove ho i permessi per vedere le statistiche</param>
        /// <param name="unitId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetUnitMandatoryPassedOnlyActivityCount(IList<long> UnitsIdWithPermission, long unitId, UnitStatistic oStat)
        {
            if (UnitsIdWithPermission.Contains(unitId))
            {
                if (oStat != null)
                {
                    return oStat.MandatoryPassedOnlyActivityCount;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="UnitsIdWithPermission">Id delle unit dove ho i permessi per vedere le statistiche</param>
        /// <param name="unitId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetUnitMandatoryCompletedOnlyActivityCount(IList<long> UnitsIdWithPermission, long unitId, UnitStatistic oStat)
        {
            if (UnitsIdWithPermission.Contains(unitId))
            {
                if (oStat != null)
                {
                    return oStat.MandatoryCompletedOnlyActivityCount;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="UnitsIdWithPermission">Id delle unit dove ho i permessi per vedere le statistiche</param>
        /// <param name="unitId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetUnitMandatoryPassedCompletedActivityCount(IList<long> UnitsIdWithPermission, long unitId, UnitStatistic oStat)
        {
            if (UnitsIdWithPermission.Contains(unitId))
            {
                if (oStat != null)
                {
                    return oStat.MandatoryPassedCompletedActivityCount;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }


        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="UnitsIdWithPermission">Id delle unit dove ho i permessi per vedere le statistiche</param>
        /// <param name="unitId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetUnitMark(IList<long> UnitsIdWithPermission, long unitId, UnitStatistic oStat)
        {
            if (UnitsIdWithPermission.Contains(unitId))
            {
                if (oStat != null)
                {
                    return oStat.Mark;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="UnitsIdWithPermission">Id delle unit dove ho i permessi per vedere le statistiche</param>
        /// <param name="unitId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int64 GetUnitCompletion(IList<long> UnitsIdWithPermission, long unitId, UnitStatistic oStat)
        {
            if (UnitsIdWithPermission.Contains(unitId))
            {
                if (oStat != null)
                {
                    return oStat.Completion;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        #endregion

        private IList<dtoItemWithStatistic> GetParticipantUnitStat(long pathId, int supervisorId, int supervisorCRoleId, int participantId, int participantCrole, DateTime viewStatBefore, bool isAutoEp)
        {

            IList<Unit> ActiveUnit = serviceEP.GetActiveUnits(manager.Get<Path>(pathId).UnitList, participantId, participantCrole, false, false).Where(u=>!serviceEP.CheckStatus(u.Status,Status.Text)).ToList();
            IList<long> UnitEvaluate = (from item in ActiveUnit
                                        where !serviceEP.CheckStatus(item.Status,Status.Text) && serviceEP.GetUserPermission_ByUnit(item.Id, supervisorId, supervisorCRoleId).ViewUserStat
                                        select item.Id).ToList();
            IList<UnitStatistic> ExistUnitsStat;

            if (isAutoEp)
            {
                ExistUnitsStat = new List<UnitStatistic>();
            }
            else
            {
                ExistUnitsStat = GetActiveUnitStat_ByPathId_Insert(pathId, participantId, viewStatBefore);
            }
         

            return (from unit in ActiveUnit
                    join stat in ExistUnitsStat
                    on unit.Id equals stat.Unit.Id into result
                    from stat in result.DefaultIfEmpty()
                    orderby stat.Unit.DisplayOrder
                    select new dtoItemWithStatistic()
                    {
                        Id = unit.Id,
                        Name = unit.Name,
                        MinCompletion = unit.MinCompletion,
                        MinMark = unit.MinMark,
                        Weight=unit.Weight,
                        Status = unit.Status,
                        canUpdate = serviceEP.AdminCanUpdate(unit.Id, ItemType.Unit, supervisorId, supervisorCRoleId),
                        Completion = GetUnitCompletion(UnitEvaluate, unit.Id, stat),
                        Mark = GetUnitMark(UnitEvaluate, unit.Id, stat),
                        TotMandatory = serviceEP.GetActiveMandatoryActivitiesCount(unit.Id, participantId, participantCrole),
                        OnlyCompletedMandatory = GetUnitMandatoryCompletedOnlyActivityCount(UnitEvaluate, unit.Id, stat),
                        OnlyPassedMandatory = GetUnitMandatoryPassedOnlyActivityCount(UnitEvaluate, unit.Id, stat),
                        CompletedPassedMandatory = GetUnitMandatoryPassedCompletedActivityCount(UnitEvaluate, unit.Id, stat),
                        StatusStat = stat != null ? GetMainStatusStatistic(stat.Status) : StatusStatistic.None,
                        Children = GetParticipantActivityStat(unit.Id, supervisorId, supervisorCRoleId, participantId, participantCrole,viewStatBefore)
                    }).ToList();
        }

        #region Activity sub to verify if supervisor can view statistic

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="ActivitiesIdWithPermission">Id delle Activity dove ho i permessi per vedere le statistiche</param>
        /// <param name="ActivityId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetActivityMandatoryPassedOnlySubActivityCount(IList<long> ActivitiesIdWithPermission, long activityId, ActivityStatistic oStat)
        {
            if (ActivitiesIdWithPermission.Contains(activityId))
            {
                if (oStat != null)
                {
                    return oStat.MandatoryPassedOnlySubActivityCount;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="ActivitiesIdWithPermission">Id delle activity dove ho i permessi per vedere le statistiche</param>
        /// <param name="activityId">id dell'activity interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetActivityMandatoryCompletedOnlySubActivityCount(IList<long> ActivitiesIdWithPermission, long activityId, ActivityStatistic oStat)
        {
            if (ActivitiesIdWithPermission.Contains(activityId))
            {
                if (oStat != null)
                {
                    return oStat.MandatoryCompletedOnlySubActivityCount;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="ActivitiesIdWithPermission">Id delle activity dove ho i permessi per vedere le statistiche</param>
        /// <param name="activityId">id dell'activity interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetActivityMandatoryPassedCompletedSubActivityCount(IList<long> ActivitiesIdWithPermission, long activityId, ActivityStatistic oStat)
        {
            if (ActivitiesIdWithPermission.Contains(activityId))
            {
                if (oStat != null)
                {
                    return oStat.MandatoryPassedCompletedSubActivityCount;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }


        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="ActivitiesIdWithPermission">Id delle activity dove ho i permessi per vedere le statistiche</param>
        /// <param name="activityId">id dell'activity interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int16 GetActivityMark(IList<long> ActivitiesIdWithPermission, long activityId, ActivityStatistic oStat)
        {
            if (ActivitiesIdWithPermission.Contains(activityId))
            {
                if (oStat != null)
                {
                    return oStat.Mark;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// usata per verificare se l'utente ha i permessi per vedere le statistiche e in caso affermativo restituisce il valore indicato
        /// </summary>
        /// <param name="UnitsIdWithPermission">Id delle unit dove ho i permessi per vedere le statistiche</param>
        /// <param name="unitId">id dell'unità interessata</param>
        /// <param name="oStat">eventuale statistica relativa ad un participante (può essere==null)</param>
        /// <returns></returns>
        private Int64 GetActivityCompletion(IList<long> UnitsIdWithPermission, long unitId, ActivityStatistic oStat)
        {
            if (UnitsIdWithPermission.Contains(unitId))
            {
                if (oStat != null)
                {
                    return oStat.Completion;
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }

        #endregion


        private IList<dtoItemWithStatistic> GetParticipantActivityStat(long unitId, int supervisorId, int supervisorCRoleId, int participantId, int participantCrole, DateTime viewStatBefore)
        {
            IList<Activity> ActiveActivities = serviceEP.GetActiveActivities(manager.Get<Unit>(unitId).ActivityList, participantId, participantCrole, false, false).Where(u => !serviceEP.CheckStatus(u.Status, Status.Text)).ToList(); 
            IList<long> ActivitiesEvaluate = (from item in ActiveActivities
                                              where !serviceEP.CheckStatus(item.Status, Status.Text) && serviceEP.GetUserPermission_ByActivity(item.Id, supervisorId, supervisorCRoleId).ViewUserStat
                                              select item.Id).ToList();
            IList<ActivityStatistic> ExistActivitiesStat = GetActiveActStat_ByUnitId_Insert(unitId, participantId, viewStatBefore);
     
            return (from act in ActiveActivities
                    join stat in ExistActivitiesStat
                    on act.Id equals stat.Activity.Id into result
                    from stat in result.DefaultIfEmpty()
                    orderby stat.Activity.DisplayOrder
                    select new dtoItemWithStatistic()
                    {
                        Id = act.Id,
                        Name = act.Name,
                        MinCompletion = act.MinCompletion,
                        MinMark = act.MinMark,
                        Weight=act.Weight,
                        Status = act.Status,
                        canUpdate = serviceEP.AdminCanUpdate(act.Id, ItemType.Activity, supervisorId, supervisorCRoleId),
                        Completion = GetActivityCompletion(ActivitiesEvaluate, act.Id, stat),
                        Mark = GetActivityMark(ActivitiesEvaluate, act.Id, stat),
                        TotMandatory = serviceEP.GetActiveMandatorySubActivitiesCount(act.SubActivityList, participantId, participantCrole),
                        OnlyCompletedMandatory = GetActivityMandatoryCompletedOnlySubActivityCount(ActivitiesEvaluate, act.Id, stat),
                        OnlyPassedMandatory = GetActivityMandatoryPassedOnlySubActivityCount(ActivitiesEvaluate, act.Id, stat),
                        CompletedPassedMandatory = GetActivityMandatoryPassedCompletedSubActivityCount(ActivitiesEvaluate, act.Id, stat),
                        StatusStat = stat != null ? GetMainStatusStatistic(stat.Status) : StatusStatistic.None,
                        Children = GetParticipantSubActivityStat(act.Id,supervisorId,supervisorCRoleId ,participantId,participantCrole,viewStatBefore)
                    }).ToList();

        }

        private IList<dtoItemWithStatistic> GetParticipantSubActivityStat(long activityId, int supervisorId, int supervisorCRoleId, int participantId, int participantCrole, DateTime viewStatBefore)
        {
            IList<SubActivity> ActiveSubActivities = serviceEP.GetActiveSubActivities(manager.Get<Activity>(activityId).SubActivityList).Where(u => !serviceEP.CheckStatus(u.Status, Status.Text)).ToList();
            IList<long> SubActivitiesEvaluate = (from item in ActiveSubActivities
                                              where !serviceEP.CheckStatus(item.Status, Status.Text) && serviceEP.GetUserPermission_ByActivity(item.Id, supervisorId, supervisorCRoleId).ViewUserStat
                                              select item.Id).ToList();
            IList<SubActivityStatistic> ExistSubActivitiesStat = GetActiveSubActStat_ByActId_Insert(activityId, participantId, viewStatBefore);

            return (from subact in ActiveSubActivities
                    join stat in ExistSubActivitiesStat
                    on subact.Id equals stat.SubActivity.Id into result
                    from stat in result.DefaultIfEmpty()
                    orderby stat.SubActivity.DisplayOrder
                    select new dtoItemWithStatistic()
                    {
                        Id = subact.Id,
                        Name = subact.Name,
                        //MinCompletion = act.MinCompletion,
                        //MinMark = act.MinMark,
                        Weight = subact.Weight,
                        Status = subact.Status,
                        canUpdate = serviceEP.AdminCanUpdate(subact.Id, ItemType.SubActivity, supervisorId, supervisorCRoleId),
                        //Completion = GetActivityCompletion(SubActivitiesEvaluate, act.Id, stat),
                        //Mark = GetActivityMark(SubActivitiesEvaluate, act.Id, stat),
                        //TotMandatory = serviceEP.GetActiveMandatorySubActivitiesCount(act.SubActivityList, participantId, participantCrole),
                        //OnlyCompletedMandatory = GetActivityMandatoryCompletedOnlySubActivityCount(ActivitiesEvaluate, act.Id, stat),
                        //OnlyPassedMandatory = GetActivityMandatoryPassedOnlySubActivityCount(ActivitiesEvaluate, act.Id, stat),
                        //CompletedPassedMandatory = GetActivityMandatoryPassedCompletedSubActivityCount(ActivitiesEvaluate, act.Id, stat),
                        StatusStat = stat != null ? GetMainStatusStatistic(stat.Status) : StatusStatistic.None,
                        Children = null
                    }).ToList();
        }

        private StatusStatistic GetMainStatusStatistic(StatusStatistic Status)
        {
            if (CheckStatusStatistic(Status, StatusStatistic.CompletedPassed))
            {
                return StatusStatistic.CompletedPassed;
            }
            else if (CheckStatusStatistic(Status, StatusStatistic.Passed))
            {
                return StatusStatistic.Passed;
            }
            else if (CheckStatusStatistic(Status, StatusStatistic.Completed))
            {
                return StatusStatistic.Completed;
            }
            else if (CheckStatusStatistic(Status, StatusStatistic.Started))
            {
                return StatusStatistic.Started;
            }
            else if (CheckStatusStatistic(Status, StatusStatistic.Browsed))
            {
                return StatusStatistic.Browsed;
            }

            return StatusStatistic.None;
        }

        #endregion


        #region evaluate

        public int GetSubActUsersStatToEvalCount(long subActId)
        {
            return (from stat in manager.GetAll<SubActivityStatistic>(s => s.SubActivity.Id == subActId && s.Deleted == BaseStatusDeleted.None).ToList()
                    where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed)
                    orderby stat.Person.SurnameAndName
                    select stat.Id).Count();
        }

        public List<long> GetSubActUsersStatIdsToEval(long subActId, EvaluationFilter filter)
        {
            switch (filter)
            {

                case EvaluationFilter.Evaluated:
                    return (from stat in manager.GetAll<SubActivityStatistic>(s => s.SubActivity.Id == subActId && s.Deleted == BaseStatusDeleted.None).ToList()
                                         where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed)
                                         orderby stat.Person.SurnameAndName
                                         select stat.Id).ToList();
                   
                case EvaluationFilter.NotEvaluated:
                    return (from stat in manager.GetAll<SubActivityStatistic>(s => s.SubActivity.Id == subActId && s.Deleted == BaseStatusDeleted.None).ToList()
                                         where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed)
                                         orderby stat.Person.SurnameAndName
                                         select stat.Id).ToList();
                   
                default:
                    return new List<long>();               
            }        
        }



        public dtoSubActListUserToEval GetSubActUsersStatToEval(long subActId, IList<long> ListSubActIds)
        {

            SubActivity oSubAct = manager.Get<SubActivity>(subActId);
            dtoSubActListUserToEval dtoStats = new dtoSubActListUserToEval();
            dtoStats.IdSubActivity = subActId;
            dtoStats.ModuleLink = oSubAct.ModuleLink;
            dtoStats.Name = oSubAct.Description;
            dtoStats.Status = oSubAct.Status;
            dtoStats.ContentType = oSubAct.ContentType;

            dtoStats.userStat = (from stat in manager.GetAll<SubActivityStatistic>(s => s.SubActivity.Id == subActId && s.Deleted == BaseStatusDeleted.None && ListSubActIds.Contains(s.Id))
                                 select new dtoUserStatToEvaluate()
                                 {
                                     UserId = stat.Person.Id,
                                     TaxCode = stat.Person.TaxCode,
                                     SurnameAndName = stat.Person.Surname + " " + stat.Person.Name,
                                     Completion = stat.Completion,
                                     Mark = stat.Mark,
                                     StatusStat = GetMainStatusStatistic(stat.Status),
                                     StatId = stat.Id
                                 }).ToList();
            return dtoStats;
        }

        #endregion


        #region get global statistic (not personalized item)

        public dtoEpGlobalStat GetGlobalEpStats(long idPath, Int32 idCommunity, Int32 adminId, Int32 adminCrole, bool isEvaluable, DateTime viewStatBefore, Boolean all=false )
        {
            Path path = manager.Get<Path>(idPath);
            if (path == null)
                return null;

            //long participantsCount = (long)serviceEP.ServiceAssignments.GetAllParticipantInPathCount(idPath, idCommunity, ItemType.Path, viewStatBefore, all);
            List<Int32> idUsers = serviceEP.ServiceAssignments.GetAllParticipantIdInPath(idPath, idCommunity, ItemType.Path, viewStatBefore, all);
            long count = idUsers.Count();
            List<liteBaseStatistic> statistics = GetAllPathStatistics(idPath, viewStatBefore);
            List<liteBaseStatistic> usedStatistics = statistics.Where(s => idUsers.Contains(s.IdPerson)).ToList();

            Boolean viewAllStatistics = serviceEP.GetUserPermission_ByPath(idPath, adminId, adminCrole).ViewUserStat;

            dtoEpGlobalStat dtoPathStat = new dtoEpGlobalStat()
            {
                itemId = idPath,
                itemName = path.Name,
                Weight = path.Weight,
                status = path.Status,
                startedCount = viewAllStatistics ? usedStatistics.Where(s=> s.Discriminator== StatisticDiscriminator.Path && CheckStatusStatistic(s.Status, StatusStatistic.Started)&& !CheckStatusStatistic(s.Status, StatusStatistic.Passed)&& !CheckStatusStatistic(s.Status, StatusStatistic.Completed) ).Count(): 0,
                completedCount = viewAllStatistics ? usedStatistics.Where(s => s.Discriminator == StatisticDiscriminator.Path && CheckStatusStatistic(s.Status, StatusStatistic.Completed) && !CheckStatusStatistic(s.Status, StatusStatistic.Passed)).Count() : 0,
                passedCount = viewAllStatistics ? usedStatistics.Where(s => s.Discriminator == StatisticDiscriminator.Path && CheckStatusStatistic(s.Status, StatusStatistic.Passed) && !CheckStatusStatistic(s.Status, StatusStatistic.Completed)).Count() : 0,
                compPassedCount = viewAllStatistics ? usedStatistics.Where(s => s.Discriminator == StatisticDiscriminator.Path && CheckStatusStatistic(s.Status, StatusStatistic.CompletedPassed)).Count() : 0,
                userCount = viewAllStatistics ? count : (long)(-1),
                childrenStat = GetGlobalUnitsStatistics(idPath, count, adminId, adminCrole, viewAllStatistics, isEvaluable, usedStatistics.Where(s=> s.Discriminator != StatisticDiscriminator.Path))
            };
            return dtoPathStat;

        }
        private IList<dtoUnitGlobalStat> GetGlobalUnitsStatistics(long idPath, long count, int adminId, int adminCrole, bool viewAllStatistics, bool isEvaluable, IEnumerable<liteBaseStatistic> statistics)
        {
            var query = (from u in manager.GetIQ<Unit>() where u.ParentPath.Id == idPath && u.Deleted == BaseStatusDeleted.None
                         select new { IdUnit = u.Id, Name = u.Name, Status = u.Status, DisplayOrder = u.DisplayOrder,Weight = u.Weight}).
                         ToList().Where(i => !serviceEP.CheckStatus(i.Status, Status.Draft) && !serviceEP.CheckStatus(i.Status, Status.Text)).ToList();

            List<dtoUnitGlobalStat> results = (from u in query
                    orderby u.DisplayOrder
                    select new dtoUnitGlobalStat()
                    {
                        itemId = u.IdUnit,
                        itemName = u.Name,
                        Weight = u.Weight,
                        status = u.Status,
                        userCount = viewAllStatistics || serviceEP.GetUserPermission_ByUnit(u.IdUnit, adminId, adminCrole).ViewUserStat ? count : -1,
                        childrenStat = GetGlobalActivitiesStatistics(u.IdUnit, count, adminId, adminCrole, (viewAllStatistics ? viewAllStatistics :serviceEP.GetUserPermission_ByUnit(u.IdUnit, adminId, adminCrole).ViewUserStat), isEvaluable, statistics.Where(s => s.Discriminator != StatisticDiscriminator.Unit))
                    }).ToList();
           
            return results;
        }

        private IList<dtoActivityGlobalStat> GetGlobalActivitiesStatistics(long idUnit, long count, int adminId, int adminCrole, bool viewUnitStatistics, bool isEvaluable, IEnumerable<liteBaseStatistic> statistics)
        {
            var query = (from a in manager.GetIQ<Activity>()
                         where a.ParentUnit.Id == idUnit && a.Deleted == BaseStatusDeleted.None
                         select new { IdActivity = a.Id, Name = a.Name, Status = a.Status, DisplayOrder = a.DisplayOrder, Weight = a.Weight }).
                       ToList().Where(i => !serviceEP.CheckStatus(i.Status, Status.Draft) && !serviceEP.CheckStatus(i.Status, Status.Text)).ToList();

            return (from a in query

                    orderby a.DisplayOrder
                    select new dtoActivityGlobalStat()
                    {
                        parentId = idUnit,
                        itemId = a.IdActivity,
                        itemName = a.Name,
                        Weight = a.Weight,
                        status = a.Status,
                        startedCount = statistics.Where(s => s.IdActivity == a.IdActivity && s.Discriminator == StatisticDiscriminator.Activity && CheckStatusStatistic(s.Status, StatusStatistic.Started) && !CheckStatusStatistic(s.Status, StatusStatistic.Passed) && !CheckStatusStatistic(s.Status, StatusStatistic.Completed)).Count(),
                        completedCount = statistics.Where(s => s.IdActivity == a.IdActivity && s.Discriminator == StatisticDiscriminator.Activity && CheckStatusStatistic(s.Status, StatusStatistic.Completed) && !CheckStatusStatistic(s.Status, StatusStatistic.Passed)).Count(),
                        passedCount = statistics.Where(s => s.IdActivity == a.IdActivity && s.Discriminator == StatisticDiscriminator.Activity && CheckStatusStatistic(s.Status, StatusStatistic.Passed) && !CheckStatusStatistic(s.Status, StatusStatistic.Completed)).Count(),
                        compPassedCount = statistics.Where(s => s.IdActivity == a.IdActivity && s.Discriminator == StatisticDiscriminator.Activity && CheckStatusStatistic(s.Status, StatusStatistic.CompletedPassed)).Count(),
                        userCount = viewUnitStatistics || serviceEP.GetUserPermission_ByActivity(a.IdActivity, adminId, adminCrole).ViewUserStat ? count : -1,
                        childrenStat = GetGlobalSubActivitiesStatistics(a.IdActivity, count, adminId, adminCrole, (viewUnitStatistics ? viewUnitStatistics : serviceEP.GetUserPermission_ByActivity(a.IdActivity, adminId, adminCrole).ViewUserStat), isEvaluable, statistics.Where(s => s.IdActivity == a.IdActivity && s.Discriminator == StatisticDiscriminator.SubActivity))
                    }).ToList();
        }

        public IList<dtoSubActGlobalStat> GetGlobalSubActivitiesStatistics(long idActivity, long count, int adminId, int adminCrole, bool viewActivityStatistics, bool isEvaluable, IEnumerable<liteBaseStatistic> statistics)
        {
            var query = (from a in manager.GetIQ<SubActivity>()
                         where a.ParentActivity.Id == idActivity && a.Deleted == BaseStatusDeleted.None
                         select a).ToList().Where(i => !serviceEP.CheckStatus(i.Status, Status.Draft) && !serviceEP.CheckStatus(i.Status, Status.Text)).ToList();
          
            Boolean canEvaluate = (isEvaluable ? serviceEP.GetUserPermission_ByActivity(idActivity, adminId, adminCrole).Evaluate : false );
            return (from sub in query
                    orderby sub.DisplayOrder
                    select new dtoSubActGlobalStat()
                    {
                        parentId = idActivity,
                        itemId = sub.Id,
                        itemName = sub.Description,
                        ContentType = sub.ContentType,
                        ModuleLink = sub.ModuleLink,
                        Weight = sub.Weight,
                        status = sub.Status,
                        startedCount = statistics.Where(s => s.IdSubActivity == sub.Id && s.Discriminator == StatisticDiscriminator.SubActivity && CheckStatusStatistic(s.Status, StatusStatistic.Started) && !CheckStatusStatistic(s.Status, StatusStatistic.Passed) && !CheckStatusStatistic(s.Status, StatusStatistic.Completed)).Count(),
                        completedCount = statistics.Where(s => s.IdSubActivity == sub.Id && s.Discriminator == StatisticDiscriminator.SubActivity && CheckStatusStatistic(s.Status, StatusStatistic.Completed) && !CheckStatusStatistic(s.Status, StatusStatistic.Passed)).Count(),
                        passedCount = statistics.Where(s => s.IdSubActivity == sub.Id && s.Discriminator == StatisticDiscriminator.SubActivity && CheckStatusStatistic(s.Status, StatusStatistic.Passed) && !CheckStatusStatistic(s.Status, StatusStatistic.Completed)).Count(),
                        compPassedCount = statistics.Where(s => s.IdSubActivity == sub.Id && s.Discriminator == StatisticDiscriminator.SubActivity && CheckStatusStatistic(s.Status, StatusStatistic.CompletedPassed)).Count(),
                        userCount = viewActivityStatistics ? count: -1,
                        canEvaluate = canEvaluate && sub.CheckStatus(Status.EvaluableAnalog),
                        SubActivity = new dtoSubActivity(sub)
                    }).ToList();
            
        }



        public dtoEpGlobalStat GetGlobalEpStatsForced(long pathId, Int32 commId, Int32 adminId,  bool isEvaluable, DateTime viewStatBefore)
        {
            Path oPath = manager.Get<Path>(pathId);

            Int16 participantsCount = (Int16)serviceEP.ServiceAssignments.GetAllParticipantInPathCount(pathId, commId, ItemType.Path, viewStatBefore);

            IList<PathStatistic> pathStats = GetUsersPathStat_Insert(pathId, viewStatBefore);

            bool canViewAllStat = true;

            dtoEpGlobalStat dtoPathStat = new dtoEpGlobalStat()
            {
                itemId = pathId,
                itemName = oPath.Name,
                Weight = oPath.Weight,
                status = oPath.Status,
                startedCount = canViewAllStat ? (Int16)(from stat in pathStats where CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count() : (Int16)0,
                completedCount = canViewAllStat ? (Int16)(from stat in pathStats where CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) select stat).Count() : (Int16)0,
                passedCount = canViewAllStat ? (Int16)(from stat in pathStats where CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count() : (Int16)0,
                compPassedCount = canViewAllStat ? (Int16)(from stat in pathStats where CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) select stat).Count() : (Int16)0,
                userCount = canViewAllStat ? participantsCount : (Int16)(-1),

                //childrenStat = GetGlobalUnitsStats(pathId, participantsCount, adminId, adminCrole, canViewAllStat, isEvaluable, viewStatBefore)
            }
                ;
            return dtoPathStat;
        }

        private IList<dtoUnitGlobalStat> GetGlobalUnitsStats(long parentPathId, long participantsCount, int adminId, int adminCrole, bool adminCanView, bool isEvaluable, DateTime viewStatBefore, Boolean all=false )
        {
            Path path = manager.Get<Path>(parentPathId);
            IList<Unit> unitsList = manager.GetAll<Unit>(u => u.ParentPath.Id == parentPathId && u.Deleted == BaseStatusDeleted.None).Where<Unit>(u =>  !u.CheckStatus(Status.Draft) && !u.CheckStatus(Status.Text)).ToList();
            IList<Int64> unitsId = unitsList.Select(u => u.Id).ToList();

            IList<UnitStatistic> unitStats = GetUsersUnitsStat_Insert(parentPathId, viewStatBefore);

            IList<Int32> activeUser = serviceEP.ServiceAssignments.GetAllParticipantIdInPath(parentPathId, path.Community.Id, ItemType.Path, viewStatBefore, all);

            unitStats = (from item in unitStats where activeUser.Contains(item.Person.Id) select item).ToList();

            return (from unit in unitsList                  
                    orderby unit.DisplayOrder
                    select new dtoUnitGlobalStat()
                    {
                        itemId = unit.Id,
                        itemName = unit.Name,
                        Weight = unit.Weight,
                        status = unit.Status,
                        startedCount = (long)(from stat in unitStats where stat.Unit.Id == unit.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count(),
                        completedCount = (long)(from stat in unitStats where stat.Unit.Id == unit.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) select stat).Count(),
                        passedCount = (long)(from stat in unitStats where stat.Unit.Id == unit.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count(),
                        compPassedCount = (long)(from stat in unitStats where stat.Unit.Id == unit.Id && CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) select stat).Count(),
                        userCount = adminCanView || serviceEP.GetUserPermission_ByUnit(unit.Id, adminId, adminCrole).ViewUserStat ? participantsCount : (long)(-1),
                        childrenStat = GetGlobalActivitiesStats(unit.Id, participantsCount, adminId, adminCrole, serviceEP.GetUserPermission_ByUnit(unit.Id, adminId, adminCrole).ViewUserStat, isEvaluable, viewStatBefore,all)
                    }).ToList();           

          
        }



        private IList<dtoActivityGlobalStat> GetGlobalActivitiesStats(long parentUnitId, long participantsCount, int adminId, int adminCrole, bool adminCanView, bool isEvaluable, DateTime viewStatBefore, Boolean all = false)
        {

            Unit unit = manager.Get<Unit>(parentUnitId);
            Path path = unit.ParentPath;

            IList<Activity> actsList = manager.GetAll<Activity>(u => u.ParentUnit.Id == parentUnitId && u.Deleted == BaseStatusDeleted.None).Where<Activity>(u =>  !u.CheckStatus(Status.Draft) && !u.CheckStatus(Status.Text)).ToList();
            IList<Int64> actsId = actsList.Select(u => u.Id).ToList();

            IList<ActivityStatistic> activityStats = GetUsersActivitiesStat_Insert(parentUnitId, viewStatBefore);


            IList<Int32> activeUser = serviceEP.ServiceAssignments.GetAllParticipantIdInPath(path.Id, path.Community.Id, ItemType.Path, viewStatBefore, all);

            activityStats = (from item in activityStats where activeUser.Contains(item.Person.Id) select item).ToList();

            return (from act in actsList
                   
                    orderby act.DisplayOrder
                    select new dtoActivityGlobalStat()
                    {
                        parentId = parentUnitId,
                        itemId = act.Id,
                        itemName = act.Name,
                        Weight = act.Weight,
                        status = act.Status,
                        startedCount = (long)(from stat in activityStats where stat.Activity.Id == act.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count(),
                        completedCount = (long)(from stat in activityStats where stat.Activity.Id == act.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) select stat).Count(),
                        passedCount = (long)(from stat in activityStats where stat.Activity.Id == act.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count(),
                        compPassedCount = (long)(from stat in activityStats where stat.Activity.Id == act.Id && CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) select stat).Count(),
                        userCount = adminCanView || serviceEP.GetUserPermission_ByActivity(act.Id, adminId, adminCrole).ViewUserStat ? participantsCount : (Int16)(-1),
                        childrenStat = GetGlobalSubActivitiesStats(act.Id, participantsCount, adminId, adminCrole, (adminCanView || serviceEP.GetUserPermission_ByActivity(act.Id, adminId, adminCrole).ViewUserStat), isEvaluable, viewStatBefore,all )
                    }).ToList();

       
        }


        public IList<dtoSubActGlobalStat> GetGlobalSubActivitiesStats(long actId, long participantsCount, int adminId, int adminCrole, bool adminCanView, bool isEvaluable, DateTime viewStatBefore, Boolean all = false)
        {

            IList<SubActivity> subActsList = manager.GetAll<SubActivity>(u => u.ParentActivity.Id == actId && u.Deleted == BaseStatusDeleted.None).Where<SubActivity>(u => !u.CheckStatus(Status.Draft) ).ToList();
            IList<Int64> subActsId = subActsList.Select(u => u.Id).ToList();

            IList<SubActivityStatistic> subActStats = GetUsersSubActivitiesStat_Insert(actId, viewStatBefore);

            Activity activity = manager.Get<Activity>(actId);
            Path path = activity.Path;

            IList<Int32> activeUser = serviceEP.ServiceAssignments.GetAllParticipantIdInPath(path.Id, path.Community.Id, ItemType.Path, viewStatBefore, all);

            subActStats = (from item in subActStats where activeUser.Contains(item.Person.Id) select item).ToList();

                      
            return (from sub in subActsList
                    orderby sub.DisplayOrder
                    select new dtoSubActGlobalStat()
                    {
                        parentId = actId,
                        itemId = sub.Id,
                        itemName = sub.Description,
                        ContentType = sub.ContentType,
                        ModuleLink = sub.ModuleLink,
                        Weight = sub.Weight,
                        status = sub.Status,
                        startedCount = (long)(from stat in subActStats where stat.SubActivity.Id == sub.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Started) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count(),
                        completedCount = (long)(from stat in subActStats where stat.SubActivity.Id == sub.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Completed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Passed) select stat).Count(),
                        passedCount = (long)(from stat in subActStats where stat.SubActivity.Id == sub.Id && CheckStatusStatistic(stat.Status, StatusStatistic.Passed) && !CheckStatusStatistic(stat.Status, StatusStatistic.Completed) select stat).Count(),
                        compPassedCount = (long)(from stat in subActStats where stat.SubActivity.Id == sub.Id && CheckStatusStatistic(stat.Status, StatusStatistic.CompletedPassed) select stat).Count(),
                        userCount = adminCanView || serviceEP.GetUserPermission_ByUnit(sub.Id, adminId, adminCrole).ViewUserStat ? participantsCount : (Int16)(-1),
                        canEvaluate = isEvaluable && sub.CheckStatus(Status.EvaluableAnalog) ? serviceEP.GetUserPermission_ByActivity(actId, adminId, adminCrole).Evaluate : false,
                        SubActivity=  new dtoSubActivity(sub)

                    }).ToList();
            
        
        }

        #endregion

        #region Count Stat

        //private Int16 GetSubActStatCount(long actStatId, StatusStatistic statusToCheck)
        //{
          
        //    return (Int16)(from stat in manager.GetAll<SubActivityStatistic>(s => s.ParentStat.Id == actStatId && s.Deleted == BaseStatusDeleted.None).ToList()
        //            where CheckStatusStatistic(stat.Status, statusToCheck)
        //            select stat).Count();

        //}
        //private Int16 GetActStatCount(long unitStatId, StatusStatistic statusToCheck)
        //{

        //    return (Int16)(from stat in manager.GetAll<ActivityStatistic>(s => s.ParentStat.Id == unitStatId && s.Deleted == BaseStatusDeleted.None).ToList()
        //                   where CheckStatusStatistic(stat.Status, statusToCheck)
        //                   select stat).Count();
        //}


        //private Int16 GetUnitStatCount(long pathStatId, StatusStatistic statusToCheck)
        //{

        //    return (Int16)(from stat in manager.GetAll<UnitStatistic>(s => s.ParentStat.Id == pathStatId && s.Deleted == BaseStatusDeleted.None).ToList()
        //                   where CheckStatusStatistic(stat.Status, statusToCheck)
        //                   select stat).Count();
        //}

        private Int64 GetSubActWeightSum_byStatusStat(IList<SubActivityStatistic> Statistics, StatusStatistic statusToCheck)
        {                             
            return (Int64)(from stat in Statistics
                           where CheckStatusStatistic(stat.Status, statusToCheck) && stat.Deleted == BaseStatusDeleted.None
                           select stat.SubActivity.Weight).Sum(a=>a);
        }

        private Int64 GetActWeightSum_byStatusStat(IList<ActivityStatistic> Statistics, StatusStatistic statusToCheck)
        {
            return (Int64)(from stat in Statistics
                           where CheckStatusStatistic(stat.Status, statusToCheck) && stat.Deleted == BaseStatusDeleted.None
                           select stat.Activity.Weight).Sum(a => a);
        }


        private Int64 GetUnitStatWeightSum_byStatusStat(IList<UnitStatistic> Statistics, StatusStatistic statusToCheck)
        {
            return (Int64)(from stat in Statistics
                           where CheckStatusStatistic(stat.Status, statusToCheck) && stat.Deleted == BaseStatusDeleted.None
                           select stat.Unit.Weight).Sum(a => a);
        }

        #endregion

        #region get users stats

        private IList<litePerson> GetOrderedPersonInPage(long itemId, int commId, ItemType itemType, int pageSize, int pageSkip, Boolean all = false)
        {
            IList<int> participantsId = serviceEP.ServiceAssignments.GetAllParticipantIdInPath(itemId, commId, itemType,DateTime.Now,all);
            if (participantsId.Count < maxItemsInContainsQuery)
                return (from p in manager.GetIQ<litePerson>()
                        where participantsId.Contains(p.Id)
                        orderby p.Surname, p.Name
                        select p).Skip(pageSkip * pageSize).Take(pageSize).ToList();
            else {
                Int32 counts = (Int32)Math.Ceiling((double)participantsId.Count() / (double)maxItemsInContainsQuery);
                List<litePerson> temp = new List<litePerson>();
                for (int i = 0; i < counts; i++)
                {
                    var q = (from p in manager.GetIQ<litePerson>()
                             where participantsId.Skip(i * maxItemsInContainsQuery).Take(maxItemsInContainsQuery).ToList().Contains(p.Id)
                             select p).ToList();

                    temp = temp.Concat(q).ToList();
                }
                return temp.Skip(pageSkip * pageSize).Take(pageSize).ToList();
            }
        }

        public dtoListUserStat GetUsersStats(long itemId, int commId, ItemType itemType, int pageSize, int pageSkip, DateTime viewStatBefore, Boolean all = false)
        {

            return GetUsersStats(itemId, itemType, GetOrderedPersonInPage(itemId, commId, itemType, pageSize, pageSkip,all),viewStatBefore);
        }

        public dtoSubActListUserStat GetUsersSubActStats(long subActId, int commId, int pageSize, int pageSkip, DateTime viewStatBefore, Boolean all = false)
        {
            return GetSubActUsersStat(subActId, GetOrderedPersonInPage(subActId, commId, ItemType.SubActivity, pageSize, pageSkip,all),viewStatBefore);
        }
        public StatusStatistic GetSubActivityStatisticsStatus(long idSubactivity, Int32 idPerson, DateTime viewStatBefore)
        {
            return GetUserSubActivityStat_Insert(idSubactivity, idPerson, viewStatBefore);
        }

        private StatusStatistic GetUserSubActivityStat_Insert(long idSubactivity, Int32 idPerson, DateTime viewStatBefore)
        {
            var statistics = (from s in manager.GetIQ<SubActivityStatistic>()
                              where s.Deleted == BaseStatusDeleted.None && s.SubActivity != null && s.SubActivity.Id == idSubactivity && s.Person != null && s.Person.Id == idPerson
                              && s.CreatedOn <= viewStatBefore
                              select new { IdStatistic = s.Id, CreatedOn = s.CreatedOn, Status = s.Status, Completion = s.Completion }).ToList();

            DateTime? createdOn = statistics.Select(s => s.CreatedOn).DefaultIfEmpty(viewStatBefore).Max();
            return statistics.Where(s => createdOn.HasValue && s.CreatedOn == createdOn).OrderByDescending(s => s.Status).ThenByDescending(s => s.Completion).Select(s => s.Status).FirstOrDefault();
        }



        public dtoSubActListUserStat GetSubActUserStat(long idSubactivity, Int32 idPerson, DateTime viewStatBefore)
        {
            IList<litePerson> persons = new List<litePerson>();
            litePerson p = manager.GetLitePerson(idPerson);
            if (p != null)
                persons.Add(p);
            return GetSubActUsersStat(idSubactivity,persons,viewStatBefore);
        }

        public dtoSubActListUserStat GetSubActUsersStat(long subActId, IList<litePerson> InterestedPersons,DateTime viewStatBefore)
        {

            SubActivity oSubAct = manager.Get<SubActivity>(subActId);
            dtoSubActListUserStat dtoStats = new dtoSubActListUserStat();
            dtoStats.IdSubActivity = subActId;
            dtoStats.ContentType = oSubAct.ContentType;
            dtoStats.Name = oSubAct.Description;         
            dtoStats.ModuleLink = oSubAct.ModuleLink;
            dtoStats.Status = oSubAct.Status;
            IList<SubActivityStatistic> statistics = GetUsersSubActivityStat_Insert(subActId, InterestedPersons, viewStatBefore);

            //if (statistics != null && statistics.Count > 0)
            //{

                dtoStats.usersStat = (from p in InterestedPersons
                                      join stat in statistics
                                      on p.Id equals stat.Person.Id into result
                                      from stat in result.DefaultIfEmpty()
                                      select new dtoUserStat()
                                      {
                                          UserId = p.Id,
                                          SurnameAndName = p.SurnameAndName,
                                          TaxCode = p.TaxCode,
                                          Completion = stat == null ? (Int64)0 : stat.Completion,
                                          Mark = stat == null ? (Int16)0 : stat.Mark,
                                          StatusStat = stat == null ? StatusStatistic.None : GetMainStatusStatistic(stat.Status)

                                      }).ToList();
            //}
            //else
            //{
            //    dtoStats.usersStat = new List<dtoUserStat>();
            //}
            return dtoStats;
        }

        public dtoListUserStat GetUsersStats(long itemId, ItemType itemType, IList<litePerson> InterestedPersons, DateTime viewStatBefore, Boolean allStatistics=false)
        {
            dtoListUserStat dtoStats = new dtoListUserStat();

            switch (itemType)
            {
                case ItemType.Path:
                    Path oPath = manager.Get<Path>(itemId);
                    dtoStats.ItemName = oPath.Name;
                    dtoStats.Status=oPath.Status;
                    //serviceEP.GetActiveMandatoryUnitCount(itemId, 0, 0);
                    dtoStats.usersStat = GetEpUsersStats(itemId, InterestedPersons,viewStatBefore);
                    break;
                case ItemType.Unit:
                    Unit oUnit = manager.Get<Unit>(itemId);
                    dtoStats.ItemName = oUnit.Name;
                    dtoStats.Status = oUnit.Status;
                   // serviceEP.GetActiveMandatoryActivitiesCount(itemId, 0, 0);
                    dtoStats.usersStat = GetUnitUsersStats(itemId, InterestedPersons,viewStatBefore);
                    break;
                case ItemType.Activity:
                    Activity oACt = manager.Get<Activity>(itemId);
                    dtoStats.ItemName = oACt.Name;
                    dtoStats.Status = oACt.Status;
                   // serviceEP.GetActiveMandatorySubActivitiesCount(itemId, 0, 0);
                    dtoStats.usersStat = GetActivityUsersStats(itemId, InterestedPersons,viewStatBefore);
                    break;
                default:
                    dtoStats = new dtoListUserStat();
                    dtoStats.usersStat = new List<dtoUserStatExtended>();
                    break;
            }
            return dtoStats;
        }

        private IList<dtoUserStatExtended> GetActivityUsersStats(long activityId, IList<litePerson> InterestedPersons, DateTime viewStatBefore)
        {

            IList<ActivityStatistic> statistics = GetUsersActivityStat_Insert(activityId, InterestedPersons, viewStatBefore);

            Int16 minMark=manager.Get<Activity>(activityId).MinMark; //DA CREARE FUNZIONE SE SI PERSONALIZZANO I CAMPI
            return (from p in InterestedPersons
                    join stat in statistics
                    on p.Id equals stat.Person.Id into result
                    from stat in result.DefaultIfEmpty()
                    select new dtoUserStatExtended()
                    {
                        UserId = p.Id,
                        TaxCode = p.TaxCode,
                        SurnameAndName = p.SurnameAndName,
                        Weight=manager.Get<Activity>(activityId).Weight,
                        MinCompletion=serviceEP.ActivityMinCompletion(activityId,p.Id),
                        Completion = stat == null ? (Int64)0 : stat.Completion,
                        Mark = stat == null ? (Int16)0 : stat.Mark,
                        MinMark=minMark,
                        StatusStat = stat == null ? StatusStatistic.None : GetMainStatusStatistic(stat.Status)

                    }).ToList();
        }

        private IList<dtoUserStatExtended> GetUnitUsersStats(long unitId, IList<litePerson> InterestedPersons, DateTime viewStatBefore)
        {

            IList<UnitStatistic> statistics = GetUsersUnitStat_Insert(unitId, InterestedPersons, viewStatBefore);

            Int16 minMark = manager.Get<Unit>(unitId).MinMark; //DA CREARE FUNZIONE SE SI PERSONALIZZANO I CAMPI
            return (from p in InterestedPersons
                    join stat in statistics
                    on p.Id equals stat.Person.Id into result
                    from stat in result.DefaultIfEmpty()
                    select new dtoUserStatExtended()
                    {
                        UserId = p.Id,
                        SurnameAndName = p.SurnameAndName,
                        TaxCode = p.TaxCode,
                        MinCompletion=serviceEP.UnitMinCompletion(unitId,p.Id),
                        Weight = manager.Get<Unit>(unitId).Weight,
                        Completion = stat == null ? (Int64)0 : stat.Completion,
                        Mark = stat == null ? (Int16)0 : stat.Mark,
                        MinMark=minMark,
                        StatusStat = stat == null ? StatusStatistic.None : GetMainStatusStatistic(stat.Status)
                    }).ToList();
        }

        private IList<dtoUserStatExtended> GetEpUsersStats(long pathId, IList<litePerson> InterestedPersons, DateTime viewStatBefore)
        {


            //IList<PathStatistic> statistics = (from stat in manager.GetIQ<PathStatistic>()
            //                                   where stat.Path.Id == pathId && stat.Deleted == BaseStatusDeleted.None && InterestedPersons.Contains<litePerson>(stat.Person) && stat.CreatedOn != null && (DateTime)stat.CreatedOn <= viewStatBefore
            //                                   orderby stat.CreatedOn descending
            //                                   select stat).ToList();

            IList<PathStatistic> statistics = GetUsersPathStat_Insert(pathId, InterestedPersons,viewStatBefore);
            
            Int16 minMark = manager.Get<Path>(pathId).MinMark; //DA CREARE FUNZIONE SE SI PERSONALIZZANO I CAMPI
            return (from p in InterestedPersons
                    join stat in statistics
                    on p.Id equals stat.Person.Id into result
                    from stat in result.DefaultIfEmpty()
                    select new dtoUserStatExtended()
                    {
                        UserId = p.Id,
                        SurnameAndName = p.SurnameAndName,
                        TaxCode = p.TaxCode,
                        MinCompletion=serviceEP.PathMinCompletion(pathId,p.Id),
                        Weight = manager.Get<Path>(pathId).Weight,
                        Completion = stat == null ? (Int64)0 : stat.Completion,
                        Mark = stat == null ? (Int16)0 : stat.Mark,
                        MinMark=minMark,
                        StatusStat = stat == null ? StatusStatistic.None : GetMainStatusStatistic(stat.Status)
                    }).ToList();
        }

        #endregion



        #region export csv xml pdf

        #region "Users"
            /// <summary>
            /// per le SubActivity usare ExportUsersSubActStat_ToCsv
            /// </summary>
            /// <param name="itemId"></param>
            /// <param name="commId"></param>
            /// <param name="itemType"></param>
            /// <param name="transaltions"></param>
            /// <returns></returns>
            public String ExportUsersStatistics_ToCsv(iApplicationContext context, long idItem, int idCommunity, ItemType itemType, Boolean allStatistics, Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations,dtoExportConfigurationSetting settings, ExporPathData exportData, DateTime viewStatBefore)
            {
                EPType pathType = EPType.None;
                HelperExportToCsv helper = GetHelperForCSV(context, translations, roleTranslations);
                dtoListUserStat itemStat = GetUsersStatistics(idItem, itemType, idCommunity, allStatistics, viewStatBefore, ref pathType);
                return helper.UsersStatistics(itemStat, viewStatBefore, itemType, serviceEP.CheckEpType(pathType, EPType.Auto), serviceEP.CheckEpType(pathType, EPType.Time), settings, exportData);
            }
            public String ExportUsersStatistics_ToXml(iApplicationContext context, long idItem, int idCommunity, ItemType itemType, Boolean allStatistics, Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations, dtoExportConfigurationSetting settings, ExporPathData exportData, DateTime viewStatBefore)
            {
                HelperExportToXml helper = GetHelperForXML(context, translations, roleTranslations);
                EPType pathType = EPType.None;
                dtoListUserStat itemStat = GetUsersStatistics(idItem, itemType, idCommunity, allStatistics, viewStatBefore, ref pathType);
                return helper.UsersStatistics(itemStat, viewStatBefore, itemType, serviceEP.CheckEpType(pathType, EPType.Auto), serviceEP.CheckEpType(pathType, EPType.Time), settings, exportData);
            }
           
            private dtoListUserStat GetUsersStatistics(long idItem, ItemType itemType, int idCommunity, Boolean allStatistics, DateTime viewStatBefore, ref EPType pathType)
            {
                //IList<int> participantsId = serviceEP.ServiceAss.GetAllParticipantIdInPath(idItem, idCommunity, itemType, viewStatBefore, allStatistics);
                //IList<litePerson> participants = (from p in manager.GetIQ<litePerson>()
                //                              where participantsId.Contains(p.Id)
                //                              orderby p.Surname, p.Name
                //                              select p).ToList();
                IList<litePerson> participants = GetParticipants(idItem, itemType, idCommunity, allStatistics, viewStatBefore);
                pathType = serviceEP.GetEpType(idItem, itemType);
                return GetUsersStats(idItem, itemType, participants, viewStatBefore);
            }
            #endregion

        #region "SubActivity"
                public String ExportUsersSubActivityStatistics_ToCsv(iApplicationContext context, long idSubActivity, int idCommunity, Boolean allStatistics, Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations,dtoExportConfigurationSetting settings, ExporPathData exportData, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, DateTime viewStatBefore)
                {
                    HelperExportToCsv helper = GetHelperForCSV(context, translations, roleTranslations);
                    EPType pathType = EPType.None;
                    dtoSubActListUserStat subActStats = GetUsersSubactivityStatistics(idSubActivity, idCommunity, allStatistics, viewStatBefore, ref pathType);

                    return helper.UsersSubActivityStatistics(serviceEP.GetDtoSubActivity(idSubActivity), viewStatBefore, subActStats, serviceEP.CheckEpType(pathType, EPType.Auto), serviceEP.CheckEpType(pathType, EPType.Time), settings, exportData, translations, quizAction,repAction, tAction, cAction);
                }
                public String ExportUsersSubActivityStatistics_ToXml(iApplicationContext context, long idSubActivity, int idCommunity, Boolean allStatistics, Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations, dtoExportConfigurationSetting settings, ExporPathData exportData, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, DateTime viewStatBefore)
                {
                    HelperExportToXml helper = GetHelperForXML(context, translations, roleTranslations);
                    EPType pathType = EPType.None;
                    dtoSubActListUserStat subActStats = GetUsersSubactivityStatistics(idSubActivity, idCommunity, allStatistics, viewStatBefore, ref pathType);

                    return helper.UsersSubActivityStatistics(serviceEP.GetDtoSubActivity(idSubActivity), viewStatBefore, subActStats, serviceEP.CheckEpType(pathType, EPType.Auto), serviceEP.CheckEpType(pathType, EPType.Time), settings, exportData, quizAction, repAction, tAction, cAction);
                }
                private dtoSubActListUserStat GetUsersSubactivityStatistics(long idSubActivity, int idCommunity, Boolean allStatistics, DateTime viewStatBefore, ref EPType pathType)
                {

                    IList<litePerson> participants = GetParticipants(idSubActivity, ItemType.SubActivity, idCommunity, allStatistics, viewStatBefore);
                    pathType = serviceEP.GetEpType(idSubActivity, ItemType.SubActivity);
                    return GetSubActUsersStat(idSubActivity,participants, viewStatBefore);
                }
                /// maxItemsInContainsQuery ADDED
                /// <summary>
                /// data una sotto attività individua tutte le "person" che sono state coinvolte a livello di statistiche in base ai parametri in input
                /// </summary>
                /// <param name="idSubActivity"></param>
                /// <param name="idCommunity"></param>
                /// <param name="allStatistics"></param>
                /// <param name="viewStatBefore"></param>
                /// <returns></returns>
                private List<litePerson> GetParticipants(long idItem, ItemType itemType, int idCommunity, Boolean allStatistics, DateTime viewStatBefore)
                {
                    List<litePerson> participants = null;
                    IList<int> participantsId = serviceEP.ServiceAssignments.GetAllParticipantIdInPath(idItem, idCommunity, itemType, viewStatBefore, allStatistics);
                 
                    return manager.GetLitePersons(participantsId.ToList()).OrderBy(p => p.SurnameAndName).ToList();
                }
        #endregion
            #region "Global"
                public String ExportGlobalEpStats_ToCsv(iApplicationContext context, long pathId, Int32 commId, Int32 adminId, Int32 adminCrole, Boolean allStatistics, Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction cTextAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction certAction, DateTime viewStatBefore)
                {
                    HelperExportToCsv helper = GetHelperForCSV(context, translations, roleTranslations);
                    dtoEpGlobalStat globalStat = GetGlobalEpStats(pathId, commId, adminId, adminCrole, false, viewStatBefore, allStatistics);
                    EPType pathType = serviceEP.GetEpType(pathId, ItemType.Path);
                    return helper.PathStatistics(globalStat, serviceEP.CheckEpType(pathType, EPType.Auto), serviceEP.CheckEpType(pathType, EPType.Time),  quizAction,repAction, cTextAction, certAction);

                }
                public String ExportGlobalEpStats_ToXml(iApplicationContext context, long pathId, Int32 commId, Int32 adminId, Int32 adminCrole, Boolean allStatistics, Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction cTextAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction certAction, DateTime viewStatBefore)
                {
                    HelperExportToXml helper = GetHelperForXML(context,translations, roleTranslations);
                    dtoEpGlobalStat globalStat = GetGlobalEpStats(pathId, commId, adminId, adminCrole, false, viewStatBefore, allStatistics);
                    EPType pathType = serviceEP.GetEpType(pathId, ItemType.Path);
                    return helper.PathStatistics(globalStat, viewStatBefore, serviceEP.CheckEpType(pathType, EPType.Auto), serviceEP.CheckEpType(pathType, EPType.Time), quizAction,repAction, cTextAction, certAction);
                }

            #endregion

            private HelperExportToCsv GetHelperForCSV(iApplicationContext context,Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations) {
                return new HelperExportToCsv(new lm.Comol.Core.Business.BaseModuleManager(context),translations, roleTranslations);
            }
            private HelperExportToXml GetHelperForXML(iApplicationContext context,Dictionary<EduPathTranslations, String> translations, Dictionary<Int32, String> roleTranslations) {
                return new HelperExportToXml(new lm.Comol.Core.Business.BaseModuleManager(context),translations, roleTranslations);
            }
        #endregion



                #region validate

                public dtoActStructureValidate GetValidateActStat(dtoActStructureValidate dtoAct, bool isAutoEp)
        {
            dtoAct.StatusStat = StatusStatistic.None;

            if (dtoAct.Completion > 0)
            {
                dtoAct.StatusStat |= StatusStatistic.Started;
            }
           
            if (isAutoEp)
            {

                if (dtoAct.Completion >= dtoAct.MinCompletion)
                {
                    dtoAct.StatusStat |= StatusStatistic.CompletedPassed;
                }              

            }
            else
            {
                
                if (dtoAct.Completion >= dtoAct.MinCompletion)
                {
                    dtoAct.StatusStat |= StatusStatistic.Completed;
                }
                if (dtoAct.Mark >= dtoAct.MinMark)
                {
                    dtoAct.StatusStat |= StatusStatistic.Passed;
                }
            
            }
           
            dtoAct.StatusStat= GetMainStatusStatistic(dtoAct.StatusStat);
            return  dtoAct;
        }

        public dtoUnitStructureValidate GetValidateUnitStat(dtoUnitStructureValidate dtoUnit, bool isAutoEp)
        {
            dtoUnit.StatusStat = StatusStatistic.None;

            if (dtoUnit.Completion > 0)
            {
                dtoUnit.StatusStat |= StatusStatistic.Started;
            }
            if (dtoUnit.Completion >= dtoUnit.MinCompletion)
            {
                dtoUnit.StatusStat |= StatusStatistic.Completed;
            }
            if (dtoUnit.Mark >= dtoUnit.MinMark &&  !isAutoEp)
            {
                dtoUnit.StatusStat |= StatusStatistic.Passed;
            }
            dtoUnit.StatusStat=  GetMainStatusStatistic(dtoUnit.StatusStat);
            return dtoUnit;
        }

        private void SetCompletionUnitStatValidate_Manual(dtoUnitStructureValidate dtoUnit)
        {
            Int64 totWeight = (Int64)dtoUnit.Activities.Sum(a => a.Weight);
            Int64 totCompletion;
            if (totWeight == 0)
            {
                totWeight = (Int64)dtoUnit.Activities.Count();
                
                if (totWeight == 0)
                {
                    return ;
                }
                
                totCompletion = (Int64)(from stat in dtoUnit.Activities
                                        select new
                                        {
                                            subTot = stat.Completion < stat.MinCompletion ? (Int64)(stat.Completion) : (Int64)(100)
                                        }).Sum(b => b.subTot);
            }
            else
            {
                totCompletion = (Int64)(from stat in dtoUnit.Activities
                                        select new
                                        {
                                            subTot =stat.Completion < stat.MinCompletion ? (Int64)(stat.Completion * stat.Weight) : (Int64)(100 * stat.Weight)
                                        }).Sum(b => b.subTot);
            }

            dtoUnit.Completion = (Int64)(totCompletion / totWeight);
        }

  
        private void SetCompletionEpStatValidate_Manual(dtoEpStructureValidate dtoEp)
        {
            Int64 totWeight = (Int16)dtoEp.Units.Sum(s => s.Weight);

            Int64 totCompletion;
            if (totWeight == 0)
            {
                totWeight = (Int16)dtoEp.Units.Count();
               
                if (totWeight == 0)
                {
                    return;
                }
               
                totCompletion = (Int64)(from stat in dtoEp.Units
                                        select new
                                        {
                                            subTot = stat.Completion < stat.MinCompletion ? (Int64)(stat.Completion) : (Int64)(100)
                                        }).Sum(b => (b.subTot));
            }
            else
            {
                totCompletion = (Int64)(from stat in dtoEp.Units
                                        select new
                                        {
                                            subTot = stat.Completion < stat.MinCompletion ? (Int64)(stat.Completion * stat.Weight) : (Int64)(100 * stat.Weight)
                                        }).Sum(b => (b.subTot));
            }

            dtoEp.Completion = (Int64)(totCompletion / totWeight);
        }  
        

        private void SetMarkUnitStatValidate_Manual(dtoUnitStructureValidate dtoUnit) 
        {

            Int64 totWeight = (Int64)dtoUnit.Activities.Sum(a => a.Weight);

            Int16 markSum; 
            Int64 partialWeight = 0;

            if (totWeight == 0) //newMark= media non pesata delle statistiche presenti
            {
                markSum = (Int16)(from stat in dtoUnit.Activities
                                  where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                  select stat.Mark).Sum(b => b);
                partialWeight = (Int64)(from stat in dtoUnit.Activities
                                        where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)                  
                                        select stat.Weight).Count();
                if (partialWeight > 0)
                {
                    dtoUnit.Mark = (Int16)(markSum / partialWeight);
                }
            }
            else
            {
                partialWeight = (Int64)(from stat in dtoUnit.Activities
                                        where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                        select stat.Weight).Sum(s => s);

                if (partialWeight > 0) //newMark= media pesata delle statistiche presenti
                {
                    markSum = (Int16)(from stat in dtoUnit.Activities
                                      where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                      select stat).Sum(b => (b.Mark * b.Weight));
                    dtoUnit.Mark = (Int16)(markSum / partialWeight);
                }
            }
        }
     
        private void SetMarkEpStatValidate_Manual(dtoEpStructureValidate dtoEp)        
        {
            Int64 totWeight = (Int64)dtoEp.Units.Sum(s => s.Weight);


            Int16 markSum;
            Int64 partialWeight;

            if (totWeight == 0) //newMark= media non pesata delle statistiche presenti
            {
                markSum = (Int16)(from stat in dtoEp.Units
                                  where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                  select stat.Mark).Sum(b => b);
                partialWeight = (Int64)(from stat in dtoEp.Units
                                        where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                        select stat.Weight).Count();
                if (partialWeight > 0)
                {
                    dtoEp.Mark = (Int16)(markSum / partialWeight);
                }
            }
            else
            {
                partialWeight = (Int64)(from stat in dtoEp.Units
                                        where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                        select stat.Weight).Sum(s => s);

                if (partialWeight > 0) //newMark= media pesata delle statistiche presenti
                {
                    markSum = (Int16)(from stat in dtoEp.Units
                                      where CheckStatusStatistic(stat.StatusStat, StatusStatistic.Passed)
                                      select stat).Sum(b => (b.Mark * b.Weight));
                    dtoEp.Mark = (Int16)(markSum / partialWeight);
                }
            }
       
        }
        
        /// <summary>
        /// da utilizzare con validazione attiva in ActivityMode
        /// </summary>
        /// <param name="dtoUnit"></param>
        /// <param name="isAutoEp"></param>
        /// <returns></returns>
        public dtoUnitStructureValidate GetUnitStatValidate(dtoUnitStructureValidate dtoUnit, bool isAutoEp)
        {
            if (!isAutoEp)
            { 
                //completion
                SetCompletionUnitStatValidate_Manual(dtoUnit);

                if (dtoUnit.Completion > 0)
                {
                    dtoUnit.StatusStat |= ( StatusStatistic.Browsed | StatusStatistic.Started);

                    //status Completed
                    if (dtoUnit.Completion >= dtoUnit.MinCompletion)
                    {
                        Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount(dtoUnit.Id, 0, 0);
                        Int16 mandatoryPassedCompletedActCount = (Int16)(from a in dtoUnit.Activities
                                                                         where serviceEP.CheckStatus(a.Status, Status.Mandatory) && CheckStatusStatistic(a.StatusStat, StatusStatistic.CompletedPassed)
                                                                         select a).Count();
                        if (actMandCount == mandatoryPassedCompletedActCount)
                        {
                            dtoUnit.StatusStat |= StatusStatistic.Completed;
                        }
                    }

                    //mark
                    SetMarkUnitStatValidate_Manual(dtoUnit);

                    //status passed
                    if (dtoUnit.Mark >= dtoUnit.MinMark && CheckStatusStatistic(dtoUnit.StatusStat, StatusStatistic.Completed))
                    {
                        Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount(dtoUnit.Id, 0, 0);
                        Int16 mandatoryPassedCompletedActCount = (Int16)(from a in dtoUnit.Activities
                                                                         where serviceEP.CheckStatus(a.Status, Status.Mandatory) && CheckStatusStatistic(a.StatusStat, StatusStatistic.CompletedPassed)
                                                                         select a).Count();

                        if (actMandCount == mandatoryPassedCompletedActCount)
                        {

                            dtoUnit.StatusStat |= StatusStatistic.Passed;
                        }
                    }
                }                       

                dtoUnit.StatusStat = GetMainStatusStatistic(dtoUnit.StatusStat);
            }
           
            return dtoUnit;        
        }
        
        public dtoEpStructureValidate GetValidateEpStat(dtoEpStructureValidate dtoEp, bool isAutoEp)
        {

            if (isAutoEp)
            {
                IList<dtoActStructureValidate> actStats = (from act in dtoEp.Units.SelectMany(u => u.Activities) select act).ToList();

                dtoEp.Weight = (Int64)actStats.Sum(a => a.Weight);

                dtoEp.Completion = (Int64)((from a in actStats
                                    where CheckStatusStatistic(a.StatusStat, StatusStatistic.CompletedPassed)
                                    select a.Weight).Sum(a=>a) * 100/ Math.Max(dtoEp.Weight,1));              

                if (dtoEp.Completion > 0 || ((from a in actStats where a.StatusStat>=StatusStatistic.Started select a).Count()>0) )
                {
                    dtoEp.StatusStat |= (StatusStatistic.Browsed | StatusStatistic.Started);
                

                    if (dtoEp.Completion >= (dtoEp.MinCompletion * dtoEp.Weight / 100))
                    {
                        Int16 mandatoryPassedCompletedActCount = (Int16)(from a in actStats
                                                                         where serviceEP.CheckStatus(a.Status, Status.Mandatory) && CheckStatusStatistic(a.StatusStat, StatusStatistic.CompletedPassed)
                                                                         select a).Count();
                        Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount_byPathId(dtoEp.Id, 0, 0);
                        if (actMandCount == mandatoryPassedCompletedActCount)
                        {
                            dtoEp.StatusStat |= StatusStatistic.CompletedPassed;
                        }
                    }
                }
            }
            else
            { 
                //calcolo completion
                SetCompletionEpStatValidate_Manual(dtoEp);
                //Status completed

                if (dtoEp.Completion > 0)
                {
                    dtoEp.StatusStat |= (StatusStatistic.Browsed | StatusStatistic.Started);

                    if (dtoEp.Completion >= dtoEp.MinCompletion)
                    {
                        Int16 unitMandCount = serviceEP.GetActiveMandatoryUnitCount(dtoEp.Id, 0, 0);
                        Int16 mandatoryPassedCompletedUnitCount = (Int16)(from a in dtoEp.Units
                                                                          where serviceEP.CheckStatus(a.Status, Status.Mandatory) && CheckStatusStatistic(a.StatusStat, StatusStatistic.CompletedPassed)
                                                                          select a).Count();

                        if (unitMandCount == mandatoryPassedCompletedUnitCount)
                        {
                            dtoEp.StatusStat |= StatusStatistic.Completed;
                        }
                    }

                    //calcolo mark
                    SetMarkEpStatValidate_Manual(dtoEp);

                    //status passed
                    if (dtoEp.Mark >= dtoEp.MinMark && CheckStatusStatistic(dtoEp.StatusStat, StatusStatistic.Completed))
                    {
                        Int16 unitMandCount = serviceEP.GetActiveMandatoryUnitCount(dtoEp.Id, 0, 0);
                        Int16 mandatoryPassedCompletedUnitCount = (Int16)(from a in dtoEp.Units
                                                                          where serviceEP.CheckStatus(a.Status, Status.Mandatory) && CheckStatusStatistic(a.StatusStat, StatusStatistic.CompletedPassed)
                                                                          select a).Count();
                        if (unitMandCount == mandatoryPassedCompletedUnitCount)
                        {
                            dtoEp.StatusStat |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
                        }

                    }
                }
                                
            }
            dtoEp.StatusStat = GetMainStatusStatistic(dtoEp.StatusStat);
            return dtoEp;
        }

        #endregion

        #region Copy Stat
        ///// <summary>
        ///// Copy Object without children
        ///// </summary>
        ///// <param name="oStat"></param>
        ///// <returns></returns>
        //public PathStatistic CopyPathStatistic(PathStatistic oStat, int copyCreatorId, String CreatorIpAddress, String CreatorProxyIpAddress)
        //{
        //   return CopyPathStatistic( oStat, manager.GetPerson(copyCreatorId),  CreatorIpAddress,  CreatorProxyIpAddress);
        //}

        /// <summary>
        /// Copy Object without children
        /// </summary>
        /// <param name="oStat"></param>
        /// <returns></returns>
        public PathStatistic CopyPathStatistic(PathStatistic oStat, Int32 idCreator, String CreatorIpAddress, String CreatorProxyIpAddress)
        {
            PathStatistic oCopy = new PathStatistic();
            oCopy.Path = oStat.Path;
            oCopy.MandatoryPassedUnitCount = oStat.MandatoryPassedUnitCount;
            oCopy.MandatoryCompletedUnitCount = oStat.MandatoryCompletedUnitCount;
            oCopy.MandatoryPassedCompletedUnitCount = oStat.MandatoryPassedCompletedUnitCount;
            oCopy.CreateMetaInfo(idCreator, CreatorIpAddress, CreatorProxyIpAddress);
            oCopy.Person = oStat.Person;
            oCopy.Status = oStat.Status;
            oCopy.StartDate = oStat.StartDate;
            oCopy.EndDate = oStat.EndDate;
            oCopy.Completion = oStat.Completion;
            return oCopy;
        }


        ///// <summary>
        ///// Copy Object without children and parent
        ///// </summary>
        ///// <param name="oStat"></param>
        ///// <returns></returns>
        //public UnitStatistic CopyUnitStatistic(UnitStatistic oStat, int copyCreatorId, String CreatorIpAddress, String CreatorProxyIpAddress)
        //{
        //    return CopyUnitStatistic(oStat, manager.GetPerson(copyCreatorId), CreatorIpAddress, CreatorProxyIpAddress);
        //}

        /// <summary>
        /// Copy Object without children and parent
        /// </summary>
        /// <param name="oStat"></param>
        /// <returns></returns>
        public UnitStatistic CopyUnitStatistic(UnitStatistic oStat, Int32 copyCreatorId, String CreatorIpAddress, String CreatorProxyIpAddress)
        {
            UnitStatistic oCopy = new UnitStatistic();
            oCopy.Unit = oStat.Unit;
            oCopy.MandatoryPassedActivityCount = oStat.MandatoryPassedActivityCount;
            oCopy.MandatoryCompletedActivityCount = oStat.MandatoryCompletedActivityCount;
            oCopy.MandatoryPassedCompletedActivityCount = oStat.MandatoryPassedCompletedActivityCount;
            oCopy.IdPath = oStat.IdPath;         
            oCopy.Person = oStat.Person;
            oCopy.CreateMetaInfo(copyCreatorId, CreatorIpAddress,CreatorProxyIpAddress);
            oCopy.Status = oStat.Status;
            oCopy.StartDate = oStat.StartDate;
            oCopy.EndDate = oStat.EndDate;
            oCopy.Completion = oStat.Completion;
            return oCopy;
        }



        ///// <summary>
        ///// Copy Object without children and parent
        ///// </summary>
        ///// <param name="oStat"></param>
        ///// <returns></returns>
        //public ActivityStatistic CopyActivityStatistic(ActivityStatistic oStat, int copyCreatorId, String CreatorIpAddress, String CreatorProxyIpAddress)
        //{
        //    return CopyActivityStatistic(oStat, manager.GetPerson(copyCreatorId), CreatorIpAddress, CreatorProxyIpAddress);
        //}

        /// <summary>
        /// Copy Object without children and parent
        /// </summary>
        /// <param name="oStat"></param>
        /// <returns></returns>
        public ActivityStatistic CopyActivityStatistic(ActivityStatistic oStat, Int32 copyCreatorId, String CreatorIpAddress, String CreatorProxyIpAddress)
        {
            ActivityStatistic oCopy = new ActivityStatistic();
            oCopy.Activity = oStat.Activity;
            oCopy.MandatoryPassedSubActivityCount = oStat.MandatoryPassedSubActivityCount;
            oCopy.MandatoryCompletedSubActivityCount = oStat.MandatoryCompletedSubActivityCount;
            oCopy.MandatoryPassedCompletedSubActivityCount = oStat.MandatoryPassedCompletedSubActivityCount;
            oCopy.IdPath = oStat.IdPath;
            oCopy.IdUnit = oStat.IdUnit;
            oCopy.Person = oStat.Person;
            oCopy.CreateMetaInfo(copyCreatorId, CreatorIpAddress, CreatorProxyIpAddress);
            oCopy.Status = oStat.Status;
            oCopy.StartDate = oStat.StartDate;
            oCopy.EndDate = oStat.EndDate;
            oCopy.Completion = oStat.Completion;
            return oCopy;
        }

        ///// <summary>
        ///// Copy Object without parent
        ///// </summary>
        ///// <param name="oStat"></param>
        ///// <returns></returns>
        //public SubActivityStatistic CopySubActivityStatistic(SubActivityStatistic oStat, int copyCreatorId, String CreatorIpAddress, String CreatorProxyIpAddress)
        //{
        //    return CopySubActivityStatistic(oStat, manager.GetPerson(copyCreatorId), CreatorIpAddress, CreatorProxyIpAddress);
        //}

        /// <summary>
        /// Copy Object without  parent
        /// </summary>
        /// <param name="oStat"></param>
        /// <returns></returns>
        public SubActivityStatistic CopySubActivityStatistic(SubActivityStatistic oStat, Int32 idCreator, String CreatorIpAddress, String CreatorProxyIpAddress)
        {
            SubActivityStatistic oCopy = new SubActivityStatistic();
            oCopy.SubActivity = oStat.SubActivity;
           
            oCopy.IdPath = oStat.IdPath;
            oCopy.IdUnit = oStat.IdUnit;
            oCopy.IdActivity = oStat.IdActivity;
            oCopy.Person = oStat.Person;
            oCopy.CreateMetaInfo(idCreator, CreatorIpAddress, CreatorProxyIpAddress);
            oCopy.Status = oStat.Status;
            oCopy.StartDate = oStat.StartDate;
            oCopy.EndDate = oStat.EndDate;
            oCopy.Completion = oStat.Completion;
            return oCopy;
        }
        
        #endregion


        private void subDeleteAllEpStat(IList<PathStatistic> EpStats)
        { 
        
        }

        public void DeleteAllEpStat(long PathId, int userId)
        {
            try
            {
                manager.BeginTransaction();
                IList<PathStatistic> EpStats = manager.GetAll<PathStatistic>(stat => stat.Path.Id == PathId && stat.Person.Id==userId).ToList();
                IList<PathStatistic> pStats = (from a in manager.GetIQ<PathStatistic>()
                                               where a.Path.Id == PathId && a.Person.Id == userId
                                               select a).ToList();
                foreach (PathStatistic item in pStats)
                {
                    manager.DeleteGeneric(item);
                }

                IList<UnitStatistic> uStats = (from a in manager.GetIQ<UnitStatistic>()
                                               where a.IdPath == PathId && a.Person.Id==userId
                                               select a).ToList();
                foreach (UnitStatistic item in uStats)
                {
                    manager.DeleteGeneric(item);
                }

                IList<ActivityStatistic> aStats = (from a in manager.GetIQ<ActivityStatistic>()
                                                   where a.IdPath == PathId && a.Person.Id == userId
                                                   select a).ToList();
                foreach (ActivityStatistic item in aStats)
                {
                    manager.DeleteGeneric(item);
                }

                IList<SubActivityStatistic> saStats = (from a in manager.GetIQ<SubActivityStatistic>()
                                                       where a.IdPath == PathId && a.Person.Id == userId
                                                       select a).ToList();
                foreach (SubActivityStatistic item in saStats)
                {
                    manager.DeleteGeneric(item);
                }
                manager.Commit();
            }
            catch (Exception ex)
            {
                manager.RollBack();
                throw;
            }

        }


        public void DeleteAllEpStat(long PathId)
        {
            try
            {
                manager.BeginTransaction();
                IList<PathStatistic> pStats = (from a in manager.GetIQ<PathStatistic>()
                                               where a.Path.Id == PathId
                                               select a).ToList();
                foreach (PathStatistic item in pStats)
                {
                    manager.DeleteGeneric(item);
                }
                IList<UnitStatistic> uStats = (from a in manager.GetIQ<UnitStatistic>()
                                               where a.IdPath == PathId
                                               select a).ToList();
                foreach (UnitStatistic item in uStats)
                {
                    manager.DeleteGeneric(item);
                }

                IList<ActivityStatistic> aStats = (from a in manager.GetIQ<ActivityStatistic>()
                                                   where a.IdPath == PathId
                                                   select a).ToList();
                foreach (ActivityStatistic item in aStats)
                {
                    manager.DeleteGeneric(item);
                }

                IList<SubActivityStatistic> saStats = (from a in manager.GetIQ<SubActivityStatistic>()
                                                       where a.IdPath == PathId
                                                       select a).ToList();
                foreach (SubActivityStatistic item in saStats)
                {
                    manager.DeleteGeneric(item);
                }
                manager.Commit();
            }
            catch (Exception ex)
            {
                manager.RollBack();
                throw;
            }
          
        }

        //ToDo: ottimizzare a livello di comnuità, dictionary, etc...
        //ToDo: estendere a livello di sistema, solo MOOCS!
        public StatusStatistic GetEpUserStatus(long pathId, int personId)
        {

            litePerson person = manager.GetLitePerson(personId);
            if (person == null)
                return StatusStatistic.None;

            IList<litePerson> InterestedPersons = new List<litePerson>();
            InterestedPersons.Add(person);

            IList<PathStatistic> statistics = GetUsersPathStat_Insert(pathId, InterestedPersons, DateTime.Now);

            Int16 minMark = manager.Get<Path>(pathId).MinMark; //DA CREARE FUNZIONE SE SI PERSONALIZZANO I CAMPI
            StatusStatistic status = (from p in InterestedPersons
                                      join stat in statistics
                                      on p.Id equals stat.Person.Id into result
                                      from stat in result.DefaultIfEmpty()
                                      select
                                          (stat == null) ? StatusStatistic.None : GetMainStatusStatistic(stat.Status)
                    ).Skip(0).Take(1).FirstOrDefault();

            return status;
        }

        #region create Meta info

        //private void CreateSubActStatMetaInfo(SubActivityStatistic oStat,Person creator, DateTime date, String creatorIpAddress, String creatorProxy)
        //{
        //    oStat.CreatedBy = creator;
        //    oStat.CreatedOn = date;
        //    oStat.CreatorIpAddress = creatorIpAddress;
        //    oStat.CreatorProxyIpAddress = creatorProxy;        
        //}

        //private void CreateActStatMetaInfo(ActivityStatistic oStat, Person creator, DateTime date, String creatorIpAddress, String creatorProxy)
        //{
        //    oStat.CreatedBy = creator;
        //    oStat.CreatedOn = date;
        //    oStat.CreatorIpAddress = creatorIpAddress;
        //    oStat.CreatorProxyIpAddress = creatorProxy;
        //}

        //private void CreateUnitStatMetaInfo(UnitStatistic oStat, Person creator, DateTime date, String creatorIpAddress, String creatorProxy)
        //{
        //    oStat.CreatedBy = creator;
        //    oStat.CreatedOn = date;
        //    oStat.CreatorIpAddress = creatorIpAddress;
        //    oStat.CreatorProxyIpAddress = creatorProxy;
        //}

        //private void CreatePathStatMetaInfo(PathStatistic oStat, Person creator, DateTime date, String creatorIpAddress, String creatorProxy)
        //{
        //    oStat.CreatedBy = creator;
        //    oStat.CreatedOn = date;
        //    oStat.CreatorIpAddress = creatorIpAddress;
        //    oStat.CreatorProxyIpAddress = creatorProxy;
        //}
        #endregion


        #region Funzioni modalità update
        #region Set ModifyMetaInfo
        //private void SetStatSubActivityModifyMetaInfo(SubActivityStatistic oStat, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oStat.ModifiedBy = AuthorOfModify;
        //    oStat.ModifiedIpAddress = PersonIpAddress;
        //    oStat.ModifiedOn = CurrentTime;
        //    oStat.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetStatActivityModifyMetaInfo(ActivityStatistic oStat, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oStat.ModifiedBy = AuthorOfModify;
        //    oStat.ModifiedIpAddress = PersonIpAddress;
        //    oStat.ModifiedOn = CurrentTime;
        //    oStat.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetStatUnitModifyMetaInfo(UnitStatistic oStat, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oStat.ModifiedBy = AuthorOfModify;
        //    oStat.ModifiedIpAddress = PersonIpAddress;
        //    oStat.ModifiedOn = CurrentTime;
        //    oStat.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetStatPathModifyMetaInfo(PathStatistic oStat, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oStat.ModifiedBy = AuthorOfModify;
        //    oStat.ModifiedIpAddress = PersonIpAddress;
        //    oStat.ModifiedOn = CurrentTime;
        //    oStat.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        #endregion

        #region remove passed status and update count executed mandatory item
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="isPassedCompleted">Passed & Completed dell'item</param>
        ///// <param name="compPassCount">Passed & Completed del Sup(item)</param>
        ///// <param name="operation"></param>
        //private Int16 updateCompPassCount(bool isPassedCompleted, Int16 compPassCount, StatusOperation operation)
        //{
        //    if (operation == StatusOperation.Add && isPassedCompleted)
        //    {
        //        compPassCount++;
        //    }
        //    else if (operation == StatusOperation.Remove && isPassedCompleted)
        //    {
        //        compPassCount--;
        //    }
        //    return compPassCount;
        //}

        //private ModifyStatField SubRemoveStatusOperation_byActStat(ActivityStatistic oActStat)
        //{
        //    oActStat.Status -= StatusStatistic.Passed;

        //    if (serviceEP.ActivityIsMandatoryForParticipant(oActStat.Activity.Id, 0, 0))
        //    {
        //        UnitStatistic oParentStat = oActStat.ParentStat;
        //        oParentStat.MandatoryPassedCompletedActivityCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oParentStat.MandatoryPassedCompletedActivityCount, StatusOperation.Remove);
        //        oParentStat.MandatoryPassedActivityCount--;
        //    }

        //    return ModifyStatField.PassedRem;
        //}

        //private ModifyStatField SubRemoveStatusOperation_byUnitStat(UnitStatistic oUnitStat)
        //{
        //    oUnitStat.Status -= StatusStatistic.Passed;
        //    if (serviceEP.UnitIsMandatoryForParticipant(oUnitStat.Unit.Id, 0, 0))
        //    {
        //        PathStatistic parentStat = oUnitStat.ParentStat;
        //        parentStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedUnitCount, StatusOperation.Remove);
        //        parentStat.MandatoryPassedUnitCount--;

        //    }
        //    return ModifyStatField.PassedRem;
        //}
        #endregion

        #region UpdateItemMandatoryCompleted
        //private void UpdateSubActMandatoryCompletedCount(long actStatId, StatusOperation Operation)
        //{
        //    ActivityStatistic oStat = manager.Get<ActivityStatistic>(actStatId);
        //    if (Operation == StatusOperation.Add)
        //    {
        //        oStat.MandatoryCompletedSubActivityCount++;
        //    }
        //    else if (Operation == StatusOperation.Remove)
        //    {
        //        oStat.MandatoryCompletedSubActivityCount--;
        //    }
        //}

        //private void UpdateActMandatoryCompletedCount(long unitStatId, StatusOperation Operation)
        //{
        //    UnitStatistic oStat = manager.Get<UnitStatistic>(unitStatId);
        //    if (Operation == StatusOperation.Add)
        //    {
        //        oStat.MandatoryCompletedActivityCount++;
        //    }
        //    else if (Operation == StatusOperation.Remove)
        //    {
        //        oStat.MandatoryCompletedActivityCount--;
        //    }
        //}

        //private void UpdateUnitMandatoryCompletedCount(long pathStatId, StatusOperation Operation)
        //{
        //    PathStatistic oStat = manager.Get<PathStatistic>(pathStatId);
        //    if (Operation == StatusOperation.Add)
        //    {
        //        oStat.MandatoryCompletedUnitCount++;
        //    }
        //    else if (Operation == StatusOperation.Remove)
        //    {
        //        oStat.MandatoryCompletedUnitCount--;
        //    }
        //}
        #endregion


        #region GetOrInit update mode
        //public PathStatistic GetOrInitPathStatistic(long PathId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        //{
        //    PathStatistic oStat = GetPathStatistic(PathId, participantId, timeToDelete);

        //    if (oStat == null)
        //    {
        //        try
        //        {
        //            manager.BeginTransaction();
        //            oStat = InitPathStatisticNoTransaction(PathId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
        //            manager.Commit();

        //        }
        //        catch (Exception ex)
        //        {
        //            manager.RollBack();
        //            Debug.Write(ex);
        //            return null;
        //        }
        //    }

        //    return oStat;
        //}

        //public UnitStatistic GetOrInitUnitStatistic(long UnitId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        //{
        //    UnitStatistic oStat = GetUnitStatistic(UnitId, participantId, timeToDelete);

        //    if (oStat == null)
        //    {
        //        try
        //        {
        //            manager.BeginTransaction();
        //            oStat = InitUnitStatisticNoTransaction(UnitId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
        //            manager.Commit();

        //        }
        //        catch (Exception ex)
        //        {
        //            manager.RollBack();
        //            Debug.Write(ex);
        //            return null;
        //        }
        //    }
        //    return oStat;
        //}

        
        //public ActivityStatistic GetOrInitActivityStatistic(long ActivityId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        //{
        //    ActivityStatistic oStat = GetActivityStatistic(ActivityId, participantId);

        //    if (oStat == null)
        //    {
        //        try
        //        {
        //            manager.BeginTransaction();
        //            oStat = InitActStatisticNoTrasaction(ActivityId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
        //            manager.Commit();

        //        }
        //        catch (Exception ex)
        //        {
        //            manager.RollBack();
        //            Debug.Write(ex);
        //            return null;
        //        }
        //    }
        //    return oStat;
        //}

        //private ActivityStatistic GetOrInitActivityStatisticNoTransaction(long ActivityId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        //{
        //    ActivityStatistic oStat = GetActivityStatistic(ActivityId, participantId);

        //    if (oStat == null)
        //    {
        //        return InitActStatisticNoTrasaction(ActivityId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
        //    }
        //    else
        //    {
        //        return oStat;
        //    }
        //}


        //private PathStatistic GetOrInitPathStatisticNoTransaction(long PathId, int participantId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress)
        //{
        //    PathStatistic oStat = GetPathStatistic(PathId, participantId, timeToDelete);

        //    if (oStat == null)
        //    {
        //        return InitPathStatisticNoTransaction(PathId, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);
        //    }
        //    else
        //    {
        //        return oStat;
        //    }
        //}

        //private UnitStatistic GetOrInitUnitStatisticNoTransaction(long UnitId, int participantId, int evaluatorId, string creatorIPaddress, string creatorProxyIPaddress)
        //{
        //    UnitStatistic oStat = GetUnitStatistic(UnitId, participantId, timeToDelete);
        //    if (oStat == null)
        //    {
        //        return InitUnitStatisticNoTransaction(UnitId, participantId, evaluatorId, creatorIPaddress, creatorProxyIPaddress);
        //    }
        //    else
        //    {
        //        return oStat;
        //    }
        //}
        #endregion

        #region stat update auto

        //private void UpdateEpStatAuto(PathStatistic oPathStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField actFieldChanged)
        //{
        //    manager.Refresh(oPathStat);
        //    Status participantStatus = serviceEP.PathStatus(oPathStat.Path.Id, participantId, partecipatCRoleId, false);
        //    bool isUpdate = false;
        //    manager.SaveOrUpdate<PathStatistic>(oPathStat);

        //    //STATUS.STARTED
        //    if (CheckFieldChanged(actFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        isUpdate = true;
        //        oPathStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    //UPDATE COMPLETION
        //    IList<ActivityStatistic> ActiveActStats = GetActiveActStat_ByPathId(oPathStat.ChildrenStats);
        //    if (CheckFieldChanged(actFieldChanged, ModifyStatField.Completion) && UpdatePathStatCompletionAuto(oPathStat, ActiveActStats))
        //    {
        //        isUpdate = true;
        //    }

        //    //UPDATE STATUS.COMPLETED ADD 
        //    if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
        //    {

        //        if (oPathStat.Completion >= (oPathStat.Path.MinCompletion * oPathStat.Path.Weight / 100))
        //        {
        //            Int16 actMandCout = serviceEP.GetActiveMandatoryActivitiesCount_byPathId(oPathStat.Path.Id, participantId, partecipatCRoleId);
        //            if (actMandCout == oPathStat.MandatoryPassedCompletedUnitCount)
        //            {
        //                oPathStat.Status |= (StatusStatistic.CompletedPassed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                isUpdate = true;

        //            }
        //        }

        //    }//UPDATE STATUS.COMPLETED REMOVE 
        //    else if (CheckFieldChanged(actFieldChanged, ModifyStatField.CompletedRem) && CheckStatusStatistic(oPathStat.Status, StatusStatistic.Completed))
        //    {
        //        if (oPathStat.Completion <= oPathStat.Path.MinCompletion)
        //        {
        //            oPathStat.Status -= StatusStatistic.CompletedPassed;
        //            isUpdate = true;
        //        }
        //        else if (serviceEP.GetActiveMandatoryActivitiesCount_byPathId(oPathStat.Path.Id, participantId, partecipatCRoleId) != oPathStat.MandatoryPassedCompletedUnitCount)
        //        {
        //            oPathStat.Status -= StatusStatistic.CompletedPassed;
        //            isUpdate = true;
        //        }
        //    }

        //    if (isUpdate)
        //    {
        //        SetStatPathModifyMetaInfo(oPathStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);
        //    }

        //}

        //private void UpdateActStatAuto(ActivityStatistic oActStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField subActFieldChanged)
        //{
        //    ModifyStatField actFieldChanged = ModifyStatField.None;
        //    Status participantStatus = serviceEP.ActivityStatus(oActStat.Activity.Id, participantId, partecipatCRoleId, false);

        //    manager.SaveOrUpdate<ActivityStatistic>(oActStat);

        //    //STATUS.STARTED
        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        actFieldChanged |= ModifyStatField.StartedStat;
        //        oActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    //UPDATE COMPLETION
        //    IList<SubActivityStatistic> ActiveSubActStats = GetActiveSubActivityStatistic(oActStat.ChildrenStats, participantId);
        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Completion) && UpdateActStatCompletionAuto(oActStat, ActiveSubActStats))
        //    {
        //        actFieldChanged |= ModifyStatField.Completion;
        //    }

        //    //UPDATE STATUS.COMPLETED ADD 
        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
        //    {

        //        if (oActStat.Completion >= (oActStat.Activity.MinCompletion * oActStat.Activity.Weight / 100))
        //        {
        //            Int16 subActMandCout = serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, participantId, partecipatCRoleId);
        //            if (subActMandCout == oActStat.MandatoryPassedCompletedSubActivityCount)
        //            {
        //                oActStat.Status |= (StatusStatistic.CompletedPassed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                actFieldChanged |= ModifyStatField.CompletedAdd;

        //                //UPDATE ACT MANDATORY COUNT ADD
        //                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                {

        //                    PathStatistic oPathStat = oActStat.ParentStat.ParentStat;
        //                    oPathStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oPathStat.MandatoryPassedCompletedUnitCount, StatusOperation.Add);

        //                }
        //            }
        //        }

        //    }//UPDATE STATUS.COMPLETED REMOVE 
        //    else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.CompletedRem) && CheckStatusStatistic(oActStat.Status, StatusStatistic.Completed))
        //    {
        //        if (oActStat.Completion <= oActStat.Activity.MinCompletion)
        //        {
        //            oActStat.Status -= StatusStatistic.CompletedPassed;
        //            actFieldChanged |= ModifyStatField.CompletedRem;

        //            //UPDATE ACT MANDATORY COUNT remove
        //            if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //            {
        //                PathStatistic oPathStat = oActStat.ParentStat.ParentStat;
        //                oPathStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oPathStat.MandatoryPassedCompletedUnitCount, StatusOperation.Remove);

        //            }
        //        }
        //        else if (serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, participantId, partecipatCRoleId) != oActStat.MandatoryPassedCompletedSubActivityCount)
        //        {
        //            oActStat.Status -= StatusStatistic.CompletedPassed;
        //            actFieldChanged |= ModifyStatField.CompletedRem;

        //            //UPDATE ACT MANDATORY COUNT remove
        //            if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //            {
        //                PathStatistic oPathStat = oActStat.ParentStat.ParentStat;
        //                oPathStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oPathStat.MandatoryPassedCompletedUnitCount, StatusOperation.Remove);
        //            }
        //        }
        //    }

        //    if (actFieldChanged > ModifyStatField.None)
        //    {
        //        SetStatActivityModifyMetaInfo(oActStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);
        //        UnitStatistic oUnitSta = oActStat.ParentStat;
        //        manager.Refresh(oUnitSta);
        //        manager.Refresh(oUnitSta.ParentStat);

        //        UpdateEpStatAuto(oActStat.ParentStat.ParentStat, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, actFieldChanged);
        //    }

        //}

        //private void UpdateSubActStatAuto(SubActivityStatistic oSubActStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, Int64 completion, bool isStarted, bool isCompleted)
        //{
        //    //NOTE: aggiornare prima la completion dello Status Completed
        //    ModifyStatField subActFieldChanged = ModifyStatField.None;
        //    Status participantStatus = serviceEP.SubActivityStatus(oSubActStat.SubActivity.Id, participantId, partecipatCRoleId, false);


        //    if (isStarted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        subActFieldChanged |= ModifyStatField.StartedStat;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    if (isCompleted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed | StatusStatistic.Passed)))
        //    {
        //        subActFieldChanged |= ModifyStatField.CompletedAdd;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.CompletedPassed);
        //        oSubActStat.EndDate = DateTime.Now;
        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            ActivityStatistic parentStat = manager.Get<ActivityStatistic>(oSubActStat.ParentStat.Id);
        //            parentStat.MandatoryPassedCompletedSubActivityCount = updateCompPassCount(CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedSubActivityCount, StatusOperation.Add);

        //        }

        //    }
        //    else if (!isCompleted && CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed))
        //    {

        //        subActFieldChanged |= ModifyStatField.CompletedRem;
        //        oSubActStat.Status -= StatusStatistic.CompletedPassed;
        //        oSubActStat.EndDate = DateTime.Now;
        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            ActivityStatistic parentStat = manager.Get<ActivityStatistic>(oSubActStat.ParentStat.Id);
        //            parentStat.MandatoryPassedCompletedSubActivityCount = updateCompPassCount(CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedSubActivityCount, StatusOperation.Remove);
        //        }
        //    }

        //    if (oSubActStat.Completion != completion)
        //    {
        //        subActFieldChanged |= ModifyStatField.Completion;
        //        oSubActStat.Completion = completion;
        //    }


        //    if (subActFieldChanged > ModifyStatField.None)
        //    {
        //        SetStatSubActivityModifyMetaInfo(oSubActStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);
        //        manager.SaveOrUpdate<SubActivityStatistic>(oSubActStat);
        //        manager.Refresh<ActivityStatistic>(oSubActStat.ParentStat);
        //        UpdateActStatAuto(oSubActStat.ParentStat, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, subActFieldChanged);
        //    }

        //}


        #endregion

        #region stat update manual

        //private void UpdateSubActManual(SubActivityStatistic oSubActStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, Int64 completion, Int16 Mark, bool isStarted, bool isCompleted, bool isPassed)
        //{
        //    //NOTE: aggiornare prima la completion dello Status Completed
        //    ModifyStatField subActFieldChanged = ModifyStatField.None;
        //    Status participantStatus = serviceEP.SubActivityStatus(oSubActStat.SubActivity.Id, participantId, partecipatCRoleId, false);

        //    oSubActStat.ParentStat = GetOrInitActivityStatisticNoTransaction(oSubActStat.IdActivity, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

        //    if (isStarted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        subActFieldChanged |= ModifyStatField.StartedStat;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    if (oSubActStat.Completion != completion)
        //    {
        //        subActFieldChanged |= ModifyStatField.Completion;
        //        oSubActStat.Completion = completion;
        //    }

        //    if (isCompleted && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed)))
        //    {
        //        subActFieldChanged |= ModifyStatField.CompletedAdd;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Completed);
        //        oSubActStat.EndDate = DateTime.Now;
        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            ActivityStatistic parentStat = manager.Get<ActivityStatistic>(oSubActStat.ParentStat.Id);
        //            parentStat.MandatoryCompletedSubActivityCount++;
        //            parentStat.MandatoryPassedCompletedSubActivityCount = updateCompPassCount(CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedSubActivityCount, StatusOperation.Add);

        //        }
        //    }
        //    else if (!isCompleted && CheckStatusStatistic(oSubActStat.Status, StatusStatistic.Completed))
        //    {

        //        subActFieldChanged |= ModifyStatField.CompletedRem;
        //        oSubActStat.Status -= StatusStatistic.Completed;
        //        oSubActStat.EndDate = DateTime.Now;
        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            ActivityStatistic parentStat = oSubActStat.ParentStat;
        //            parentStat.MandatoryCompletedSubActivityCount--;
        //            parentStat.MandatoryPassedCompletedSubActivityCount = updateCompPassCount(CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedSubActivityCount, StatusOperation.Remove);

        //        }
        //    }

        //    subActFieldChanged |= SubUpdateSubActStaMarkAndStatusPassed(oSubActStat, Mark, isPassed, participantStatus);

        //    if (subActFieldChanged > ModifyStatField.None)
        //    {
        //        SetStatSubActivityModifyMetaInfo(oSubActStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);


        //        UpdateActStatManual(oSubActStat.ParentStat, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, subActFieldChanged);
        //    }
        //}
                
        //private void UpdateActStatManual(ActivityStatistic oActStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField subActFieldChanged)
        //{
        //    manager.Refresh(oActStat);
        //    ModifyStatField actFieldChanged = ModifyStatField.None;
        //    Status participantStatus = serviceEP.ActivityStatus(oActStat.Activity.Id, participantId, partecipatCRoleId, false);
        //    oActStat.ParentStat = GetOrInitUnitStatisticNoTransaction(oActStat.IdUnit, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

        //    manager.SaveOrUpdate<ActivityStatistic>(oActStat);

        //    //STATUS.STARTED
        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        actFieldChanged |= ModifyStatField.StartedStat;
        //        oActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    //UPDATE COMPLETION
        //    IList<SubActivityStatistic> ActiveSubActStats = GetActiveSubActivityStatistic(oActStat.ChildrenStats, participantId);
        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Completion) && UpdateActStatCompletionManual(oActStat, ActiveSubActStats))
        //    {
        //        actFieldChanged |= ModifyStatField.Completion;
        //    }

        //    //UPDATE STATUS.COMPLETED ADD //REMOVE
        //    actFieldChanged |= UpdateCompletedStatus_byActStat(oActStat, participantStatus, subActFieldChanged);


        //    //MArk and Passed Status
        //    actFieldChanged |= SubUpdateActStaMarkAndStatusPassed(oActStat, subActFieldChanged);

        //    if (actFieldChanged > ModifyStatField.None)
        //    {
        //        SetStatActivityModifyMetaInfo(oActStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);

        //        UpdateUnitStatManual(oActStat.ParentStat, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, actFieldChanged);
        //    }

        //}
            
        //private void UpdateUnitStatManual(UnitStatistic oUnitStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField actFieldChanged)
        //{
        //    manager.Refresh(oUnitStat);
        //    ModifyStatField unitFieldChanged = ModifyStatField.None;
        //    Status participantStatus = serviceEP.UnitStatus(oUnitStat.Unit.Id, participantId, partecipatCRoleId, false);
        //    oUnitStat.ParentStat = GetOrInitPathStatisticNoTransaction(oUnitStat.IdPath, participantId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress);

        //    manager.SaveOrUpdate<UnitStatistic>(oUnitStat);

        //    //STATUS.STARTED
        //    if (CheckFieldChanged(actFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        unitFieldChanged |= ModifyStatField.StartedStat;
        //        oUnitStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    //UPDATE COMPLETION
        //    IList<ActivityStatistic> ActiveActStats = GetActiveActivityStatistic(oUnitStat.ChildrenStats, participantId, partecipatCRoleId);
        //    if (CheckFieldChanged(actFieldChanged, ModifyStatField.Completion) && UpdateUnitStatCompletionManual(oUnitStat, ActiveActStats))
        //    {
        //        unitFieldChanged |= ModifyStatField.Completion;
        //    }

        //    //UPDATE STATUS.COMPLETED ADD //REMOVE
        //    unitFieldChanged |= UpdateCompletedStatus_byUnitStat(oUnitStat, participantStatus, actFieldChanged);


        //    //MArk and Passed Status
        //    unitFieldChanged |= SubUpdateUnitStaMarkAndStatusPassed(oUnitStat, actFieldChanged);

        //    if (unitFieldChanged > ModifyStatField.None)
        //    {
        //        SetStatUnitModifyMetaInfo(oUnitStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);
        //        //update EP
        //        UpdateEpStatManual(oUnitStat.ParentStat, participantId, partecipatCRoleId, evaluatorId, evaluatorIPaddress, evaluatorProxyIPaddress, unitFieldChanged);
        //    }

        //}

        //private void UpdateEpStatManual(PathStatistic oPathStat, int participantId, int partecipatCRoleId, int evaluatorId, string evaluatorIPaddress, string evaluatorProxyIPaddress, ModifyStatField unitFieldChanged)
        //{
        //    manager.Refresh(oPathStat);
        //    Status participantStatus = serviceEP.PathStatus(oPathStat.Path.Id, participantId, partecipatCRoleId, false);

        //    manager.SaveOrUpdate<PathStatistic>(oPathStat);

        //    //STATUS.STARTED
        //    if (CheckFieldChanged(unitFieldChanged, ModifyStatField.StartedStat) && !CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        oPathStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started);
        //    }

        //    //UPDATE COMPLETION           
        //    if (CheckFieldChanged(unitFieldChanged, ModifyStatField.Completion))
        //    {
        //        UpdatePathStatCompletionManual(oPathStat, GetActiveUnitStatistic(oPathStat.ChildrenStats, participantId, partecipatCRoleId));
        //    }

        //    //UPDATE STATUS.COMPLETED ADD //REMOVE
        //    UpdateCompletedStatus_byPathStat(oPathStat, participantStatus, unitFieldChanged);


        //    //MArk and Passed Status
        //    SubUpdateEpStaMarkAndStatusPassed(oPathStat, unitFieldChanged);

        //    if (unitFieldChanged > ModifyStatField.None)
        //    {
        //        SetStatPathModifyMetaInfo(oPathStat, manager.GetPerson(evaluatorId), evaluatorIPaddress, evaluatorProxyIPaddress, DateTime.Now);

        //    }

        //}

        #endregion

        #region Get Active Item
        //private IList<SubActivityStatistic> GetActiveSubActivityStatistic(IList<SubActivityStatistic> SubActivityStatistics, int userId)
        //{
        //    if (SubActivityStatistics == null)
        //    {
        //        return new List<SubActivityStatistic>();
        //    }


        //    IList<SubActivityStatistic> temp = (from stat in SubActivityStatistics
        //                                        where stat.Deleted == BaseStatusDeleted.None && stat.SubActivity.Deleted == BaseStatusDeleted.None
        //                                        select stat).ToList();
        //    return temp;
        //}

        //private IList<ActivityStatistic> GetActiveActivityStatistic(IList<ActivityStatistic> ActivityStatistics, int userId, int croleID)
        //{
        //    if (ActivityStatistics == null)
        //    {
        //        return new List<ActivityStatistic>();
        //    }
        //    return (from stat in ActivityStatistics
        //            where stat.Deleted == BaseStatusDeleted.None && stat.Activity.Deleted == BaseStatusDeleted.None
        //            select stat).ToList();
        //}

        //private IList<ActivityStatistic> GetActiveActivityStatistic(long parentStatId)
        //{

        //    return (from stat in manager.GetIQ<ActivityStatistic>()
        //            where stat.ParentStat.Id == parentStatId && stat.Deleted == BaseStatusDeleted.None && stat.Activity.Deleted == BaseStatusDeleted.None
        //            select stat).ToList();
        //}

        //private IList<UnitStatistic> GetActiveUnitStatistic(IList<UnitStatistic> UnitStatistics, int userId, int croleID)
        //{
        //    if (UnitStatistics == null)
        //    {
        //        return new List<UnitStatistic>();
        //    }
        //    return (from stat in UnitStatistics
        //            where stat.Deleted == BaseStatusDeleted.None && stat.Unit.Deleted == BaseStatusDeleted.None
        //            select stat).ToList();
        //}

        #endregion

        #region sub update mark / status passed
        //private ModifyStatField SubUpdateActStaMarkAndStatusPassed(ActivityStatistic oActStat, ModifyStatField subActFieldChanged)
        //{

        //    ModifyStatField actFieldChanged = ModifyStatField.None;

        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Mark))
        //    {
        //        actFieldChanged |= UpdateActStatMarkManual(oActStat, GetActiveSubActivityStatistic(oActStat.ChildrenStats, 0));
        //    }

        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedAdd) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        if (oActStat.Mark >= oActStat.Activity.MinMark)
        //        {
        //            Int16 subActMandCount = serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0);
        //            if (subActMandCount == oActStat.MandatoryPassedSubActivityCount)
        //            {
        //                Int16 PassedCompletion = (Int16)(100 * GetSubActWeightSum_byStatusStat(oActStat.ChildrenStats, StatusStatistic.Passed) / serviceEP.GetActiveSubActivitiesWeightSum(oActStat.Activity.SubActivityList));
        //                if (PassedCompletion >= oActStat.Activity.MinCompletion)
        //                {
        //                    oActStat.Status |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                    actFieldChanged |= ModifyStatField.PassedAdd;
        //                }

        //                if (serviceEP.ActivityIsMandatoryForParticipant(oActStat.Activity.Id, 0, 0))
        //                {
        //                    UnitStatistic oParentStat = manager.Get<UnitStatistic>(oActStat.ParentStat.Id);
        //                    oParentStat.MandatoryPassedActivityCount++;
        //                    oParentStat.MandatoryPassedCompletedActivityCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oParentStat.MandatoryPassedCompletedActivityCount, StatusOperation.Add);

        //                }

        //            }
        //        }
        //    }
        //    else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedRem) && CheckStatusStatistic(oActStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {

        //        if (oActStat.Mark <= oActStat.Activity.MinMark)
        //        {
        //            actFieldChanged |= SubRemoveStatusOperation_byActStat(oActStat);
        //        }
        //        else if (serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0) != oActStat.MandatoryPassedSubActivityCount)
        //        {
        //            actFieldChanged |= SubRemoveStatusOperation_byActStat(oActStat);
        //        }
        //        else if ((Int16)(100 * GetSubActWeightSum_byStatusStat(oActStat.ChildrenStats, StatusStatistic.Passed) / serviceEP.GetActiveSubActivitiesWeightSum(oActStat.Activity.SubActivityList)) < oActStat.Activity.MinCompletion)
        //        {
        //            actFieldChanged |= SubRemoveStatusOperation_byActStat(oActStat);
        //        }
        //    }


        //    return actFieldChanged;
        //}

        //private ModifyStatField SubUpdateUnitStaMarkAndStatusPassed(UnitStatistic oUnitStat, ModifyStatField subActFieldChanged)
        //{

        //    ModifyStatField unitFieldChanged = ModifyStatField.None;

        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.Mark))
        //    {
        //        unitFieldChanged |= UpdateUnitStatMarkManual(oUnitStat, GetActiveActivityStatistic(oUnitStat.ChildrenStats, 0, 0));
        //    }

        //    if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedAdd) && !CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {
        //        if (oUnitStat.Mark >= oUnitStat.Unit.MinMark)
        //        {
        //            Int16 actMandCount = serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0);
        //            if (actMandCount == oUnitStat.MandatoryPassedActivityCount)
        //            {

        //                Int16 PassedCompletion = (Int16)(100 * GetActWeightSum_byStatusStat(oUnitStat.ChildrenStats, StatusStatistic.Passed) / serviceEP.GetActiveActivitiesWeightSum(oUnitStat.Unit.Id));
        //                if (PassedCompletion >= oUnitStat.Unit.MinCompletion)
        //                {
        //                    oUnitStat.Status |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                    unitFieldChanged |= ModifyStatField.PassedAdd;
        //                }

        //                if (serviceEP.UnitIsMandatoryForParticipant(oUnitStat.Unit.Id, 0, 0))
        //                {
        //                    PathStatistic oParentStat = oUnitStat.ParentStat;
        //                    oParentStat.MandatoryPassedUnitCount++;
        //                    oParentStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed), oParentStat.MandatoryPassedCompletedUnitCount, StatusOperation.Add);

        //                }
        //            }
        //        }
        //    }
        //    else if (CheckFieldChanged(subActFieldChanged, ModifyStatField.PassedRem) && CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {

        //        if (oUnitStat.Mark <= oUnitStat.Unit.MinMark)
        //        {
        //            unitFieldChanged |= SubRemoveStatusOperation_byUnitStat(oUnitStat);
        //        }
        //        else if (serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0) != oUnitStat.MandatoryPassedActivityCount)
        //        {
        //            unitFieldChanged |= SubRemoveStatusOperation_byUnitStat(oUnitStat);
        //        }
        //        else if ((Int16)(100 * GetActWeightSum_byStatusStat(oUnitStat.ChildrenStats, StatusStatistic.Passed) / serviceEP.GetActiveActivitiesWeightSum(oUnitStat.Unit.Id)) < oUnitStat.Unit.MinCompletion)
        //        {
        //            unitFieldChanged |= SubRemoveStatusOperation_byUnitStat(oUnitStat);
        //        }
        //    }


        //    return unitFieldChanged;
        //}

        //private void SubUpdateEpStaMarkAndStatusPassed(PathStatistic oEpStat, ModifyStatField unitFieldChanged)
        //{

        //    if (CheckFieldChanged(unitFieldChanged, ModifyStatField.Mark))
        //    {
        //        UpdatePathStatMarkManual(oEpStat, GetActiveUnitStatistic(oEpStat.ChildrenStats, 0, 0));
        //    }

        //    if (CheckFieldChanged(unitFieldChanged, ModifyStatField.PassedAdd))
        //    {

        //        if (!CheckStatusStatistic(oEpStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {

        //            if (oEpStat.Mark >= oEpStat.Path.MinMark)
        //            {
        //                Int16 unittMandCout = serviceEP.GetActiveMandatoryUnitCount(oEpStat.Path.Id, 0, 0);
        //                if (unittMandCout == oEpStat.MandatoryPassedUnitCount)
        //                {
        //                    Int16 PassedCompletion = (Int16)(100 * GetUnitStatWeightSum_byStatusStat(oEpStat.ChildrenStats, StatusStatistic.Passed) / serviceEP.GetActiveUnitsWeightSum(oEpStat.Path.Id));
        //                    if (PassedCompletion >= oEpStat.Path.MinCompletion)
        //                    {
        //                        oEpStat.Status |= (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                    }
        //                }

        //            }
        //        }

        //    }
        //    else if (CheckFieldChanged(unitFieldChanged, ModifyStatField.PassedRem))
        //    {
        //        if (CheckStatusStatistic(oEpStat.Status, (StatusStatistic.Passed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {
        //            if (oEpStat.Mark <= oEpStat.Path.MinMark)
        //            {
        //                oEpStat.Status -= StatusStatistic.Passed;
        //            }
        //            else if (serviceEP.GetActiveMandatoryUnitCount(oEpStat.Path.Id, 0, 0) != oEpStat.MandatoryPassedUnitCount)
        //            {
        //                oEpStat.Status -= StatusStatistic.Passed;
        //            }
        //            else if ((Int16)(100 * GetUnitStatWeightSum_byStatusStat(oEpStat.ChildrenStats, StatusStatistic.Passed) / serviceEP.GetActiveUnitsWeightSum(oEpStat.Path.Id)) < oEpStat.Path.MinCompletion)
        //            {
        //                oEpStat.Status -= StatusStatistic.Passed;
        //            }
        //        }
        //    }
        //}

        //private ModifyStatField SubUpdateSubActStaMarkAndStatusPassed(SubActivityStatistic oSubActStat, Int16 Mark, bool isPassed, Status participantStatus)
        //{

        //    ModifyStatField subActFieldChanged = ModifyStatField.None;

        //    //  bool isMarkChanged = false, isUpdated=false;
        //    if (oSubActStat.Mark != Mark)
        //    {
        //        subActFieldChanged |= ModifyStatField.Mark;
        //        oSubActStat.Mark = Mark;

        //    }

        //    //    StatusOperation OperationStatPass = StatusOperation.None;
        //    if (isPassed && !CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Passed)))
        //    {
        //        subActFieldChanged |= ModifyStatField.PassedAdd;
        //        oSubActStat.Status |= (StatusStatistic.Browsed | StatusStatistic.Started | StatusStatistic.Passed);
        //        oSubActStat.EndDate = DateTime.Now;

        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {
        //            ActivityStatistic parentStat = manager.Get<ActivityStatistic>(oSubActStat.ParentStat.Id);
        //            parentStat.MandatoryPassedSubActivityCount++;
        //            parentStat.MandatoryPassedCompletedSubActivityCount = updateCompPassCount(CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedSubActivityCount, StatusOperation.Add);

        //        }
        //    }
        //    else if (!isPassed && CheckStatusStatistic(oSubActStat.Status, (StatusStatistic.Passed)))
        //    {
        //        subActFieldChanged |= ModifyStatField.PassedRem;
        //        oSubActStat.Status -= StatusStatistic.Passed;

        //        if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //        {

        //            ActivityStatistic parentStat = manager.Get<ActivityStatistic>(oSubActStat.ParentStat.Id);
        //            parentStat.MandatoryPassedSubActivityCount--;
        //            parentStat.MandatoryPassedCompletedSubActivityCount = updateCompPassCount(CheckStatusStatistic(oSubActStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedSubActivityCount, StatusOperation.Remove);

        //        }
        //    }

        //    return subActFieldChanged;
        //}

        #endregion

        #region update completed status
        //private ModifyStatField UpdateCompletedStatus_byActStat(ActivityStatistic oActStat, Status participantStatus, ModifyStatField Operation)
        //{

        //    if (CheckFieldChanged(Operation, ModifyStatField.CompletedAdd) && !CheckStatusStatistic(oActStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //    {

        //        if (oActStat.Completion >= oActStat.Activity.MinCompletion)
        //        {
        //            Int16 subActMandCout = serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0);
        //            if (subActMandCout == oActStat.MandatoryCompletedSubActivityCount)
        //            {
        //                oActStat.Status |= (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started);

        //                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                {
        //                    oActStat.ParentStat.MandatoryCompletedActivityCount++;
        //                    oActStat.ParentStat.MandatoryPassedCompletedActivityCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oActStat.ParentStat.MandatoryPassedCompletedActivityCount, StatusOperation.Add);

        //                }
        //                return ModifyStatField.CompletedAdd;
        //            }
        //        }

        //    }
        //    else if (CheckFieldChanged(Operation, ModifyStatField.CompletedRem))
        //    {
        //        if (CheckStatusStatistic(oActStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {

        //            if (oActStat.Completion <= oActStat.Activity.MinCompletion)
        //            {
        //                oActStat.Status -= StatusStatistic.Completed;

        //                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                {
        //                    oActStat.ParentStat.MandatoryCompletedActivityCount--;
        //                    oActStat.ParentStat.MandatoryPassedCompletedActivityCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oActStat.ParentStat.MandatoryPassedCompletedActivityCount, StatusOperation.Remove);

        //                }
        //                return ModifyStatField.CompletedRem;

        //            }
        //            else if (serviceEP.GetActiveMandatorySubActivitiesCount(oActStat.Activity.SubActivityList, 0, 0) != oActStat.MandatoryCompletedSubActivityCount)
        //            {
        //                oActStat.Status -= StatusStatistic.Completed;

        //                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                {
        //                    oActStat.ParentStat.MandatoryCompletedActivityCount--;
        //                    oActStat.ParentStat.MandatoryPassedCompletedActivityCount = updateCompPassCount(CheckStatusStatistic(oActStat.Status, StatusStatistic.CompletedPassed), oActStat.ParentStat.MandatoryPassedCompletedActivityCount, StatusOperation.Remove);

        //                }
        //                return ModifyStatField.CompletedRem;
        //            }

        //        }
        //    }
        //    return ModifyStatField.None;
        //}

        //private ModifyStatField UpdateCompletedStatus_byUnitStat(UnitStatistic oUnitStat, Status participantStatus, ModifyStatField Operation)
        //{

        //    if (CheckFieldChanged(Operation, ModifyStatField.CompletedAdd))
        //    {
        //        if (!CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {

        //            if (oUnitStat.Completion >= oUnitStat.Unit.MinCompletion)
        //            {
        //                Int16 actMandCout = serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0);
        //                if (actMandCout == oUnitStat.MandatoryCompletedActivityCount)
        //                {
        //                    oUnitStat.Status |= (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                    if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                    {
        //                        PathStatistic parentStat = oUnitStat.ParentStat;
        //                        parentStat.MandatoryCompletedUnitCount++;
        //                        parentStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedUnitCount, StatusOperation.Add);

        //                    }
        //                    return ModifyStatField.CompletedAdd;
        //                }
        //            }
        //        }
        //    }
        //    else if (CheckFieldChanged(Operation, ModifyStatField.CompletedRem))
        //    {
        //        if (CheckStatusStatistic(oUnitStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {
        //            if (oUnitStat.Completion <= oUnitStat.Unit.MinCompletion)
        //            {
        //                oUnitStat.Status -= StatusStatistic.Completed;
        //                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                {
        //                    PathStatistic parentStat = oUnitStat.ParentStat;
        //                    parentStat.MandatoryCompletedUnitCount--;
        //                    parentStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedUnitCount, StatusOperation.Remove);


        //                }
        //                return ModifyStatField.CompletedRem;
        //            }
        //            else if (serviceEP.GetActiveMandatoryActivitiesCount(oUnitStat.Unit.Id, 0, 0) != oUnitStat.MandatoryCompletedActivityCount)
        //            {
        //                oUnitStat.Status -= StatusStatistic.Completed;
        //                if (serviceEP.CheckStatus(participantStatus, Status.Mandatory))
        //                {
        //                    PathStatistic parentStat = oUnitStat.ParentStat;
        //                    parentStat.MandatoryCompletedUnitCount--;
        //                    parentStat.MandatoryPassedCompletedUnitCount = updateCompPassCount(CheckStatusStatistic(oUnitStat.Status, StatusStatistic.CompletedPassed), parentStat.MandatoryPassedCompletedUnitCount, StatusOperation.Remove);

        //                }
        //                return ModifyStatField.CompletedRem;
        //            }
        //        }
        //    }
        //    return ModifyStatField.None;
        //}

        //private void UpdateCompletedStatus_byPathStat(PathStatistic oPathStat, Status participantStatus, ModifyStatField Operation)
        //{

        //    if (CheckFieldChanged(Operation, ModifyStatField.CompletedAdd))
        //    {
        //        if (!CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {
        //            if (oPathStat.Completion >= oPathStat.Path.MinCompletion)
        //            {
        //                Int16 unitMandCout = serviceEP.GetActiveMandatoryUnitCount(oPathStat.Path.Id, 0, 0);
        //                if (unitMandCout == oPathStat.MandatoryCompletedUnitCount)
        //                {
        //                    oPathStat.Status |= (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started);
        //                }
        //            }
        //        }
        //    }
        //    else if (CheckFieldChanged(Operation, ModifyStatField.CompletedRem))
        //    {
        //        if (CheckStatusStatistic(oPathStat.Status, (StatusStatistic.Completed | StatusStatistic.Browsed | StatusStatistic.Started)))
        //        {

        //            if (oPathStat.Completion <= oPathStat.Path.MinCompletion)
        //            {
        //                oPathStat.Status -= StatusStatistic.Completed;
        //            }
        //            else if (serviceEP.GetActiveMandatoryUnitCount(oPathStat.Path.Id, 0, 0) != oPathStat.MandatoryCompletedUnitCount)
        //            {
        //                oPathStat.Status -= StatusStatistic.Completed;
        //            }
        //        }
        //    }
        //}

        #endregion

        //public ActivityStatistic GetActivityStatistic(long ActivityId, int participantId)
        //{
        //    IList<ActivityStatistic> ListOfStat = manager.GetAll<ActivityStatistic>(stat => stat.Person.Id == participantId && stat.Activity.Id == ActivityId && stat.Activity.Deleted == BaseStatusDeleted.None && stat.Deleted == BaseStatusDeleted.None);

        //    if (ListOfStat.Count == 1)
        //    {
        //        return ListOfStat[0];
        //    }

        //    return null;
        //}

        #region get status sta
        //public IList<dtoStatusStatItem> GetUnitsStatusStats(long pathId, int userId)
        //{
        //    return (from stat in manager.GetIQ<UnitStatistic>()
        //            where stat.Unit.ParentPath.Id == pathId
        //            select new dtoStatusStatItem()
        //            {
        //                idItem = stat.Unit.Id,
        //                statusStat = stat.Status
        //            }).ToList();        
        //}

        //public IList<dtoStatusStatItem> GetActivitiesStatusStats(long pathId, int userId)
        //{
        //    return (from stat in manager.GetIQ<ActivityStatistic>()
        //            where stat.Activity.Path.Id == pathId
        //            select new dtoStatusStatItem()
        //            {
        //                idItem = stat.Activity.Id,
        //                statusStat = stat.Status
        //            }).ToList();
        //}
        #endregion

        #endregion

        
#region CokadeInfo

        public bool CokadeEnable(int communityId)
        {
            return (from MoocCokade ck in manager.GetIQ<MoocCokade>()
                        where ck.CokadeEnable select ck.Id).Any();
        }

        public IList<dtoCokadeInfo> CokadeCommunityInfo(
            int communityId,
            int personId)
        {
             IList<dtoCokadeInfo> outList = new List<dtoCokadeInfo>();

            IList<dtoEPitemList> EPList = this.serviceEP.GetMyEduPaths(
                personId,
                communityId,
                manager.GetIdCommunityRole(personId, communityId),
                EpViewModeType.View,
                true);

            foreach (dtoEPitemList dtoItem in EPList)
            {
                dtoStatWithWeight statForBar = this.serviceEP.ServiceStat.GetPassedCompletedWeight_byActivity(
                    dtoItem.Id, personId, DateTime.Now);

                StatusStatistic moocStatus = this.GetEpUserStatus(dtoItem.Id, personId);

                dtoCokadeInfo info = new dtoCokadeInfo();
                info.CommunityId = communityId;
                info.PathId = dtoItem.Id;
                info.PathName = dtoItem.Name;

                info.Info = new dtoCokadeMoocInfo();
                info.Info.Completion = statForBar.Completion;
                info.Info.MinCompletion = statForBar.MinCompletion;
                info.Info.mType = dtoItem.moocType;
                info.Info.mookCompleted = CheckStatusStatistic(moocStatus, StatusStatistic.Completed)
                                          || CheckStatusStatistic(moocStatus, StatusStatistic.CompletedPassed)
                                          || CheckStatusStatistic(moocStatus, StatusStatistic.Passed);

                outList.Add(info);
            }


            return outList;

            //'CurrentContext.UserContext.CurrentUserID, CurrentCommunityID, CurrentCommRoleID, Me.ViewModeType, IsMoocPath)
        }

        public void ActionStatistics()
        {
            
        }


#endregion

        private enum StatusOperation
        {
            None, Add, Remove
        }

        [FlagsAttribute]
        public enum ModifyStatField 
        {
            None = 0,
            StartedStat = 1,
            CompletedAdd = 2,
            CompletedRem = 4,
            Completion = 8,
            PassedAdd=16,
            PassedRem=32,
            Mark=64,
            ComplPassMandatoryAdd=128,
            ComplPassMandatoryRem=256,            
            CompletedMandatoryAdd=512,
            CompletedMandatoryRem=1024,
            PassedMandatoryAdd = 2048,
            PassedMandatoryRem = 4096,
            Updated=8192,
            CompletionIncreased = 16384,
            CompletionDecreased = 32768
        }

    }
}

public enum StatusFilter
{    
    all,
    unlocked,
    locked
}
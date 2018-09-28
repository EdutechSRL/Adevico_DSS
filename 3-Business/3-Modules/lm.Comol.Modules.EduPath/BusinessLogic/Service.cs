using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using System.Diagnostics;
using Entity = Comol.Entity;
using lm.Comol.Modules.Base.BusinessLogic;
using NHibernate;
using System.Linq.Expressions;
using lm.Comol.Modules.EduPath.Domain.DTO;
using COL_BusinessLogic_v2.CL_permessi;
using COL_BusinessLogic_v2.Comunita;
using lm.Comol.Core.BaseModules.CommunityManagement.Business;
//using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{

    public partial class Service
    {
        private const Int32 maxItemsForSingleQuery = 1000;
        protected iApplicationContext _Context;
        private ManagerEP Manager { get; set; }
        private bool RulesChecked { get { return true; } }

        #region Services
            private ServiceStat _ServiceStat;
            public ServiceStat ServiceStat
            {
                get
                {
                    if (_ServiceStat == null)
                    {
                        _ServiceStat = new ServiceStat(Manager, this);
                    }
                    return _ServiceStat;
                }
            }

            private ServiceAssignment _ServiceAssignments;
            public ServiceAssignment ServiceAssignments
            {
                get
                {
                    if (_ServiceAssignments == null)
                    {
                        _ServiceAssignments = new ServiceAssignment(Manager, this);
                    }
                    return _ServiceAssignments;
                }
            }

            private lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService pmService;
            public lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService PMService
            {
                get
                {
                    if (pmService == null)
                    {
                        pmService = new lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(_Context);
                    }
                    return pmService;
                }
            }
            private lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities _ServiceDashboard;
            public lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities ServiceDashboard
            {
                get
                {
                    if (_ServiceDashboard == null)
                    {
                        _ServiceDashboard = new lm.Comol.Core.BaseModules.Dashboard.Business.ServiceDashboardCommunities(_Context);
                    }
                    return _ServiceDashboard;
                }
            }
            private ServiceCommunityManagement _ServiceCommunityManagement;
            public ServiceCommunityManagement ServiceCommunityManagement
            {
                get
                {
                    if (_ServiceCommunityManagement == null)
                    {
                        _ServiceCommunityManagement = new ServiceCommunityManagement(_Context);
                    }
                    return _ServiceCommunityManagement;
                }
            }
            private lm.Comol.Core.FileRepository.Business.ServiceFileRepository _ServiceRepository;
            private lm.Comol.Core.FileRepository.Business.ServiceFileRepository ServiceRepository
            {
                get
                {
                    if (_ServiceRepository == null)
                        _ServiceRepository = new lm.Comol.Core.FileRepository.Business.ServiceFileRepository(_Context);
                    return _ServiceRepository;
                }
            }
            //private lm.Comol.Core.Certifications.Business.CertificationsService _ServiceCertifications;
            //public lm.Comol.Core.Certifications.Business.CertificationsService ServiceCertifications
            //{
            //    get
            //    {
            //        if (_ServiceCertifications == null)
            //            _ServiceCertifications = new lm.Comol.Core.Certifications.Business.CertificationsService(_Context);
            //        return _ServiceCertifications;
            //    }
            //}
        #endregion

        #region initClass
            protected iUserContext UC { set; get; }
            public Service() { }

            public Service(iApplicationContext oContext)
            {
                this.Manager = new ManagerEP(oContext.DataContext);
                this.UC = oContext.UserContext;
                _Context = oContext;
            }

            public Service(iDataContext oDC)
            {
                this.Manager = new ManagerEP(oDC);
                _Context = new ApplicationContext() { DataContext = oDC };
            }

            public Service(ManagerEP Manager)
            {
                this.Manager = Manager;
            }

        #endregion

            public iApplicationContext Context { get { return _Context; } }

        public String GetUserName(int userId)
        {
            return Manager.GetPerson(userId).SurnameAndName;
        }

        public void DeleteActivityRule(long id)
        {
            try
            {
                Manager.BeginTransaction();
                RuleActivityCompletion rule = Manager.Get<RuleActivityCompletion>(id);
                Manager.DeletePhysical<RuleActivityCompletion>(rule);

                Manager.Commit();
            }
            catch
            {
                Manager.RollBack();
            }
        }

        public void DeleteUnitRule(long id)
        {
            try
            {
                Manager.BeginTransaction();
                RuleUnitCompletion rule = Manager.Get<RuleUnitCompletion>(id);
                Manager.DeletePhysical<RuleUnitCompletion>(rule);

                Manager.Commit();
            }
            catch
            {
                Manager.RollBack();
            }
        }

        public IList<Role> GetActiveRoles(Int32 idcommunity)
        {
            return Manager.GetActiveRoles(idcommunity);
        }
       

        #region SaveUpdate EP

        public Path SaveEP(Path path, Int32 idCreator, string proxyIPaddress, string IPaddress)
        {

            try
            {
                Manager.BeginTransaction();
                path.CreateMetaInfo(idCreator, IPaddress, proxyIPaddress);
                Manager.SaveOrUpdate<Path>(path);
                Manager.Commit();
                return path;
            }
            catch (Exception ex)
            {

                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }

        public Path SaveEPNoTransaction(Path path, Int32 idCreator, string proxyIPaddress, string IPaddress)
        {

            try
            {
                path.CreateMetaInfo(idCreator, IPaddress, proxyIPaddress);
                Manager.SaveOrUpdate<Path>(path);

                return path;
            }
            catch (Exception ex)
            {

                
                Debug.Write(ex);
                return null;
            }
        }

        public bool DeleteOnlyEP(long PathId)
        {
            try
            {
                Manager.BeginTransaction();
                Manager.DeleteOnlyEP(PathId);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }
        private Int16 GetNewPathDisplayOrder(int ComId)
        {
            try
            {
                Int16 displayOrder = (from path in Manager.GetAll<Path>(p => p.Community.Id == ComId && p.Deleted == BaseStatusDeleted.None).ToList() where !((path.Status & Status.Draft) == Status.Draft) select path.DisplayOrder).Max();
                displayOrder++;
                return displayOrder;
            }
            catch (Exception)
            {
                return 1;
                throw;
            }
        }
        private void SetPathPartialDetail(Path outputPath, Path inputPath)
        {
            outputPath.Name = inputPath.Name;
            outputPath.Description = inputPath.Description;
            outputPath.IsMooc = inputPath.IsMooc;
            outputPath.Duration = inputPath.Duration;
            outputPath.MinCompletion = inputPath.MinCompletion;
            outputPath.Status = inputPath.Status;
            outputPath.MinMark = inputPath.MinMark;
            outputPath.Policy.Statistics = inputPath.Policy.Statistics;
            outputPath.Policy.DisplaySubActivity = inputPath.Policy.DisplaySubActivity;
        }
        public bool SaveOrUpdateEPandAssignment(Path oPath, List<dtoGenericAssignment> dtoCRoleAssignments, List<dtoGenericAssignmentWithDelete> dtoPersonAssignments, int CurrentCommunityID, int PersonID, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                DateTime TimeNow = DateTime.Now;
                Manager.BeginTransaction();
                if ((oPath.Status & Status.Draft) == Status.Draft)//nuovo
                {
                    Path oPathNew = Manager.Get<Path>(oPath.Id);
                    SetPathPartialDetail(oPathNew, oPath);

                    oPathNew.Community = Manager.GetLiteCommunity(CurrentCommunityID);
                    oPathNew.DisplayOrder = GetNewPathDisplayOrder(CurrentCommunityID);

                    ServiceAssignments.SaveNewPathPersonAssignmentsNoTranction(dtoPersonAssignments, oPathNew, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                    ServiceAssignments.SaveNewPathCRoleAssignmentsNoTransaction(dtoCRoleAssignments, oPathNew, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                    Manager.SaveOrUpdate<Path>(oPathNew);
                }
                else //update
                {
                    Path oPathToUpdate = Manager.Get<Path>(oPath.Id);
                    SetPathPartialDetail(oPathToUpdate, oPath);
                    oPathToUpdate.UpdateMetaInfo(PersonID, PersonIpAddress, PersonProxyIpAddress, TimeNow);
                    Manager.SaveOrUpdate(oPathToUpdate);
                    //esistenti da eliminare
                    List<long> IdCRoleAssToDelete = (from item in dtoCRoleAssignments where item.DB_ID > 0 && item.RoleEP == 0 select item.DB_ID).ToList();
                    List<long> idPersonAssToDelete = (from item in dtoPersonAssignments where item.DB_ID > 0 && (item.RoleEP == 0 || item.isDeleted) select item.DB_ID).ToList();
                    ServiceAssignments.DeletePathCRoleAssignments(IdCRoleAssToDelete);
                    ServiceAssignments.DeletePathPersonAssignments(idPersonAssToDelete);
                    //esistenti da aggiornare
                    List<dtoGenericAssignmentWithDelete> dtoPersonAssToUpdate = (from item in dtoPersonAssignments where !item.isDeleted && item.DB_ID > 0 && item.RoleEP > 0 select item).ToList();
                    List<dtoGenericAssignment> dtoCRoleAssToUpdate = (from item in dtoCRoleAssignments where item.DB_ID > 0 && item.RoleEP > 0 select item).ToList();
                    ServiceAssignments.UpdateRoleAssignmentsPathNoTransaction(dtoPersonAssToUpdate, dtoCRoleAssToUpdate, PersonID, PersonIpAddress, PersonProxyIpAddress, TimeNow);

                    //nuovi
                    ServiceAssignments.SaveNewPathPersonAssignmentsNoTranction(dtoPersonAssignments, oPath, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                    ServiceAssignments.SaveNewPathCRoleAssignmentsNoTransaction(dtoCRoleAssignments, oPath, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                }

                Manager.Commit();

            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }


            public dtoBasePath GetBasePath(long idPath)
            {
                return (from p in Manager.Linq<Path>()
                        where p.Id == idPath
                        select new dtoBasePath() { Id = p.Id, EndDate = p.EndDate, EndDateOverflow = p.EndDateOverflow, EndSpan = p.EndSpan, FloatingDeadlines = p.FloatingDeadlines, Name = p.Name, SingleAction = p.SingleAction, StartDate = p.StartDate, StartSpan = p.StartSpan, IsMooc= p.IsMooc }).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public Path GetPath(long PathId)
            {
                return Manager.Get<Path>(PathId);
            }
            public String GetPathName(long idPath)
            {
                return (from item in Manager.GetIQ<Path>() where item.Id == idPath select item.Name).FirstOrDefault<String>();
            }
            public EPType GetPathType(long idPath)
            {
                return (from item in Manager.GetIQ<Path>() where item.Id == idPath select item.EPType).FirstOrDefault<EPType>();
            }
            public Status GetPathStatus(long idPath, Boolean alsoDeleted = false )
            {
                return (from item in Manager.GetIQ<Path>() where item.Id == idPath && (alsoDeleted || ( item.Deleted== BaseStatusDeleted.None)) select item.Status).FirstOrDefault<Status>();
            }
            public Status GetUnitStatus(long idUnit, Boolean alsoDeleted = false)
            {
                return (from item in Manager.GetIQ<Unit>() where item.Id == idUnit && (alsoDeleted || (item.Deleted == BaseStatusDeleted.None)) select item.Status).FirstOrDefault<Status>();
            }
            public Status GetActivityStatus(long idActivity, Boolean alsoDeleted = false)
            {
                return (from item in Manager.GetIQ<Activity>() where item.Id == idActivity && (alsoDeleted || (item.Deleted == BaseStatusDeleted.None)) select item.Status).FirstOrDefault<Status>();
            }
            public Boolean isCreatorOf(Int32 idPerson, long idItem, ItemType type)
            {
                switch (type)
                {
                    case ItemType.Path:
                        return (from p in Manager.GetIQ<Path>() where p.Id == idItem && p.IdCreatedBy == idPerson select p.Id).Any();
                    case ItemType.Unit:
                        return (from p in Manager.GetIQ<Unit>() where p.Id == idItem && p.IdCreatedBy == idPerson select p.Id).Any();
                    case ItemType.Activity:
                        return (from p in Manager.GetIQ<Activity>() where p.Id == idItem && p.IdCreatedBy == idPerson select p.Id).Any();
                    case ItemType.SubActivity:
                        return (from p in Manager.GetIQ<SubActivity>() where p.Id == idItem && p.IdCreatedBy == idPerson select p.Id).Any();
                }
                return false;
            }
            public long GetIdFaher(long idItem, ItemType type)
            {
                switch (type)
                {
                    case ItemType.Path:
                        return 0;
                    case ItemType.Unit:
                        return (from p in Manager.GetIQ<Unit>() where p.Id == idItem && p.ParentPath != null select p.ParentPath.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                    case ItemType.Activity:
                        return (from p in Manager.GetIQ<Activity>() where p.Id == idItem && p.ParentUnit != null select p.ParentUnit.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                    case ItemType.SubActivity:
                        return (from p in Manager.GetIQ<SubActivity>() where p.Id == idItem && p.ParentActivity != null select p.ParentActivity.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                return 0;
            }
            public long GetIdActivityBySubActivity(long idSubActivity)
            {
                long idActivity = 0;
                try
                {
                    idActivity = (from s in Manager.GetIQ<liteSubActivity>() where s.Id == idSubActivity select s.IdActivity).Skip(0).Take(1).ToList().FirstOrDefault();
                }
                catch (Exception) { }
                return idActivity;
            }
            public liteCommunity GetPathCommunity(long idPath)
            {
                liteCommunity result = null;
                try {
                    Path p = GetPath(idPath);
                    if (p!=null)
                        result = p.Community;
                }
                catch (Exception ex) {
                    result = null;
                }
                return result;
            }
            public Int32 GetPathIdCommunity(long idPath)
            {
                Int32 result = (from p in Manager.GetIQ<Path>() where p.Id == idPath && p.Community != null select p.Community.Id).Skip(0).Take(1).ToList().FirstOrDefault();
                return result;
            }
        #endregion

        #region TextItem


        public Status GetStatus(long itemId, ItemType type)
        {
            switch (type)
            {
                case ItemType.Activity:
                    return GetActStatus(itemId);
                case ItemType.Unit:
                    return GetUnitStatus(itemId);
                default:
                    return Status.None;
            }
        }


        public Status GetUnitStatus(long unitId)
        {

            return Manager.Get<Unit>(unitId).Status;
        }

        public Status GetActStatus(long actId)
        {
            return Manager.Get<Activity>(actId).Status;
        }

        public bool isTextActivity(long actId)
        {
            return CheckStatus(Manager.Get<Activity>(actId).Status, Status.Text);
        }
        public bool isTextUnit(long unitId)
        {
            return CheckStatus(Manager.Get<Unit>(unitId).Status, Status.Text);
        }

        public bool IsTextItem(long itemId, ItemType type)
        {
            if (type == ItemType.Unit)
            {
                return isTextUnit(itemId);
            }
            else if (type == ItemType.Activity)
            {
                return isTextActivity(itemId);
            }
            return false;
        }

        public long SaveActOrUnitNoteDraft(ItemType type, long parentId, int CreatorID, string UserProxyIPaddress, string UserIPaddress, Int32 CurrentCommunityID)
        {
            if (type == ItemType.Activity)
            {
                Activity oAct = new Activity();
                oAct.Status |= Status.Text | Status.NotLocked | Status.Draft;
                return SaveActivity(oAct, parentId, CreatorID, UserProxyIPaddress, UserIPaddress, CurrentCommunityID).Id;
            }
            else if (type == ItemType.Unit)
            {
                Unit oUnit = new Unit();
                oUnit.Status |= Status.Text | Status.NotLocked | Status.Draft;
                return SaveUnit(oUnit, parentId, CreatorID, UserProxyIPaddress, UserIPaddress, CurrentCommunityID).Id;
            }
            return 0;
        }

        public bool SaveOrUpdateTextItem(long itemId, String text, Status status, ItemType type, long parentId, List<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, int CreatorID, string UserProxyIPaddress, string UserIPaddress, Int32 CurrentCommunityID, Int32 idLanguage)
        {
            if (type == ItemType.Activity)
            {
                Activity oAct = Manager.Get<Activity>(itemId);
                oAct.Description = text;
                oAct.Status = status;
                return SaveOrUpdateActivityandAssignment(oAct, parentId, dtoCRoleAssignments, dtoPersonAssignments, CreatorID, UserIPaddress, UserProxyIPaddress, CurrentCommunityID, idLanguage);
            }
            else if (type == ItemType.Unit)
            {
                Unit oUnit = Manager.Get<Unit>(itemId);
                oUnit.Description = text;
                oUnit.Status = Status.Text | Status.NotLocked;
                return SaveOrUpdateUnitandAssignment(oUnit, parentId, dtoCRoleAssignments, dtoPersonAssignments, CreatorID, UserIPaddress, UserProxyIPaddress, idLanguage);

            }
            else
            {
                return false;
            }
        }

        //public bool UpdateTextItem(long itemId, String text, ItemType type, int userID, string userProxyIPaddress, string userIPaddress)
        //{
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        if (type == ItemType.Activity)
        //        {
        //            Activity oAct = Manager.Get<Activity>(itemId);
        //            oAct.Description = text;
        //            SetActivityModifyMetaInfo(oAct, Manager.GetPerson(userID), userIPaddress, userProxyIPaddress, DateTime.Now);
        //        }
        //        else if (type == ItemType.Unit)
        //        {
        //            Unit oUnit = Manager.Get<Unit>(itemId);
        //            oUnit.Description = text;
        //            SetUnitModifyMetaInfo(oUnit, Manager.GetPerson(userID), userIPaddress, userProxyIPaddress, DateTime.Now);
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //        Manager.Commit();
        //    }
        //    catch (Exception ex)
        //    {

        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //    return true;
        //}

        public String GetTextItem(long itemId, ItemType type)
        {
            if (type == ItemType.Activity)
            {
                return Manager.Get<Activity>(itemId).Description;
            }
            else if (type == ItemType.Unit)
            {
                return Manager.Get<Unit>(itemId).Description;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region SaveUpdate Unit

        public Unit GetUnit(long UnitId)
        {
            return Manager.Get<Unit>(UnitId);
        }
        public Unit SaveUnit(Unit oUnit, long ParentPathId, Int32 idCreator, string proxyIPaddress, string IPaddress, Int32 idComunity)
        {

            try
            {
                Manager.BeginTransaction();
                oUnit.ParentPath = Manager.Get<Path>(ParentPathId);
                oUnit.CreateMetaInfo(idCreator, IPaddress, proxyIPaddress);
                oUnit.Community = Manager.GetLiteCommunity(idComunity);
                Manager.SaveOrUpdate<Unit>(oUnit);
                Manager.Commit();
                return oUnit;
            }
            catch (Exception ex)
            {

                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }

        private Int16 GetNewUnitDisplayOrder(long PathId)
        {
            try
            {
                Int16 displayOrder = (from unit in Manager.GetAll<Unit>(u => u.ParentPath.Id == PathId && u.Deleted ==  BaseStatusDeleted.None).ToList() where !((unit.Status & Status.Draft) == Status.Draft) select unit.DisplayOrder).Max();
                displayOrder++;
                return displayOrder;
            }
            catch (Exception)
            {
                return 1;
                throw;
            }
        }

        public void SetUnitPartialDetail(Unit outputUnit, Unit inputUnit)
        {
            outputUnit.Name = inputUnit.Name;
            outputUnit.Description = inputUnit.Description;
            outputUnit.Duration = inputUnit.Duration;
            outputUnit.MinCompletion = inputUnit.MinCompletion;
            outputUnit.Status = inputUnit.Status;
        }

        public bool SaveOrUpdateUnitandAssignment(Unit unit, long idPath, List<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, int PersonID, String PersonIpAddress, String PersonProxyIpAddress, Int32 idLanguage)
        {
            try
            {
                DateTime TimeNow = DateTime.Now;
                List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssToUpdate;
                IList<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssToUpdate;
                Unit oUnitToUpdate = Manager.Get<Unit>(unit.Id);
                Manager.BeginTransaction();
                if (unit.DisplayOrder == 0)
                {
                    oUnitToUpdate.DisplayOrder = GetNewUnitDisplayOrder(idPath);
                }
                if ((unit.Status & Status.Draft) == Status.Draft)//nuovo
                {

                    SetUnitPartialDetail(oUnitToUpdate, unit);

                    oUnitToUpdate.Community = unit.ParentPath.Community;


                    dtoCRoleAssToUpdate = ServiceAssignments.GetListCRoleUnitAssignment(unit.Id,(unit.Community != null ? unit.Community.Id :0), idLanguage);
                    dtoPersonAssToUpdate = ServiceAssignments.GetListPersonUnitAssignment(unit.Id);

                    foreach (dtoGenericAssignmentWithOldRoleEP dbItem in dtoCRoleAssToUpdate)
                    {
                        dtoGenericAssignmentWithOldRoleEP temp = (from item in dtoCRoleAssignments where dbItem.ItemID == item.ItemID select item).FirstOrDefault();
                        if (temp != null)
                        {
                            temp.DB_ID = dbItem.DB_ID;
                        }
                    }

                    foreach (dtoGenericAssWithOldRoleEpAndDelete dbItem in dtoPersonAssToUpdate)
                    {
                        dtoGenericAssWithOldRoleEpAndDelete temp = (from item in dtoPersonAssignments where dbItem.ItemID == item.ItemID select item).FirstOrDefault();
                        if (temp != null)
                        {
                            temp.DB_ID = dbItem.DB_ID;
                        }
                    }
                }
                else //update
                {

                    SetUnitPartialDetail(oUnitToUpdate, unit);
                    unit.UpdateMetaInfo(PersonID, PersonIpAddress, PersonProxyIpAddress, TimeNow);
                    //SetUnitModifyMetaInfo(oUnit, InterestedPerson, PersonIpAddress, PersonProxyIpAddress, TimeNow);

                }


                Manager.SaveOrUpdate<Unit>(oUnitToUpdate);

                //esistenti da eliminare
                List<long> IdCRoleAssToDelete = (from item in dtoCRoleAssignments where item.DB_ID > 0 && item.RoleEP == 0 select item.DB_ID).ToList();
                List<long> IdPersonAssToDelete = (from item in dtoPersonAssignments where item.DB_ID > 0 && item.RoleEP == 0 select item.DB_ID).ToList();
                ServiceAssignments.DeleteUnitAssignments(IdPersonAssToDelete, IdCRoleAssToDelete);

                //esistenti da aggiornare
                dtoPersonAssToUpdate = (from item in dtoPersonAssignments where item.DB_ID > 0 && item.RoleEP > 0 && item.isDeleted select item).ToList();
                dtoCRoleAssToUpdate = (from item in dtoCRoleAssignments where item.DB_ID > 0 && item.RoleEP > 0 select item).ToList();
                ServiceAssignments.UpdateRoleAssignmentsUnitNoTransaction(dtoPersonAssToUpdate, dtoCRoleAssToUpdate, PersonID, PersonIpAddress, PersonProxyIpAddress, TimeNow);

                //nuovi
                ServiceAssignments.SaveNewUnitPersonAssignmentsNoTransaction(dtoPersonAssignments, oUnitToUpdate, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                ServiceAssignments.SaveNewUnitCRoleAssignmentsNoTransaction(dtoCRoleAssignments, oUnitToUpdate, oUnitToUpdate.ParentPath.Community.Id, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);

                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }



        public bool DeleteOnlyUnit(long UnitId)
        {
            try
            {
                Manager.BeginTransaction();
                Manager.DeleteOnlyUnit(UnitId);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }

        #endregion

        #region SaveUpdate Activity

            public dtoSubActivityCertificate GetDtoSubActivityCertificate(long idSubactivity)
            {
                return (from s in Manager.GetIQ<SubActivity>()
                        where s.Id == idSubactivity && s.ParentActivity!=null && s.Path !=null 
                        select new
                        {
                            Id = s.Id,
                            Name = s.Name,
                            IdActivity = s.ParentActivity.Id,
                            IdPath = s.Path.Id,
                            IdCertificate = s.IdCertificate,
                            IdCertificateVersion = s.IdCertificateVersion,
                            AutoGenerated = s.AutoGenerated,
                            SaveCertificate = s.SaveCertificate,
                            ActiveOnMinCompletion = s.ActiveOnMinCompletion,
                            ActiveOnMinMark = s.ActiveOnMinMark,
                            UsePathEndDateStatistics = s.UsePathEndDateStatistics,
                            ActiveAfterPathEndDate = s.ActiveAfterPathEndDate,
                            AllowWithEmptyPlaceHolders = s.AllowWithEmptyPlaceHolders
                        }).Skip(0).
                           Take(1).ToList().Select(i => new dtoSubActivityCertificate() { 
                               Id = i.Id, Name=i.Name,
                                IdActivity = i.IdActivity,
                               IdPath = i.IdPath,
                               IdTemplate = i.IdCertificate,
                               IdTemplateVersion = i.IdCertificateVersion,
                               AutoGenerated = i.AutoGenerated,
                               SaveCertificate = i.SaveCertificate,
                               ActiveOnMinCompletion = i.ActiveOnMinCompletion,
                               ActiveOnMinMark = i.ActiveOnMinMark,
                               UsePathEndDateStatistics = i.UsePathEndDateStatistics,
                               ActiveAfterPathEndDate = i.ActiveAfterPathEndDate,
                               AllowWithEmptyPlaceHolders = i.AllowWithEmptyPlaceHolders,
                           }).FirstOrDefault();
            }

            public dtoSubActivity GetDtoSubActivity(long idSubactivity)
            {
                return new dtoSubActivity(GetSubActivity(idSubactivity));
            }
            public SubActivity GetSubActivity(long SubActivityId)
            {
                return Manager.Get<SubActivity>(SubActivityId);
            }
            public Activity GetActivity(long ActivityId)
            {
                return Manager.Get<Activity>(ActivityId);
            }
        public Activity SaveActivity(Activity oActivity, long ParentUnitId, int CreatorID, string UserProxyIPaddress, string UserIPaddress, Int32 CurrentCommunityID)
        {

            try
            {
                Manager.BeginTransaction();
                oActivity.ParentUnit = Manager.Get<Unit>(ParentUnitId);
                oActivity.Path = oActivity.ParentUnit.ParentPath;
                oActivity.CreateMetaInfo(CreatorID, UserIPaddress, UserProxyIPaddress);
                oActivity.Community = Manager.GetLiteCommunity(CurrentCommunityID);
                if (oActivity.Community != null)
                    Manager.SaveOrUpdate<Activity>(oActivity);
                else
                    oActivity = null;
                Manager.Commit();
                return oActivity;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }
        private Int16 GetNewActivityDisplayOrder(long UnitId)
        {
            try
            {
                Int16 displayOrder = (from activity in Manager.GetAll<Activity>(act => act.ParentUnit.Id == UnitId && act.Deleted == BaseStatusDeleted.None).ToList() where !((activity.Status & Status.Draft) == Status.Draft) select activity.DisplayOrder).Max();
                displayOrder++;
                return displayOrder;
            }
            catch (Exception)
            {
                return 1;
                throw;
            }
        }

        private void SetActivityPartialDetail(Activity outputActivity, Activity oInputActivity)
        {
            outputActivity.Name = oInputActivity.Name;
            outputActivity.Description = oInputActivity.Description;
            outputActivity.Duration = oInputActivity.Duration;
            outputActivity.MinCompletion = oInputActivity.MinCompletion;
            outputActivity.Status = oInputActivity.Status;
            outputActivity.StartDate = oInputActivity.StartDate;
            outputActivity.EndDate = oInputActivity.EndDate;
            outputActivity.Duration = oInputActivity.Duration;
            outputActivity.isQuiz = oInputActivity.isQuiz;
        }

        public bool SaveOrUpdateActivityandAssignment(Activity oActivity, long UnitId, List<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, int PersonID, String PersonIpAddress, String PersonProxyIpAddress, int idCommunity, Int32 idLanguage)
        {
            try
            {
                liteCommunity community = Manager.GetLiteCommunity(idCommunity);
                List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssToUpdate;
                IList<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssToUpdate;
                DateTime TimeNow = DateTime.Now;

                Manager.BeginTransaction();
                Activity oActivityToUpdate = Manager.Get<Activity>(oActivity.Id);
                if (oActivityToUpdate != null)
                {
                    if (oActivity.DisplayOrder == 0)
                    {
                        oActivityToUpdate.DisplayOrder = GetNewActivityDisplayOrder(UnitId);
                    }
                    if ((oActivity.Status & Status.Draft) == Status.Draft)//nuovo
                    {
                        SetActivityPartialDetail(oActivityToUpdate, oActivity);
                        oActivityToUpdate.Community = oActivity.ParentUnit.Community;
                        dtoCRoleAssToUpdate = ServiceAssignments.GetListCRoleActivityAssignment(oActivity.Id, (oActivityToUpdate.Community != null ? oActivityToUpdate.Community.Id : 0), idLanguage);
                        dtoPersonAssToUpdate = ServiceAssignments.GetListPersonActivityAssignment(oActivity.Id);

                        foreach (dtoGenericAssignmentWithOldRoleEP dbItem in dtoCRoleAssToUpdate)
                        {
                            dtoGenericAssignmentWithOldRoleEP temp = (from item in dtoCRoleAssignments where dbItem.ItemID == item.ItemID select item).FirstOrDefault();
                            if (temp != null)
                            {
                                temp.DB_ID = dbItem.DB_ID;
                            }
                        }

                        foreach (dtoGenericAssWithOldRoleEpAndDelete dbItem in dtoPersonAssToUpdate)
                        {
                            dtoGenericAssWithOldRoleEpAndDelete temp = (from item in dtoPersonAssignments where dbItem.ItemID == item.ItemID select item).FirstOrDefault();
                            if (temp != null)
                            {
                                temp.DB_ID = dbItem.DB_ID;
                            }
                        }
                    }
                    else //update
                    {

                        SetActivityPartialDetail(oActivityToUpdate, oActivity);
                        oActivityToUpdate.UpdateMetaInfo(PersonID, PersonIpAddress, PersonProxyIpAddress, TimeNow);
                    }
                    Manager.SaveOrUpdate<Activity>(oActivityToUpdate);
                    //esistenti da eliminare
                    List<long> IdCRoleAssToDelete = (from item in dtoCRoleAssignments where item.DB_ID > 0 && item.RoleEP == 0 select item.DB_ID).ToList();
                    List<long> IdPersonAssToDelete = (from item in dtoPersonAssignments where item.DB_ID > 0 && item.RoleEP == 0 select item.DB_ID).ToList();
                    ServiceAssignments.DeleteActivityAssignments(IdPersonAssToDelete, IdCRoleAssToDelete);

                    //esistenti da aggiornare
                    dtoPersonAssToUpdate = (from item in dtoPersonAssignments where item.DB_ID > 0 && item.RoleEP > 0 && !item.isDeleted select item).ToList();
                    dtoCRoleAssToUpdate = (from item in dtoCRoleAssignments where item.DB_ID > 0 && item.RoleEP > 0 select item).ToList();
                    ServiceAssignments.UpdateRoleAssignmentsActivityNoTransaction(dtoPersonAssToUpdate, dtoCRoleAssToUpdate, PersonID, PersonIpAddress, PersonProxyIpAddress, TimeNow);

                    //nuovi
                    ServiceAssignments.SaveNewActivityPersonAssignmentsNoTransction(dtoPersonAssignments, oActivityToUpdate, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                    ServiceAssignments.SaveNewActivityCRoleAssignmentsNoTransction(dtoCRoleAssignments, oActivityToUpdate, community.Id, TimeNow, PersonID, PersonIpAddress, PersonProxyIpAddress);
                    if (oActivityToUpdate.Path != null)
                    {
                        long pathNewWeight = GetActiveUnitsWeightSum(oActivityToUpdate.Path.Id);
                        if (oActivityToUpdate.Path.Weight < pathNewWeight)
                        {
                            oActivityToUpdate.Path.WeightAuto = pathNewWeight;
                            oActivityToUpdate.Path.Weight = pathNewWeight;
                        }
                        //else if (oActivityToUpdate.Path.Weight > pathNewWeight && (oActivityToUpdate.Path.Weight == (pathNewWeight - oActivityToUpdate.Weight + previousActivityWeight)))
                        //{
                        //    oActivityToUpdate.Path.WeightAuto = pathNewWeight;
                        //    oActivityToUpdate.Path.Weight = pathNewWeight;
                        //}
                    }
                }
                
              
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }

        public bool DeleteOnlyActivity(long ActivityId)
        {
            try
            {
                Manager.BeginTransaction();
                Manager.DeleteOnlyActivity(ActivityId);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }

        #endregion

        #region EduPathList

        public int GetEvaluateManagerLockedPathCount(List<dtoEPitemList> EpList)
        {
            return (from item in EpList
                    where CheckStatus(item.Status, Status.Locked) && (item.UnitToManage > 0 || item.UnitToEvaluate > 0 || item.ActivityToManage > 0 || item.ActivityToEvaluate > 0)
                    select item).Count<dtoEPitemList>();
        }


        //public List<dtoEPitemList> GetVisibleEp(List<dtoEPitemList> EpList, int userId, int croleId)
        //{
        //    return (from item in EpList where CheckStatus(item.Status, Status.NotLocked) select item).ToList<dtoEPitemList>();
        //}

        public String GetCommunityName(Int32 idCommunity)
        {
            return Manager.GetCommunityName(idCommunity);
        }

        private IList<long> GetActivitiesId_ByCreator(long PathId, int UserId)
        {
            IList<Activity> Activities;
            Activities = Manager.GetAll<Activity>(act => act.IdCreatedBy == UserId && act.Deleted == BaseStatusDeleted.None
                    && act.Path.Id == PathId).ToList();
            return (from act in Activities
                    where !act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
                    select act.Id).ToList();
        }
        private IList<long> GetActivitiesId_ByCreator(List<long> idActivities, int UserId)
        {
            IList<Activity> Activities;
            Activities = Manager.GetAll<Activity>(act => act.IdCreatedBy == UserId && act.Deleted == BaseStatusDeleted.None
                    && idActivities.Contains(act.Id)).ToList();
            return (from act in Activities
                    where !act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
                    select act.Id).ToList();
        }
        private IList<long> GetUnitsId_ByCreator(long PathId, int UserId)
        {
            IList<Unit> Units;
            Units = Manager.GetAll<Unit>(unit => unit.IdCreatedBy == UserId && unit.Deleted == BaseStatusDeleted.None
                    && unit.ParentPath.Id == PathId).ToList();
            return (from unit in Units
                    where !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                    select unit.Id).ToList();
        }
        private IList<long> GetUnitsId_ByCreator(List<long> idUnits, int idPerson)
        {
            IList<Unit> Units;
            Units = Manager.GetAll<Unit>(unit => unit.IdCreatedBy == idPerson && unit.Deleted == BaseStatusDeleted.None
                    && idUnits.Contains(unit.Id)).ToList();
            return (from unit in Units
                    where !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                    select unit.Id).ToList();
        }
        private long GetUnitToManageCount(long PathId, int UserId, int CommId, int CRoleId)
        {
            return ((ServiceAssignments.GetUnitsId_ByPersonAssignment(PathId, UserId, RoleEP.Manager).Union(ServiceAssignments.GetUnitsId_ByCRoleAssignment(PathId, CRoleId, RoleEP.Manager)).Union(GetUnitsId_ByCreator(PathId, UserId))).Distinct()).Count();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUnits"></param>
        /// <param name="idPerson"></param>
        /// <param name="idCommunity"></param>
        /// <param name="idRole"></param>
        /// <returns></returns>
        private long GetUnitToManageCount(List<long> idUnits, int idPerson, int idCommunity, int idRole)
        {

            List<long> results = ServiceAssignments.GetUnitsId_ByCRoleAssignment(idUnits, idRole, RoleEP.Manager);
            if (idUnits.Except(results).Any()){
                results.Union(ServiceAssignments.GetUnitsId_ByPersonAssignment(idUnits.Except(results).ToList(), idPerson, RoleEP.Manager));
                if (idUnits.Except(results).Any())
                    results.Union(GetUnitsId_ByCreator(idUnits.Except(results).ToList(), idPerson));
            }
            return results.Distinct().Count();
        }
        private long GetUnitToEvaluateCount(long PathId, int UserId, int CommId, int CRoleId)
        {
            return ((ServiceAssignments.GetUnitsId_ByPersonAssignment(PathId, UserId, RoleEP.Evaluator).Union(ServiceAssignments.GetUnitsId_ByCRoleAssignment(PathId, CRoleId, RoleEP.Evaluator))).Distinct()).Count();
        }
        private long GetUnitToEvaluateCount(List<long> idUnits, int idPerson, int idCommunity, int idRole)
        {

            List<long> results = ServiceAssignments.GetUnitsId_ByCRoleAssignment(idUnits, idRole, RoleEP.Evaluator);
            if (idUnits.Except(results).Any())
            {
                results.Union(ServiceAssignments.GetUnitsId_ByPersonAssignment(idUnits.Except(results).ToList(), idPerson, RoleEP.Evaluator));
            }
            return results.Distinct().Count();
        }
        private long GetActivityToManageCount(long PathId, int UserId, int CommId, int CRoleId)
        {
            return ((ServiceAssignments.GetActivitiesId_ByPersonAssignment(PathId, UserId, RoleEP.Manager).Union(ServiceAssignments.GetActivitiesId_ByCRoleAssignment(PathId, CRoleId, RoleEP.Manager)).Union(GetActivitiesId_ByCreator(PathId, UserId))).Distinct()).Count();
        }
        private long GetActivityToManageCount(List<long> idActivities, int idPerson, int idCommunity, int idRole)
        {
            List<long> results = ServiceAssignments.GetActivitiesId_ByCRoleAssignment(idActivities, idRole, RoleEP.Manager);
            if (idActivities.Except(results).Any())
            {
                results.Union(ServiceAssignments.GetActivitiesId_ByPersonAssignment(idActivities.Except(results).ToList(), idPerson, RoleEP.Manager));
                if (idActivities.Except(results).Any())
                    results.Union(GetActivitiesId_ByCreator(idActivities.Except(results).ToList(), idPerson));
            }
            return results.Distinct().Count();
        }

        private long GetActivityToEvaluateCount(long PathId, int UserId, int CommId, int CRoleId)
        {
            return ((ServiceAssignments.GetActivitiesId_ByPersonAssignment(PathId, UserId, RoleEP.Evaluator).Union(ServiceAssignments.GetActivitiesId_ByCRoleAssignment(PathId, CRoleId, RoleEP.Evaluator))).Distinct()).Count();
        }

        private long GetActivityToEvaluateCount(List<long> idActivities, int idPerson, int idCommunity, int idRole)
        {
            List<long> results = ServiceAssignments.GetActivitiesId_ByCRoleAssignment(idActivities, idRole, RoleEP.Evaluator);
            if (idActivities.Except(results).Any())
            {
                results.Union(ServiceAssignments.GetActivitiesId_ByPersonAssignment(idActivities.Except(results).ToList(), idPerson, RoleEP.Evaluator));
            }
            return results.Distinct().Count();
        }
        private List<dtoPathManageStatistics> GetPathManageStatistics(List<long> idPaths, Int32 idPerson, Int32 idCommunity, Int32 idRole)
        {
            List<dtoPathItems> items = GetPagedUnits(idPaths);
            items.AddRange(GetPagedActivities(idPaths));
            List<dtoPathManageStatistics> statistics = idPaths.Select(i=> new dtoPathManageStatistics(i, items)).ToList();

            foreach (dtoPathManageStatistics item in statistics.Where(s=> s.Units>0 || s.Activities>0))
            {
                if (item.Units > 0)
                {
                    item.UnitsToManage = GetUnitToManageCount(item.IdAvailableUnits, idPerson, idCommunity, idRole);
                    item.UnitsToEvaluate= GetUnitToEvaluateCount(item.IdAvailableUnits, idPerson, idCommunity, idRole);
                }
                if (item.Activities > 0)
                {
                    item.ActivitiesToManage = GetActivityToManageCount(item.IdAvailableActivities, idPerson, idCommunity, idRole);
                    item.ActivitiesToEvaluate = GetActivityToEvaluateCount(item.IdAvailableActivities, idPerson, idCommunity, idRole);
                }
            }
            return statistics;
        }
        private dtoPathManageStatistics GetPathManageStatistics(long idPath, Int32 idPerson, Int32 idCommunity, Int32 idRole)
        {
            List<dtoPathItems> items = GetPagedUnits(new List<long>() { idPath });
            items.AddRange(GetPagedActivities(new List<long>() { idPath }));
            dtoPathManageStatistics statistics = new dtoPathManageStatistics(idPath, items);

            if (statistics.Units > 0)
            {
                statistics.UnitsToManage = GetUnitToManageCount(statistics.IdAvailableUnits, idPerson, idCommunity, idRole);
                statistics.UnitsToEvaluate = GetUnitToEvaluateCount(statistics.IdAvailableUnits, idPerson, idCommunity, idRole);
            }
            if (statistics.Activities > 0)
            {
                statistics.ActivitiesToManage = GetActivityToManageCount(statistics.IdAvailableActivities, idPerson, idCommunity, idRole);
                statistics.ActivitiesToEvaluate = GetActivityToEvaluateCount(statistics.IdAvailableActivities, idPerson, idCommunity, idRole);
            }
            return statistics;
        }
        List<dtoPathItems> GetPagedUnits(List<long> idPaths, Boolean allUnits = false)
        {
            var query = (from u in Manager.Linq<Unit>() where u.Deleted == BaseStatusDeleted.None && u.ParentPath != null select u);
            if (idPaths.Count <= maxItemsForSingleQuery)
                return query.Where(u => idPaths.Contains(u.ParentPath.Id)).Select(u=> new { IdUnit= u.Id,IdPath= u.ParentPath.Id, Status = u.Status}).ToList().Where(i=> allUnits ||( !((i.Status & Status.Draft) == Status.Draft) && !((i.Status & Status.Text) == Status.Text))).GroupBy(i => i.IdPath).Select(i => new dtoPathItems { IdPath = i.Key, IdItems = i.Select(j=> j.IdUnit).ToList(), Type = ItemType.Unit }).ToList();
            else
            {
                List<dtoPathItems> results = new List<dtoPathItems>();
                Int32 pageIndex = 0;
                List<long> idPagedItems = idPaths.Skip(pageIndex * maxItemsForSingleQuery).Take(maxItemsForSingleQuery).ToList();
                while (idPagedItems.Any())
                {
                    results.AddRange(query.Where(u => idPagedItems.Contains(u.ParentPath.Id)).ToList().Where(i => allUnits || (!((i.Status & Status.Draft) == Status.Draft) && !((i.Status & Status.Text) == Status.Text))).Select(u => new { idPath = u.ParentPath.Id, idUnit = u.Id }).ToList().GroupBy(i => i.idPath).Select(i => new dtoPathItems { IdPath = i.Key, IdItems = i.Select(u => u.idUnit).ToList(), Type = ItemType.Unit }).ToList());
                    pageIndex++;
                    idPagedItems = idPaths.Skip(pageIndex * maxItemsForSingleQuery).Take(maxItemsForSingleQuery).ToList();
                }
                return results;
            }
        }
        List<dtoPathItems> GetPagedActivities(List<long> idPaths, Boolean allActivities = false )
        {
            var query = (from u in Manager.Linq<Activity>()
                         where u.Deleted == BaseStatusDeleted.None && u.Path != null 
                         select u);
            if (idPaths.Count <= maxItemsForSingleQuery)
                return query.Where(u => idPaths.Contains(u.Path.Id)).Select(u => new { idPath = u.Path.Id, IdActivity = u.Id, Status = u.Status }).ToList().Where(a=>
                        allActivities || (!CheckStatus(a.Status, Status.Draft) && !CheckStatus(a.Status, Status.Text))).GroupBy(i => i.idPath).Select(i => new dtoPathItems { IdPath = i.Key, IdItems = i.Select(u => u.IdActivity).ToList(), Type = ItemType.Activity }).ToList();
            else
            {
                List<dtoPathItems> results = new List<dtoPathItems>();
                Int32 pageIndex = 0;
                List<long> idPagedItems = idPaths.Skip(pageIndex * maxItemsForSingleQuery).Take(maxItemsForSingleQuery).ToList();
                while (idPagedItems.Any())
                {
                    results.AddRange(query.Where(u => idPagedItems.Contains(u.Path.Id)).Select(u => new { idPath = u.Path.Id, IdActivity = u.Id, Status = u.Status }).ToList().Where(a =>
                        allActivities || (!CheckStatus(a.Status, Status.Draft) && !CheckStatus(a.Status, Status.Text))).GroupBy(i => i.idPath).Select(i => new dtoPathItems { IdPath = i.Key, IdItems = i.Select(u => u.IdActivity).ToList(), Type = ItemType.Activity }).ToList());
                    pageIndex++;
                    idPagedItems = idPaths.Skip(pageIndex * maxItemsForSingleQuery).Take(maxItemsForSingleQuery).ToList();
                }
                return results;
            }
        }

        public IList<dtoEPitemList> GetMyEduPaths(int idPerson, int idCommunity, int idRole, EpViewModeType ViewModeType, Boolean areMoocs)
        {
            List<Path> items = Manager.GetAll<Path>(ep => ep.Community.Id == idCommunity && ep.Deleted == BaseStatusDeleted.None && areMoocs == ep.IsMooc).ToList<Path>();

            switch (ViewModeType)
            {
                case EpViewModeType.View:
                    return (from ep in items
                            where !((ep.Status & Status.Draft) == Status.Draft) && 
                            (
                            CheckRoleEp(GetUserRole_ByPath(ep, idPerson, idRole), RoleEP.Participant)
                            ||
                            CheckRoleEp(GetUserRole_ByPath(ep, idPerson, idRole), RoleEP.StatViewer)
                            )
                            
                            orderby ep.isDefault descending, ep.DisplayOrder
                            select new dtoEPitemList(ep, PathStatus(ep.Id, idPerson, idRole, false), RoleEP.Participant)
                                {
                                    type = ep.EPType,
                                    moocType = ep.MoocType
                                }
                            ).ToList();

                case EpViewModeType.Manage:
                    List<long> idPaths = items.Where(p => !((p.Status & Status.Draft) == Status.Draft) && (CheckRoleEp(GetUserRole_ByPath(p, idPerson, idRole,true), RoleEP.Manager))).Select(p => p.Id).ToList();
                    List<dtoPathManageStatistics> statistics = GetPathManageStatistics(idPaths, idPerson, idCommunity,idRole);
                    Dictionary<long, Boolean> startedPaths = ServiceStat.UsersStartedPath(idPaths);
                    return (from ep in items
                            where !((ep.Status & Status.Draft) == Status.Draft) &&
                            (CheckRoleEp(GetUserRole_ByPath(ep, idPerson, idRole), RoleEP.Manager) || //sono ep Manager
                            statistics.Any(s=> s.HasItemsToDo && s.IdPath== ep.Id)) //sono Activity Eval  
                            orderby ep.isDefault descending, ep.DisplayOrder
                            select new dtoEPitemList(ep, PathStatus(ep.Id, idPerson, idRole, true), GetUserRole_ByPath(ep, idPerson, idRole),
                                statistics.Where(s=> s.IdPath==ep.Id).FirstOrDefault())
                                {
                                    type = ep.EPType,
                                    isModificable = !startedPaths.ContainsKey(ep.Id) || !startedPaths[ep.Id]
                                }

                                ).ToList();

                default:
                    return null;
            }
        }


        public bool SetDefaultPath(long idPath, int idCommunity, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Path oNewDefaultEp = Manager.Get<Path>(idPath);
            List<Path> oOldDefaultEp = Manager.GetAll<Path>(ep => ep.Community.Id == idCommunity && ep.isDefault).ToList();
            DateTime CurrentTime = DateTime.Now;
            try
            {
                Manager.BeginTransaction();
                if (oOldDefaultEp.Count == 1)
                {
                    oOldDefaultEp[0].isDefault = false;

                    //DisplayOrder < 0 -> IsDefault
                    //if (oOldDefaultEp[0].DisplayOrder < 0)
                    //{
                    //    oOldDefaultEp[0].DisplayOrder =(short) (-1 * oOldDefaultEp[0].DisplayOrder);
                    //}
                    //DisplayOrder < 0 -> IsDefault
                    oOldDefaultEp[0].UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                    //SetPathModifyMetaInfo(oOldDefaultEp[0], oAutorOfUpdate, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                    Manager.SaveOrUpdate<Path>(oOldDefaultEp[0]);
                }
                oNewDefaultEp.isDefault = true;

                //DisplayOrder < 0 -> IsDefault
                //oNewDefaultEp.DisplayOrder = (short)(-1 * oNewDefaultEp.DisplayOrder);
                //DisplayOrder < 0 -> IsDefault
                oNewDefaultEp.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetPathModifyMetaInfo(oNewDefaultEp, oAutorOfUpdate, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                Manager.SaveOrUpdate<Path>(oNewDefaultEp);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }
        public bool VirtualDeleteAllEp(long EpId, int idCommunity, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                

                Path path = Manager.Get<Path>(EpId);
                if (path != null)
                {
                    DateTime CurrentTime = DateTime.Now;

                    Boolean isInAutoEp = CheckEpType(path.EPType, lm.Comol.Modules.EduPath.Domain.EPType.Auto);

                    Manager.BeginTransaction();
                    path.SetDeleteMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);

                    foreach (Unit oUnit in path.UnitList)
                    {
                        VirtualDeleteUnitWithoutTransaction(oUnit, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Cascade, isInAutoEp);
                    }
                    Manager.SaveOrUpdate<Path>(path);
                    ServiceAssignments.VirtualDeletePathAssignmentsNoTransaction(path.Id, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Automatic);

                    Manager.Commit();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        public bool VirtualDeleteAllUnit(long UnitId, int idCommunity, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Unit unit = Manager.Get<Unit>(UnitId);
                if (unit != null) {
                    DateTime CurrentTime = DateTime.Now;
                    Manager.BeginTransaction();

                    Boolean isInAutoEp = CheckEpType(unit.ParentPath.EPType, lm.Comol.Modules.EduPath.Domain.EPType.Auto);

                    VirtualDeleteUnitWithoutTransaction(unit, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Manual, isInAutoEp);
                    Manager.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        public bool CheckWeightAutoCorrect(long PathId)
        {
            Path path = Manager.Get<Path>(PathId);

            return CheckWeightAutoCorrect(path);

        }

        public bool CheckWeightAutoCorrect(Path path)
        {

            long weightAuto = path.WeightAuto;

            long units = GetActiveUnitsWeightSum(path.Id);

            return weightAuto == units;

        }

        public dtoPathWeightFix PathWeightAutoCorrect(Path path)
        {
            dtoPathWeightFix pwf = new dtoPathWeightFix();
            //pwf.Path = path;
            pwf.Id = path.Id;
            pwf.Name = path.Name;
            pwf.SavedWeightAuto = path.WeightAuto;
            pwf.CalculatedWeightAuto = GetActiveUnitsWeightSum(path.Id);
            pwf.SavedWeight = path.Weight;

            if (path.Community != null)
            {
                //pwf.Community = path.Community;
                pwf.CommunityId = path.Community.Id;
                pwf.CommunityName = path.Community.Name;
            }

            return pwf;
        }

        public Boolean FixPathDescriptionUrls(Int64 pathId, String replace, String replaceWith)
        {
            try
            {
                Manager.BeginTransaction();
                Path path = Manager.Get<Path>(pathId);
                path.Description = path.Description.Replace(replace, replaceWith);

                foreach (var unit in path.UnitList)
                {
                    unit.Description = unit.Description.Replace(replace, replaceWith);
                    foreach (var activity in unit.ActivityList)
                    {
                        activity.Description = activity.Description.Replace(replace, replaceWith);
                        foreach (var subactivity in activity.SubActivityList)
                        {
                            subactivity.Description = subactivity.Description.Replace(replace, replaceWith);
                            Manager.SaveOrUpdate(subactivity);
                        }
                        Manager.SaveOrUpdate(activity);
                    }
                    Manager.SaveOrUpdate(unit);
                }

                Manager.SaveOrUpdate(path);

                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }
        }

        public Boolean FixPathsWeightAutoCorrect(IEnumerable<dtoPathWeightFix> items)
        {
            try
            {
                Manager.BeginTransaction();

                foreach (var item in items)
                {
                    Path path = Manager.Get<Path>(item.Id);

                    path.WeightAuto = item.CalculatedWeightAuto;
                    if (path.Weight < path.WeightAuto)
                    {
                        path.Weight = path.WeightAuto;
                    }

                    SaveEPNoTransaction(path, Context.UserContext.CurrentUserID, Context.UserContext.ProxyIpAddress, Context.UserContext.IpAddress);
                }

                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }
        }

        public IList<dtoPathWeightFix> PathsWeightAutoCorrect()
        {
            //IList<dtoPathWeightFix> list = new List<dtoPathWeightFix>();

            IList<Path> list =(from item in Manager.GetIQ<Path>() select item).ToList();

            if (list == null || list.Count == 0)
            {
                return new List<dtoPathWeightFix>();
            }

            return (from item in list select PathWeightAutoCorrect(item)).Where(x => x.NeedFix == true).ToList();
        }

        public bool VirtualDeleteAllActivity(long ActivityId, bool isInAutoEp, int CommId, Int32 idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Activity activity = Manager.Get<Activity>(ActivityId);
                if (activity != null)
                {
                    DateTime CurrentTime = DateTime.Now;
                    Manager.BeginTransaction();
                    if (isInAutoEp)
                    {
                        if (activity.ParentUnit != null)
                            activity.ParentUnit.Weight -= activity.Weight;

                        if (activity.Path != null)
                        {
                            activity.Path.WeightAuto -= activity.Weight;
                            activity.Path.Weight -= activity.Weight;
                        }
                        Manager.SaveOrUpdate(activity);
                    }
                    VirtualDeleteActivityWithoutTransaction(activity, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Manual);
                    Manager.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
            }
            return false;
        }
        public bool VirtualDeleteSubActivity(long SubActivityId, int CommId, Int32 idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                SubActivity oSubActivity = Manager.Get<SubActivity>(SubActivityId);
                if (oSubActivity != null)
                {
                    DateTime CurrentTime = DateTime.Now;
                    Manager.BeginTransaction();
                    VirtualDeleteSubActivityWithoutTransaction(oSubActivity, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Manual);
                    Manager.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
            }
            return false;
        }
        private void VirtualDeleteUnitWithoutTransaction(Unit oUnit, Int32 idPerson, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType, Boolean isInAutoEp)
        {
            foreach (Activity activity in oUnit.ActivityList)
            {
                if (isInAutoEp)
                {
                    if (activity.ParentUnit != null)
                        activity.ParentUnit.Weight -= activity.Weight;
                    if (activity.Path != null)
                    {
                        activity.Path.WeightAuto -= activity.Weight;
                        activity.Path.Weight -= activity.Weight;
                    }
                    Manager.SaveOrUpdate(activity);
                }
                VirtualDeleteActivityWithoutTransaction(activity, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Cascade);
            }
            oUnit.Deleted = DeleteType;
            oUnit.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            //SetUnitModifyMetaInfo(oUnit, oAutorOfDelete
            Manager.SaveOrUpdate<Unit>(oUnit);
            ServiceAssignments.VirtualDeleteUnitAssignmentsNoTransaction(oUnit.Id, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Automatic);

        }

        private void VirtualDeleteActivityWithoutTransaction(Activity oActivity, Int32 idPerson, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            foreach (SubActivity oSubAct in oActivity.SubActivityList)
            {
                VirtualDeleteSubActivityWithoutTransaction(oSubAct, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Cascade);
            }
            oActivity.Deleted = DeleteType;
            oActivity.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            //SetActivityModifyMetaInfo(oActivity, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            Manager.SaveOrUpdate<Activity>(oActivity);
            ServiceAssignments.VirtualDeleteActivityAssignmentsNoTransaction(oActivity.Id, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Automatic);

            DeleteActivityRolesNoTransaction(oActivity.Id, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Automatic);
        }

        private void DeleteActivityRolesNoTransaction(long activityId, Int32 idAutorOfDelete, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            IList<RuleActivityCompletion> rules = Manager.GetAll<RuleActivityCompletion>(rl => rl.DestinationId == activityId || rl.SourceId == activityId);

            Manager.DeletePhysicalList<RuleActivityCompletion>(rules);

            //foreach (RuleActivityCompletion rl in rules)
            //{
            //    rl.Deleted |= DeleteType;
            //    rl.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            //    //SetActivityPersonAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            //}

            //Manager.SaveOrUpdateList<RuleActivityCompletion>(rules);
        }

        private void VirtualDeleteSubActivityWithoutTransaction(SubActivity oSubActivity, Int32 idPerson, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            oSubActivity.Deleted = DeleteType;
            oSubActivity.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            //SetSubActivityModifyMetaInfo(oSubActivity, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            Manager.SaveOrUpdate<SubActivity>(oSubActivity);


            if (oSubActivity.ContentType == SubActivityType.Quiz)
            {
                IList<SubActivityLink> linked = (from item in Manager.GetIQ<SubActivityLink>() where item.IdObject == oSubActivity.IdObjectLong && item.IdModule == oSubActivity.IdModule select item).ToList();
                foreach (var item in linked)
                {
                    item.Deleted = BaseStatusDeleted.Cascade;
                    item.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                  
                }
                Manager.SaveOrUpdateList<SubActivityLink>(linked);
            } else if
                (oSubActivity.ContentType == SubActivityType.Certificate)
            {
                IList<SubActivityLink> linked = (from item in Manager.GetIQ<SubActivityLink>() where item.SubActivity.Id == oSubActivity.Id select item).ToList();
                foreach (var item in linked)
                {
                    item.Deleted = BaseStatusDeleted.Cascade;
                    item.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);

                }
                Manager.SaveOrUpdateList<SubActivityLink>(linked);
            }

            ServiceAssignments.VirtualDeleteSubActivityAssignmentsNoTransaction(oSubActivity.Id, idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime, BaseStatusDeleted.Automatic);
        }

        #endregion

        #region update min completion da attivare e rivedere se si personalizzano le statistiche

        //public bool UpdateActivityMinCompletion(long ActivityId, Int64 minCompletion, UpdateAssignemtOrStatistic updateAssOrStat, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {
        //        Activity oActivity = Manager.Get<Activity>(ActivityId);
        //        Int64 minCompletionForAssignments = oActivity.MinCompletion;

        //        SetActivityModifyMetaInfo(oActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<Activity>(oActivity);
        //        switch (updateAssOrStat)
        //        {
        //            case UpdateAssignemtOrStatistic.Assignment:
        //                ServiceAss.SetActivityAssignment_PersonalCompletion(ActivityId, PersonId, PersonProxyIpAddress, PersonIpAddress, minCompletionForAssignments, true);
        //                break;

        //            case UpdateAssignemtOrStatistic.Statistic:
        //                ServiceStat.UpdateActivityStatisticNoTransaction(ActivityId, oActivity.Community.Id, PersonId, PersonIpAddress, PersonProxyIpAddress);

        //                break;
        //        }

        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}

        //public bool UpdateUnitMinCompletion(long UnitId, Int64 minCompletion, UpdateAssignemtOrStatistic updateAssOrStat, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {
        //        Unit oUnit = Manager.Get<Unit>(UnitId);
        //        Int64 minCompletionForAssignments = oUnit.MinCompletion;

        //        SetUnitModifyMetaInfo(oUnit, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<Unit>(oUnit);
        //        switch (updateAssOrStat)
        //        {
        //            case UpdateAssignemtOrStatistic.Assignment:
        //                ServiceAss.SetUnitAssignment_PersonalCompletion(UnitId, PersonId, PersonProxyIpAddress, PersonIpAddress, minCompletionForAssignments, true);
        //                break;

        //            case UpdateAssignemtOrStatistic.Statistic:
        //                ServiceStat.UpdateUnitStatisticNoTransaction(UnitId, oUnit.Community.Id, PersonId, PersonIpAddress, PersonProxyIpAddress);

        //                break;
        //        }

        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}

        //public bool UpdatePathMinCompletion(long PathId, Int64 minCompletion, UpdateAssignemtOrStatistic updateAssOrStat, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {
        //        Path oPath = Manager.Get<Path>(PathId);
        //        Int64 minCompletionForAssignments = oPath.MinCompletion;

        //        SetPathModifyMetaInfo(oPath, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<Path>(oPath);
        //        switch (updateAssOrStat)
        //        {
        //            case UpdateAssignemtOrStatistic.Assignment:
        //                ServiceAss.SetPathAssignment_PersonalCompletion(PathId, PersonId, PersonProxyIpAddress, PersonIpAddress, minCompletionForAssignments, true);
        //                break;

        //            case UpdateAssignemtOrStatistic.Statistic:
        //                ServiceStat.UpdatePathStatisticNoTransaction(PathId, oPath.Community.Id, PersonId, PersonIpAddress, PersonProxyIpAddress);

        //                break;
        //        }

        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}
        #endregion

        #region View Edu Path
        public List<dtoEPitemList> GetVisibleEp(List<dtoEPitemList> EpList, int userId, int croleId)
        {
            return (from item in EpList where CheckStatus(item.Status, Status.NotLocked) select item).ToList<dtoEPitemList>();
        }
        public int GetEduPathCountInCommunity(int CommunityId, bool OnlyVisible)
        {
            IList<Path> PathList = Manager.GetAll<Path>(p => p.Community.Id == CommunityId && p.Deleted == BaseStatusDeleted.None).ToList();
            if (OnlyVisible)
            {

                return (from p in PathList where !((p.Status & Status.Draft) == Status.Draft) && !((p.Status & Status.Locked) == Status.Locked) select p).Count();
            }
            else
            {
                return (from p in PathList where !((p.Status & Status.Draft) == Status.Draft) select p).Count();
            }
        }

        public int GetPathCount_ViewMode(int userId, int cRoleId, int currentCommunityId)
        {
            var pathInCommunity = (from p in Manager.GetIQ<Path>()
                                   where p.Community.Id == currentCommunityId && p.Deleted == BaseStatusDeleted.None
                                   select new { Id = p.Id, pStatus = p.Status }).ToList();

            return (from p in pathInCommunity
                    where !CheckStatus(p.pStatus, Status.Draft) && CheckRoleEp(GetUserRole_ByPath(p.Id, userId, cRoleId), RoleEP.Participant)
                    select p.Id).Count();
        }


        public IList<Activity> GetActiveActivities(IList<Activity> Activities, int UserId, int CRoleId, bool OnlyVisible, bool isManageMode)
        {
            if (OnlyVisible && !isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && ActivityIsVisibleForParticipant(activity.Id, UserId, CRoleId)
                        orderby activity.DisplayOrder
                        select activity).ToList();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
                        orderby activity.DisplayOrder
                        select activity).ToList();
            }
            else if (OnlyVisible && isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && activity.CheckStatus(Status.NotLocked)
                        select activity).ToList();
            }
            else if (!OnlyVisible && isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
                        orderby activity.DisplayOrder
                        select activity).ToList();
            }
            else
            {
                return null;
            }
        }
         public Boolean isCertificationActionActive(long idPath, dtoSubActivity sActivity, List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links, List<dtoQuizInfo> qItems, Int32 idUser, DateTime viewStatBefore){
             return isCertificationActionActive(idPath, sActivity.Id, sActivity.ActiveOnMinCompletion, sActivity.ActiveOnMinMark, sActivity.ActiveAfterPathEndDate, sActivity.UsePathEndDateStatistics,links, qItems, idUser, viewStatBefore);
         }
         public Boolean isCertificationActionActive(long idPath, dtoSubActivityCertificate sActivity, List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links, List<dtoQuizInfo> qItems, Int32 idUser, DateTime viewStatBefore)
         {
             return isCertificationActionActive(idPath, sActivity.Id, sActivity.ActiveOnMinCompletion, sActivity.ActiveOnMinMark, sActivity.ActiveAfterPathEndDate, sActivity.UsePathEndDateStatistics,links, qItems, idUser, viewStatBefore);
         }
        public Boolean isCertificationActionActive(long idPath, long idSubactivity, Boolean activeOnMinCompletion, Boolean activeOnMinMark, Boolean activeAfterPathEndDate, Boolean usePathEndDateStatistics,List<lm.Comol.Modules.EduPath.Domain.DTO.dtoSubActivityLink> links, List<dtoQuizInfo> qItems, Int32 idUser, DateTime viewStatBefore)
        {
            Boolean active = false;
            Path p = Manager.Get<Path>(idPath);
            DateTime? endDate = GetEpEndDate(p, idUser);
            
            if (idSubactivity > 0 && p != null && (activeAfterPathEndDate || !endDate.HasValue) || (endDate.HasValue && !activeAfterPathEndDate && endDate.Value >= viewStatBefore))
            {
                dtoStatWithWeight stat = ServiceStat.GetPassedCompletedWeight_byActivity(idPath, idUser, (usePathEndDateStatistics && p.EndDate.HasValue) ? endDate.Value : viewStatBefore);
                active = ((!activeOnMinCompletion && CheckEpType(p.EPType, EPType.Time)) || (CheckEpType(p.EPType, EPType.Mark) && !activeOnMinMark)) || ((activeOnMinCompletion || activeOnMinMark) && stat.Completion >= stat.MinCompletion);
                if (links != null && links.Any())
                {
                    links.Where(l => l.Mandatory).Select(l => l.IdObject).ToList().ForEach(l => active = active && qItems.Where(q => q.IdQuestionnaire == l && ((!usePathEndDateStatistics && q.Passed) || (usePathEndDateStatistics && q.PassedByDate(endDate)))).Any());
                    // DA SISTEMARE IN CASO FUTURO SERVA VERIFICARE IL PASSATO
                    //links.Where(l => !l.Mandatory && l.Visible).Select(l => l.IdObject).ToList().ForEach(l => active = active && qItems.Where(q => q.IdQuestionnaire == l && ((!usePathEndDateStatistics) || (usePathEndDateStatistics && q.CompiledBeforeDate(endDate)))).Any());
                }                
            }
            return active;
        }


        
        public IList<long> GetActiveActivitiesId(IList<Activity> Activities, int UserId, int CRoleId, bool OnlyVisible, bool isManageMode)
        {
            if (OnlyVisible && !isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && ActivityIsVisibleForParticipant(activity.Id, UserId, CRoleId)
                        orderby activity.DisplayOrder
                        select activity.Id).ToList();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
                        orderby activity.DisplayOrder
                        select activity.Id).ToList();
            }
            else if (OnlyVisible && isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && activity.CheckStatus(Status.NotLocked)
                        select activity.Id).ToList();
            }
            else if (!OnlyVisible && isManageMode)
            {
                return (from activity in Activities
                        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
                        orderby activity.DisplayOrder
                        select activity.Id).ToList();
            }
            else
            {
                return null;
            }
        }

        public IList<Unit> GetActiveUnits(IList<Unit> Units, int UserId, int CRoleId, bool OnlyVisible, bool isManageMode)
        {
            if (OnlyVisible && !isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && UnitIsVisibleForParticipant(unit.Id, UserId, CRoleId)
                        orderby unit.DisplayOrder
                        select unit).ToList();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft)
                        orderby unit.DisplayOrder
                        select unit).ToList();
            }
            else if (OnlyVisible && isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && unit.CheckStatus(Status.NotLocked)
                        orderby unit.DisplayOrder
                        select unit).ToList();
            }
            else if (!OnlyVisible && isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft)
                        orderby unit.DisplayOrder
                        select unit).ToList();
            }
            else
            {
                return new List<Unit>();
            }
        }

        public IList<long> GetActiveUnitsId(IList<Unit> Units, int UserId, int CRoleId, bool OnlyVisible, bool isManageMode)
        {
            if (OnlyVisible && !isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && UnitIsVisibleForParticipant(unit.Id, UserId, CRoleId)
                        orderby unit.DisplayOrder
                        select unit.Id).ToList();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft)
                        orderby unit.DisplayOrder
                        select unit.Id).ToList();
            }
            else if (OnlyVisible && isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && unit.CheckStatus(Status.NotLocked)
                        orderby unit.DisplayOrder
                        select unit.Id).ToList();
            }
            else if (!OnlyVisible && isManageMode)
            {
                return (from unit in Units
                        where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft)
                        orderby unit.DisplayOrder
                        select unit.Id).ToList();
            }
            else
            {
                return new List<long>();
            }
        }

        public dtoEduPath GetEduPathStructure_Manage(long idPath, int idPerson, int idRole, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Path oPath = Manager.Get<Path>(idPath);
            RoleEP RoleEPinPath = GetUserRole_ByPath(idPath, idPerson, idRole);
            dtoEduPath dtoEpStructure = null;
            if (oPath.Deleted == BaseStatusDeleted.None && !((oPath.Status & Status.Draft) == Status.Draft))
            {
                dtoEpStructure = new dtoEduPath(oPath, PathStatus(idPath, idPerson, idRole, true), RoleEPinPath);
                dtoPathManageStatistics pathStat = GetPathManageStatistics(idPath, idPerson, (oPath.Community != null ? oPath.Community.Id : 0), idRole);
                ////get number of unit where user is manager and Evaluator              
                //Int16 UnitToManage = GetUnitToManageCount(oPath.Id, idPerson, oPath.Community.Id, idRole);
                //Int16 UnitToEvaluate = GetUnitToEvaluateCount(oPath.Id, idPerson, oPath.Community.Id, idRole);
                ////get number of activity where user is manager and evaluator         
                //Int16 ActivityToManage = GetActivityToManageCount(oPath.Id, idPerson, oPath.Community.Id, idRole);
                //Int16 ActivityToEvaluate = GetActivityToEvaluateCount(oPath.Id, idPerson, oPath.Community.Id, idRole);

                if (pathStat.UnitsToEvaluate > 0 || pathStat.UnitsToEvaluate > 0 || pathStat.ActivitiesToManage > 0 || pathStat.ActivitiesToEvaluate > 0 || RoleEPinPath > RoleEP.Participant)
                {
                    dtoEpStructure.Units = (from unit in oPath.UnitList
                                            where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft)
                                            orderby unit.DisplayOrder
                                            select new dtoUnit(unit, UnitStatus(unit.Id, idPerson, idRole, true), GetUserRole_ByUnit(unit.Id, idPerson, idRole),
                                                RulesChecked, GetdtoActiveActivities_Manage(unit.ActivityList, idPerson, idRole))).ToList();
                }

            }
            return dtoEpStructure;
        }

        public dtoEduPath GetEduPathStructure_View(long PathId, int UserId, int CRoleId, String PersonIpAddress, String PersonProxyIpAddress, DateTime viewStatBefore)
        {
            Path oPath = Manager.Get<Path>(PathId);


            //verify person assignment

            dtoEduPath dtoEpStructure = new dtoEduPath(oPath, PathStatus(PathId, UserId, CRoleId, false), RoleEP.Participant) { statusStat = ServiceStat.GetStatusStat_byPathStat(PathId, UserId, viewStatBefore) };
            Func<Unit, bool> UnitCondition = unit => !((unit.Status & Status.Draft) == Status.Draft) && unit.Deleted == BaseStatusDeleted.None;
            Func<Activity, bool> ActivityCondition = activity => !((activity.Status & Status.Draft) == Status.Draft) && activity.Deleted == BaseStatusDeleted.None;

            dtoEpStructure.Units = (from unit in oPath.UnitList
                                    where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft)
                                    orderby unit.DisplayOrder
                                    select new dtoUnit(unit, UnitStatus(unit.Id, UserId, CRoleId, false), RoleEP.Participant,
                                        RulesChecked, GetdtoActiveActivities_View(unit.ActivityList, UserId, CRoleId, viewStatBefore)) { statusStat = ServiceStat.GetStatusStat_byUnitStat(unit.Id, UserId, viewStatBefore) }
                                        ).ToList();
            return dtoEpStructure;


        }

        private dtoEduPath SubGetEduPathStruct_OnlyViewMode(Path oPath, int UserId, int CRoleId, bool isPlayMode, bool viewStatusStat, DateTime viewStatBefore)
        {
            //verify person assignment

            dtoEduPath dtoEpStructure = new dtoEduPath(oPath, PathStatus(oPath.Id, UserId, CRoleId, false), RoleEP.Participant) { statusStat = ServiceStat.GetStatusStat_byPathStat(oPath.Id, UserId, viewStatBefore) };
            Func<Unit, bool> UnitCondition = unit => !((unit.Status & Status.Draft) == Status.Draft) && unit.Deleted == BaseStatusDeleted.None;
            //   Func<Activity, bool> ActivityCondition = activity => !((activity.Status & Status.Draft) == Status.Draft) && activity.Deleted == BaseStatusDeleted.None;

            dtoEpStructure.Units = (
                from unit in oPath.UnitList
                where 
                    unit.Deleted == BaseStatusDeleted.None 
                    && !unit.CheckStatus(Status.Draft)
                orderby 
                    unit.DisplayOrder
                select new dtoUnit(
                    unit, 
                    UnitStatus(unit.Id, UserId, CRoleId, false),
                    RoleEP.Participant,
                    RulesChecked, 
                    GetdtoActiveActivities_View(
                        unit.ActivityList, 
                        UserId, 
                        CRoleId, 
                        viewStatBefore)) 
                        { statusStat = 
                            ServiceStat.GetStatusStat_byUnitStat(
                                unit.Id, 
                                UserId, 
                                viewStatBefore) 
                        }
                ).ToList();
            return dtoEpStructure;
        }

        public dtoEduPath GetEduPathStructure_PlayMode(long pathId, int UserId, int CRoleId, DateTime viewStatBefore)
        {
            return SubGetEduPathStruct_OnlyViewMode(Manager.Get<Path>(pathId), UserId, CRoleId, true, true, viewStatBefore);
        }


        public IList<dtoActivity> GetdtoActiveActivities_Manage(IList<Activity> Activities, int UserId, int CRoleId)
        {
            return (from activity in Activities
                    where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
                    orderby activity.DisplayOrder
                    select new dtoActivity(activity, activity.Status, GetUserRole_ByActivity(activity.Id, UserId, CRoleId), RulesChecked)).ToList();

        }


        private IList<dtoActivity> GetdtoActiveActivities_View(IList<Activity> Activities, int UserId, int CRoleId, DateTime viewStatBefore)
        {
            return (from activity in Activities
                    where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
                    orderby activity.DisplayOrder
                    select new dtoActivity(activity, ActivityStatus(activity.Id, UserId, CRoleId, false), RoleEP.Participant, RulesChecked, GetSubActivitiesStructure_View(activity.SubActivityList, UserId, CRoleId, viewStatBefore)) { statusStat = ServiceStat.GetStatusStat_byActivityStat(activity.Id, UserId, viewStatBefore) }).ToList();
        }

        public dtoActivityPlayer GetLastViewedActivity(int userId, int croleId, long PathId, bool isAutoEp, DateTime viewStatBefore)
        {
            dtoActivityPlayer LastActivity = null;
            Activity oLastAct = ServiceStat.GetActivity_ByLastModifyStatistic(PathId, userId, croleId, isAutoEp);
            if (oLastAct == null)//Path Non Ancora iniziato
            {
                return null;

            }
            else
            {
                LastActivity = new dtoActivityPlayer() { Id = oLastAct.Id, ParentUnitId = oLastAct.ParentUnit.Id, Name = oLastAct.Name, Description = oLastAct.Description, StartDate = oLastAct.StartDate, EndDate = oLastAct.EndDate, StatusStatistic = ServiceStat.GetStatusStat_byActivityStat(oLastAct.Id, userId, viewStatBefore) };
            }
            return LastActivity;
        }

        public IList<dtoActivityPlayer> GetViewedActivity(int userId, int croleId, long PathId, bool isAutoEp, DateTime viewStatBefore)
        {
            IList<dtoActivityPlayer> LastActivity = null;
            IList<Activity> oLastAct = ServiceStat.GetActivities_ByLastModifyStatistic(PathId, userId, croleId, isAutoEp);
            if (oLastAct == null)//Path Non Ancora iniziato
            {
                return null;

            }
            else
            {
                LastActivity = (from item in oLastAct select new dtoActivityPlayer() { Id = item.Id, ParentUnitId = item.ParentUnit.Id, Name = item.Name, Description = item.Description, StartDate = item.StartDate, EndDate = item.EndDate, StatusStatistic = ServiceStat.GetStatusStat_byActivityStat(item.Id, userId, viewStatBefore) }).ToList();
                //LastActivity = new dtoActivityPlayer() { Id = oLastAct.Id, ParentUnitId = oLastAct.ParentUnit.Id, Name = oLastAct.Name, Description = oLastAct.Description, StartDate = oLastAct.StartDate, EndDate = oLastAct.EndDate, StatusStatistic = ServiceStat.GetStatusStat_byActivityStat(oLastAct.Id, userId, viewStatBefore) };
            }
            return LastActivity;
        }

        public Int64 GetCountDtoSubActivity_View(Int64 idActivity, int UserId, int idRole)
        {
            //return (from subact in Activities
            //        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
            //        orderby activity.DisplayOrder
            //        select new dtoActivity(activity, ActivityStatus(activity.Id, UserId, CRoleId, false), RoleEP.Participant, RulesChecked, GetSubActivitiesStructure_View(activity.SubActivityList, UserId, CRoleId, viewStatBefore)) { statusStat = ServiceStat.GetStatusStat_byActivityStat(activity.Id, UserId, viewStatBefore) }).ToList();

            //Activity activity = Manager.Get<Activity>(idActivity);

            return (from subact in Manager.GetIQ<SubActivity>()
                    where subact.ParentActivity != null && subact.ParentActivity.Id == idActivity
                    && subact.Deleted == BaseStatusDeleted.None 
                    //&& !subact.CheckStatus(Status.Draft)
                    select subact.Id).Count();
        }

        public SubActivity GetFirstSubActivity_View(Int64 idActivity, int UserId, int CRoleId)
        {
            //return (from subact in Activities
            //        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
            //        orderby activity.DisplayOrder
            //        select new dtoActivity(activity, ActivityStatus(activity.Id, UserId, CRoleId, false), RoleEP.Participant, RulesChecked, GetSubActivitiesStructure_View(activity.SubActivityList, UserId, CRoleId, viewStatBefore)) { statusStat = ServiceStat.GetStatusStat_byActivityStat(activity.Id, UserId, viewStatBefore) }).ToList();

           // Activity activity = Manager.Get<Activity>(idActivity);

            return (from subact in Manager.GetIQ<SubActivity>()
                    where subact.ParentActivity != null && subact.ParentActivity.Id == idActivity
                    && subact.Deleted == BaseStatusDeleted.None
                    //&& !subact.CheckStatus(Status.Draft)
                    select subact).First();
        }

        //public dtoSubActivity GetFirstDtoSubActivity_View(Int64 ActivityId, int UserId, int CRoleId)
        //{
        //    //return (from subact in Activities
        //    //        where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft)
        //    //        orderby activity.DisplayOrder
        //    //        select new dtoActivity(activity, ActivityStatus(activity.Id, UserId, CRoleId, false), RoleEP.Participant, RulesChecked, GetSubActivitiesStructure_View(activity.SubActivityList, UserId, CRoleId, viewStatBefore)) { statusStat = ServiceStat.GetStatusStat_byActivityStat(activity.Id, UserId, viewStatBefore) }).ToList();
        //    Activity activity = Manager.Get<Activity>(ActivityId);
        //    return (from subact in Manager.GetIQ<SubActivity>()
        //            where subact.ParentActivity == activity 
        //            && subact.Deleted == BaseStatusDeleted.None 
        //            //&& !subact.CheckStatus(Status.Draft)
        //            select
        //                new dtoSubActivity(subact, SubActivityStatus(subact.Id, UserId, CRoleId, false))).First();
        //}


        public bool IsSubActivityModificable(SubActivityType ContentType)
        {
            switch (ContentType)
            {
                case SubActivityType.Quiz:
                    return true;
                case SubActivityType.Text:
                    return true;
                case SubActivityType.Certificate:
                    return true;
                default:
                    return false;
            }
        }

        public bool isSubActityInternalModule(SubActivityType ContentType)
        {
            switch (ContentType)
            {
                case SubActivityType.Certificate:
                case SubActivityType.Text:
                    return true;

                default:
                    return false;
            }
        }


        #endregion

        #region View Activity and SubActivity

        public dtoNavigationActivity GetdtoNextActivity(long CurrentActivityId, int userId, int croleId)
        {
            dtoNavigationActivity dtoNextActivity = null;
            Activity CurrentActivity = Manager.Get<Activity>(CurrentActivityId);

            dtoNextActivity = GetNextActivityFilter(Manager.GetAll<Activity>(act => act.ParentUnit.Id == CurrentActivity.ParentUnit.Id && act.DisplayOrder > CurrentActivity.DisplayOrder && act.Deleted == BaseStatusDeleted.None), userId, croleId);

            if (dtoNextActivity == null)
            {
                Unit CurrentUnit = Manager.Get<Unit>(CurrentActivity.ParentUnit.Id);
                IList<Unit> NextUnits = (from unit in Manager.GetAll<Unit>(u => u.ParentPath.Id == CurrentUnit.ParentPath.Id && u.DisplayOrder > CurrentUnit.DisplayOrder && u.Deleted == BaseStatusDeleted.None)
                                         where !unit.CheckStatus(Status.Draft) && UnitIsVisibleForParticipant(unit.Id, userId, croleId) && GetActiveActivitiesCount(unit.ActivityList, userId, croleId, false, false) > 0 && this.RulesChecked
                                         orderby unit.DisplayOrder
                                         select unit).ToList();
                if (NextUnits.Count > 0)
                {
                    dtoNextActivity = GetNextActivityFilter(NextUnits[0].ActivityList, userId, croleId);
                }
            }
            return dtoNextActivity;
        }

        private dtoNavigationActivity GetNextActivityFilter(IList<Activity> NextActivitiesWithDraftLocked, int userId, int croleId)
        {
            IList<Activity> NextActivitiesFilter = (from activity in NextActivitiesWithDraftLocked
                                                    where !activity.CheckStatus(Status.Draft) && ActivityIsVisibleForParticipant(activity.Id, userId, croleId) && this.RulesChecked
                                                    orderby activity.DisplayOrder
                                                    select activity).ToList();
            if (NextActivitiesFilter.Count > 0)
            {
                return new dtoNavigationActivity() { ActivityId = NextActivitiesFilter[0].Id, ParentUnitId = NextActivitiesFilter[0].ParentUnit.Id, ParentUnitName = NextActivitiesFilter[0].ParentUnit.Name };
            }
            return null;
        }

        public dtoNavigationActivity GetdtoPreviousActivity(long CurrentActivityId, int userId, int croleID)
        {
            dtoNavigationActivity dtoPreviousActivity = null;
            Activity CurrentActivity = Manager.Get<Activity>(CurrentActivityId);

            dtoPreviousActivity = GetPreviousActivityFilter(Manager.GetAll<Activity>(act => act.ParentUnit.Id == CurrentActivity.ParentUnit.Id && act.DisplayOrder < CurrentActivity.DisplayOrder && act.DisplayOrder > 0 && act.Deleted == BaseStatusDeleted.None), userId, croleID);

            if (dtoPreviousActivity == null)
            {
                Unit CurrentUnit = Manager.Get<Unit>(CurrentActivity.ParentUnit.Id);
                IList<Unit> PreviousUnits = (from unit in Manager.GetAll<Unit>(u => u.ParentPath.Id == CurrentUnit.ParentPath.Id && u.DisplayOrder < CurrentUnit.DisplayOrder && u.DisplayOrder > 0 && u.Deleted == BaseStatusDeleted.None)
                                             where !unit.CheckStatus(Status.Draft) && UnitIsVisibleForParticipant(unit.Id, userId, croleID) && GetActiveActivitiesCount(unit.ActivityList, userId, croleID, false, false) > 0 && this.RulesChecked
                                             orderby unit.DisplayOrder
                                             select unit).ToList();
                if (PreviousUnits.Count > 0)
                {
                    dtoPreviousActivity = GetPreviousActivityFilter(PreviousUnits[0].ActivityList, userId, croleID);
                }
            }

            return dtoPreviousActivity;
        }

        private dtoNavigationActivity GetPreviousActivityFilter(IList<Activity> PreviousActivitiesWithDraftLocked, int userId, int croleID)
        {
            IList<Activity> PreviousActivitiesFilter = (from activity in PreviousActivitiesWithDraftLocked
                                                        where !activity.CheckStatus(Status.Draft) && ActivityIsVisibleForParticipant(activity.Id, userId, croleID) && this.RulesChecked
                                                        orderby activity.DisplayOrder descending
                                                        select activity).ToList();
            if (PreviousActivitiesFilter.Count > 0)
            {
                return new dtoNavigationActivity() { ActivityId = PreviousActivitiesFilter[0].Id, ParentUnitId = PreviousActivitiesFilter[0].ParentUnit.Id, ParentUnitName = PreviousActivitiesFilter[0].ParentUnit.Name };
            }
            return null;
        }

        public Int16 GetActiveSubActivitiesCount(IList<SubActivity> SubActivities, int userId, int croleId, bool OnlyVisible, bool isManageMode)
        {
            Int16 count = 0;
            if (OnlyVisible && !isManageMode)
            {
                count = (Int16)(from subAct in SubActivities
                                where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft) && SubActivityIsVisibleForParticipant(subAct.Id, userId, croleId)
                                orderby subAct.DisplayOrder
                                select subAct).Count();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                count = (Int16)(from subAct in SubActivities
                                where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft)
                                orderby subAct.DisplayOrder
                                select subAct).Count();
            }
            else if (OnlyVisible && isManageMode)
            {
                count = (Int16)(from subAct in SubActivities
                                where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft) && subAct.CheckStatus(Status.NotLocked)
                                orderby subAct.DisplayOrder
                                select subAct).Count();
            }
            else if (!OnlyVisible && isManageMode)
            {
                count = (Int16)(from subAct in SubActivities
                                where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft)
                                orderby subAct.DisplayOrder
                                select subAct).Count();
            }
            return count;
        }


        public Int64 GetActiveSubActivitiesWeightSum(IList<SubActivity> subActs)
        {
            return (Int64)(from subAct in subActs
                           where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft)
                           select subAct.Weight).Sum(a => a);
        }

        /// <summary>
        /// no draft, no deleted
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public Int16 GetActiveMandatorySubActivitiesCount_NotPersonalizeProperties(IList<SubActivity> subActs)
        {
            return (Int16)(from subAct in subActs
                           where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft) && subAct.CheckStatus(Status.Mandatory)
                           select subAct).Count();
        }

        /// <summary>
        /// no draft, no deleted
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public Int16 GetActiveMandatorySubActivitiesCount(IList<SubActivity> subActs, int userId, int croleId)
        {
            return (Int16)(from subAct in subActs
                           where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft) && SubActivityIsMandatoryForParticipant(subAct.Id, userId, croleId)
                           select subAct).Count();
        }

        /// <summary>
        /// no draft, no deleted, no Note testuali
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public Int16 GetActiveMandatoryUnitCount(long pathId, int userId, int croleId)
        {
            return (Int16)(from unit in Manager.Get<Path>(pathId).UnitList
                           where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text) && UnitIsMandatoryForParticipant(unit.Id, userId, croleId)
                           select unit).Count();
        }

        /// <summary>
        /// no draft, no deleted
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IList<SubActivity> GetActiveSubActivities(IList<SubActivity> SubActivities)
        {
            return (from subAct in SubActivities
                    where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft)
                    orderby subAct.DisplayOrder
                    select subAct).ToList();
        }

        /// <summary>
        /// no draft, no deleted
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IList<long> GetActiveSubActivitiesId(IList<SubActivity> SubActivities)
        {

            return (from subAct in SubActivities
                    where subAct.Deleted == BaseStatusDeleted.None && !subAct.CheckStatus(Status.Draft)
                    orderby subAct.DisplayOrder
                    select subAct.Id).ToList();

        }

        /// <summary>
        /// no draft, no deleted, no note di testo
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IList<Activity> GetActiveActivity_ByPathId(long pathId)
        {
            return (from act in Manager.GetAll<Activity>(a => a.Path.Id == pathId && a.Deleted == BaseStatusDeleted.None)
                    where act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
                    select act).ToList();
        }

        ///// <summary>
        ///// Sum of Activities Weight
        ///// no draft, no deleted
        ///// </summary>
        ///// <param name="pathId"></param>
        ///// <returns></returns>
        //public long GetPathWeight(long pathId)
        //{
        //    return (from act in Manager.GetAll<Activity>(a => a.Path.Id == pathId && a.Deleted == BaseStatusDeleted.None)
        //            where act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
        //            select act.Weight).Sum(a => a);
        //}



        /// <summary>
        /// no draft, no deleted
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public Int64 GetActiveActivitiesWeightSum(long unitId)
        {
            return (Int64)(from activity in Manager.Get<Unit>(unitId).ActivityList
                           where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && !activity.CheckStatus(Status.Text)
                           select activity.Weight).Sum(a => a);
        }

        public Int16 GetActiveMandatoryActivitiesCount_NotPersonalizeProperties(long unitId)
        {
            return (Int16)(from activity in Manager.Get<Unit>(unitId).ActivityList
                           where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && activity.CheckStatus(Status.Mandatory) && !activity.CheckStatus(Status.Text)
                           select activity).Count();
        }

        public Int16 GetActiveMandatoryActivitiesCount(long unitId, int userId, int croleId)
        {
            return (Int16)(from activity in Manager.Get<Unit>(unitId).ActivityList
                           where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && !activity.CheckStatus(Status.Text) && ActivityIsMandatoryForParticipant(activity.Id, userId, croleId)
                           select activity).Count();
        }

        public Int16 GetActiveMandatoryActivitiesCount_byPathId(long pathId, int userId, int croleId)
        {
            return (Int16)(from activity in Manager.GetAll<Activity>(a => a.Path.Id == pathId && a.Deleted == BaseStatusDeleted.None)
                           where !activity.CheckStatus(Status.Draft) && !activity.CheckStatus(Status.Text) && ActivityIsMandatoryForParticipant(activity.Id, userId, croleId)
                           select activity).Count();
        }



        //OK
        public Int16 GetActiveActivitiesCount(IList<Activity> Activities, int userId, int croleId, bool OnlyVisible, bool isManageMode)
        {
            Int16 count = 0;
            if (OnlyVisible && !isManageMode)
            {
                count = (Int16)(from activity in Activities
                                where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && !activity.CheckStatus(Status.Text) && ActivityIsVisibleForParticipant(activity.Id, userId, croleId)
                                select activity).Count();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                count = (Int16)(from activity in Activities
                                where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && !activity.CheckStatus(Status.Text)
                                select activity).Count();
            }
            else if (OnlyVisible && isManageMode)
            {
                count = (Int16)(from activity in Activities
                                where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && activity.CheckStatus(Status.NotLocked) && !activity.CheckStatus(Status.Text)
                                select activity).Count();
            }
            else if (!OnlyVisible && isManageMode)
            {
                count = (Int16)(from activity in Activities
                                where activity.Deleted == BaseStatusDeleted.None && !activity.CheckStatus(Status.Draft) && !activity.CheckStatus(Status.Text)
                                select activity).Count();
            }

            return count;
        }

        public Int16 GetActiveUnitsCount(IList<Unit> Units, int userId, int croleId, bool OnlyVisible, bool isManageMode)
        {
            Int16 count;
            if (OnlyVisible && !isManageMode)
            {
                count = (Int16)(from unit in Units
                                where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text) && UnitIsVisibleForParticipant(unit.Id, userId, croleId)
                                select unit).Count();
            }
            else if (!OnlyVisible && !isManageMode)
            {
                count = (Int16)(from unit in Units
                                where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                                select unit).Count();
            }
            else if (OnlyVisible && isManageMode)
            {
                count = (Int16)(from unit in Units
                                where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && unit.CheckStatus(Status.NotLocked) && !unit.CheckStatus(Status.Text)
                                select unit).Count();
            }
            else
            {
                count = (Int16)(from unit in Units
                                where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                                select unit).Count();
            }
            return count;
        }

        public Int64 GetActiveUnitsWeightSum(long pathId)
        {
            return (Int64)(from unit in Manager.Get<Path>(pathId).UnitList
                           where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && unit.CheckStatus(Status.NotLocked) && !unit.CheckStatus(Status.Text)
                           select unit.Weight).DefaultIfEmpty(0).Sum(a => a);
        }

        public Int16 GetActiveMandatoryUnitsCount_NotPersonalizeProperties(long pathId)
        {
            return (Int16)(from unit in Manager.Get<Path>(pathId).UnitList
                           where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && unit.CheckStatus(Status.Mandatory) && unit.CheckStatus(Status.NotLocked)
                           select unit).Count();
        }

        public IList<dtoSubActivity> GetSubActivitiesStructure_View(IList<SubActivity> subActivities, int PersonId, int CRoleId, DateTime viewStatBefore)
        {

            return (from subAct in subActivities
                    where subAct.Deleted == BaseStatusDeleted.None
                        && !subAct.CheckStatus(Status.Draft)
                    orderby subAct.DisplayOrder
                    select new dtoSubActivity(subAct, SubActivityStatus(subAct.Id, PersonId, CRoleId, false), ServiceStat.GetStatusCompletion_bySubActivityStatistic(subAct.Id, PersonId, viewStatBefore))).ToList();
        }

        public IList<dtoSubActivity> GetSubActivitiesStructure_Manage(IList<SubActivity> subActivities, liteCommunity community, int PersonId, int CRoleId, RoleEP ActivityRole, String unknownUserTranslation)
        {
            List<dtoSubActivity> sItems = (from sub in subActivities
                    where !sub.CheckStatus(Status.Draft) && sub.Deleted == BaseStatusDeleted.None
                    orderby sub.DisplayOrder
                    select new dtoSubActivity(sub, SubActivityStatus(sub.Id, PersonId, CRoleId, true), ActivityRole)).ToList();
            if (sItems.Any(s => s.ContentType == SubActivityType.File ))
            {
                lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier =lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier.Create(lm.Comol.Core.FileRepository.Domain.RepositoryType.Community, (community == null ? 0 : community.Id));
                lm.Comol.Core.FileRepository.Domain.ModuleRepository pRepository = ServiceRepository.GetPermissions(identifier, PersonId);
                List<long> idItems = sItems.Where(s=> s.ContentType== SubActivityType.File ).Select(s=> s.IdObject).Distinct().ToList();
                List<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem> items = (from lm.Comol.Core.FileRepository.Domain.liteRepositoryItem item in Manager.GetIQ<lm.Comol.Core.FileRepository.Domain.liteRepositoryItem>()
                                                                                      where item.Deleted== BaseStatusDeleted.None && idItems.Contains(item.Id) select item).ToList();
                List<long> allowVersioning = items.Where(i=> i.IsInternal).Select(i=> i.Id).ToList();
                idItems = idItems.Except(allowVersioning).ToList();
                if (idItems.Any()){
                    List<lm.Comol.Core.FileRepository.Domain.dtoDisplayRepositoryItem> rItems = ServiceRepository.GetItemsWithPermissions(idItems,PersonId,identifier,unknownUserTranslation, true);
                    if (items!=null && rItems!= null)
                        allowVersioning.AddRange(rItems.Where(i=> i.Permissions.AddVersion).Select(i=> i.Id).ToList());
                }

                foreach (dtoSubActivity s in sItems.Where(s => s.ContentType == SubActivityType.File))
                {
                    s.AllowAddVersion = allowVersioning.Contains(s.IdObject);
                }
            }
            return sItems;
        }


        public dtoActivity GetActivityStructure_Manage(long ActivityId, int PersonId, int CRoleId, String unknownUserTranslation)
        {
            dtoActivity dtoActivity = null;

            RoleEP ActivityRole = GetUserRole_ByActivity(ActivityId, PersonId, CRoleId);

            if (ActivityRole > RoleEP.Participant)
            {
                Activity oActivity = Manager.Get<Activity>(ActivityId);
                IList<dtoSubActivity> dtoSubActivities = GetSubActivitiesStructure_Manage(oActivity.SubActivityList, oActivity.Community, PersonId, CRoleId, ActivityRole, unknownUserTranslation);
                dtoActivity = new dtoActivity(oActivity, oActivity.Status, ActivityRole, RulesChecked, dtoSubActivities);
            }

            return dtoActivity;
        }


        public dtoActivity GetActivityStructure_View(long ActivityId, int PersonId, int CRoleId, String PersonIpAddress, String PersonProxyIpAddress, DateTime viewStatBefore)
        {
            dtoActivity dtoActivity = null;

            Activity CurrentActivity = Manager.Get<Activity>(ActivityId);


            RoleEP RoleInPath = GetUserRole_ByPath(CurrentActivity.Path.Id, PersonId, CRoleId);
            if (CheckRoleEp(RoleInPath, RoleEP.Participant))
            {

                IList<dtoSubActivity> SubActivities = (from subAct in CurrentActivity.SubActivityList
                                                       where subAct.Deleted == BaseStatusDeleted.None
                                                           && !subAct.CheckStatus(Status.Draft)
                                                       orderby subAct.DisplayOrder
                                                       select new dtoSubActivity(subAct, SubActivityStatus(subAct.Id, PersonId, CRoleId, false), ServiceStat.GetStatusCompletion_bySubActivityStatistic(subAct.Id, PersonId, viewStatBefore))).ToList();
                dtoActivity = new dtoActivity(CurrentActivity, ActivityStatus(ActivityId, PersonId, CRoleId, false), RoleEP.Participant, RulesChecked, SubActivities);
            }

            return dtoActivity;
        }

        public Int64 GetUnitId_ByActivityId(long ActivityId)
        {
            if (ActivityId != -1)
            {
                return Manager.Get<Activity>(ActivityId).ParentUnit.Id;
            }
            else
            {
                return -1;
            }
        }
        public Int64 GetPathId_ByActivityId(long idActivity)
        {
            if (idActivity != -1)
                return (from a in Manager.GetIQ<Activity>() where a.Id== idActivity && a.Path != null select a.Path.Id).Skip(0).Take(1).ToList().FirstOrDefault();
            else
                return -1;
        }

        public Int64 GetPathId_BySubActivityId(long idSubActivity)
        {
            if (idSubActivity != -1)
            {
                return (from a in Manager.GetIQ<SubActivity>() where a.Id == idSubActivity && a.Path != null select a.Path.Id).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            else
            {
                return -1;
            }
        }

        public Int64 GetPathId_ByUnitId(long UnitId)
        {
            if (UnitId != -1)
            {
                return Manager.Get<Unit>(UnitId).ParentPath.Id;
            }
            else
            {
                return -1;
            }
        }

        public Int64 GetPathId_ByItemId(long ItemId, ItemType type)
        {
            switch (type)
            {

                case ItemType.Path:
                    return ItemId;

                case ItemType.Unit:
                    return GetPathId_ByUnitId(ItemId);

                case ItemType.Activity:
                    return GetPathId_ByActivityId(ItemId);

                case ItemType.SubActivity:
                    return GetPathId_BySubActivityId(ItemId);

                default:
                    return -1;
            }

        }

        #endregion

        #region set MetaInfo modify

        //public void SetSubActivityLinkModifyMetaInfo(SubActivityLink oSubActityLink, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oSubActityLink.ModifiedBy = AuthorOfModify;
        //    oSubActityLink.ModifiedIpAddress = PersonIpAddress;
        //    oSubActityLink.ModifiedOn = CurrentTime;
        //    oSubActityLink.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //public void SetSubActivityModifyMetaInfo(SubActivity oSubActity, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oSubActity.ModifiedBy = AuthorOfModify;
        //    oSubActity.ModifiedIpAddress = PersonIpAddress;
        //    oSubActity.ModifiedOn = CurrentTime;
        //    oSubActity.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //public void SetActivityModifyMetaInfo(Activity oActity, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oActity.ModifiedBy = AuthorOfModify;
        //    oActity.ModifiedIpAddress = PersonIpAddress;
        //    oActity.ModifiedOn = CurrentTime;
        //    oActity.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //public void SetUnitModifyMetaInfo(Unit oUnit, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oUnit.ModifiedBy = AuthorOfModify;
        //    oUnit.ModifiedIpAddress = PersonIpAddress;
        //    oUnit.ModifiedOn = CurrentTime;
        //    oUnit.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //public void SetPathModifyMetaInfo(Path oPath, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oPath.ModifiedBy = AuthorOfModify;
        //    oPath.ModifiedIpAddress = PersonIpAddress;
        //    oPath.ModifiedOn = CurrentTime;
        //    oPath.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        #endregion



        #region get permission\RoleEp over item
        public RoleEP GetAdministrationRoles(RoleEP actualRole, int idUser, Activity activity)
        {
            return GetAdministrationRoles(actualRole,idUser,(activity==null) ? null : activity.Path);
        }
        public RoleEP GetAdministrationRoles(RoleEP actualRole, int idUser, Path path)
        {
            Person p = Manager.GetPerson(idUser);
            if (p != null && path != null)
            {
                switch (p.TypeID)
                {
                    case (int)UserTypeStandard.Administrator:
                    case (int)UserTypeStandard.SysAdmin:
                        actualRole |= RoleEP.Manager;
                        break;
                    case (int)UserTypeStandard.Administrative:
                        if (path != null && path.Community != null)
                        {
                            List<Organization> organizations = PMService.GetAvailableOrganizations(idUser, SearchCommunityFor.CommunityManagement);
                            if (organizations.Select(o => o.Id == path.Community.IdOrganization).Any())
                            {
                                actualRole |= RoleEP.Manager;
                            }
                        }
                        break;
                }
            }
            return actualRole;
        }
        public RoleEP GetUserRole_ByActivity(long ActivityId, int UserId, int CRoleId, Boolean skipDraftActivity)
        {
            RoleEP Role = RoleEP.None;

            if (ActivityId > 0)
            {

                Role = ServiceAssignments.GetUserRole_ByActivityPersonAssignment(ActivityId, UserId, skipDraftActivity);
                if (Role == RoleEP.None)
                {
                    Role = ServiceAssignments.GetUserRole_ByActivityRoleAssignment(ActivityId, CRoleId, skipDraftActivity);
                }
                if (!CheckRoleEp(Role, RoleEP.Manager)) //se non e' manager, verifico che non sia il creatore.
                {
                    Activity oActivity = Manager.Get<Activity>(ActivityId);
                    if (oActivity == null)
                    { return RoleEP.None; }

                    if (skipDraftActivity)
                    {
                        if (!(oActivity == null) &&
                            ((oActivity.IdCreatedBy == UserId && !oActivity.CheckStatus(Status.Draft) && oActivity.Deleted == BaseStatusDeleted.None)
                            || (oActivity.ParentUnit.IdCreatedBy == UserId && !oActivity.ParentUnit.CheckStatus(Status.Draft) && oActivity.ParentUnit.Deleted == BaseStatusDeleted.None)
                            || (oActivity.Path.IdCreatedBy == UserId && !oActivity.Path.CheckStatus(Status.Draft) && oActivity.Path.Deleted == BaseStatusDeleted.None)))
                        {
                            Role |= RoleEP.Manager;
                        }
                    }
                    else
                    {
                        if (!(oActivity == null) &&
                       ((oActivity.IdCreatedBy == UserId && oActivity.Deleted == BaseStatusDeleted.None)
                       || (oActivity.ParentUnit.IdCreatedBy == UserId && !oActivity.ParentUnit.CheckStatus(Status.Draft) && oActivity.ParentUnit.Deleted == BaseStatusDeleted.None)
                       || (oActivity.Path.IdCreatedBy == UserId && !oActivity.Path.CheckStatus(Status.Draft) && oActivity.Path.Deleted == BaseStatusDeleted.None)))
                        {
                            Role |= RoleEP.Manager;
                        }
                    }
                    /// ADD FOR PORTAL PERMISSION MANAGEMENT
                    /// 
                    if (!CheckRoleEp(Role, RoleEP.Manager))
                        Role = GetAdministrationRoles(Role, UserId, oActivity);
                }

                RoleEP RoleInPath = GetUserRole_ByPath(Manager.Get<Activity>(ActivityId).Path.Id, UserId, CRoleId);
                if (CheckRoleEp(RoleInPath, RoleEP.Participant))
                {
                    Role |= RoleEP.Participant;
                }
            }

            return Role;
        }



        public RoleEP GetUserRole_ByActivity(long ActivityId, int UserId, int CRoleId)
        {
            return GetUserRole_ByActivity(ActivityId, UserId, CRoleId, true);
        }
        public RoleEP GetUserRole_ByUnit(long UnitId, int UserId, int CRoleId)
        {
            RoleEP Role = RoleEP.None;
            if (UnitId > 0)
            {
                Unit oUnit = Manager.Get<Unit>(UnitId);
                if (oUnit == null)
                {
                    return Role;
                }
                Role = ServiceAssignments.GetUserRole_ByUnitPersonAss(UnitId, UserId);
                if (Role == RoleEP.None)
                {
                    Role = ServiceAssignments.GetUserRole_ByUnitRoleAssignment(UnitId, CRoleId);
                }
                if (!CheckRoleEp(Role, RoleEP.Manager)
                    && (oUnit.IdCreatedBy == UserId && !oUnit.CheckStatus(Status.Draft) && oUnit.Deleted == BaseStatusDeleted.None)
                    || (oUnit.ParentPath.Deleted == BaseStatusDeleted.None && oUnit.ParentPath.IdCreatedBy == UserId && !oUnit.CheckStatus(Status.Draft)))
                {
                    Role |= RoleEP.Manager;
                }
                RoleEP RoleInPath = GetUserRole_ByPath(Manager.Get<Unit>(UnitId).ParentPath.Id, UserId, CRoleId);
                if (CheckRoleEp(RoleInPath, RoleEP.Participant))
                {
                    Role |= RoleEP.Participant;
                }
            }
            return Role;
        }

        public RoleEP GetUserRole_ByPath(long PathId, int UserId)
        {
            int crole = Manager.GetActiveSubscriptionIdRole(UserId, GetPathIdCommunity(PathId));
            return GetUserRole_ByPath(PathId, UserId, crole);
        }
        private Dictionary<String, RoleEP> _UserRoles_ByPath = new Dictionary<string, RoleEP>();
        public RoleEP GetUserRole_ByPath(long idPath, int idPerson, int idRole, Boolean force = false)
        {
            if (idPath > 0)
            {
                String key = idPath.ToString() + "-" + idPerson.ToString() + "-" + idRole.ToString() + "-";
                if (_UserRoles_ByPath.ContainsKey(key) && !force)
                    return _UserRoles_ByPath[key];
                else
                    return GetUserRole_ByPath(Manager.Get<Path>(idPath), idPerson, idRole);
            }
            else
                return RoleEP.None;
        }
        public RoleEP GetUserRole_ByPath(Path path, int idPerson, int idRole, Boolean force = false)
        {
            RoleEP Role = RoleEP.None;
            if (path!=null)
            {
                String key = path.Id.ToString() + "-" + idPerson.ToString() + "-" + idRole.ToString() + "-";
                if (_UserRoles_ByPath.ContainsKey(key) && !force)
                    return _UserRoles_ByPath[key];
                else
                {
                    Role = ServiceAssignments.GetUserRole_ByPathPersonAssignment(path, idPerson);
                    if (Role == RoleEP.None)
                    {
                        Role = ServiceAssignments.GetUserRole_ByPathRoleAssignment(path, idRole);
                    }
                    if (!CheckRoleEp(Role, RoleEP.Manager) && path.IdCreatedBy == idPerson && !path.CheckStatus(Status.Draft) && path.Deleted == BaseStatusDeleted.None)
                    {
                        Role |= RoleEP.Manager;
                    }

                    /// ADD FOR PORTAL PERMISSION MANAGEMENT
                    /// 
                    if (Role == RoleEP.None)
                    {
                        Role = GetAdministrationRoles(Role, idPerson, path);
                    }
                    _UserRoles_ByPath[key] = Role;
                }
            }
            return Role;
        }

        public PermissionEP GetUserPermission_ByActivity(long ActivityId, int UserId, int CRoleId, Boolean skipDraft)
        {
            return new PermissionEP(GetUserRole_ByActivity(ActivityId, UserId, CRoleId, skipDraft));
        }
        public PermissionEP GetUserPermission_ByActivity(long ActivityId, int UserId, int CRoleId)
        {
            return GetUserPermission_ByActivity(ActivityId, UserId, CRoleId, true);
        }
        public PermissionEP GetUserPermission_ByUnit(long UnitId, int UserId, int CRoleId)
        {
            return new PermissionEP(GetUserRole_ByUnit(UnitId, UserId, CRoleId));
        }

        public PermissionEP GetUserPermission_ByPath(long PathId, int UserId, int CRoleId)
        {
            return new PermissionEP(GetUserRole_ByPath(PathId, UserId, CRoleId));
        }

            private Dictionary<Int32, ModuleEduPath> _communityPermissions = new Dictionary<int, ModuleEduPath>();
            private Dictionary<Int32, Int32> _communityRole = new Dictionary<int, Int32>();
            private Int32 GetCommunityRole(Int32 idCommunity)
            {
                if (_communityRole==null)
                    _communityRole = new Dictionary<int,int>();

                if (!_communityRole.ContainsKey(idCommunity))
                    _communityRole.Add(idCommunity, Manager.GetSubscriptionIdRole(UC.CurrentUserID, idCommunity));
                else if (idCommunity > 0 && _communityRole[idCommunity] == idCommunity)
                    _communityRole[idCommunity] = Manager.GetSubscriptionIdRole(UC.CurrentUserID, idCommunity);
                return _communityRole[idCommunity];
            }
            public PermissionEP GetCurrentUserPermission_ByPath(long idPath)
            {
                litePath path = Manager.Get<litePath>(idPath);
                if (path == null)
                    return new PermissionEP(RoleEP.None);
                else
                {
                    RoleEP role = GetUserRole_ByPath(idPath, UC.CurrentUserID, GetCommunityRole(path.IdCommunity));
                    if (role != RoleEP.Manager)
                    {
                        ModuleEduPath permissions = GetModulePermissionForStatistics(idPath, role);
                        if (permissions.Administration)
                            role = RoleEP.Manager;

                    }
                    return new PermissionEP(role);
                }
            }



            public ModuleEduPath GetModulePermissionForStatistics(long idPath, RoleEP role = RoleEP.None )
            {
                ModuleEduPath module = new ModuleEduPath();
                litePerson person = Manager.GetLitePerson(UC.CurrentUserID);
                if (person != null)
                {
                    litePath path = Manager.Get<litePath>(idPath);
                    if (path != null)
                    {
                        module.ManagePermission = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                        module.Administration = (person != null && (person.TypeID == (int)UserTypeStandard.SysAdmin || person.TypeID == (int)UserTypeStandard.Administrator));
                        module.ViewMyStatistics = (person != null);
                        if (!module.Administration)
                        {
                            ModuleEduPath cPermissions = null;
                            if (_communityPermissions.ContainsKey(path.IdCommunity))
                                cPermissions = _communityPermissions[path.IdCommunity];
                            else
                            {
                                cPermissions = new ModuleEduPath(Manager.GetModulePermission(person.Id, path.IdCommunity, Manager.GetModuleID(ModuleEduPath.UniqueCode)));
                                _communityPermissions.Add(path.IdCommunity, cPermissions);
                            }

                            module.Administration = cPermissions.Administration;
                            module.ManagePermission = module.ManagePermission || cPermissions.ManagePermission;
                            module.ViewMyStatistics = module.ViewMyStatistics || cPermissions.ViewMyStatistics;
                            module.ViewPathList = module.ViewPathList || cPermissions.ViewPathList;
                            if (!module.Administration)
                            {
                                if (role== RoleEP.None)
                                    role = GetUserRole_ByPath(idPath, person.Id, Manager.GetSubscriptionIdRole(person.Id, path.IdCommunity));
                                module.Administration = (role == RoleEP.Manager);
                                module.ViewMyStatistics = module.ViewMyStatistics || (role == RoleEP.Manager) || (role == RoleEP.StatViewer);
                                module.ViewPathList = (role != RoleEP.None);
                            }
                        }
                    }
                   
                }
                return module;
            }
        #endregion

        public IList<dtoActivity> GetDtoActivitiesByUnitId(long UnitId)
        {
            IList<dtoActivity> temp = (from item in Manager.GetIQ<Activity>() where item.ParentUnit.Id == UnitId && item.Deleted == BaseStatusDeleted.None select new dtoActivity() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoActivity>();

            temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoActivity>();

            return temp;
        }

        public IList<dtoActivity> GetDtoActivitiesByUnitIdFilteredByRules(long UnitId)
        {
            //IList<dtoActivity> temp = (from item in Manager.GetIQ<Activity>() where item.ParentUnit.Id == UnitId && item.Deleted == BaseStatusDeleted.None select new dtoActivity() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoActivity>();

            //IList<long> ruledActivity = GetActivitiesIdWithRule();

            //temp = (from item in temp where !ruledActivity.Contains(item.Id) select item).ToList<dtoActivity>();

            //temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoActivity>();

            //return temp;

            IList<dtoActivity> temp = GetDtoActivitiesByUnitId(UnitId);

            IList<long> ruledActivity = GetActivitiesIdWithRule();

            temp = (from item in temp where !ruledActivity.Contains(item.Id) select item).ToList<dtoActivity>();

            return temp;
        }

        public IList<dtoUnit> GetDtoUnitsByPathIdFilteredByRules(long PathId)
        {
            //IList<dtoUnit> temp = (from item in Manager.GetIQ<Unit>() where item.ParentPath.Id == PathId && item.Deleted == BaseStatusDeleted.None select new dtoUnit() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoUnit>();

            //IList<long> ruledUnit = GetUnitsIdWithRule();

            //temp = (from item in temp where !ruledUnit.Contains(item.Id) select item).ToList<dtoUnit>();

            //temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoUnit>();

            //return temp;

            IList<dtoUnit> temp = GetDtoUnitsByPathId(PathId);

            IList<long> ruledUnit = GetUnitsIdWithRule();

            temp = (from item in temp where !ruledUnit.Contains(item.Id) select item).ToList<dtoUnit>();

            return temp;
        }

        public IList<dtoGenericItem> GetGenericDtoActivitiesByUnitId(long UnitId)
        {
            IList<dtoGenericItem> temp = (from item in Manager.GetIQ<Activity>() where item.ParentUnit.Id == UnitId && item.Deleted == BaseStatusDeleted.None select new dtoGenericItem() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoGenericItem>();

            temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoGenericItem>();

            return temp;
        }

        public IList<dtoGenericItem> GetGenericDtoActivitiesByPathId(long PathId)
        {
            IList<dtoGenericItem> temp = (from item in Manager.GetIQ<Activity>() where item.Path.Id == PathId && item.Deleted == BaseStatusDeleted.None select new dtoGenericItem() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoGenericItem>();

            temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoGenericItem>();

            return temp;
        }

        public IList<dtoUnit> GetDtoUnitsByPathId(long PathId)
        {
            IList<dtoUnit> temp = (from item in Manager.GetIQ<Unit>() where item.ParentPath.Id == PathId && item.Deleted == BaseStatusDeleted.None select new dtoUnit() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoUnit>();

            temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoUnit>();

            return temp;
        }

        ////+EP[Rob]
        //public IList<SubActivity> GetSubactiviesByPathAndType(Path path, SubActivityType type)
        //{
        //    return GetSubactiviesByPathIdAndType(path.Id, type);
        //}

        ////+EP[Rob]
        //public IList<SubActivity> GetSubactiviesByPathIdAndType(Int64 pathId, SubActivityType type)
        //{
        //    IList<SubActivity> temp = (from item in Manager.GetIQ<SubActivity>() where item.Path.Id == pathId && item.ContentType == type && item.Deleted == BaseStatusDeleted.None select item).ToList();
        //    return temp;
        //}
       
        public Boolean PathHasSubActivityType(Path path, params SubActivityType[] types)
        {
            return PathHasSubActivityType(path.Id);
        }
        //+EP[Fra]
        public Boolean PathHasCertificateWithQuiz(Path path)
        {
            return PathHasCertificateWithQuiz(path.Id);
        }

        //+EP[Fra]
        public Boolean PathHasSubActivityType(Int64 idPath, params SubActivityType[] types)
        {
            return (from item in Manager.GetIQ<SubActivity>() 
                    where item.Path.Id == idPath where types.Contains(item.ContentType) && item.Deleted == BaseStatusDeleted.None
                    select item.IdObjectLong).Any();
        }
        //+EP[Fra]
        public Boolean PathHasCertificateWithQuiz(Int64 idPath)
        {
            return PathHasSubActivityType(idPath, SubActivityType.Certificate,SubActivityType.Quiz ) && (from item in Manager.GetIQ<SubActivityLink>()
                                                                                                         where item.ContentType == SubActivityLinkType.quiz && item.Visible && item.Deleted == BaseStatusDeleted.None && item.SubActivity.Path.Id == idPath
                    select item.IdObject).Any();
        }
        //+EP[Rob]
        public IList<SubActivity> GetSubactiviesByPathAndTypes(Path path, params SubActivityType[] types)
        {
            return GetSubactiviesByPathIdAndTypes(path.Id);
        }
         
        //+EP[Rob]
        public IList<Int64> GetSubactiviesIdLinkedObjects(Int64 pathId, params SubActivityType[] types)
        {
            var temp = (from item in Manager.GetIQ<SubActivity>() where item.Path.Id == pathId where types.Contains(item.ContentType) && item.Deleted == BaseStatusDeleted.None select item.IdObjectLong);

            return temp.ToList();
        }

        //+EP[Rob]
        public Int32 CountActivePaths(Int32 cmntId, Boolean isMooc)
        {
            var qPath = (from item in Manager.GetIQ<Path>() where item.Community!=null && item.Community.Id == cmntId && item.Deleted == BaseStatusDeleted.None && item.IsMooc == isMooc select new { IdPath = item.Id, Status = item.Status }).ToList();

            return (from p in qPath where !CheckStatus(p.Status, Status.Draft) select p.IdPath).Count();
        }
        

        //public IList<Int64> GetActivePathsId(Int32 cmntId)
        //{
        //    var query = (from item in Manager.GetIQ<Path>() where item.Community.Id == cmntId && item.Deleted == BaseStatusDeleted.None && !CheckStatus(item.Status, Status.Draft) select item.Id);            

        //    return query.ToList();
        //}

        public IList<SubActivity> GetSubactiviesByPathIdAndTypes(Int64 pathId, params SubActivityType[] types)
        {
            var temp = (from item in Manager.GetIQ<SubActivity>() where item.Path.Id == pathId where types.Contains(item.ContentType) && item.Deleted == BaseStatusDeleted.None select item);
         
            return temp.ToList();
        }

        public IList<Int64> GetSubactivityIdsByPathAndTypes(Path path, params SubActivityType[] types)
        {
            return (from item in Manager.GetIQ<SubActivity>() where item.Path.Id == path.Id where types.Contains(item.ContentType) && item.Deleted == BaseStatusDeleted.None select item.Id).ToList();
        }

        public IList<Int64> GetSubactivityIdsByPathIdAndTypes(Int64 pathId, params SubActivityType[] types)
        {
            return (from item in Manager.GetIQ<SubActivity>() where item.Path.Id == pathId where types.Contains(item.ContentType) && item.Deleted == BaseStatusDeleted.None select item.Id).ToList();
        }

        public IList<SubActivityLink> GetSubactivyLinksByPathAndTypes(Path path, params SubActivityType[] types)
        {
            return GetSubactivyLinksByPathIdAndTypes(path.Id, types);
        }

        public IList<dtoSubActivityLink> GetDtoSubactivyLinksByPathAndTypes(Path path, params SubActivityType[] types)
        {
            return GetDtoSubactivyLinksByPathIdAndTypes(path.Id, types);
        }

        public IList<dtoSubActivityLink> GetDtoSubactivyLinksByPathIdAndTypes(Int64 pathId, params SubActivityType[] types)
        {

            var temp = GetSubactivityIdsByPathIdAndTypes(pathId, types);
            var all = GetSubactiviesByPathIdAndTypes(pathId, types);

            var query = (from item in Manager.GetIQ<SubActivityLink>() where item.SubActivity != null && temp.Contains(item.SubActivity.Id) && item.Deleted == BaseStatusDeleted.None select new dtoSubActivityLink(item)).ToList();

            var q1 = (from item in query select item.IdSubActivity).ToList();

            var q2 = (from item in temp where !q1.Contains(item) select item).ToList();

            foreach (var item in q2)
            {
                SubActivity s = (from sa in all where sa.Id==item select sa).FirstOrDefault();
                if (s != null)
                {
                    query.Add(new dtoSubActivityLink()
                    {
                        IdSubActivity = item,
                        ContentType = SubActivityLinkType.quiz,
                        IdModule = s.IdModule,
                        IdObject = s.IdObjectLong
                    });
                }
            }

            return query;
        }

        public IList<dtoSubActivityLink> GetDtoSubactivyLinksByPathIdAndSubActIdAndTypes(Int64 pathId, Int64 subactId, params SubActivityType[] types)
        {

            //var IdActivityobjects = GetSubactivityIdsByPathIdAndTypes(pathId, types);
            var objects = GetSubactiviesByPathIdAndTypes(pathId, types);

            var Idobjects = (from item in objects where item.IdObjectLong>0 select item.IdObjectLong).ToList();

            var subactlinks = (from item in Manager.GetIQ<SubActivityLink>() where item.SubActivity != null && item.SubActivity.Id == subactId && item.Deleted == BaseStatusDeleted.None select item).ToList();

            var dtolinks = (from item in subactlinks select new dtoSubActivityLink(item, subactId)).ToList();


            var linkedIdObjects = (from item in subactlinks select item.IdObject).ToList();

            var missingIdObjects = (from item in Idobjects where !linkedIdObjects.Contains(item) select item).ToList();

            foreach (var item in missingIdObjects)
            {
                var itemObject = objects.Where(x => x.IdObjectLong == item).FirstOrDefault();

                dtolinks.Add(new dtoSubActivityLink()
                {
                    IdSubActivity = subactId,
                    ContentType = SubActivityLinkType.quiz,
                    IdModule = itemObject.IdModule,
                    IdObject =itemObject.IdObjectLong                    
                });
            }
            return dtolinks;
        }

        public List<dtoSubActivityLink> GetDtoSubactivityActiveLinks(Int64 idSubActivity)
        {
            var subactlinks = (from sl in Manager.GetIQ<SubActivityLink>()
                               where sl.SubActivity != null && sl.SubActivity.Id == idSubActivity && sl.Deleted == BaseStatusDeleted.None
                               select new dtoSubActivityLink() { 
                                   Id = sl.Id,
                                   IdObject = sl.IdObject,
                                   IdModule = sl.IdModule,
                                   ContentType = sl.ContentType,
                                   Mandatory = sl.Mandatory,
                                   Visible = sl.Visible
            
                               } ).ToList();
            return subactlinks;
        }


        public void SaveSubactivityLinks(IEnumerable<SubActivityLink> items)
        {
            try
            {
                Manager.BeginTransaction();

                foreach (var item in items)
                {
                    Manager.SaveOrUpdate(item);
                }

                Manager.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveDtoSubactivityLinks(IEnumerable<dtoSubActivityLink> items, Int32 idPerson)
        {
            try
            {
                Manager.BeginTransaction();
                foreach (var item in items)
                {

                    SubActivityLink sal = null;
                    if (item.Id == 0)
                    {
                       sal = new SubActivityLink();
                    }
                    else
                    {
                        sal = Manager.Get<SubActivityLink>(item.Id);
                    }


                    sal.ContentType = item.ContentType;
                    sal.CreateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress);

                    sal.Mandatory = item.Mandatory;
                    sal.SubActivity = Manager.Get<SubActivity>(item.IdSubActivity);
                    sal.Visible = item.Visible;
                    sal.IdObject = item.IdObject;
                    sal.IdModule = item.IdModule;


                    Manager.SaveOrUpdate(sal);
                }

                Manager.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IList<SubActivityLink> GetSubactivyLinksByPathIdAndTypes(Int64 pathId, params SubActivityType[] types)
        {

            var temp = GetSubactivityIdsByPathIdAndTypes(pathId, types);

            var query = (from item in Manager.GetIQ<SubActivityLink>() where item.SubActivity!=null && temp.Contains(item.SubActivity.Id) && item.Deleted==BaseStatusDeleted.None  select item).ToList();

            var q1 = (from item in query select item.SubActivity.Id).ToList();

            var q2 = (from item in temp where !q1.Contains(item) select item).ToList();

            foreach (var item in q2)
            {
                SubActivity s = GetSubActivity(item);
                query.Add(new SubActivityLink() { SubActivity = s });
            }

            return query;
        }

        public Int32 GetIdCommunityRole(Int32 idPerson, Int32 idCommunity)
        {
            return Manager.GetIdCommunityRole(idPerson, idCommunity);
        }
        public IList<dtoGenericItem> GetGenericDtoUnitsByPathId(long PathId)
        {
            IList<dtoGenericItem> temp = (from item in Manager.GetIQ<Unit>() where item.ParentPath.Id == PathId && item.Deleted == BaseStatusDeleted.None select new dtoGenericItem() { Name = item.Name, Id = item.Id, Status = item.Status }).ToList<dtoGenericItem>();

            temp = (from item in temp where !CheckStatus(item.Status, Status.Draft) select item).ToList<dtoGenericItem>();

            return temp;
        }

        public RuleActivityCompletion GetActivityRuleById(long ruleId)
        {
            return Manager.Get<RuleActivityCompletion>(ruleId);
        }

        public IList<RuleActivityCompletion> GetActivityRuleByActivityId(long ActivityId)
        {
            return (from item in Manager.GetIQ<RuleActivityCompletion>() where item.DestinationId == ActivityId select item).ToList<RuleActivityCompletion>();
        }

        public bool DeleteActivityRules_byActivityId(long actId)
        {
            IList<RuleActivityCompletion> rules = (from item in Manager.GetIQ<RuleActivityCompletion>()
                                                   where item.SourceId == actId || item.DestinationId == actId
                                                   select item).ToList();
            try
            {
                Manager.BeginTransaction();
                Manager.DeletePhysicalList<RuleActivityCompletion>(rules);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                return false;
            }

            return true;
        }


        public bool ExistRule_byActivityId(long ActivityId)
        {
            return (from item in Manager.GetIQ<RuleActivityCompletion>() where item.SourceId == ActivityId || item.DestinationId == ActivityId select item).Count() > 0;
        }

        public IList<String> GetActivityRuleDescriptionByActivityId(long ActivityId)
        {
            IList<RuleActivityCompletion> list = GetActivityRuleByActivityId(ActivityId);

            return (from item in list select RuleActivityDescription(item)).ToList<string>();
        }

        public IList<dtoRule> GetActivityDtoRuleByActivityId(long ActivityId)
        {
            IList<RuleActivityCompletion> list = GetActivityRuleByActivityId(ActivityId);

            return (from item in list select new dtoRule() { Name = RuleActivityDescription(item), Id = item.Id }).ToList<dtoRule>();
        }

        public String GetActivityName(long activityId)
        {
            return (from item in Manager.GetIQ<Activity>() where item.Id == activityId select item.Name).FirstOrDefault<String>();
        }

        public Boolean SaveOrUpdateRule<T>(EngineRuleCompletion<T> rule, int idCreator, string UserProxyIPaddress, string UserIPaddress) where T : IRuleElement
        {
            Boolean ret = false;

            try
            {
                Manager.BeginTransaction();
                rule.CreateMetaInfo(idCreator,UserIPaddress,UserProxyIPaddress);
                Manager.SaveOrUpdate(rule);
                Manager.Commit();
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                Manager.RollBack();
            }

            return ret;
        }

        public String RuleActivityDescription(RuleActivityCompletion rule)
        {
            String description = "";

            String act1 = GetActivityName(rule.SourceId);
            String act2 = GetActivityName(rule.DestinationId);

            //almeno fino a quando le regole sono solo "in entrata"
            //description = String.Format("\"{0}\" -> \"{1}\" :", act1, act2);
            description = act1 + ":";
            //switch (rule.RangeType)
            //{
            //    case RuleRangeType.GreaterThan:
            //        description += String.Format("Complete > {0}%", rule.MinValue);
            //        break;
            //    case RuleRangeType.LowerThan:
            //        description += String.Format("Complete < {0}%", rule.MaxValue);
            //        break;
            //    case RuleRangeType.Between:
            //        description += String.Format("{0}% < Complete < {1}%", rule.MinValue, rule.MaxValue);
            //        break;
            //}            
            if (rule.CompletionMinValue > 0 || rule.CompletionMaxValue < 100)
            {
                description += String.Format("{0}% < Complete < {1}%", rule.CompletionMinValue, rule.CompletionMaxValue) + "   ";
            }
            if (rule.MarkMinValue > 0 || rule.MarkMaxValue < 100)
            {
                description += String.Format("{0}% < Mark < {1}%", rule.MarkMinValue, rule.MarkMaxValue);
            }
            return description;
        }

        public String GetUnitName(long unitId)
        {
            return (from item in Manager.GetIQ<Unit>() where item.Id == unitId select item.Name).FirstOrDefault<String>();
        }

        public String RuleUnitDescription(RuleUnitCompletion rule)
        {
            String description = "";

            String act1 = GetUnitName(rule.SourceId);
            String act2 = GetUnitName(rule.DestinationId);
            //almeno fino a quando le regole sono solo "in entrata"
            //description = String.Format("\"{0}\" -> \"{1}\" :", act1, act2);
            description = act1 + ":";

            //switch (rule.RangeType)
            //{
            //    case RuleRangeType.GreaterThan:
            //        description += String.Format("Complete > {0}%", rule.MinValue);
            //        break;
            //    case RuleRangeType.LowerThan:
            //        description += String.Format("Complete < {0}%", rule.MaxValue);
            //        break;
            //    case RuleRangeType.Between:
            //        description += String.Format("{0}% < Complete < {1}%", rule.MinValue, rule.MaxValue);
            //        break;
            //}

            if (rule.CompletionMinValue > 0 || rule.CompletionMaxValue < 100)
            {
                description += String.Format("{0}% < Complete < {1}%", rule.CompletionMinValue, rule.CompletionMaxValue) + "   ";
            }
            if (rule.MarkMinValue > 0 || rule.MarkMaxValue < 100)
            {
                description += String.Format("{0}% < Mark < {1}%", rule.MarkMinValue, rule.MarkMaxValue);
            }

            return description;
        }

        public RuleUnitCompletion GetUnitRuleById(long ruleId)
        {
            return Manager.Get<RuleUnitCompletion>(ruleId);
        }

        public IList<long> GetActivitiesIdWithRule()
        {
            return (from item in Manager.GetIQ<RuleActivityCompletion>() select item.SourceId).ToList<long>();
        }

        public IList<long> GetUnitsIdWithRule()
        {
            return (from item in Manager.GetIQ<RuleUnitCompletion>() select item.SourceId).ToList<long>();
        }

        public IList<RuleUnitCompletion> GetUnitRuleByActivityId(long UnitId)
        {
            return (from item in Manager.GetIQ<RuleUnitCompletion>() where item.DestinationId == UnitId select item).ToList<RuleUnitCompletion>();
        }

        public IList<String> GetUnitRuleDescriptionByActivityId(long unitId)
        {
            IList<RuleUnitCompletion> list = GetUnitRuleByActivityId(unitId);

            return (from item in list select RuleUnitDescription(item)).ToList<string>();
        }

        public IList<dtoRule> GetUnitDtoRuleByUnitId(long unitId)
        {
            IList<RuleUnitCompletion> list = GetUnitRuleByActivityId(unitId);

            return (from item in list select new dtoRule() { Name = RuleUnitDescription(item), Id = item.Id }).ToList<dtoRule>();
        }

        #region swich/change display order

        public bool IsLastDisplayedActivity(long actId)
        {
            Activity oAct = Manager.Get<Activity>(actId);

            return (from a in Manager.GetIQ<Activity>()
                    where oAct.ParentUnit.Id == a.ParentUnit.Id && a.Deleted == BaseStatusDeleted.None
                    select a.DisplayOrder).Max() == oAct.DisplayOrder;
        }

        public bool IsFirstDisplayedActivity(long actId)
        {
            Activity oAct = Manager.Get<Activity>(actId);

            return (from a in Manager.GetIQ<Activity>()
                    where oAct.ParentUnit.Id == a.ParentUnit.Id && a.Deleted == BaseStatusDeleted.None
                    select a.DisplayOrder).Min() == oAct.DisplayOrder;
        }

        private long GetActivityId_ByListOfSubActivities(IList<long> OrderedSubActivitiesId)
        {
            IList<long> ActivitiesIds = new List<long>();
            foreach (long subActivityId in OrderedSubActivitiesId)
            {
                ActivitiesIds.Add(Manager.Get<SubActivity>(subActivityId).ParentActivity.Id);
            }
            ActivitiesIds = ActivitiesIds.Distinct<long>().ToList();
            if (ActivitiesIds.Count == 1)
            {
                return ActivitiesIds[0];
            }
            else
            {
                return -1;
            }
        }

        private long GetUnitId_ByListOfActivities(IList<long> OrderedActivitiesId)
        {
            IList<long> UnitId = new List<long>();
            foreach (long activityId in OrderedActivitiesId)
            {
                UnitId.Add(Manager.Get<Activity>(activityId).ParentUnit.Id);
            }
            UnitId = UnitId.Distinct<long>().ToList();
            if (UnitId.Count == 1)
            {
                return UnitId[0];
            }
            else
            {
                return -1;
            }
        }

        private long GetPathId_ByListOfUnit(IList<long> OrderedUnitId)
        {
            IList<long> PathId = new List<long>();
            foreach (long unitId in OrderedUnitId)
            {
                PathId.Add(Manager.Get<Unit>(unitId).ParentPath.Id);
            }
            PathId = PathId.Distinct<long>().ToList();
            if (PathId.Count == 1)
            {
                return PathId[0];
            }
            else
            {
                return -1;
            }
        }



        public bool UpdateSubActivityDisplayOrder(IList<long> OrderedSubActivitiesId, int idPerson, int CRoleId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            long ActivityId = GetActivityId_ByListOfSubActivities(OrderedSubActivitiesId);
            RoleEP RoleEp_ByAct = GetUserRole_ByActivity(ActivityId, idPerson, CRoleId);
            if (CheckRoleEp(RoleEp_ByAct, RoleEP.Manager))
            {
                DateTime CurrentTime = DateTime.Now;
                try
                {
                    SubActivity subactivity = null;
                    Manager.BeginTransaction();
                    Int16 DisplayOrder = 1;
                    foreach (var subActId in OrderedSubActivitiesId)
                    {
                        subactivity = Manager.Get<SubActivity>(subActId);
                        if (subactivity!=null)
                        {
                            subactivity.DisplayOrder = DisplayOrder;
                            subactivity.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                            Manager.SaveOrUpdate<SubActivity>(subactivity);
                            DisplayOrder++;
                        }
                    }
                    Manager.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    Debug.Write(ex);
                    return false;
                }
            }
            return false;
        }

        //public bool UpdateSubActivityList(IList<SubActivity> SubActivityList)
        //{
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdateList(SubActivityList);
        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}
        public bool UpdateActivityDisplayOrder(IList<long> OrderedActivitiesId, int idPerson, int CRoleId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            long UnitId = GetUnitId_ByListOfActivities(OrderedActivitiesId);
            RoleEP RoleEp_ByUnit = GetUserRole_ByUnit(UnitId, idPerson, CRoleId);
            if (CheckRoleEp(RoleEp_ByUnit, RoleEP.Manager))
            {
                DateTime CurrentTime = DateTime.Now;
                Activity activity = null;
                try
                {
                    Manager.BeginTransaction();
                    Int16 DisplayOrder = 1;
                    foreach (var actId in OrderedActivitiesId)
                    {
                        activity = Manager.Get<Activity>(actId);
                        if (activity != null)
                        {
                            activity.DisplayOrder = DisplayOrder;
                            activity.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                            Manager.SaveOrUpdate<Activity>(activity);
                            DisplayOrder++;
                        }
                    }
                    Manager.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    Debug.Write(ex);
                    return false;
                }
            }
            return false;
        }

        public bool UpdateUnitDisplayOrder(IList<long> OrderedUnitsId, int idPerson, int CRoleId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            long PathId = GetPathId_ByListOfUnit(OrderedUnitsId);
            RoleEP RoleEp_ByUnit = GetUserRole_ByPath(PathId, idPerson, CRoleId);
            if (CheckRoleEp(RoleEp_ByUnit, RoleEP.Manager))
            {
                DateTime CurrentTime = DateTime.Now;
                Unit unit = null;
                try
                {
                    Manager.BeginTransaction();
                    Int16 DisplayOrder = 1;
                    foreach (var unitId in OrderedUnitsId)
                    {
                        unit = Manager.Get<Unit>(unitId);
                        if (unit != null)
                        {
                            unit.DisplayOrder = DisplayOrder;
                            unit.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                            Manager.SaveOrUpdate<Unit>(unit);
                            DisplayOrder++;
                        }
                    }
                    Manager.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    Debug.Write(ex);
                    return false;
                }
            }
            return false;
        }

        public bool GetIfCanChangePathsDisplayOrder(IList<dtoEPitemList> dtoPaths, int UserId, int CRoleId)
        {
            IList<long> PathsId = (from dto in dtoPaths select dto.Id).ToList();
            return GetIfCanChangePathsDisplayOrder(PathsId, UserId, CRoleId);
        }

        private bool GetIfCanChangePathsDisplayOrder(IList<long> PathsId, int UserId, int CRoleId)
        {
            int CommunityID = 0;
            if (PathsId.Count > 0)
            {
                CommunityID = Manager.Get<Path>(PathsId[0]).Community.Id;
            }
            foreach (long pathId in PathsId)
            {
                if (CommunityID != Manager.Get<Path>(pathId).Community.Id)
                {
                    return false;
                }
                if (!CheckRoleEp(GetUserRole_ByPath(pathId, UserId, CRoleId), Domain.RoleEP.Manager))
                {
                    return false;
                }
            }
            return true;
        }

        public bool UpdatePathDisplayOrder(IList<long> OrderedPathsId, int idPerson, int CRoleId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            if (GetIfCanChangePathsDisplayOrder(OrderedPathsId, idPerson, CRoleId))
            {
                DateTime CurrentTime = DateTime.Now;
                Path oPath;
                try
                {
                    Manager.BeginTransaction();
                    Int16 DisplayOrder = 1;
                    foreach (var pathId in OrderedPathsId)
                    {
                        oPath = Manager.Get<Path>(pathId);
                        if (oPath != null)
                        {
                            oPath.DisplayOrder = DisplayOrder;
                            oPath.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);;
                            Manager.SaveOrUpdate<Path>(oPath);
                            DisplayOrder++;
                        }
                    }
                    Manager.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                    Debug.Write(ex);
                    return false;
                }
            }
            return false;
        }

        public bool MoveSubActivityDisplayOrderAfter(long SubActivityd, int UserId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            SubActivity oSubActivityToMoveDown = Manager.Get<SubActivity>(SubActivityd);
            IList<SubActivity> SubActivityToMoveUp = Manager.GetAll<SubActivity>(subact => subact.ParentActivity.Id == oSubActivityToMoveDown.ParentActivity.Id && subact.Deleted == BaseStatusDeleted.None && subact.DisplayOrder > oSubActivityToMoveDown.DisplayOrder).OrderBy(subact => subact.DisplayOrder).ToList();
            if (SubActivityToMoveUp.Count > 0)
            {
                return SwitchSubActivityDisplayOrder(oSubActivityToMoveDown, SubActivityToMoveUp[0], UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            return true;
        }

        public bool MoveSubActivityDisplayOrderBefore(long SubActivityd, int UserId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            SubActivity oSubActivityToMoveDown = Manager.Get<SubActivity>(SubActivityd);
            IList<SubActivity> SubActivityToMoveUp = Manager.GetAll<SubActivity>(subact => subact.ParentActivity.Id == oSubActivityToMoveDown.ParentActivity.Id && subact.Deleted == BaseStatusDeleted.None && subact.DisplayOrder < oSubActivityToMoveDown.DisplayOrder).OrderByDescending(subact => subact.DisplayOrder).ToList();
            if (SubActivityToMoveUp.Count > 0)
            {
                return SwitchSubActivityDisplayOrder(oSubActivityToMoveDown, SubActivityToMoveUp[0], UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            return true;
        }

        public bool MoveActivityDisplayOrderAfter(long ActivityId, int UserId, int commRoleID, bool isEditablePath, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Activity oActivityToMoveDown = Manager.Get<Activity>(ActivityId);
            Activity ActivityToMoveUp = Manager.GetAll<Activity>(activity => activity.ParentUnit.Id == oActivityToMoveDown.ParentUnit.Id &&
                                                            activity.Deleted == BaseStatusDeleted.None && activity.DisplayOrder > oActivityToMoveDown.DisplayOrder)
                                                            .OrderBy(activity => activity.DisplayOrder).DefaultIfEmpty(null).First();
            if (ActivityToMoveUp != null)
            {
                return SwitchActivityDisplayOrder(oActivityToMoveDown, ActivityToMoveUp, UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            else if (isEditablePath)
            {
                int parentUnitOrder = oActivityToMoveDown.ParentUnit.DisplayOrder;

                Unit newParent = Manager.GetAll<Unit>(unit => oActivityToMoveDown.Path.Id == unit.ParentPath.Id
                                                      && unit.Deleted == BaseStatusDeleted.None
                                                      && unit.DisplayOrder > parentUnitOrder).OrderBy(unit => unit.DisplayOrder)
                                                      .Where(unit => (this.GetUserPermission_ByUnit(unit.Id, UserId, commRoleID).Update || this.GetUserPermission_ByPath(unit.ParentPath.Id, UserId, commRoleID).Update))
                                                      .DefaultIfEmpty(null).First();
                if (newParent != null)
                {
                    try
                    {
                        Manager.BeginTransaction();

                        oActivityToMoveDown.ParentUnit.Weight -= oActivityToMoveDown.Weight;
                        Manager.SaveOrUpdate(oActivityToMoveDown.ParentUnit);

                        oActivityToMoveDown.ParentUnit = newParent;
                        newParent.Weight += oActivityToMoveDown.Weight;

                        oActivityToMoveDown.DisplayOrder = -100;
                        newParent.ActivityList.Insert(0, oActivityToMoveDown);
                        newParent.ActivityList = newParent.ActivityList.OrderBy(act => act.DisplayOrder).ToList();

                        for (int i = 0; i < newParent.ActivityList.Count; i++)
                        {
                            newParent.ActivityList[i].DisplayOrder = (short)(i + 1);
                            Manager.SaveOrUpdate(newParent.ActivityList[i]);
                        }

                        Manager.SaveOrUpdate(newParent);
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        Debug.Write(ex);
                        return false;
                    }
                }


            }
            return true;
        }

        public bool MoveActivityDisplayOrderBefore(long ActivityId, int UserId, int commRoleID, bool isEditablePath, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Activity oActivityToMoveDown = Manager.Get<Activity>(ActivityId);
            Activity ActivityToMoveUp = Manager.GetAll<Activity>(activity => activity.ParentUnit.Id == oActivityToMoveDown.ParentUnit.Id
                && activity.Deleted == BaseStatusDeleted.None && activity.DisplayOrder < oActivityToMoveDown.DisplayOrder).OrderByDescending(activity => activity.DisplayOrder).DefaultIfEmpty(null).
                First();
            if (ActivityToMoveUp != null)
            {
                return SwitchActivityDisplayOrder(oActivityToMoveDown, ActivityToMoveUp, UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            else if (isEditablePath)
            {
                int parentUnitOrder = oActivityToMoveDown.ParentUnit.DisplayOrder;
                Unit newParent = Manager.GetAll<Unit>(unit => oActivityToMoveDown.Path.Id == unit.ParentPath.Id
                                                       && unit.Deleted == BaseStatusDeleted.None && unit.DisplayOrder < parentUnitOrder)
                                                       .Where(unit => (this.GetUserPermission_ByUnit(unit.Id, UserId, commRoleID).Update || this.GetUserPermission_ByPath(unit.ParentPath.Id, UserId, commRoleID).Update))
                                                       .OrderByDescending(unit => unit.DisplayOrder).DefaultIfEmpty(null).First();
                if (newParent != null)
                {
                    try
                    {
                        Manager.BeginTransaction();
                        oActivityToMoveDown.ParentUnit.Weight -= oActivityToMoveDown.Weight;
                        Manager.SaveOrUpdate(oActivityToMoveDown.ParentUnit);

                        oActivityToMoveDown.ParentUnit = newParent;
                        newParent.Weight += oActivityToMoveDown.Weight;
                        oActivityToMoveDown.DisplayOrder = GetNewActivityDisplayOrder(newParent.Id);

                        Manager.SaveOrUpdate(newParent);
                        Manager.SaveOrUpdate<Activity>(oActivityToMoveDown);
                        Manager.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        Debug.Write(ex);
                        return false;
                    }
                }

            }
            return true;
        }

        public bool MoveUnitDisplayOrderAfter(long UnitId, int UserId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Unit oUnitToMoveUp = Manager.Get<Unit>(UnitId);
            IList<Unit> UnitToMoveDown = Manager.GetAll<Unit>(unit => unit.ParentPath.Id == oUnitToMoveUp.ParentPath.Id && unit.Deleted == BaseStatusDeleted.None && unit.DisplayOrder > oUnitToMoveUp.DisplayOrder).OrderBy(unit => unit.DisplayOrder).ToList();
            if (UnitToMoveDown.Count > 0)
            {
                return SwitchUnitDisplayOrder(oUnitToMoveUp, UnitToMoveDown[0], UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            return true;
        }

        public bool MoveUnitDisplayOrderBefore(long UnitId, int UserId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Unit oUnitToMoveDown = Manager.Get<Unit>(UnitId);
            IList<Unit> UnitToMoveUp = Manager.GetAll<Unit>(unit => unit.ParentPath.Id == oUnitToMoveDown.ParentPath.Id && unit.Deleted == BaseStatusDeleted.None && unit.DisplayOrder < oUnitToMoveDown.DisplayOrder).OrderByDescending(unit => unit.DisplayOrder).ToList();
            if (UnitToMoveUp.Count > 0)
            {
                return SwitchUnitDisplayOrder(oUnitToMoveDown, UnitToMoveUp[0], UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            return true;
        }

        public bool MovePathDisplayOrderAfter(long PathId, int UserId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Path oPathToMoveUp = Manager.Get<Path>(PathId);
            IList<Path> PathToMoveDown = Manager.GetAll<Path>(path => path.Community.Id == oPathToMoveUp.Community.Id && path.Deleted == BaseStatusDeleted.None && path.DisplayOrder > oPathToMoveUp.DisplayOrder).OrderBy(p => p.DisplayOrder).ToList();
            if (PathToMoveDown.Count > 0)
            {
                return SwitchPathDisplayOrder(oPathToMoveUp, PathToMoveDown[0], UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            return true;
        }

        public bool MovePathDisplayOrderBefore(long PathId, int UserId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Path oPathToMoveDown = Manager.Get<Path>(PathId);
            IList<Path> PathToMoveUp = Manager.GetAll<Path>(path => path.Community.Id == oPathToMoveDown.Community.Id && path.Deleted == BaseStatusDeleted.None && path.DisplayOrder < oPathToMoveDown.DisplayOrder).OrderByDescending(p => p.DisplayOrder).ToList();
            if (PathToMoveUp.Count > 0)
            {
                return SwitchPathDisplayOrder(oPathToMoveDown, PathToMoveUp[0], UserId, PersonIpAddress, PersonProxyIpAddress);
            }
            return true;
        }

        private bool SwitchSubActivityDisplayOrder(SubActivity oSubActFirst, SubActivity oSubActSecond, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {

            Int16 DisplayOrderFirst = oSubActFirst.DisplayOrder;
            oSubActFirst.DisplayOrder = oSubActSecond.DisplayOrder;
            oSubActSecond.DisplayOrder = DisplayOrderFirst;
            DateTime CurrentTime = DateTime.Now;
            oSubActFirst.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            oSubActSecond.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<SubActivity>(oSubActFirst);
                Manager.SaveOrUpdate<SubActivity>(oSubActSecond);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        private bool SwitchActivityDisplayOrder(Activity oActFirst, Activity oActSecond, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Int16 DisplayOrderFirst = oActFirst.DisplayOrder;
            oActFirst.DisplayOrder = oActSecond.DisplayOrder;
            oActSecond.DisplayOrder = DisplayOrderFirst;
            DateTime CurrentTime = DateTime.Now;
            oActFirst.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            oActSecond.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<Activity>(oActFirst);
                Manager.SaveOrUpdate<Activity>(oActSecond);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        private bool SwitchUnitDisplayOrder(Unit oUnitFirst, Unit oUnitSecond, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Int16 DisplayOrderFirst = oUnitFirst.DisplayOrder;
            oUnitFirst.DisplayOrder = oUnitSecond.DisplayOrder;
            oUnitSecond.DisplayOrder = DisplayOrderFirst;
            DateTime CurrentTime = DateTime.Now;
            oUnitFirst.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            oUnitSecond.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<Unit>(oUnitFirst);
                Manager.SaveOrUpdate<Unit>(oUnitSecond);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        private bool SwitchPathDisplayOrder(Path oPathFirst, Path oPathSecond, int idPerson, String PersonIpAddress, String PersonProxyIpAddress)
        {
            Int16 DisplayOrderFirst = oPathFirst.DisplayOrder;
            oPathFirst.DisplayOrder = oPathSecond.DisplayOrder;
            oPathSecond.DisplayOrder = DisplayOrderFirst;
            DateTime CurrentTime = DateTime.Now;
            oPathFirst.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            oPathSecond.UpdateMetaInfo(idPerson, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            try
            {
                Manager.BeginTransaction();
                Manager.SaveOrUpdate<Path>(oPathFirst);
                Manager.SaveOrUpdate<Path>(oPathSecond);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }




        #endregion

        private Int16 GetNewSubActivityDisplayOrder(long ActivityId)
        {
            try
            {
                Int16 displayOrder = (from subActivity in Manager.GetAll<SubActivity>(act => act.ParentActivity.Id == ActivityId && act.Deleted == BaseStatusDeleted.None).ToList() where !((subActivity.Status & Status.Draft) == Status.Draft) select subActivity.DisplayOrder).Max();
                displayOrder++;
                return displayOrder;
            }
            catch (Exception)
            {
                return 1;

            }
        }
        public SubActivity SaveOrUpdateSubQuiz(Int64 ActivityID, Int64 idQuest, object DestObj, int DestSrvID, int SrcSrvId, int CurrentCommunityID, int CurrentUserId, string ProxyIPadress, string ClientIPadress)
        {
            Activity activity = Manager.Get<Activity>(ActivityID);

            SubActivity oSubQuiz;
            if (activity.isQuiz)
            {
                oSubQuiz = Manager.GetAll<SubActivity>(sub => sub.Deleted == BaseStatusDeleted.None && sub.ParentActivity == activity).FirstOrDefault<SubActivity>();
            }
            else
            {
                oSubQuiz = Manager.GetAll<SubActivity>(sub => sub.Deleted == BaseStatusDeleted.None && sub.ParentActivity == activity && sub.IdModule == DestSrvID && sub.IdObjectLong == idQuest).FirstOrDefault<SubActivity>();
            }


            ModuleLink oModuleLink = null;
            if (oSubQuiz == null)
            {
                oSubQuiz = new SubActivity();
                oSubQuiz.Name = String.Empty;
                oSubQuiz.ContentType = SubActivityType.Quiz;
                oSubQuiz.IdObjectLong = idQuest;
                oSubQuiz.ParentActivity = activity;
                oSubQuiz.Path = activity.Path;
                oSubQuiz.IdModule = DestSrvID;
                oSubQuiz.IdModuleAction = (int)COL_BusinessLogic_v2.UCServices.Services_Questionario.ActionType.CompileExternal;
                oSubQuiz.ContentPermission = (int)COL_BusinessLogic_v2.UCServices.Services_Questionario.Base2Permission.Compila;
                oSubQuiz.CodeModule = COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex;
                oModuleLink = new ModuleLink();
                oModuleLink.Permission = oSubQuiz.ContentPermission;
                oModuleLink.Action = oSubQuiz.IdModuleAction;
                oModuleLink.DestinationItem = ModuleObject.CreateLongObject(idQuest, DestObj, (int)oSubQuiz.ContentType, 0, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex, DestSrvID);
                //oModuleLink.DestinationItem = ModuleObject.CreateLongObject(idQuest, DestObj, (int)oSubQuiz.ContentType, 0, COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex, DestSrvID);
                oModuleLink.EditEnabled = true;
            }
            else
            {
                oModuleLink = oSubQuiz.ModuleLink;
            }

            return SaveOrUpdateSubActivity(oSubQuiz, oModuleLink, SrcSrvId, COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex, ActivityID, CurrentCommunityID, CurrentUserId, ProxyIPadress, ClientIPadress);
        }

        public dtoSubActText GetSubActText(long subActId)
        {
            SubActivity oSubAct = Manager.Get<SubActivity>(subActId);
            return new dtoSubActText()
            {
                CommunityId = oSubAct.Community.Id,
                Description = oSubAct.Description,
                Id = oSubAct.Id,
                Status = oSubAct.Status,
                Weight = oSubAct.Weight
            };

        }

        public SubActivity SaveOrUpdateSubActivityText(dtoSubActText dto, long idActivity, int idCommunity, int idPerson, string UserProxyIPaddress, string UserIPaddress)
        {
            try
            {
                Activity activity = Manager.Get<Activity>(idActivity);
                if (activity == null && dto.Id == 0)
                    return null;
                Manager.BeginTransaction();
                SubActivity subactivity;
                if (dto.Id == 0)
                {
                    subactivity = new SubActivity();
                    subactivity.CreateMetaInfo(idPerson, UC.IpAddress, UC.IpAddress);
                    subactivity.ContentType = SubActivityType.Text;
                    subactivity.Community = Manager.GetLiteCommunity(idCommunity);
                    subactivity.ParentActivity = Manager.Get<Activity>(idActivity);
                    subactivity.Path = Manager.Get<Activity>(idActivity).Path;
                    subactivity.DisplayOrder = GetNewSubActivityDisplayOrder(idActivity);
                    Manager.SaveOrUpdate(subactivity);
                }
                else
                {
                    subactivity = Manager.Get<SubActivity>(dto.Id);
                    subactivity.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress);
                }

                subactivity.Status = dto.Status;
                subactivity.Description = dto.Description;
                subactivity.Weight = dto.Weight;

                Manager.Commit();

                return subactivity;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }

        public SubActivity SaveOrUpdateSubActivityCertificate(dtoSubActivity dto, long idActivity, int idCommunity, int idPerson)
        {
            try
            {
                SubActivity subactivity = null;
                Activity activity = Manager.Get <Activity>(idActivity);
                if (activity == null && dto.Id == 0)
                    return null;
                Manager.BeginTransaction();
                if (dto.Id == 0)
                {
                    subactivity = new SubActivity();
                    subactivity.CreateMetaInfo(idPerson, UC.IpAddress, UC.IpAddress);
                    subactivity.ContentType = SubActivityType.Certificate;
                 
                    subactivity.Community = Manager.GetLiteCommunity(idCommunity);
                    subactivity.ParentActivity = activity;
                    subactivity.Path = activity.Path;
                    subactivity.DisplayOrder = GetNewSubActivityDisplayOrder(idActivity);

                    subactivity.Name = dto.Name;

                    subactivity.IdCertificate = dto.IdCertificate;
                    subactivity.IdCertificateVersion = dto.IdCertificateVersion;
                    subactivity.ActiveOnMinCompletion = dto.ActiveOnMinCompletion;
                    subactivity.ActiveOnMinMark = dto.ActiveOnMinMark;
                    subactivity.ActiveAfterPathEndDate = dto.ActiveAfterPathEndDate;
                    subactivity.UsePathEndDateStatistics = dto.UsePathEndDateStatistics;
                    subactivity.AutoGenerated = dto.AutoGenerated;
                    subactivity.SaveCertificate = dto.SaveCertificate;
                    subactivity.AllowWithEmptyPlaceHolders = dto.AllowWithEmptyPlaceHolders;
                    //oSubAct.IdModule = DestSrvID;
                    //oSubAct.IdModuleAction = (int)COL_BusinessLogic_v2.UCServices.Services_Questionario.ActionType.CompileExternal;

                    //oSubAct.IdObjectLong = idObject;
 
                    Manager.SaveOrUpdate(subactivity);
                }
                else
                {
                    subactivity = Manager.Get<SubActivity>(dto.Id);

                    subactivity.Name = dto.Name;

                    subactivity.IdCertificate = dto.IdCertificate;
                    subactivity.IdCertificateVersion = dto.IdCertificateVersion;
                    subactivity.ActiveOnMinCompletion = dto.ActiveOnMinCompletion;
                    subactivity.ActiveOnMinMark = dto.ActiveOnMinMark;
                    subactivity.ActiveAfterPathEndDate = dto.ActiveAfterPathEndDate;
                    subactivity.UsePathEndDateStatistics = dto.UsePathEndDateStatistics;
                    subactivity.AutoGenerated = dto.AutoGenerated;
                    subactivity.SaveCertificate = dto.SaveCertificate;
                    subactivity.AllowWithEmptyPlaceHolders = dto.AllowWithEmptyPlaceHolders;

                    subactivity.UpdateMetaInfo(idPerson, UC.IpAddress, UC.ProxyIpAddress);
                }

                subactivity.Status = dto.Status;
                subactivity.Description = dto.Description;
                subactivity.Weight = dto.Weight;

                Manager.Commit();

                return subactivity;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }


        public SubActivity SaveOrUpdateSubActivity(SubActivity oSubActivity, ModuleLink oModuleLink, int CurrentModuleID, String CurrentModuleCode, long ParentActivityId, int CommId, int CreatorID, string UserProxyIPaddress, string UserIPaddress)
        {
            try
            {
                Boolean editSubActivity = true;
                Manager.BeginTransaction();
                if (oSubActivity.Id <= 0)
                {
                    oSubActivity.ParentActivity = Manager.Get<Activity>(ParentActivityId);
                    oSubActivity.Path = Manager.Get<Activity>(ParentActivityId).Path;
                    oSubActivity.Community = Manager.GetLiteCommunity(CommId);
                    oSubActivity.DisplayOrder = GetNewSubActivityDisplayOrder(ParentActivityId);
                    oSubActivity.CreateMetaInfo(CreatorID, UserIPaddress, UserProxyIPaddress);
                    editSubActivity = false;
                }
                else
                {
                    oSubActivity.UpdateMetaInfo(CreatorID, UserIPaddress, UserProxyIPaddress);
                    editSubActivity = true;
                }
                Manager.SaveOrUpdate<SubActivity>(oSubActivity);
                Manager.Commit();

                Manager.BeginTransaction();
                //if (oSubActivity.ParentActivity.isQuiz)
                //{
                //    if (!editSubActivity)
                //    {
                //        oModuleLink.SourceItem = ModuleObject.CreateLongObject(oSubActivity.ParentActivity.Id, oSubActivity.ParentActivity, (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity , oSubActivity.Community.Id, CurrentModuleCode, CurrentModuleID);
                //        oModuleLink.CreateMetaInfo(Manager.GetPerson(CreatorID), UserIPaddress, UserProxyIPaddress);
                //    }
                //    else
                //    {
                //        oModuleLink.UpdateMetaInfo(Manager.GetPerson(CreatorID), UserIPaddress, UserProxyIPaddress);
                //    }
                //}
                //else
                //{
                if (!editSubActivity)
                {
                    oModuleLink.SourceItem = ModuleObject.CreateLongObject(oSubActivity.Id, oSubActivity, (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.SubActivity, oSubActivity.Community.Id, CurrentModuleCode, CurrentModuleID);
                    oModuleLink.CreateMetaInfo(Manager.GetPerson(CreatorID), UserIPaddress, UserProxyIPaddress);
                }
                else
                {
                    oModuleLink.UpdateMetaInfo(Manager.GetPerson(CreatorID), UserIPaddress, UserProxyIPaddress);
                }
                //}
                //if (oModuleLink.DestinationItem.ServiceCode == COL_BusinessLogic_v2.UCServices.Services_Questionario.Codex)
                //{
                //    oModuleLink.SourceItem.ObjectTypeID = (int)COL_BusinessLogic_v2.UCServices.Services_EduPath.ObjectType.Activity;
                //    oModuleLink.SourceItem.ObjectLongID = oSubActivity.ParentActivity.Id;
                //}
                Manager.SaveOrUpdate<ModuleLink>(oModuleLink);

                oSubActivity.ModuleLink = oModuleLink;
                Manager.SaveOrUpdate<SubActivity>(oSubActivity);
                Manager.Commit();

                return oSubActivity;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }



        public List<SubActivity> SubActivityAddFiles(Activity activity, lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier identifier,  List<lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem> items, Int32 idPathModule, Int32 idRepositoryModule, Int32 idCreator, String ipAddress, String proxyIpAddress)
        {
            Person person = Manager.GetPerson(idCreator);
            List<SubActivity> aItems = new List<SubActivity>();
            DateTime createdOn = DateTime.Now;
            foreach (lm.Comol.Core.FileRepository.Domain.dtoModuleUploadedItem item in items)
            {
                try
                {
                    SubActivity subActivity = new SubActivity();
                    subActivity.ParentActivity = activity;
                    subActivity.CreateMetaInfo(idCreator, ipAddress, proxyIpAddress, createdOn);
                    subActivity.IdObjectLong = item.ItemAdded.Id;
                    subActivity.IdObjectVersion = 0;
                    subActivity.IdModuleAction = item.Link.Action;
                    subActivity.Description = "";
                    subActivity.Link = "";
                    subActivity.Name = "";
                    subActivity.ContentPermission = item.Link.Permission;
                    subActivity.IdModule = idRepositoryModule;
                    subActivity.CodeModule = lm.Comol.Core.FileRepository.Domain.ModuleRepository.UniqueCode;
                    subActivity.ContentType = SubActivityType.File;
                     if (subActivity.ParentActivity != null)
                    {
                        subActivity.Path = subActivity.ParentActivity.Path;
                        subActivity.DisplayOrder = GetNewSubActivityDisplayOrder(subActivity.ParentActivity.Id);
                    }
                    subActivity.Community = Manager.GetLiteCommunity(identifier.IdCommunity);
                    Manager.BeginTransaction();
                    Manager.SaveOrUpdate<SubActivity>(subActivity);

                    ModuleLink link = new ModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);
                    link.CreateMetaInfo(person, ipAddress, proxyIpAddress, createdOn);
                    link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
                    link.AutoEvaluable = true;
                    link.SourceItem = ModuleObject.CreateLongObject(subActivity.Id, subActivity, (int)ModuleEduPath.ObjectType.SubActivity, identifier.IdCommunity, ModuleEduPath.UniqueCode, idPathModule);
                    Manager.SaveOrUpdate(link);
                    subActivity.ModuleLink = link;
                    if (item.ItemAdded.IsInternal)
                    {
                        if (item.ItemAdded.Module == null)
                            item.ItemAdded.Module = new lm.Comol.Core.FileRepository.Domain.ItemModuleSettings();
                        item.ItemAdded.Module.IdObject = subActivity.Id;
                        //item.ItemAdded.Module.IdObjectType = (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile;
                        Manager.SaveOrUpdate(item.ItemAdded);
                    }
                    Manager.Commit();
                    aItems.Add(subActivity);
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }

                //SubActivity added = Service.SaveOrUpdateSubActivity(subActivity, l, CurrentIdModule, ModuleEduPath.UniqueCode, identifier.IdCommunity, person, UserContext.IpAddress, UserContext.ProxyIpAddress);


                //liteModuleLink link = new liteModuleLink(item.Link.Description, item.Link.Permission, item.Link.Action);
                //link.CreateMetaInfo(person.Id, UC.IpAddress, UC.ProxyIpAddress, date);
                //link.DestinationItem = (ModuleObject)item.Link.ModuleObject;
                //link.AutoEvaluable = false;
                //link.SourceItem = ModuleObject.CreateLongObject(attachment.Id, attachment, (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile, attachment.IdCommunity, ModuleCommunityDiary.UniqueID, ServiceModuleID());
                //Manager.SaveOrUpdate(link);
                //attachment.Link = link;

                //if (item.ItemAdded.IsInternal)
                //{
                //    if (item.ItemAdded.Module == null)
                //        item.ItemAdded.Module = new lm.Comol.Core.FileRepository.Domain.ItemModuleSettings();
                //    item.ItemAdded.Module.IdObject = attachment.Id;
                //    item.ItemAdded.Module.IdObjectType = (int)ModuleCommunityDiary.ObjectType.DiaryItemLinkedFile;
                //    Manager.SaveOrUpdate(item.ItemAdded);
                //}




                //if (added != null)
                //    aItems.Add(added);
            }
            return aItems;
        }
        public SubActivity SaveOrUpdateSubActivity(SubActivity subactivity, ModuleActionLink link, Int32 idModule, String moduleCode, Int32 idCommunity, Person person, String ipAddress, String proxyIpaddress)
        {
            try
            {
                Boolean editSubActivity = true;
                Manager.BeginTransaction();
                if (subactivity.Id <= 0)
                {
                    if (subactivity.ParentActivity != null)
                    {
                        subactivity.Path = subactivity.ParentActivity.Path;
                        subactivity.DisplayOrder = GetNewSubActivityDisplayOrder(subactivity.ParentActivity.Id);
                    }
                    subactivity.Community = Manager.GetLiteCommunity(idCommunity);
                    editSubActivity = false;
                }
                else
                    editSubActivity = true;
                Manager.SaveOrUpdate<SubActivity>(subactivity);
                Manager.Commit();

                Manager.BeginTransaction();

                if (!editSubActivity)
                {
                    ModuleLink mLink = new ModuleLink(link.Description, link.Permission, link.Action);
                    mLink.CreateMetaInfo(person, ipAddress, proxyIpaddress, subactivity.CreatedOn);
                    mLink.DestinationItem = (ModuleObject)link.ModuleObject;
                    mLink.AutoEvaluable = true;
                    mLink.SourceItem = ModuleObject.CreateLongObject(subactivity.Id, subactivity, (Int32)ModuleEduPath.ObjectType.SubActivity, idCommunity, moduleCode, idModule);
                    Manager.SaveOrUpdate(mLink);
                    subactivity.ModuleLink = mLink;
                    Manager.SaveOrUpdate<SubActivity>(subactivity);
                }
                Manager.Commit();

                return subactivity;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return null;
            }
        }

        public Boolean isEditablePath(Int64 pathId, Int64 personId)
        {
            return CheckStatus(GetPath(pathId).Status, Status.Locked) && !ServiceStat.UsersStartedPath(pathId, personId);
        }
        public PathAvailability GetPathAvailability(long idPath, Int32 idPerson)
        {
            PathAvailability result =  PathAvailability.None;
            Path p = GetPath(idPath);
            if (p!=null & p.Id>0){
                if (CheckStatus(p.Status, Status.Locked))
                    result |= PathAvailability.Blocked;
                if (ServiceStat.UsersStartedPath(idPath, idPerson))
                    result |= PathAvailability.WithOtherUserStatistics;
                else if (ServiceStat.UsersStartedPath(idPath, 0))
                    result |= PathAvailability.WithMyStatistics;
                if (result == PathAvailability.None)
                    result = PathAvailability.Available;
            }
            else
                result = PathAvailability.UnknownItem;
            return result;
        }

        public bool isPlayModePath(Int64 pathId)
        {
            return CheckEpType(Manager.Get<Path>(pathId).EPType, EPType.PlayMode);

        }

        #region check

        public bool ItemIsVisible(long itemId, ItemType type)
        {
            switch (type)
            {
                case ItemType.Path:
                    return CheckStatus(Manager.Get<Path>(itemId).Status, Status.NotLocked);

                case ItemType.Unit:
                    Unit oUnit = Manager.Get<Unit>(itemId);
                    if (CheckStatus(oUnit.Status, Status.NotLocked))
                    {
                        return ItemIsVisible(oUnit.ParentPath.Id, ItemType.Path);
                    }
                    break;

                case ItemType.Activity:
                    Activity oAct = Manager.Get<Activity>(itemId);
                    if (CheckStatus(oAct.Status, Status.NotLocked))
                    {
                        return ItemIsVisible(oAct.ParentUnit.Id, ItemType.Unit);
                    }
                    break;

                case ItemType.SubActivity:
                    SubActivity oSub = Manager.Get<SubActivity>(itemId);
                    if (CheckStatus(oSub.Status, Status.NotLocked))
                    {
                        return ItemIsVisible(oSub.ParentActivity.Id, ItemType.Activity);
                    }
                    break;
            }
            return false;
        }

        public bool CheckCommunityId<T>(long itemId, int commId) where T : IEduPathItem
        {
            if (itemId > 0)
            {
                T item = Manager.Get<T>(itemId);
                return CheckCommunityId(item, commId);
            }
            else
            {
                return false;
            }
        }

        public bool CheckCommunityId<T>(T item, int commId) where T : IEduPathItem
        {
            if (item != null)
            {
                return CheckCommunityId(commId) && item.Community != null && item.Community.Id == commId;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCommunityId(int idCommunity)
        {
            return Manager.GetLiteCommunity(idCommunity) != null;
        }

        /// <summary>
        /// verifica la possibilità di creare o modificare una subActivity di tipo Text
        /// (permessi dell'utente, comunità corretta, nessuno ha iniziato il percorso e
        /// in caso di update actId == subact.Parent.ID
        /// </summary>
        /// <param name="subActId"></param>
        /// <param name="actId"></param>
        /// <param name="commId"></param>
        /// <param name="userId"></param>
        /// <param name="userCRoleId"></param>
        /// <returns></returns>
        public bool CheckSubActText(long subActId, long actId, int commId, int userId, int userCRoleId)
        {
            PermissionEP permission_ByAct = GetUserPermission_ByActivity(actId, userId, userCRoleId);


            if (subActId == 0 && permission_ByAct.Create)//new
            {
                Activity oAct = Manager.Get<Activity>(actId);
                return CheckCommunityId<Activity>(oAct, commId) && !ServiceStat.UsersStartedPath(oAct.Path.Id);

            }
            else if (subActId > 0 && permission_ByAct.Update)//update
            {
                SubActivity oSub = Manager.Get<SubActivity>(subActId);

                return oSub.ContentType == SubActivityType.Text && CheckCommunityId<SubActivity>(oSub, commId) &&
                        oSub.ParentActivity.Id == actId && !ServiceStat.UsersStartedPath(oSub.ParentActivity.Path.Id);

            }

            return false;
        }

        #endregion

        public bool AdminCanViewUsersStatRecursive(long itemId, ItemType type, int adminId, int adminCrole)
        {
            switch (type)
            {

                case ItemType.Path:
                    return GetUserPermission_ByPath(itemId, adminId, adminCrole).ViewUserStat;

                case ItemType.Unit:
                    if (GetUserPermission_ByUnit(itemId, adminId, adminCrole).ViewUserStat)
                    {
                        return true;
                    }
                    else
                    { return AdminCanViewUsersStatRecursive(GetPathId_ByUnitId(itemId), ItemType.Path, adminId, adminCrole); }

                case ItemType.Activity:
                    if (GetUserPermission_ByActivity(itemId, adminId, adminCrole).ViewUserStat)
                    {
                        return true;
                    }
                    else
                    { return AdminCanViewUsersStatRecursive(GetUnitId_ByActivityId(itemId), ItemType.Unit, adminId, adminCrole); }


                case ItemType.SubActivity:
                    return AdminCanViewUsersStatRecursive(Manager.Get<SubActivity>(itemId).ParentActivity.Id, ItemType.Activity, adminId, adminCrole);

                default:
                    return false;
            }
        }



        public bool AdminCanUpdate(long itemId, ItemType type, int adminId, int adminCrole)
        {
            switch (type)
            {

                case ItemType.Path:
                    return GetUserPermission_ByPath(itemId, adminId, adminCrole).Update;

                case ItemType.Unit:
                    if (GetUserPermission_ByUnit(itemId, adminId, adminCrole).Update)
                    {
                        return true;
                    }
                    break;

                case ItemType.Activity:
                    if (GetUserPermission_ByActivity(itemId, adminId, adminCrole).Update)
                    {
                        return true;
                    }
                    break;

                case ItemType.SubActivity:
                    return AdminCanUpdate(Manager.Get<SubActivity>(itemId).ParentActivity.Id, ItemType.Activity, adminId, adminCrole);

                default:
                    return false;
            }
            return false;
        }



        /// <summary>
        /// Verifica che la sottoattività sia EvaluableAnalog
        /// e che l'utente disponga i permessi per analizzarla
        /// </summary>
        /// <param name="subActId"></param>
        /// <param name="userId"></param>
        /// <param name="userCrole"></param>
        /// <returns></returns>
        public bool UserCanEvaluateParticipants(long subActId, int userId, int userCrole)
        {
            if (Manager.Get<SubActivity>(subActId).CheckStatus(Status.EvaluableAnalog))
            {
                return SubUserCanEvaluateParticipants(Manager.Get<SubActivity>(subActId).ParentActivity.Id, ItemType.Activity, userId, userCrole);
            }
            return false;
        }

        private bool SubUserCanEvaluateParticipants(long itemId, ItemType type, int userId, int userCrole)
        {
            switch (type)
            {

                case ItemType.Path:
                    return GetUserPermission_ByPath(itemId, userId, userCrole).Evaluate;

                case ItemType.Unit:
                    if (GetUserPermission_ByUnit(itemId, userId, userCrole).Evaluate)
                    {
                        return true;
                    }
                    break;

                case ItemType.Activity:
                    if (GetUserPermission_ByActivity(itemId, userId, userCrole).Evaluate)
                    {
                        return true;
                    }
                    break;

                case ItemType.SubActivity:
                    return SubUserCanEvaluateParticipants(Manager.Get<SubActivity>(itemId).ParentActivity.Id, ItemType.Activity, userId, userCrole);

                default:
                    return false;
            }
            return false;
        }


        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ModuleEduPath.UniqueCode);
        }
        public List<dtoUser> GetUsersByPermission(Int32 idCommunity, long permission)
        {
            List<dtoUser> users = new List<dtoUser>();
            try
            {

                // trovo tutti i permessi definiti nella community
                Int32 idModule = ServiceModuleID();
                List<LazyCommunityModulePermission> permissions = (from p in Manager.GetIQ<LazyCommunityModulePermission>()
                                                                   where p.CommunityID == idCommunity && p.ModuleID == idModule
                                                                   select p).ToList();

                List<Int32> idRoles = permissions.Where(p => PermissionHelper.CheckPermissionSoft(permission, p.PermissionLong)).Select(p => p.RoleID).ToList();
                foreach (Int32 idRole in idRoles)
                {
                    List<Int32> idUsers = (from s in Manager.GetIQ<LazySubscription>() where s.IdRole == idRole && s.IdCommunity == idCommunity select s.IdPerson).ToList();
                    users.AddRange(GetPagedUsers(idUsers));
                }

            }
            catch (Exception ex)
            {

            }
            return users;
        }
        private List<dtoUser> GetPagedUsers(List<Int32> idUsers)
        {
            List<dtoUser> users = new List<dtoUser>();
            try
            {
                int pageIndex = 0;
                int pageSize = 100;
                List<Int32> paged = idUsers.Skip(pageIndex++).Take(pageSize).ToList();
                while (paged.Count > 0)
                {
                    users.AddRange((from p in Manager.GetIQ<litePerson>() where paged.Contains(p.Id) select new dtoUser() { Id = p.Id, Surname = p.Surname, Name = p.Name }).ToList());
                    paged = idUsers.Skip(pageIndex++ * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return users;
        }
        /// <summary>
        /// Controlla che l'item non sia diverso da null e/o draft e/o cancellato 
        /// e verifica che la comunità corrente sia uguale a quella dell'item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="commId"></param>
        /// <returns></returns>
        public bool HasPermessi_ByItem(long itemId, int commId, ItemType type)
        {
            switch (type)
            {
                case ItemType.Path:
                    Path oPath = Manager.Get<Path>(itemId);
                    return isActive<Path>(oPath) && CheckCommunityId<Path>(oPath, commId);

                case ItemType.Unit:
                    Unit oUnit = Manager.Get<Unit>(itemId);
                    return isActive<Unit>(oUnit) && CheckCommunityId<Unit>(oUnit, commId);

                case ItemType.Activity:
                    Activity oAct = Manager.Get<Activity>(itemId);
                    return isActive<Activity>(oAct) && CheckCommunityId<Activity>(oAct, commId);

                case ItemType.SubActivity:
                    SubActivity oSubAct = Manager.Get<SubActivity>(itemId);
                    return isActive<SubActivity>(oSubAct) && CheckCommunityId<SubActivity>(oSubAct, commId);

                default:
                    return false;
            }

        }

        
        /// <summary>
        /// Controlla che l'item non sia diverso da null e/o draft e/o cancellato 
        /// e verifica che la comunità corrente sia uguale a quella dell'item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="commId"></param>
        /// <returns></returns>
        public bool HasPermessi_ByItem<T>(long itemId, int commId) where T : DomainBaseMetaInfoStatus, IEduPathItem
        {
            T oItem = Manager.Get<T>(itemId);
            return isActive<T>(oItem) && CheckCommunityId<T>(oItem, commId);
        }

        /// <summary>
        /// Controlla che l'item non sia diverso da null e/o draft e/o cancellato 
        /// e verifica che la comunità corrente sia uguale a quella dell'item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="commId"></param>
        /// <returns></returns>
        public bool HasPermessi_ByItem<T>(T Item, int commId) where T : DomainBaseMetaInfoStatus, IEduPathItem
        {
            return isActive<T>(Item) && CheckCommunityId<T>(Item, commId);
        }

        public Boolean IsServiceActive(String code, Int32 idCommunity)
        {
            return Manager.IsModuleActive(code, idCommunity);
        }
        public lm.Comol.Core.DomainModel.ModuleStatus GetEdupathServiceStatus(Int32 idCommunity=0)
        {
            ModuleStatus status =  Manager.GetModuleStatus(ModuleEduPath.UniqueCode, idCommunity);
            switch (status)
            { 
                case ModuleStatus.DisableForCommunity:
                case ModuleStatus.DisableForSystem:
                    Int32 idType = Manager.GetIdProfileType(UC.CurrentUserID);
                    switch (idType)
                    {
                        case (Int32)UserTypeStandard.SysAdmin:
                        case (Int32)UserTypeStandard.Administrator:
                            status = (status == ModuleStatus.DisableForCommunity) ? ModuleStatus.DisableForCommunityAvailableForAdmin : ModuleStatus.DisableForSystemAvailableForAdmin;
                            break;
                        case (Int32)UserTypeStandard.Administrative:
                            if (idCommunity > 0)
                            {
                                List<Int32> idOrganizations = ServiceCommunityManagement.GetAvailableIdOrganizations(UC.CurrentUserID, SearchCommunityFor.SystemManagement);
                                if (idOrganizations.Contains(Manager.GetIdOrganizationFromCommunity(idCommunity)))
                                    status = (status == ModuleStatus.DisableForCommunity) ? ModuleStatus.DisableForCommunityAvailableForAdmin : ModuleStatus.DisableForSystemAvailableForAdmin;
                                else
                                {
                                    List<Int32> cOrganizations = ServiceDashboard.GetAllAvailableOrganizations(idCommunity);
                                    if (cOrganizations.Count > 1 && idOrganizations.Any(i=> cOrganizations.Contains(i)))
                                        status = (status == ModuleStatus.DisableForCommunity) ? ModuleStatus.DisableForCommunityAvailableForAdmin : ModuleStatus.DisableForSystemAvailableForAdmin;
                                }
                            }
                            break;
                        default:
                            if (idCommunity > 0)
                            {
                                lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement mCommunity = new lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement(Manager.GetModulePermission(UC.CurrentUserID, idCommunity, lm.Comol.Core.DomainModel.Domain.ModuleCommunityManagement.UniqueID));
                                if (status == ModuleStatus.DisableForCommunity && (mCommunity.Manage || mCommunity.Administration))
                                    status = ModuleStatus.DisableForCommunityAvailableForAdmin;
                            }
                            break;
                    }
                    break;
            }
            return status;
        }
        #region check flag enum
        /// <summary>
        /// Controlla che l'item non sia diverso da null e/o draft e/o cancellato
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool isActive<T>(T item) where T : DomainBaseMetaInfoStatus
        {
            return item != null && !CheckStatus(item.Status, Status.Draft) && item.Deleted == BaseStatusDeleted.None;
        }

        public bool CheckStatusAssignment(StatusAssignment Actual, StatusAssignment Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public bool CheckStatus(Status Actual, Status Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public bool CheckRoleEp(RoleEP Actual, RoleEP Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public bool CheckDeleted(BaseStatusDeleted Actual, BaseStatusDeleted Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public bool CheckEpType(EPType Actual, EPType Expected)
        {

            return (Actual & Expected) == Expected;
        }

        public bool CheckRoleEpNotStrict(RoleEP Actual, RoleEP Expected)
        {
            return (Actual & Expected) > 0;
        }

        #endregion

        public String GetTime(Int16 totMin)
        {
            int h = totMin / 60;
            int min = totMin % 60;
            return h + "h " + (min < 10 ? "0" : "") + min + "m";
        }
        public String GetTime(long totMin)
        {
            int h = (int)totMin / 60;
            int min = (int)totMin % 60;
            return h + "h " + (min < 10 ? "0" : "") + min + "m";
        }
        public String GetTime(TimeSpan time)
        {

            return time.Hours + "h " + (time.Minutes < 10 ? "0" : "") + time.Minutes + "m";
        }

        public String GetTime(Int32 hour, Int32 min)
        {

            return hour + "h " + (min < 10 ? "0" : "") + min + "m";
        }

        public int GetMin(Int16 totMin)
        {
            return totMin % 60;
        }

        public int GetHour(Int16 totMin)
        {
            return totMin / 60;
        }

        public Int16 ConvertTime(Int16 hours, Int16 mins)
        {
            return (Int16)(hours * 60 + mins);
        }
        public String ConvertTimeFromLong(long time)
        {
            return GetTime(new TimeSpan((int)Math.Floor((double)time / 60),(int)Math.Floor((double)time% 60),0));
        }
        public String GetMinTime(Int16 totMin, Int64 minCompletion)
        {
            return GetTime((Int16)(totMin * minCompletion / 100));

        }
        public void UpdateWeight(Activity currentActivity, Int64 newWeight)
        {
            Int16 diff = (Int16)(newWeight - currentActivity.Weight);
            currentActivity.ParentUnit.Weight += diff;
            currentActivity.ParentUnit.ParentPath.WeightAuto += diff;
        }

        public DateTime? GetEpEndDate(long pathId)
        {
            return Manager.Get<Path>(pathId).EndDate;

            //RIATTIVARE 
            //dtoEndTime dtoRet = new dtoEndTime()
            //{
            //    CertifiedTime=ServiceStat.GetLastCertifiedDate(pathId,userId),
            //    EndDateStr = oPath.EndDate != null ? GetDate((DateTime)oPath.EndDate) : "",
            //    OverEndDateStr = oPath.EndDate != null && oPath.EndDateOverflow != null ? GetTime((DateTime)oPath.EndDateOverflow - (DateTime)oPath.EndDate) : "",
            //    EndDate = oPath.EndDate
            //};

            //return dtoRet;
        }
        public DateTime? GetEpEndDate(long pathId, Int32 idUser)
        {
            return GetEpEndDate(Manager.Get<Path>(pathId),idUser);
        }
        public DateTime? GetEpEndDate(Path path, Int32 idUser)
        {
            if (path == null)
                return null;
            else
                return (path.FloatingDeadlines) ? path.EndDate : path.EndDate;
        }
        #region get MinCompletion (select between item min completion and personal min completion)->>PER RENDERE ATTIVA LA MIN COMPLETION PERSONALIZZATA DESELEZIONARE IL CODICE COMMENTATO
        public Int64 PathMinCompletion(long pathId, int userId)
        {
            int crole = 0;// Manager.GetActiveSubscriptionRoleId(userId, Manager.Get<Path>(pathId).Community.Id);//recupera dati x VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            return PathMinCompletion(pathId, userId, crole);
        }
        public Int64 PathMinCompletion(long pathId, int userId, int croleId)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //Int16 personalMinCompletion = ServiceAss.GetPersonalPathMinCompletion(pathId, userId, croleId);
            //if (personalMinCompletion != -1)
            //{
            //    return personalMinCompletion;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI
            return Manager.Get<Path>(pathId).MinCompletion;
        }

        public Int64 UnitMinCompletion(long unitId, int userId)
        {
            int crole = 0;// Manager.GetActiveSubscriptionRoleId(userId, Manager.Get<Unit>(unitId).Community.Id);//recupera dati x VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            return UnitMinCompletion(unitId, userId, crole);
        }

        public Int64 UnitMinCompletion(long unitId, int userId, int croleId)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //Int16 personalMinCompletion = ServiceAss.GetPersonalUnitMinCompletion(unitId, userId, croleId);
            //if (personalMinCompletion != -1)
            //{
            //    return personalMinCompletion;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI
            return Manager.Get<Unit>(unitId).MinCompletion;
        }

        public Int64 ActivityMinCompletion(long activityId, int userId)
        {
            int crole = 0;//Manager.GetActiveSubscriptionRoleId(userId, Manager.Get<Activity>(activityId).Community.Id);//recupera dati x VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            return ActivityMinCompletion(activityId, userId, crole);
        }

        public Int64 ActivityMinCompletion(long activityId, int userId, int croleId)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //Int16 personalMinCompletion = ServiceAss.GetPersonalActivityMinCompletion(activityId, userId, croleId);
            //if (personalMinCompletion != -1)
            //{
            //    return personalMinCompletion;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI
            return Manager.Get<Activity>(activityId).MinCompletion;
        }
        #endregion
        #region Is MandatoryForParticipant (select between item mandatory status e personal mandatory status)

        /// <summary>
        /// CheckStatus (no more mandatory ad personam)
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns></returns>
        public bool UnitIsMandatoryForParticipant(long unitId, int userId, int croleId)
        {
            return CheckStatus(UnitStatus(unitId, userId, croleId, false), Status.Mandatory);
        }

        public IList<Int32> GetActiveRolesId(Int32 communityId)
        {
            return (from item in Manager.GetIQ<LazySubscription>() where item.IdCommunity == communityId && item.IdRole > 0 select item.IdRole).ToList();
        }

        public IList<Entity.Role> GetActiveRoles(Int32 communityId, Int32 languageId)
        {
            //Dim l As New List(Of Comol.Entity.Role)

            //l = COL_TipoRuolo.List(LinguaID)

            IList<Int32> RolesId = GetActiveRolesId(communityId);

            IList<Entity.Role> l = COL_TipoRuolo.List(languageId).Where(x => RolesId.Contains(x.ID)).ToList();

            return l;
        }

        /// <summary>
        /// CheckStatus (no more mandatory ad personam)
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns></returns>
        public bool ActivityIsMandatoryForParticipant(long activityId, int userId, int croleId)
        {
            return CheckStatus(ActivityStatus(activityId, userId, croleId, false), Status.Mandatory);
        }

        /// <summary>
        /// CheckStatus (no more mandatory ad personam)
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns></returns>
        public bool SubActivityIsMandatoryForParticipant(long subActivityId, int userId, int croleId)
        {
            return CheckStatus(SubActivityStatus(subActivityId, userId, croleId, false), Status.Mandatory);
        }
        #endregion

        #region Is VisibleForParticipant (select between item mandatory status e personal mandatory status

        //public bool PathIsVisibleForParticipant(long pathId, int userId, int croleId)
        //{
        //    return CheckStatus(PathStatus(pathId, userId, croleId, false), Status.NotLocked);
        //}
        /// <summary>
        /// CheckStatus (no more mandatory ad personam)
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns></returns>
        public bool UnitIsVisibleForParticipant(long unitId, int userId, int croleId)
        {
            return CheckStatus(UnitStatus(unitId, userId, croleId, false), Status.NotLocked);
        }

        public bool ActivityIsVisibleForParticipant(long activityId, int userId, int croleId)
        {
            Activity cAct = GetActivity(activityId);
            dtoActivityUser dtoAct = new dtoActivityUser();
            dtoAct.Id = activityId;
            //            For Each rule As RuleActivityCompletion In rulesRelatedToAct //or all is indi
            //                engine.AddRule(rule)
            //Next
            IList<dtoActivityUser> acts = GetFreeActivitiesByPathId(cAct.Path.Id, userId);

            IList<RuleActivityCompletion> ruleList = MergeActivityRulesWithUserCompletion(cAct.Path.Id, userId);
            RuleEngine<dtoActivityUser> rEngine = new RuleEngine<dtoActivityUser>();

            foreach (RuleActivityCompletion rule in ruleList)
            { rEngine.AddRule(rule); }

            //IList<RuleActivityCompletion> ruleList = GetActivityRulesByActivityId(activityId);
            //foreach (RuleActivityCompletion oRule in ruleList)
            //{
            //    rEngine.AddRule(oRule);
            //}
            return CheckStatus(ActivityStatus(activityId, userId, croleId, false), Status.NotLocked) && rEngine.ExecuteStep(dtoAct).isValid;
        }

        public bool SubActivityIsVisibleForParticipant(long subActivityId, int userId, int croleId)
        {
            return CheckStatus(SubActivityStatus(subActivityId, userId, croleId, false), Status.NotLocked);
        }
        #endregion

        #region Get PERSONAL Item Status ->>PER RENDERE ATTIVO LO STATUS PERSONALIZZATO DESELEZIONARE IL CODICE COMMENTATO E CANCELLARE IL RETURN FINALE

        public Status PathStatus(long pathId, int userId, int croleId, bool isManageMode)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //if (!isManageMode)
            //{
            //    Status personalStatus = ServiceAss.GetPersonalPathStatus(pathId, userId, croleId);

            //    if (personalStatus > Status.None)
            //    {
            //        return personalStatus;
            //    }
            //    else
            //    {
            //        return Manager.Get<Path>(pathId).Status;
            //    }
            //}
            //else
            //{
            //    return Manager.Get<Path>(pathId).Status;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI
            return Manager.Get<Path>(pathId).Status;//DA CANCELLARE SE SI RIABILITA IL CODICE SOPRA
        }

        public Status UnitStatus(long unitId, int userId, int croleId, bool isManageMode)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //if (!isManageMode)
            //{
            //    Status personalStatus = ServiceAss.GetPersonalUnitStatus(unitId, userId, croleId);

            //    if (personalStatus > Status.None)
            //    {
            //        return personalStatus;
            //    }
            //    else
            //    {
            //        return Manager.Get<Unit>(unitId).Status;
            //    }
            //}
            //else
            //{
            //    return Manager.Get<Unit>(unitId).Status;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI
            return Manager.Get<Unit>(unitId).Status;//DA CANCELLARE SE SI RIABILITA IL CODICE SOPRA
        }

        public Status ActivityStatus(long activityId, int userId, int croleId, bool isManageMode)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //if (!isManageMode)
            //{
            //    Status personalStatus = ServiceAss.GetPersonalActivityStatus(activityId, userId, croleId);

            //    if (personalStatus > Status.None)
            //    {
            //        return personalStatus;
            //    }
            //    else
            //    {
            //        return Manager.Get<Activity>(activityId).Status;
            //    }
            //}
            //else
            //{
            //    return Manager.Get<Activity>(activityId).Status;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI

            return Manager.Get<Activity>(activityId).Status;//DA CANCELLARE SE SI RIABILITA IL CODICE SOPRA
        }

        public Status SubActivityStatus(long subActivityId, int userId, int croleId, bool isManageMode)
        {
            //VERIFICA CAMPI PERSONALIZZATI SE SI ATTIVA
            //if (!isManageMode)
            //{
            //    Status personalStatus = ServiceAss.GetPersonalSubActivityStatus(subActivityId, userId, croleId);

            //    if (personalStatus > Status.None)
            //    {
            //        return personalStatus;
            //    }
            //    else
            //    {
            //        return Manager.Get<SubActivity>(subActivityId).Status;
            //    }
            //}
            //else
            //{
            //    return Manager.Get<SubActivity>(subActivityId).Status;
            //}
            //FINE PARTE CAMPI PERSONALIZZATI
            return Manager.Get<SubActivity>(subActivityId).Status; //DA CANCELLARE SE SI RIABILITA IL CODICE SOPRA
        }

        #endregion

        #region get if modify statistics when I change min completion ->FUNZIONI TUTTE COMMENTATE ATTUALMENTE NON IN USO

        //public bool  CheckActivityStats_byCompletion(long activityId, Int16 OldMinCompletion, Int16 NewMinCompletion)
        //{
        //    IList<int> PersonIdWithPersonalMinCompletion = ServiceAss.GetPersonIdWithPersonalActivityMinCompletion(activityId, Manager.Get<Activity>(activityId).Community.Id);

        //    int NotPersonallyStat;
        //    if (OldMinCompletion < NewMinCompletion)
        //    {
        //        NotPersonallyStat = ServiceStat.GetActivityStatisticNotPersonalized(activityId, PersonIdWithPersonalMinCompletion, OldMinCompletion, NewMinCompletion).Count;
        //    }
        //    else
        //    {
        //        NotPersonallyStat = ServiceStat.GetActivityStatisticNotPersonalized(activityId, PersonIdWithPersonalMinCompletion, NewMinCompletion, OldMinCompletion).Count;
        //    }
        //    return NotPersonallyStat > 0;
        //}

        //public bool CheckUnitStats_byCompletion(long UnitId, Int16 OldMinCompletion, Int16 NewMinCompletion)
        //{
        //    IList<int> PersonIdWithPersonalMinCompletion = ServiceAss.GetPersonIdWithPersonalUnitMinCompletion(UnitId, Manager.Get<Unit>(UnitId).Community.Id);

        //    int NotPersonallyStat;
        //    if (OldMinCompletion < NewMinCompletion)
        //    {
        //        NotPersonallyStat = ServiceStat.GetUnitStatisticNotPersonalized(UnitId, PersonIdWithPersonalMinCompletion, OldMinCompletion, NewMinCompletion).Count;
        //    }
        //    else
        //    {
        //        NotPersonallyStat = ServiceStat.GetUnitStatisticNotPersonalized(UnitId, PersonIdWithPersonalMinCompletion, NewMinCompletion, OldMinCompletion).Count;
        //    }
        //    return NotPersonallyStat > 0;
        //}

        //public bool CheckPathStats_byCompletion(long PathId, Int16 OldMinCompletion, Int16 NewMinCompletion)
        //{
        //    IList<int> PersonIdWithPersonalMinCompletion = ServiceAss.GetPersonIdWithPersonalPathMinCompletion(PathId, Manager.Get<Path>(PathId).Community.Id);

        //    int NotPersonallyStat;
        //    if (OldMinCompletion < NewMinCompletion)
        //    {
        //        NotPersonallyStat = ServiceStat.GetPathStatisticNotPersonalized(PathId, PersonIdWithPersonalMinCompletion, OldMinCompletion, NewMinCompletion).Count;
        //    }
        //    else
        //    {
        //        NotPersonallyStat = ServiceStat.GetPathStatisticNotPersonalized(PathId, PersonIdWithPersonalMinCompletion, NewMinCompletion, OldMinCompletion).Count;
        //    }
        //    return NotPersonallyStat > 0;
        //}
        #endregion

        #region "Rules"
        public IList<IRuleElement> GetPathUserCompletionByPathId(Int64 PathId, Int32 UserId)
        {
            // PathStatistic pathStat = ServiceStat.GetPathStatistic(PathId, UserId, timeToDelete);

            IList<IRuleElement> retVal = null;

            IList<UnitStatistic> unitStats = ServiceStat.GetActiveUnitStat_ByPathId_Insert(PathId, UserId);
            // int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, pathStat.Path.Community.Id);
            retVal = (from item in unitStats
                      select (IRuleElement)new dtoUnitUser()
                     {
                         Name = item.Unit.Name,
                         Id = item.Unit.Id,
                         Status = UnitStatus(item.Unit.Id, UserId, 0, false),
                         MinCompletion = UnitMinCompletion(item.Unit.Id, UserId, 0),
                         UserCompletion = item.Completion
                     }).ToList();

            IList<ActivityStatistic> actsStat = ServiceStat.GetActiveActStat_ByPathId_Insert(PathId, UserId);
            retVal = retVal.Concat((from item in actsStat
                                    select (IRuleElement)new dtoActivityUser()
                                        {
                                            Name = item.Activity.Name,
                                            Id = item.Activity.Id,
                                            Status = ActivityStatus(item.Activity.Id, UserId, 0, false),
                                            MinCompletion = ActivityMinCompletion(item.Activity.Id, UserId, 0),
                                            UserCompletion = item.Completion
                                        })).ToList();

            return retVal;

        }

        public IList<dtoActivityUser> GetActivitiesUserCompletionByPathId(long PathId, Int32 UserId)
        {
            IList<ActivityStatistic> actsStat = ServiceStat.GetActiveActStat_ByPathId_Insert(PathId, UserId);

            if (actsStat.Count > 0)
            {
                //int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, pathStat.Path.Community.Id);

                IList<dtoActivityUser> retVal = (from item in actsStat
                                                 select new dtoActivityUser()
                                                     {
                                                         Name = item.Activity.Name,
                                                         Id = item.Activity.Id,
                                                         Status = ActivityStatus(item.Activity.Id, UserId, 0, false),
                                                         MinCompletion = ActivityMinCompletion(item.Activity.Id, UserId, 0),
                                                         UserMark = item.Mark,
                                                         UserCompletion = ServiceStat.CheckStatusStatistic(item.Status, Domain.StatusStatistic.Completed) ? 100 : ServiceStat.GetActivityCompletion_Insert(item.Activity.Id, UserId)
                                                     }).ToList<dtoActivityUser>();

                return retVal;
            }
            else
            {
                return new List<dtoActivityUser>();
            }
        }

        public IList<dtoActivityUser> GetActivitiesUserCompletionByPathId_Validate(dtoEpStructureValidate dtoEp)
        {
            if (dtoEp.Units != null)
            {
                IList<dtoActivityUser> retVal = (from item in dtoEp.Units.Where(x => x.Activities != null).SelectMany(x => x.Activities)
                                                 select new dtoActivityUser()
                                                 {
                                                     Name = item.Name,
                                                     Id = item.Id,
                                                     Status = item.Status,
                                                     MinCompletion = item.MinCompletion,
                                                     UserCompletion = item.Completion,
                                                     UserMark = item.Mark
                                                 }).ToList<dtoActivityUser>();

                return retVal;
            }
            else
            {
                return new List<dtoActivityUser>();
            }
        }

        public IList<dtoActivityUser> GetActivitiesUserCompletionByUnitId(long UnitId, Int32 UserId)
        {

            IList<ActivityStatistic> actsStat = ServiceStat.GetActiveActStat_ByUnitId_Insert(UnitId, UserId);


            if (actsStat.Count > 0)
            {
                // int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, unitStat.Unit.Community.Id);

                IList<dtoActivityUser> retVal = (from item in actsStat
                                                 select new dtoActivityUser()
                                                 {
                                                     Name = item.Activity.Name,
                                                     Id = item.Activity.Id,
                                                     Status = ActivityStatus(item.Activity.Id, UserId, 0, false),
                                                     MinCompletion = ActivityMinCompletion(item.Activity.Id, UserId, 0),
                                                     UserCompletion = item.Completion,
                                                     UserMark = item.Mark
                                                 }).ToList<dtoActivityUser>();

                return retVal;
            }
            else
            {
                return new List<dtoActivityUser>();
            }
        }

        public IList<dtoUnitUser> GetUnitsUserCompletionByPathId(long PathId, Int32 UserId)
        {
            IList<UnitStatistic> unitStats = ServiceStat.GetActiveUnitStat_ByPathId_Insert(PathId, UserId);


            if (unitStats.Count > 0)
            {
                //  int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, pathStat.Path.Community.Id);

                IList<dtoUnitUser> retVal = (from item in unitStats
                                             select new dtoUnitUser()
                                            {
                                                Name = item.Unit.Name,
                                                Id = item.Unit.Id,
                                                Status = UnitStatus(item.Unit.Id, UserId, 0, false),
                                                MinCompletion = UnitMinCompletion(item.Unit.Id, UserId, 0),
                                                UserCompletion = item.Completion,
                                                UserMark = item.Mark
                                            }).ToList<dtoUnitUser>();

                return retVal;
            }
            else
            {
                return new List<dtoUnitUser>();
            }
        }

        public IList<dtoUnitUser> GetUnitsUserCompletionByPathId_Validate(dtoEpStructureValidate dtoEp)
        {

            if (dtoEp != null && dtoEp.Units != null)
            {

                IList<dtoUnitUser> retVal = (from item in dtoEp.Units
                                             select new dtoUnitUser()
                                             {
                                                 Name = item.Name,
                                                 Id = item.Id,
                                                 Status = item.Status,
                                                 MinCompletion = item.MinCompletion,
                                                 UserCompletion = item.Completion,
                                                 UserMark = item.Mark
                                             }).ToList<dtoUnitUser>();

                return retVal;
            }
            else
            {
                return new List<dtoUnitUser>();
            }
        }

        public dtoUnitUser GetUnitUserCompletionByUnitId(long UnitId, Int32 UserId)
        {
            UnitStatistic unitStat = ServiceStat.GetUnitStatistic(UnitId, UserId, DateTime.Now.AddSeconds(1));
            if (unitStat != null)
            {
                int CRoleId = Manager.GetActiveSubscriptionIdRole(UserId, unitStat.Unit.Community.Id);
                return new dtoUnitUser()
                {
                    Name = unitStat.Unit.Name,
                    Id = unitStat.Unit.Id,
                    Status = UnitStatus(unitStat.Unit.Id, UserId, CRoleId, false),
                    MinCompletion = UnitMinCompletion(unitStat.Unit.Id, UserId, CRoleId),
                    UserCompletion = unitStat.Completion
                };
            }
            else
            {
                return null;
            }
        }

        //public dtoActivityUser GetActivityUserCompletion(long PathId, long ActivityId, Int32 UserId)
        //{
        //    IList<ActivityStatistic> actsStat = ServiceStat.GetActiveActStat_ByPathId_Insert(PathId, UserId);
        //    if (actsStat.Count>0)
        //    {
        //       // int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, pathStat.Path.Community.Id);
        //        dtoActivityUser retVal = (from item in actsStat
        //                                  where item.Id == ActivityId
        //                                  select new dtoActivityUser()
        //                                  {
        //                                      Name = item.Activity.Name,
        //                                      Id = item.Activity.Id,
        //                                      Status = ActivityStatus(item.Activity.Id, UserId, 0, false),
        //                                      MinCompletion = ActivityMinCompletion(item.Activity.Id, UserId, 0),
        //                                      UserCompletion = item.Completion
        //                                  }).FirstOrDefault<dtoActivityUser>();

        //        return retVal;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public IList<dtoActivityUser> GetActivityUserCompletion_Validate(dtoEpStructureValidate dtoEp, IEnumerable<long> ActivityId)
        {

            if (dtoEp != null && dtoEp.Units != null)
            {
                IList<dtoActivityUser> retVal = (from item in dtoEp.Units.Where(x => x.Activities != null).SelectMany(x => x.Activities)
                                                 where ActivityId.Contains(item.Id)
                                                 select new dtoActivityUser()
                                                 {
                                                     Name = item.Name,
                                                     Id = item.Id,
                                                     Status = item.Status,
                                                     MinCompletion = item.MinCompletion,
                                                     UserCompletion = item.Completion,
                                                     UserMark = item.Mark
                                                 }).ToList<dtoActivityUser>();

                return retVal;
            }
            else
                return new List<dtoActivityUser>();
        }


        public IList<dtoActivityUser> GetActivityUserCompletion(long PathId, IEnumerable<long> ActivityId, Int32 UserId)
        {

            IList<ActivityStatistic> actsStat = ServiceStat.GetActiveActStat_ByPathId_Insert(PathId, UserId);

            if (actsStat.Count > 0)
            {
                //  int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, pathStat.Path.Community.Id);
                IList<dtoActivityUser> retVal = (from item in actsStat
                                                 where ActivityId.Contains(item.Activity.Id)
                                                 select new dtoActivityUser()
                                                 {
                                                     Name = item.Activity.Name,
                                                     Id = item.Activity.Id,
                                                     Status = ActivityStatus(item.Activity.Id, UserId, 0, false),
                                                     MinCompletion = ActivityMinCompletion(item.Activity.Id, UserId, 0),
                                                     UserCompletion = (ServiceStat.CheckStatusStatistic(item.Status, Domain.StatusStatistic.Completed) ? 100 : item.Completion),
                                                     UserMark = item.Mark,
                                                 }).ToList<dtoActivityUser>();

                return retVal;
            }
            else
                return new List<dtoActivityUser>();
        }


        public IList<dtoUnitUser> GetUnitUserCompletion(long PathId, IEnumerable<long> UnitId, Int32 UserId)
        {
            IList<UnitStatistic> unitStats = ServiceStat.GetActiveUnitStat_ByPathId_Insert(PathId, UserId);
            if (unitStats.Count > 0)
            {
                // int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, pathStat.Path.Community.Id);

                IList<dtoUnitUser> retVal = (from item in unitStats
                                             where UnitId.Contains(item.Unit.Id)
                                             select new dtoUnitUser()
                                             {
                                                 Name = item.Unit.Name,
                                                 Id = item.Unit.Id,
                                                 Status = UnitStatus(item.Unit.Id, UserId, 0, false),
                                                 MinCompletion = UnitMinCompletion(item.Unit.Id, UserId, 0),
                                                 UserCompletion = item.Completion
                                             }).ToList<dtoUnitUser>();

                return retVal;
            }
            else
            {
                return new List<dtoUnitUser>();
            }
        }


        public IList<dtoUnitUser> GetUnitUserCompletion_Validate(dtoEpStructureValidate dtoEp, IEnumerable<long> UnitId)
        {

            if (dtoEp != null && dtoEp.Units != null)
            {

                IList<dtoUnitUser> retVal = (from item in dtoEp.Units
                                             where UnitId.Contains(item.Id)
                                             select new dtoUnitUser()
                                             {
                                                 Name = item.Name,
                                                 Id = item.Id,
                                                 Status = item.Status,
                                                 MinCompletion = item.MinCompletion,
                                                 UserCompletion = item.Completion
                                             }).ToList<dtoUnitUser>();

                return retVal;
            }
            else
            {
                return new List<dtoUnitUser>();
            }
        }

        public IList<RuleActivityCompletion> GetActivityRulesByPathId(long PathId)
        {
            IList<RuleActivityCompletion> retVal = null;

            IList<long> activityids = (from item in Manager.GetIQ<Activity>() where item.Path.Id == PathId select item.Id).ToList();

            retVal = Manager.GetAll<RuleActivityCompletion>(x => activityids.Contains(x.SourceId) || activityids.Contains(x.DestinationId));

            return retVal;
        }
        public IList<RuleActivityCompletion> GetActivityRulesByActivityId(long ActivityId)
        {
            IList<RuleActivityCompletion> retVal = null;

            retVal = Manager.GetAll<RuleActivityCompletion>(x => (x.Destination.Id == ActivityId));

            return retVal;
        }
        public IList<RuleUnitCompletion> GetUnitRulesByPathId(long PathId)
        {
            IList<RuleUnitCompletion> retVal = null;

            IList<long> unitids = (from item in Manager.GetIQ<Unit>() where item.ParentPath.Id == PathId select item.Id).ToList();

            retVal = Manager.GetAll<RuleUnitCompletion>(x => unitids.Contains(x.SourceId) || unitids.Contains(x.DestinationId));

            return retVal;
        }

        public IList<RuleActivityCompletion> MergeActivityRulesWithUserCompletion(long PathId, Int32 UserId)
        {
            IList<RuleActivityCompletion> retVal = GetActivityRulesByPathId(PathId);

            IList<dtoActivityUser> activitiesUser = GetActivitiesUserCompletionByPathId(PathId, UserId);

            foreach (var rule in retVal)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }

                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return retVal;
        }

        public IList<RuleActivityCompletion> MergeActivityRulesWithUserCompletion_Validate(dtoEpStructureValidate dtoEp)
        {
            IList<RuleActivityCompletion> retVal = GetActivityRulesByPathId(dtoEp.Id);

            IList<dtoActivityUser> activitiesUser = GetActivitiesUserCompletionByPathId_Validate(dtoEp);

            foreach (var rule in retVal)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return retVal;
        }

        public IList<RuleActivityCompletion> MergeActivityRulesWithUserCompletion(long PathId, IList<dtoActivityUser> activitiesUser)
        {
            IList<RuleActivityCompletion> retVal = GetActivityRulesByPathId(PathId);

            foreach (var rule in retVal)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return retVal;
        }

        public IList<RuleUnitCompletion> MergeUnitRulesWithUserCompletion(long PathId, Int32 UserId)
        {
            IList<RuleUnitCompletion> retVal = GetUnitRulesByPathId(PathId);

            IList<dtoUnitUser> activitiesUser = GetUnitsUserCompletionByPathId(PathId, UserId);

            foreach (var rule in retVal)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoUnitUser dtoAct = new dtoUnitUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoUnitUser dtoAct = new dtoUnitUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return retVal;
        }

        public IList<RuleUnitCompletion> MergeUnitRulesWithUserCompletion_Validate(dtoEpStructureValidate dtoEp)
        {
            IList<RuleUnitCompletion> retVal = GetUnitRulesByPathId(dtoEp.Id);

            IList<dtoUnitUser> activitiesUser = GetUnitsUserCompletionByPathId_Validate(dtoEp);

            foreach (var rule in retVal)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoUnitUser dtoAct = new dtoUnitUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoUnitUser dtoAct = new dtoUnitUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return retVal;
        }


        public IList<long> GetFreeActivitiesIdByPathId(long idPath)
        {
            IList<long> retVal = null;

            IList<RuleActivityCompletion> rules = GetActivityRulesByPathId(idPath);
            IList<long> RuledActivity = (from item in rules select item.DestinationId).ToList();


            retVal = (from item in Manager.GetIQ<Activity>() where item.Path.Id == idPath && !RuledActivity.Contains(item.Id) select item.Id).ToList();

            return retVal;
        }

        public IList<long> GetFreeUnitsIdByPathId(long PathId)
        {
            IList<long> retVal = null;

            IList<RuleUnitCompletion> rules = GetUnitRulesByPathId(PathId);
            IList<long> RuledUnit = (from item in rules select item.DestinationId).ToList();


            retVal = (from item in Manager.GetIQ<Unit>() where item.ParentPath.Id == PathId && !RuledUnit.Contains(item.Id) select item.Id).ToList();

            return retVal;
        }

        public IList<dtoActivityUser> GetFreeActivitiesByPathId_Validate(dtoEpStructureValidate dtoEp)
        {
            IList<dtoActivityUser> retVal = null;

            IList<long> ids = GetFreeActivitiesIdByPathId(dtoEp.Id);
            //  int CRoleId = Manager.GetActiveSubscriptionRoleId(UserId, Manager.Get<Path>(PathId).Community.Id);

            retVal = GetActivityUserCompletion_Validate(dtoEp, ids);

            IList<long> idsRetVal = (from item in retVal select item.Id).ToList();

            IList<dtoActivityUser> merging = (from item in Manager.GetIQ<Activity>()
                                              where ids.Contains(item.Id) && !idsRetVal.Contains(item.Id)
                                              select new dtoActivityUser()
                                              {
                                                  Name = item.Name,
                                                  Id = item.Id,
                                                  Status = item.Status,
                                                  MinCompletion = item.MinCompletion,
                                                  UserMark = 0,
                                                  UserCompletion = 0
                                              }
                                           ).ToList();

            retVal = retVal.Concat(merging).ToList();

            return retVal;
        }

        //public Boolean IsFreeActivity(long idPath,long idActivity, Int32 UserId)
        //{
          
        //}

        public IList<dtoActivityUser> GetFreeActivitiesByPathId(long PathId, Int32 UserId)
        {
            IList<dtoActivityUser> retVal = null;

            IList<long> ids = GetFreeActivitiesIdByPathId(PathId);

            int CRoleId = Manager.GetActiveSubscriptionIdRole(UserId, GetPathIdCommunity(PathId));

            retVal = GetActivityUserCompletion(PathId, ids, UserId);

            IList<long> idsRetVal = (from item in retVal select item.Id).ToList();


            IList<dtoActivityUser> merging = (from item in Manager.GetIQ<Activity>()
                                              where ids.Contains(item.Id) && !idsRetVal.Contains(item.Id)
                                              select new dtoActivityUser()
                                              {
                                                  Name = item.Name,
                                                  Id = item.Id,
                                                  //Status = ActivityStatus(item.Id, UserId, CRoleId, false),
                                                  //MinCompletion = GetActivityMinCompletion(item.Id, UserId, CRoleId),
                                                  UserMark = 0,
                                                  UserCompletion = 0
                                              }
                                           ).ToList();

            retVal = retVal.Concat(merging).ToList();

            return retVal;
        }

        public IList<dtoUnitUser> GetFreeUnitsByPathId(long PathId, Int32 UserId)
        {
            IList<dtoUnitUser> retVal = null;
            int CRoleId = Manager.GetActiveSubscriptionIdRole(UserId, GetPathIdCommunity(PathId));
            IList<long> ids = GetFreeUnitsIdByPathId(PathId);

            retVal = GetUnitUserCompletion(PathId, ids, UserId);

            IList<long> idsRetVal = (from item in retVal select item.Id).ToList();

            IList<dtoUnitUser> merging = (from item in Manager.GetIQ<Unit>()
                                          where ids.Contains(item.Id) && !idsRetVal.Contains(item.Id)
                                          select new dtoUnitUser()
                                          {
                                              Name = item.Name,
                                              Id = item.Id,
                                              //Status = UnitStatus(item.Id, UserId, CRoleId, false),
                                              // MinCompletion = 0,
                                              UserMark = 0,
                                              UserCompletion = 0
                                          }
                                           ).ToList();

            foreach (dtoUnitUser item in merging)
            {
                item.Status = UnitStatus(item.Id, UserId, CRoleId, false);
                item.MinCompletion = UnitMinCompletion(item.Id, UserId, CRoleId);
            }

            retVal = retVal.Concat(merging).ToList();

            return retVal;
        }


        public IList<dtoUnitUser> GetFreeUnitsByPathId_Validate(dtoEpStructureValidate dtoEp)
        {
            IList<dtoUnitUser> retVal = null;

            IList<long> ids = GetFreeUnitsIdByPathId(dtoEp.Id);

            retVal = GetUnitUserCompletion_Validate(dtoEp, ids);

            IList<long> idsRetVal = (from item in retVal select item.Id).ToList();

            IList<dtoUnitUser> merging = (from item in Manager.GetIQ<Unit>()
                                          where ids.Contains(item.Id) && !idsRetVal.Contains(item.Id)
                                          select new dtoUnitUser()
                                          {
                                              Name = item.Name,
                                              Id = item.Id,
                                              Status = item.Status,
                                              MinCompletion = item.MinCompletion,
                                              UserMark = 0,
                                              UserCompletion = 0
                                          }
                                           ).ToList();

            retVal = retVal.Concat(merging).ToList();

            return retVal;
        }

        /// <summary>
        /// missing completed Activities
        /// </summary>
        /// <param name="PathId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<long> GetFreeOrUserCompletedActivitiesIdByPathId(long PathId, Int32 UserId)
        {
            IList<long> retVal = GetFreeActivitiesIdByPathId(PathId);

            //

            return retVal;
        }

        /// <summary>
        /// missing completed Units
        /// </summary>
        /// <param name="PathId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<long> GetFreeOrUserCompletedUnitsIdByPathId(long PathId, Int32 UserId)
        {
            IList<long> retVal = GetFreeUnitsIdByPathId(PathId);

            //

            return retVal;
        }

        public IList<RuleActivityCompletion> MergeActivityRulesWithUserCompletion(IList<RuleActivityCompletion> rules, long PathId, Int32 UserId)
        {
            IList<dtoActivityUser> activitiesUser = GetActivitiesUserCompletionByPathId(PathId, UserId);

            foreach (var rule in rules)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return rules;
        }



        public IList<RuleUnitCompletion> MergeUnitRulesWithUserCompletion(IList<RuleUnitCompletion> rules, long PathId, Int32 UserId)
        {

            IList<dtoUnitUser> unitsUser = GetUnitsUserCompletionByPathId(PathId, UserId);

            foreach (var rule in rules)
            {
                rule.Destination = (from item in unitsUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoUnitUser dtoAct = new dtoUnitUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in unitsUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoUnitUser dtoAct = new dtoUnitUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }


            return rules;
        }

        public IList<RuleActivityCompletion> MergeActivityRulesWithUserCompletion(IList<RuleActivityCompletion> rules, IList<dtoActivityUser> activitiesUser)
        {

            foreach (var rule in rules)
            {
                rule.Destination = (from item in activitiesUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.DestinationId;
                    rule.Destination = dtoAct;
                }
                rule.Source = (from item in activitiesUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoActivityUser dtoAct = new dtoActivityUser();
                    dtoAct.Id = rule.SourceId;
                    rule.Source = dtoAct;
                }
            }

            return rules;
        }

        public IList<RuleUnitCompletion> MergeUnitRulesWithUserCompletion(IList<RuleUnitCompletion> rules, IList<dtoUnitUser> unitsUser)
        {

            foreach (var rule in rules)
            {
                rule.Destination = (from item in unitsUser where item.Id == rule.DestinationId select item).FirstOrDefault();
                if (rule.Destination == null)
                {
                    dtoUnitUser dtoUnit = new dtoUnitUser();
                    dtoUnit.Id = rule.DestinationId;
                    rule.Destination = dtoUnit;
                }
                rule.Source = (from item in unitsUser where item.Id == rule.SourceId select item).FirstOrDefault();
                if (rule.Source == null)
                {
                    dtoUnitUser dtoUnit = new dtoUnitUser();
                    dtoUnit.Id = rule.SourceId;
                    rule.Source = dtoUnit;
                }
            }

            return rules;
        }
        #endregion



        #region Validate

        public dtoEpStructureValidate GetEpStructure_toValidate(long PathId, int UserId, int CRoleId)//,  String PersonIpAddress, String PersonProxyIpAddress)
        {
            Path oPath = Manager.Get<Path>(PathId);

            RoleEP RoleEPinPath = GetUserRole_ByPath(PathId, UserId, CRoleId);

            dtoEpStructureValidate dtoEpStructure = new dtoEpStructureValidate()
            {
                Id = PathId,
                Description = oPath.Description,
                Name = oPath.Name,
                IsMooc = oPath.IsMooc,
                MinCompletion = oPath.MinCompletion,
                MinMark = oPath.MinMark,
                Status = oPath.Status,
                StatusStat = StatusStatistic.None,
                Weight = oPath.Weight
            };

            dtoEpStructure.Units = (from unit in oPath.UnitList
                                    where unit.Deleted == BaseStatusDeleted.None && !unit.CheckStatus(Status.Draft) && !unit.CheckStatus(Status.Text)
                                    orderby unit.DisplayOrder
                                    select new dtoUnitStructureValidate()
                                    {
                                        Id = unit.Id,
                                        Name = unit.Name,
                                        Description = unit.Description,
                                        MinCompletion = unit.MinCompletion,
                                        MinMark = unit.MinMark,
                                        Status = unit.Status,
                                        StatusStat = StatusStatistic.None,
                                        Weight = unit.Weight,
                                        Activities = (from act in unit.ActivityList
                                                      where act.Deleted == BaseStatusDeleted.None && !act.CheckStatus(Status.Draft) && !act.CheckStatus(Status.Text)
                                                      orderby act.DisplayOrder
                                                      select new dtoActStructureValidate()
                                                      {
                                                          Id = act.Id,
                                                          Name = act.Name,
                                                          MinCompletion = act.MinCompletion,
                                                          MinMark = act.MinMark,
                                                          StatusStat = StatusStatistic.None,
                                                          Status = act.Status,
                                                          Weight = act.Weight,
                                                          StartDate = act.StartDate,
                                                          EndDate = act.EndDate
                                                      }).ToList()
                                    }).ToList();

            return dtoEpStructure;
        }


        #endregion
        public bool ExecuteSubActivityInternal(long subActId, int userID, int croleId, string UserProxyIPaddress, string UserIPaddress)
        {
            Boolean isPassed = false;
            Int16 Mark = 0;

            if (CheckStatus(Manager.Get<SubActivity>(subActId).Status, Status.EvaluableDigital))
            {
                isPassed = true;
                Mark = 100;
            }

            try
            {
                Manager.BeginTransaction();
                ServiceStat.InitOrUpdateSubActivityNoTransaction(subActId, userID, croleId, userID, UserIPaddress, UserProxyIPaddress, 100, Mark, true, true, isPassed);

                Manager.Commit();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }
        public bool ExecuteSubActivityInternal(long idSubactivity, int idUser,  StatusStatistic status)
        {
            Boolean isPassed = false;
            Int16 mark = 0;
            try{
                Manager.BeginTransaction();
                SubActivity s = Manager.Get<SubActivity>(idSubactivity);
                if (s != null)
                {
                    Int32 idRole = (from ls in Manager.GetIQ<LazySubscription>()
                                    where ls.IdCommunity == ((s.Community == null) ? 0 : s.Community.Id) && ls.IdPerson == idUser
                                    select ls.IdRole).Skip(0).Take(1).ToList().FirstOrDefault();
                    long completion = (status == StatusStatistic.CompletedPassed || status == StatusStatistic.Completed) ? 100 : 0;
                    if (CheckStatus(s.Status, Status.EvaluableDigital))
                    {
                        isPassed = (status == StatusStatistic.CompletedPassed || status == StatusStatistic.Passed);
                        mark = (status == StatusStatistic.CompletedPassed || status == StatusStatistic.Completed) ? (Int16)100 : (Int16)0;
                    }
                    ServiceStat.InitOrUpdateSubActivityNoTransaction(idSubactivity, idUser, idRole, idUser, UC.IpAddress, UC.ProxyIpAddress, completion, mark, true, (status == StatusStatistic.CompletedPassed || status == StatusStatistic.Completed), isPassed);
                }
                Manager.Commit();
                //Manager.Flush();
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
            return true;
        }

        public Int32 CommunityRoleId(Int32 IdCommunity, Int32 IdUser)
        {
            Int32 idRole = (from ls in Manager.GetIQ<LazySubscription>()
                            where ls.IdCommunity == IdCommunity && ls.IdPerson == IdUser
                            select ls.IdRole).Skip(0).Take(1).ToList().FirstOrDefault();
            return idRole;
        }


        public EPType GetEpType(long itemId, ItemType type)
        {
            switch (type)
            {
                case ItemType.Path:
                    return Manager.Get<Path>(itemId).EPType;

                case ItemType.Unit:
                    return Manager.Get<Unit>(itemId).ParentPath.EPType;

                case ItemType.Activity:
                    return Manager.Get<Activity>(itemId).Path.EPType;

                case ItemType.SubActivity:
                    return Manager.Get<SubActivity>(itemId).Path.EPType;

                default:
                    return EPType.None;
            }
        }

        public IList<long> GetRepositoryLinksByActivity(long actId)
        {
            return (from sub in Manager.GetIQ<SubActivity>()
                    where sub.ParentActivity.Id == actId && sub.Deleted == BaseStatusDeleted.None && sub.ContentType == SubActivityType.File
                    select sub.ModuleLink.Id).ToList();
        }

        public IList<long> GetRepositoryLinksPath(long idPath, Int32 idPerson)
        {
            List<long> toCheck = new List<long>();
            DateTime dPlay = DateTime.Now;
            Path path = GetPath(idPath);
            if (path!=null){
                   var query = (from sub in Manager.GetIQ<liteSubActivity>()
                    where sub.IdPath == idPath && sub.Deleted == BaseStatusDeleted.None && sub.ContentType == SubActivityType.File && sub.ModuleLink != null
                    select new {IdLink = sub.ModuleLink.Id, IdSubActivity=sub.Id}).ToList();
                switch(path.Policy.Statistics){
                    case CompletionPolicy.NoUpdateIfCompleted:
                        foreach (var x in query)
                        {
                            if (!GetUserStatistics(x.IdSubActivity, idPerson, dPlay).Any(i => ServiceStat.CheckStatusStatistic(i.Status, StatusStatistic.CompletedPassed)))
                                toCheck.Add(x.IdLink);
                        }
                        return toCheck;
                    default:
                        return query.Select(i=> i.IdLink).Distinct().ToList();
                }
            }
            return new List<long>();
        }

        //public bool ExistActsEvaluableAnalogic(long pathId, int evalId, int evalCRole)
        //{
        //  //  var x = ServiceAss.

        //    IList<long> subActsEvalAnal = (from subAct in Manager.GetAll<SubActivity>(item => item.Path.Id == pathId && item.Deleted == BaseStatusDeleted.None)
        //                                   where CheckStatus(subAct.Status, Status.EvaluableAnalog)
        //                                   select subAct.Id).ToList();

        //}

        #region Update visibility & Mandatory Status

        public bool ExistActivityEmpty(long pathId)
        {
            IList<Activity> activities = (from act in Manager.GetIQ<Activity>() where act.Path.Id == pathId && act.Deleted ==  BaseStatusDeleted.None select act).ToList();

            activities = (from act in activities where !CheckStatus(act.Status, Status.Draft) && !CheckStatus(act.Status, Status.Text) select act).ToList();
            if (activities.Count == 0)
            {
                return true;
            }

            foreach (Activity act in activities)
            {
                if (act.SubActivityList.Count == 0)
                {
                    return true;
                }
                else if ((from sub in act.SubActivityList where !CheckStatus(sub.Status, Status.Draft) && sub.Deleted ==  BaseStatusDeleted.None select sub).Count() == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public short UpdatePathVisibilityStatus(long pathId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            short result = 0;
            try
            {
                Manager.BeginTransaction();
                Path oPath = Manager.Get<Path>(pathId);

                if (CheckStatus(oPath.Status, Status.Locked))
                {
                    if (CheckEpType(oPath.EPType, EPType.Auto) && !(oPath.Weight == oPath.WeightAuto))
                        result = 2;
                    else if (ExistActivityEmpty(pathId))
                        result = 3;
                    else
                    { oPath.setNotLocked(); }
                }
                else
                    oPath.setLocked();
                if (result==0)
                    oPath.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                Manager.Commit();
                if (result == 0)
                    result = 1;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                result = 0;
            }
            finally
            {
                if (Manager.IsInTransaction())
                    Manager.Commit();
            }
            return result;
        }
        public Boolean PathSetToBlockStatus(long idPath, int idPerson, String ipAddress, String proxyIpAddress)
        {
            Boolean updated = false;
            try
            {
                Manager.BeginTransaction();
                Path oPath = Manager.Get<Path>(idPath);

                if (CheckStatus(oPath.Status, Status.Locked))
                    updated = true;
                else
                    oPath.setLocked();
                if (!updated)
                    oPath.UpdateMetaInfo(idPerson, ipAddress, proxyIpAddress);
                Manager.Commit();
                updated = true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
            }
            return updated;
        }
        public Boolean IsMooc(long idPath)
        {
            return (from p in Manager.GetIQ<Path>() where p.Id == idPath && p.IsMooc select p.Id).Any();
        }
        //public bool UpdatePathMandatoryStatus(long pathId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        Path oPath = Manager.Get<Path>(pathId);
        //        if (CheckStatus(oPath.Status, Status.Mandatory))
        //        {
        //            oPath.setNotMandatory();
        //        }
        //        else
        //        {
        //            oPath.setMandatory();
        //        }
        //        SetPathModifyMetaInfo(oPath, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}

        public bool UpdateUnitVisibilityStatus(long unitId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Manager.BeginTransaction();
                Unit oUnit = Manager.Get<Unit>(unitId);
                if (CheckStatus(oUnit.Status, Status.Locked))
                {
                    oUnit.setNotLocked();
                }
                else
                {
                    oUnit.setLocked();
                }
                oUnit.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                //SetUnitModifyMetaInfo(oUnit, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        public bool UpdateUnitMandatoryStatus(long unitId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Manager.BeginTransaction();
                Unit oUnit = Manager.Get<Unit>(unitId);
                if (CheckStatus(oUnit.Status, Status.Mandatory))
                {
                    oUnit.setNotMandatory();
                }
                else
                {
                    oUnit.setMandatory();
                }
                oUnit.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                //SetUnitModifyMetaInfo(oUnit, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        public bool UpdateActivityVisibilityStatus(long activityId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Manager.BeginTransaction();
                Activity oActivity = Manager.Get<Activity>(activityId);
                if (CheckStatus(oActivity.Status, Status.Locked))
                {
                    oActivity.setNotLocked();
                }
                else
                {
                    oActivity.setLocked();
                }
                oActivity.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                //SetActivityModifyMetaInfo(oActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        public bool UpdateActivityMandatoryStatus(long activityId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Manager.BeginTransaction();
                Activity oActivity = Manager.Get<Activity>(activityId);
                if (CheckStatus(oActivity.Status, Status.Mandatory))
                {
                    oActivity.setNotMandatory();
                }
                else
                {
                    oActivity.setMandatory();
                }
                oActivity.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                //SetActivityModifyMetaInfo(oActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }


        public bool UpdateSubActivityVisibilityStatus(long subActivityId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Manager.BeginTransaction();
                SubActivity oSubActivity = Manager.Get<SubActivity>(subActivityId);
                if (CheckStatus(oSubActivity.Status, Status.Locked))
                {
                    oSubActivity.setNotLocked();
                }
                else
                {
                    oSubActivity.setLocked();
                }
                oSubActivity.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                //SetSubActivityModifyMetaInfo(oSubActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }

        public bool UpdateSubActivityWeight(Int64 actId, IList<dtoWeight> subactivities, bool isAutoEp, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            bool isUpdated = false;
            SubActivity oSubActivity;


            try
            {
                Manager.BeginTransaction();

                foreach (dtoWeight item in subactivities)
                {
                    oSubActivity = Manager.Get<SubActivity>(item.Id);
                    if (oSubActivity.Weight != item.Weight)
                    {
                        isUpdated = true;
                        oSubActivity.Weight = item.Weight;
                        oSubActivity.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                        //SetSubActivityModifyMetaInfo(oSubActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);

                    }
                }

                if (isUpdated && isAutoEp)
                {
                    Activity oAct = Manager.Get<Activity>(actId);
                    oAct.Weight = (Int16)(from item in subactivities select item.Weight).Sum(a => a);
                }

                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }


        public bool UpdateSubActivityMandatoryStatus(long subActivityId, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        {
            try
            {
                Manager.BeginTransaction();
                SubActivity oSubActivity = Manager.Get<SubActivity>(subActivityId);
                if (CheckStatus(oSubActivity.Status, Status.Mandatory))
                {
                    oSubActivity.setNotMandatory();
                }
                else
                {
                    oSubActivity.setMandatory();
                }
                oSubActivity.UpdateMetaInfo(PersonId, PersonIpAddress, PersonProxyIpAddress);
                //SetSubActivityModifyMetaInfo(, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
                Manager.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
                Debug.Write(ex);
                return false;
            }
        }
        #endregion
        #region update mandatory or visibility status ->>SERVIVAVANO IN CASO DI UPDATE DI UN ITEM PER AGGIORNARE LE STATISTICHE DI CHI AVEVA GIA' ESEGUITO IL PERCORSO O PERSONALIZZARE L'ASSEGNAMENTO A CHI AVEVA GIA' INIZIATO IL PERCORSO (DA DEBUGGARE)

        //public bool UpdateSubActivityMandatoryOrVisibilityStatus(long SubActivityId, UpdateAssignemtOrStatistic updateAssOrStat, UpdateStatusType updateStatusType, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {
        //        SubActivity oSubActivity = Manager.Get<SubActivity>(SubActivityId);
        //        Status statusForAssignment = oSubActivity.Status;
        //        switch (updateStatusType)
        //        {
        //            case UpdateStatusType.MandatoryStatus:
        //                if (oSubActivity.CheckStatus(Status.Mandatory))
        //                {
        //                    oSubActivity.setNotMandatory();
        //                    statusForAssignment = Status.Mandatory;
        //                }
        //                else
        //                {
        //                    oSubActivity.setMandatory();
        //                    statusForAssignment = Status.NotMandatory;
        //                }
        //                break;

        //            case UpdateStatusType.VisibilityStatus:
        //                if ((oSubActivity.Status & Status.Locked) == Status.Locked)
        //                {
        //                    oSubActivity.setNotLocked();
        //                    statusForAssignment = Status.Locked;
        //                }
        //                else
        //                {
        //                    oSubActivity.setLocked();
        //                    statusForAssignment = Status.NotLocked;
        //                }
        //                break;

        //            default:
        //                return false;
        //        }

        //        SetSubActivityModifyMetaInfo(oSubActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<SubActivity>(oSubActivity);
        //        switch (updateAssOrStat)
        //        {
        //            case UpdateAssignemtOrStatistic.Assignment:
        //                ServiceAss.SetSubActivityAssignment_PersonalStatus(SubActivityId, PersonId, PersonProxyIpAddress, PersonIpAddress, updateStatusType, statusForAssignment, true);
        //                break;

        //            case UpdateAssignemtOrStatistic.Statistic:
        //                ServiceStat.UpdateParentSubActivityStatisticNoTransaction(SubActivityId, oSubActivity.Community.Id, PersonId, PersonIpAddress, PersonProxyIpAddress);
        //                break;
        //        }

        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}

        //public bool UpdateActivityMandatoryOrVisibilityStatus(long ActivityId, UpdateAssignemtOrStatistic updateAssOrStat, UpdateStatusType updateType, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {
        //        Activity oActivity = Manager.Get<Activity>(ActivityId);
        //        Status statusForAssignment;
        //        switch (updateType)
        //        {
        //            case UpdateStatusType.MandatoryStatus:
        //                if (oActivity.CheckStatus(Status.Mandatory))
        //                {
        //                    oActivity.setNotMandatory();
        //                    statusForAssignment = Status.Mandatory;
        //                }
        //                else
        //                {
        //                    oActivity.setMandatory();
        //                    statusForAssignment = Status.NotMandatory;
        //                }
        //                break;

        //            case UpdateStatusType.VisibilityStatus:
        //                if ((oActivity.Status & Status.Locked) == Status.Locked)
        //                {
        //                    oActivity.setNotLocked();
        //                    statusForAssignment = Status.Locked;
        //                }
        //                else
        //                {
        //                    oActivity.setLocked();
        //                    statusForAssignment = Status.NotLocked;
        //                }
        //                break;

        //            default:
        //                return false;
        //        }

        //        SetActivityModifyMetaInfo(oActivity, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<Activity>(oActivity);
        //        switch (updateAssOrStat)
        //        {
        //            case UpdateAssignemtOrStatistic.Assignment:
        //                ServiceAss.SetActivityAssignment_PersonalStatus(ActivityId, PersonId, PersonProxyIpAddress, PersonIpAddress, updateType, statusForAssignment, true);
        //                break;

        //            case UpdateAssignemtOrStatistic.Statistic:
        //                ServiceStat.UpdateParentActivityStatisticNoTransaction(ActivityId, oActivity.Community.Id, PersonId, PersonIpAddress, PersonProxyIpAddress);
        //                break;
        //        }

        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}

        //public bool UpdateUnitMandatoryOrVisibilityStatus(long UnitId, UpdateAssignemtOrStatistic updateAssOrStat, UpdateStatusType updateType, int PersonId, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    try
        //    {

        //        Unit oUnit = Manager.Get<Unit>(UnitId);
        //        Status statusForAssignment;

        //        switch (updateType)
        //        {
        //            case UpdateStatusType.MandatoryStatus:
        //                if (oUnit.CheckStatus(Status.Mandatory))
        //                {
        //                    oUnit.setNotMandatory();
        //                    statusForAssignment = Status.Mandatory;
        //                }
        //                else
        //                {
        //                    oUnit.setMandatory();
        //                    statusForAssignment = Status.NotMandatory;
        //                }
        //                break;

        //            case UpdateStatusType.VisibilityStatus:
        //                if ((oUnit.Status & Status.Locked) == Status.Locked)
        //                {
        //                    oUnit.setNotLocked();
        //                    statusForAssignment = Status.Locked;
        //                }
        //                else
        //                {
        //                    oUnit.setLocked();
        //                    statusForAssignment = Status.NotLocked;
        //                }
        //                break;

        //            default:
        //                return false;
        //        }

        //        SetUnitModifyMetaInfo(oUnit, Manager.GetPerson(PersonId), PersonIpAddress, PersonProxyIpAddress, DateTime.Now);
        //        Manager.BeginTransaction();
        //        Manager.SaveOrUpdate<Unit>(oUnit);
        //        switch (updateAssOrStat)
        //        {
        //            case UpdateAssignemtOrStatistic.Assignment:
        //                ServiceAss.SetUnitAssignment_PersonalStatus(UnitId, PersonId, PersonProxyIpAddress, PersonIpAddress, updateType, statusForAssignment, true);
        //                break;

        //            case UpdateAssignemtOrStatistic.Statistic:
        //                ServiceStat.UpdateParentUnitStatisticNoTransaction(UnitId, oUnit.Community.Id, PersonId, PersonIpAddress, PersonProxyIpAddress);
        //                break;
        //        }

        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}


        //public bool UpdatePathVisibilityStatus(long EpId, bool updateAssignments, int PersonID, String PersonIpAddress, String PersonProxyIpAddress)
        //{
        //    Path oEp = Manager.Get<Path>(EpId);
        //    DateTime CurrentTime = DateTime.Now;
        //    Person oAutorOfUpdate = Manager.GetPerson(PersonID);
        //    try
        //    {
        //        Manager.BeginTransaction();
        //        SetPathModifyMetaInfo(oEp, oAutorOfUpdate, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
        //        Status statusForAssignments = Status.None;
        //        if ((oEp.Status & Status.Locked) == Status.Locked)
        //        {
        //            oEp.setNotLocked();
        //            statusForAssignments |= (Status.Locked);
        //        }
        //        else
        //        {
        //            oEp.setLocked();
        //            statusForAssignments |= (Status.NotLocked);
        //        }
        //        if (updateAssignments)
        //        {
        //            ServiceAss.SetPathtAssignment_StatusAsPath_Status(EpId, PersonID, PersonProxyIpAddress, PersonIpAddress, statusForAssignments, true);
        //        }
        //        Manager.SaveOrUpdate<Path>(oEp);
        //        Manager.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Manager.RollBack();
        //        Debug.Write(ex);
        //        return false;
        //    }
        //}
        #endregion

        public IList<long> GetActvityWithEvalAnal(long pathId)
        {
            return (from sub in Manager.GetAll<SubActivity>(s => s.Path.Id == pathId && s.Deleted == BaseStatusDeleted.None)
                    where sub.CheckStatus(Status.EvaluableAnalog)
                    select sub.ParentActivity.Id).Distinct().ToList();


        }

        public bool ExistItemToEvaluate(Int64 epId, EPType type)
        {
            if (CheckEpType(type, EPType.Manual))
            {
                var a = (from sub in Manager.GetIQ<SubActivity>()
                         where sub.Path.Id == epId && sub.ContentType == SubActivityType.Text
                         select sub.Status).ToList();
                foreach (Status item in a)
                {
                    if (CheckStatus(item, Status.EvaluableAnalog))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanAddUnitRule(IList<dtoUnit> units)
        {
            return (from unit in units
                    where !CheckStatus(unit.Status, Status.Text)
                    select unit.Status).Count() > 1;
        }


        public bool CanAddActivityRule(IList<dtoActivity> activities)
        {
            return (from act in activities
                    where !CheckStatus(act.Status, Status.Text)
                    select act.Status).Count() > 1;
        }

        public bool CanViewAct_byDate(DateTime? startDate, DateTime? endDate)
        {
            DateTime now = DateTime.Now;
            if (startDate.HasValue && endDate.HasValue)
                return (startDate.Value <= now && now <= endDate.Value);
            else if (startDate.HasValue)
                return (startDate.Value <= now);
            else if (endDate.HasValue)
                return (endDate.Value >= now);
            else
                return true;
        }

        public string GetDate(System.DateTime? oDateToView)
        {
            if (oDateToView == null)
            {
                return "";
            }

            System.DateTime oDate = (DateTime)oDateToView;


            if (oDate.Second == 0 && oDate.Minute == 0 && oDate.Hour == 0)
            {
                return string.Format("{0:d/M/yyyy}", oDate);

            }
            else if (oDate.Second == 0 && oDate.Minute > 0)
            {
                return string.Format("{0:d/M/yyyy HH:mm}", oDate);

            }
            else if (oDate.Second == 0 && oDate.Minute == 0 && oDate.Hour > 0)
            {
                return string.Format("{0:d/M/yyyy HH}", oDate);

            }
            else
            {
                return string.Format("{0:d/M/yyyy HH:mm:ss}", oDate);

            }
        }

        public string GetHourMinute(DateTime date)
        {
            return date.Hour + "h " + date.Minute + "m";
        }

        public DtoCommunitiesPaths GetCommunitiesPaths(String community="",  Int32 pageindex = 0, Int32 pagesize = 0, string order="", Boolean ascending=true, IEnumerable<Int32> idcommunities = null)
        {
            DtoCommunitiesPaths ret = new DtoCommunitiesPaths();

            //var allPath = (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None select item).ToList();

            ILookup<Int32, Path> comm = null;

            if (string.IsNullOrEmpty(community))
            {
                if (idcommunities != null && idcommunities.Count() > 0)
                {

                    comm = (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Status != Status.None && item.Community != null && idcommunities.Contains( item.Community.Id)  select item).ToLookup(x => x.Community.Id, x => x);                                
                }
                else
                {
                    comm = (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Status != Status.None && item.Community != null select item).ToLookup(x => x.Community.Id, x => x);                                
                }

                
            }
            else
            {
                if (idcommunities != null && idcommunities.Count() > 0)
                {
                    var allComIds = (from item in Manager.GetIQ<Community>() where item.Name.Contains(community) && idcommunities.Contains(item.Id) select item.Id).ToList();

                    comm = (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Status != Status.None && item.Community != null && allComIds.Contains(item.Community.Id) select item).ToLookup(x => x.Community.Id, x => x);
                }
                else
                {
                    var allComIds = (from item in Manager.GetIQ<Community>() where item.Name.Contains(community) select item.Id).ToList();

                    comm = (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Status != Status.None && item.Community != null && allComIds.Contains(item.Community.Id) select item).ToLookup(x => x.Community.Id, x => x);
                }

                
            }
            
            ret.CommunityPaths = new List<DtoCommunityPaths>();                       

            foreach (var item in comm)
            {
                DtoCommunityPaths dto = new DtoCommunityPaths();
                dto.IdCommunity = item.Key;

                COL_Comunita c = new COL_Comunita(dto.IdCommunity);
                dto.CommunityName = c.EstraiNomeBylingua(1);

                dto.PathsCount = item.Count();
                dto.LockedCount = (from i in item where CheckStatus(i.Status, Status.Locked) select i.Id).Count();
                dto.UnlockedCount = (from i in item where CheckStatus(i.Status, Status.NotLocked) select i.Id).Count();
                dto.DraftCount = (from i in item where CheckStatus(i.Status, Status.Draft) select i.Id).Count();

                if (item.Count() > 0)
                {
                    ret.CommunityPaths.Add(dto);
                }
            }

            if (!string.IsNullOrEmpty(order))
            {
                if (ascending)
                {
                    switch (order)
                    {
                        case "community":
                            ret.CommunityPaths = ret.CommunityPaths.OrderBy(x => x.CommunityName).ToList();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (order)
                    {
                        case "community":
                            ret.CommunityPaths = ret.CommunityPaths.OrderByDescending(x => x.CommunityName).ToList();
                            break;
                        default:
                            break;
                    }
                }

            }

            if (pagesize > 0)
            {
                ret.CommunityPaths = ret.CommunityPaths.Skip(pagesize * pageindex).Take(pagesize).ToList();
            }

            ret.PathsCount = (from item in ret.CommunityPaths select item.PathsCount).Sum();
            ret.LockedCount = (from item in ret.CommunityPaths select item.LockedCount).Sum();
            ret.UnlockedCount = (from item in ret.CommunityPaths select item.UnlockedCount).Sum();
            ret.DraftCount = (from item in ret.CommunityPaths select item.DraftCount).Sum();

            return ret;
        }

        public Int32 GetCommunitiesPathsCount(Int32 idOrganization = 0)
        {
            return (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Community != null select item.Community.Id).ToList().Distinct().Count();
        }

        public Int32 GetCommunitiesPathsCount(IList<Int32> communities)
        {
            return (from item in Manager.GetIQ<Path>() where item.Deleted == BaseStatusDeleted.None && item.Community != null && communities.Contains(item.Community.Id) select item.Community.Id).ToList().Distinct().Count();
        }


        #region "New functions"
            public String GetCertificationRestoreUrl(long idPath, long idActivity, long idSubactivity, Int32 idUser, Boolean modal = false)
            {
                lm.Comol.Core.Authentication.liteGenericEncryption m= Manager.GetUrlMacEncryptor();
                String mac = "";
                DateTime date = DateTime.Now;
                TimeSpan tValidity = new TimeSpan(2, 0, 0);

                if (m!=null)
                    mac = m.Encrypt(UC.WorkSessionID, date.Ticks, tValidity.Ticks, idPath, idSubactivity, idUser);
                //return RootObject.CertificationRestorePage(mac, date, tValidity, idPath, idActivity, idSubactivity, idUser, modal);

                return RootObject.CertificationGenerateAndDownloadPage(mac, date, tValidity, idPath, idActivity, idSubactivity, idUser, modal);
            }
            public String ReplaceInvalidFileName(String filename)
            {
                string regex = string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())));
                System.Text.RegularExpressions.Regex removeInvalidChars = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

                return removeInvalidChars.Replace(filename.Replace(" ", "_"), "_");
            }
        #endregion


        #region "Configuration"
        public dtoConfigurationSetting GetConfigurationSetting(long idPath=0, Int32 idCommunity=0, ConfigurationType type = ConfigurationType.Module)
        {
            dtoConfigurationSetting result = null;
            try
            {
                Int32 idOrganization = 0;
                if (type == ConfigurationType.Export)
                    type = ConfigurationType.Module;
                if (idCommunity > 0)
                    idOrganization = Manager.GetIdOrganizationFromCommunity(idCommunity); 
                List<dtoConfigurationSetting> settings = (from s in Manager.GetIQ<ConfigurationSetting>()
                                                          where s.ConfigType == ConfigurationType.Module && s.Deleted == BaseStatusDeleted.None && s.IsEnabled &&
                                                       (s.ForAllPath || (idCommunity>0 && s.IdCommunity == idCommunity && s.Path == null)
                                                       || (idOrganization > 0 && s.IdOrganization == idOrganization && s.Path == null)
                                                       || (s.Path!=null && s.Path.Id==idPath)) select s).ToList().Select(s=>
                                                          new dtoConfigurationSetting(s)).ToList().OrderBy( s=> s.ValidityLevel).ToList();

                result = settings.Any() ? settings.FirstOrDefault() : new dtoConfigurationSetting() { AllowDeleteFullStatistics = false, AllowDeleteStatistics = false };

            }
            catch (Exception ex) {
            
            }
            if (result == null)
                result = new dtoConfigurationSetting() { AllowDeleteFullStatistics = false, AllowDeleteStatistics = false };
            return result;
        }
        public dtoExportConfigurationSetting GetExportSetting(long idPath, Int32 idCommunity, StatisticsPageType pageType,StatisticType sType, ConfigurationType type= ConfigurationType.Export)
        {
            dtoExportConfigurationSetting result = null;
            try
            {
                Int32 idOrganization = 0;
                if (idCommunity > 0)
                    idOrganization = Manager.GetIdOrganizationFromCommunity(idCommunity);
                List<dtoExportConfigurationSetting> settings = (from s in Manager.GetIQ<ExportConfigurationSetting>()
                                                                where s.ConfigType == type && s.Deleted == BaseStatusDeleted.None && s.IsEnabled &&
                                                      s.PageType == pageType &&
                                                          (s.ForAllPath || (idCommunity > 0 && s.IdCommunity == idCommunity && s.Path == null)
                                                       || (idOrganization > 0 && s.IdOrganization == idOrganization && s.Path == null)
                                                          || (s.Path != null && s.Path.Id == idPath)) select s).ToList().Select(s=>
                                                            new dtoExportConfigurationSetting(s)).ToList().OrderBy(s => s.ValidityLevel).ToList();

                result = settings.Any() ? settings.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {

            }
            if (result == null)
                result = dtoExportConfigurationSetting.GetDefaultSetting(pageType, type, sType);
            return result;
        }
        public List<StatisticsPageType> GetAvailableExportStatistics(long idPath, Int32 idCommunity)
        {
            List<StatisticsPageType> items = new List<StatisticsPageType>();
            try
            {
                Int32 idOrganization = 0;
                if (idCommunity > 0)
                    idOrganization = Manager.GetIdOrganizationFromCommunity(idCommunity); 
                items = (from s in Manager.GetIQ<ExportConfigurationSetting>()
                            where s.ConfigType == ConfigurationType.Export && s.Deleted == BaseStatusDeleted.None && s.IsEnabled &&
                            (s.ForAllPath || (idCommunity > 0 && s.IdCommunity == idCommunity && s.Path == null)
                                                       || (idOrganization > 0 && s.IdOrganization == idOrganization && s.Path == null)
                            || (s.Path != null && s.Path.Id == idPath))
                            select s.PageType).ToList();
            }
            catch (Exception ex)
            {

            }
            return items;
        }

#endregion

        #region Cokade

        public bool CokadeEnabled(int communityId)
        {
            MoocCokade cok = (from MoocCokade c in Manager.GetIQ<MoocCokade>()
                where c.CommunityId == communityId
                select c).Skip(0).Take(1).FirstOrDefault();

            return (cok != null) && cok.CokadeEnable;
        }

        public void CokadeSet(int communityId, bool value)
        {
            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();

            MoocCokade cok = (from MoocCokade c in Manager.GetIQ<MoocCokade>()
                              where c.CommunityId == communityId
                              select c).Skip(0).Take(1).FirstOrDefault();

            if (cok == null)
            {
                cok = new MoocCokade();
                cok.CommunityId = communityId;
            }

            cok.CokadeEnable = value;


            try
            {
                Manager.SaveOrUpdate(cok);
                Manager.Commit();
            }
            catch (Exception)
            {
                Manager.RollBack();
            }

        }

        #endregion
    }




    public enum UpdateStatusType
    {
        MandatoryStatus,
        VisibilityStatus
    }

    public enum UpdateAssignemtOrStatistic
    {
        None,
        Assignment,
        Statistic
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using System.Diagnostics;
using lm.Comol.Core.DomainModel;
using Entity = Comol.Entity;
using System.Linq.Expressions;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public partial class ServiceAssignment
    {
        private const Int32 maxItemsForSingleQuery = 1000;
        private ManagerEP Manager { get; set; }
        private Service ServiceEp { get; set; }


        

        #region init class
        public ServiceAssignment()
        {
        }

        public ServiceAssignment(ManagerEP Manager, Service ServiceEp)
        {
            this.ServiceEp = ServiceEp;
            this.Manager = Manager;
        }

        #endregion


        #region RoleUpdate No Tranasaction

            public void UpdateRoleAssignmentsPathNoTransaction(IList<dtoGenericAssignmentWithDelete> dtoPersonAssignments, IList<dtoGenericAssignment> dtoCRoleAssignments, int UpdaterID, String UpdaterIpAddress, String UpdaterProxyIpAddress, DateTime TimeNow)
            {
                PathPersonAssignment pAssignment;
                foreach (dtoGenericAssignmentWithDelete item in dtoPersonAssignments)
                {
                    pAssignment = Manager.Get<PathPersonAssignment>(item.DB_ID);
                    if (pAssignment != null)
                    {
                        pAssignment.UpdateMetaInfo(UpdaterID, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        //SetPathPersonAssModifyMetaInfo(PersonAssToUpdate, InterestedPerson, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        pAssignment.RoleEP = item.RoleEP;
                        Manager.SaveOrUpdate<PathPersonAssignment>(pAssignment);
                    }
                }
                PathCRoleAssignment roleAssignment;
                foreach (dtoGenericAssignment item in dtoCRoleAssignments)
                {
                    roleAssignment = Manager.Get<PathCRoleAssignment>(item.DB_ID);
                    if (roleAssignment != null)
                    {
                        roleAssignment.UpdateMetaInfo(UpdaterID, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        //SetPathCRoleAssModifyMetaInfo(CRoleAssToUpdate, InterestedPerson, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        roleAssignment.RoleEP = item.RoleEP;
                        Manager.SaveOrUpdate<PathCRoleAssignment>(roleAssignment);
                    }
                }
            }

            public void UpdateRoleAssignmentsUnitNoTransaction(IList<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, IList<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, int UpdaterID, String UpdaterIpAddress, String UpdaterProxyIpAddress, DateTime TimeNow)
            {
                UnitPersonAssignment pAssignment;
                foreach (dtoGenericAssWithOldRoleEpAndDelete item in dtoPersonAssignments)
                {
                    pAssignment = Manager.Get<UnitPersonAssignment>(item.DB_ID);
                    if (pAssignment != null)
                    {
                        pAssignment.UpdateMetaInfo(UpdaterID, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        //SetUnitPersonAssModifyMetaInfo(PersonAssToUpdate, InterestedPerson, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        pAssignment.RoleEP = item.RoleEP;
                        Manager.SaveOrUpdate<UnitPersonAssignment>(pAssignment);
                    }
                }

                UnitCRoleAssignment roleAssignment;
                foreach (dtoGenericAssignmentWithOldRoleEP item in dtoCRoleAssignments)
                {
                    roleAssignment = Manager.Get<UnitCRoleAssignment>(item.DB_ID);
                    if (roleAssignment != null)
                    {
                        roleAssignment.RoleEP = item.RoleEP;
                        roleAssignment.UpdateMetaInfo(UpdaterID, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        //SetUnitCRoleAssModifyMetaInfo(CRoleAssToUpdate, InterestedPerson, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                        Manager.SaveOrUpdate<UnitCRoleAssignment>(roleAssignment);
                    }
                }
            }

        public void UpdateRoleAssignmentsActivityNoTransaction(IList<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, IList<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, int UpdaterID, String UpdaterIpAddress, String UpdaterProxyIpAddress, DateTime TimeNow)
        {
            ActivityPersonAssignment pAssignment;
            foreach (dtoGenericAssignmentWithOldRoleEP item in dtoPersonAssignments)
            {
                pAssignment = Manager.Get<ActivityPersonAssignment>(item.DB_ID);
                if (pAssignment != null)
                {
                    pAssignment.UpdateMetaInfo(UpdaterID, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                    //SetActivityPersonAssModifyMetaInfo(PersonAssToUpdate, InterestedPerson, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                    pAssignment.RoleEP = item.RoleEP;
                    Manager.SaveOrUpdate<ActivityPersonAssignment>(pAssignment);
                }
            }
            ActivityCRoleAssignment roleAssignment;
            foreach (dtoGenericAssignmentWithOldRoleEP item in dtoCRoleAssignments)
            {
                roleAssignment = Manager.Get<ActivityCRoleAssignment>(item.DB_ID);
                if (roleAssignment != null)
                {
                    roleAssignment.UpdateMetaInfo(UpdaterID, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                    //SetActivityCRoleAssModifyMetaInfo(CRoleAssToUpdate, InterestedPerson, UpdaterIpAddress, UpdaterProxyIpAddress, TimeNow);
                    roleAssignment.RoleEP = item.RoleEP;
                    Manager.SaveOrUpdate<ActivityCRoleAssignment>(roleAssignment);
                }
            }
        }

        #endregion

        #region Delete Permanently
        public void DeletePathPersonAssignments(List<long> ListOfInterestedId)
        {
            PathPersonAssignment AssignmentToDelete;
            foreach (long item in ListOfInterestedId)
            {
                AssignmentToDelete = Manager.Get<PathPersonAssignment>(item);
                Manager.DeleteGeneric(AssignmentToDelete);
            }
        }
        public void DeletePathCRoleAssignments(List<long> ListOfInterestedId)
        {
            PathCRoleAssignment AssignmentToDelete;
            foreach (long item in ListOfInterestedId)
            {
                AssignmentToDelete = Manager.Get<PathCRoleAssignment>(item);
                Manager.DeleteGeneric(AssignmentToDelete);
            }
        }
        public void DeleteUnitAssignments(List<long> PersonAssignmentsId, List<long> CRoleAssignmentId)
        {
            UnitPersonAssignment PersonAssignmentToDelete;
            foreach (long item in PersonAssignmentsId)
            {
                PersonAssignmentToDelete = Manager.Get<UnitPersonAssignment>(item);
                Manager.DeleteGeneric(PersonAssignmentToDelete);
            }
            UnitCRoleAssignment CRoleAssignmentToDelete;
            foreach (long item in CRoleAssignmentId)
            {
                CRoleAssignmentToDelete = Manager.Get<UnitCRoleAssignment>(item);
                Manager.DeleteGeneric(CRoleAssignmentToDelete);
            }
        }
        public void DeleteActivityAssignments(List<long> PersonAssignmentsId, List<long> CRoleAssignmentId)
        {
            ActivityPersonAssignment PersonAssignmentToDelete;
            foreach (long item in PersonAssignmentsId)
            {
                PersonAssignmentToDelete = Manager.Get<ActivityPersonAssignment>(item);
                Manager.DeleteGeneric(PersonAssignmentToDelete);
            }
            ActivityCRoleAssignment CRoleAssignmentToDelete;
            foreach (long item in CRoleAssignmentId)
            {
                CRoleAssignmentToDelete = Manager.Get<ActivityCRoleAssignment>(item);
                Manager.DeleteGeneric(CRoleAssignmentToDelete);
            }
        }

        #endregion

        #region virtual delete no transaction

        public void VirtualDeleteSubActivityAssignmentsNoTransaction(long subActivityId, Int32 idAutorOfDelete, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            IList<SubActivityPersonAssignment> PersonAssignments = Manager.GetAll<SubActivityPersonAssignment>(ass => ass.IdSubActivity == subActivityId && ass.CreatedOn < CurrentTime);
            IList<SubActivityCRoleAssignment> CRoleAssignments = Manager.GetAll<SubActivityCRoleAssignment>(ass => ass.IdSubActivity == subActivityId && ass.CreatedOn < CurrentTime);

            foreach (SubActivityPersonAssignment ass in PersonAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetSubActivityPersonAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }

            foreach (SubActivityCRoleAssignment ass in CRoleAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetSubActivityCRoleAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress,  CurrentTime  );
            }
        }

        public void VirtualDeleteActivityAssignmentsNoTransaction(long activityId, Int32 idAutorOfDelete, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            IList<ActivityPersonAssignment> PersonAssignments = Manager.GetAll<ActivityPersonAssignment>(ass => ass.IdActivity == activityId && ass.CreatedOn < CurrentTime);
            IList<ActivityCRoleAssignment> CRoleAssignments = Manager.GetAll<ActivityCRoleAssignment>(ass => ass.IdActivity == activityId && ass.CreatedOn < CurrentTime);

            foreach (ActivityPersonAssignment ass in PersonAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetActivityPersonAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }

            foreach (ActivityCRoleAssignment ass in CRoleAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetActivityCRoleAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }
        }

        

        public void VirtualDeleteUnitAssignmentsNoTransaction(long UnitId, Int32 idAutorOfDelete, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            IList<UnitPersonAssignment> PersonAssignments = Manager.GetAll<UnitPersonAssignment>(a => a.IdUnit == UnitId && a.CreatedOn < CurrentTime);
            IList<UnitCRoleAssignment> CRoleAssignments = Manager.GetAll<UnitCRoleAssignment>(a => a.IdUnit == UnitId && a.CreatedOn < CurrentTime);

            foreach (UnitPersonAssignment ass in PersonAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetUnitPersonAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }

            foreach (UnitCRoleAssignment ass in CRoleAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
               // SetUnitCRoleAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }
        }

        public void VirtualDeletePathAssignmentsNoTransaction(long PathId, Int32 idAutorOfDelete, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime, BaseStatusDeleted DeleteType)
        {
            IList<PathPersonAssignment> PersonAssignments = Manager.GetAll<PathPersonAssignment>(a => a.IdPath == PathId && a.CreatedOn < CurrentTime);
            IList<PathCRoleAssignment> CRoleAssignments = Manager.GetAll<PathCRoleAssignment>(a => a.IdPath == PathId && a.CreatedOn < CurrentTime);

            foreach (PathPersonAssignment ass in PersonAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }

            foreach (PathCRoleAssignment ass in CRoleAssignments)
            {
                ass.Deleted |= DeleteType;
                ass.UpdateMetaInfo(idAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
                //SetPathCRoleAssModifyMetaInfo(ass, oAutorOfDelete, PersonIpAddress, PersonProxyIpAddress, CurrentTime);
            }
        }

        #endregion

        #region get personId with personal MinCompletion

        public IList<int> GetPersonIdWithPersonalActivityMinCompletion(long idActivity, int idCommunity)
        {
            IList<int> idPersonsWithPersonalMinCompletion = (from a in Manager.GetIQ<ActivityPersonAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdActivity == idActivity && a.MinCompletion >= 0 select a.IdPerson).ToList();
            IList<int> idRolesWithPersonalMinCompletion = (from a in Manager.GetIQ<ActivityCRoleAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdActivity == idActivity && a.MinCompletion >= 0 select a.IdRole).ToList();
            foreach (int idRole in idRolesWithPersonalMinCompletion)
            {
               idPersonsWithPersonalMinCompletion = idPersonsWithPersonalMinCompletion.Union(Manager.GetIdPerson_ByIdRole(idRole, idCommunity)).ToList() ;
            }         
            return idPersonsWithPersonalMinCompletion;
        }
        public IList<int> GetPersonIdWithPersonalUnitMinCompletion(long idUnit, int idCommunity)
        {
            IList<int> idPersonsWithPersonalMinCompletion = (from a in Manager.GetIQ<UnitPersonAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdUnit == idUnit && a.MinCompletion >= 0 select a.IdPerson).ToList();
            IList<int> idRolesWithPersonalMinCompletion = (from a in Manager.GetIQ<UnitCRoleAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdUnit == idUnit && a.MinCompletion >= 0 select a.IdRole).ToList();
            foreach (int croleId in idRolesWithPersonalMinCompletion)
            {
               idPersonsWithPersonalMinCompletion = idPersonsWithPersonalMinCompletion.Union(Manager.GetIdPerson_ByIdRole(croleId, idCommunity)).ToList();
            }
            return idPersonsWithPersonalMinCompletion;
        }
        public IList<int> GetPersonIdWithPersonalPathMinCompletion(long idPath, int idCommunity)
        {
            IList<int> idPersonsWithPersonalMinCompletion = (from a in Manager.GetIQ<PathPersonAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPath == idPath && a.MinCompletion >= 0 select a.IdPerson).ToList();
            IList<int> idRolesWithPersonalMinCompletion = (from a in Manager.GetIQ<PathCRoleAssignment>() where a.Deleted == BaseStatusDeleted.None && a.IdPath == idPath && a.MinCompletion >= 0 select a.IdRole).ToList();
            foreach (int idRole in idRolesWithPersonalMinCompletion)
            {
                idPersonsWithPersonalMinCompletion = idPersonsWithPersonalMinCompletion.Union(Manager.GetIdPerson_ByIdRole(idRole, idCommunity)).ToList();
            }
            return idPersonsWithPersonalMinCompletion;
        }
        #endregion

        #region save new Assignment no transaction

        public void SaveNewActivityCRoleAssignmentsNoTransction(List<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, Activity oActivity, Int32 idCommunity, DateTime TimeNow, Int32 idCreatedBy, String UserIPaddress, String UserProxyIPaddress)
        {
            IList<ActivityCRoleAssignment> Assignments = (from item in dtoCRoleAssignments
                                                          where item.RoleEP > 0 && item.DB_ID == 0
                                                          select new ActivityCRoleAssignment(oActivity.Id,item.DB_ID, item.ItemID, idCommunity, item.RoleEP, -1,
                                                                  idCreatedBy, TimeNow, UserIPaddress, UserProxyIPaddress)).ToList();
            Manager.SaveOrUpdateList<ActivityCRoleAssignment>(Assignments);
        }

        public void SaveNewActivityPersonAssignmentsNoTransction(List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, Activity oActivity, DateTime TimeNow, Int32 idCreatedBy, String UserIPaddress, String UserProxyIPaddress)
        {
            IList<ActivityPersonAssignment> Assignments = (from item in dtoPersonAssignments
                                                           where item.RoleEP > 0 && item.DB_ID == 0
                                                           select new ActivityPersonAssignment(oActivity.Id,item.DB_ID, item.ItemID, item.RoleEP,  -1,
                                                                   idCreatedBy, TimeNow, UserIPaddress, UserProxyIPaddress)).ToList();
            Manager.SaveOrUpdateList<ActivityPersonAssignment>(Assignments);
        }

        public void SaveNewPathCRoleAssignmentsNoTransaction(List<dtoGenericAssignment> dtoCRoleAssignments, Path oPath, DateTime TimeNow, Int32 idCreatedBy, String UserIPaddress, String UserProxyIPaddress)
        {
            IList<PathCRoleAssignment> CRoleAssignments = (from item in dtoCRoleAssignments
                                                           where item.RoleEP > 0 && item.DB_ID == 0
                                                           select new PathCRoleAssignment(oPath.Id,item.DB_ID, item.ItemID, oPath.Community.Id, item.RoleEP,  -1,
                                                                   idCreatedBy, TimeNow, UserIPaddress, UserProxyIPaddress)).ToList();
            Manager.SaveOrUpdateList<PathCRoleAssignment>(CRoleAssignments);
        }

        public void SaveNewPathPersonAssignmentsNoTranction(List<dtoGenericAssignmentWithDelete> dtoPersonAssignments, Path oPath, DateTime TimeNow, Int32 idCreatedBy, String UserIPaddress, String UserProxyIPaddress)
        {
            List<PathPersonAssignment> PersonAssignments = (from item in dtoPersonAssignments
                                                            where !item.isDeleted && item.RoleEP > 0 && item.DB_ID == 0
                                                            select new PathPersonAssignment(oPath.Id,item.DB_ID,item.ItemID, item.RoleEP, -1,
                                                                    idCreatedBy, TimeNow, UserIPaddress, UserProxyIPaddress)).ToList();
            Manager.SaveOrUpdateList<PathPersonAssignment>(PersonAssignments);
        }

        public void SaveNewUnitCRoleAssignmentsNoTransaction(List<dtoGenericAssignmentWithOldRoleEP> dtoCRoleAssignments, Unit oUnit, Int32 idCommunity, DateTime TimeNow, Int32 idCreatedBy, String UserIPaddress, String UserProxyIPaddress)
        {
            List<UnitCRoleAssignment> CRoleAssignments = (from item in dtoCRoleAssignments
                                                          where item.RoleEP > 0 && item.DB_ID == 0
                                                          select new UnitCRoleAssignment(oUnit.Id,item.DB_ID,item.ItemID, idCommunity, item.RoleEP, -1,
                                                                  idCreatedBy, TimeNow, UserIPaddress, UserProxyIPaddress)).ToList();
            Manager.SaveOrUpdateList<UnitCRoleAssignment>(CRoleAssignments);
        }

        public void SaveNewUnitPersonAssignmentsNoTransaction(List<dtoGenericAssWithOldRoleEpAndDelete> dtoPersonAssignments, Unit oUnit, DateTime TimeNow, Int32 idCreatedBy, String UserIPaddress, String UserProxyIPaddress)
        {
            List<UnitPersonAssignment> PersonAssignments = (from item in dtoPersonAssignments
                                                            where item.RoleEP > 0 && item.DB_ID == 0 && !item.isDeleted
                                                            select new UnitPersonAssignment(oUnit.Id,item.DB_ID, item.ItemID, item.RoleEP, -1,
                                                                    idCreatedBy, TimeNow, UserIPaddress, UserProxyIPaddress)).ToList();
            Manager.SaveOrUpdateList<UnitPersonAssignment>(PersonAssignments);
        }

        #endregion

        #region update assignment mincompletion
            public bool SetActivityAssignment_PersonalCompletion(long idActivity, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int16 personalCompletion, bool noTransaction)
            {
                DateTime currentTime = DateTime.Now;
                IList<ActivityPersonAssignment> personAssignments = Manager.GetAll<ActivityPersonAssignment>(a => a.IdActivity == idActivity && a.CreatedOn < currentTime && a.MinCompletion == -1);
                IList<ActivityCRoleAssignment> roleAssignments = Manager.GetAll<ActivityCRoleAssignment>(a => a.IdActivity == idActivity && a.CreatedOn < currentTime && a.MinCompletion == -1);
                if (noTransaction)
                {
                    UpdateActivityAssignmentsMinCompletionNoTransaction(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalCompletion);
                     return true;
                }
                else
                {
                    return UpdateActivityAssignmentsMinCompletion(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalCompletion);        
                }
            }

            public bool SetUnitAssignment_PersonalCompletion(long idUnit, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int16 personalCompletion, bool noTransaction)
            {
                DateTime currentTime = DateTime.Now;
                IList<UnitPersonAssignment> personAssignments = Manager.GetAll<UnitPersonAssignment>(a => a.IdUnit == idUnit && a.CreatedOn < currentTime && a.MinCompletion == -1);
                IList<UnitCRoleAssignment> roleAssignments = Manager.GetAll<UnitCRoleAssignment>(a => a.IdUnit == idUnit && a.CreatedOn < currentTime && a.MinCompletion == -1);
           
                if (noTransaction)
                {
                    UpdateUnitAssignmentsMinCompletionNoTransaction(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalCompletion);
                    return true;
                }
                else
                {
                    return UpdateUnitAssignmentsMinCompletion(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalCompletion);
                }
            }

            public bool SetPathAssignment_PersonalCompletion(long idPath, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int16 personalCompletion, bool noTransaction)
            {
                DateTime currentTime = DateTime.Now;
                IList<PathPersonAssignment> personAssignments = Manager.GetAll<PathPersonAssignment>(a => a.IdPath == idPath && a.CreatedOn < currentTime && a.MinCompletion == -1);
                IList<PathCRoleAssignment> roleAssignments = Manager.GetAll<PathCRoleAssignment>(a => a.IdPath == idPath && a.CreatedOn < currentTime && a.MinCompletion == -1);
                if (noTransaction)
                {
                    UpdatePathAssignmentsMinCompletionNoTransaction(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalCompletion);
                    return true;
                }
                else
                {
                    return UpdatePathAssignmentsMinCompletion(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalCompletion);
                }
            }

            private bool UpdateActivityAssignmentsMinCompletion(IList<ActivityPersonAssignment> personAssignments, IList<ActivityCRoleAssignment> roleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int64 MinCompletion)
            {
                try
                {
                    Manager.BeginTransaction();
                    UpdateActivityAssignmentsMinCompletionNoTransaction(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, MinCompletion);
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

            private void UpdateActivityAssignmentsMinCompletionNoTransaction(IList<ActivityPersonAssignment> personAssignments, IList<ActivityCRoleAssignment> roleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int64 MinCompletion)
            {
                DateTime currentTime = DateTime.Now;
                UpdateActivityPersonAssignmentsMinCompletionNoTransaction(personAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, MinCompletion);
                UpdateActivityCRoleAssignmentsMinCompletionNoTransaction(roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, MinCompletion);
            }

            private void UpdateActivityPersonAssignmentsMinCompletionNoTransaction(IList<ActivityPersonAssignment> personAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Int64 MinCompletion)
            {
                foreach (ActivityPersonAssignment item in personAssignments)
                {
                    item.MinCompletion = MinCompletion;
                    item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                    //SetActivityPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                    Manager.SaveOrUpdate<ActivityPersonAssignment>(item);
                }
            }

            private void UpdateActivityCRoleAssignmentsMinCompletionNoTransaction(IList<ActivityCRoleAssignment> roleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Int64 MinCompletion)
            {
                foreach (ActivityCRoleAssignment item in roleAssignments)
                {
                    item.MinCompletion = MinCompletion;
                    item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                    Manager.SaveOrUpdate<ActivityCRoleAssignment>(item);
                }
            }

        private bool UpdateUnitAssignmentsMinCompletion(IList<UnitPersonAssignment> PersonAssignments, IList<UnitCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int64 MinCompletion)
        {
            try
            {
                Manager.BeginTransaction();
                UpdateUnitAssignmentsMinCompletionNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, MinCompletion);
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

        private void UpdateUnitAssignmentsMinCompletionNoTransaction(IList<UnitPersonAssignment> PersonAssignments, IList<UnitCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int64 MinCompletion)
        {
            DateTime currentTime = DateTime.Now;
            UpdateUnitPersonAssignmentsMinCompletionNoTransaction(PersonAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, MinCompletion);
            UpdateUnitCRoleAssignmentsMinCompletionNoTransaction(CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, MinCompletion);
        }

        private void UpdateUnitPersonAssignmentsMinCompletionNoTransaction(IList<UnitPersonAssignment> PersonAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Int64 MinCompletion)
        {
            foreach (UnitPersonAssignment item in PersonAssignments)
            {
                item.MinCompletion = MinCompletion;
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetUnitPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<UnitPersonAssignment>(item);
            }
        }

        private void UpdateUnitCRoleAssignmentsMinCompletionNoTransaction(IList<UnitCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Int64 MinCompletion)
        {
            foreach (UnitCRoleAssignment item in CRoleAssignments)
            {
                item.MinCompletion = MinCompletion;
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetUnitCRoleAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<UnitCRoleAssignment>(item);
            }
        }

        private bool UpdatePathAssignmentsMinCompletion(IList<PathPersonAssignment> PersonAssignments, IList<PathCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int64 MinCompletion)
        {
            try
            {
                Manager.BeginTransaction();
                UpdatePathAssignmentsMinCompletionNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, MinCompletion);
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

        private void UpdatePathAssignmentsMinCompletionNoTransaction(IList<PathPersonAssignment> PersonAssignments, IList<PathCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Int64 MinCompletion)
        {
            DateTime currentTime = DateTime.Now;
            UpdatePathPersonAssignmentsMinCompletionNoTransaction(PersonAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, MinCompletion);
            UpdatePathCRoleAssignmentsMinCompletionNoTransaction(CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, MinCompletion);
        }

        private void UpdatePathPersonAssignmentsMinCompletionNoTransaction(IList<PathPersonAssignment> PersonAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Int64 MinCompletion)
        {
            foreach (PathPersonAssignment item in PersonAssignments)
            {
                item.MinCompletion = MinCompletion;
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetPathPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<PathPersonAssignment>(item);
            }
        }

        private void UpdatePathCRoleAssignmentsMinCompletionNoTransaction(IList<PathCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Int64 MinCompletion)
        {
            foreach (PathCRoleAssignment item in CRoleAssignments)
            {
                item.MinCompletion = MinCompletion;
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetPathCRoleAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<PathCRoleAssignment>(item);
            }
        }
        #endregion

        #region modify metainfo
        //private void SetPathPersonAssModifyMetaInfo(PathPersonAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetPathCRoleAssModifyMetaInfo(PathCRoleAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //private void SetUnitPersonAssModifyMetaInfo(UnitPersonAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetUnitCRoleAssModifyMetaInfo(UnitCRoleAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //private void SetActivityPersonAssModifyMetaInfo(ActivityPersonAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetActivityCRoleAssModifyMetaInfo(ActivityCRoleAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}

        //private void SetSubActivityPersonAssModifyMetaInfo(SubActivityPersonAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        //private void SetSubActivityCRoleAssModifyMetaInfo(SubActivityCRoleAssignment oAssignment, Person AuthorOfModify, String PersonIpAddress, String PersonProxyIpAddress, DateTime CurrentTime)
        //{
        //    oAssignment.ModifiedBy = AuthorOfModify;
        //    oAssignment.ModifiedIpAddress = PersonIpAddress;
        //    oAssignment.ModifiedOn = CurrentTime;
        //    oAssignment.ModifiedProxyIpAddress = PersonProxyIpAddress;
        //}
        #endregion

        #region Get

        public int GetAllParticipantInPathCount(long idItem, int idCommunity, ItemType itemType, DateTime viewStatBefore, Boolean all = false)
        {
            long idPath=0;
            int participantCount = 0;
            switch (itemType)
            {
                case ItemType.Path:
                    idPath = idItem;
                    break;
                case ItemType.Unit:
                    idPath = ServiceEp.GetPathId_ByUnitId(idItem);
                    break;
                case ItemType.Activity:
                    idPath = ServiceEp.GetPathId_ByActivityId(idItem);
                    break;
                case ItemType.SubActivity:
                    idPath = ServiceEp.GetPathId_BySubActivityId(idItem);
                    break;              
            }

            List<Int32> partecipantIds =(from a in Manager.GetIQ<PathPersonAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                    where ServiceEp.CheckRoleEp(a.RoleEP, RoleEP.Participant) || all
                                    select a.IdPerson).ToList();
            IList<int> idRoles = (from a in Manager.GetIQ<PathCRoleAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                   where ServiceEp.CheckRoleEp(a.RoleEP, RoleEP.Participant) || all
                                   select a.IdRole).ToList();
            
            foreach (int roleId in idRoles.Distinct())
            {
                partecipantIds = partecipantIds.Union(Manager.GetIdPerson_ByIdRole(roleId, idCommunity)).ToList();
            }
            if (all)
            {
                IList<Int32> idUsersAS = GetUsersWithStat(idPath, viewStatBefore);
                partecipantIds = partecipantIds.Union(idUsersAS).ToList();
            }
            participantCount = partecipantIds.Distinct().Count();
            return participantCount;
        }
      

        public List<int> GetAllParticipantIdInPath(long idItem, int idCommunity, ItemType itemType,DateTime viewStatBefore, Boolean all=false)
        {
            long idPath =0;
            List<int> idUsers = new List<int>();
            switch (itemType)         
            {
                case ItemType.Path:
                    idPath = idItem;
                    break;
                case ItemType.Unit:
                    idPath = ServiceEp.GetPathId_ByUnitId(idItem);
                    break;
                case ItemType.Activity:
                    idPath = ServiceEp.GetPathId_ByActivityId(idItem);
                    break;
                case ItemType.SubActivity:
                    idPath = ServiceEp.GetPathId_BySubActivityId(idItem);
                    break;
                default:
                    return idUsers;
            }
            if (idPath <= 0)
            {
                return idUsers;
            }

            idUsers = (from a in Manager.GetIQ<PathPersonAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                              where ServiceEp.CheckRoleEp(a.RoleEP, RoleEP.Participant) || all
                                              select a.IdPerson).ToList();
            IList<int> idRoles = (from a in Manager.GetIQ<PathCRoleAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                    where ServiceEp.CheckRoleEp(a.RoleEP, RoleEP.Participant) || all
                                    select a.IdRole).ToList();
          
            foreach (int roleId in idRoles.Distinct())
            {
               idUsers =  idUsers.Union(Manager.GetIdPerson_ByIdRole(roleId,idCommunity)).ToList();
            }
            if (all)
            {
                IList<Int32> idUsersAS = GetUsersWithStat(idPath, viewStatBefore);
                idUsers = idUsers.Union(idUsersAS).ToList();
            }
            return idUsers.Distinct().ToList();
        }

        private List<Int32> GetUsersWithStat(Int64 idPath, DateTime viewStatBefore, DateTime? viewStatAfter = null)
        {
            DateTime actualViewStatAfter = DateTimeExt.ValueOrMinDBTimeValue(viewStatAfter);

            return (from stat in Manager.GetIQ<liteBaseStatistic>()
                     where stat.IdPath == idPath && stat.Deleted == BaseStatusDeleted.None && stat.CreatedOn > actualViewStatAfter && stat.CreatedOn <= viewStatBefore
                     select stat.IdPerson).Distinct().ToList();
        }


        public IList<litePerson> GetAllParticipantInPath(long itemId, int idCommunity, ItemType itemType)
        {
            long idPath;
            IList<litePerson> participantsList = new List<litePerson>();
            switch (itemType)
            {
                case ItemType.Path:
                    idPath = itemId;
                    break;
                case ItemType.Unit:
                    idPath = ServiceEp.GetPathId_ByUnitId(itemId);
                    break;
                case ItemType.Activity:
                    idPath = ServiceEp.GetPathId_ByActivityId(itemId);
                    break;
                case ItemType.SubActivity:
                    idPath = ServiceEp.GetPathId_BySubActivityId(itemId);
                    break;
                default:
                    return participantsList;
            }
            if (idPath <= 0)
            {
                return participantsList;
            }
            List<Int32> idPersons =(from a in Manager.GetIQ<PathPersonAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                  where ServiceEp.CheckRoleEp(a.RoleEP, RoleEP.Participant)
                                  select a.IdPerson).ToList();
            List<Int32> idRoles = (from a in Manager.GetIQ<PathCRoleAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                   where ServiceEp.CheckRoleEp(a.RoleEP, RoleEP.Participant)
                                   select a.IdRole).ToList();

            foreach (int idRole in idRoles)
            {
                idPersons.AddRange(Manager.GetIdPerson_ByIdRole(idRole, idCommunity));
            }
            return Manager.GetLitePersons(idPersons.Distinct().ToList());
        }

        /// COMMENTATO IL 01/10/2015
        //public IEnumerable<PathPersonAssignment> GetAllActivePersonAssignmentsInPath(long idPath, int idPerson)
        //{
        //    Status pathStatus = (from p in Manager.GetIQ<Path>() where p.Id == idPath select p.Status).DefaultIfEmpty(Status.Draft).Skip(0).Take(1).ToList().FirstOrDefault();
        //    Func<PathPersonAssignment, bool> PathPersonConditionRestrict;
        //    Expression<Func<PathPersonAssignment, bool>> PathPersonConditionAll = a => a.IdPath == idPath && a.IdPerson == idPerson;

        //    PathPersonConditionRestrict = a => a.IdPath == idPath && a.IdPerson == idPerson && (a.RoleEP & RoleEP.Participant) == RoleEP.Participant
        //      && a.Deleted == BaseStatusDeleted.None  && !((ass.Path.Status & Status.Draft) == Status.Draft);

        //    IList<PathPersonAssignment> AllPersonAssignments = Manager.GetAll<PathPersonAssignment>(PathPersonConditionAll).ToList();

        //    return AllPersonAssignments.Where(PathPersonConditionRestrict);
        //}

        //public IEnumerable<PathCRoleAssignment> GetAllActiveCRoleAssignmentsInPath(long PathId, int CRoleId)
        //{
        //    Expression<Func<PathCRoleAssignment, bool>> PathCRoleConditionAll = ass => ass.Path.Id == PathId && ass.Path.Deleted == BaseStatusDeleted.None && ass.Role.Id == CRoleId;
        //    Func<PathCRoleAssignment, bool> PathCRoleConditionRestrict = ass => ass.Path.Id == PathId && ass.Role.Id == CRoleId && (ass.RoleEP & RoleEP.Participant) == RoleEP.Participant
        //     && ass.Deleted == BaseStatusDeleted.None && ass.Path.Deleted == BaseStatusDeleted.None  && !((ass.Path.Status & Status.Draft) == Status.Draft);

        //    IList<PathCRoleAssignment> AllCRoloAssignments = Manager.GetAll<PathCRoleAssignment>(PathCRoleConditionAll).ToList();
        //    return AllCRoloAssignments.Where(PathCRoleConditionRestrict);
        //}

        public List<dtoGenericAssignmentWithDelete> GetPathPersonAssignments(long idPath)
        {
            List<dtoGenericAssignmentWithDelete> results = (from a in Manager.GetIQ<PathPersonAssignment>() where a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None
                                                            select new dtoGenericAssignmentWithDelete(a.Id, a.IdPerson, "", a.RoleEP, false)).ToList();
            Dictionary<Int32, String> users = Manager.GetLitePersons(results.Select(r => r.ItemID).Distinct().ToList()).ToDictionary(p => p.Id, p => p.SurnameAndName);
            results.ForEach(r => r.ItemName = (users.ContainsKey(r.ItemID) ? users[r.ItemID] : ""));
            return results;


            //return (from item in Manager<PathPersonAssignment>(a => a.IdPath == idPath && a.Deleted == 0)
            //        select new dtoGenericAssignmentWithDelete(item.Id, item.Person.Id, item.Person.Surname + " " + item.Person.Name, item.RoleEP, false)).ToList();
        }

        public List<int> GetIdFromDtoAssignementGenericWithDelete(List<dtoGenericAssignmentWithDelete> InputList)
        {
            return (from item in InputList select item.ItemID).ToList();
        }

        public List<int> GetIdFromDtoAssignementGenericWithOldRoleEP(List<dtoGenericAssignmentWithOldRoleEP> InputList)
        {
            return (from item in InputList select item.ItemID).ToList();
        }
        public List<int> GetIdFromDtoAssignementGenericWithOldRoleEP(List<dtoGenericAssWithOldRoleEpAndDelete> InputList)
        {
            return (from item in InputList select item.ItemID).ToList();
        }
        public List<dtoGenericAssignment> GetListCRolePathAssignment(long idPath, int idCommunity, int UserID, int idLanguage)
        {
            List<dtoGenericAssignment> ListCRole;
            List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> tRoles = Manager.GetTranslatedRoles(idCommunity,idLanguage);
            COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription oMS = new COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription(UserID, idCommunity, idLanguage);
            List<int> MyComm = new List<int>();
            MyComm.Add(idCommunity);

            ListCRole = (from r in tRoles select new dtoGenericAssignment(r.Id, r.Name)).ToList<dtoGenericAssignment>();
            if (idPath != 0)
            {
                List<PathCRoleAssignment> PathAssignments = Manager.GetAll<PathCRoleAssignment>(item => item.IdPath == idPath && item.Deleted == 0).OrderBy(item => item.RoleEP).ToList();
                List<dtoGenericAssignment> ExistingAssignment = (from r in PathAssignments
                                                                 select new dtoGenericAssignment(r.Id, r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList();
                return ExistingAssignment.Union(ListCRole, new dtoGenericAssignmentItemIDcomparer()).ToList();
            }
            return ListCRole;
        }

        public List<dtoGenericAssignmentWithDelete> GetDtoAssignemtGenericWithDeleteFromMembers(List<Entity.MemberContact> ListOfMember)
        {
            List<dtoGenericAssignmentWithDelete> List;
            List = (from item in ListOfMember select new dtoGenericAssignmentWithDelete(item.Id, (item.Name + " " + item.Surname))).ToList<dtoGenericAssignmentWithDelete>();
            return List;
        }

        public List<dtoGenericAssignment> GetActiveAssignment(List<dtoGenericAssWithOldRoleEpAndDelete> ListAssignment)
        {
            return (from item in ListAssignment where !item.isDeleted && item.RoleEP > 0 select new dtoGenericAssignment(item.ItemID, item.ItemName, item.RoleEP)).ToList();
        }

        public List<dtoGenericAssignment> GetActiveAssignment(List<dtoGenericAssignmentWithDelete> ListAssignment)
        {
            return (from item in ListAssignment where !item.isDeleted && item.RoleEP > 0 select new dtoGenericAssignment(item.ItemID, item.ItemName, item.RoleEP)).ToList();
        }

        public List<dtoGenericAssignment> GetActiveAssignment(List<dtoGenericAssignment> ListAssignment)
        {
            return (from item in ListAssignment where item.RoleEP > 0 select item).ToList();
        }
        public List<dtoGenericAssignment> GetActiveAssignment(List<dtoGenericAssignmentWithOldRoleEP> ListAssignment)
        {
            return (from item in ListAssignment where item.RoleEP > 0 select new dtoGenericAssignment(item.ItemID, item.ItemName, item.RoleEP)).ToList();
        }

        public List<dtoGenericAssignmentWithOldRoleEP> GetDtoAssignemtGenericWithOldRoleEPFromMembers(List<Entity.MemberContact> ListOfMember)
        {
            List<dtoGenericAssignmentWithOldRoleEP> List;
            List = (from item in ListOfMember select new dtoGenericAssignmentWithOldRoleEP(item.Id, (item.Name + " " + item.Surname))).ToList<dtoGenericAssignmentWithOldRoleEP>();
            return List;
        }

        public List<dtoGenericAssWithOldRoleEpAndDelete> GetDtoAssignemtGenericWithOldRoleEPAndDelFromMembers(List<Entity.MemberContact> ListOfMember)
        {
            List<dtoGenericAssWithOldRoleEpAndDelete> List;
            List = (from item in ListOfMember select new dtoGenericAssWithOldRoleEpAndDelete(item.Id, (item.Name + " " + item.Surname))).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
            return List;
        }

        public List<dtoGenericAssignmentWithOldRoleEP> GetListCRoleUnitAssignment(long idUnit, long idPath, int idCommunity, int UserId, int idLanguage)
        {
            List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> tRoles = Manager.GetTranslatedRoles(idCommunity, idLanguage);
            List<dtoGenericAssignmentWithOldRoleEP> ListCRole;
            COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription oMS = new COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription(UserId, idCommunity, idLanguage);
            List<int> MyComm = new List<int>();
            MyComm.Add(idCommunity);

            ListCRole = (from r in tRoles select new dtoGenericAssignmentWithOldRoleEP(r.Id, r.Name)).ToList<dtoGenericAssignmentWithOldRoleEP>();
            if (idUnit == 0)
            {
                List<dtoGenericAssignmentWithOldRoleEP> ListAssignedPathCRole = (from r in Manager.GetAll<PathCRoleAssignment>(tt => tt.IdPath == idPath  && tt.Deleted == BaseStatusDeleted.None)
                                                                                 select new dtoGenericAssignmentWithOldRoleEP(r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();

                return ListAssignedPathCRole.Union(ListCRole, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
            }
            else
            {
                List<dtoGenericAssignmentWithOldRoleEP> PathAss = (from r in Manager.GetAll<PathCRoleAssignment>(tt => tt.IdPath == idPath  && tt.Deleted == BaseStatusDeleted.None)
                                                                   select new dtoGenericAssignmentWithOldRoleEP( r.IdRole, GetRoleName(tRoles, r.IdRole), RoleEP.None, r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
                List<dtoGenericAssignmentWithOldRoleEP> ExistingUnitAss = (from r in Manager.GetAll<UnitCRoleAssignment>(tt => tt.IdUnit == idUnit &&  tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
                                                                           select new dtoGenericAssignmentWithOldRoleEP(r.Id, r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();

                ExistingUnitAss = ExistingUnitAss.Union(PathAss, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
                return ExistingUnitAss.Union(ListCRole, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
            }
        }


        public List<dtoGenericAssignmentWithOldRoleEP> GetListCRoleUnitAssignment(long idUnit, Int32 idCommunity, Int32 idLanguage)
        {
            List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> tRoles = Manager.GetTranslatedRoles(idCommunity, idLanguage);
            return  (from r in Manager.GetAll<UnitCRoleAssignment>(tt => tt.IdUnit == idUnit  && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
                     select new dtoGenericAssignmentWithOldRoleEP(r.Id, r.IdRole, GetRoleName(tRoles,r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        
        }
        private String GetRoleName(List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> roles, Int32 idRole)
        {
            return roles.Where(r => r.Id == idRole).Select(r => r.Name).DefaultIfEmpty("").FirstOrDefault();
        }



        //public List<dtoGenericAssignmentWithOldRoleEP> GetListPersonUnitAssignment(long UnitID, long PathID)
        //{
        //    if (UnitID == 0)
        //    {
        //        return (from croleAss in Manager.GetAll<PathPersonAssignment>(tt => tt.Path.Id == PathID && tt.Path.Deleted == BaseStatusDeleted.None  && tt.Deleted == BaseStatusDeleted.None)
        //                select new dtoGenericAssignmentWithOldRoleEP(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        //    }
        //    else
        //    {
        //        List<dtoGenericAssignmentWithOldRoleEP> PathAss = (from croleAss in Manager.GetAll<PathPersonAssignment>(tt => tt.Path.Id == PathID && tt.Path.Deleted == BaseStatusDeleted.None  && tt.Deleted == BaseStatusDeleted.None)
        //                                                           select new dtoGenericAssignmentWithOldRoleEP(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, RoleEP.None, croleAss.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        //        List<dtoGenericAssignmentWithOldRoleEP> ExistingUnitAss = (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.Unit.Id == UnitID && tt.Unit.Deleted == BaseStatusDeleted.None && tt.RoleEP >= RoleEP.Evaluator && tt.Deleted == BaseStatusDeleted.None)
        //                                                                   select new dtoGenericAssignmentWithOldRoleEP(croleAss.Id, croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        //        return ExistingUnitAss.Union(PathAss, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
        //    }
        //}

        public List<dtoGenericAssWithOldRoleEpAndDelete> GetListPersonUnitAssignment(long idUnit, long idPath)
        {
            List<dtoGenericAssWithOldRoleEpAndDelete> results = null;
            List<dtoGenericAssWithOldRoleEpAndDelete> pathItems = (from a in Manager.GetIQ<PathPersonAssignment>()
                                                                     where a.Deleted == BaseStatusDeleted.None && a.IdPath == idPath
                                                                     select new dtoGenericAssWithOldRoleEpAndDelete(a.IdPerson, "", a.RoleEP)).ToList();
            if (idUnit > 0)
            {
                List<dtoGenericAssWithOldRoleEpAndDelete> unitItems = (from a in Manager.GetIQ<UnitPersonAssignment>()
                                                                       where a.Deleted == BaseStatusDeleted.None && a.IdUnit == idUnit
                                                                       select new dtoGenericAssWithOldRoleEpAndDelete(a.Id,a.IdPerson, "", a.RoleEP)).ToList();
                results = unitItems.Union(pathItems, new dtoGenericAssWithOldRoleEpAndDeleteItemIDcomparer()).ToList();
            }
            else
                results = pathItems;
            Dictionary<Int32, String> users = Manager.GetLitePersons(results.Select(r => r.ItemID).Distinct().ToList()).ToDictionary(p=> p.Id, p=> p.SurnameAndName);
            results.ForEach(r => r.ItemName = (users.ContainsKey(r.ItemID) ? users[r.ItemID] : ""));
            return results;
            //if (idUnit == 0)
            //{
            //    return (from croleAss in Manager.GetAll<PathPersonAssignment>(tt => tt.Path.Id == idPath && tt.Path.Deleted == BaseStatusDeleted.None && tt.Deleted == BaseStatusDeleted.None)
            //            select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
            //}
            //else
            //{
            //    //
            //    List<dtoGenericAssWithOldRoleEpAndDelete> PathAss = (from croleAss in Manager.GetAll<PathPersonAssignment>(tt => tt.Path.Id == idPath && tt.Path.Deleted == BaseStatusDeleted.None && tt.Deleted == BaseStatusDeleted.None)
            //                                                       select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, RoleEP.None, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
                
            //    List<dtoGenericAssWithOldRoleEpAndDelete> ExistingUnitAss = (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.Unit.Id == idUnit && tt.Unit.Deleted == BaseStatusDeleted.None && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
            //                                                               select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Id, croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
            //    return ExistingUnitAss.Union(PathAss, new dtoGenericAssWithOldRoleEpAndDeleteItemIDcomparer()).ToList();
            //}
        }


        /// <summary>
        /// Only Existing Assignment
        /// </summary>
        /// <param name="idUnit"></param>
        /// <param name="PathID"></param>
        /// <returns></returns>
        public List<dtoGenericAssWithOldRoleEpAndDelete> GetListPersonUnitAssignment(long idUnit)
        {
            List<dtoGenericAssWithOldRoleEpAndDelete> items = (from a in Manager.GetIQ<UnitPersonAssignment>()
                                                               where a.Deleted == BaseStatusDeleted.None && a.IdUnit == idUnit && a.RoleEP> RoleEP.None 
                                                               select new dtoGenericAssWithOldRoleEpAndDelete(a.Id,a.IdPerson, "", a.RoleEP)).ToList();

            Dictionary<Int32, String> users = Manager.GetLitePersons(items.Select(r => r.ItemID).Distinct().ToList()).ToDictionary(p => p.Id, p => p.SurnameAndName);
            items.ForEach(r => r.ItemName = (users.ContainsKey(r.ItemID) ? users[r.ItemID] : ""));
            return items;
            //return (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.IdUnit == idUnit && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
            //        select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Id, croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
        }

        /// <summary>
        /// Only Existing Assignment
        /// </summary>
        /// <param name="UnitID"></param>
        /// <param name="PathID"></param>
        /// <returns></returns>
        public List<dtoGenericAssignmentWithOldRoleEP> GetListCRoleActivityAssignment(long idActivity, Int32 idCommunity, Int32 idLanguage)
        {
            List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> tRoles = Manager.GetTranslatedRoles(idCommunity, idLanguage);
            return (from r in Manager.GetAll<ActivityCRoleAssignment>(tt => tt.IdActivity == idActivity  && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
                    select new dtoGenericAssignmentWithOldRoleEP(r.Id, r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        }

        public List<dtoGenericAssignmentWithOldRoleEP> GetListCRoleActivityAssignment(long idActivity, long idUnit, int idCommunity, int UserId, int idLanguage)
        {
            List<dtoGenericAssignmentWithOldRoleEP> ListCRole;
            COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription oMS = new COL_BusinessLogic_v2.Comol.Manager.ManagerSubscription(UserId, idCommunity, idLanguage);
            List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> tRoles = Manager.GetTranslatedRoles(idCommunity,idLanguage);

            List<int> MyComm = new List<int>();
            MyComm.Add(idCommunity);

            ListCRole = (from r in tRoles select new dtoGenericAssignmentWithOldRoleEP(r.Id, r.Name)).ToList<dtoGenericAssignmentWithOldRoleEP>();
            if (idActivity == 0)
            {
                List<dtoGenericAssignmentWithOldRoleEP> ListAssignedUnitCRole = (from r in Manager.GetAll<UnitCRoleAssignment>(tt => tt.IdUnit == idUnit && tt.Deleted == BaseStatusDeleted.None)
                                                                                 select new dtoGenericAssignmentWithOldRoleEP(r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();

                return ListAssignedUnitCRole.Union(ListCRole, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
            }
            else
            {
                List<dtoGenericAssignmentWithOldRoleEP> UnitAss = (from r in Manager.GetAll<UnitCRoleAssignment>(tt => tt.IdUnit == idUnit && tt.Deleted == BaseStatusDeleted.None && tt.RoleEP > RoleEP.None)
                                                                   select new dtoGenericAssignmentWithOldRoleEP(r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
                List<dtoGenericAssignmentWithOldRoleEP> ExistingActivitytAss = (from r in Manager.GetAll<ActivityCRoleAssignment>(tt => tt.IdActivity == idActivity && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
                                                                                select new dtoGenericAssignmentWithOldRoleEP(r.Id, r.IdRole, GetRoleName(tRoles, r.IdRole), r.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();

               ExistingActivitytAss = ExistingActivitytAss.Union(UnitAss, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
                return ExistingActivitytAss.Union(ListCRole, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
            }
        }

        //public List<dtoGenericAssignmentWithOldRoleEP> GetListPersonActivityAssignment(long ActivityId, long UnitId)
        //{
        //    if (ActivityId == 0)
        //    {
        //        return (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.Unit.Id == UnitId && tt.Unit.Deleted == BaseStatusDeleted.None && tt.RoleEP >= RoleEP.Evaluator && tt.Deleted == BaseStatusDeleted.None)
        //                select new dtoGenericAssignmentWithOldRoleEP(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        //    }
        //    else
        //    {
        //        List<dtoGenericAssignmentWithOldRoleEP> UnitAss = (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.Unit.Id == UnitId && tt.Unit.Deleted == BaseStatusDeleted.None && tt.RoleEP >= RoleEP.Evaluator && tt.Deleted == BaseStatusDeleted.None)
        //                                                           select new dtoGenericAssignmentWithOldRoleEP(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, RoleEP.None, croleAss.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        //        List<dtoGenericAssignmentWithOldRoleEP> ExistingActivityAss = (from croleAss in Manager.GetAll<ActivityPersonAssignment>(tt => tt.Activity.Id == ActivityId && tt.Activity.Deleted == BaseStatusDeleted.None && tt.RoleEP >= RoleEP.Evaluator && tt.Deleted == BaseStatusDeleted.None)
        //                                                                       select new dtoGenericAssignmentWithOldRoleEP(croleAss.Id, croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssignmentWithOldRoleEP>();
        //        return ExistingActivityAss.Union(UnitAss, new dtoGenericAssignmentWithOldRoleEPItemIDcomparer()).ToList();
        //    }
        //}

        public List<dtoGenericAssWithOldRoleEpAndDelete> GetListPersonActivityAssignment(long idActivity, long idUnit)
        {

            List<dtoGenericAssWithOldRoleEpAndDelete> results = null;
            List<dtoGenericAssWithOldRoleEpAndDelete> unitItems = (from a in Manager.GetIQ<UnitPersonAssignment>()
                                                                   where a.Deleted == BaseStatusDeleted.None && a.IdUnit == idUnit
                                                                   select new dtoGenericAssWithOldRoleEpAndDelete(a.IdPerson, "", a.RoleEP)).ToList();
            if (idUnit > 0)
            {
                List<dtoGenericAssWithOldRoleEpAndDelete> activityItems = (from a in Manager.GetIQ<UnitPersonAssignment>()
                                                                           where a.Deleted == BaseStatusDeleted.None && a.IdUnit == idActivity
                                                                       select new dtoGenericAssWithOldRoleEpAndDelete(a.Id,a.IdPerson, "", a.RoleEP)).ToList();
                results = activityItems.Union(unitItems, new dtoGenericAssWithOldRoleEpAndDeleteItemIDcomparer()).ToList();
            }
            else
                results = unitItems;
            Dictionary<Int32, String> users = Manager.GetLitePersons(results.Select(r => r.ItemID).Distinct().ToList()).ToDictionary(p => p.Id, p => p.SurnameAndName);
            results.ForEach(r => r.ItemName = (users.ContainsKey(r.ItemID) ? users[r.ItemID] : ""));
            return results;


            //if (idActivity == 0)
            //{
            //    return (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.IdUnit == idUnit && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
            //            select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
            //}
            //else
            //{
            //    List<dtoGenericAssWithOldRoleEpAndDelete> UnitAss = (from croleAss in Manager.GetAll<UnitPersonAssignment>(tt => tt.Unit.Id == idUnit && tt.Unit.Deleted == BaseStatusDeleted.None && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
            //                                                         select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, RoleEP.None, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
            //    List<dtoGenericAssWithOldRoleEpAndDelete> ExistingActivityAss = (from croleAss in Manager.GetAll<ActivityPersonAssignment>(tt => tt.Activity.Id == idActivity && tt.Activity.Deleted == BaseStatusDeleted.None && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
            //                                                                     select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Id, croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
            //    return ExistingActivityAss.Union(UnitAss, new dtoGenericAssWithOldRoleEpAndDeleteItemIDcomparer()).ToList();
            //}
        }

        public List<dtoGenericAssWithOldRoleEpAndDelete> GetListPersonActivityAssignment(long idActivity)
        {
            List<dtoGenericAssWithOldRoleEpAndDelete> items = (from a in Manager.GetIQ<ActivityPersonAssignment>()
                                                               where a.Deleted == BaseStatusDeleted.None && a.IdActivity == idActivity && a.RoleEP > RoleEP.None
                                                               select new dtoGenericAssWithOldRoleEpAndDelete(a.Id, a.IdPerson, "", a.RoleEP)).ToList();

            Dictionary<Int32, String> users = Manager.GetLitePersons(items.Select(r => r.ItemID).Distinct().ToList()).ToDictionary(p => p.Id, p => p.SurnameAndName);
            items.ForEach(r => r.ItemName = (users.ContainsKey(r.ItemID) ? users[r.ItemID] : ""));
            return items;
            //return (from croleAss in Manager.GetAll<ActivityPersonAssignment>(tt => tt.Activity.Id == ActivityId && tt.Activity.Deleted == BaseStatusDeleted.None && tt.RoleEP > RoleEP.None && tt.Deleted == BaseStatusDeleted.None)
            //        select new dtoGenericAssWithOldRoleEpAndDelete(croleAss.Id, croleAss.Person.Id, croleAss.Person.Surname + " " + croleAss.Person.Name, croleAss.RoleEP)).ToList<dtoGenericAssWithOldRoleEpAndDelete>();
        }


        #endregion
               
        #region Get ItemId by Assignment
        public IList<long> GetActivitiesId_ByPersonAssignment(long idPath, int idUser, RoleEP roleToVerify)
            {
                //List<long> idActivities = (from a in Manager.GetIQ<Activity>()
                //                           where a.Path != null && a.Path.Id == idPath && a.Deleted == BaseStatusDeleted.None
                //                           && !a.CheckStatus(Status.Draft) && !a.CheckStatus(Status.Text)
                //                           select a.Id).ToList();
                //List<long> idActivities = (from i in Manager.GetAll<Activity>(a=>a.Path != null && a.Path.Id == idPath && a.Deleted == BaseStatusDeleted.None).ToList()
                //                           where !i.CheckStatus(Status.Draft) && !i.CheckStatus(Status.Text)
                //                           select i.Id).ToList();
                List<long> idActivities = (from a in Manager.GetIQ<Activity>()
                                           where a.Path != null && a.Path.Id == idPath && a.Deleted == BaseStatusDeleted.None
                                           select new { IdActivity = a.Id, Status = a.Status }).ToList().Where(i => !ServiceEp.CheckStatus(i.Status, Status.Draft) && !ServiceEp.CheckStatus(i.Status, Status.Text)).Select(i => i.IdActivity).Distinct().ToList();
                //List<ActivityPersonAssignment> assignments = (from a in Manager.GetIQ<ActivityPersonAssignment>()
                //                                              where a.Deleted == BaseStatusDeleted.None && a.IdPerson == idUser && idActivities.Contains(a.IdActivity)
                //                                             select a).ToList();

                //return assignments.Where(a => (a.RoleEP & RoleEpToVerify) == RoleEpToVerify).Select(a => a.IdActivity).Distinct().ToList();
                return GetActivitiesId_ByPersonAssignment(idActivities, idUser, roleToVerify);
            }
            public List<long> GetActivitiesId_ByPersonAssignment(List<long> idActivities, int idUser, RoleEP roleToVerify)
            {
                //List<ActivityPersonAssignment> assignments = (from a in Manager.GetIQ<ActivityPersonAssignment>()
                //                                              where a.Deleted == BaseStatusDeleted.None && a.IdPerson == idUser && idActivities.Contains(a.IdActivity)
                //                                              select a).ToList();

                return (from a in Manager.Linq<ActivityPersonAssignment>()
                        where a.Deleted == BaseStatusDeleted.None && a.IdPerson == idUser && idActivities.Contains(a.IdActivity)
                        select new { IdActivity = a.Id, Role = a.RoleEP }).ToList().Where(a => (a.Role & roleToVerify) == roleToVerify).Select(a => a.IdActivity).Distinct().ToList();
            }
            public IList<long> GetActivitiesId_ByCRoleAssignment(long idPath, int idRole, RoleEP roleToVerify)
            {
                List<long> idActivities = (from a in Manager.GetIQ<Activity>()
                                           where a.Path != null && a.Path.Id == idPath && a.Deleted == BaseStatusDeleted.None
                                           select new { IdActivity = a.Id, Status = a.Status }).ToList().Where(i => !ServiceEp.CheckStatus(i.Status, Status.Draft) && !ServiceEp.CheckStatus(i.Status, Status.Text)).Select(i => i.IdActivity).Distinct().ToList();
                //List<long> idActivities = (from i in Manager.GetAll<Activity>(a=> a.Path != null && a.Path.Id == idPath && a.Deleted == BaseStatusDeleted.None).ToList()
                //                           where !i.CheckStatus(Status.Draft) && !i.CheckStatus(Status.Text)
                //                           select i.Id).ToList();
                return GetActivitiesId_ByCRoleAssignment(idActivities, idRole, roleToVerify);
            }
            public List<long> GetActivitiesId_ByCRoleAssignment(List<long> idActivities, int idRole, RoleEP roleToVerify)
            {
                //List<ActivityCRoleAssignment> assignments = (from a in Manager.GetIQ<ActivityCRoleAssignment>()
                //                                             where a.Deleted == BaseStatusDeleted.None && a.IdRole == idRole && idActivities.Contains(a.IdActivity)
                //                                             select a).ToList();
                //return assignments.Where(a => (a.RoleEP & roleToVerify) == roleToVerify).Select(a => a.IdActivity).Distinct().ToList();

                return (from a in Manager.Linq<ActivityCRoleAssignment>()
                        where a.Deleted == BaseStatusDeleted.None && a.IdRole == idRole && idActivities.Contains(a.IdActivity)
                        select new { IdActivity = a.Id, Role = a.RoleEP }).ToList().Where(a => (a.Role & roleToVerify) == roleToVerify).Select(a => a.IdActivity).Distinct().ToList();
            }
            /// <summary>
            /// no Text note
            /// </summary>
            /// <param name="idPath"></param>
            /// <param name="idUser"></param>
            /// <param name="roleToVerify"></param>
            /// <returns></returns>
            public IList<long> GetUnitsId_ByPersonAssignment(long idPath, int idPerson, RoleEP roleToVerify)
            {
                List<long> idUnits = (from u in Manager.Linq<Unit>() where u.Deleted == BaseStatusDeleted.None && u.ParentPath != null && u.ParentPath.Id == idPath 
                                      select  new {IdUnit=u.Id, Status= u.Status}).ToList().Where(i=> !ServiceEp.CheckStatus(i.Status,Status.Draft) && !ServiceEp.CheckStatus(i.Status,Status.Text)).Select(i=> i.IdUnit).Distinct().ToList();
                return GetUnitsId_ByPersonAssignment(idUnits, idPerson, roleToVerify);
                //return (from ass in UnitPersonAss
                //        where !ass.Unit.CheckStatus(Status.Draft) && !ass.Unit.CheckStatus(Status.Text) &&(ass.RoleEP & RoleEpToVerify) == RoleEpToVerify 
                //        select ass.Unit.Id).ToList();
            }
            public List<long> GetUnitsId_ByPersonAssignment(List<long> idUnits, int idPerson, RoleEP roleToVerify)
            {
                return (from a in Manager.Linq<UnitPersonAssignment>()
                        where a.Deleted == BaseStatusDeleted.None && a.IdPerson == idPerson && idUnits.Contains(a.IdUnit)
                        select new { IdUnit = a.Id, Role = a.RoleEP }).ToList().Where(a => (a.Role & roleToVerify) == roleToVerify).Select(a => a.IdUnit).Distinct().ToList();
            }
            /// <summary>
            /// no Text note
            /// </summary>
            /// <param name="idPath"></param>
            /// <param name="UserId"></param>
            /// <param name="roleToVerify"></param>
            /// <returns></returns>
            public IList<long> GetUnitsId_ByCRoleAssignment(long idPath, int idRole, RoleEP roleToVerify)
            {
                List<long> idUnits = (from u in Manager.Linq<Unit>()
                                      where u.Deleted == BaseStatusDeleted.None && u.ParentPath != null && u.ParentPath.Id == idPath 
                                      select new {IdUnit=u.Id, Status= u.Status}).ToList().Where(i=> !ServiceEp.CheckStatus(i.Status,Status.Draft) && !ServiceEp.CheckStatus(i.Status,Status.Text)).Select(i=> i.IdUnit).Distinct().ToList();
                return GetUnitsId_ByCRoleAssignment(idUnits, idRole, roleToVerify);
            }
            public List<long> GetUnitsId_ByCRoleAssignment(List<long> idUnits, int idRole, RoleEP roleToVerify)
            {
                var x = (from a in Manager.GetIQ<UnitCRoleAssignment>()
                        where a.Deleted == BaseStatusDeleted.None && idUnits.Contains(a.IdUnit) && a.IdRole == idRole
                        select new {IdUnit=a.Id, Role= a.RoleEP}).ToList();

                var y = x.Where(i=>ServiceEp.CheckRoleEp(i.Role,roleToVerify)).Select(i=> i.IdUnit).Distinct().ToList();

                return y.ToList();
            }
        #endregion

        #region Get RoleEp in Active Assignments

        private RoleEP GetUserRole_ByUnitPersonAss(long UnitId, int UserId, RoleEP MinRoleEp)
        {
            IList<RoleEP> RolesEP = (from a in
                                         Manager.GetIQ<UnitPersonAssignment>()
                                     where a.IdUnit == UnitId && a.Deleted == BaseStatusDeleted.None && a.IdPerson == UserId && a.RoleEP >= MinRoleEp
                                     select a.RoleEP).ToList();
            if (RolesEP.Count > 0)
            {
                return RolesEP[0];
            }
            return RoleEP.None;
        }

        private RoleEP GetUserRole_ByUnitRoleAssignment(long idUnit, int idRole, RoleEP MinRoleEp)
        {
            IList<RoleEP> RolesEP = (from a in Manager.GetIQ<UnitCRoleAssignment>( )
                                     where a.IdUnit == idUnit && a.Deleted == BaseStatusDeleted.None && a.IdRole == idRole && a.RoleEP >= MinRoleEp
                                     select a.RoleEP).ToList();
            if (RolesEP.Count > 0)
            {
                return RolesEP[0];
            }
            return RoleEP.None;
        }

        public RoleEP GetUserRole_ByUnitPersonAss(long UnitId, int UserId)
        {
            return GetUserRole_ByUnitPersonAss(UnitId, UserId, RoleEP.None);
        }

        public RoleEP GetUserRole_ByUnitRoleAssignment(long idUnit, int idRole)
        {
            return GetUserRole_ByUnitRoleAssignment(idUnit, idRole, RoleEP.None);
        }

        public RoleEP GetUserRole_ByActivityRoleAssignment(long idActivity, int idRole, Boolean skipDraft)
        {
            RoleEP result = RoleEP.None;
            if (!skipDraft || ServiceEp.GetActivityStatus(idActivity) != Status.Draft)
            {
                //IList<RoleEP> roles = (from a in Manager.GetIQ<ActivityCRoleAssignment>()
                //                       where a.IdActivity == idActivity && a.IdRole == idRole && a.Deleted == BaseStatusDeleted.None
                //                         && !ServiceEp.CheckStatusAssignment(a.StatusAssignment, StatusAssignment.Locked)
                //                        select a.RoleEP).ToList();
                IList<RoleEP> roles = (from i in Manager.GetAll<ActivityCRoleAssignment>(a=> a.IdActivity == idActivity && a.IdRole == idRole && a.Deleted == BaseStatusDeleted.None).ToList()
                                       where !ServiceEp.CheckStatusAssignment(i.StatusAssignment, StatusAssignment.Locked)
                                       select i.RoleEP).ToList();
                if (roles.Count > 0)
                    result = roles.FirstOrDefault();

                if (ServiceEp.CheckRoleEp(GetUserRole_ByPathRoleAssignment(ServiceEp.GetPathId_ByActivityId(idActivity), idRole), RoleEP.Participant))
                {
                    result |= RoleEP.Participant;
                }
            }
            return result;
        }

        public RoleEP GetUserRole_BySubActivityRoleAssignment(long subActivityId, int idRole, Boolean skipDraft)
        {
            return GetUserRole_ByActivityRoleAssignment(ServiceEp.GetIdFaher(subActivityId, ItemType.SubActivity), idRole, skipDraft);
        }
        public RoleEP GetUserRole_ByActivityRoleAssignment(long idActivity, int idRole)
        {
            return GetUserRole_ByActivityRoleAssignment(idActivity, idRole, true);
        }


        public RoleEP GetUserRole_ByActivityPersonAssignment(long idActivity, int idUser, Boolean skipDraftActivity)
        {
            RoleEP result = RoleEP.None;
            if (!skipDraftActivity || ServiceEp.GetActivityStatus(idActivity) != Status.Draft)
            {
                //IList<RoleEP> roles = (from a in Manager.GetIQ<ActivityPersonAssignment>()
                //         where a.IdActivity==idActivity && a.IdPerson == idUser && a.Deleted == BaseStatusDeleted.None 
                //           && !ServiceEp.CheckStatusAssignment(a.StatusAssignment, StatusAssignment.Locked)
                //           select a.RoleEP).ToList();
                IList<RoleEP> roles = (from i in Manager.GetAll<ActivityPersonAssignment>(a=> a.IdActivity == idActivity && a.IdPerson == idUser && a.Deleted == BaseStatusDeleted.None).ToList()
                                       where !ServiceEp.CheckStatusAssignment(i.StatusAssignment, StatusAssignment.Locked)
                                       select i.RoleEP).ToList();
                if (roles.Count > 0)
                    result = roles[0];
                if (!ServiceEp.CheckRoleEp(result, RoleEP.Manager))
                    result |= ServiceEp.isCreatorOf(idUser, idActivity, ItemType.Activity) ? RoleEP.Manager : RoleEP.None;
            }
            return result;
        }
        public RoleEP GetUserRole_ByActivityPersonAssignment(long idActivity, int idUser)
        {
            return GetUserRole_ByActivityPersonAssignment(idActivity, idUser, true);
        }

        public RoleEP GetUserRole_ByPathPersonAssignment(long idPath, Status pathStatus, int idUser)
        {
            //IList<RoleEP> RolesEP = (from ass in Manager.GetAll<PathPersonAssignment>(a => a.Path.Id == PathId && a.Path.Deleted == BaseStatusDeleted.None && a.Person.Id == UserId && a.Deleted == BaseStatusDeleted.None && a.Path.Deleted == BaseStatusDeleted.None).ToList()
            //                         where !ServiceEp.CheckStatusAssignment(ass.StatusAssignment, StatusAssignment.Locked) && !ass.Path.CheckStatus(Status.Draft)
            //                         select ass.RoleEP).ToList();
            //if (ServiceEp.GetPathStatus(idPath) != Status.Draft)
            if (pathStatus != Status.Draft)
            {
                //IList<RoleEP> RolesEP = (from a in Manager.GetIQ<PathPersonAssignment>()
                //                             where a.IdPath == idPath && a.IdPerson == idUser && a.Deleted == BaseStatusDeleted.None  && !ServiceEp.CheckStatusAssignment(a.StatusAssignment, StatusAssignment.Locked)
                //                         select a.RoleEP).ToList();
                IList<RoleEP> RolesEP = (from a in Manager.GetAll<PathPersonAssignment>(p=> p.IdPath == idPath && p.IdPerson == idUser && p.Deleted == BaseStatusDeleted.None).ToList()
                                         where  !ServiceEp.CheckStatusAssignment(a.StatusAssignment, StatusAssignment.Locked)
                                         select a.RoleEP).ToList();

                if (RolesEP.Count > 0)
                {
                    return RolesEP[0];
                }
            }
            return RoleEP.None;
        }
        public RoleEP GetUserRole_ByPathPersonAssignment(Path path, int idUser)
        {
            return GetUserRole_ByPathPersonAssignment(path.Id, path.Status, idUser);
        }
        public RoleEP GetUserRole_ByPathRoleAssignment(long idPath,  int idRole)
        {
            return GetUserRole_ByPathRoleAssignment(idPath, ServiceEp.GetPathStatus(idPath), idRole);
        }

        public RoleEP GetUserRole_ByPathRoleAssignment(Path path, int idRole)
        {
            return GetUserRole_ByPathRoleAssignment(path.Id, path.Status, idRole);
        }
        public RoleEP GetUserRole_ByPathRoleAssignment(long idPath, Status pathStatus, int idRole)
        {
            //IList<RoleEP> RolesEP = (from ass in Manager.GetAll<PathCRoleAssignment>(a => a.Path.Id == idPath && a.Path.Deleted == BaseStatusDeleted.None && a.Role.Id == idRole && a.Deleted == BaseStatusDeleted.None && a.Path.Deleted == BaseStatusDeleted.None).ToList()
            //                         where !ServiceEp.CheckStatusAssignment(ass.StatusAssignment, StatusAssignment.Locked) && !ass.Path.CheckStatus(Status.Draft)
            //                         select ass.RoleEP).ToList();
            //if (ServiceEp.GetPathStatus(idPath) != Status.Draft)
            if (pathStatus != Status.Draft)
            {
                //IList<RoleEP> roles = (from a in Manager.GetIQ<PathCRoleAssignment>()
                //                       where a.IdPath == idPath && a.IdRole == idRole && a.Deleted == BaseStatusDeleted.None
                //                           && !ServiceEp.CheckStatusAssignment(a.StatusAssignment, StatusAssignment.Locked) 
                //                       select a.RoleEP).ToList();
                IList<RoleEP> roles = (from a in Manager.GetAll<PathCRoleAssignment>(p=> p.IdPath == idPath && p.IdRole == idRole && p.Deleted == BaseStatusDeleted.None).ToList()
                                       where !ServiceEp.CheckStatusAssignment(a.StatusAssignment, StatusAssignment.Locked)
                                       select a.RoleEP).ToList();
                if (roles.Count > 0)
                {
                    return roles[0];
                }
            }
            return RoleEP.None;
        }
        #endregion

        #region get Personal MinCompletion

        public Int64 GetPersonalPathMinCompletion(long idPath, int idPerson, int idRole)
        {
            IList<Int64> MinCompletion = (from a in Manager.GetIQ<PathPersonAssignment>()
                                              where a.IdPerson == idPerson && a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None && a.MinCompletion != -1
                                          select a.MinCompletion).ToList();
            if (MinCompletion.Count == 1)
            {
                return MinCompletion[0];
            }
            MinCompletion = (from a in Manager.GetIQ<PathCRoleAssignment>() where a.IdRole == idRole && a.IdPath == idPath && a.Deleted == BaseStatusDeleted.None && a.MinCompletion != -1
                             select a.MinCompletion).ToList();
            if (MinCompletion.Count == 1)
            {
                return MinCompletion[0];
            }
            return -1;
        }

        public Int64 GetPersonalUnitMinCompletion(long idUnit, int idPerson, int idRole)
        {
            IList<Int64> MinCompletion = (from a in Manager.GetIQ<UnitPersonAssignment>() where a.IdPerson==idPerson && a.Deleted == BaseStatusDeleted.None && a.MinCompletion != -1
                                          select a.MinCompletion).ToList();
            if (MinCompletion.Count == 1)
            {
                return MinCompletion[0];
            }
            MinCompletion = (from a in Manager.GetIQ<UnitCRoleAssignment>()
                                 where  a.IdRole == idRole && a.IdUnit == idUnit && a.Deleted == BaseStatusDeleted.None && a.MinCompletion != -1
                             select a.MinCompletion).ToList();
            if (MinCompletion.Count == 1)
            {
                return MinCompletion[0];
            }
            return -1;
        }

        public Int64 GetPersonalActivityMinCompletion(long idActivity, int idPerson, int idRole)
        {
            IList<Int64> MinCompletion = (from a in Manager.Linq<ActivityPersonAssignment>() where a.IdPerson==idPerson && a.IdActivity ==idActivity
                                             && a.Deleted == BaseStatusDeleted.None && a.MinCompletion != -1
                                          select a.MinCompletion).ToList();
            if (MinCompletion.Count == 1)
            {
                return MinCompletion[0];
            }
            MinCompletion = (from a in Manager.Linq<ActivityCRoleAssignment>() where  a.IdRole == idRole && a.IdActivity ==idActivity && a.Deleted == BaseStatusDeleted.None && a.MinCompletion != -1
                             select a.MinCompletion).ToList();
            if (MinCompletion.Count == 1)
            {
                return MinCompletion[0];
            }
            return -1;
        }
        #endregion

        #region update assignment status

        public bool SetSubActivityAssignment_PersonalStatus(long idSubActivity, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress,UpdateStatusType updateType, Status personalStatus, bool NoTransaction)
        {
            DateTime currentTime = DateTime.Now;
            IList<SubActivityPersonAssignment> personAssignments = (from item in Manager.GetAll<SubActivityPersonAssignment>(a => a.IdSubActivity == idSubActivity && a.CreatedOn < currentTime )
                                                                    where item.Status==Status.None
                                                                    select item).ToList();
            IList<SubActivityCRoleAssignment> roleAssignments = (from item in Manager.GetAll<SubActivityCRoleAssignment>(a => a.IdSubActivity == idSubActivity && a.CreatedOn < currentTime)
                                                                  where item.Status == Status.None
                                                                  select item).ToList();
            if (NoTransaction)
            {
                UpdateSubActivityAssignmentsStatusNoTransaction(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, personalStatus);
                return true;
            }
            else
            {
                return UpdateSubActivityAssignmentsStatus(personAssignments, roleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, personalStatus);
            }
        }

        public bool SetActivityAssignment_PersonalStatus(long idActivity, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress,UpdateStatusType updateType, Status personalStatus, bool NoTransaction)
        {
            DateTime currentTime = DateTime.Now;
            IList<ActivityPersonAssignment> PersonAssignments = (from item in Manager.GetAll<ActivityPersonAssignment>(a => a.IdActivity == idActivity && a.CreatedOn < currentTime )
                                                                 where item.Status == Status.None
                                                                 select item).ToList();
            IList<ActivityCRoleAssignment> CRoleAssignments = (from item in Manager.GetAll<ActivityCRoleAssignment>(a => a.IdActivity == idActivity && a.CreatedOn < currentTime)
                                                               where item.Status == Status.None
                                                               select item).ToList();
           
            if (NoTransaction)
            {
                UpdateActivityAssignmentsStatusNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, personalStatus);
                return true;
            }
            else
            {
                return UpdateActivityAssignmentsStatus(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, personalStatus);
            }
        }

        public bool SetUnitAssignment_PersonalStatus(long idUnit, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status personalStatus, bool NoTransaction)
        {
            DateTime currentTime = DateTime.Now;
            IList<UnitPersonAssignment> PersonAssignments = (from item in Manager.GetAll<UnitPersonAssignment>(a => a.IdUnit == idUnit && a.CreatedOn < currentTime )
                                                             where item.Status == Status.None
                                                             select item).ToList();

            IList<UnitCRoleAssignment> CRoleAssignments = (from item in Manager.GetAll<UnitCRoleAssignment>(a => a.IdUnit == idUnit && a.CreatedOn < currentTime)
                                                           where item.Status == Status.None
                                                           select item).ToList();            
            if (NoTransaction)
            {
                UpdateUnitAssignmentsStatusNoTransaction (PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, personalStatus);
                return true;
            }
            else
            {
                return UpdateUnitAssignmentsStatus(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, personalStatus);
            }
        }

        public bool SetPathtAssignment_StatusAsPath_Status(long pathId, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Status personalStatus, bool NoTransaction)
        {
            DateTime currentTime = DateTime.Now;
            IList<PathPersonAssignment> PersonAssignments = (from item in Manager.GetAll<PathPersonAssignment>(a => a.IdPath == pathId && a.CreatedOn < currentTime)
                                                             where item.Status == Status.None
                                                             select item).ToList();

            IList<PathCRoleAssignment> CRoleAssignments = (from item in Manager.GetAll<PathCRoleAssignment>(a => a.IdPath == pathId && a.CreatedOn < currentTime )
                                                           where item.Status == Status.None
                                                           select item).ToList();
            if (NoTransaction)
            {
                UpdatePathAssignmentsStatusNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalStatus);
                return true;
            }
            else
            {
                return UpdatePathAssignmentsStatus(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, personalStatus);
            }
        }

        private bool UpdatePathAssignmentsStatus(IList<PathPersonAssignment> PersonAssignments, IList<PathCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Status newStatus)
        {
            try
            {
                Manager.BeginTransaction();
                UpdatePathAssignmentsStatusNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, newStatus);
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

        private void UpdatePathAssignmentsStatusNoTransaction(IList<PathPersonAssignment> PersonAssignments, IList<PathCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, Status newStatus)
        {
            DateTime currentTime = DateTime.Now;
            UpdatePathPersonAssignmentsStatusNoTransaction(PersonAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, newStatus);
            UpdatePathCRoleAssignmentsStatusNoTransaction(CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, newStatus);
        }

        private void UpdatePathPersonAssignmentsStatusNoTransaction(IList<PathPersonAssignment> PersonAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Status newStatus)
        {

            foreach (PathPersonAssignment item in PersonAssignments)
            {
                if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                {
                    item.setLocked();
                }
                else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                {
                    item.setNotLocked();
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetPathPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<PathPersonAssignment>(item);
            }
        }

        private void UpdatePathCRoleAssignmentsStatusNoTransaction(IList<PathCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, Status newStatus)
        {
            foreach (PathCRoleAssignment item in CRoleAssignments)
            {
                if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                {
                    item.setLocked();
                }
                else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                {
                    item.setNotLocked();
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetPathCRoleAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<PathCRoleAssignment>(item);
            }
        }

        private bool UpdateSubActivityAssignmentsStatus(IList<SubActivityPersonAssignment> PersonAssignments, IList<SubActivityCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status newStatus)
        {
            try
            {
                Manager.BeginTransaction();
                UpdateSubActivityAssignmentsStatusNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, newStatus);
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

        private void UpdateSubActivityAssignmentsStatusNoTransaction(IList<SubActivityPersonAssignment> PersonAssignments, IList<SubActivityCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status newStatus)
        {
            DateTime currentTime = DateTime.Now;
            UpdateSubActivityPersonAssignmentsStatusNoTransaction(PersonAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime,updateType, newStatus);
            UpdateSubActivityCRoleAssignmentsStatusNoTransaction(CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime, updateType, newStatus);
        }

        private void UpdateSubActivityPersonAssignmentsStatusNoTransaction(IList<SubActivityPersonAssignment> PersonAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, UpdateStatusType updateType, Status newStatus)
        {

            foreach (SubActivityPersonAssignment item in PersonAssignments)
            {
                switch (updateType)
                {
                    case UpdateStatusType.MandatoryStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Mandatory))
                        {
                            item.setMandatory();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotMandatory))
                        {
                            item.setNotMandatory();
                        }
                        break;
                    case UpdateStatusType.VisibilityStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                        {
                            item.setLocked();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                        {
                            item.setNotLocked();
                        }
                        break;
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetSubActivityPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<SubActivityPersonAssignment>(item);
            }
        }

        private void UpdateSubActivityCRoleAssignmentsStatusNoTransaction(IList<SubActivityCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, UpdateStatusType updateType, Status newStatus)
        {
            foreach (SubActivityCRoleAssignment item in CRoleAssignments)
            {
                switch (updateType)
                {
                    case UpdateStatusType.MandatoryStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Mandatory))
                        {
                            item.setMandatory();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotMandatory))
                        {
                            item.setNotMandatory();
                        }
                        break;
                    case UpdateStatusType.VisibilityStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                        {
                            item.setLocked();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                        {
                            item.setNotLocked();
                        }
                        break;
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetSubActivityCRoleAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<SubActivityCRoleAssignment>(item);
            }
        }

        private bool UpdateActivityAssignmentsStatus(IList<ActivityPersonAssignment> PersonAssignments, IList<ActivityCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status newStatus)
        {
            try
            {
                Manager.BeginTransaction();
                UpdateActivityAssignmentsStatusNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, newStatus);
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

        private void UpdateActivityAssignmentsStatusNoTransaction(IList<ActivityPersonAssignment> PersonAssignments, IList<ActivityCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status newStatus)
        {
            DateTime currentTime = DateTime.Now;
            UpdateActivityPersonAssignmentsStatusNoTransaction(PersonAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime,updateType, newStatus);
            UpdateActivityCRoleAssignmentsStatusNoTransaction(CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime,updateType, newStatus);
        }

        private void UpdateActivityPersonAssignmentsStatusNoTransaction(IList<ActivityPersonAssignment> PersonAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, UpdateStatusType updateType, Status newStatus)
        {

            foreach (ActivityPersonAssignment item in PersonAssignments)
            {
                switch (updateType)
                {
                    case UpdateStatusType.MandatoryStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Mandatory))
                        {
                            item.setMandatory();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotMandatory))
                        {
                            item.setNotMandatory();
                        }
                        break;
                    case UpdateStatusType.VisibilityStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                        {
                            item.setLocked();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                        {
                            item.setNotLocked();
                        }
                        break;
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetActivityPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<ActivityPersonAssignment>(item);
            }
        }

        private void UpdateActivityCRoleAssignmentsStatusNoTransaction(IList<ActivityCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, UpdateStatusType updateType, Status newStatus)
        {
            foreach (ActivityCRoleAssignment item in CRoleAssignments)
            {
                switch (updateType)
                {
                    case UpdateStatusType.MandatoryStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Mandatory))
                        {
                            item.setMandatory();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotMandatory))
                        {
                            item.setNotMandatory();
                        }
                        break;
                    case UpdateStatusType.VisibilityStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                        {
                            item.setLocked();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                        {
                            item.setNotLocked();
                        }
                        break;
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetActivityCRoleAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<ActivityCRoleAssignment>(item);
            }
        }

        private bool UpdateUnitAssignmentsStatus(IList<UnitPersonAssignment> PersonAssignments, IList<UnitCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status newStatus)
        {
            try
            {
                Manager.BeginTransaction();
                UpdateUnitAssignmentsStatusNoTransaction(PersonAssignments, CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress,updateType, newStatus);
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

        private void UpdateUnitAssignmentsStatusNoTransaction(IList<UnitPersonAssignment> PersonAssignments, IList<UnitCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, UpdateStatusType updateType, Status newStatus)
        {
            DateTime currentTime = DateTime.Now;
            UpdateUnitPersonAssignmentsStatusNoTransaction(PersonAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime,updateType, newStatus);
            UpdateUnitCRoleAssignmentsStatusNoTransaction(CRoleAssignments, UpdaterId, UpdaterProxyIPaddress, UpdaterIPaddress, currentTime,updateType, newStatus);
        }

        private void UpdateUnitPersonAssignmentsStatusNoTransaction(IList<UnitPersonAssignment> PersonAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, UpdateStatusType updateType, Status newStatus)
        {
            foreach (UnitPersonAssignment item in PersonAssignments)
            {
                switch (updateType)
                {
                    case UpdateStatusType.MandatoryStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Mandatory))
                        {
                            item.setMandatory();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotMandatory))
                        {
                            item.setNotMandatory();
                        }
                        break;
                    case UpdateStatusType.VisibilityStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                        {
                            item.setLocked();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                        {
                            item.setNotLocked();
                        }
                        break;
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetUnitPersonAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<UnitPersonAssignment>(item);
            }
        }

        private void UpdateUnitCRoleAssignmentsStatusNoTransaction(IList<UnitCRoleAssignment> CRoleAssignments, int UpdaterId, string UpdaterProxyIPaddress, string UpdaterIPaddress, DateTime currentTime, UpdateStatusType updateType, Status newStatus)
        {
            foreach (UnitCRoleAssignment item in CRoleAssignments)
            {
                switch (updateType)
                {
                    case UpdateStatusType.MandatoryStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Mandatory))
                        {
                            item.setMandatory();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotMandatory))
                        {
                            item.setNotMandatory();
                        }
                        break;
                    case UpdateStatusType.VisibilityStatus:
                        if (ServiceEp.CheckStatus(newStatus, Status.Locked))
                        {
                            item.setLocked();
                        }
                        else if (ServiceEp.CheckStatus(newStatus, Status.NotLocked))
                        {
                            item.setNotLocked();
                        }
                        break;
                }
                item.UpdateMetaInfo(UpdaterId, UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                //SetUnitCRoleAssModifyMetaInfo(item, Manager.GetPerson(UpdaterId), UpdaterIPaddress, UpdaterProxyIPaddress, currentTime);
                Manager.SaveOrUpdate<UnitCRoleAssignment>(item);
            }
        }


        #endregion

        #region get Personal Status

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns>Status.None if not exist personal status</returns>
        public Status GetPersonalPathStatus(long idPath, int userId, int croleId)
        {
            IList<PathPersonAssignment> StatusListPers = Manager.GetAll<PathPersonAssignment>(a => a.IdPerson == userId &&
                                        a.IdPath == idPath &&
                                        a.Status > Status.None && a.Deleted == BaseStatusDeleted.None);

            if (StatusListPers.Count == 1 && ServiceEp.CheckRoleEp(StatusListPers[0].RoleEP, RoleEP.Participant))
            {
                return StatusListPers[0].Status;
            }
            IList<PathCRoleAssignment> StatusListCRole = Manager.GetAll<PathCRoleAssignment>(a => a.IdRole == croleId && a.IdPath == idPath
                                                         && a.Status > Status.None
                                                         && a.Deleted == BaseStatusDeleted.None);

            if (StatusListCRole.Count == 1 && ServiceEp.CheckRoleEp(StatusListCRole[0].RoleEP, RoleEP.Participant))
            {
                return StatusListCRole[0].Status;
            }
            return Status.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns>Status.None if not exist personal status</returns>
        public Status GetPersonalUnitStatus(long unitId, int userId, int croleId)
        {
            IList<UnitPersonAssignment> StatusListPers = Manager.GetAll<UnitPersonAssignment>(a => a.IdPerson == userId &&
                                        a.IdUnit == unitId &&
                                        a.Status > Status.None && a.Deleted == BaseStatusDeleted.None);

            if (StatusListPers.Count == 1 && ServiceEp.CheckRoleEp(StatusListPers[0].RoleEP, RoleEP.Participant))
            {
                return StatusListPers[0].Status;
            }
            IList<UnitCRoleAssignment> StatusListCRole = Manager.GetAll<UnitCRoleAssignment>(a => a.IdRole == croleId && a.IdUnit == unitId
                                                        && a.Status > Status.None
                                                         && a.Deleted == BaseStatusDeleted.None);

            if (StatusListCRole.Count == 1 && ServiceEp.CheckRoleEp(StatusListCRole[0].RoleEP, RoleEP.Participant))
            {
                return StatusListCRole[0].Status;
            }
            return Status.None;
        }

        /// <summary>
        /// 
        /// </summary> 
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns>Status.None if not exist personal status</returns>
        public Status GetPersonalActivityStatus(long activityId, int userId, int croleId)
        {
            IList<ActivityPersonAssignment> StatusListPers = Manager.GetAll<ActivityPersonAssignment>(a => a.IdPerson == userId &&
                                                              a.IdActivity == activityId 
                                                                && a.Status > Status.None && a.Deleted == BaseStatusDeleted.None);

            if (StatusListPers.Count == 1 && ServiceEp.CheckRoleEp(StatusListPers[0].RoleEP, RoleEP.Participant))
            {
                return StatusListPers[0].Status;
            }
            IList<ActivityCRoleAssignment> StatusListCRole = Manager.GetAll<ActivityCRoleAssignment>(a => a.IdRole == croleId &&
                                                             a.IdActivity == activityId && a.Status > Status.None 
                                                            && a.Deleted == BaseStatusDeleted.None);

            if (StatusListCRole.Count == 1 && ServiceEp.CheckRoleEp(StatusListCRole[0].RoleEP, RoleEP.Participant))
            {
                return StatusListCRole[0].Status;
            }
            return Status.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subActivityId"></param>
        /// <param name="userId"></param>
        /// <param name="croleId"></param>
        /// <returns>Status.None if not exist personal status</returns>
        public Status GetPersonalSubActivityStatus(long subActivityId, int userId, int croleId)
        {
            IList<SubActivityPersonAssignment> StatusListPers = Manager.GetAll<SubActivityPersonAssignment>(a => a.IdPerson == userId &&
                                        a.IdSubActivity == subActivityId &&
                                        a.Status > Status.None && a.Deleted == BaseStatusDeleted.None);
            if (StatusListPers.Count == 1 && ServiceEp.CheckRoleEp(StatusListPers[0].RoleEP, RoleEP.Participant))
            {
                return StatusListPers[0].Status;
            }

            IList<SubActivityCRoleAssignment> StatusListCRole = Manager.GetAll<SubActivityCRoleAssignment>(a => a.IdRole == croleId &&
                                    a.IdSubActivity == subActivityId &&
                                    a.Deleted == BaseStatusDeleted.None && a.Status > Status.None);

            if (StatusListCRole.Count == 1 && ServiceEp.CheckRoleEp(StatusListCRole[0].RoleEP, RoleEP.Participant))
            {
                return StatusListCRole[0].Status;
            }
            return Status.None;
        }
        #endregion

        #region evaluate

        public bool ExistActsEvaluableAnalogic(long pathId, int evalId, int evalCroleId)
        {
            bool isparticipant_byPersonAss = ServiceEp.CheckRoleEp(ServiceEp.GetUserRole_ByPath(pathId, evalId, evalCroleId), RoleEP.Participant);
            IList<long> activitiesId = ServiceEp.GetActvityWithEvalAnal(pathId);
            if (isparticipant_byPersonAss)
            {
                //verifico esclusivamente i PersonAss
                return ExistActsEvaluable_byPersonAssignment(pathId, activitiesId, evalId);
            }
            else
            {
                //controllare se posso eval in entrambi
                return ExistActsEvaluable_byPersonAssignment(pathId, activitiesId,  evalId) || ExistActsEvaluable_byRoleAssignment(pathId, activitiesId,  evalCroleId);
            }
        }
                    

        private bool ExistActsEvaluable_byPersonAssignment(long pathId,IList<long> activitiesId , int evalId)
        {
            return (from a in Manager.GetAll<ActivityPersonAssignment>(ass => ass.IdPerson == evalId && ass.RoleEP > RoleEP.Participant && ass.Deleted == BaseStatusDeleted.None && activitiesId.Contains(ass.IdActivity))
                    where  new PermissionEP(a.RoleEP).Evaluate
                    select a.IdActivity).Count() > 0;
        
        }

        private bool ExistActsEvaluable_byRoleAssignment(long pathId,IList<long> activitiesId , int evalCroleId)
        {
            return (from a in Manager.GetAll<ActivityCRoleAssignment>(ass => ass.IdRole == evalCroleId && ass.RoleEP > RoleEP.Participant && ass.Deleted == BaseStatusDeleted.None && activitiesId.Contains(ass.IdActivity))
                    where  new PermissionEP(a.RoleEP).Evaluate
                    select a.IdActivity).Count() > 0;
        }      

        #endregion

    }
}

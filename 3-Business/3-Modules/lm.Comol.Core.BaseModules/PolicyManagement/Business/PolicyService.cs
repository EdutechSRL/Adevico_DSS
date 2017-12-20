using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.PersonalInfo;

namespace lm.Comol.Core.BaseModules.PolicyManagement.Business
{
    public class PolicyService : CoreServices
    {
        private const string UniqueCode = "POLICY";
        private iApplicationContext _Context;

        #region initClass
            public PolicyService() { }
            public PolicyService(iApplicationContext oContext)
            {
                this.Manager = new BaseModuleManager(oContext.DataContext);
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public PolicyService(iDataContext oDC)
            {
                this.Manager = new BaseModuleManager(oDC);
                _Context = new ApplicationContext();
                _Context.DataContext = oDC;
                this.UC = null;
            }
        #endregion

        public Boolean PolicyAccepted(Person person){
            if (person == null)
                return true;
            else
                return (person.AcceptPolicy);
        }

        public Boolean ExistPolicyToAccept()
        {
            return (from dp in Manager.GetIQ<DataPolicy>() where dp.Deleted == BaseStatusDeleted.None && dp.isActive && dp.Mandatory select dp.Id).Any();
        }

        public Boolean UserHasPolicyToAccept(Person person)
        {
            Boolean result = (from dp in Manager.GetIQ<DataPolicy>() where dp.Deleted == BaseStatusDeleted.None && dp.isActive && dp.Mandatory select dp.Id).Any();
            return (result && (person== null || !person.AcceptPolicy));
        }

        public List<dtoUserDataPolicy> GetActivePolicy(Int32 idUser) { 
            return GetActivePolicy(Manager.GetPerson(idUser));
        }

        public List<dtoUserDataPolicy> GetActivePolicy(Person person)
        {
            List<dtoUserDataPolicy> list = null;
            
            try{
                list = GetActivePolicy();
                list.ForEach(i => i.UserInfo = (from ui in Manager.GetIQ<UserPolicyInfo>()
                                                where ui.Deleted == BaseStatusDeleted.None && person != null && ui.Owner != null && ui.Owner == person && ui.Policy !=null && ui.Policy.Id == i.Id 
                                                select new dtoUserPolicyInfo()
                                                {
                                                    Id = ui.Id,
                                                    Accepted = ui.Accepted
                                                }).Skip(0).Take(1).ToList().FirstOrDefault());
        //         var q =
        //from c in categories
        //join p in products on c equals p.Category into ps
        //from p in ps.DefaultIfEmpty()
        //select new { Category = c, ProductName = p == null ? "(No products)" : p.ProductName };
            }
            catch(Exception ex){
                list = null;
            }
            return list;
        }

        public Boolean SaveUserSelection(Person person, List<dtoUserPolicyInfo> items)
        {
            Boolean result = false;
            try
            {
                Manager.BeginTransaction();
                foreach (dtoUserPolicyInfo dto in items)
                {
                    UserPolicyInfo info = null;
                    if (dto.Id > 0)
                        info = Manager.Get<UserPolicyInfo>(dto.Id);
                    else
                        info = (from ui in Manager.GetIQ<UserPolicyInfo>() where ui.Owner== person && ui.Policy != null && ui.Policy.Id== dto.PolicyId select ui).Skip(0).Take(1).ToList().FirstOrDefault();
                    if (info == null || info.Owner != person)
                    {
                        info = new UserPolicyInfo();
                        info.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        info.Policy = Manager.Get<DataPolicy>(dto.PolicyId);
                        info.Owner = person;
                    }
                    else
                    {
                        info.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
                        info.Deleted = BaseStatusDeleted.None;
                    }
                    info.Accepted = dto.Accepted;
                    Manager.SaveOrUpdate(info);
                }
                if (!HasMandatoryItemsToAccept(person))
                {
                    person.AcceptPolicy = true;
                    person.AcceptPolicyOn = DateTime.Now;
                }
                else
                {
                    Manager.Commit();
                    throw new MandatoryError("") { Items = GetMandatoryItemsToAccept(person) };
                }
                Manager.Commit();
                result = true;
            }
            catch (MandatoryError ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return result;
        }

        private List<dtoUserDataPolicy> GetMandatoryItemsToAccept(Person person)
        {
            List<dtoUserDataPolicy> list = null;

            try
            {
                List<long> itemsAccepted = (from i in Manager.GetIQ<UserPolicyInfo>() where i.Accepted && i.Deleted == BaseStatusDeleted.None && i.Owner == person select i.Policy.Id).ToList();
                list = (from dp in Manager.GetIQ<DataPolicy>()
                        where dp.Deleted == BaseStatusDeleted.None && dp.isActive && dp.Mandatory && itemsAccepted.Contains(dp.Id)==false 
                        select new dtoUserDataPolicy()
                        {
                            Id = dp.Id,
                            DisplayOrder = dp.DisplayOrder,
                            Mandatory = dp.Mandatory,
                            Name = dp.Name,
                            Text = dp.Text,
                            Type = dp.Type
                        }).ToList().OrderBy(i => i.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        private Boolean HasMandatoryItemsToAccept(Person person)
        {
            Boolean result = false;

            try
            {
                List<long> itemsAccepted = (from i in Manager.GetIQ<UserPolicyInfo>() where i.Accepted && i.Deleted == BaseStatusDeleted.None && i.Owner == person select i.Policy.Id ).ToList();
                result = (from dp in Manager.GetIQ<DataPolicy>()
                          where dp.Deleted == BaseStatusDeleted.None && dp.isActive && dp.Mandatory && itemsAccepted.Contains(dp.Id) == false
                          select dp.Id).Any();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public List<dtoUserDataPolicy> GetActivePolicy()
        {
            List<dtoUserDataPolicy> list = null;

            try
            {
                list = (from dp in Manager.GetIQ<DataPolicy>()
                        where dp.Deleted == BaseStatusDeleted.None && dp.isActive
                        select new dtoUserDataPolicy()
                        {
                            Id = dp.Id,
                            DisplayOrder = dp.DisplayOrder,
                            Mandatory = dp.Mandatory,
                            Name = dp.Name,
                            Text = dp.Text,
                            Type = dp.Type
                        }).ToList().OrderBy(i => i.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }
    }
}
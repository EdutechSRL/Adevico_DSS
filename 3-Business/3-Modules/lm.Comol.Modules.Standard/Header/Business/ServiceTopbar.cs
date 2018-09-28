using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
//using System.IO;
using lm.Comol.Core.Business;
using lm.Comol.Modules.Standard.Menu.Domain;

namespace lm.Comol.Modules.Standard.Header.Business
{
    public class ServiceTopbar : CoreServices
    {
        private const string UniqueCode = "SRVtopBar";
        private iUserContext UC { set; get; }        

        #region initClass
            public ServiceTopbar() { }
            public ServiceTopbar(iApplicationContext oContext)
            {
                this.Manager = new BaseModuleManager(oContext.DataContext);
                this.UC = oContext.UserContext;
            }
            public ServiceTopbar(iDataContext datacontext)
            {
                this.Manager = new BaseModuleManager(datacontext);
                this.UC = null;
            }
        #endregion

        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ServiceTopbar.UniqueCode);
        }
        public Person GetPerson(int Iduser)
        {
            return Manager.GetPerson(Iduser);
        }
        public List<String> GetAvailableModules(Person person) {
            if (person == null || person.TypeID == (int)UserTypeStandard.Guest)
                return new List<String>();
            else
                return (from m in Manager.GetIQ<ModuleDefinition>() where m.Available == true select m.Code).ToList();
        }
        public List<int> GetAvailableAuthenticationProviderTypes(Person person)
        {
            if (person == null || person.TypeID == (int)UserTypeStandard.Guest)
                return new List<int>();
            else
                return (from m in Manager.GetIQ<lm.Comol.Core.Authentication.BaseLoginInfo>()
                        where m.Deleted == BaseStatusDeleted.None && m.Person.Id == person.Id  && m.isEnabled
                        select (int)m.Provider.ProviderType).ToList().Distinct().ToList();
        }
        public Language GetLanguage(int IdLanguage)
        {
            return Manager.GetLanguage(IdLanguage);
        }
        public String GetCachedTopBar(Person p, Language l)
        {
            return lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<String>(CacheKeys.RenderTopBar(p.Id, (l==null) ? 0 : l.Id));
        }
        public void SetCachedTopBar(Person p, Language l,String render)
        {
            lm.Comol.Core.DomainModel.Helpers.CacheHelper.AddToCache<String>(CacheKeys.RenderTopBar(p.Id, ((l==null) ? 0 : l.Id)), render, lm.Comol.Core.DomainModel.Helpers.CacheExpiration.Minutes(30));
        }
        
    }
}
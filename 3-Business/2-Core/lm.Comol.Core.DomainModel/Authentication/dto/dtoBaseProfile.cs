using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoBaseProfile
    {
        public virtual Int32 Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual String Login { get; set; }
        public virtual String Password { get; set; }
        public virtual String Mail { get; set; }
        public virtual String FirstLetter { get; set; }
        public virtual String TaxCode { get; set; }
        public virtual Int32 IdLanguage { get; set; }
        public virtual String LanguageName { get; set; }
        public virtual Boolean  ShowMail { get; set; }
        public virtual AuthenticationProviderType AuthenticationProvider { get; set; }
        public virtual String DisplayName { get { return Name + ' ' + Surname; } }
        public virtual String SurnameName { get { return Surname + ' ' + Name; } }
        public virtual Int32 IdProfileType { get; set; }
        public virtual long IdDefaultProvider { get; set; }

        public virtual String Sector { get; set; }
        public virtual String Job { get; set; }

        //public virtual DateTime BirthDate { get; set; }
        //public virtual String BirthPlace { get; set; }
        public virtual PersonInfo PersonInfo { get; set; }
    }

    [Serializable]
    public class dtoExternal :dtoBaseProfile
    {
        public virtual String ExternalUserInfo { get; set; }
    }
    [Serializable]
    public class dtoCompany : dtoBaseProfile
    {
        public virtual CompanyInfo Info { get; set; }

        public dtoCompany() {
            Info = new CompanyInfo();
        }
        public dtoCompany(CompanyUser user)
        {
            Info = user.CompanyInfo;
            Id= user.Id;
            Name= user.Name;
            Surname= user.Surname;
            Mail= user.Mail;
            FirstLetter= user.FirstLetter;
            TaxCode= user.TaxCode;
            IdLanguage= user.LanguageID;
            ShowMail = user.PersonInfo.DefaultShowMailAddress;
            IdProfileType= user.TypeID;
            IdDefaultProvider= user.IdDefaultProvider;

            PersonInfo = user.PersonInfo;
        }
    }

    [Serializable]
    public class dtoEmployee : dtoBaseProfile
    {
        public virtual KeyValuePair<long, String> CurrentAgency { get; set; }
        public virtual List<dtoAgencyAffiliation> Affiliations { get; set; }
        public virtual List<dtoAgencyAffiliation> OrderedAffiliations { get { return Affiliations.OrderByDescending(a => a.IsEnabled).ThenByDescending(a => a.ToDate).ToList(); } }

        public dtoEmployee()
        {
            CurrentAgency = new KeyValuePair<long, String>();
            Affiliations = new List<dtoAgencyAffiliation>();
        }
        public dtoEmployee(Employee user,List<dtoAgencyAffiliation> affiliations): this()
        {
            if (affiliations.Where(a => a.IsEnabled && a.ToDate == null).Any())
                CurrentAgency = affiliations.Where(a => a.IsEnabled && a.ToDate == null).Select(a => a.Agency).FirstOrDefault();
            Affiliations = affiliations;
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Mail = user.Mail;
            FirstLetter = user.FirstLetter;
            TaxCode = user.TaxCode;
            IdLanguage = user.LanguageID;
            ShowMail = user.PersonInfo.DefaultShowMailAddress;
            IdProfileType = user.TypeID;
            IdDefaultProvider = user.IdDefaultProvider;

            Job = user.Job;
            Sector = user.Sector;

            PersonInfo = user.PersonInfo;
            //BirthDate = user.PersonInfo.BirthDate;
            //BirthPlace = user.PersonInfo.BirthPlace;

        }
        public dtoEmployee(Employee user) :this(user, (from a in user.Affiliations select new dtoAgencyAffiliation() { FromDate=a.FromDate, ToDate= a.ToDate, Id=a.Id, IsEnabled= a.IsEnabled , Agency = new KeyValuePair<long,string>(a.Agency.Id, a.Agency.Name)}).ToList())
        {
        }
        public dtoEmployee(Employee user, AgencyVisibility visibility)
            : this(user, GetAffiliations(user.Affiliations,visibility))
        {
        }

        private static List<dtoAgencyAffiliation> GetAffiliations(IList<AgencyAffiliation> affiliations, AgencyVisibility visibility)
        {
            if (affiliations==null || affiliations.Count==0)
                return new List<dtoAgencyAffiliation>();
            else
                 return affiliations.Where(a => 
                (
                a.Agency !=null &&
                (visibility == AgencyVisibility.Deleted && a.Deleted != BaseStatusDeleted.None) 
                || 
                    ( 
                        ( a.Deleted == BaseStatusDeleted.None)
                        &&
                        (
                        visibility == AgencyVisibility.NotDeleted
                        ||
                        (visibility == AgencyVisibility.Active && a.IsEnabled)
                        )
                    ))).Select(a=>
                new dtoAgencyAffiliation() { FromDate=a.FromDate, ToDate= a.ToDate, Id=a.Id, IsEnabled= a.IsEnabled , Agency = new KeyValuePair<long,string>(a.Agency.Id, a.Agency.Name)}).ToList();

        }
    }
}
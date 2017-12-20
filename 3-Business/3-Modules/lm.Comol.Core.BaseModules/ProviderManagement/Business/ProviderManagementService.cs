using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.PersonalInfo;
using lm.Comol.Core.Authentication.Business;
using NHibernate.Linq;

namespace lm.Comol.Core.BaseModules.ProviderManagement.Business
{
    public class ProviderManagementService : CoreServices 
    {
        protected iApplicationContext _Context;

        #region initClass
            public ProviderManagementService() :base() { }
            public ProviderManagementService(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public ProviderManagementService(iDataContext oDC)
                : base(oDC)
            {
                _Context = new ApplicationContext() { DataContext = oDC };
            }
        #endregion

            #region "Management"
                public ModuleProviderManagement GetPermission() {
                    Person person = Manager.GetPerson(UC.CurrentUserID);
                    if (person==null)
                        return ModuleProviderManagement.CreatePortalmodule((int)UserTypeStandard.Guest);
                    else
                        return ModuleProviderManagement.CreatePortalmodule(person.TypeID);
                }
                public long ProvidersCount()
                {
                    long count = (long)0;
                    try
                    {
                        Manager.BeginTransaction();
                        count = (from p in Manager.GetIQ<AuthenticationProvider>() select p.Id).Count();
                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                    }
                    return count;
                }

                public List<dtoProviderPermission> GetAuthenticationProviders(int pageIndex, int pageSize, ModuleProviderManagement module)
                {
                    List<dtoProviderPermission> results = null;

                    try
                    {
                        Language language = Manager.GetLanguage(UC.Language.Id);
                        Language dLanguage = Manager.GetDefaultLanguage();
                        List <dtoProvider> providers = (from p in Manager.GetAll<AuthenticationProvider>()
                                   select new dtoProvider()
                                   {
                                       IdProvider = p.Id,
                                       DisplayToUser = p.DisplayToUser,
                                       AllowAdminProfileInsert = p.AllowAdminProfileInsert,
                                       Type = p.ProviderType,
                                       IdentifierFields = p.IdentifierFields,
                                       AllowMultipleInsert= p.AllowMultipleInsert,
                                       Name = p.Name,
                                       UniqueCode= p.UniqueCode,
                                       Deleted = p.Deleted,
                                       isEnabled = p.IsEnabled,
                                       LogoutMode = p.LogoutMode,
                                       Translation = TranslateProviderInfo(language, dLanguage, p),
                                   }).ToList();
                        foreach (dtoProvider d in providers) { 
                            var query = (from ui in Manager.GetIQ<BaseLoginInfo>() 
                                                 where ui.Deleted == BaseStatusDeleted.None && ui.Provider.Id == d.IdProvider && ui.Person != null select ui);

                            try
                            {
                                d.UsedBy = query.Select(li => li.Person.Id).Distinct().ToList().Count();
                            }
                            catch (Exception ex) {
                                d.UsedBy = query.Select(li => li.Person.Id).Distinct().Count();
                            }
                        }
                        results = (from r in providers orderby r.Deleted, r.Name select new dtoProviderPermission(r) { Permission= new dtoPermission(module,r.Deleted, r.UsedBy, r.isEnabled  )}).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    }
                    catch (Exception ex) {
                        results = new List<dtoProviderPermission>();
                    }
                    return results;
                }

                public dtoProvider GetAuthenticationProvider(long idProvider)
                {
                    dtoProvider result = null;

                    try
                    {
                        Language language = Manager.GetLanguage(UC.Language.Id);
                        Language dLanguage = Manager.GetDefaultLanguage();
                        AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                        if (provider.GetType() == typeof(UrlAuthenticationProvider))
                        {
                            UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
                            result = new dtoUrlProvider()
                            {
                                DeltaTime = urlProvider.DeltaTime,
                                EncryptionInfo = urlProvider.EncryptionInfo,
                                RemoteLoginUrl = urlProvider.RemoteLoginUrl,
                                SenderUrl = urlProvider.SenderUrl,
                                TokenFormat = urlProvider.TokenFormat,
                                VerifyRemoteUrl = urlProvider.VerifyRemoteUrl,
                                UrlIdentifier = urlProvider.UrlIdentifier,
                                LoginFormats = (from f in urlProvider.LoginFormats orderby f.isDefault, f.Name select new dtoLoginFormat() { Id=f.Id, After= f.After, Before=f.Before, Deleted=f.Deleted, isDefault= f.isDefault, Name= f.Name, IdProvider=idProvider}).ToList()
                            };
                        }
                        else if (provider.GetType() == typeof(MacUrlAuthenticationProvider))
                        {
                            MacUrlAuthenticationProvider macProvider = (MacUrlAuthenticationProvider)provider;
                            result = new dtoMacUrlProvider()
                            {
                                DeltaTime = macProvider.DeltaTime,
                                EncryptionInfo = macProvider.EncryptionInfo,
                                RemoteLoginUrl = macProvider.RemoteLoginUrl,
                                SenderUrl = macProvider.SenderUrl,
                                NotifySubscriptionTo= macProvider.NotifySubscriptionTo,
                                AutoAddAgency = macProvider.AutoAddAgency,
                                AutoEnroll = macProvider.AutoEnroll,
                                VerifyRemoteUrl = macProvider.VerifyRemoteUrl,
                                AllowTaxCodeDuplication = macProvider.AllowTaxCodeDuplication,
                                Attributes = macProvider.Attributes.Where(a=> a.Deleted== BaseStatusDeleted.None).ToList(),
                                DenyRequestFromIpAddresses = macProvider.DenyRequestFromIpAddresses,
                                AllowRequestFromIpAddresses = macProvider.AllowRequestFromIpAddresses,
                            };
                        }
                        else if (provider.GetType() == typeof(InternalAuthenticationProvider))
                            result = new dtoInternalProvider() { ChangePasswordAfterDays = ((InternalAuthenticationProvider)provider).ChangePasswordAfterDays };
                        else
                            result = new dtoProvider();
                        result.MultipleItemsForRecord = provider.MultipleItemsForRecord;
                        result.MultipleItemsSeparator = provider.MultipleItemsSeparator;
                        result.IdProvider = provider.Id;
                        result.DisplayToUser = provider.DisplayToUser;
                        result.AllowAdminProfileInsert = provider.AllowAdminProfileInsert;
                        result.Type = provider.ProviderType;
                        result.IdentifierFields = provider.IdentifierFields;
                        result.AllowMultipleInsert = provider.AllowMultipleInsert;
                        result.Name = provider.Name;
                        result.UniqueCode = provider.UniqueCode;
                        result.Deleted = provider.Deleted;
                        result.isEnabled = provider.IsEnabled;
                        result.LogoutMode = provider.LogoutMode;
                        result.UsedBy = (from ui in Manager.GetIQ<BaseLoginInfo>() where ui.Provider == provider select ui.Id).Count();
                        result.Translation = TranslateProviderInfo(language, dLanguage, provider);
                    }
                    catch (Exception ex)
                    {
                        result = null;
                    }
                    return result;
                }
               
                #region "Translations"
                    public dtoProviderTranslation GetAuthenticationTranslation(long idProvider, Int32 idLanguage)
                {
                    dtoProviderTranslation translation = null;
                    try
                    {
                        Manager.BeginTransaction();
                        AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                        Language language = Manager.GetLanguage(idLanguage);
                        translation = (from li in Manager.GetIQ<AuthenticationProviderTranslation>()
                                       where li.Deleted == BaseStatusDeleted.None && (li.Language == language) && li.Provider != null && li.Provider == provider
                                       select new dtoProviderTranslation()
                                       {
                                           Id = li.Id,
                                           IdAuthenticationProvider = li.Provider.Id,
                                           Description = li.Description,
                                           Name = li.Name,
                                           ForSubscribeDescription = li.ForSubscribeDescription,
                                           ForSubscribeName = li.ForSubscribeName,
                                           idLanguage = li.Language.Id,
                                           FieldLong = li.FieldLong,
                                           FieldString = li.FieldString
                                       }).Skip(0).Take(1).ToList().FirstOrDefault();

                        Manager.Commit();
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        translation = new dtoProviderTranslation() { idLanguage = idLanguage };
                    }
                    return translation;
                }
                    public dtoProviderTranslation TranslateProviderInfo(Language ulanguage, Language planguage, AuthenticationProvider provider)
                {
                    dtoProviderTranslation translation = null;
                    try
                    {
                        List<dtoProviderTranslation> list = (from li in Manager.GetIQ<AuthenticationProviderTranslation>()
                                                             where li.Deleted == BaseStatusDeleted.None && (li.Language == ulanguage || li.Language == planguage) && li.Provider == provider
                                                             select new dtoProviderTranslation()
                                                             {
                                                                 Id = li.Id,
                                                                 IdAuthenticationProvider= li.Provider.Id,
                                                                 Description = li.Description,
                                                                 Name = li.Name,
                                                                 ForSubscribeDescription = li.ForSubscribeDescription,
                                                                 ForSubscribeName = li.ForSubscribeName,
                                                                 idLanguage = li.Language.Id,
                                                                 FieldLong = li.FieldLong,
                                                                 FieldString = li.FieldString
                                                             }
                                                ).ToList();

                        if (list.Where(l => l.idLanguage == ulanguage.Id).Any())
                            translation = list.Where(l => l.idLanguage == ulanguage.Id).FirstOrDefault();
                        else
                            translation = list.Where(l => l.idLanguage == planguage.Id).FirstOrDefault();
                        if (translation == null)
                            translation = new dtoProviderTranslation() { Name = provider.Name };
                    }
                    catch (Exception ex)
                    {
                        translation = new dtoProviderTranslation();
                    }
                    return translation;
                }
                    public AuthenticationProviderTranslation SaveTranslation(dtoProviderTranslation dto)
                    {
                        AuthenticationProviderTranslation translation = null;
                        try
                        {
                            Manager.BeginTransaction();
                            if (dto.Id>0)
                                translation = Manager.Get<AuthenticationProviderTranslation>(dto.Id);
                            if (translation == null) {
                                translation = new AuthenticationProviderTranslation();
                                translation.Deleted = BaseStatusDeleted.None;
                                translation.Language = Manager.GetLanguage(dto.idLanguage);
                                translation.Provider = Manager.Get<AuthenticationProvider>(dto.IdAuthenticationProvider);
                            }
                            translation.Name = dto.Name;
                            translation.Description = dto.Description;
                            translation.FieldLong = dto.FieldLong;
                            translation.FieldString = dto.FieldString;
                            translation.ForSubscribeDescription = dto.ForSubscribeDescription;
                            translation.ForSubscribeName = dto.ForSubscribeName;
                            Manager.SaveOrUpdate(translation);
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            translation = null;
                        }
                        return translation;
                    }
                #endregion

                #region "Provider CRUD"
                    public AuthenticationProvider AddProvider(dtoProvider dto, dtoProviderTranslation dtoTranslation) {
                        AuthenticationProvider provider = null;
                        try
                        {
                            Manager.BeginTransaction();
                            switch(dto.Type){
                                case AuthenticationProviderType.Internal:
                                    dtoInternalProvider dtoInternal = (dtoInternalProvider)dto;
                                    InternalAuthenticationProvider internalProvider = new InternalAuthenticationProvider();
                                    internalProvider.ChangePasswordAfterDays= dtoInternal.ChangePasswordAfterDays;
                                    provider = internalProvider;
                                    break;
                                case AuthenticationProviderType.Url:
                                    dtoUrlProvider dtoUrlProvider = (dtoUrlProvider)dto;
                                    UrlAuthenticationProvider urlProvider = new UrlAuthenticationProvider();
                                    urlProvider.NotifySubscriptionTo = dtoUrlProvider.NotifySubscriptionTo;
                                    urlProvider.DeltaTime = dtoUrlProvider.DeltaTime;
                                    urlProvider.TokenFormat = dtoUrlProvider.TokenFormat;
                                    urlProvider.SenderUrl = dtoUrlProvider.SenderUrl;
                                    urlProvider.RemoteLoginUrl = dtoUrlProvider.RemoteLoginUrl;
                                    urlProvider.VerifyRemoteUrl = dtoUrlProvider.VerifyRemoteUrl;
                                    urlProvider.EncryptionInfo = dtoUrlProvider.EncryptionInfo;
                                    urlProvider.UrlIdentifier = dtoUrlProvider.UrlIdentifier;
                                    urlProvider.LoginFormats = new List<LoginFormat>();
                                    provider = urlProvider;
                                    break;
                                case AuthenticationProviderType.UrlMacProvider:
                                    dtoMacUrlProvider dtoMacProvider = (dtoMacUrlProvider)dto;
                                    MacUrlAuthenticationProvider macProvider = new MacUrlAuthenticationProvider();
                                    macProvider.NotifySubscriptionTo = macProvider.NotifySubscriptionTo;
                                    macProvider.DeltaTime = dtoMacProvider.DeltaTime;
                                    macProvider.SenderUrl = dtoMacProvider.SenderUrl;
                                    macProvider.RemoteLoginUrl = dtoMacProvider.RemoteLoginUrl;
                                    macProvider.VerifyRemoteUrl = dtoMacProvider.VerifyRemoteUrl;
                                    macProvider.EncryptionInfo = dtoMacProvider.EncryptionInfo;
                                    macProvider.AutoAddAgency = dtoMacProvider.AutoAddAgency;
                                    macProvider.AutoEnroll = dtoMacProvider.AutoEnroll;
                                    macProvider.Attributes = new List<BaseUrlMacAttribute>();
                                    macProvider.DenyRequestFromIpAddresses = dtoMacProvider.DenyRequestFromIpAddresses;
                                    macProvider.AllowRequestFromIpAddresses = dtoMacProvider.AllowRequestFromIpAddresses;
                                    provider = macProvider;
                                    break;
                                case AuthenticationProviderType.Ldap:
                                    provider = new LDAPAuthenticationProvider();
                                    break;
                                default:
                                    provider = new AuthenticationProvider();
                                    break;
                            }
                            provider.AllowAdminProfileInsert = dto.AllowAdminProfileInsert;
                            provider.AllowMultipleInsert = dto.AllowMultipleInsert;
                            provider.Deleted = BaseStatusDeleted.None;
                            provider.DisplayToUser = dto.DisplayToUser;
                            provider.IdentifierFields = dto.IdentifierFields;
                            provider.IdOldAuthentication = 0;
                            provider.Name = dto.Name;
                            provider.ProviderType = dto.Type;
                            provider.UniqueCode = dto.UniqueCode;
                            provider.Translations = new List<AuthenticationProviderTranslation>();
                            provider.IsEnabled = false;
                            Manager.SaveOrUpdate(provider);
                            foreach (Language language in Manager.GetAllLanguages().OrderBy(l=>l.Id).ToList()){
                                AuthenticationProviderTranslation translation = new AuthenticationProviderTranslation();
                                translation.Deleted= BaseStatusDeleted.None;
                                translation.Description= dtoTranslation.Description;
                                translation.FieldLong= dtoTranslation.FieldLong;
                                translation.FieldString= dtoTranslation.FieldString;
                                translation.ForSubscribeDescription= dtoTranslation.ForSubscribeDescription;
                                translation.ForSubscribeName= dtoTranslation.ForSubscribeName;
                                translation.Language= language;
                                translation.Name= dtoTranslation.Name;
                                translation.Provider= provider;
                                Manager.SaveOrUpdate(translation);
                                provider.Translations.Add(translation);
                            }
                            if (typeof(UrlAuthenticationProvider) == provider.GetType()) {
                                UrlAuthenticationProvider urlProvider = (UrlAuthenticationProvider)provider;
                                LoginFormat loginFormat = new LoginFormat()
                                {
                                    isDefault = true,
                                    Deleted = BaseStatusDeleted.None,
                                    Name = "Default",
                                    Provider = urlProvider
                                };
                                Manager.SaveOrUpdate(loginFormat);
                                urlProvider.LoginFormats.Add(loginFormat);
                            }
                            if (typeof(MacUrlAuthenticationProvider) == provider.GetType())
                            {
                                MacUrlAuthenticationProvider macProvider = (MacUrlAuthenticationProvider)provider;
                                ApplicationAttribute appAttribute = null;
                                FunctionAttribute funAttribute = null;
                                TimestampAttribute timeAttribute = null;

                                if (macProvider.Attributes.Where(a => a.Type == UrlMacAttributeType.applicationId).Any())
                                {
                                    appAttribute = macProvider.Attributes.Where(a=> a.Type== UrlMacAttributeType.applicationId).Select(a=> (ApplicationAttribute)a).FirstOrDefault();
                                    appAttribute.Deleted= BaseStatusDeleted.None;
                                    appAttribute.Provider = macProvider;
                                }
                                else
                                    appAttribute = new ApplicationAttribute()
                                    {
                                        Deleted = BaseStatusDeleted.None,
                                        Name = "ApplicationID",
                                        Type = UrlMacAttributeType.applicationId,
                                        QueryStringName = "ApplicationID",
                                        Value = macProvider.Id.ToString(),
                                        Provider = macProvider
                                    };
                                Manager.SaveOrUpdate(appAttribute);

                                if (macProvider.Attributes.Where(a => a.Type == UrlMacAttributeType.functionId).Any())
                                {
                                    funAttribute = macProvider.Attributes.Where(a => a.Type == UrlMacAttributeType.functionId).Select(a => (FunctionAttribute)a).FirstOrDefault();
                                    funAttribute.Deleted = BaseStatusDeleted.None;
                                    funAttribute.Provider = macProvider;
                                }
                                else
                                    funAttribute = new FunctionAttribute()
                                {
                                    Deleted = BaseStatusDeleted.None,
                                    Name = "FunctionID",
                                    Type = UrlMacAttributeType.functionId,
                                    QueryStringName = "FunctionID",
                                    Value = macProvider.Id.ToString(),
                                    Provider = macProvider
                                };
                                Manager.SaveOrUpdate(funAttribute);

                                if (macProvider.Attributes.Where(a => a.Type == UrlMacAttributeType.timestamp).Any())
                                {
                                    timeAttribute = macProvider.Attributes.Where(a => a.Type == UrlMacAttributeType.timestamp).Select(a => (TimestampAttribute)a).FirstOrDefault();
                                    timeAttribute.Deleted = BaseStatusDeleted.None;
                                    timeAttribute.Provider = macProvider;
                                }
                                else
                                    timeAttribute = new TimestampAttribute()
                                    {
                                        Deleted = BaseStatusDeleted.None,
                                        Name = "TimeStamp",
                                        Type = UrlMacAttributeType.timestamp,
                                        QueryStringName = "TimeStamp",
                                        Format = TimestampFormat.aaaammgghhmmss,
                                        Provider = macProvider
                                    };
                                Manager.SaveOrUpdate(timeAttribute);

                                macProvider.Attributes.Add(appAttribute);
                                macProvider.Attributes.Add(funAttribute);
                                macProvider.Attributes.Add(timeAttribute);
                                Manager.SaveOrUpdate(macProvider);
                            }
                            Manager.Commit();

                        }
                        catch (Exception ex) {
                            Manager.RollBack();
                            provider = null;
                        }
                        return provider;
                    }
                    public Boolean isUniqueProviderCode(long idProvider,String code) {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            result = !(from p in Manager.GetIQ<AuthenticationProvider>() where p.Id != idProvider && p.UniqueCode == code select p.Id).Any();
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                        }
                        return result;
                    }
                    public AuthenticationProvider SaveProvider(dtoProvider dto)
                    {
                        AuthenticationProvider provider = null;
                        try
                        {
                            Manager.BeginTransaction();
                            if (dto.IdProvider > 0)
                                provider = Manager.Get<AuthenticationProvider>(dto.IdProvider);
                            if (provider == null)
                            {
                                provider = new AuthenticationProvider();
                                provider.Deleted = BaseStatusDeleted.None;
                            }
                            UpdateProvider(dto, provider);
                            Manager.SaveOrUpdate(provider);
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            provider = null;
                        }
                        return provider;
                    }

                    #region "Url Provider"
                        public Boolean UpdateProviderEncryptionInfo(long idProvider, lm.Comol.Core.Authentication.Helpers.EncryptionInfo dto)
                        {
                            Boolean result = false;
                            try
                            {
                                Manager.BeginTransaction();
                                UrlAuthenticationProvider provider = Manager.Get<UrlAuthenticationProvider>(idProvider);
                                if (provider != null)
                                {
                                    provider.EncryptionInfo = dto;
                                    Manager.SaveOrUpdate(provider);
                                }
                                Manager.Commit();
                                result = (provider != null);
                            }
                            catch (Exception ex) {
                                Manager.RollBack();
                            }
                            return result;
                        }
                        public UrlAuthenticationProvider SaveProvider(dtoUrlProvider dto)
                        {
                            UrlAuthenticationProvider provider = null;
                            try
                            {
                                Manager.BeginTransaction();
                                if (dto.IdProvider > 0)
                                    provider = Manager.Get<UrlAuthenticationProvider>(dto.IdProvider);
                                if (provider == null)
                                {
                                    provider = new UrlAuthenticationProvider();
                                    provider.Deleted = BaseStatusDeleted.None;
                                }
                                provider.UrlIdentifier = dto.UrlIdentifier;
                                provider.DeltaTime = dto.DeltaTime;
                                provider.TokenFormat = dto.TokenFormat;
                                provider.EncryptionInfo = dto.EncryptionInfo;
                                provider.SenderUrl = dto.SenderUrl;
                                provider.RemoteLoginUrl = dto.RemoteLoginUrl;
                                provider.VerifyRemoteUrl = dto.VerifyRemoteUrl;
                                provider.NotifySubscriptionTo = dto.NotifySubscriptionTo;
                                UpdateProvider(dto, provider);
                                Manager.SaveOrUpdate(provider);
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                provider = null;
                            }
                            return provider;
                        }

                        public LoginFormat SaveLoginFormat(long idProvider, dtoLoginFormat dto)
                        {
                            LoginFormat loginFormat = null;
                            try
                            {
                                Manager.BeginTransaction();
                                UrlAuthenticationProvider provider = Manager.Get<UrlAuthenticationProvider>(idProvider);

                                if (dto.Id > 0)
                                    loginFormat = Manager.Get<LoginFormat>(dto.Id);
                                if (loginFormat == null)
                                {
                                    loginFormat = new LoginFormat();
                                    loginFormat.Deleted = BaseStatusDeleted.None;
                                    loginFormat.Provider = provider;
                                }
                                loginFormat.After = dto.After;
                                loginFormat.Before = dto.Before;
                                loginFormat.Name = dto.Name;
                                loginFormat.isDefault = dto.isDefault;
                                if (dto.isDefault)
                                {
                                    LoginFormat defaultFormat = (from lf in Manager.GetIQ<LoginFormat>()
                                                                 where lf.Provider == provider && lf.isDefault && lf.Id != dto.Id
                                                                 select lf).Skip(0).Take(1).ToList().FirstOrDefault();
                                    if (defaultFormat != null) {
                                        defaultFormat.isDefault = false;
                                        Manager.SaveOrUpdate(defaultFormat);
                                    }
                                }

                                
                                Manager.SaveOrUpdate(loginFormat);
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                loginFormat = null;
                            }
                            return loginFormat;
                        }
                        public dtoLoginFormat GetProviderLoginFormat(long idFormat)
                        {
                            dtoLoginFormat loginFormat = null;
                            try
                            {
                                Manager.BeginTransaction();
                                loginFormat = (from lf in Manager.GetIQ<LoginFormat>()
                                               where lf.Id == idFormat
                                         select new dtoLoginFormat()
                                         {
                                             Id = lf.Id,
                                             After = lf.After,
                                             Before = lf.Before,
                                             Deleted = lf.Deleted,
                                             isDefault = lf.isDefault,
                                             Name = lf.Name
                                         }).Skip(0).Take(1).ToList().FirstOrDefault();
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                            }
                            return loginFormat;
                        }
                        public List<dtoLoginFormat> GetProviderLoginFormats(long idProvider)
                        {
                            List<dtoLoginFormat> items = null;
                            try
                            {
                                Manager.BeginTransaction();
                                items = (from lf in Manager.GetIQ<LoginFormat>()
                                         where lf.Provider != null && lf.Provider.Id == idProvider
                                         orderby lf.isDefault, lf.Name 
                                         select new dtoLoginFormat()
                                         {
                                             Id = lf.Id,
                                             After = lf.After,
                                             Before = lf.Before,
                                             Deleted = lf.Deleted,
                                             isDefault = lf.isDefault,
                                             Name = lf.Name,
                                             IdProvider = idProvider
                                         }).ToList();
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                items = new List<dtoLoginFormat>();
                            }
                            return items;
                        }
                        public Boolean VirtualDeleteLoginFormat(long idFormat, Boolean delete)
                        {
                            Boolean result = false;
                            try
                            {
                                Manager.BeginTransaction();
                                LoginFormat format = Manager.Get<LoginFormat>(idFormat);
                                if (format != null)
                                {
                                    format.Deleted = (delete == true) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                    Manager.SaveOrUpdate(format);
                                    result = true;
                                }
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                result = false;
                            }
                            return result;
                        }
                        public Boolean VirtualDeleteLoginFormat(long idFormat)
                        {
                            return VirtualDeleteLoginFormat(idFormat, true);
                        }
                        public Boolean VirtualUndeleteLoginFormat(long idFormat)
                        {
                            return VirtualDeleteLoginFormat(idFormat, false);
                        }
                        public Boolean PhisicalDeleteLoginFormat(long idFormat)
                        {
                            Boolean result = false;
                            try
                            {
                                Manager.BeginTransaction();
                                LoginFormat loginFormat = Manager.Get<LoginFormat>(idFormat);
                                if (loginFormat != null && !loginFormat.isDefault)
                                    Manager.DeletePhysical(loginFormat);
                                Manager.Commit();
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                result = false;
                            }
                            return result;
                        }
                    #endregion


                    #region "Url Mac Provider"
                        //public Boolean UpdateProviderEncryptionInfo(long idProvider, lm.Comol.Core.Authentication.Helpers.EncryptionInfo dto)
                        //{
                        //    Boolean result = false;
                        //    try
                        //    {
                        //        Manager.BeginTransaction();
                        //        UrlAuthenticationProvider provider = Manager.Get<UrlAuthenticationProvider>(idProvider);
                        //        if (provider != null)
                        //        {
                        //            provider.EncryptionInfo = dto;
                        //            Manager.SaveOrUpdate(provider);
                        //        }
                        //        Manager.Commit();
                        //        result = (provider != null);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Manager.RollBack();
                        //    }
                        //    return result;
                        //}
                        public MacUrlAuthenticationProvider SaveProvider(dtoMacUrlProvider dto)
                        {
                            MacUrlAuthenticationProvider provider = null;
                            try
                            {
                                Manager.BeginTransaction();
                                if (dto.IdProvider > 0)
                                    provider = Manager.Get<MacUrlAuthenticationProvider>(dto.IdProvider);
                                if (provider == null)
                                {
                                    provider = new MacUrlAuthenticationProvider();
                                    provider.Deleted = BaseStatusDeleted.None;
                                }
                                //added on 10/05/2013
                                provider.AllowRequestFromIpAddresses = dto.AllowRequestFromIpAddresses;
                                provider.DenyRequestFromIpAddresses = dto.DenyRequestFromIpAddresses;

                                provider.AllowTaxCodeDuplication = dto.AllowTaxCodeDuplication;
                                provider.AutoAddAgency = dto.AutoAddAgency;
                                provider.AutoEnroll = dto.AutoEnroll;
                                provider.DeltaTime = dto.DeltaTime;                               
                                provider.EncryptionInfo = dto.EncryptionInfo;
                                provider.SenderUrl = dto.SenderUrl;
                                provider.RemoteLoginUrl = dto.RemoteLoginUrl;
                                provider.VerifyRemoteUrl = dto.VerifyRemoteUrl;
                                provider.NotifySubscriptionTo = dto.NotifySubscriptionTo;
                                UpdateProvider(dto, provider);
                                Manager.SaveOrUpdate(provider);

                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                provider = null;
                            }
                            return provider;
                        }

                        public List<UrlMacAttributeType> GetAvailableAttributeTypes(long idProvider) {
                            List<UrlMacAttributeType> types = new List<UrlMacAttributeType>();
                            try
                            {
                                MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                if (provider == null || !provider.Attributes.Where(a=> a.Deleted == BaseStatusDeleted.None && a.Type== UrlMacAttributeType.applicationId).Any())
                                    types.Add(UrlMacAttributeType.applicationId);
                                if (provider == null || !provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.functionId).Any())
                                    types.Add(UrlMacAttributeType.functionId);
                                if (provider == null || !provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.mac).Any())
                                    types.Add(UrlMacAttributeType.mac);

                                if (provider == null || !provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.coursecatalogue).Any())
                                    types.Add(UrlMacAttributeType.coursecatalogue);
                                if (provider == null || !provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.organization).Any())
                                    types.Add(UrlMacAttributeType.organization);
                                if (provider == null || !provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.timestamp).Any())
                                    types.Add(UrlMacAttributeType.timestamp);
                                if (provider == null || !provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.compositeProfile).Any())
                                    types.Add(UrlMacAttributeType.compositeProfile);
                                types.Add(UrlMacAttributeType.profile);
                                types.Add(UrlMacAttributeType.url);
                            }
                            catch (Exception ex) { 

                            }
                            return types;
                        }

                        public BaseUrlMacAttribute AddUrlMacAttribute(long idProvider, BaseUrlMacAttribute nAttribute)
                        {
                            BaseUrlMacAttribute attribute = null;
                            try
                            {
                                Manager.BeginTransaction();
                                Person person = Manager.GetPerson(UC.CurrentUserID);
                                MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                if (provider != null && person != null)
                                {
                                    attribute = nAttribute;
                                    attribute.Provider = provider;
                                    Manager.SaveOrUpdate(attribute);
                                    if (attribute.Name.Contains("{0}"))
                                        attribute.Name = string.Format(attribute.Name, attribute.Id);
                                    provider.Attributes.Add(attribute);
                                }
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                                attribute = null;
                            }
                            return attribute;
                        }
                       
                        private BaseUrlMacAttribute GenerateAttribute(BaseUrlMacAttribute nAttribute) {
                            BaseUrlMacAttribute result = null;

                            switch (nAttribute.Type) { 
                                case UrlMacAttributeType.applicationId:
                                    result = new ApplicationAttribute() { Value = ((ApplicationAttribute)nAttribute).Value };
                                    break;
                                case UrlMacAttributeType.coursecatalogue:
                                    result = new CatalogueAttribute() { AllowMultipleValue = ((CatalogueAttribute)nAttribute).AllowMultipleValue,
                                                                    MultipleValueSeparator = ((CatalogueAttribute)nAttribute).MultipleValueSeparator,
                                                                    };
                                    break;
                                case UrlMacAttributeType.functionId:
                                    result = new FunctionAttribute() { Value = ((FunctionAttribute)nAttribute).Value };
                                    break;
                                case UrlMacAttributeType.mac:
                                    break;
                                case UrlMacAttributeType.organization:
                                    break;
                                case UrlMacAttributeType.profile:
                                    break;
                                case UrlMacAttributeType.timestamp:
                                    break;
                                case UrlMacAttributeType.url:
                                    break;
                                case UrlMacAttributeType.compositeProfile:
                                    result = new CompositeProfileAttribute()
                                    {
                                        MultipleValueSeparator = ((CompositeProfileAttribute)nAttribute).MultipleValueSeparator
                                    };
                                    break;
                            }
                            return result;
                        }
                        public List<ProfileAttributeType> GetUsedMacProfileAttribute(long idProvider)
                        {
                            List<ProfileAttributeType> items = new List<ProfileAttributeType>();
                            try
                            {
                                items = (from a in Manager.GetIQ<UserProfileAttribute>()
                                         where a.Deleted == BaseStatusDeleted.None && a.Provider.Id == idProvider
                                         select a.Attribute).ToList().Distinct().ToList();
                                items.AddRange((from a in Manager.GetIQ<CompositeProfileAttribute>()
                                         where a.Deleted == BaseStatusDeleted.None && a.Provider.Id == idProvider
                                         select a.Attribute).ToList().Distinct().ToList());
                            }
                            catch (Exception ex) { 
                            
                            }
                            return items;
                        }

                        #region "Attributes Manage"
                            public BaseUrlMacAttribute SaveProviderAttribute(long idProvider, long idAttribute, BaseUrlMacAttribute dto, ref Boolean validCodes )
                            {
                                BaseUrlMacAttribute attribute = null;
                                validCodes = true;
                                try
                                {
                                    Manager.BeginTransaction();
                                    Person person = Manager.GetPerson(UC.CurrentUserID);
                                    MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                    BaseUrlMacAttribute att = Manager.Get<BaseUrlMacAttribute>(idAttribute);
                                    if (provider != null && person != null && att != null && dto != null && !String.IsNullOrEmpty(dto.QueryStringName))
                                    {
                                        List<String> remoteCodes = null;
                                        att.Name = dto.Name;
                                        att.Description = dto.Description;
                                        att.QueryStringName = dto.QueryStringName;
                                        switch (att.Type) { 
                                            case UrlMacAttributeType.applicationId:
                                                ((ApplicationAttribute)att).Value = ((ApplicationAttribute)dto).Value;
                                                break;
                                            case UrlMacAttributeType.compositeProfile:
                                                ((CompositeProfileAttribute)att).Attribute = ((CompositeProfileAttribute)dto).Attribute;
                                                ((CompositeProfileAttribute)att).MultipleValueSeparator = ((CompositeProfileAttribute)dto).MultipleValueSeparator;
                                                break;
                                            case UrlMacAttributeType.coursecatalogue:
                                                CatalogueAttribute cAttribute = (CatalogueAttribute)dto;
                                                remoteCodes = cAttribute.Items.Select(i => i.RemoteCode).ToList();
                                                validCodes = IsValidCodes(remoteCodes);

                                                ((CatalogueAttribute)att).AllowMultipleValue = cAttribute.AllowMultipleValue;
                                                ((CatalogueAttribute)att).MultipleValueSeparator = cAttribute.MultipleValueSeparator;

                                                foreach (CatalogueAttributeItem item in ((CatalogueAttribute)att).Items)
                                                {
                                                    CatalogueAttributeItem oItem = cAttribute.Items.Where(o => o.Id == item.Id).FirstOrDefault();
                                                    if (oItem == null && item.Deleted == BaseStatusDeleted.None)
                                                    {
                                                        item.Deleted = BaseStatusDeleted.Manual;
                                                        Manager.SaveOrUpdate(item);
                                                    }
                                                    else if (oItem != null && item.Deleted == BaseStatusDeleted.None && (validCodes || (!validCodes && remoteCodes.Where(r => r == oItem.RemoteCode).Count() <= 0)))
                                                    {
                                                        item.RemoteCode = oItem.RemoteCode;
                                                        Manager.SaveOrUpdate(item);
                                                    }
                                                    Manager.SaveOrUpdate(item);
                                                }

                                                break;
                                            case UrlMacAttributeType.functionId:
                                                ((FunctionAttribute)att).Value = ((FunctionAttribute)dto).Value;
                                                break;
                                            case UrlMacAttributeType.mac:
                                                //((MacAttribute)att). = ((MacAttribute)dto).Value;
                                                break;
                                            case UrlMacAttributeType.organization:
                                                OrganizationAttribute oAttribute = (OrganizationAttribute)dto;
                                                ((OrganizationAttribute)att).AllowMultipleValue = oAttribute.AllowMultipleValue;
                                                ((OrganizationAttribute)att).MultipleValueSeparator = oAttribute.MultipleValueSeparator;
                                                remoteCodes = oAttribute.Items.Select(i => i.RemoteCode).ToList();
                                                validCodes = IsValidCodes(remoteCodes);
                                                
                                                foreach (OrganizationAttributeItem item in ((OrganizationAttribute)att).Items)
                                                {
                                                    OrganizationAttributeItem oItem = oAttribute.Items.Where(o => o.Id == item.Id).FirstOrDefault();
                                                    if (oItem == null && item.Deleted == BaseStatusDeleted.None)
                                                    {
                                                        item.Deleted = BaseStatusDeleted.Manual;
                                                        Manager.SaveOrUpdate(item);
                                                    }
                                                    else if (oItem!=null && item.Deleted == BaseStatusDeleted.None && (validCodes || (!validCodes && remoteCodes.Where(r => r == oItem.RemoteCode).Count() <= 0)))
                                                    {
                                                        item.RemoteCode = oItem.RemoteCode;
                                                        item.IdDefaultPage = oItem.IdDefaultPage;
                                                        item.IdDefaultProfile = oItem.IdDefaultProfile;
                                                        Manager.SaveOrUpdate(item);
                                                    }
                                                }

                                                /*foreach (OrganizationAttributeItem item in ((OrganizationAttribute)att).Items.Where(i => !String.IsNullOrEmpty(i.RemoteCode) && remoteCodes.Where(r => r == i.RemoteCode).Count() <= 1).ToList())
                                                {
                                                    OrganizationAttributeItem oItem = oAttribute.Items.Where(o => o.Id == item.Id).FirstOrDefault();
                                                    if (oItem == null)
                                                        item.Deleted = BaseStatusDeleted.Manual;
                                                    else
                                                    {
                                                        item.RemoteCode = oItem.RemoteCode;
                                                        item.IdDefaultPage = oItem.IdDefaultPage;
                                                        item.IdDefaultProfile = oItem.IdDefaultProfile;
                                                    }
                                                    Manager.SaveOrUpdate(item);
                                                }*/
                                                break;
                                            case UrlMacAttributeType.profile:
                                                ((UserProfileAttribute)att).Attribute = ((UserProfileAttribute)dto).Attribute;
                                                break;
                                            case UrlMacAttributeType.timestamp:
                                                ((TimestampAttribute)att).Format = ((TimestampAttribute)dto).Format;
                                                ((TimestampAttribute)att).UserFormat = ((TimestampAttribute)dto).UserFormat;
                                                break;
                                        }
                                        Manager.SaveOrUpdate(att);
                                    }
                                    Manager.Commit();
                                    attribute = att;
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                    attribute = null;
                                }
                                return attribute;
                            }
                            private Boolean IsValidCodes(List<String> remoteCodes) {
                                List<String> toValidate = remoteCodes.Where(r => !String.IsNullOrEmpty(r) && !String.IsNullOrEmpty(r.Trim())).ToList();
                                return (toValidate.Distinct().Count() == remoteCodes.Count());
                            }
                            public Boolean VirtualDeleteMacAttribute(long idProvider, long idAttribute, Boolean delete)
                            {
                                Boolean result = false;
                                try
                                {
                                    Person person = Manager.GetPerson(UC.CurrentUserID);
                                    MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                    BaseUrlMacAttribute attribute = Manager.Get<BaseUrlMacAttribute>(idAttribute);
                                    if (attribute != null && provider != null && provider.Id == attribute.Provider.Id)
                                    {
                                        Manager.BeginTransaction();
                                        attribute.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                        switch (attribute.Type)
                                        {
                                            case UrlMacAttributeType.mac:
                                                foreach (MacAttributeItem option in ((MacAttribute)attribute).Items)
                                                {
                                                    option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                                    Manager.SaveOrUpdate(option);
                                                }
                                                break;
                                            case UrlMacAttributeType.compositeProfile:
                                                foreach (CompositeAttributeItem option in ((CompositeProfileAttribute)attribute).Items)
                                                {
                                                    option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                                    Manager.SaveOrUpdate(option);
                                                }
                                                break;
                                            case UrlMacAttributeType.coursecatalogue:
                                                foreach (CatalogueAttributeItem option in ((CatalogueAttribute)attribute).Items)
                                                {
                                                    option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                                    Manager.SaveOrUpdate(option);
                                                }
                                                break;
                                            case UrlMacAttributeType.organization:
                                                foreach (OrganizationAttributeItem option in ((OrganizationAttribute)attribute).Items)
                                                {
                                                    option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                                    Manager.SaveOrUpdate(option);
                                                }
                                                break;
                                        }
                                        if (delete && attribute.Type != UrlMacAttributeType.mac && attribute.Type != UrlMacAttributeType.compositeProfile)
                                        {
                                            foreach (var cAtt in provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.mac).ToList()) {
                                                if (((MacAttribute)cAtt).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Attribute.Id == attribute.Id).Any()) {
                                                    foreach (MacAttributeItem option in ((MacAttribute)cAtt).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Attribute.Id == attribute.Id))
                                                    {
                                                        option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                                        Manager.SaveOrUpdate(option);
                                                    }
                                                }

                                            }
                                            foreach (var cmpAtt in provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type == UrlMacAttributeType.compositeProfile).ToList())
                                            {
                                                if (((CompositeProfileAttribute)cmpAtt).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Attribute.Id == attribute.Id).Any())
                                                {
                                                    foreach (CompositeAttributeItem option in ((CompositeProfileAttribute)cmpAtt).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Attribute.Id == attribute.Id))
                                                    {
                                                        option.Deleted = delete ? (option.Deleted | BaseStatusDeleted.Cascade) : (option.Deleted = (BaseStatusDeleted)((int)option.Deleted - (int)BaseStatusDeleted.Cascade));
                                                        Manager.SaveOrUpdate(option);
                                                    }
                                                }

                                            }
                                        }
                                        Manager.SaveOrUpdate(attribute);
                                        Manager.Commit();
                                        result = true;

                                    }
                                    else
                                        result = true;
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                }
                                return result;
                            }
                            public Boolean VirtualDeleteUrlMacAttributeItem(long idAttribute, long idItem, UrlMacAttributeType type, ref long pIdAttributeItem, Boolean delete)
                            {
                                Boolean result = false;
                                try
                                {
                                    Manager.BeginTransaction();
                                    Person person = Manager.GetPerson(UC.CurrentUserID);
                                    BaseUrlMacAttribute attribute = Manager.Get<BaseUrlMacAttribute>(idAttribute);
                                    if (attribute != null)
                                    {
                                        switch (type)
                                        {
                                            case UrlMacAttributeType.mac:
                                                MacAttributeItem aItem = Manager.Get<MacAttributeItem>(idItem);
                                                if (aItem != null && attribute != null)
                                                {
                                                    aItem.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                                    Manager.SaveOrUpdate(aItem);

                                                    int displayOrder = 1;
                                                    ((MacAttribute)attribute).Items.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder).ThenBy(i => i.Attribute.Name).ToList().ForEach(i => i.DisplayOrder = displayOrder++);

                                                    pIdAttributeItem = ((MacAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Id < idItem).OrderByDescending(a => a.Id).Select(a => a.Id).FirstOrDefault();
                                                    if (pIdAttributeItem == 0)
                                                        pIdAttributeItem = ((MacAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.Id).FirstOrDefault();
                                                    
                                                }
                                                else
                                                    result = true;
                                                break;
                                            case UrlMacAttributeType.compositeProfile:
                                                CompositeAttributeItem cmItem = Manager.Get<CompositeAttributeItem>(idItem);
                                                if (cmItem != null && attribute != null)
                                                {
                                                    cmItem.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                                    Manager.SaveOrUpdate(cmItem);

                                                    int displayOrder = 1;
                                                    ((CompositeProfileAttribute)attribute).Items.Where(i => i.Deleted == BaseStatusDeleted.None).OrderBy(i => i.DisplayOrder).ThenBy(i => i.Attribute.Name).ToList().ForEach(i => i.DisplayOrder = displayOrder++);

                                                    pIdAttributeItem = ((CompositeProfileAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Id < idItem).OrderByDescending(a => a.Id).Select(a => a.Id).FirstOrDefault();
                                                    if (pIdAttributeItem == 0)
                                                        pIdAttributeItem = ((CompositeProfileAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None).Select(a => a.Id).FirstOrDefault();

                                                }
                                                else
                                                    result = true;
                                                break;
                                            case UrlMacAttributeType.coursecatalogue:
                                                CatalogueAttributeItem cItem = Manager.Get<CatalogueAttributeItem>(idItem);
                                                if (cItem != null && attribute != null)
                                                {
                                                    cItem.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                                    Manager.SaveOrUpdate(cItem);

                                                    pIdAttributeItem = ((CatalogueAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Catalogue !=null && a.Id < idItem).OrderBy(a=> a.Catalogue.Name).Select(a => a.Id).FirstOrDefault();
                                                    if (pIdAttributeItem == 0)
                                                        pIdAttributeItem = ((CatalogueAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Catalogue != null).OrderBy(a => a.Catalogue.Name).Select(a => a.Id).FirstOrDefault();
                                                    
                                                }
                                                else
                                                    result = true;
                                                break;
                                            case UrlMacAttributeType.organization:
                                                OrganizationAttributeItem oItem = Manager.Get<OrganizationAttributeItem>(idItem);
                                                if (oItem != null && attribute != null)
                                                {
                                                    int idOrganization = (oItem.Organization==null) ? 0 : oItem.Organization.Id;
                                                    oItem.Deleted = delete ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                                                    Manager.SaveOrUpdate(oItem);

                                                    pIdAttributeItem = ((OrganizationAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Organization != null && a.Id < idItem).OrderBy(a => a.Organization.Name).Select(a => a.Id).FirstOrDefault();
                                                    if (pIdAttributeItem == 0)
                                                        pIdAttributeItem = ((OrganizationAttribute)attribute).Items.Where(a => a.Deleted == BaseStatusDeleted.None && a.Organization != null).OrderBy(a => a.Organization.Name).Select(a => a.Id).FirstOrDefault();
                                                    
                                                }
                                                else
                                                    result = true;
                                                break;
                                        }
                                    }
                                    Manager.Commit();
                                    result = true;
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                }
                                return result;
                            }
                            public long GetPreviousMacAttribute(long idProvider, long idAttribute)
                            {
                                long result = 0;
                                try
                                {
                                    Person person = Manager.GetPerson(UC.CurrentUserID);
                                    MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                    if (provider != null)
                                    {
                                        result = provider.Attributes.Where(a => a.Deleted != BaseStatusDeleted.None && a.Id < idAttribute).Select(a => a.Id).Max();
                                        if (result == 0)
                                            result = provider.Attributes.Where(a => a.Deleted != BaseStatusDeleted.None).Select(a => a.Id).Min();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Manager.RollBack();
                                }
                                return result;
                            }

                            public Boolean MacAttributeExist(long idAttribute) {
                                Boolean found = false;
                                try
                                {
                                    Manager.BeginTransaction();
                                    found = (from a in Manager.GetIQ<BaseUrlMacAttribute>() where a.Id == idAttribute && a.Deleted == BaseStatusDeleted.None select a.Id).Any();
                                    Manager.Commit();
                                }
                                catch (Exception ex) {
                                    found = false;
                                    Manager.RollBack();
                                }
                                return found;
                            }
                            #region "Mac Attributes"
                                public List<BaseUrlMacAttribute> GetAvailableAttributesForMac(long idProvider, long idMacAttribute)
                                {
                                    List<BaseUrlMacAttribute> items = new List<BaseUrlMacAttribute>();
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                        if (provider != null)
                                        {
                                            MacAttribute mAttribute = Manager.Get<MacAttribute>(idMacAttribute);
                                            items = provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && a.Type != UrlMacAttributeType.mac && (mAttribute == null || (!mAttribute.Items.Where(i => i.Deleted == BaseStatusDeleted.None).Select(i => i.Attribute.Id).Contains(a.Id)))).OrderBy(a => a.Name).ToList();
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                    }
                                    return items;
                                }
                                public MacAttributeItem AddMacAttributeItem(long idAttribute, long idAttributeItem)
                                {
                                    MacAttributeItem item = null;
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        MacAttribute owner = Manager.Get<MacAttribute>(idAttribute);
                                        BaseUrlMacAttribute att = Manager.Get<BaseUrlMacAttribute>(idAttributeItem);
                                        if (owner != null && att != null)
                                        {
                                            Boolean added = false;
                                            int displayOrder = 1;
                                            owner.Items.Where(i => i.Deleted == BaseStatusDeleted.None).ToList().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Attribute.Name).ToList().ForEach(i => i.DisplayOrder = displayOrder++);

                                            item = owner.Items.Where(i => i.Attribute.Id == idAttributeItem).FirstOrDefault();
                                            if (item == null)
                                            {
                                                item = new MacAttributeItem();
                                                item.Attribute = att;
                                                item.Owner = owner;
                                                added = true;
                                            }
                                            else
                                                item.Deleted = BaseStatusDeleted.None;

                                            item.DisplayOrder = displayOrder;
                                            Manager.SaveOrUpdate(item);
                                            if (added)
                                            {
                                                owner.Items.Add(item);
                                                Manager.SaveOrUpdate(owner);
                                            }
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                        item = null;
                                    }
                                    return item;
                                }
                                public Boolean UpdateMacAttributeItemsDisplayOrder(List<long> idAttributes)
                                {
                                    Person person = Manager.GetPerson(UC.CurrentUserID);
                                    if (person != null)
                                    {
                                        DateTime CurrentTime = DateTime.Now;
                                        try
                                        {
                                            Manager.BeginTransaction();
                                            int displayOrder = 1;
                                            MacAttributeItem attribute;
                                            foreach (var idItem in idAttributes)
                                            {
                                                attribute = Manager.Get<MacAttributeItem>(idItem);
                                                if (attribute != null)
                                                {
                                                    attribute.DisplayOrder = displayOrder;
                                                    Manager.SaveOrUpdate(attribute);
                                                    displayOrder++;
                                                }
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
                                    return false;
                                }
                            #endregion

                            #region "Composite Attributes"
                                public List<BaseUrlMacAttribute> GetAvailableAttributesForComposite(long idProvider, long idAttribute)
                                {
                                    List<BaseUrlMacAttribute> items = new List<BaseUrlMacAttribute>();
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                        if (provider != null)
                                        {
                                            CompositeProfileAttribute cAttribute = Manager.Get<CompositeProfileAttribute>(idAttribute);
                                            items = provider.Attributes.Where(a => a.Deleted == BaseStatusDeleted.None && (a.Type == UrlMacAttributeType.profile || a.Type == UrlMacAttributeType.url)  && (cAttribute == null || (!cAttribute.Items.Where(i => i.Deleted == BaseStatusDeleted.None).Select(i => i.Attribute.Id).Contains(a.Id)))).OrderBy(a => a.Name).ToList();
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                    }
                                    return items;
                                }
                                public CompositeAttributeItem AddCompositeAttributeItem(long idAttribute, long idAttributeItem)
                                {
                                    CompositeAttributeItem item = null;
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        CompositeProfileAttribute owner = Manager.Get<CompositeProfileAttribute>(idAttribute);
                                        BaseUrlMacAttribute att = Manager.Get<BaseUrlMacAttribute>(idAttributeItem);
                                        if (owner != null && att != null)
                                        {
                                            Boolean added = false;
                                            int displayOrder = 1;
                                            owner.Items.Where(i => i.Deleted == BaseStatusDeleted.None).ToList().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Attribute.Name).ToList().ForEach(i => i.DisplayOrder = displayOrder++);

                                            item = owner.Items.Where(i => i.Attribute.Id == idAttributeItem).FirstOrDefault();
                                            if (item == null)
                                            {
                                                item = new CompositeAttributeItem();
                                                item.Attribute = att;
                                                item.Owner = owner;
                                                added = true;
                                            }
                                            else
                                                item.Deleted = BaseStatusDeleted.None;

                                            item.DisplayOrder = displayOrder;
                                            Manager.SaveOrUpdate(item);
                                            if (added)
                                            {
                                                owner.Items.Add(item);
                                                Manager.SaveOrUpdate(owner);
                                            }
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                        item = null;
                                    }
                                    return item;
                                }
                                public Boolean UpdateCompositeAttributeItemsDisplayOrder(List<long> idAttributes)
                                {
                                    Person person = Manager.GetPerson(UC.CurrentUserID);
                                    if (person != null)
                                    {
                                        DateTime CurrentTime = DateTime.Now;
                                        try
                                        {
                                            Manager.BeginTransaction();
                                            int displayOrder = 1;
                                            CompositeAttributeItem attribute;
                                            foreach (var idItem in idAttributes)
                                            {
                                                attribute = Manager.Get<CompositeAttributeItem>(idItem);
                                                if (attribute != null)
                                                {
                                                    attribute.DisplayOrder = displayOrder;
                                                    Manager.SaveOrUpdate(attribute);
                                                    displayOrder++;
                                                }
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
                                    return false;
                                }
                            #endregion

                            #region "Organization Attributes"
                                public Dictionary<Int32, String> GetAvailableItemsForOrganizationAttribute(long idProvider, long idAttribute)
                                {
                                    Dictionary<Int32, String> items = new Dictionary<Int32, String>();
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                        if (provider != null)
                                        {
                                            OrganizationAttribute mAttribute = Manager.Get<OrganizationAttribute>(idAttribute);
                                            List<Int32> idOrganizations = mAttribute.Items.Where(o => o.Deleted == BaseStatusDeleted.None && o.Organization != null).Select(o => o.Organization.Id).ToList();
                                            items = (from o in Manager.GetIQ<Organization>()
                                                     where !idOrganizations.Contains(o.Id)
                                                     orderby o.Name
                                                     select o).ToDictionary(o => o.Id, o => o.Name);
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                    }
                                    return items;
                                }
                                public Dictionary<long, String> GetAvailableModulePages(Int32 idLanguage)
                                {
                                    Dictionary<long, String> items = new Dictionary<long, String>();
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        List<ModulePage> pages = (from p in Manager.GetIQ<ModulePage>()
                                                                  where p.Deleted == BaseStatusDeleted.None && p.isForPortal
                                                                  select p).ToList();
                                        items = pages.ToDictionary(p => p.Id, p => p.Name);
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                    }
                                    return items;
                                }
                                public Boolean isUniqueOrganizationCode(long idAttribute, Int32 idOrganization, String remoteCode)
                                {
                                    Boolean result = true;
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        OrganizationAttribute owner = Manager.Get<OrganizationAttribute>(idAttribute);
                                        if (owner != null)
                                            result = owner.Items.Where(i => i.Deleted == BaseStatusDeleted.None && i.Organization != null && i.Organization.Id != idOrganization && i.RemoteCode == remoteCode).Any();
                                        else
                                            result = true;
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                        result = false;
                                    }
                                    return result;
                                }
                                public OrganizationAttributeItem AddOrganizationAttributeItem(long idAttribute, Int32 idOrganization, long idDefaultPage, Int32 idDefaultProfile, String remoteCode)
                                {
                                    OrganizationAttributeItem item = null;
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        OrganizationAttribute owner = Manager.Get<OrganizationAttribute>(idAttribute);
                                        Organization org = Manager.Get<Organization>(idOrganization);
                                        if (owner != null && org != null && owner.isUniqueCode(remoteCode))
                                        {
                                            Boolean added = false;
                                            item = owner.Items.Where(i => i.Organization != null && i.Organization.Id == idOrganization).FirstOrDefault();
                                            if (item == null)
                                            {
                                                item = new OrganizationAttributeItem();
                                                item.Organization = org;
                                                item.Owner = owner;
                                                added = true;
                                            }
                                            else
                                                item.Deleted = BaseStatusDeleted.None;

                                            item.IdDefaultPage = idDefaultPage;
                                            item.IdDefaultProfile = idDefaultProfile;
                                            item.RemoteCode = remoteCode;

                                            Manager.SaveOrUpdate(item);
                                            if (added)
                                            {
                                                owner.Items.Add(item);
                                                Manager.SaveOrUpdate(owner);
                                            }
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                        item = null;
                                    }
                                    return item;
                                }
                            #endregion

                            #region "Catalogue Attributes"
                                public Dictionary<long, String> GetAvailableItemsForCatalogueAttribute(long idProvider, long idAttribute, Int32 idLanguage)
                                {
                                    Dictionary<long, String> items = new Dictionary<long, String>();
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        MacUrlAuthenticationProvider provider = Manager.Get<MacUrlAuthenticationProvider>(idProvider);
                                        if (provider != null)
                                        {
                                            CatalogueAttribute mAttribute = Manager.Get<CatalogueAttribute>(idAttribute);
                                            List<long> idCatalogues = mAttribute.Items.Where(o => o.Deleted == BaseStatusDeleted.None && o.Catalogue != null).Select(o => o.Catalogue.Id).ToList();
                                            items = (from c in Manager.GetIQ<Catalogues.Catalogue>()
                                                     where !idCatalogues.Contains(c.Id) && c.Deleted == BaseStatusDeleted.None && c.isEnabled
                                                     orderby c.Name
                                                     select c).ToDictionary(o => o.Id, o => o.Name);
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                    }
                                    return items;
                                }
                                public Boolean isUniqueCatalogueCode(long idAttribute, long idCatalogue, String remoteCode)
                                {
                                    Boolean result = true;
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        CatalogueAttribute owner = Manager.Get<CatalogueAttribute>(idAttribute);
                                        if (owner != null)
                                            result = owner.Items.Where(i => i.Deleted == BaseStatusDeleted.None && i.Catalogue != null && i.Catalogue.Id != idCatalogue && i.RemoteCode == remoteCode).Any();
                                        else
                                            result = true;
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                        result = false;
                                    }
                                    return result;
                                }
                                public CatalogueAttributeItem AddCatalogueAttributeItem(long idAttribute, long idCatalogue, String remoteCode)
                                {
                                    CatalogueAttributeItem item = null;
                                    try
                                    {
                                        Manager.BeginTransaction();
                                        CatalogueAttribute owner = Manager.Get<CatalogueAttribute>(idAttribute);
                                        Catalogues.Catalogue cat = Manager.Get<Catalogues.Catalogue>(idCatalogue);
                                        if (owner != null && cat != null && owner.isUniqueCode(remoteCode))
                                        {
                                            Boolean added = false;
                                            item = owner.Items.Where(i => i.Catalogue != null && i.Catalogue.Id == idCatalogue).FirstOrDefault();
                                            if (item == null)
                                            {
                                                item = new CatalogueAttributeItem();
                                                item.Catalogue = cat;
                                                item.Owner = owner;
                                                added = true;
                                            }
                                            else
                                                item.Deleted = BaseStatusDeleted.None;
                                            item.RemoteCode = remoteCode;

                                            Manager.SaveOrUpdate(item);
                                            if (added)
                                            {
                                                owner.Items.Add(item);
                                                Manager.SaveOrUpdate(owner);
                                            }
                                        }
                                        Manager.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        Manager.RollBack();
                                        item = null;
                                    }
                                    return item;
                                }
                            #endregion
                        #endregion
                        
                 
                    #endregion


                    public InternalAuthenticationProvider SaveProvider(dtoInternalProvider dto)
                    {
                        InternalAuthenticationProvider provider = null;
                        try
                        {
                            Manager.BeginTransaction();
                            if (dto.IdProvider > 0)
                                provider = Manager.Get<InternalAuthenticationProvider>(dto.IdProvider);
                            if (provider == null)
                            {
                                provider = new InternalAuthenticationProvider();
                                provider.Deleted = BaseStatusDeleted.None;
                            }
                            provider.ChangePasswordAfterDays = dto.ChangePasswordAfterDays;
                            UpdateProvider(dto, provider);
                            Manager.SaveOrUpdate(provider);
                            Manager.Commit();
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            provider = null;
                        }
                        return provider;
                    }
                    
                    private void UpdateProvider(dtoProvider dto, AuthenticationProvider provider)
                    {
                        provider.AllowAdminProfileInsert = dto.AllowAdminProfileInsert;
                        provider.AllowMultipleInsert = dto.AllowMultipleInsert;
                        provider.Deleted = BaseStatusDeleted.None;
                        provider.DisplayToUser = dto.DisplayToUser;
                        provider.IdentifierFields = dto.IdentifierFields;
                        provider.IsEnabled = dto.isEnabled;
                        provider.LogoutMode = dto.LogoutMode;
                        provider.MultipleItemsForRecord = dto.MultipleItemsForRecord;
                        provider.MultipleItemsSeparator = dto.MultipleItemsSeparator;
                        provider.Name = dto.Name;
                        provider.ProviderType = dto.Type;
                        provider.UniqueCode = dto.UniqueCode;
                        
                     }

                    public Boolean VirtualDeleteItem(long idProvider,Boolean delete)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                        if (provider != null) {
                            provider.Deleted = (delete == true) ? BaseStatusDeleted.Manual : BaseStatusDeleted.None;
                            result = true;
                        }
                        Manager.Commit();
                    }
                    catch (Exception ex) {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                    public Boolean VirtualDelete(long idProvider) {
                        return VirtualDeleteItem(idProvider, true);
                    }
                    public Boolean VirtualUndelete(long idProvider) {
                        return VirtualDeleteItem(idProvider, false);
                    }
                    public Boolean PhisicalDelete(long idProvider)
                {
                    Boolean result = false;
                    try
                    {
                        Manager.BeginTransaction();
                        AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                        if (provider != null)
                            Manager.DeletePhysical(provider);
                        Manager.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Manager.RollBack();
                        result = false;
                    }
                    return result;
                }
                    public Boolean Enable(long idProvider, Boolean enable)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            AuthenticationProvider provider = Manager.Get<AuthenticationProvider>(idProvider);
                            if (provider != null){
                                provider.IsEnabled= enable;
                                Manager.SaveOrUpdate(provider);
                            }
                            Manager.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            result = false;
                        }
                        return result;
                    }
                    public Boolean ProviderExist(long idProvider)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            result = (from p in Manager.GetIQ<AuthenticationProvider>() where p.Id == idProvider select p.Id).Any();
                            Manager.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            result = false;
                        }
                        return result;
                    }
                    public Boolean ProviderTypeExist(AuthenticationProviderType type)
                    {
                        Boolean result = false;
                        try
                        {
                            Manager.BeginTransaction();
                            result = (from p in Manager.GetIQ<AuthenticationProvider>() where p.Deleted== BaseStatusDeleted.None && p.ProviderType== type select p.Id).Any();
                            Manager.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Manager.RollBack();
                            result = false;
                        }
                        return result;
                    }
                #endregion
            #endregion
    }
}
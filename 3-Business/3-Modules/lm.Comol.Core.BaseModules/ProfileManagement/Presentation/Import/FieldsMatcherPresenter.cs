using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.BaseModules.ProfileManagement.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public class FieldsMatcherPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        private ProfileManagementService _Service;
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
        protected virtual IViewFieldsMatcher View
        {
            get { return (IViewFieldsMatcher)base.View; }
        }
        private ProfileManagementService Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ProfileManagementService(AppContext);
                return _Service;
            }
        }
        public FieldsMatcherPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public FieldsMatcherPresenter(iApplicationContext oContext, IViewFieldsMatcher view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(List<ProfileColumnComparer<String>> columns)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.AutoGenerateLogin = false;
                List<lm.Comol.Core.BaseModules.ProviderManagement.dtoBaseProvider>providers = Service.GetAuthenticationProviders(UserContext.Language.Id,true);
                View.InitializeControl(providers);
                Int32 idType = View.SelectedProfileTypeId;
                View.Columns = columns;
                View.LoadItems(columns,Service.GetAvailableAttributeTypes(idType,(providers.Count>0) ? View.SelectedIdProvider : 0, View.AddTaxCode,View.AddPassword, false ));
            }
        }

        public void ReloadItems(Int32 idType, long idProvider, Boolean addTaxCode, Boolean addPassword, Boolean autoGenerateLogin)
        {
            View.ReloadAvailableAttributes(View.Columns,Service.GetAvailableAttributeTypes(idType, idProvider, addTaxCode, addPassword, autoGenerateLogin));
        }

        public Boolean isValid(List<ProfileColumnComparer<String>> columns) {
            List<dtoInvalidMatch<lm.Comol.Core.Authentication.ProfileAttributeType>> items = GetInvalidItems(columns);

            return (items == null || items.Count == 0 || !items.Where(d => d.InvalidMatch != InvalidMatch.None).Any());
        }

        public List<dtoInvalidMatch<lm.Comol.Core.Authentication.ProfileAttributeType>> GetInvalidItems(List<ProfileColumnComparer<String>> columns)
        {
            List<dtoInvalidMatch<ProfileAttributeType>> items = new List<dtoInvalidMatch<ProfileAttributeType>>();
            List<dtoProfileAttributeType> mandatoryAttributes = Service.GetProfileTypeBaseAttributes(View.SelectedProfileTypeId, View.SelectedIdProvider, View.AddTaxCode, View.AddPassword, View.AutoGenerateLogin);
            mandatoryAttributes = mandatoryAttributes.Where(a => a.Mandatory).ToList();
            // Retrieve required item
            List<ProfileAttributeType> baseAttributes = mandatoryAttributes.Select(a=>a.Attribute).ToList();

            Boolean valid = !(columns.Count== columns.Where(c=>!c.isValid).Count());
            if (valid){
                List<ProfileAttributeType> usedAttributes = columns.Where(c => c.isValid).Select(c => c.DestinationColumn).ToList();

                valid = (baseAttributes.Count > 0 && usedAttributes.Count > 0);
                if (valid)
                {
                    var query = usedAttributes.GroupBy(a => a).Select(a => new { Type = a, Count = a.Count() });
                    if (query.Where(c => c.Count > 1).Any())
                        items.AddRange(query.Where(c => c.Count > 1).ToList().Select(c => new dtoInvalidMatch<lm.Comol.Core.Authentication.ProfileAttributeType>() { Attribute = c.Type.FirstOrDefault(), InvalidMatch = InvalidMatch.DuplicatedItem }).ToList());
                    
                    List<ProfileAttributeType> alternatives = mandatoryAttributes.Where(a => a.HasAlternative).Select(a => a.Attribute).ToList();

                    items.AddRange(baseAttributes.Where(a => !alternatives.Contains(a) && !usedAttributes.Contains(a)).Select(c => new dtoInvalidMatch<ProfileAttributeType>() { Attribute = c, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());
                    if (mandatoryAttributes.Where(a => a.HasAlternative).Any())
                    {
                        // find all attributes without alternatives
                        List<ProfileAttributeType> alternativeAttributes = new List<ProfileAttributeType>();

                        foreach (ProfileAttributeType alternative in alternatives) {
                            // Get alternative attributes
                            alternativeAttributes = mandatoryAttributes.Where(ma => ma.Attribute == alternative).Select(ma => ma.AlternativeAttributes).FirstOrDefault();
                            if ((alternativeAttributes == null || alternativeAttributes.Count==0) && !usedAttributes.Contains(alternative))
                                items.Add(new dtoInvalidMatch<ProfileAttributeType>() { Attribute = alternative, InvalidMatch = InvalidMatch.IgnoredRequiredItem });
                            else if (alternativeAttributes != null && !usedAttributes.Contains(alternative) && usedAttributes.Where(ua=>alternativeAttributes.Contains(ua)).Any() ==false  )
                                items.Add(new dtoInvalidMatch<ProfileAttributeType>() { Attribute = alternative, InvalidMatch = InvalidMatch.IgnoredAlternativeRequiredItem });
                        }

                        //alternatives.ForEach(a=> 
                        

                        //List<ProfileAttributeType> ignoredAttributes = baseAttributes.Except(mandatoryAttributes.Where(a => !a.HasAlternative).Select(a => a.Attribute).ToList()).Except(usedAttributes.Distinct()).ToList();
                        
                        //mandatoryAttributes.Where(a => ignoredAttributes.Contains(a.Attribute)).ToList().ForEach(a => alternativeAttributes.AddRange(a.AlternativeAttributes));

                        
                    }
                }
                else
                    items.AddRange(baseAttributes.Select(a => new dtoInvalidMatch<lm.Comol.Core.Authentication.ProfileAttributeType>() { Attribute = a, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());
            }
            else
                items.AddRange(baseAttributes.Select(a => new dtoInvalidMatch<lm.Comol.Core.Authentication.ProfileAttributeType>() { Attribute = a, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());
            return items;
        }

        public KeyValuePair<long, String> GetDefaultAgency() {
            return Service.GetEmptyAgency(0);
        }

        //public Boolean ValidateColumns(List<ProfileColumnComparer<String>> columns, ref  Dictionary<InvalidMatch, List<lm.Comol.Core.Authentication.ProfileAttributeType>> invalidColumns)
        //{
        //    Boolean valid = !(columns.Count == columns.Where(c => !c.isValid).Count());
        //    if (valid)
        //    {
        //        List<lm.Comol.Core.Authentication.ProfileAttributeType> usedAttributes = columns.Where(c => c.isValid).Select(c => c.DestinationColumn).ToList();
        //        List<lm.Comol.Core.Authentication.ProfileAttributeType> attributes = Service.GetProfileMandatoryAttributes(View.SelectedProfileTypeId, View.SelectedIdProvider, View.AddTaxCode, View.AddPassword, View.AutoGenerateLogin);
        //        valid = (attributes.Count > 0 && usedAttributes.Count > 0);
        //        if (valid)
        //        {
        //            var query = usedAttributes.GroupBy(a => a).Select(a => new { Type = a, Count = a.Count() });
        //            if (query.Where(c => c.Count == 0).Any())
        //            {
        //                invalidColumns.Add( (query.Where(c => c.Count == 0).Count() == 1) ? InvalidMatch.IgnoredRequiredItem : InvalidMatch.IgnoredRequiredItems
        //                , query.Where(c => c.Count == 0).Select(c=>c.Type.Key).ToList());
        //                 valid = false;
        //            }
        //            if (query.Where(c => c.Count > 1).Any()){
        //                invalidColumns.Add( (query.Where(c => c.Count >1).Count() == 1) ? InvalidMatch.DuplicatedItem : InvalidMatch.DuplicatedItems
        //                , query.Where(c => c.Count >1).Select(c=>c.Type.Key).ToList());
        //                 valid = false;
        //            }
        //            if (attributes.Where(a=> !usedAttributes.Contains(a)).Any()){
        //                invalidColumns.Add((attributes.Where(a => !usedAttributes.Contains(a)).Count() == 1) ? InvalidMatch.IgnoredRequiredItem : InvalidMatch.IgnoredRequiredItems
        //                   , attributes.Where(a=> !usedAttributes.Contains(a)).ToList());
        //                valid = false;
        //            }
        //        }
        //        else
        //            invalidColumns.Add(InvalidMatch.IgnoredAllItems, attributes);
        //    }
        //    return valid;
        //}
    }
}

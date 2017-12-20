using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dss.Domain.Templates;
using lm.Comol.Core.Dss.Domain;

namespace lm.Comol.Core.Dss.Business
{
    public partial class ServiceDss : CoreServices
    {
        protected const Int32 maxItemsForQuery = 500;
        #region initClass
        public ServiceDss() :base() { }
        public ServiceDss(iApplicationContext oContext)
            : base(oContext)
        {

        }
        public ServiceDss(iDataContext oDC)
            : base(oDC)
        {

        }
    #endregion

        public TemplateMethod MethodGet(long idMethod)
        {
            return Manager.Get<TemplateMethod>(idMethod);
        }
        public Boolean MethodIsFuzzy(long idMethod)
        {
            TemplateMethod m = MethodGet(idMethod);
            return (m != null && m.IsFuzzy);
        }
        public dtoSelectMethod MethodGetAvailable(long idMethod,Int32 idLanguage)
        {
            return MethodGetAvailable(idMethod,idLanguage, Manager.GetDefaultIdLanguage());
        }
        public List<long> MethodGetIdAvailable(AlgorithmType type, Boolean isFuzzy)
        {
            return (from m in Manager.GetIQ<TemplateMethod>()
                    where m.Deleted == BaseStatusDeleted.None && m.IsFuzzy == isFuzzy
                    && m.IsEnabled
                    select m.Id).ToList();
        }
        public dtoSelectMethod MethodGetAvailable(long idMethod, Int32 idLanguage, Int32 idDefaultLanguage)
        {
            dtoSelectMethod method = null;
            try
            {
                TemplateMethod tMethod = (from m in Manager.GetIQ<TemplateMethod>() where m.Id == idMethod select m).Skip(0).Take(1).ToList().FirstOrDefault();
                if (tMethod != null)
                {
                    List<dtoSelectRatingSet> sets = (from s in Manager.GetIQ<TemplateRatingSet>() where s.Deleted == BaseStatusDeleted.None select s).ToList().Select(s => dtoSelectRatingSet.Create(s, idLanguage, idDefaultLanguage)).ToList();
                    method = dtoSelectMethod.Create(tMethod, sets, idLanguage, idDefaultLanguage);
                }
            }
            catch (Exception ex)
            {
                method = null;
            }
            return method;
        }
        public List<dtoSelectMethod> MethodsGetAvailable(Int32 idLanguage)
        {
            return MethodsGetAvailable(idLanguage,Manager.GetDefaultIdLanguage());
        }
        public List<TemplateMethod> MethodsGetAll()
        {
            List<TemplateMethod> methods = null;
            try
            {
                methods = (from m in Manager.GetIQ<TemplateMethod>() where m.Deleted == BaseStatusDeleted.None && m.IsEnabled select m).ToList();
            }
            catch (Exception ex)
            {
                methods = null;
            }
            return methods;
        }
        public List<dtoSelectMethod> MethodsGetAvailable(Int32 idLanguage, Int32 idDefaultLanguage)
        {
            List<dtoSelectMethod> methods = null;
            try
            {
                List<TemplateMethod> items = (from m in Manager.GetIQ<TemplateMethod>() where m.Deleted == BaseStatusDeleted.None && m.IsEnabled select m).ToList();
                List<dtoSelectRatingSet> sets = (from s in Manager.GetIQ<TemplateRatingSet>() where s.Deleted == BaseStatusDeleted.None select s).ToList().Select(s => dtoSelectRatingSet.Create(s, idLanguage, idDefaultLanguage)).ToList();
                methods = items.Select(m => dtoSelectMethod.Create(m, sets, idLanguage, idDefaultLanguage)).ToList();
            }
            catch (Exception ex)
            {
                methods = null;
            }
            return methods;
        }
        public List<dtoSelectRatingSet> RatingSetGetAvailable(long idMethod,Int32 idLanguage, Int32 idDefaultLanguage)
        {
            List<dtoSelectRatingSet> items = null;
            try
            {
                TemplateMethod method = Manager.Get<TemplateMethod>(idMethod);
                if (method != null)
                {
                    List<dtoSelectRatingSet> sets = (from s in Manager.GetIQ<TemplateRatingSet>() where s.Deleted == BaseStatusDeleted.None select s).ToList().Select(s => dtoSelectRatingSet.Create(s, idLanguage, idDefaultLanguage)).ToList();

                    if (sets != null && sets.Any())
                    {
                        if (sets.Any(s => s.OnlyForAlgorithm == method.Type && s.IsFuzzy == method.IsFuzzy))
                            items = sets.Where(s => s.OnlyForAlgorithm == method.Type && s.IsFuzzy == method.IsFuzzy).Select(s => s.Copy(method.Id)).ToList();
                        else
                            items = sets.Where(s => (s.OnlyForAlgorithm == AlgorithmType.none && s.IsFuzzy == method.IsFuzzy)).Select(s => s.Copy(method.Id)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                items = null;
            }
            return items;
        }
        public long RatingSetGetIdByValue(long idValue)
        {
            long result = 0;
            try
            {
                TemplateRatingValue value = Manager.Get<TemplateRatingValue>(idValue);
                if (value != null && value.RatingSet != null)
                    result = value.RatingSet.Id;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        public TemplateRatingValue RatingValueGet(long id)
        {
            return Manager.Get<TemplateRatingValue>(id);
        }

        public List<dtoGenericRatingValue> RatingValuesGet(long id, Int32 idLanguage, Int32 idDefaultLanguage)
        {
            return (from s in Manager.GetIQ<TemplateRatingValue>()
                    where s.Deleted == BaseStatusDeleted.None
                    where s.RatingSet != null && s.RatingSet.Id == id
                    select s).ToList().Select(s => dtoGenericRatingValue.Create(s, idLanguage, idDefaultLanguage)).ToList();
        }
    }
}
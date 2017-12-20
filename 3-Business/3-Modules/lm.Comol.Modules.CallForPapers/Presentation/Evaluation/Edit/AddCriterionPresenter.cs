using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dss.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation.Evaluation
{
    public class AddCriterionPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAddCriterion View
            {
                get { return (IViewAddCriterion)base.View; }
            }
            private ServiceEvaluation _Service;
            private ServiceEvaluation CallService
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceEvaluation(AppContext);
                    return _Service;
                }
            }
            private ServiceDss _ServiceDss;
            private ServiceDss ServiceDss
            {
                get
                {
                    if (_ServiceDss == null)
                        _ServiceDss = new ServiceDss(AppContext);
                    return _ServiceDss;
                }
            }
            private ServiceCallOfPapers _ServiceCallAdv;
            private ServiceCallOfPapers CallServiceAdv
            {
                get
                {
                    if (_ServiceCallAdv == null)
                    _ServiceCallAdv = new ServiceCallOfPapers(AppContext);
                    return _ServiceCallAdv;
                }
            }

        public AddCriterionPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddCriterionPresenter(iApplicationContext oContext, IViewAddCriterion view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(
            long idCall, 
            Boolean useDss, 
            lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings = null, 
            lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod method =null)
        {
            
            View.UseDss = useDss;
            View.IdCall = idCall;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<CriterionType> types = (from e in Enum.GetValues(typeof(CriterionType)).Cast<CriterionType>().ToList()
                                             where 
                                             e != CriterionType.None 
                                             && e != CriterionType.Textual 
                                             && ((!useDss && e!= CriterionType.RatingScale && e!= CriterionType.RatingScaleFuzzy) 
                                             || (useDss && e != CriterionType.StringRange && settings != null
                                              && (settings.IsFuzzyMethod || (!settings.IsFuzzyMethod && e != CriterionType.RatingScaleFuzzy))))
                                             select e).ToList();
                if (useDss){
                    if (types.Contains( CriterionType.RatingScaleFuzzy)){
                        View.InitializeRatingScale(method);
                    }
                    if (types.Contains( CriterionType.RatingScale)){
                        if (method.IsFuzzy)
                            View.InitializeRatingScale(method.Type,false);
                        else
                            View.InitializeRatingScale(method);
                    }
                }
                    
                View.LoadCriteria(CreateCriteria(types, settings, method));
                View.LoadAvailableTypes((from e in types
                                         select new DisplayCriterionType() { Id = (Int32)e, Name = "", Type = e }).ToList());
                
            }
        }

        private List<dtoCriterionEvaluated> CreateCriteria(List<CriterionType> types, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings = null, lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod method = null)
        {
            List<dtoCriterionEvaluated> criteria = new List<dtoCriterionEvaluated>();
            criteria = (from t in types
                        select new dtoCriterionEvaluated()
                      {
                          Id = 0,
                          StringValue = "",
                          DecimalValue = 1,
                         // IntValue= 1,
                          IdOption = 0,
                          DssValue = (settings != null && method !=null  ? new lm.Comol.Core.Dss.Domain.Templates.dtoItemRating()
                          { 
                                             RatingType= Core.Dss.Domain.RatingType.simple,
                                             IsFuzzy = (t== CriterionType.RatingScaleFuzzy),
                                             IdRatingValue= ( method.RatingSets.Any() && method.RatingSets.FirstOrDefault().Values.Any() ? method.RatingSets.FirstOrDefault().Values.FirstOrDefault().Id : -1),
                                        }  : null ),
                          Criterion = new dtoCriterion() { 
                              Type = t,
                              Options = CreateOptions(t, settings, method),
                              MinOption = 1,
                              CommentType = Domain.Evaluation.CommentType.None,
                              DecimalMinValue = 0,
                              DecimalMaxValue= 10,
                              MaxOption = 1,
                              MaxLength = (t == CriterionType.Textual) ? 10000 : 0,
                              CommentMaxLength=0,
                              UseDss = (settings!=null),
                              MethodSettings = settings
                          }
                      }).ToList();
            return criteria;
        }
        private List<dtoCriterionOption> CreateOptions(CriterionType type, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings = null, lm.Comol.Core.Dss.Domain.Templates.dtoSelectMethod method = null)
        {
            List<dtoCriterionOption> options = new List<dtoCriterionOption>();
            switch (type)
            {
                case CriterionType.StringRange:
                    for (Int32 i = 1; i <= 5; i++)
                    {
                        dtoCriterionOption opt = new dtoCriterionOption();
                        opt.DisplayOrder = i;
                        opt.Id = i;
                        opt.Value = (Decimal)(i * (0.5));
                        opt.Name = i.ToString();
                        opt.UseDss = (settings != null);
                        options.Add(opt);
                    }
                    break;
                case CriterionType.RatingScale:
                    if (method != null && method.RatingSets.Any())
                    {
                        long displayOrder = 1;
                        options = method.RatingSets.FirstOrDefault().Values.Select(v => new dtoCriterionOption()
                                    {
                                        DisplayOrder = displayOrder++,
                                        DoubleValue = v.Value,
                                        FuzzyValue =v.FuzzyValue,
                                        IsFuzzy = v.IsFuzzy,
                                        IdRatingSet = v.IdRatingSet,
                                        IdRatingValue = v.Id,
                                        Name = v.Name,
                                        UseDss = true,
                                        Id = displayOrder 
                                    }).ToList();
                    }
                    break;
                case CriterionType.RatingScaleFuzzy:
                    if (method != null && method.RatingSets.Any())
                    {
                        long displayOrder = 1;
                        options = method.RatingSets.FirstOrDefault().Values.Select(v => new dtoCriterionOption()
                                    {
                                        DisplayOrder = displayOrder++,
                                        DoubleValue = v.Value,
                                        FuzzyValue =v.FuzzyValue,
                                        IsFuzzy = v.IsFuzzy,
                                        IdRatingSet = v.IdRatingSet,
                                        IdRatingValue = v.Id,
                                        Name = v.Name,
                                        UseDss = true,
                                        Id = displayOrder 
                                    }).ToList();
                    }
                    break;
            }

            return options;
        }

        public List<BaseCriterion> AddCriteria(List<dtoCommittee> commitees, lm.Comol.Core.Dss.Domain.Templates.dtoItemMethodSettings settings, long idCommittee, List<dtoCriterion> items)
        {
            return CallService.AddCriteria(View.IdCall, commitees,settings, idCommittee, items);
        }

        public List<BaseCriterion> AddCriteriaAdv(Int64 CommId, List<dtoCriterion> items)
        {
            return CallServiceAdv.AddCriteriaAdv(CommId, items);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class AddFieldPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewAddField View
            {
                get { return (IViewAddField)base.View; }
            }
            private ServiceCallOfPapers _ServiceCall;
            private ServiceCallOfPapers CallService
            {
                get
                {
                    if (_ServiceCall == null)
                        _ServiceCall = new ServiceCallOfPapers(AppContext);
                    return _ServiceCall;
                }
            }
            private ServiceRequestForMembership _ServiceRequest;
            private ServiceRequestForMembership RequestService
            {
                get
                {
                    if (_ServiceRequest == null)
                        _ServiceRequest = new ServiceRequestForMembership(AppContext);
                    return _ServiceRequest;
                }
            }
            public AddFieldPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public AddFieldPresenter(iApplicationContext oContext, IViewAddField view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idCall)
        {
            View.IdCall = idCall;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                List<FieldType> types = (from e in Enum.GetValues(typeof(FieldType)).Cast<FieldType>().ToList() where e != FieldType.None && e !=  FieldType.Disclaimer  select e).ToList();
                List<DisplayFieldType> items = (from e in types
                                                select new DisplayFieldType() { Name = "", Type = e }).ToList();
                List<DisclaimerType> dTypes = (from e in Enum.GetValues(typeof(DisclaimerType)).Cast<DisclaimerType>().ToList() where e != DisclaimerType.None select e).ToList();
                items.AddRange((from d in dTypes
                                                select new DisplayFieldType() { Name = "", Type = FieldType.Disclaimer, DisclaimerType = d }).ToList());

                View.LoadAvailableTypes(items);
                View.LoadFields(CreateFields(items));
            }
        }

        private List<dtoSubmissionValueField> CreateFields(List<DisplayFieldType> types)
        {
            List<dtoSubmissionValueField> fields = new List<dtoSubmissionValueField>();
            fields = (from t in types
                      select new dtoSubmissionValueField()
                      {
                          Id = 0,
                          Value = new dtoValueField(""),
                          IdOption = 0,
                          Field = new dtoCallField() { 
                              Type = t.Type,
                              Options = CreateOptions(t.Type),
                              MinOption = (t.Type == FieldType.CheckboxList || t.DisclaimerType== DisclaimerType.CustomMultiOptions) ? 0 : 1,
                              MaxOption = (t.Type == FieldType.CheckboxList) ? 5 : ((t.Type == FieldType.Disclaimer && (t.DisclaimerType== DisclaimerType.CustomMultiOptions || t.DisclaimerType== DisclaimerType.CustomSingleOption)) ? 2 : 1),
                              DisclaimerType = t.DisclaimerType
                          },
                      }).ToList();
            return fields;
        }
        private List<dtoFieldOption> CreateOptions(FieldType type)
        {
            Int32 maxValue = (type == FieldType.Disclaimer) ? 2 : 5;
            List<dtoFieldOption> options = new List<dtoFieldOption>();
            if (type == FieldType.CheckboxList || type == FieldType.DropDownList || type == FieldType.RadioButtonList)
            {
                for (Int32 i = 1; i <= maxValue; i++)
                {
                    dtoFieldOption opt = new dtoFieldOption();
                    opt.DisplayOrder=i;
                    opt.Id=i;
                    opt.Value=i.ToString();
                    opt.Name=i.ToString();
                    options.Add(opt);
                }
            }
            return options;
        }

        public List<FieldDefinition> CreateFields(List<dtoCallSection<dtoCallField>> sections, long idSection,List<dtoCallField> items) {
            List<FieldDefinition> fields = CallService.AddFields(View.IdCall,sections, idSection, items);

            return fields;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class GenericFieldsMatcherPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
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
            protected virtual IViewGenericFieldsMatcher View
            {
                get { return (IViewGenericFieldsMatcher)base.View; }
            }

            public GenericFieldsMatcherPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public GenericFieldsMatcherPresenter(iApplicationContext oContext, IViewGenericFieldsMatcher view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

            public void InitView(List<ExternalColumnComparer<String, Int32>> columns, List<DestinationItem<Int32>> fields) //, List<DestinationItem<Int32>> rFields)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    View.DestinationFields = fields;
                    foreach (ExternalColumnComparer<String, Int32> column in columns.Where(c => fields.Select(f => f.ColumnName).Contains(c.SourceColumn))) {
                        column.DestinationColumn = fields.Where(f => f.ColumnName == column.SourceColumn).FirstOrDefault();
                    }
                    //View.RequiredFields = rFields;
                    View.LoadItems(columns);
                }
            }

            public Boolean isValid(List<ExternalColumnComparer<String, Int32>> columns)
            {
                List<dtoInvalidMatch<DestinationItem<Int32>>> items = GetInvalidItems(columns);

                return (items == null || items.Count == 0 || !items.Where(d => d.InvalidMatch != InvalidMatch.None).Any());
            }

            public List<dtoInvalidMatch<DestinationItem<Int32>>> GetInvalidItems(List<ExternalColumnComparer<String, Int32>> columns)
            {
                List<dtoInvalidMatch<DestinationItem<Int32>>> items = new List<dtoInvalidMatch<DestinationItem<Int32>>>();

                // Retrieve required item
                List<DestinationItem<Int32>> requiredFields = View.DestinationFields.Where(f => f.Mandatory).ToList();

                Boolean valid = !(columns.Count == columns.Where(c => !c.isValid).Count());
                if (valid)
                {
                    List<DestinationItem<Int32>> usedFields  = columns.Where(c => c.isValid).Select(c => c.DestinationColumn).ToList();

                    valid = (requiredFields.Count > 0 && usedFields.Count > 0);
                    if (valid)
                    {

                        var query = usedFields.GroupBy(a => a).Select(a => new { Type = a, Count = a.Count() });

                        if (query.Where(c => c.Count > 1).Any())
                            items.AddRange(query.Where(c => c.Count > 1).ToList().Select(c => new dtoInvalidMatch<DestinationItem<Int32>>() { Attribute = c.Type.FirstOrDefault(), InvalidMatch = InvalidMatch.DuplicatedItem }).ToList());




                        List<Int32> alternatives = requiredFields.Where(a => a.HasAlternative).Select(a => a.Id).ToList();

                        //items.AddRange(baseAttributes.Where(a => !alternatives.Contains(a) && !usedAttributes.Contains(a)).Select(c => new dtoInvalidMatch<ProfileAttributeType>() { Attribute = c, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());
                        items.AddRange(requiredFields.Where(r => !alternatives.Contains(r.Id) && !usedFields.Select(u => u.Id).Contains(r.Id)).Select(c => new dtoInvalidMatch<DestinationItem<Int32>>() { Attribute = c, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());
                        if (requiredFields.Where(a => a.HasAlternative).Any())
                        {
                            // find all attributes without alternatives
                            List<Int32> alternativeAttributes = new List<Int32>();

                            foreach (Int32 alternative in alternatives)
                            {
                                // Get alternative attributes
                                alternativeAttributes = requiredFields.Where(ma => ma.Id == alternative).Select(ma => ma.AlternativeAttributes).FirstOrDefault();
                                if ((alternativeAttributes == null || alternativeAttributes.Count == 0) && !usedFields.Select(u => u.Id).Contains(alternative))
                                    items.Add(new dtoInvalidMatch<DestinationItem<Int32>>() { Attribute = requiredFields.Where(rf => rf.Id == alternative).FirstOrDefault(), InvalidMatch = InvalidMatch.IgnoredRequiredItem });
                                else if (alternativeAttributes != null && !usedFields.Select(u => u.Id).Contains(alternative) && usedFields.Where(ua => alternativeAttributes.Contains(ua.Id)).Any() == false)
                                    items.Add(new dtoInvalidMatch<DestinationItem<Int32>>() { Attribute = requiredFields.Where(rf => rf.Id == alternative).FirstOrDefault(), InvalidMatch = InvalidMatch.IgnoredAlternativeRequiredItem });
                            }

                            //alternatives.ForEach(a=> 


                            //List<ProfileAttributeType> ignoredAttributes = baseAttributes.Except(mandatoryAttributes.Where(a => !a.HasAlternative).Select(a => a.Attribute).ToList()).Except(usedAttributes.Distinct()).ToList();

                            //mandatoryAttributes.Where(a => ignoredAttributes.Contains(a.Attribute)).ToList().ForEach(a => alternativeAttributes.AddRange(a.AlternativeAttributes));


                        }

                       
                    }
                    else
                        items.AddRange(requiredFields.Select(a => new dtoInvalidMatch<DestinationItem<Int32>>() { Attribute = a, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());

                }
                else
                    items.AddRange(requiredFields.Select(a => new dtoInvalidMatch<DestinationItem<Int32>>() { Attribute = a, InvalidMatch = InvalidMatch.IgnoredRequiredItem }).ToList());
                return items;
            }

            public Boolean ValidateColumns(List<ExternalColumnComparer<String, Int32>> columns, ref  Dictionary<InvalidMatch, List<DestinationItem<Int32>>> invalidColumns)
            {
                Boolean valid = !(columns.Count == columns.Where(c => !c.isValid).Count());
                if (valid)
                {
                    List<DestinationItem<Int32>> requiredFields = View.DestinationFields.Where(f => f.Mandatory).ToList();

                    List<DestinationItem<Int32>> usedFields = columns.Where(c => c.isValid).Select(c => c.DestinationColumn).ToList();
                    valid = (requiredFields.Count > 0 && usedFields.Count > 0);
                    if (valid)
                    {
                        var query = usedFields.GroupBy(a => a).Select(a => new { Type = a, Count = a.Count() });
                        if (query.Where(c => c.Count == 0).Any())
                        {
                            invalidColumns.Add((query.Where(c => c.Count == 0).Count() == 1) ? InvalidMatch.IgnoredRequiredItem : InvalidMatch.IgnoredRequiredItems
                            , query.Where(c => c.Count == 0).Select(c => c.Type.Key).ToList());
                            valid = false;
                        }
                        if (query.Where(c => c.Count > 1).Any())
                        {
                            invalidColumns.Add((query.Where(c => c.Count > 1).Count() == 1) ? InvalidMatch.DuplicatedItem : InvalidMatch.DuplicatedItems
                            , query.Where(c => c.Count > 1).Select(c => c.Type.Key).ToList());
                            valid = false;
                        }
                        if (requiredFields.Where(a => !usedFields.Contains(a)).Any())
                        {
                            List<Int32> alternatives = requiredFields.Where(a => a.HasAlternative).Select(a => a.Id).ToList();
                            // Trovo gli elementi obbligatori
                            List<DestinationItem<Int32>> errorItems = requiredFields.Where(a =>!alternatives.Contains(a.Id) && !usedFields.Contains(a)).ToList();

                            
                             if (requiredFields.Where(a => a.HasAlternative).Any()) {
                                 List<Int32> alternativeAttributes = new List<Int32>();
                                 foreach (Int32 alternative in alternatives)
                                 {
                                     // Get alternative attributes
                                     alternativeAttributes = requiredFields.Where(ma => ma.Id == alternative).Select(ma => ma.AlternativeAttributes).FirstOrDefault();
                                     if ((alternativeAttributes == null || alternativeAttributes.Count == 0) && !usedFields.Select(u => u.Id).Contains(alternative))
                                         errorItems.Add(requiredFields.Where(rf => rf.Id == alternative).FirstOrDefault());
                                     else if (alternativeAttributes != null && !usedFields.Select(u => u.Id).Contains(alternative) && usedFields.Where(ua => alternativeAttributes.Contains(ua.Id)).Any() == false)
                                         errorItems.Add(requiredFields.Where(rf => rf.Id == alternative).FirstOrDefault());
                                 }

                             }


                            if (errorItems.Count>0)
                                invalidColumns.Add((errorItems.Count() == 1) ? InvalidMatch.IgnoredRequiredItem : InvalidMatch.IgnoredRequiredItems, errorItems);
                                                       
                            //invalidColumns.Add((requiredFields.Where(a => !usedFields.Contains(a)).Count() == 1) ? InvalidMatch.IgnoredRequiredItem : InvalidMatch.IgnoredRequiredItems
                            //   , requiredFields.Where(a => !usedFields.Contains(a)).ToList());
                            valid = (invalidColumns.Count>0);
                        }
                    }
                    else
                        invalidColumns.Add(InvalidMatch.IgnoredAllItems, requiredFields);
                }
                return valid;
            }
    }
}
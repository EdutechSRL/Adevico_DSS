using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers.Export;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export;
using LM.MathLibrary.Extensions;
using lm.Comol.Core.Dss.Domain;
using lm.Comol.Core.Dss.Business;
namespace lm.Comol.Modules.CallForPapers.Business
{
    public class HelperExportToCsv : lm.Comol.Core.DomainModel.Helpers.Export.ExportCsvBaseHelper
    {
        private Dictionary<SubmissionsListTranslations, string> CommonTranslations { get; set; }
        private Dictionary<SubmissionStatus, String> StatusTranslations { get; set; }
        private Dictionary<RevisionStatus, String> RevisionStatusTranslations { get; set; }
        private Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, String> EvaluationsTranslations { get; set; }
        private Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> EvaluationStatusTranslations { get; set; }
        private ExportContentType ContentType { get; set; }

        public HelperExportToCsv() : base() { 
            CommonTranslations = new Dictionary<SubmissionsListTranslations, string>();
            StatusTranslations = new Dictionary<SubmissionStatus, string>();
            RevisionStatusTranslations = new Dictionary<RevisionStatus, string>();
        }
        public HelperExportToCsv(String endRow, char delimiter, char endField) : base(endRow, delimiter, endField) {
            CommonTranslations = new Dictionary<SubmissionsListTranslations, string>();
            StatusTranslations = new Dictionary<SubmissionStatus, string>();
            RevisionStatusTranslations = new Dictionary<RevisionStatus, string>();
        }
        public HelperExportToCsv(Dictionary<SubmissionsListTranslations, string> translations,Dictionary<SubmissionStatus, String> status,Dictionary<RevisionStatus, string> revisions)
            : this()
        {
             CommonTranslations = translations;
             StatusTranslations = status;
             RevisionStatusTranslations = revisions;
             ContentType = ExportContentType.SubmissionList;
        }
        public HelperExportToCsv(Dictionary<SubmissionsListTranslations, string> translations,Dictionary<SubmissionStatus, String> status,Dictionary<RevisionStatus, string> revisions, ExportContentType content)
            : this(translations,status,revisions)
        {
             ContentType = content;
        }
        public HelperExportToCsv(Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, string> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
            : this()
        {
            EvaluationsTranslations = translations;
            EvaluationStatusTranslations = status;
            ContentType = ExportContentType.Evaluations;
        }


        #region "Submissions List"
            public String ExportSubmissionsList(CallForPaperType type, String name, String edition, List<dtoSubmissionDisplay> submissions, litePerson person, SubmissionFilterStatus filter)
            {
                Boolean hasRevisions = submissions.Where(s => s.Revisions.Count > 0).Any();
                Boolean showMail = submissions.Where(s => !s.IsAnonymous).Any();

                Int32 columNumber = (hasRevisions ? 7 : 4);
                if (showMail)
                    columNumber += 1;
                String export = "";
                String header = AddSubmissionsListInfo(type, name, edition, person, columNumber);
                header += SubmissionsListTableHeader(filter, hasRevisions, showMail);
                int rowNumber = 1;
                foreach (dtoSubmissionDisplay sub in submissions)
                {
                    header += AddSubmissionListRow(sub, filter, hasRevisions, showMail, rowNumber);
                    rowNumber++;
                }
                return header;
            }
            public static String GetErrorDocument(String translation)
            {
                return translation;
            }
            private String AddSubmissionsListInfo(CallForPaperType type, String name, String edition, litePerson person, Int32 columNumber)
            {
                String infoRow = ""; 
                if (String.IsNullOrEmpty(edition))
                    infoRow += AppendItem(string.Format(CommonTranslations[(type == CallForPaperType.CallForBids) ? SubmissionsListTranslations.CallName : SubmissionsListTranslations.RequestName], name)) + EndRowItem;
                else
                    infoRow += AppendItem(string.Format(CommonTranslations[(type == CallForPaperType.CallForBids) ? SubmissionsListTranslations.CallNameNameAndEdition : SubmissionsListTranslations.RequestNameNameAndEdition], name, edition)) + EndRowItem;

                DateTime printTime = DateTime.Now;
                if (person != null)
                    infoRow += AppendItem(string.Format(CommonTranslations[SubmissionsListTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString())) + EndRowItem;

                infoRow += AppendRow(3);
                return infoRow;
            }
            private String SubmissionsListTableHeader(SubmissionFilterStatus filter, Boolean hasRevisions, Boolean showMail)
            {
                SubmissionsListTranslations dateCell = SubmissionsListTranslations.CellLastUpdateOn;
                switch (filter)
                {
                    case SubmissionFilterStatus.OnlySubmitted:
                        dateCell = SubmissionsListTranslations.CellSubmittedOn;
                        break;
                    case SubmissionFilterStatus.VirtualDeletedSubmission:
                        dateCell = SubmissionsListTranslations.CellDeletedOn;
                        break;
                }
                String tableData = AppendItem(CommonTranslations[SubmissionsListTranslations.CellSubmitter]);
                if (showMail)
                    tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellSubmitterMail]);
                tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellSubmissionType])
                                + AppendItem(CommonTranslations[dateCell]);

                if (hasRevisions)
                {
                    //tableData += BuilderXmlDocument.AddData(translations[SubmissionsListTranslations.CellIsRevision]());
                    tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellHasRevision]);
                    tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellRevisionRequest]);
                    tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellRevisionApproved]);
                }

                tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellStatus]);

                return tableData + EndRowItem;
            }
            private String AddSubmissionListRow(dtoSubmissionDisplay sub, SubmissionFilterStatus filter, Boolean hasRevisions, Boolean showMail, int rowNumber)
            {
                String row = "";

                String owner = "";
                if (sub.Owner == null && sub.IsAnonymous == false)
                    owner = CommonTranslations[SubmissionsListTranslations.DeletedUser];
                else if (sub.IsAnonymous || (sub.Owner != null && sub.Owner.TypeID == (int)UserTypeStandard.Guest))
                    owner = CommonTranslations[SubmissionsListTranslations.AnonymousOwner];
                else
                    owner = sub.Owner.SurnameAndName;

                if (!sub.SubmittedOn.HasValue || sub.SubmittedBy == null)
                    row += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.CreatedByInfo], owner));
                else if (sub.Owner == sub.SubmittedBy)
                    row += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedByInfo], owner));
                else
                    row += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedForInfo], owner, sub.SubmittedBy.SurnameAndName));

                if (showMail)
                {
                    if (sub.Owner == null || sub.IsAnonymous || (sub.Owner != null && sub.Owner.TypeID == (int)UserTypeStandard.Guest))
                        row += AppendItem("");
                    else
                        row += AppendItem(sub.Owner.Mail);
                }

                row += AppendItem(sub.Type.Name);
                if (filter == SubmissionFilterStatus.OnlySubmitted && sub.SubmittedOn.HasValue)
                    row += AppendItem(sub.SubmittedOn.Value.ToShortDateString() + ' ' + sub.SubmittedOn.Value.ToShortTimeString());
                else
                    row += AppendItem(sub.ModifiedOn.Value.ToShortDateString() + ' ' + sub.ModifiedOn.Value.ToShortTimeString());

                if (hasRevisions)
                {
                    if (sub.Revisions.Count > 1)
                    {
                        row += AppendItem(CommonTranslations[SubmissionsListTranslations.TrueValue]);
                        row += AppendItem(sub.Revisions.Where(r => r.RevisionType != RevisionType.Original).Count().ToString());
                        row += AppendItem(sub.Revisions.Where(r => r.RevisionType != RevisionType.Original && r.isActive).Count().ToString());
                    }
                    else
                    {
                        row += AppendItem(CommonTranslations[SubmissionsListTranslations.FalseValue]);
                        row += AppendItem("");
                        row += AppendItem("");
                    }
                }
                row += AppendItem(StatusTranslations[sub.Status]);

                return row + EndRowItem;
            }

        #endregion

        #region "evaluations"
            public String ExportEvaluatorStatistics(dtoCall call, List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluatorCommitteeStatistic> statistics, litePerson person)
            {
                Int32 columNumber = statistics.Select(s => GetColumnNumber(s.Criteria)).Max();
                String export = "";
                String header = AddStandardEvaluationsInfo(call, person, columNumber);
                export += header;
                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluatorCommitteeStatistic committee in statistics)
                {
                    StringBuilder sBuilder = new StringBuilder();
                    AddEmptyRow(sBuilder);

                    AppendToRow(sBuilder, 
                        string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CommitteeName], 
                        committee.Name),
                        false);

                    AddEmptyRow(sBuilder, 3);

                    AddCommitteeTableHeader(sBuilder, call.EvaluationType,committee);

                    int rowNumber = 1;

                    List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption> options = 
                        new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption>();

                    committee.Criteria
                        .Where(c => 
                            (c.Type == Domain.Evaluation.CriterionType.StringRange 
                                || c.Type== CriterionType.RatingScale 
                                || c.Type== CriterionType.RatingScaleFuzzy) 
                            && c.Options.Any())
                            .ToList()
                            .ForEach(c => options.AddRange(c.Options));

                    Boolean validEvaluations = !committee.Evaluations.Any(e => e.Status == EvaluationStatus.None || e.Criteria.Any(c => c.IsValueEmpty || !c.IsValidForEvaluation || !c.IsValidForCriterionSaving ));


                committee.Evaluations.ForEach(
                    e => 
                        AddEvaluationStatisticRow(
                            sBuilder, 
                            call.EvaluationType,
                            committee.IsFuzzy,
                            validEvaluations, 
                            committee.Criteria, 
                            options, 
                            e));

                    export += sBuilder.ToString();
                }
                return export;
            }
            private String AddStandardEvaluationsInfo(dtoCall call, litePerson person, Int32 columNumber)
            {
                String infoRow = "";
                if (String.IsNullOrEmpty(call.Edition))
                    infoRow += AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitle], call.Name), false);
                else
                    infoRow += AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitleAndEdition], call.Name, call.Edition), false);
                infoRow += EndRowItem;

                DateTime printTime = DateTime.Now;
                if (person != null)
                    infoRow += AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), false);
                infoRow += EndRowItem + EndRowItem;
                return infoRow;
            }

            private void AddStandardEvaluationsInfo(StringBuilder sBuilder, dtoCall call, litePerson person, Int32 columNumber)
            {
                if (String.IsNullOrEmpty(call.Edition))
                    AppendToRow(sBuilder, string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitle], call.Name),false);
                else
                    AppendToRow(sBuilder, string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitleAndEdition], call.Name, call.Edition), false);

                DateTime printTime = DateTime.Now;
                if (person != null)
                    AppendToRow(sBuilder, string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), false);
                AddEmptyRow(sBuilder, 2);
            }

            private void AddCommitteeTableHeader(StringBuilder sBuilder, EvaluationType type, lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluatorCommitteeStatistic committee)
            {
                AddField(sBuilder,"#");
                AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]);
                AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]);
                AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]);
                switch (type)
                {
                    case EvaluationType.Sum:
                        AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]);
                        break;
                    case EvaluationType.Average:
                        AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage]);
                        break;
                    case EvaluationType.Dss:
                        AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDss]);
                        if (committee.IsFuzzy)
                            AddField(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssFuzzyValue]);
                        break;
                }
                

                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion criterion in committee.Criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList())
                {
                    AddField(sBuilder, String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterion], criterion.Name));
                    if (criterion.IsFuzzy || committee.IsFuzzy)
                        AddField(sBuilder, String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionFuzzyValue], criterion.Name));
                    else
                        AddField(sBuilder, String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionRangeValue], criterion.Name));
                    AddField(sBuilder, String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionComment], criterion.Name));
                }
                AppendToRow(sBuilder, EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment],false);
            }
            private void AddEvaluationStatisticRow(
                StringBuilder sBuilder, 
                EvaluationType type, 
                Boolean isFuzzyCommittee, 
                Boolean displayRating,
                List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria, 
                List<dtoCriterionOption> availableOptions, 
                lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation)
            {
                
                AddField(sBuilder, evaluation.Position);

                AddField(sBuilder, evaluation.DisplayName);

                AddField(sBuilder, evaluation.SubmitterType);

                AddField(sBuilder, EvaluationStatusTranslations[evaluation.Status]);

                switch (type)
                {
                    case EvaluationType.Sum:
                        AddField(sBuilder, Math.Round((decimal)evaluation.SumRating, 2));
                        break;
                    case EvaluationType.Average:
                        AddField(sBuilder, Math.Round((decimal)evaluation.AverageRating, 2));
                        break;
                    case EvaluationType.Dss:
                        if (displayRating) {
                            AddField(sBuilder, Math.Round((decimal)evaluation.DssRating.Value, 10));
                            if (isFuzzyCommittee)
                                AddField(sBuilder, evaluation.DssRating.ValueFuzzy);
                        }
                        else
                        {
                            AddField(sBuilder, "--");
                            if (isFuzzyCommittee)
                                AddField(sBuilder, "--");
                        }
                        break;
                }
                

                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated vCriterion 
                    in evaluation.Criteria
                        .Where(c => c.Deleted == BaseStatusDeleted.None)
                        .ToList()
                        .OrderBy(c => c.Criterion.DisplayOrder)
                        .ThenBy(c => c.Criterion.Name)
                        .ToList())
                {
                    if (vCriterion.IsValueEmpty)
                        AddEmptyField(sBuilder,2);
                    else
                    {
                        switch (vCriterion.Criterion.Type)
                        {
                            case Domain.Evaluation.CriterionType.StringRange:
                                dtoCriterionOption option = availableOptions.Where(o => o.Id == vCriterion.IdOption).FirstOrDefault();
                                if (option == null)
                                    AddEmptyField(sBuilder, 2);
                                else
                                {
                                    if (type == EvaluationType.Dss)
                                    {
                                        AddField(sBuilder, option.Name);
                                        if (isFuzzyCommittee)
                                            AddField(sBuilder, (((double)option.Value).ToFuzzy().ToString()));
                                        else
                                            AddField(sBuilder, option.Value);
                                    }
                                    else
                                    {
                                        AddField(sBuilder, option.Name);
                                        AddField(sBuilder, option.Value);
                                    }
                                }
                                break;
                            case Domain.Evaluation.CriterionType.Textual:
                                if (string.IsNullOrEmpty(vCriterion.StringValue))
                                    AddEmptyField(sBuilder);
                                else
                                    AddField(sBuilder, vCriterion.StringValue);
                                break;
                            case Domain.Evaluation.CriterionType.DecimalRange:
                                if (type == EvaluationType.Dss && isFuzzyCommittee )
                                {
                                    AddField(sBuilder, Math.Round(vCriterion.DecimalValue, 2));
                                    AddField(sBuilder, ((double)(vCriterion.DecimalValue)).ToFuzzy().ToString());
                                }
                                else
                                {
                                    AddField(sBuilder, Math.Round(vCriterion.DecimalValue, 2));
                                    AddField(sBuilder, Math.Round(vCriterion.DecimalValue, 2));
                                }
                                
                                break;
                            case Domain.Evaluation.CriterionType.IntegerRange:
                                if (type == EvaluationType.Dss && isFuzzyCommittee)
                                {
                                    AddField(sBuilder, Math.Round(vCriterion.DecimalValue, 2));
                                    AddField(sBuilder, ((double)(vCriterion.DecimalValue)).ToFuzzy().ToString());
                                }
                                else
                                {
                                    AddField(sBuilder, ((int)vCriterion.DecimalValue));
                                    AddField(sBuilder, ((int)vCriterion.DecimalValue));
                                }
                                break;
                            case Domain.Evaluation.CriterionType.RatingScale:
                            case Domain.Evaluation.CriterionType.RatingScaleFuzzy:
                                dtoCriterionOption dssOption = availableOptions.Where(o => o.IdRatingValue == vCriterion.DssValue.IdRatingValue).FirstOrDefault();
                                if (type == EvaluationType.Dss && isFuzzyCommittee)
                                {
                                    if (vCriterion.DssValue.Error == DssError.None)
                                    {
                                        String displayName = "";
                                        dtoCriterionOption dssOptionEnd = availableOptions.Where(o => o.IdRatingValue == vCriterion.DssValue.IdRatingValueEnd).FirstOrDefault();
                                        switch(vCriterion.DssValue.RatingType){
                                            case RatingType.simple:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                break;
                                            case RatingType.extended:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                if (dssOptionEnd != null)
                                                    displayName = displayName + "->" + dssOptionEnd.Name;
                                                break;
                                            case RatingType.intermediateValues:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                if (dssOptionEnd != null)
                                                    displayName = displayName + "/" + dssOptionEnd.Name;
                                                break;
                                        }
                                        AddField(sBuilder, displayName);
                                        if (isFuzzyCommittee)
                                        {
                                            if (vCriterion.DssValue.IsFuzzy && !String.IsNullOrWhiteSpace(vCriterion.DssValue.ValueFuzzy))
                                                AddField(sBuilder, vCriterion.DssValue.ValueFuzzy);
                                            else
                                                AddField(sBuilder, ((double)(vCriterion.DssValue.Value)).ToFuzzy().ToString());
                                        }
                                        else
                                        {
                                            AddField(sBuilder, ((double)(vCriterion.DssValue.Value)));
                                        }
                                    }
                                    else
                                         AddEmptyField(sBuilder, 2);
                                }
                                else
                                {
                                    AddField(sBuilder, dssOption.DoubleValue);
                                    if (isFuzzyCommittee)
                                        AddField(sBuilder, dssOption.FuzzyValue);
                                    else
                                        AddField(sBuilder, dssOption.DoubleValue);
                                }
                                break;
                        case Domain.Evaluation.CriterionType.Boolean:
                            string Value = (vCriterion.DecimalValue > 0) ? "Superato" : "Non Superato";
                            AddField(sBuilder, Value);
                            Value = (vCriterion.DecimalValue > 0) ? "1" : "0";
                            AddField(sBuilder, Value);
                            break;
                        case Domain.Evaluation.CriterionType.None:
                                AddEmptyField(sBuilder, 2);
                                break;
                        }
                    }
                    if (vCriterion.Criterion.CommentType != CommentType.None && !String.IsNullOrEmpty(vCriterion.Comment))
                        AddField(sBuilder, vCriterion.Comment);
                    else 
                        AddEmptyField(sBuilder);
                }
                AppendToRow(sBuilder,evaluation.Comment, false);
            }
            private Int32 GetColumnNumber(List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria)
            {
                Int32 columNumber = 5;
                columNumber += (criteria.Count *3); //criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType == Domain.Evaluation.CommentType.None).Count();
                //columNumber += (criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType != Domain.Evaluation.CommentType.None).Count() * 2);
                //columNumber += criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == Domain.Evaluation.CriterionType.StringRange).Count();
                return columNumber;
            }
        #endregion

        #region "Summary"
            public String ExportSummaryDisplayStatistics(dtoCall call, List<dtoEvaluationSummaryItem> items, litePerson person)
            {
                Dictionary<EvaluationStatus, Boolean> counters = GetCounters(items);
                Int32 columNumber = 4 + ((counters.Where(c => c.Value).Count() > 1) ? counters.Where(c => c.Value).Count() : 0);

                String export = "";
                String header = AddStandardEvaluationsInfo(call, person, columNumber);

                String content = AppendItem(EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle]) + EndRowItem;
                content += EndRowItem + EndRowItem + EndRowItem;
                content += EvaluationsTableHeader(call.EvaluationType, call.IsDssMethodFuzzy, counters);
                int rowNumber = 1;

                foreach (dtoEvaluationSummaryItem evaluation in items)
                {
                    content += EvaluationStatisticRow(call.EvaluationType, call.IsDssMethodFuzzy,  evaluation, counters, rowNumber);
                    rowNumber++;
                }
                export += header + content;

                return export;
            }
            #region "Committee"
        public String ExportSummaryDisplayStatistics(dtoCall call, expCommittee committee, List<expEvaluation> evaluations, String anonymousDisplayName, litePerson person, Boolean oneColumnForCriteria=true)
        {

            string UnknownUsers = "Unknown";
            try
            {
                UnknownUsers = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser];
            }  catch (Exception ex)
            {
                
            }
            

            String result = "";
            Int32 columNumber = (committee.Criteria !=null) ? 5 : (committee.Criteria.Count * 3) + 5;
            StringBuilder content = new StringBuilder();
            AddStandardEvaluationsInfo(content,call, person, columNumber);
            AppendToRow(content, String.Format(EvaluationsTranslations[EvaluationTranslations.CommitteeName], committee.Name), false);
            AddEmptyRow(content, 2);
            AppendToRow(content, EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle], false);
            AddEmptyRow(content);
            AddEvaluationsTableHeader(call.EvaluationType, (call.IsDssMethodFuzzy || (committee.UseDss && committee.MethodSettings!= null && committee.MethodSettings.IsFuzzyMethod)), content, committee, oneColumnForCriteria);
            result = content.ToString();
            if (committee.Evaluators.Any()) { 
                if (oneColumnForCriteria)
                {
                    IList<expEvaluator> evaluators = new List<expEvaluator>();
                    try
                    {
                        evaluators = committee.Evaluators.OrderBy(e => e.DisplayName(UnknownUsers))
                            .ToList();
                    }
                    catch (Exception ex)
                    {

                    }

                    
                    foreach(expEvaluator evaluator in evaluators)
                    {
                        try
                        {
                            string currentvalue = "";

                            IList<expEvaluation> curEvaluations = evaluations.Where(ev => ev.Evaluator == evaluator && ev.Submission != null).OrderBy(ev => ev.IdSubmission).ToList();

                            if(curEvaluations != null)
                            {
                                currentvalue = AddEvaluatorStatisticRows(
                                                        new StringBuilder(),
                                                        call.EvaluationType,
                                                        (call.EvaluationType == EvaluationType.Dss
                                                            && (call.IsDssMethodFuzzy || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod))),
                                                        committee,
                                                        evaluator,
                                                        curEvaluations.ToList()
                                                        )
                                                    .ToString();
                            }

                            if (!String.IsNullOrWhiteSpace(currentvalue))
                            {
                                result += currentvalue;
                            }
                        } catch(Exception ex)
                        {

                        }

                    }

                    //committee.Evaluators
                    //.OrderBy(e => e.DisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]))
                    //.ToList()
                    //.ForEach(e =>
                    //                result += AddEvaluatorStatisticRows(
                    //                                new StringBuilder(),
                    //                                call.EvaluationType,
                    //                                (call.EvaluationType == EvaluationType.Dss
                    //                                    && (call.IsDssMethodFuzzy || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod))),
                    //                                committee,
                    //                                e,
                    //                                evaluations.Where(ev => ev.Evaluator == e && ev.Submission != null)
                    //                                .OrderBy(ev => ev.IdSubmission)
                    //                            .ToList())
                    //                .ToString());

                }                
                else
                {
                    committee.Evaluators.OrderBy(e => e.DisplayName(UnknownUsers)).ToList().ForEach(e =>
                            result += AddEvaluatorStatisticCriteriaRows(new StringBuilder(), call.EvaluationType, (call.EvaluationType == EvaluationType.Dss && (call.IsDssMethodFuzzy || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod))), committee, e, evaluations.Where(ev => ev.Evaluator == e && ev.Submission != null).OrderBy(ev => ev.IdSubmission).ToList()).ToString());
                }
                    
            }

            return result;
        }
                private void AddEvaluationsTableHeader(EvaluationType type, Boolean isFuzzy, StringBuilder builder, expCommittee committee, Boolean oneColumnForCriteria)
                {
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluator]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]));
                    switch (type) { 
                        case EvaluationType.Average:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage]));
                            break;
                        case EvaluationType.Dss:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDss]));
                            if(isFuzzy)
                                AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssFuzzyValue]));
                            break;
                        default:
                           AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]));
                           break;
                    }
                    
                    if (!oneColumnForCriteria)
                        AddCommitteeCriteriaTableHeader(type, (isFuzzy || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod)), builder);
                    else if (committee.Criteria !=null && committee.Criteria.Any())
                        committee.Criteria.ToList().ForEach(cr => AddCommitteeCriteriaTableHeader(type, (isFuzzy || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod)),builder));
                    AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]));

                }
                private StringBuilder AddEvaluatorStatisticRows(StringBuilder sBuilder,EvaluationType type, Boolean isFuzzyCall,  expCommittee committee, expEvaluator evaluator, List<expEvaluation> evaluations)
                {
                    isFuzzyCall =(isFuzzyCall || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod)); 
                    foreach (expEvaluation evaluation in evaluations)
                    {
                        AddField(sBuilder, (evaluation.Evaluator == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser] : evaluation.Evaluator.DisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]));
                        AddField(sBuilder, evaluation.Submission.OwnerDisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser],EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]));
                        AddField(sBuilder, evaluation.Submission.Type.Name);
                        AddField(sBuilder, EvaluationStatusTranslations[evaluation.Status]);
                        switch (type)
                        {
                            case EvaluationType.Average:
                                AddField(sBuilder, Math.Round((decimal)evaluation.AverageRating, 2));
                                break;
                            case EvaluationType.Dss:
                                AddField(sBuilder, evaluation.DssRanking);
                                if (isFuzzyCall)
                                {
                                    if (String.IsNullOrWhiteSpace(evaluation.DssValueFuzzy))
                                        AddField(sBuilder, evaluation.DssValue.ToFuzzy().ToString());
                                    else
                                        AddField(sBuilder, evaluation.DssValueFuzzy);
                                }
                                break;
                            default:
                                AddField(sBuilder, Math.Round((decimal)evaluation.SumRating, 2));
                                break;
                        }

                        committee.Criteria.OrderBy(c => c.DisplayOrder).ToList().ForEach(c=>
                            AddCriterionValue(sBuilder,type,isFuzzyCall, c, (evaluation.EvaluatedCriteria == null) ? null : evaluation.EvaluatedCriteria.Where(ev => ev.Criterion == c).FirstOrDefault()));
                        AppendToRow(sBuilder, evaluation.Comment);
                    }
                    return sBuilder;
                }
                private StringBuilder AddEvaluatorStatisticCriteriaRows(StringBuilder sBuilder, EvaluationType type, Boolean isFuzzyCall, expCommittee committee, expEvaluator evaluator, List<expEvaluation> evaluations)
                {
                    isFuzzyCall =(isFuzzyCall || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod)); 
                    foreach (expCriterion criterion in committee.Criteria.OrderBy(c => c.DisplayOrder))
                    {
                        foreach (expEvaluation evaluation in evaluations)
                        {
                            AddField(sBuilder, AppendItem((evaluation.Evaluator == null || evaluation.Evaluator.Person == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser] : evaluation.Evaluator.Person.SurnameAndName));
                            AddField(sBuilder, (evaluation.Submission.isAnonymous || evaluation.Submission.Owner == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser] : evaluation.Submission.Owner.SurnameAndName);
                            AddField(sBuilder, evaluation.Submission.Type.Name);
                            AddField(sBuilder, EvaluationStatusTranslations[evaluation.Status]);
                                
                            switch(type) {
                                case EvaluationType.Average:
                                    AddField(sBuilder, Math.Round((decimal)evaluation.AverageRating, 2));
                                    break;
                                case EvaluationType.Dss:
                                    AddField(sBuilder, evaluation.DssRanking);
                                    if (isFuzzyCall) {
                                        if (String.IsNullOrWhiteSpace(evaluation.DssValueFuzzy))
                                            AddField(sBuilder, evaluation.DssValue.ToFuzzy().ToString());
                                        else
                                            AddField(sBuilder, evaluation.DssValueFuzzy);
                                    }
                                    break;
                                default:
                                    AddField(sBuilder, Math.Round((decimal)evaluation.SumRating, 2));
                                    break;
                                }

                            AddCriterionValue(sBuilder,type,isFuzzyCall, criterion, (evaluation.EvaluatedCriteria == null) ? null : evaluation.EvaluatedCriteria.Where(ev => ev.Criterion == criterion).FirstOrDefault());
                            AppendToRow(sBuilder, evaluation.Comment);
                        }
                    }
                    return sBuilder;
                }
                private void AddCriterionValue(StringBuilder sBuilder, EvaluationType type, Boolean isFuzzyCommittee, expCriterion criterion, expCriterionEvaluated eCriterion)
                {
                    AddField(sBuilder, criterion.Name);
                    if (eCriterion == null || eCriterion.IsValueEmpty)
                        AddEmptyField(sBuilder, (isFuzzyCommittee ? 3 : 2));
                    else
                    {
                        switch (eCriterion.Criterion.Type)
                        {
                            case Domain.Evaluation.CriterionType.StringRange:
                                lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCriterionOption option = eCriterion.Criterion.Options.Where(o => eCriterion.Option != null && o.Id == eCriterion.Option.Id).FirstOrDefault();
                                if (option == null)
                                    AddEmptyField(sBuilder, (isFuzzyCommittee ? 3 : 2));
                                else
                                {
                                    try
                                    {
                                        AddField(sBuilder, option.Name);
                                        AddField(sBuilder, option.Value);
                                        if (isFuzzyCommittee)
                                            AddField(sBuilder, ((double)option.Value).ToFuzzy().ToString());
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;
                            case Domain.Evaluation.CriterionType.Textual:
                                if (string.IsNullOrEmpty(eCriterion.StringValue))
                                    AddEmptyField(sBuilder);
                                else
                                    AddField(sBuilder, eCriterion.StringValue);
                                AddEmptyField(sBuilder);
                                if (isFuzzyCommittee)
                                    AddEmptyField(sBuilder);
                                break;
                            case Domain.Evaluation.CriterionType.DecimalRange:
                                AddField(sBuilder, Math.Round(eCriterion.DecimalValue, 2));
                                AddField(sBuilder, Math.Round(eCriterion.DecimalValue, 2));
                                
                                if (isFuzzyCommittee)
                                    AddField(sBuilder, ((double)eCriterion.DecimalValue).ToFuzzy().ToString());
                                break;
                            case Domain.Evaluation.CriterionType.IntegerRange:
                                AddField(sBuilder, ((int)eCriterion.DecimalValue));
                                AddField(sBuilder, ((int)eCriterion.DecimalValue));
                                if (isFuzzyCommittee)
                                    AddField(sBuilder, ((double)eCriterion.DecimalValue).ToFuzzy().ToString());
                                break;
                            case Domain.Evaluation.CriterionType.RatingScale:
                            case Domain.Evaluation.CriterionType.RatingScaleFuzzy:
                                lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCriterionOption dssOption = eCriterion.Criterion.Options.Where(o => o.IdRatingValue == eCriterion.DssValue.IdRatingValue).FirstOrDefault();
                                if (type == EvaluationType.Dss )
                                {
                                    if (eCriterion.DssValue.IsValid())
                                    {
                                        String displayName = "";
                                        lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCriterionOption dssOptionEnd = eCriterion.Criterion.Options.Where(o => o.IdRatingValue == eCriterion.DssValue.IdRatingValueEnd).FirstOrDefault();
                                        switch (eCriterion.DssValue.RatingType)
                                        {
                                            case RatingType.simple:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                break;
                                            case RatingType.extended:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                if (dssOptionEnd != null)
                                                    displayName = displayName + "->" + dssOptionEnd.Name;
                                                break;
                                            case RatingType.intermediateValues:
                                                if (dssOption != null)
                                                    AddField(sBuilder, dssOption.Name);
                                                if (dssOptionEnd != null)
                                                    displayName = displayName + "/" + dssOptionEnd.Name;
                                                break;
                                        }
                                        AddField(sBuilder, displayName);
                                        if (isFuzzyCommittee)
                                        {
                                            AddField(sBuilder, ((double)(eCriterion.DssValue.Value)));
                                            if (eCriterion.DssValue.IsFuzzy && !String.IsNullOrWhiteSpace(eCriterion.DssValue.ValueFuzzy))
                                                AddField(sBuilder, eCriterion.DssValue.ValueFuzzy);
                                            else
                                                AddField(sBuilder, ((double)(eCriterion.DssValue.Value)).ToFuzzy().ToString());
                                        }
                                        else
                                        {
                                            AddField(sBuilder, ((double)(eCriterion.DssValue.Value)));
                                        }

                                    }
                                    else
                                        AddEmptyField(sBuilder, (isFuzzyCommittee ? 3 : 2));
                                }
                                else
                                {
                                    AddField(sBuilder, dssOption.DoubleValue);
                                    AddField(sBuilder, dssOption.DoubleValue);
                                    if (isFuzzyCommittee)
                                        AddField(sBuilder, dssOption.FuzzyValue);
                                }
                                break;
                            case Domain.Evaluation.CriterionType.None:
                                AddEmptyField(sBuilder, (isFuzzyCommittee ? 3 : 2));
                                break;
                        }
                    }
                    if (eCriterion == null || criterion.CommentType == CommentType.None || string.IsNullOrEmpty(eCriterion.Comment))
                        AddEmptyField(sBuilder);
                    else
                        AddField(sBuilder, eCriterion.Comment);
                }
            #endregion

            #region "Committees"
                public String ExportSummaryDisplayStatistics(dtoCall call, List<dtoCommittee> committees, List<dtoCommitteesSummaryItem> items, litePerson person)
                {
                    Dictionary<EvaluationStatus, Boolean> counters = GetCommitteesCounters( items);
                    Int32 columNumber = counters.Values.ToList().Where(v => v == true).Count();
                    String export = AddStandardEvaluationsInfo(call, person, columNumber) + EndRowItem;
                    String content = AddRow(EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle]);
                    content += AppendRow(3) + CommitteesTableHeader(counters);

                    items.ForEach(item => content += EvaluationStatisticRow(item, counters));

                    export += content;
                    return export;
                }
                private Dictionary<EvaluationStatus, Boolean> GetCommitteesCounters(List<dtoCommitteesSummaryItem> items)
                {
                    Dictionary<EvaluationStatus, Boolean> result = new Dictionary<EvaluationStatus, Boolean>();
                    result[EvaluationStatus.Evaluated] = items.Where(i => i.Status == EvaluationStatus.Evaluated).Any();
                    result[EvaluationStatus.Evaluating] = items.Where(i => i.Status == EvaluationStatus.Evaluating).Any();
                    result[EvaluationStatus.Invalidated] = items.Where(i => i.Status == EvaluationStatus.Invalidated).Any();
                    result[EvaluationStatus.EvaluatorReplacement] = items.Where(i => i.Status == EvaluationStatus.EvaluatorReplacement).Any();
                    result[EvaluationStatus.None] = items.Where(i => i.Status == EvaluationStatus.None).Any();                 
                    return result;
                }
                private String CommitteesTableHeader(Dictionary<EvaluationStatus, Boolean> counters)
                {
                    StringBuilder builder = new StringBuilder();
                    AddField(builder, "#");
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommittee]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsCount]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluated]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluating]));

                    if (!(counters[EvaluationStatus.EvaluatorReplacement] && counters[EvaluationStatus.Invalidated]))
                        AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted]), false);
                    else{
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted]));
                        if (counters[EvaluationStatus.EvaluatorReplacement] && counters[EvaluationStatus.Invalidated]){
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluatorReplacement]));
                            AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsInvalidated]),false);
                        }
                        else if (counters[EvaluationStatus.EvaluatorReplacement])
                            AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluatorReplacement]),false);
                        else
                             AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsInvalidated]),false);
                    }
                    return builder.ToString();
                }
                private String EvaluationStatisticRow(dtoCommitteesSummaryItem item, Dictionary<EvaluationStatus, Boolean> counters)
                {
                    StringBuilder sBuilder = new StringBuilder();
                    String owner = "";
                    if (item.Anonymous)
                        owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                    else
                        owner = item.DisplayName;

                    foreach (dtoCommitteeDisplayItem committee in item.Committees.Where(c=>c.Evaluations.Any()))
                    {
                        AddField(sBuilder,item.Position);
                        AddField(sBuilder,owner);
                        AddField(sBuilder,item.SubmitterType);
                        AddField(sBuilder, committee.CommitteeName);
                        if (committee.Evaluations == null)
                        {
                            AddField(sBuilder, 0);
                            AddField(sBuilder, 0);
                            AddField(sBuilder, 0);
                            AppendToRow(sBuilder, 0, false);
                        }
                        else
                        {
                            AddField(sBuilder, committee.Evaluations.Count);
                            AddField(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.Evaluated));
                            AddField(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.Evaluating));

                            if (!(counters[EvaluationStatus.EvaluatorReplacement] && counters[EvaluationStatus.Invalidated]))
                                AppendToRow(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.None), false);
                            else
                            {
                                AddField(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.None));
                                if (counters[EvaluationStatus.EvaluatorReplacement] && counters[EvaluationStatus.Invalidated])
                                {
                                    AddField(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.EvaluatorReplacement));
                                    AppendToRow(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.Invalidated), false);
                                }
                                else if (counters[EvaluationStatus.EvaluatorReplacement])
                                    AppendToRow(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.EvaluatorReplacement), false);
                                else
                                    AppendToRow(sBuilder, committee.GetEvaluationsCount(EvaluationStatus.Invalidated), false);
                            }
                        }
                    }

                    return sBuilder.ToString();
                }
            #endregion

            #region "Full"
                public String ExportFullSummaryStatistics(dtoCall call, List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCommittee> committees, String anonymousDisplayName, litePerson person, Boolean oneColumnForCriteria)
                {
                    return InternalExportFullSummaryStatistics(call, committees, anonymousDisplayName, person, oneColumnForCriteria);
                }
                public String InternalExportFullSummaryStatistics(dtoCall call, List<expCommittee> committees, String anonymousDisplayName, litePerson person, Boolean oneColumnForCriteria)
                {
                    String export = "";
                    export = EvaluationsTableHeader(call.EvaluationType, call.IsDssMethodFuzzy,committees,oneColumnForCriteria).ToString();
                    if (oneColumnForCriteria)
                    {
                        committees.ToList().ForEach(c => c.Evaluators.OrderBy(e => e.DisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser])).ToList().ForEach(e =>
                         export += AddEvaluatorCriteriaRows(call.EvaluationType,(call.EvaluationType == EvaluationType.Dss && call.IsDssMethodFuzzy), c, e)));
                    }
                    else {
                        int beforeEmptyColumns = 0;
                        Dictionary<long, int> committeeColumns = new Dictionary<long, int>();
                        foreach (expCommittee committee in committees)
                        {
                            if (committee.Criteria == null)
                                committeeColumns.Add(committee.Id, 0);
                            else if (call.EvaluationType == EvaluationType.Dss) { 
                                 if (call.IsDssMethodFuzzy || committee.MethodSettings.IsFuzzyMethod)
                                     committeeColumns.Add(committee.Id, committee.Criteria.Count * 5);
                                 else
                                     committeeColumns.Add(committee.Id, committee.Criteria.Count * 4);
                            }
                            else
                            {
                                committeeColumns.Add(committee.Id, committee.Criteria.Count * 4);
                            }
                        }
                        int criteriaColumns = committeeColumns.Values.Sum();
                        //int criteriaColumns = (committees.Where(c => c.Criteria != null).Any()) ? committees.Where(c => c.Criteria != null).Select(c => c.Criteria.Count).Sum() : 0;
                        //criteriaColumns = criteriaColumns * 4;
                        //if (call.EvaluationType == EvaluationType.Dss && criteriaColumns>0)
                        //{
                        //    if (call.IsDssMethodFuzzy)
                        //        criteriaColumns = criteriaColumns * 5;
                        //    else
                        //    {
                        //        var query = committees.Where(c => c.Criteria != null);
                        //        criteriaColumns = committees.Where(c => c.Criteria != null && c.UseDss && c.MethodSettings.IsFuzzyMethod).Select(c=> c.Criteria.Count()).DefaultIfEmpty(0).Sum()*5;
                        //        criteriaColumns += committees.Where(c => c.Criteria != null && (!c.UseDss || !c.MethodSettings.IsFuzzyMethod)).Select(c => c.Criteria.Count()).DefaultIfEmpty(0).Sum() * 4;
                        //    }
                        //}
                        //else
                        //    criteriaColumns = criteriaColumns * 4;
                        foreach(expCommittee committee in committees){
                            int cColumns = committeeColumns[committee.Id];
                            int afterEmptyColumns = criteriaColumns - beforeEmptyColumns - cColumns;
                            //Boolean isFuzzy = ((call.EvaluationType == EvaluationType.Dss && call.IsDssMethodFuzzy) || committees.Any(c => c.UseDss && c.MethodSettings.IsFuzzyMethod && c.MethodSettings.IsFuzzyMethod));
                            Boolean isFuzzy = ((call.EvaluationType == EvaluationType.Dss && call.IsDssMethodFuzzy) || (committee.UseDss && committee.MethodSettings.IsFuzzyMethod && committee.MethodSettings.IsFuzzyMethod));
                            if (committee.Evaluators !=null)
                                committee.Evaluators.OrderBy(e => e.DisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser])).ToList().ForEach(e => export += AddEvaluatorStatisticsRow(call.EvaluationType,isFuzzy, committee, e, beforeEmptyColumns, afterEmptyColumns));
                            beforeEmptyColumns += cColumns;
                        }
                    }
                    return export;
                }

                private StringBuilder EvaluationsTableHeader(EvaluationType type, Boolean isFuzzy,List<expCommittee> committees,Boolean oneColumnForCriteria)
                {
                    StringBuilder builder = new StringBuilder();
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommittee]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluator]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]));
                    if (oneColumnForCriteria)
                        AddCommitteeCriteriaTableHeader(type, (isFuzzy || committees.Any(c=> c.UseDss && c.MethodSettings.IsFuzzyMethod && c.MethodSettings.IsFuzzyMethod)), builder);
                    else
                        committees.Where(c => c.Criteria != null).ToList().ForEach(c => c.Criteria.ToList().ForEach(cr => AddCommitteeCriteriaTableHeader(type, (isFuzzy || c.MethodSettings.IsFuzzyMethod),builder,cr)));
                    AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]),false);
                    return builder;
                }

                private void AddCommitteeCriteriaTableHeader(EvaluationType type, Boolean isFuzzy,StringBuilder builder, expCriterion criterion = null)
                {
                    if (criterion==null){
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterion]));
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionUserValue]));
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionRangeValue]));
                        if(isFuzzy)
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionFuzzyValue]));
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionComment]));
                    }
                    else{
                        AddField(builder, String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterion], criterion.Name));
                        //AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterion]));
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionUserValue]));
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionRangeValue]));
                        if (isFuzzy)
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionFuzzyValue]));
                        AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionComment]));
                    }                  
                }
                private String AddEvaluatorCriteriaRows(EvaluationType type,Boolean isFuzzyCall,expCommittee committee, expEvaluator evaluator)
                {
                    StringBuilder sBuilder = new StringBuilder();
                   
                    foreach (expEvaluation evaluation in committee.GetEvaluations(evaluator))
                    {
                        foreach (expCriterion criterion in committee.Criteria.OrderBy(c => c.DisplayOrder))
                        {
                            AddField(sBuilder, committee.Name);
                            AddField(sBuilder, (evaluator == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser] : evaluator.DisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]));

                            AddField(sBuilder, evaluation.Submission.OwnerDisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser], EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]));


                            AddField(sBuilder, evaluation.Submission.Type.Name);
                            AddField(sBuilder, EvaluationStatusTranslations[evaluation.Status]);
                            AddCriterionValue(sBuilder, type, isFuzzyCall, criterion, (evaluation.EvaluatedCriteria == null) ? null : evaluation.EvaluatedCriteria.Where(ev => ev.Criterion == criterion).FirstOrDefault());

                            AppendToRow(sBuilder, evaluation.Comment, false);
                        }
                    }
                    return sBuilder.ToString();

                }
                private String AddEvaluatorStatisticsRow(EvaluationType type, Boolean isFuzzyCall, expCommittee committee, expEvaluator evaluator, Int32 beforeEmptyColumns = 0, Int32 afterEmptyColumns = 0)
                {
                    String result ="";
                    StringBuilder sBuilder = new StringBuilder();
                    AddField(sBuilder,committee.Name);
                    AddField(sBuilder, (evaluator == null ) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser] : evaluator.DisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]));

                    foreach (expEvaluation evaluation in committee.GetEvaluations(evaluator))
                    {
                        StringBuilder evaluationBuilder = new StringBuilder();
                        AddField(evaluationBuilder, evaluation.Submission.OwnerDisplayName(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser],EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.UnknownUser]));

                        AddField(evaluationBuilder, evaluation.Submission.Type.Name);
                        AddField(evaluationBuilder, EvaluationStatusTranslations[evaluation.Status]);

                        if (beforeEmptyColumns > 0)
                            AddEmptyField(evaluationBuilder,beforeEmptyColumns);
                        foreach (expCriterion criterion in committee.Criteria.OrderBy(c => c.DisplayOrder))
                        {
                            //
                            AddCriterionValue(evaluationBuilder, type, (isFuzzyCall || (committee.UseDss && committee.MethodSettings != null && committee.MethodSettings.IsFuzzyMethod)), criterion, (evaluation.EvaluatedCriteria == null) ? null : evaluation.EvaluatedCriteria.Where(ev => ev.Criterion == criterion).FirstOrDefault());
                        }
                        if (afterEmptyColumns > 0)
                            AddEmptyField(evaluationBuilder, afterEmptyColumns);
                        AppendToRow(evaluationBuilder, evaluation.Comment, false);
                        result += String.Concat(sBuilder.ToString(), evaluationBuilder.ToString());
                    }
                    return result;
                }
                //Boolean displayFuzzyValue,
                //private String AddCriterionValue(expCriterion criterion, expCriterionEvaluated eCriterion)
                //{
                //    String cColumns = AppendItem(criterion.Name);
                //    if (eCriterion == null || eCriterion.IsValueEmpty)
                //    {
                //        cColumns += AppendItem("");
                //        cColumns += AppendItem("");
                //    }
                //    else
                //    {
                //        switch (eCriterion.Criterion.Type)
                //        {
                //            case Domain.Evaluation.CriterionType.StringRange:
                //                lm.Comol.Modules.CallForPapers.Domain.Evaluation.Export.expCriterionOption option = eCriterion.Criterion.Options.Where(o => eCriterion.Option != null && o.Id == eCriterion.Option.Id).FirstOrDefault();
                //                if (option == null)
                //                {
                //                    cColumns += AppendItem("");
                //                    cColumns += AppendItem("");
                //                }
                //                else
                //                {
                //                    try
                //                    {
                //                        cColumns += AppendItem(option.Name);
                //                        cColumns += AppendItem(option.Value);
                //                    }
                //                    catch (Exception ex)
                //                    {

                //                    }
                //                }
                //                break;
                //            case Domain.Evaluation.CriterionType.Textual:
                //                if (string.IsNullOrEmpty(eCriterion.StringValue))
                //                    cColumns += AppendItem("");
                //                else
                //                    cColumns += AppendItem(eCriterion.StringValue);
                //                cColumns += AppendItem("");
                //                break;
                //            case Domain.Evaluation.CriterionType.DecimalRange:
                //                cColumns += AppendItem(Math.Round(eCriterion.DecimalValue, 2));
                //                cColumns += AppendItem("");
                //                break;
                //            case Domain.Evaluation.CriterionType.IntegerRange:
                //                cColumns += AppendItem(((int)eCriterion.DecimalValue));
                //                cColumns += AppendItem("");
                //                break;
                //            case Domain.Evaluation.CriterionType.None:
                //                cColumns += AppendItem("");
                //                cColumns += AppendItem("");
                //                break;
                //        }
                //    }
                //    if (eCriterion == null || criterion.CommentType == CommentType.None || string.IsNullOrEmpty(eCriterion.Comment))
                //        cColumns+= AppendItem("");
                //    else
                //        cColumns += AppendItem( eCriterion.Comment);
                //    return cColumns;
                //}
            #endregion

            #region "Submission evaluations"
                public String ExportSummaryDisplayStatistics(litePerson person, dtoCall call, Dictionary<long, Boolean > committeesFuzzy,long idSubmission, String owner,String submittedBy, DateTime?  submittedOn, List<dtoSubmissionCommitteeItem> evaluations, Boolean oneColumnForCriteria = true)
                {
                    String result = "";
                    Int32 cNumber = 1;
                    if (!oneColumnForCriteria)
                        cNumber = (evaluations != null && evaluations.Where(e => e.Criteria != null).Any()) ?  evaluations.Where(e => e.Criteria != null).Select(e => e.Criteria.Count).Sum()  : 0;
                    Int32 columNumber =  5 + (cNumber *3);
                    StringBuilder content = new StringBuilder();
                    AddStandardEvaluationInfo(content,call,idSubmission,owner,submittedBy,submittedOn, person, columNumber);

                    if (oneColumnForCriteria)
                    {
                        foreach (dtoSubmissionCommitteeItem item in evaluations.Where(e=> e.Evaluators !=null && e.Evaluators.Any()))
                        {
                            Boolean isFuzzyCommittee = (call.IsDssMethodFuzzy || (item.DssEvaluation!= null && item.DssEvaluation.IsFuzzy) );
                            AddEvaluationCommitteeTableHeader(call.EvaluationType,isFuzzyCommittee, content, item.Criteria);
                            result += content.ToString();
                            item.Evaluators.OrderBy(e => e.EvaluatorName).ToList().ForEach(e =>
                                result += AddEvaluatorStatisticRows(new StringBuilder(), call.EvaluationType, isFuzzyCommittee, item, e).ToString());
                            result += EndRowItem + EndRowItem;
                            content = new StringBuilder();
                        }
                    }
                    else {
                        AddEvaluationCommitteeTableHeader(call.EvaluationType,call.IsDssMethodFuzzy, content);
                        result = content.ToString();
                        foreach (dtoSubmissionCommitteeItem item in evaluations.Where(e=> e.Evaluators != null && e.Evaluators.Any()))
                        {
                            item.Evaluators.OrderBy(e => e.EvaluatorName).ToList().ForEach(e =>
                                result += AddEvaluatorStatisticCriteriaRows(new StringBuilder(), call.EvaluationType, (call.IsDssMethodFuzzy ||(committeesFuzzy.ContainsKey(item.IdCommittee) && committeesFuzzy[item.IdCommittee])), item, e).ToString());
                        }
                    }
                    return result;
                }

                private void AddStandardEvaluationInfo(StringBuilder content, dtoCall call,long idSubmission,String owner,String submittedBy, DateTime?  submittedOn, litePerson person, Int32 columNumber)
                {
                    if (String.IsNullOrEmpty(call.Edition))
                        AppendToRow(content,String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitle], call.Name), false);
                    else
                        AppendToRow(content,AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitleAndEdition], call.Name, call.Edition), false));

                    AppendToRow(content,String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.SubmissionInfo],idSubmission, owner), false);
                    if (submittedOn.HasValue){
                        if (submittedBy != owner)
                            AppendToRow(content,String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.SubmittedByOther], submittedBy, submittedOn.Value.ToShortDateString() , submittedOn.Value.ToShortTimeString()), false);
                        else
                            AppendToRow(content,String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.SubmittedOn], submittedOn.Value.ToShortDateString() , submittedOn.Value.ToShortTimeString()), false);
                    }
               
                    DateTime printTime = DateTime.Now;
                    if (person != null)
                        AppendToRow(content, String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), false);
                    AddEmptyRow(content,2);
                }
                private void AddEvaluationCommitteeTableHeader(EvaluationType type, Boolean isFuzzy, StringBuilder builder)
                {
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommittee]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluator]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]));
                    switch (type)
                    {
                        case EvaluationType.Average:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage]));
                            break;
                        case EvaluationType.Dss:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDss]));
                            if (isFuzzy)
                                AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssFuzzyValue]));
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssEvaluation]));
                            if (isFuzzy)
                                AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssEvaluationFuzzyValue]));
                            break;
                        default:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]));
                            break;
                    }
                    
                    AddCommitteeCriteriaTableHeader(type,isFuzzy, builder);
                    AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]));
                }
                private void AddEvaluationCommitteeTableHeader(EvaluationType type, Boolean isFuzzy, StringBuilder builder, List<dtoCriterionSummaryItem> criteria)
                {
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommittee]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluator]));
                    AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]));
                    switch (type)
                    {
                        case EvaluationType.Average:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage]));
                            break;
                        case EvaluationType.Dss:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDss]));
                            if (isFuzzy)
                                AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssFuzzyValue]));
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssEvaluation]));
                            if (isFuzzy)
                                AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssEvaluationFuzzyValue]));
                            break;
                        default:
                            AddField(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]));
                            break;
                    }
                    if (criteria != null && criteria.Any())
                        criteria.ForEach(cr => AddCommitteeCriteriaTableHeader(type, isFuzzy, builder));
                    AppendToRow(builder, (EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]));

                }
                private StringBuilder AddEvaluatorStatisticCriteriaRows(StringBuilder sBuilder, EvaluationType type, Boolean isFuzzy, dtoSubmissionCommitteeItem committee, dtoEvaluatorDisplayItem evaluator)
                {
                    foreach (dtoCriterionSummaryItem criterion in committee.Criteria.OrderBy(c => c.DisplayOrder))
                    {
                        AddField(sBuilder, committee.Name);
                        AddField(sBuilder, evaluator.EvaluatorName);
                        AddField(sBuilder, EvaluationStatusTranslations[evaluator.Status]);
                        switch (type)
                        {
                            case EvaluationType.Average:
                                AddField(sBuilder, Math.Round((decimal)evaluator.AverageRating, 4));
                                break;
                            case EvaluationType.Dss:
                                if (committee.DssEvaluation == null || !committee.DssEvaluation.IsCompleted || !committee.DssEvaluation.IsValid)
                                {
                                    AddField(sBuilder, "--");
                                    if (isFuzzy)
                                        AddField(sBuilder, "");
                                }
                                else
                                {
                                    AddField(sBuilder, committee.DssEvaluation.Ranking);
                                    if (isFuzzy)
                                        AddField(sBuilder, GetFuzzyValue(committee.DssEvaluation.Value, committee.DssEvaluation.ValueFuzzy));
                                }

                                if (evaluator.DssEvaluation == null || !evaluator.DssEvaluation.IsValid)
                                {
                                    AddField(sBuilder, "--");
                                    if (isFuzzy)
                                        AddField(sBuilder, "");
                                }
                                else
                                {
                                    AddField(sBuilder, evaluator.DssEvaluation.Ranking);
                                    if (isFuzzy)
                                        AddField(sBuilder, GetFuzzyValue(evaluator.DssEvaluation.Value, evaluator.DssEvaluation.ValueFuzzy));
                                }
                                break;
                            default:
                                AddField(sBuilder, Math.Round((decimal)evaluator.SumRating, 4));
                                break;
                        } 
                        AddCriterionValue(sBuilder,type,isFuzzy, criterion, (evaluator.Values == null) ? null : evaluator.Values.Where(ev => ev.Criterion.Id == criterion.Id).FirstOrDefault());
                        AppendToRow(sBuilder, evaluator.Comment);
                    }
                    return sBuilder;
                }
                private StringBuilder AddEvaluatorStatisticRows(StringBuilder sBuilder, EvaluationType type, Boolean isFuzzy, dtoSubmissionCommitteeItem committee, dtoEvaluatorDisplayItem evaluator)
                {
                    AddField(sBuilder, committee.Name);
                    AddField(sBuilder, evaluator.EvaluatorName);
                    AddField(sBuilder, EvaluationStatusTranslations[evaluator.Status]);
                    switch (type)
                    {
                        case EvaluationType.Average:
                            AddField(sBuilder, Math.Round((decimal)evaluator.AverageRating, 4));
                            break;
                        case EvaluationType.Dss:
                            if (committee.DssEvaluation == null || !committee.DssEvaluation.IsCompleted || !committee.DssEvaluation.IsValid)
                            {
                                AddField(sBuilder, "--");
                                if (isFuzzy)
                                    AddField(sBuilder, "");
                            }
                            else
                            {
                                AddField(sBuilder, committee.DssEvaluation.Ranking);
                                if (isFuzzy)
                                    AddField(sBuilder, GetFuzzyValue(committee.DssEvaluation.Value, committee.DssEvaluation.ValueFuzzy));
                            }
                            if (evaluator.DssEvaluation == null || !evaluator.DssEvaluation.IsValid)
                            {
                                AddField(sBuilder, "--");
                                if (isFuzzy)
                                    AddField(sBuilder, "");
                            }
                            else
                            {
                                AddField(sBuilder, evaluator.DssEvaluation.Ranking);
                                if (isFuzzy)
                                    AddField(sBuilder, GetFuzzyValue(evaluator.DssEvaluation.Value, evaluator.DssEvaluation.ValueFuzzy));
                            }
                            break;
                        default:
                            AddField(sBuilder, Math.Round((decimal)evaluator.SumRating, 4));
                            break;
                    }
                    committee.Criteria.OrderBy(c => c.DisplayOrder).ToList().ForEach(c =>
                        AddCriterionValue(sBuilder,type,isFuzzy, c, (evaluator.Values == null) ? null : evaluator.Values.Where(ev => ev.Criterion.Id == c.Id).FirstOrDefault()));
                   
                    AppendToRow(sBuilder, evaluator.Comment);
                    return sBuilder;
                }

                private void AddCriterionValue(StringBuilder sBuilder, EvaluationType type, Boolean isFuzzy, dtoCriterionSummaryItem criterion, dtoCriterionEvaluated eCriterion)
                {
                    AddField(sBuilder, criterion.Name);
                    if (eCriterion == null || eCriterion.IsValueEmpty)
                        AddEmptyField(sBuilder, (isFuzzy ? 3 : 2));
                    else
                    {
                        switch (eCriterion.Criterion.Type)
                        {
                            case Domain.Evaluation.CriterionType.StringRange:
                                dtoCriterionOption option = eCriterion.Criterion.Options.Where(o => o.Id == eCriterion.IdOption).FirstOrDefault();
                                if (option == null)
                                    AddEmptyField(sBuilder,(isFuzzy? 3: 2));
                                else
                                {
                                    try
                                    {
                                        AddField(sBuilder, option.Name);
                                        AddField(sBuilder, option.Value);
                                        if (isFuzzy)
                                            AddField(sBuilder, ((double)option.Value).ToFuzzy().ToString());
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;
                            case Domain.Evaluation.CriterionType.Textual:
                                if (string.IsNullOrEmpty(eCriterion.StringValue))
                                    AddEmptyField(sBuilder);
                                else
                                    AddField(sBuilder, eCriterion.StringValue);
                                AddEmptyField(sBuilder);
                                if (isFuzzy)
                                    AddEmptyField(sBuilder);
                                break;
                            case Domain.Evaluation.CriterionType.DecimalRange:
                                AddField(sBuilder, Math.Round(eCriterion.DecimalValue, 2));
                                AddField(sBuilder, Math.Round(eCriterion.DecimalValue, 2));
                                if (isFuzzy)
                                    AddEmptyField(sBuilder);
                                break;
                            case Domain.Evaluation.CriterionType.IntegerRange:
                                AddField(sBuilder, ((int)eCriterion.DecimalValue));
                                AddField(sBuilder, ((int)eCriterion.DecimalValue));
                                if (isFuzzy)
                                    AddField(sBuilder, ((double)eCriterion.DecimalValue).ToFuzzy().ToString());
                                break;
                            case Domain.Evaluation.CriterionType.RatingScale:
                            case Domain.Evaluation.CriterionType.RatingScaleFuzzy:
                                lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption dssOption = eCriterion.Criterion.Options.Where(o => o.IdRatingValue == eCriterion.DssValue.IdRatingValue).FirstOrDefault();
                                if (type == EvaluationType.Dss )
                                {
                                    if (eCriterion.DssValue.IsValid())
                                    {
                                        String displayName = "";
                                        lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption dssOptionEnd = eCriterion.Criterion.Options.Where(o => o.IdRatingValue == eCriterion.DssValue.IdRatingValueEnd).FirstOrDefault();
                                        switch (eCriterion.DssValue.RatingType)
                                        {
                                            case RatingType.simple:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                break;
                                            case RatingType.extended:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;

                                                if (dssOptionEnd != null)
                                                    displayName = displayName + "->" + dssOptionEnd.Name;
                                                break;
                                            case RatingType.intermediateValues:
                                                if (dssOption != null)
                                                    displayName = dssOption.Name;
                                                if (dssOptionEnd != null)
                                                    displayName = displayName + "/" + dssOptionEnd.Name;
                                                break;
                                        }
                                        AddField(sBuilder, displayName);
                                        if (isFuzzy)
                                        {
                                            AddField(sBuilder, ((double)(eCriterion.DssValue.Value)));
                                            if (eCriterion.DssValue.IsFuzzy && !String.IsNullOrWhiteSpace(eCriterion.DssValue.ValueFuzzy))
                                                AddField(sBuilder, eCriterion.DssValue.ValueFuzzy);
                                            else
                                                AddField(sBuilder, ((double)(eCriterion.DssValue.Value)).ToFuzzy().ToString());
                                        }
                                        else
                                        {
                                            AddField(sBuilder, ((double)(eCriterion.DssValue.Value)));
                                        }
                                    }
                                    else
                                        AddEmptyField(sBuilder, (isFuzzy ? 3 : 2));
                                }
                                else
                                {
                                    AddField(sBuilder, dssOption.DoubleValue);
                                    AddField(sBuilder, dssOption.DoubleValue);
                                    if (isFuzzy)
                                        AddField(sBuilder, dssOption.FuzzyValue);
                                }
                                break;
                            case Domain.Evaluation.CriterionType.None:
                                AddEmptyField(sBuilder, (isFuzzy ? 3 :2));
                                break;
                        }
                    }
                    if (eCriterion == null || criterion.CommentType == CommentType.None || string.IsNullOrEmpty(eCriterion.Comment))
                        AddEmptyField(sBuilder);
                    else
                        AddField(sBuilder, eCriterion.Comment);
                }
            #endregion
        

                private Dictionary<EvaluationStatus, Boolean> GetCounters(List<dtoEvaluationSummaryItem> items)
                {
                    Dictionary<EvaluationStatus, Boolean> counters = new Dictionary<EvaluationStatus, Boolean>();
                    counters[EvaluationStatus.Confirmed] = items.Where(i => i.Evaluations.Where(e => e.Evaluated && e.Status == EvaluationStatus.Confirmed).Any()).Any();
                    counters[EvaluationStatus.Evaluated] = items.Where(i => i.Evaluations.Where(e => e.Evaluated && e.Status == EvaluationStatus.Evaluated).Any()).Any();
                    counters[EvaluationStatus.Evaluating] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Evaluating).Any()).Any();
                    counters[EvaluationStatus.EvaluatorReplacement] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).Any();
                    counters[EvaluationStatus.Invalidated] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).Any();
                    counters[EvaluationStatus.None] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.None).Any()).Any();

                    return counters;
                }

                private String EvaluationsTableHeader(EvaluationType type, Boolean isFuzzy)
                {
                    String tableData = AppendItem("#");
                    tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]);
                    tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]);
                    switch (type)
                    {
                        case EvaluationType.Average:
                            tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleAverage]);
                            break;
                        case EvaluationType.Dss:
                            tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDss]);
                            if (isFuzzy)
                                tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleDssFuzzyValue]);
                            break;
                        case EvaluationType.Sum:
                            tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]);
                            break;
                    }
                    
                    ///// REMOVE delimiter
                    //if (tableData.EndsWith(EndFieldItem))
                    //    tableData = tableData.Remove(tableData.Length - EndFieldItem.Length, EndFieldItem.Length);
                    return tableData;
                }
                private String EvaluationsTableHeader(EvaluationType type, Boolean isFuzzy,Dictionary<EvaluationStatus, Boolean> counters)
                {
                    String tableData = EvaluationsTableHeader(type, isFuzzy);
                    if (counters.Where(c => c.Value).Count() > 1)
                        tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsCount]);
                    if (counters[EvaluationStatus.Evaluated])
                        tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluated]);
                    if (counters[EvaluationStatus.Evaluating])
                        tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluating]);
                    if (counters[EvaluationStatus.None])
                        tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted]);
                    if (counters[EvaluationStatus.EvaluatorReplacement])
                        tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluatorReplacement]);
                    if (counters[EvaluationStatus.Invalidated])
                        tableData += AppendItem(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsInvalidated]);
                    /// REMOVE delimiter
                    if (tableData.EndsWith(EndFieldItem))
                        tableData = tableData.Remove(tableData.Length - EndFieldItem.Length, EndFieldItem.Length);
                    return tableData + EndRowItem;
                }
                private String EvaluationStatisticRow(EvaluationType type,Boolean useFuzzy, dtoEvaluationSummaryItem evaluation, Dictionary<EvaluationStatus, Boolean> counters, int rowNumber)
                {
                    String row = "";
                    String owner = "";
                    if (evaluation.Anonymous)
                        owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                    else
                        owner = evaluation.DisplayName;

                    row += AppendItem(evaluation.Position.ToString());
                    row += AppendItem(owner);
                    row += AppendItem(evaluation.SubmitterType);
                    switch (type)
                    {
                        case EvaluationType.Average:
                            row += AppendItem(Math.Round((decimal)evaluation.AverageRating, 4));
                            break;
                        case EvaluationType.Dss:
                            if (counters.Any(c => c.Value && c.Key == EvaluationStatus.None) || evaluation.DssEvaluation == null || (!evaluation.DssEvaluation.IsCompleted))
                            {
                                row += AppendItem("--");
                                if (useFuzzy)
                                    row += AppendItem("");
                            }
                            else
                            {
                                if (evaluation.DssEvaluation == null)
                                {
                                    row += AppendItem(0);
                                    if (useFuzzy)
                                        row += AppendItem(((double)0).ToFuzzy().ToString());
                                }
                                else
                                {
                                    row += AppendItem(evaluation.DssRatingToString(4));
                                    if (evaluation.DssEvaluation.IsFuzzy || useFuzzy)
                                        row += AppendItem(GetFuzzyValue(evaluation.DssEvaluation.Value, evaluation.DssEvaluation.ValueFuzzy));
                                }
                            }
                            break;
                        default:
                            row += AppendItem(Math.Round((decimal)evaluation.SumRating, 2));
                            break;
                    }
                    

                    if (counters.Where(c => c.Value).Count() > 1)
                        row += AppendItem(evaluation.Evaluations.Count());
                    if (counters[EvaluationStatus.Evaluated])
                        row += AppendItem(evaluation.Counters[EvaluationStatus.Evaluated]);
                    if (counters[EvaluationStatus.Evaluating])
                        row += AppendItem(evaluation.Counters[EvaluationStatus.Evaluating]);
                    if (counters[EvaluationStatus.None])
                        row += AppendItem(evaluation.Counters[EvaluationStatus.None]);
                    if (counters[EvaluationStatus.EvaluatorReplacement])
                        row += AppendItem(evaluation.Counters[EvaluationStatus.EvaluatorReplacement]);
                    if (counters[EvaluationStatus.Invalidated])
                        row += AppendItem(evaluation.Counters[EvaluationStatus.Invalidated]);
                    if (counters[EvaluationStatus.Confirmed])
                        row += AppendItem(evaluation.Counters[EvaluationStatus.Confirmed]);
                    return row + EndRowItem;
                }
                private String GetFuzzyValue(double value, String valueFuzzy)
                {
                    if (!String.IsNullOrWhiteSpace(valueFuzzy))
                        return valueFuzzy;
                    else
                        return value.ToFuzzy().ToString();
                }
                public String GetErrorDocument(dtoCall call, litePerson person)
                {
                    String header = AddStandardErrorInfo(call, person) + EndRowItem;
                    return header;
                }
                private String AddStandardErrorInfo(dtoCall call, litePerson person)
                {
                    String infoRow = "";
                    if (String.IsNullOrEmpty(call.Edition))
                        infoRow += AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitle], call.Name));
                    else
                        infoRow += AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitleAndEdition], call.Name, call.Edition));

                    infoRow += EndRowItem;

                    DateTime printTime = DateTime.Now;
                    if (person != null)
                        infoRow += AppendItem(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString())) +EndRowItem;
                    infoRow += AppendRow(3);
                    return infoRow;
                }
            #endregion
            #region "RequestsData"
                public String ExportSubmissionsData(String name, String edition, List<dtoCallSection<dtoCallField>> requestSchema, List<dtoSubmissionDataDisplay> submissions, litePerson person, SubmissionFilterStatus filter)
                {
                    Int32 columNumber = 4 + GetColumnNumberFromSchema(requestSchema);
                    String export = "";
                    String header = AddSubmissionsListInfo(CallForPaperType.RequestForMembership, name, edition, person, columNumber);
                    header += RequestsListTableHeader(filter, requestSchema);
                    int rowNumber = 1;
                    foreach (dtoSubmissionDataDisplay sub in submissions)
                    {
                        header += AddSubmissionDataRow(requestSchema, sub, filter, rowNumber);
                        rowNumber++;
                    }
                    export += header;
                    return export;
                }
                private Int32 GetColumnNumberFromSchema(List<dtoCallSection<dtoCallField>> requestSchema)
                {
                    Int32 column = 0;
                    requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).ToList().ForEach(s => column += s.Fields.Where(f => f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.None && f.Type != FieldType.Note && f.Type != FieldType.RadioButtonList && f.Type != FieldType.DropDownList && f.Type != FieldType.CheckboxList).Count());
                    requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).ToList().ForEach(s => column += s.Fields.Where(f => f.Deleted == BaseStatusDeleted.None && (f.Type == FieldType.RadioButtonList || f.Type == FieldType.DropDownList || f.Type == FieldType.CheckboxList)).Count() * 2);
                    return column;
                }
                private String RequestsListTableHeader(SubmissionFilterStatus filter, List<dtoCallSection<dtoCallField>> requestSchema)
                {
                    SubmissionsListTranslations dateCell = SubmissionsListTranslations.CellLastUpdateOn;
                    switch (filter)
                    {
                        case SubmissionFilterStatus.OnlySubmitted:
                            dateCell = SubmissionsListTranslations.CellSubmittedOn;
                            break;
                        case SubmissionFilterStatus.VirtualDeletedSubmission:
                            dateCell = SubmissionsListTranslations.CellDeletedOn;
                            break;
                    }
                    String tableData = AppendItem(CommonTranslations[SubmissionsListTranslations.CellSubmitter]);
                    tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellSubmissionType])
                                    + AppendItem(CommonTranslations[dateCell]);
                    tableData += AppendItem(CommonTranslations[SubmissionsListTranslations.CellStatus]);

                    foreach (var section in requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).OrderBy(s => s.DisplayOrder).ToList())
                    {
                        foreach (var field in section.Fields.Where(f =>
                            f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.None && f.Type != FieldType.Note).OrderBy(f => f.DisplayOrder).ToList())
                        {
                            if (field.Type == FieldType.DropDownList || field.Type == FieldType.RadioButtonList || field.Type == FieldType.CheckboxList)
                            {
                                tableData += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.CellMultipleFieldName], field.Name));
                                tableData += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.CellMultipleFieldValues], field.Name));
                                if (field.HasFreeOption)
                                    tableData += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.CellMultipleFieldFreeText], field.Name));
                            }
                            else
                                tableData += AppendItem(field.Name);
                        }
                    }
                    return tableData + EndRowItem;
                }

                private String AddSubmissionDataRow(List<dtoCallSection<dtoCallField>> requestSchema, dtoSubmissionDataDisplay sub, SubmissionFilterStatus filter, int rowNumber)
                {
                    String row = "";
                    String owner = "";
                    if (sub.Owner == null && sub.IsAnonymous == false)
                        owner = CommonTranslations[SubmissionsListTranslations.DeletedUser];
                    else if (sub.IsAnonymous || (sub.Owner != null && sub.Owner.TypeID == (int)UserTypeStandard.Guest))
                        owner = CommonTranslations[SubmissionsListTranslations.AnonymousOwner];
                    else
                        owner = sub.Owner.SurnameAndName;

                    if (!sub.SubmittedOn.HasValue || sub.SubmittedBy == null)
                        row += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.CreatedByInfo], owner));
                    else if (sub.Owner == sub.SubmittedBy)
                        row += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedByInfo], owner));
                    else
                        row += AppendItem(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedForInfo], owner, sub.SubmittedBy.SurnameAndName));

                    row += AppendItem(sub.Type.Name);
                    if (filter == SubmissionFilterStatus.OnlySubmitted && sub.SubmittedOn.HasValue)
                        row += AppendItem(sub.SubmittedOn.Value.ToShortDateString() + ' ' + sub.SubmittedOn.Value.ToShortTimeString());
                    else
                        row += AppendItem(sub.ModifiedOn.Value.ToShortDateString() + ' ' + sub.ModifiedOn.Value.ToShortTimeString());
                    row += AppendItem(StatusTranslations[sub.Status]);

                    //String emptyItem = CommonTranslations[SubmissionsListTranslations.EmptyItem];
                    String itemAccept = CommonTranslations[SubmissionsListTranslations.DisclaimerAccept];
                    String itemRefused = CommonTranslations[SubmissionsListTranslations.DisclaimerReject];
                    String itemRead = CommonTranslations[SubmissionsListTranslations.DisclaimerRead];


                    foreach (var section in requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).OrderBy(s => s.DisplayOrder).ToList())
                    {
                        foreach (var field in section.Fields.Where(f =>
                            f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.None && f.Type != FieldType.Note).OrderBy(f => f.DisplayOrder).ToList())
                        {
                            if (sub.Sections.Where(s => s.Fields.Where(f => f.IdField == field.Id).Any()).Any())
                            {
                                dtoSubmissionValueField item = sub.Sections.Where(s => s.Id == section.Id).FirstOrDefault().Fields.Where(f => f.IdField == field.Id).FirstOrDefault();
                                switch (field.Type)
                                {
                                    case FieldType.Disclaimer:
                                        switch (field.DisclaimerType) { 
                                            case DisclaimerType.None:
                                                row += AppendItem("");
                                                break;
                                            case DisclaimerType.Standard:
                                                row += AppendItem((item.Value.Text == "True" ? itemAccept : itemRefused));
                                                break;
                                            case DisclaimerType.CustomDisplayOnly:
                                                row += AppendItem(itemRead);
                                                break;
                                            default:
                                                MultipleChoiceFieldToCell(ref row, item, this.CommonTranslations, field.HasFreeOption);
                                                break;
                                        }
                                        break;
                                    case FieldType.CheckboxList:
                                    case FieldType.RadioButtonList:
                                    case FieldType.DropDownList:
                                        MultipleChoiceFieldToCell(ref row, item, this.CommonTranslations, field.HasFreeOption);
                                        break; 
                                    case FieldType.FileInput:
                                        row += AppendItem((item.Value ==null || item.Value.IdLink == 0) ? CommonTranslations[SubmissionsListTranslations.FileNotSubmitted] : CommonTranslations[SubmissionsListTranslations.FileSubmitted]);
                                        break;
                                    case FieldType.Note:
                                        break;
                                    default:
                                        row += AppendItem((item.Value == null || String.IsNullOrEmpty(item.Value.Text)) ? "" : item.Value.Text);
                                        break;
                                }
                            }
                            else
                            {
                                row += AppendItem("");
                                if (field.Type == FieldType.CheckboxList || field.Type == FieldType.DropDownList || field.Type == FieldType.RadioButtonList || (field.Type == FieldType.Disclaimer && (field.DisclaimerType == DisclaimerType.CustomSingleOption || field.DisclaimerType == DisclaimerType.CustomMultiOptions)))
                                    row += AppendItem("");
                                if (field.HasFreeOption)
                                    row += AppendItem("");
                            }
                        }
                    }
                    return row + EndRowItem;
                }

                private void MultipleChoiceFieldToCell(ref String row, dtoSubmissionValueField item, Dictionary<SubmissionsListTranslations, string> translations, Boolean hasFreeOption)
                {
                    List<String> mValue = (item.Value == null || String.IsNullOrEmpty(item.Value.Text)) ? new List<String>() : item.Value.Text.Split('|').ToList();
                    String itemValues = "";
                    String itemNames = "";

                    itemNames = String.Join("- ", item.Field.Options.Where(o => mValue.Contains(o.Id.ToString())).OrderByDescending(o => o.Name).Select(o => o.Name).ToArray());
                    itemValues = item.Value.Text;

                    row += AppendItem(itemNames);
                    row += AppendItem(itemValues);
                    if (hasFreeOption)
                        row += AppendItem(item.Value.FreeText);
                }
                #endregion 
        #region Common
            public enum ExportContentType
            {
                SubmissionList = 1,
                Evaluations = 2,
                RequestsData = 3
            }
        #endregion


      
    }
}
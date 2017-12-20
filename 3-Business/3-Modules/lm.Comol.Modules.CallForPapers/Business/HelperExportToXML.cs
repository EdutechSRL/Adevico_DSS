using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Modules.CallForPapers.Domain.Evaluation;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public class HelperExportToXml //: lm.Comol.Core.DomainModel.Helpers.Export.ExportXmlBaseHelper 
    {
        private Dictionary<SubmissionsListTranslations, string> CommonTranslations { get; set; }
        private Dictionary<SubmissionStatus, String> StatusTranslations { get; set; }
        private Dictionary<RevisionStatus, String> RevisionStatusTranslations { get; set; }
        private Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations,String> EvaluationsTranslations { get; set; }
        private Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> EvaluationStatusTranslations { get; set; }

        private ExportContentType ContentType { get; set; }

        public HelperExportToXml() {
            CommonTranslations = new Dictionary<SubmissionsListTranslations, string>();
            StatusTranslations = new Dictionary<SubmissionStatus, string>();
            RevisionStatusTranslations = new Dictionary<RevisionStatus, string>();
        }
        public HelperExportToXml(Dictionary<SubmissionsListTranslations, string> translations,Dictionary<SubmissionStatus, String> status,Dictionary<RevisionStatus, string> revisions)
            : this()
        {
             CommonTranslations = translations;
             StatusTranslations = status;
             RevisionStatusTranslations = revisions;
             ContentType = ExportContentType.SubmissionList;
        }
        public HelperExportToXml(Dictionary<SubmissionsListTranslations, string> translations,Dictionary<SubmissionStatus, String> status,Dictionary<RevisionStatus, string> revisions, ExportContentType content)
            : this(translations,status,revisions)
        {
             ContentType = content;
        }

        public HelperExportToXml(Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationTranslations, string> translations, Dictionary<lm.Comol.Modules.CallForPapers.Domain.Evaluation.EvaluationStatus, String> status)
            : this()
        {
            EvaluationsTranslations = translations;
            EvaluationStatusTranslations = status;
            ContentType = ExportContentType.Evaluations;
        }

        //#region "Implemented"
        //    protected override String RenderErrorDocument()
        //    {
        //        return "";
        //        //doc.Add(GetPageTitle(Translations[SubmissionTranslations.FileCreationError], GetFont(ItemType.Title)));
        //    }
        //    protected override String RenderDocument()
        //    {
        //        return "";
        //        //dtoExportSubmission settings = Settings;
        //        //if (settings.ForCompile)
        //        //    ExportSubmission(doc, settings.Call, settings.Submitter, settings.RequiredFiles, settings.Sections, settings.PrintBy, Translations);
        //        //else
        //        //    ExportSubmission(doc, settings.Submission, settings.RequiredFiles, settings.Sections, settings.PrintBy, Translations);
        //    }
        //#endregion
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
                header += SubmissionsListTableHeader(filter,  hasRevisions, showMail);
                int rowNumber = 1;
                foreach (dtoSubmissionDisplay sub in submissions)
                {
                    header += AddSubmissionListRow(sub, filter,hasRevisions, showMail, rowNumber);
                    rowNumber++;
                }
                export += BuilderXmlDocument.AddWorkSheet("--", header);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            public static String GetErrorDocument(String translation)
            {
                String export = "";
                String header = BuilderXmlDocument.AddData(translation, DefaultXmlStyleElement.Title.ToString(), 6);
                export += BuilderXmlDocument.AddWorkSheet("--", header);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private String AddSubmissionsListInfo(CallForPaperType type, String name, String edition, litePerson person,  Int32 columNumber)
            {
                String infoRow = ""; // BuilderXmlDocument.AddData(CommonTranslations[(type == CallForPaperType.CallForBids) ? SubmissionsListTranslations.CallTitle : SubmissionsListTranslations.RequestTitle]);

                if (String.IsNullOrEmpty(edition))
                    infoRow += BuilderXmlDocument.AddData(string.Format(CommonTranslations[(type == CallForPaperType.CallForBids) ? SubmissionsListTranslations.CallName : SubmissionsListTranslations.RequestName], name), DefaultXmlStyleElement.Title.ToString(), columNumber);
                else
                    infoRow += BuilderXmlDocument.AddData(string.Format(CommonTranslations[(type == CallForPaperType.CallForBids) ? SubmissionsListTranslations.CallNameNameAndEdition:SubmissionsListTranslations.RequestNameNameAndEdition  ], name, edition), DefaultXmlStyleElement.Title.ToString(), columNumber);

                infoRow = BuilderXmlDocument.AddRow(infoRow);

                DateTime printTime = DateTime.Now;
                if (person != null)
                    infoRow += BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(string.Format(CommonTranslations[SubmissionsListTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), DefaultXmlStyleElement.PrintInfo.ToString(), columNumber));
                else
                    infoRow += BuilderXmlDocument.AddEmptyRow();
                infoRow += BuilderXmlDocument.AddEmptyRows(3);
                return infoRow;
            }
            private String SubmissionsListTableHeader(SubmissionFilterStatus filter, Boolean hasRevisions, Boolean showMail)
            {
                SubmissionsListTranslations dateCell = SubmissionsListTranslations.CellLastUpdateOn;
                switch (filter) { 
                    case SubmissionFilterStatus.OnlySubmitted:
                        dateCell = SubmissionsListTranslations.CellSubmittedOn;
                        break;
                    case SubmissionFilterStatus.VirtualDeletedSubmission:
                        dateCell = SubmissionsListTranslations.CellDeletedOn;
                        break;
                }
                String tableData = BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellSubmitter], DefaultXmlStyleElement.HeaderTable.ToString());
                if (showMail)
                    tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellSubmitterMail], DefaultXmlStyleElement.HeaderTable.ToString());
                tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellSubmissionType], DefaultXmlStyleElement.HeaderTable.ToString())
                                + BuilderXmlDocument.AddData(CommonTranslations[dateCell], DefaultXmlStyleElement.HeaderTable.ToString());
              
                if (hasRevisions) {
                    //tableData += BuilderXmlDocument.AddData(translations[SubmissionsListTranslations.CellIsRevision], DefaultXmlStyleElement.HeaderTable.ToString());
                    tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellHasRevision], DefaultXmlStyleElement.HeaderTable.ToString());
                    tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellRevisionRequest], DefaultXmlStyleElement.HeaderTable.ToString());
                    tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellRevisionApproved], DefaultXmlStyleElement.HeaderTable.ToString());
                }

                tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellStatus], DefaultXmlStyleElement.HeaderTable.ToString());

                return BuilderXmlDocument.AddRow(tableData);
            }
            private String AddSubmissionListRow(dtoSubmissionDisplay sub,  SubmissionFilterStatus filter, Boolean hasRevisions, Boolean showMail, int rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (sub.Deleted == BaseStatusDeleted.None ? (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem) : DefaultXmlStyleElement.RowDeleted);
                String row = "";

                String owner = "";
                if (sub.Owner == null && sub.IsAnonymous==false)
                    owner = CommonTranslations[SubmissionsListTranslations.DeletedUser];
                else if (sub.IsAnonymous || (sub.Owner != null && sub.Owner.TypeID == (int) UserTypeStandard.Guest))
                    owner = CommonTranslations[SubmissionsListTranslations.AnonymousOwner];
                else
                    owner = sub.Owner.SurnameAndName;

                if (!sub.SubmittedOn.HasValue || sub.SubmittedBy==null)
                    row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.CreatedByInfo], owner), rowstyle.ToString());
                else if (sub.Owner == sub.SubmittedBy)
                    row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedByInfo], owner), rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedForInfo], owner, sub.SubmittedBy.SurnameAndName));

                if (showMail)
                {
                    if (sub.Owner == null || sub.IsAnonymous || (sub.Owner != null && sub.Owner.TypeID == (int) UserTypeStandard.Guest))
                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData(sub.Owner.Mail, rowstyle.ToString());
                }

                row += BuilderXmlDocument.AddData(sub.Type.Name, rowstyle.ToString());
                if (filter == SubmissionFilterStatus.OnlySubmitted && sub.SubmittedOn.HasValue)
                    row += BuilderXmlDocument.AddData(sub.SubmittedOn.Value.ToShortDateString() + ' ' + sub.SubmittedOn.Value.ToShortTimeString(), rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData(sub.ModifiedOn.Value.ToShortDateString() + ' ' + sub.ModifiedOn.Value.ToShortTimeString(), rowstyle.ToString());

                if (hasRevisions){
                    if (sub.Revisions.Count > 1)
                    {
                        row += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.TrueValue], rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(sub.Revisions.Where(r => r.RevisionType != RevisionType.Original).Count().ToString(), rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(sub.Revisions.Where(r => r.RevisionType != RevisionType.Original && r.isActive).Count().ToString(), rowstyle.ToString());
                    }
                    else {
                        row += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.FalseValue], rowstyle.ToString());
                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                    }
                }
                row += BuilderXmlDocument.AddData(StatusTranslations[sub.Status], rowstyle.ToString());

                String rows = BuilderXmlDocument.AddRow(row);
                //if (hasRevisions)
                //    sub.Revisions.ForEach(r => rows += AddRevisionRow(rowstyle, r, filter, StatusTranslations[sub.Status], showMail));
                return rows;
            }
            //private String AddRevisionRow(DefaultXmlStyleElement rowstyle, dtoSubmissionItem dto, SubmissionFilterStatus filter, String submissionStatus, Boolean showMail)
            //{
            //    String row = "";
            //    switch(dto.RevisionType){
            //        case RevisionType.Original:
            //            row += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.RevisionTypeOrigin], rowstyle.ToString());
            //            break;
            //        case RevisionType.UserRequired:
            //            row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.RevisionTypeUserRequest], dto.DisplayNumber), rowstyle.ToString());
            //            break;
            //        case RevisionType.Manager:
            //            row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.RevisionTypeManagerRequest], dto.DisplayNumber), rowstyle.ToString());
            //            break;
            //    }

            //    if (showMail)
            //    {
            //        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
            //        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
            //    }
            //    // type cell
            //    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());

            //    if (dto.RevisionType == RevisionType.Original)
            //    {
            //        if (filter == SubmissionFilterStatus.OnlySubmitted && dto.Submission.SubmittedOn.HasValue)
            //            row += BuilderXmlDocument.AddData(dto.Submission.SubmittedOn.Value.ToShortDateString() + ' ' + dto.Submission.SubmittedOn.Value.ToShortTimeString(), rowstyle.ToString());
            //        else
            //            row += BuilderXmlDocument.AddData(dto.Submission.ModifiedOn.Value.ToShortDateString() + ' ' + dto.Submission.ModifiedOn.Value.ToShortTimeString(), rowstyle.ToString());
            //        row += BuilderXmlDocument.AddData(submissionStatus, rowstyle.ToString());
            //    }
            //    else
            //    {
            //        row += BuilderXmlDocument.AddData(dto.Submission.ModifiedOn.Value.ToShortDateString() + ' ' + dto.Submission.ModifiedOn.Value.ToShortTimeString(), rowstyle.ToString());
            //        row += BuilderXmlDocument.AddData(RevisionStatusTranslations[dto.RevisionStatus], rowstyle.ToString());
            //    }
            //    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());


            //    return BuilderXmlDocument.AddRow(row);
            //}
           
        #endregion

        #region "evaluations"
            public String ExportEvaluatorStatistics(dtoCall call, List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluatorCommitteeStatistic> statistics, litePerson person)
            {
                Int32 columNumber = statistics.Select(s => GetColumnNumber(s.Criteria)).Max();
                String export = "";
                String header = AddStandardEvaluationsInfo(call, person, columNumber);

                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluatorCommitteeStatistic committee in statistics) {
                    String content = BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CommitteeName], committee.Name), DefaultXmlStyleElement.Title.ToString(), columNumber));
                    content += BuilderXmlDocument.AddEmptyRows(3, columNumber);
                    content += CommitteeTableHeader(committee);
                    int rowNumber = 1;
                    List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption> options = new List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionOption>();
                    committee.Criteria.Where(c => c.Type == Domain.Evaluation.CriterionType.StringRange && c.Options.Any()).ToList().ForEach(c => options.AddRange(c.Options));
                    foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation in committee.Evaluations)
                    {
                        content += EvaluationStatisticRow(committee.Criteria, options, evaluation, rowNumber);
                        rowNumber++;
                    }
                    export += BuilderXmlDocument.AddWorkSheet(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommittee], header + content);
                }
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private String AddStandardEvaluationsInfo(dtoCall call, litePerson person, Int32 columNumber)
            {
                String infoRow = "";
                //infoRow += BuilderXmlDocument.AddEmptyRows(2, columNumber);
                if (String.IsNullOrEmpty(call.Edition))
                    infoRow += BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitle], call.Name), DefaultXmlStyleElement.Title.ToString(), columNumber);
                else
                    infoRow += BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitleAndEdition], call.Name, call.Edition), DefaultXmlStyleElement.Title.ToString(), columNumber);

                infoRow = BuilderXmlDocument.AddRow(infoRow);

                DateTime printTime = DateTime.Now;
                if (person != null)
                    infoRow += BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), DefaultXmlStyleElement.PrintInfo.ToString(), columNumber));
                infoRow += BuilderXmlDocument.AddEmptyRows(3,columNumber);
                return infoRow;
            }
            private String CommitteeTableHeader(lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluatorCommitteeStatistic committee)
            {
                String tableData = AddTableCellTitle("#");
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]);
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]);
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]);
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]);
                foreach(lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion criterion in committee.Criteria.OrderBy(c=>c.DisplayOrder).ThenBy(c=>c.Name).ToList()){
                    tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterion], criterion.Name));
                    if (criterion.Type== Domain.Evaluation.CriterionType.StringRange )
                        tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionRangeValue], criterion.Name));
                    if (criterion.CommentType != Domain.Evaluation.CommentType.None)
                        tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionComment], criterion.Name));
                }
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]);

                return BuilderXmlDocument.AddRow(tableData);
            }
            private String EvaluationStatisticRow(List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria, List<dtoCriterionOption> availableOptions,lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoEvaluation evaluation,int rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (evaluation.Deleted == BaseStatusDeleted.None ? (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem) : DefaultXmlStyleElement.RowDeleted);
                String row = "";

                String owner = "";
                if (evaluation.Anonymous)
                    owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                else
                    owner = evaluation.DisplayName;

                row += BuilderXmlDocument.AddData(rowNumber.ToString(), rowstyle.ToString());
                row += BuilderXmlDocument.AddData(owner, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(evaluation.SubmitterType, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(EvaluationStatusTranslations[evaluation.Status], rowstyle.ToString());
                row += BuilderXmlDocument.AddData((decimal)evaluation.SumRating, rowstyle.ToString());
                foreach (lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterionEvaluated vCriterion in evaluation.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).ToList().OrderBy(c => c.Criterion.DisplayOrder).ThenBy(c => c.Criterion.Name).ToList())
                {
                    if (vCriterion.IsValueEmpty)
                    {
                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        if (vCriterion.Criterion.Type == Domain.Evaluation.CriterionType.StringRange) 
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                    }
                    else
                    {
                        switch (vCriterion.Criterion.Type)
                        {
                            case Domain.Evaluation.CriterionType.StringRange:
                                dtoCriterionOption option = availableOptions.Where(o => o.Id == vCriterion.IdOption).FirstOrDefault();
                                if (option == null)
                                {
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                }
                                else
                                {
                                    row += BuilderXmlDocument.AddData(option.Name, rowstyle.ToString());
                                    row += BuilderXmlDocument.AddData(option.Value, rowstyle.ToString());
                                }
                                break;
                            case Domain.Evaluation.CriterionType.Textual:
                                if (string.IsNullOrEmpty(vCriterion.StringValue))
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                else
                                    row += BuilderXmlDocument.AddData(vCriterion.StringValue, rowstyle.ToString());
                                break;
                            case Domain.Evaluation.CriterionType.DecimalRange:
                                row += BuilderXmlDocument.AddData(Math.Round(vCriterion.DecimalValue,2), rowstyle.ToString());
                                break;
                            case Domain.Evaluation.CriterionType.IntegerRange:
                                row += BuilderXmlDocument.AddData(((int)vCriterion.DecimalValue), rowstyle.ToString());
                                break;
                            case Domain.Evaluation.CriterionType.None:
                                row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                break;
                        }
                    }
                    if (vCriterion.Criterion.CommentType != CommentType.None) {
                        if (string.IsNullOrEmpty(vCriterion.Comment))
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        else
                            row += BuilderXmlDocument.AddData(vCriterion.Comment, rowstyle.ToString());
                    }
                }
                row += BuilderXmlDocument.AddData(evaluation.Comment, rowstyle.ToString());

                String rows = BuilderXmlDocument.AddRow(row);
                return rows;
            }
            private Int32 GetColumnNumber(List<lm.Comol.Modules.CallForPapers.Domain.Evaluation.dtoCriterion> criteria) {
                Int32 columNumber = 5;
                columNumber += criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType == Domain.Evaluation.CommentType.None).Count();
                columNumber += (criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType != Domain.Evaluation.CommentType.None).Count()*2);
                columNumber += criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type== Domain.Evaluation.CriterionType.StringRange).Count();
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

                String content = BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle], DefaultXmlStyleElement.Title.ToString(), columNumber));
                content += BuilderXmlDocument.AddEmptyRows(3, columNumber);
                content += EvaluationsTableHeader(counters);
                int rowNumber = 1;

                foreach (dtoEvaluationSummaryItem evaluation in items)
                {
                    content += EvaluationStatisticRow(evaluation,counters, rowNumber);
                    rowNumber++;
                }
                export += BuilderXmlDocument.AddWorkSheet("--", header + content);

                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }

            #region "Committee"
                public String ExportSummaryDisplayStatistics(dtoCall call, EvaluationCommittee committee, List<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values, String anonymousDisplayName, litePerson person)
                {
                    Int32 columNumber = GetColumnNumber(committee.Criteria.Where(c=>c.Deleted== BaseStatusDeleted.None).ToList()) + 5;
                    String export = "";
                    String header = AddStandardEvaluationsInfo(call, person, columNumber);
                    String content = BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(String.Format(EvaluationsTranslations[EvaluationTranslations.CommitteeName],committee.Name), DefaultXmlStyleElement.Title.ToString(), columNumber));                    
                    content += BuilderXmlDocument.AddEmptyRows(3, columNumber);
                    content += BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle], DefaultXmlStyleElement.Title.ToString(), columNumber));
                    content += BuilderXmlDocument.AddEmptyRows(1, columNumber);
                    content += EvaluationsTableHeader(committee);
                    int rowNumber = 1;

                    List<CriterionOption> availableOptions = new List<CriterionOption>();
                    committee.Criteria.Where(c => c.Type == CriterionType.StringRange).ToList().ForEach(c => availableOptions.AddRange(((StringRangeCriterion)c).Options));

                    List<dtoBaseCommitteeMember> members = committee.Members.Select(e => new dtoBaseCommitteeMember(e, anonymousDisplayName)).ToList().OrderBy(e => e.DisplayName).ThenByDescending(e => e.IdMembership).ToList();
                    bool aa = evaluations.Where(a => a.Id == 298).Any();
                    foreach (dtoBaseCommitteeMember member in members)
                    {
                        CommitteeMember cMember = committee.Members.Where(m=>m.Id==member.IdMembership).FirstOrDefault();
                        content += EvaluationsStatisticRows(committee.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).ToList(), cMember, availableOptions, evaluations.Where(e => e.Committee.Id == committee.Id && e.Evaluator.Id == member.IdCallEvaluator).ToList(), values, rowNumber);
                    }
                    export += BuilderXmlDocument.AddWorkSheet(committee.Name, header + content);

                    return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
                }
                private String EvaluationsTableHeader(EvaluationCommittee committee)
                {
                    String result = "";
                    String tableData = "";
                    tableData += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluator]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]);
                    
                    foreach (BaseCriterion criterion in committee.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).OrderBy(c=>c.DisplayOrder).ThenBy(c=>c.Name).ToList())
                    {
                        tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterion], criterion.Name));
                        if (criterion.Type == Domain.Evaluation.CriterionType.StringRange)
                            tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionRangeValue], criterion.Name));
                        if (criterion.CommentType != Domain.Evaluation.CommentType.None)
                            tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionComment], criterion.Name));
                    }

                    tableData += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]);
                    result = BuilderXmlDocument.AddRow(tableData);

                    return result;
                }
                private String EvaluationsStatisticRows(List<BaseCriterion> criteria, CommitteeMember member, List<CriterionOption> availableOptions, List<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values, int rowNumber)
                {
                    String rows = "";

                    foreach (Evaluation evaluation in evaluations.Where(e => e.Deleted == BaseStatusDeleted.None && e.Submission != null).OrderBy(e => e.Submission.Id).ToList())
                    {
                        String row = "";
                        DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem); //: DefaultXmlStyleElement.RowDeleted);
                        String owner = "";
                        if (evaluation.Submission.isAnonymous || evaluation.Submission.Owner == null)
                            owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                        else
                            owner = evaluation.Submission.Owner.SurnameAndName;

                        row += BuilderXmlDocument.AddData((member.Evaluator == null || member.Evaluator.Person == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser] : member.Evaluator.Person.SurnameAndName, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(owner, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(evaluation.Submission.Type.Name, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(EvaluationStatusTranslations[evaluation.Status], rowstyle.ToString());
                        row += BuilderXmlDocument.AddData((decimal)evaluation.SumRating, rowstyle.ToString());
                        row += AddCriteriaValues(availableOptions,criteria, values.Where(v => v.Evaluation.Id == evaluation.Id).ToList(), rowstyle);

                        row += BuilderXmlDocument.AddData(evaluation.Comment, rowstyle.ToString());
                        rows += BuilderXmlDocument.AddRow(row);
                        rowNumber++;
                    }
                    return rows;
                }

                private String AddCriteriaValues(List<CriterionOption> availableOptions,List<BaseCriterion> criteria, List<CriterionEvaluated> evaluated, DefaultXmlStyleElement rowstyle)
                {
                    String row = "";
                    foreach (BaseCriterion criterion in criteria.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name).ToList())
                    {
                        CriterionEvaluated vCriterion = evaluated.Where(c => c.Criterion.Id == criterion.Id).FirstOrDefault();
                        if (vCriterion == null || vCriterion.IsValueEmpty)
                        {
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                            if (criterion.Type == Domain.Evaluation.CriterionType.StringRange)
                                row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        }
                        else
                        {
                            switch (vCriterion.Criterion.Type)
                            {
                                case Domain.Evaluation.CriterionType.StringRange:
                                    CriterionOption option = availableOptions.Where(o => vCriterion.Option != null && o.Id == vCriterion.Option.Id).FirstOrDefault();
                                    if (option == null)
                                    {
                                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                    }
                                    else
                                    {
                                        try
                                        {
                                            row += BuilderXmlDocument.AddData(option.Name, rowstyle.ToString());
                                            row += BuilderXmlDocument.AddData(option.Value, rowstyle.ToString());
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    break;
                                case Domain.Evaluation.CriterionType.Textual:
                                    if (string.IsNullOrEmpty(vCriterion.StringValue))
                                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                    else
                                        row += BuilderXmlDocument.AddData(vCriterion.StringValue, rowstyle.ToString());
                                    break;
                                case Domain.Evaluation.CriterionType.DecimalRange:
                                    row += BuilderXmlDocument.AddData(Math.Round(vCriterion.DecimalValue, 2), rowstyle.ToString());
                                    break;
                                case Domain.Evaluation.CriterionType.IntegerRange:
                                    row += BuilderXmlDocument.AddData(((int)vCriterion.DecimalValue), rowstyle.ToString());
                                    break;
                                case Domain.Evaluation.CriterionType.None:
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                    break;
                            }
                        }
                        if (criterion.CommentType != CommentType.None)
                        {
                            if (vCriterion == null || string.IsNullOrEmpty(vCriterion.Comment))
                                row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                            else
                                row += BuilderXmlDocument.AddData(vCriterion.Comment, rowstyle.ToString());
                        }
                    }
                    return row;
                }
                private String AddCriterionValue(BaseCriterion criterion, List<CriterionOption> availableOptions, List<CriterionEvaluated> evaluated, Boolean hasComment, Boolean hasRange, DefaultXmlStyleElement rowstyle)
                {
                    String row = "";
                    CriterionEvaluated vCriterion = evaluated.Where(c => c.Criterion.Id == criterion.Id).FirstOrDefault();
                    row += BuilderXmlDocument.AddData(criterion.Name, rowstyle.ToString());
               
                    if (vCriterion == null || vCriterion.IsValueEmpty)
                    {
                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        if (criterion.Type == Domain.Evaluation.CriterionType.StringRange)
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        else if (hasRange)
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                    }
                    else
                    {
                        switch (vCriterion.Criterion.Type)
                        {
                            case Domain.Evaluation.CriterionType.StringRange:
                                CriterionOption option = availableOptions.Where(o => vCriterion.Option != null && o.Id == vCriterion.Option.Id).FirstOrDefault();
                                if (option == null)
                                {
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                }
                                else
                                {
                                    try
                                    {
                                        row += BuilderXmlDocument.AddData(option.Name, rowstyle.ToString());
                                        row += BuilderXmlDocument.AddData(option.Value, rowstyle.ToString());
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;
                            case Domain.Evaluation.CriterionType.Textual:
                                if (string.IsNullOrEmpty(vCriterion.StringValue))
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                else
                                    row += BuilderXmlDocument.AddData(vCriterion.StringValue);
                                if (hasRange)
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                break;
                            case Domain.Evaluation.CriterionType.DecimalRange:
                                row += BuilderXmlDocument.AddData(Math.Round(vCriterion.DecimalValue, 2), rowstyle.ToString());
                                if (hasRange)
                                    row += BuilderXmlDocument.AddData(Math.Round(vCriterion.DecimalValue,2), rowstyle.ToString());
                                break;
                            case Domain.Evaluation.CriterionType.IntegerRange:
                                row += BuilderXmlDocument.AddData(((int)vCriterion.DecimalValue), rowstyle.ToString());
                                if (hasRange)
                                    row += BuilderXmlDocument.AddData(((int)vCriterion.DecimalValue), rowstyle.ToString());
                                break;
                            case Domain.Evaluation.CriterionType.None:
                                row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                if (hasRange)
                                    row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                break;
                        }
                    }
                    if (criterion.CommentType != CommentType.None)
                    {
                        if (vCriterion == null || string.IsNullOrEmpty(vCriterion.Comment))
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        else
                            row += BuilderXmlDocument.AddData(vCriterion.Comment);
                    }
                    else if (hasComment)
                        row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());

                    return row;
                }
            #endregion

            #region "Committees"
                public String ExportSummaryDisplayStatistics(dtoCall call, List<dtoCommittee> committees, List<dtoCommitteesSummaryItem> items, litePerson person)
                {
                    Int32 columNumber = items.Select(s => GetColumnNumber(s.Committees)).Max(); ;

                    Dictionary<long, Dictionary<EvaluationStatus, Boolean>> counters = GetCommitteesCounters(committees, items);

                    String export = "";
                    String header = AddStandardEvaluationsInfo(call, person, columNumber);
                    String content = BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle], DefaultXmlStyleElement.Title.ToString(), columNumber));
                    content += BuilderXmlDocument.AddEmptyRows(3, columNumber);
                    content += EvaluationsTableHeader(committees, counters);
                    int rowNumber = 1;

                    foreach (dtoCommitteesSummaryItem item in items)
                    {
                        content += EvaluationStatisticRow(item, counters, rowNumber);
                        rowNumber++;
                    }
                    export += BuilderXmlDocument.AddWorkSheet("--", header + content);

                    return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
                }
                private Int32 GetColumnNumber(List<dtoCommitteeDisplayItem> items)
                {
                    Int32 columNumber = 4;
                    items.ForEach(c => columNumber += GetColumnNumber(c));
                    return columNumber;
                }
                private Int32 GetColumnNumber(dtoCommitteeDisplayItem committee)
                {
                    Int32 columNumber = committee.Evaluations.Select(e => e.Status).Distinct().Count();

                    if (columNumber > 1)
                        columNumber++;
                    columNumber++;
                    return columNumber;
                }
                private Dictionary<long, Dictionary<EvaluationStatus, Boolean>> GetCommitteesCounters(List<dtoCommittee> committees, List<dtoCommitteesSummaryItem> items)
                {
                    Dictionary<long, Dictionary<EvaluationStatus, Boolean>> result = new Dictionary<long, Dictionary<EvaluationStatus, Boolean>>();
                    foreach (dtoCommittee committee in committees)
                    {
                        Dictionary<EvaluationStatus, Boolean> cInfo = new Dictionary<EvaluationStatus, Boolean>();
                        cInfo[EvaluationStatus.Evaluated] = items.Where(i =>
                                        i.Committees.Where(c => c.IdCommittee == committee.Id && c.Evaluations.Where(e => e.Status == EvaluationStatus.Evaluated && e.Evaluated).Any()).Any()).Any();
                        cInfo[EvaluationStatus.Evaluating] = items.Where(i =>
                                        i.Committees.Where(c => c.IdCommittee == committee.Id && c.Evaluations.Where(e => e.Status == EvaluationStatus.Evaluating).Any()).Any()).Any();
                        cInfo[EvaluationStatus.EvaluatorReplacement] = items.Where(i =>
                                        i.Committees.Where(c => c.IdCommittee == committee.Id && c.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).Any()).Any();
                        cInfo[EvaluationStatus.Invalidated] = items.Where(i =>
                                        i.Committees.Where(c => c.IdCommittee == committee.Id && c.Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).Any()).Any();
                        cInfo[EvaluationStatus.None] = items.Where(i =>
                                        i.Committees.Where(c => c.IdCommittee == committee.Id && c.Evaluations.Where(e => e.Status == EvaluationStatus.None).Any()).Any()).Any();
                        //{{EvaluationStatus.Evaluated, false}, {EvaluationStatus.Evaluating,false}, {EvaluationStatus.EvaluatorReplacement,false}, {EvaluationStatus.Invalidated,false}, {EvaluationStatus.None,false}};
                        //List<EvaluationStatus> availableStatus = new List<EvaluationStatus>();

                        //IEnumerable<dtoEvaluationDisplayItem> tt = items.SelectMany(i=> i.Committees.Where(c=>c.IdCommittee== committee.Id).Select(c=> c.Evaluations.Select(e=> e.Status).Distinct().ToList())).Select(ed=> ed.Distinct());

                        //var p = items.SelectMany(i=> i.Committees.Where(c=>c.IdCommittee== committee.Id),
                        //    (i,c) =>  c.Evaluations.Select(e=> e.Status).Distinct().ToList());
                        //availableStatus.AddRange(p.Select(c=> c..ToList());

                        result.Add(committee.Id, cInfo);
                    }
                    return result;
                }
                private String EvaluationsTableHeader(List<dtoCommittee> committees, Dictionary<long, Dictionary<EvaluationStatus, Boolean>> counters)
                {
                    String result = "";
                    String tableData = EvaluationsTableHeader();
                    Int32 columNumber = 0;
                    String rowColumn = "";
                    (from i in Enumerable.Range(1, 4) select i).ToList().ForEach(i => rowColumn += AddTableCellSubTitle(""));

                    foreach (dtoCommittee committee in committees)
                    {
                        // Committee vote
                        columNumber = 1;
                        rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommitteeSum]);
                        columNumber += counters[committee.Id].Where(c => c.Value).Count();
                        //if (columNumber > 2)
                        //    columNumber++;
                        tableData += AddTableCellTitle(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommitteeName], committee.Name), columNumber);

                        if (counters[committee.Id].Where(c => c.Value).Count() > 1)
                            rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsCount]);
                        if (counters[committee.Id][EvaluationStatus.Evaluated])
                            rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluated]);
                        if (counters[committee.Id][EvaluationStatus.Evaluating])
                            rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluating]);
                        if (counters[committee.Id][EvaluationStatus.None])
                            rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted]);
                        if (counters[committee.Id][EvaluationStatus.EvaluatorReplacement])
                            rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluatorReplacement]);
                        if (counters[committee.Id][EvaluationStatus.Invalidated])
                            rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsInvalidated]);
                    }
                    result = BuilderXmlDocument.AddRow(tableData);
                    result += BuilderXmlDocument.AddRow(rowColumn);

                    return result;
                }
                private String EvaluationStatisticRow(dtoCommitteesSummaryItem item, Dictionary<long, Dictionary<EvaluationStatus, Boolean>> counters, int rowNumber)
                {
                    DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem); //: DefaultXmlStyleElement.RowDeleted);
                    String row = "";
                    String owner = "";
                    if (item.Anonymous)
                        owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                    else
                        owner = item.DisplayName;

                    row += BuilderXmlDocument.AddData(item.Position.ToString(), rowstyle.ToString());
                    row += BuilderXmlDocument.AddData(owner, rowstyle.ToString());
                    row += BuilderXmlDocument.AddData(item.SubmitterType, rowstyle.ToString());
                    row += BuilderXmlDocument.AddData(item.SumRating, rowstyle.ToString());


                    foreach (dtoCommitteeDisplayItem committee in item.Committees)
                    {
                        // Committee vote
                        row += BuilderXmlDocument.AddData(committee.SumRating, rowstyle.ToString());

                        if (counters[committee.IdCommittee].Where(c => c.Value).Count() > 1)
                            row += BuilderXmlDocument.AddData(committee.Evaluations.Count, rowstyle.ToString());

                        counters[committee.IdCommittee].Where(c => c.Value).ToList().ForEach(c => row += BuilderXmlDocument.AddData(committee.GetEvaluationsCount(c.Key), rowstyle.ToString()));
                        //if (counters[committee.Id][EvaluationStatus.Evaluated])
                        //    ;
                        //if (counters[committee.Id][EvaluationStatus.Evaluating])
                        //    rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluating]);
                        //if (counters[committee.Id][EvaluationStatus.None])
                        //    rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted]);
                        //if (counters[committee.Id][EvaluationStatus.EvaluatorReplacement])
                        //    rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluatorReplacement]);
                        //if (counters[committee.Id][EvaluationStatus.Invalidated])
                        //    rowColumn += AddTableCellSubTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsInvalidated]);
                    }


                    String rows = BuilderXmlDocument.AddRow(row);
                    return rows;
                }
            #endregion

            #region "Full"
                public String ExportFullSummaryStatistics(dtoCall call, List<EvaluationCommittee> committees, IEnumerable<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values, String anonymousDisplayName, litePerson person,Boolean oneColumnForCriteria) 
                {
                    Boolean hasComment = oneColumnForCriteria && committees.Where(cm => cm.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType != Domain.Evaluation.CommentType.None).Any()).Any();
                    Boolean hasRange = oneColumnForCriteria && committees.Where(cm => cm.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == CriterionType.StringRange).Any()).Any();

                    int criteriaColumns = (oneColumnForCriteria) ? committees.Select(c => GetColumnNumber(c.Criteria, oneColumnForCriteria)).Max() : committees.Select(c => GetColumnNumber(c.Criteria, oneColumnForCriteria)).Sum();
                    //              committee +  evaluator name +  Submitter + Type  + evaluation statu + criteria number + comment
                    Int32 columNumber = 5 + criteriaColumns + 1;

                    String export = "";
                    String header = AddStandardEvaluationsInfo(call, person, columNumber);
                    String content = BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(EvaluationsTranslations[EvaluationTranslations.EvaluationsSummaryTitle], DefaultXmlStyleElement.Title.ToString(), columNumber));
                    content += BuilderXmlDocument.AddEmptyRows(3, columNumber);
                    content += EvaluationsTableHeader(committees, oneColumnForCriteria);

                    int rowNumber = 1;
                    int beforeEmptyColumns = 0;
                    foreach (EvaluationCommittee committee in committees.OrderBy(c=>c.DisplayOrder).ThenBy(c=>c.Name).ToList()) {
                        List<CriterionOption> availableOptions = new List<CriterionOption>();
                        foreach (BaseCriterion criterion in committee.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == CriterionType.StringRange).ToList())
                        {
                            availableOptions.AddRange(((StringRangeCriterion)criterion).Options);
                        }

                        if (oneColumnForCriteria)
                        {
                            foreach (CommitteeMember member in committee.Members.Where(m => m.Evaluator != null).ToList())
                            {
                                content += EvaluationsStatisticRows(committee, member, availableOptions, evaluations.Where(e => e.Committee.Id == committee.Id && e.Evaluator.Id == member.Evaluator.Id).ToList(), values, hasComment, hasRange, rowNumber);
                            }
                        }
                        else
                        {
                            int cColumns = GetColumnNumber(committee.Criteria);
                            int afterEmptyColumns = criteriaColumns - beforeEmptyColumns - cColumns;

                            foreach (CommitteeMember member in committee.Members.Where(m => m.Evaluator != null).ToList())
                            {
                                content += EvaluationsStatisticRows(committee, member, availableOptions, evaluations.Where(e => e.Committee.Id == committee.Id && e.Evaluator.Id == member.Evaluator.Id).ToList(), values, beforeEmptyColumns, afterEmptyColumns, rowNumber);
                            }
                            beforeEmptyColumns += cColumns;
                        }
                    }
                    export += BuilderXmlDocument.AddWorkSheet("--", header + content);
                    return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
                }

                private Int32 GetColumnNumber(IList<BaseCriterion> criteria, Boolean oneColumnForCriteria)
                {
                    Int32 columNumber = 0;
                    if (oneColumnForCriteria)
                    {
                        columNumber = 1;
                        columNumber += (criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType != Domain.Evaluation.CommentType.None).Any()) ? 1 : 0;
                        columNumber += (criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == Domain.Evaluation.CriterionType.StringRange).Any()) ? 1 : 0;
                    }
                    else
                        columNumber = GetColumnNumber(criteria);
                    return columNumber;
                }
                private Int32 GetColumnNumber(IList<BaseCriterion> criteria)
                {
                    Int32 columNumber = 0;
                    columNumber += criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType == Domain.Evaluation.CommentType.None).Count();
                    columNumber += (criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType != Domain.Evaluation.CommentType.None).Count() * 2);
                    columNumber += criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == Domain.Evaluation.CriterionType.StringRange).Count();
                    return columNumber;
                }
                private String EvaluationsTableHeader(List<EvaluationCommittee> committees, Boolean oneColumnForCriteria)
                {
                    String result = "";
                    String tableData = AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCommittee]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluator]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]);
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationStatus]);

                    if (oneColumnForCriteria)
                    {
                        tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterion]);
                        tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionUserValue]);
                        if (committees.Where(cm => cm.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.Type == CriterionType.StringRange).Any()).Any())
                            tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionRangeValue]);
                        if (committees.Where(cm => cm.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None && c.CommentType != Domain.Evaluation.CommentType.None).Any()).Any())
                            tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleGenericCriterionComment]);
                    }
                    else
                    {
                        foreach (EvaluationCommittee committee in committees)
                        {
                            foreach (BaseCriterion criterion in committee.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).ToList())
                            {
                                tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterion], criterion.Name));
                                if (criterion.Type == Domain.Evaluation.CriterionType.StringRange)
                                    tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionRangeValue], criterion.Name));
                                if (criterion.CommentType != Domain.Evaluation.CommentType.None)
                                    tableData += AddTableCellTitle(String.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleCriterionComment], criterion.Name));
                            }
                        }
                    }
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationComment]);
                    result = BuilderXmlDocument.AddRow(tableData);
                    return result;
                }
                private String EvaluationsStatisticRows(EvaluationCommittee committee, CommitteeMember member, List<CriterionOption> availableOptions, List<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values, int beforeEmptyColumns, int afterEmptyColumns, int rowNumber)
                {
                    String rows = "";

                    foreach (Evaluation evaluation in evaluations.Where(e=> e.Deleted== BaseStatusDeleted.None && e.Submission !=null).OrderBy(e=> e.Submission.Id).ToList())
                    {
                        String row = "";
                        DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem); //: DefaultXmlStyleElement.RowDeleted);
                        String owner = "";
                        if (evaluation.Submission.isAnonymous || evaluation.Submission.Owner == null)
                            owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                        else
                            owner = evaluation.Submission.Owner.SurnameAndName;

                        row += BuilderXmlDocument.AddData(committee.Name, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData((member.Evaluator == null || member.Evaluator.Person == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser] : member.Evaluator.Person.SurnameAndName, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(owner, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(evaluation.Submission.Type.Name, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(EvaluationStatusTranslations[evaluation.Status], rowstyle.ToString());

                        if (beforeEmptyColumns>0)
                            (from i in Enumerable.Range(1, beforeEmptyColumns) select i).ToList().ForEach(i =>row += BuilderXmlDocument.AddData("", rowstyle.ToString()));

                        row += AddCriteriaValues(availableOptions, committee.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).ToList(), values.Where(v => v.Evaluation.Id == evaluation.Id).ToList(), rowstyle);
                     
                        if (afterEmptyColumns > 0)
                            (from i in Enumerable.Range(1, afterEmptyColumns) select i).ToList().ForEach(i => row += BuilderXmlDocument.AddData("", rowstyle.ToString()));
                        row += BuilderXmlDocument.AddData(evaluation.Comment, rowstyle.ToString());
                        rows += BuilderXmlDocument.AddRow(row);
                        rowNumber++;
                    }
                    return rows;
                }
                private String EvaluationsStatisticRows(EvaluationCommittee committee, CommitteeMember member, List<CriterionOption> availableOptions, List<Evaluation> evaluations, IEnumerable<CriterionEvaluated> values, Boolean hasComment, Boolean hasRange, int rowNumber)
                {
                    String rows = "";

                    foreach (Evaluation evaluation in evaluations.Where(e => e.Deleted == BaseStatusDeleted.None && e.Submission != null).OrderBy(e => e.Submission.Id).ToList())
                    {
                        String owner = "";
                        if (evaluation.Submission.isAnonymous || evaluation.Submission.Owner == null)
                            owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                        else
                            owner = evaluation.Submission.Owner.SurnameAndName;


                        foreach (BaseCriterion criterion in committee.Criteria.Where(c => c.Deleted == BaseStatusDeleted.None).ToList()) {
                            String row = "";
                            DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem); //: DefaultXmlStyleElement.RowDeleted);
                            row += BuilderXmlDocument.AddData(committee.Name, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData((member.Evaluator == null || member.Evaluator.Person == null) ? EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser] : member.Evaluator.Person.SurnameAndName, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData(owner, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData(evaluation.Submission.Type.Name, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData(EvaluationStatusTranslations[evaluation.Status], rowstyle.ToString());

                            row += AddCriterionValue(criterion, availableOptions, values.Where(v => v.Evaluation.Id == evaluation.Id).ToList(), hasComment, hasRange, rowstyle);
                            row += BuilderXmlDocument.AddData(evaluation.Comment, rowstyle.ToString());
                            rows += BuilderXmlDocument.AddRow(row);
                            rowNumber++;
                        }
                    }
                    return rows;
                }
            #endregion
            
            private Dictionary<EvaluationStatus, Boolean> GetCounters(List<dtoEvaluationSummaryItem> items)
            {
                Dictionary<EvaluationStatus, Boolean> counters = new Dictionary<EvaluationStatus, Boolean>();
                counters[EvaluationStatus.Evaluated] = items.Where(i=> i.Evaluations.Where(e=> e.Evaluated && e.Status== EvaluationStatus.Evaluated).Any()).Any();
                counters[EvaluationStatus.Evaluating] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Evaluating).Any()).Any();
                counters[EvaluationStatus.EvaluatorReplacement] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).Any();
                counters[EvaluationStatus.Invalidated] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).Any();
                counters[EvaluationStatus.None] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.None).Any()).Any();

                return counters;
            }
           
            
            //private Dictionary<long, Dictionary<EvaluationStatus, Boolean>> GetCommitteeCounters(List<dtoCommitteesSummaryItem> items)
            //{
            //    Dictionary<long, Dictionary<EvaluationStatus, Boolean>> counters = new Dictionary<long, Dictionary<EvaluationStatus, Boolean>>();
            //    items.ForEach(i=> i.Committees.Where(c=>c
            //    counters[EvaluationStatus.Evaluated] = items.Where(i => i.Evaluations.Where(e => e.Evaluated && e.Status == EvaluationStatus.Evaluated).Any()).Any();
            //    counters[EvaluationStatus.Evaluating] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Evaluating).Any()).Any();
            //    counters[EvaluationStatus.EvaluatorReplacement] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.EvaluatorReplacement).Any()).Any();
            //    counters[EvaluationStatus.Invalidated] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.Invalidated).Any()).Any();
            //    counters[EvaluationStatus.None] = items.Where(i => i.Evaluations.Where(e => e.Status == EvaluationStatus.None).Any()).Any();

            //    return counters;
            //}
            private String EvaluationsTableHeader()
            {
                String tableData = AddTableCellTitle("#");
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmissionOwner]);
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSubmitterType]);
                tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleSum]);
                return tableData;
            }
            private String EvaluationsTableHeader(Dictionary<EvaluationStatus, Boolean> counters)
            {
                String tableData = EvaluationsTableHeader();
                if (counters.Where(c => c.Value).Count() > 1)
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsCount]);
                if (counters[EvaluationStatus.Evaluated])
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluated]);
                if (counters[EvaluationStatus.Evaluating])
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluating]);
                if (counters[EvaluationStatus.None])
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsNotStarted]);
                if (counters[EvaluationStatus.EvaluatorReplacement])
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsEvaluatorReplacement]);
                if (counters[EvaluationStatus.Invalidated])
                    tableData += AddTableCellTitle(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CellTitleEvaluationsInvalidated]);
                return BuilderXmlDocument.AddRow(tableData);
            }

            private String EvaluationStatisticRow(dtoEvaluationSummaryItem evaluation,Dictionary<EvaluationStatus, Boolean> counters, int rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem); //: DefaultXmlStyleElement.RowDeleted);
                String row = "";

                String owner = "";
                if (evaluation.Anonymous)
                    owner = EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.AnonymousUser];
                else
                    owner = evaluation.DisplayName;

                row += BuilderXmlDocument.AddData(evaluation.Position.ToString(), rowstyle.ToString());
                row += BuilderXmlDocument.AddData(owner, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(evaluation.SubmitterType, rowstyle.ToString());
                row += BuilderXmlDocument.AddData((decimal)evaluation.SumRating, rowstyle.ToString());

                if (counters.Where(c => c.Value).Count() > 1)
                    row += BuilderXmlDocument.AddData(evaluation.Evaluations.Count(), rowstyle.ToString());
                if (counters[EvaluationStatus.Evaluated])
                    row += BuilderXmlDocument.AddData(evaluation.Counters[EvaluationStatus.Evaluated], rowstyle.ToString());
                if (counters[EvaluationStatus.Evaluating])
                    row += BuilderXmlDocument.AddData(evaluation.Counters[EvaluationStatus.Evaluating], rowstyle.ToString());
                if (counters[EvaluationStatus.None])
                    row += BuilderXmlDocument.AddData(evaluation.Counters[EvaluationStatus.None], rowstyle.ToString());
                if (counters[EvaluationStatus.EvaluatorReplacement])
                    row += BuilderXmlDocument.AddData(evaluation.Counters[EvaluationStatus.EvaluatorReplacement], rowstyle.ToString());
                if (counters[EvaluationStatus.Invalidated])
                    row += BuilderXmlDocument.AddData(evaluation.Counters[EvaluationStatus.Invalidated], rowstyle.ToString());
            if (counters[EvaluationStatus.Confirmed])
                row += BuilderXmlDocument.AddData(evaluation.Counters[EvaluationStatus.Confirmed], rowstyle.ToString());
            String rows = BuilderXmlDocument.AddRow(row);
                return rows;
            }
            public String GetErrorDocument(dtoCall call, litePerson person)
            {
                String header = AddStandardErrorInfo(call, person);
                String export = BuilderXmlDocument.AddWorkSheet("--", header);

                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private String AddStandardErrorInfo(dtoCall call, litePerson person)
            {
                String infoRow = "";
                if (String.IsNullOrEmpty(call.Edition))
                    infoRow += BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitle], call.Name), DefaultXmlStyleElement.Title.ToString(), 4);
                else
                    infoRow += BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.CallTitleAndEdition], call.Name, call.Edition), DefaultXmlStyleElement.Title.ToString(), 4);

                infoRow = BuilderXmlDocument.AddRow(infoRow);

                DateTime printTime = DateTime.Now;
                if (person != null)
                    infoRow += BuilderXmlDocument.AddRow(BuilderXmlDocument.AddData(string.Format(EvaluationsTranslations[Domain.Evaluation.EvaluationTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), DefaultXmlStyleElement.PrintInfo.ToString(), 4));
                infoRow += BuilderXmlDocument.AddEmptyRows(3, 4);
                return infoRow;
            }
        #endregion

            #region "Styles"
            private String AddTableCellTitle(String data){
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.HeaderTable.ToString());    
            }
            private String AddTableCellTitle(String data, Int32 colspan)
            {
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.HeaderTable.ToString(), colspan);
            }
            private String AddTableCellSubTitle(String data)
            {
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.SubHeaderTable.ToString());
            }
            private String AddTableCellSubTitle(String data, Int32 colspan)
            {
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.SubHeaderTable.ToString(), colspan);
            }
            //public enum EvaluationsStyle
            //{
            //    HeaderTable = 1,
            //    RowItem = 2,
            //    RowAlternatingItem = 3,
            //    RowDeleted = 4,
            //    ItemEmpty = 5,
            //    ItemEmptyAlternate = 6,
            //    ItemYellow = 7,
            //    ItemRed = 8,
            //    CallForPaperTitle = 9,
            //    PrintInfo = 10
            //}
        #endregion

        public enum ExportContentType { 
            SubmissionList = 1,
            Evaluations = 2,
            RequestsData = 3
        }

        #region "RequestsData"
            public String ExportSubmissionsData(String name, String edition,List<dtoCallSection<dtoCallField>> requestSchema, List<dtoSubmissionDataDisplay> submissions, litePerson person, SubmissionFilterStatus filter)
            {
                Int32 columNumber = 4 + GetColumnNumberFromSchema(requestSchema);
                String export = "";
                String header = AddSubmissionsListInfo(CallForPaperType.RequestForMembership, name, edition, person, columNumber);
                header += RequestsListTableHeader(filter, requestSchema);
                int rowNumber = 1;
                foreach (dtoSubmissionDataDisplay sub in submissions)
                {
                    header += AddSubmissionDataRow(requestSchema,sub, filter, rowNumber);
                    rowNumber++;
                }
                export += BuilderXmlDocument.AddWorkSheet("--", header);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private Int32 GetColumnNumberFromSchema(List<dtoCallSection<dtoCallField>> requestSchema)
            {
                Int32 column = 0;
                requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).ToList().ForEach(s => column += s.Fields.Where(f => f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.None && f.Type != FieldType.Note && f.Type != FieldType.RadioButtonList && f.Type != FieldType.DropDownList && f.Type != FieldType.CheckboxList).Count());
                requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).ToList().ForEach(s => column += s.Fields.Where(f => f.Deleted == BaseStatusDeleted.None && (f.Type == FieldType.RadioButtonList || f.Type == FieldType.DropDownList || f.Type == FieldType.CheckboxList)).Count() *2);
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
                String tableData = BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellSubmitter], DefaultXmlStyleElement.HeaderTable.ToString());
                tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellSubmissionType], DefaultXmlStyleElement.HeaderTable.ToString())
                                + BuilderXmlDocument.AddData(CommonTranslations[dateCell], DefaultXmlStyleElement.HeaderTable.ToString());
                tableData += BuilderXmlDocument.AddData(CommonTranslations[SubmissionsListTranslations.CellStatus], DefaultXmlStyleElement.HeaderTable.ToString());

                foreach(var section in requestSchema.Where(s => s.Deleted == BaseStatusDeleted.None).OrderBy(s=>s.DisplayOrder).ToList()){
                    foreach(var field in section.Fields.Where(f =>
                        f.Deleted == BaseStatusDeleted.None && f.Type != FieldType.None && f.Type != FieldType.Note).OrderBy(f => f.DisplayOrder).ToList())
                    {
                            if (field.Type == FieldType.DropDownList || field.Type == FieldType.RadioButtonList || field.Type == FieldType.CheckboxList)
                            {
                                tableData += BuilderXmlDocument.AddData(field.Name, DefaultXmlStyleElement.HeaderTable.ToString());
                                tableData += BuilderXmlDocument.AddData("* - " + field.Name, DefaultXmlStyleElement.HeaderTable.ToString());
                            }
                            else
                                tableData += BuilderXmlDocument.AddData(field.Name, DefaultXmlStyleElement.HeaderTable.ToString());
                    }
                }
                return BuilderXmlDocument.AddRow(tableData);
            }

            private String AddSubmissionDataRow(List<dtoCallSection<dtoCallField>> requestSchema,dtoSubmissionDataDisplay sub, SubmissionFilterStatus filter, int rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (sub.Deleted == BaseStatusDeleted.None ? (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem) : DefaultXmlStyleElement.RowDeleted);
                String row = "";

                String owner = "";
                if (sub.Owner == null && sub.IsAnonymous == false)
                    owner = CommonTranslations[SubmissionsListTranslations.DeletedUser];
                else if (sub.IsAnonymous || (sub.Owner != null && sub.Owner.TypeID == (int)UserTypeStandard.Guest))
                    owner = CommonTranslations[SubmissionsListTranslations.AnonymousOwner];
                else
                    owner = sub.Owner.SurnameAndName;

                if (!sub.SubmittedOn.HasValue || sub.SubmittedBy == null)
                    row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.CreatedByInfo], owner), rowstyle.ToString());
                else if (sub.Owner == sub.SubmittedBy)
                    row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedByInfo], owner), rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData(String.Format(CommonTranslations[SubmissionsListTranslations.SubmittedForInfo], owner, sub.SubmittedBy.SurnameAndName));

                row += BuilderXmlDocument.AddData(sub.Type.Name, rowstyle.ToString());
                if (filter == SubmissionFilterStatus.OnlySubmitted && sub.SubmittedOn.HasValue)
                    row += BuilderXmlDocument.AddData(sub.SubmittedOn.Value.ToShortDateString() + ' ' + sub.SubmittedOn.Value.ToShortTimeString(), rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData(sub.ModifiedOn.Value.ToShortDateString() + ' ' + sub.ModifiedOn.Value.ToShortTimeString(), rowstyle.ToString());
                row += BuilderXmlDocument.AddData(StatusTranslations[sub.Status], rowstyle.ToString());

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
                                    switch (field.DisclaimerType)
                                    {
                                        case DisclaimerType.None:
                                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                                            break;
                                        case DisclaimerType.Standard:
                                            row += BuilderXmlDocument.AddData((item.Value.Text == "True" ? itemAccept : itemRefused));
                                            break;
                                        case DisclaimerType.CustomDisplayOnly:
                                            row += BuilderXmlDocument.AddData(itemRead);
                                            break;
                                        default:
                                            MultipleChoiceFieldToCell(ref row, item, this.CommonTranslations, field.HasFreeOption, rowstyle.ToString());
                                            break;
                                    }
                                    break;
                                case FieldType.CheckboxList:
                                case FieldType.RadioButtonList:
                                case FieldType.DropDownList:
                                    MultipleChoiceFieldToCell(ref row, item,this.CommonTranslations, field.HasFreeOption, rowstyle.ToString());
                                    break;
                                case FieldType.FileInput:
                                    row += BuilderXmlDocument.AddData((item.Value == null || item.Value.IdLink == 0) ? CommonTranslations[SubmissionsListTranslations.FileNotSubmitted] : CommonTranslations[SubmissionsListTranslations.FileSubmitted], rowstyle.ToString());
                                    break;
                                case FieldType.Note:
                                    break;
                                default:
                                    row += BuilderXmlDocument.AddData(item.Value.Text, rowstyle.ToString());
                                    break;
                            }
                        }
                        else {
                            row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                            if (field.Type== FieldType.CheckboxList || field.Type== FieldType.DropDownList || field.Type== FieldType.RadioButtonList || (field.Type== FieldType.Disclaimer && (field.DisclaimerType == DisclaimerType.CustomSingleOption || field.DisclaimerType == DisclaimerType.CustomMultiOptions)))
                                row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                            if (field.HasFreeOption)
                                row += BuilderXmlDocument.AddEmptyData(rowstyle.ToString());
                        }
                    }
                }
                String rows = BuilderXmlDocument.AddRow(row);

                return rows;
            }
            private void MultipleChoiceFieldToCell(ref String row, dtoSubmissionValueField item,Dictionary<SubmissionsListTranslations, string> translations, Boolean hasFreeOption, String rowstyle)
            {
                List<String> mValue = (item.Value == null || String.IsNullOrEmpty(item.Value.Text)) ? new List<String>() : item.Value.Text.Split('|').ToList();
                String itemValues = "";
                String itemNames = "";

                itemNames = String.Join(", ",item.Field.Options.Where(o => mValue.Contains(o.Id.ToString())).OrderByDescending(o => o.Name).Select(o=>o.Name).ToArray());
                itemValues = item.Value.Text;

                row += BuilderXmlDocument.AddData(itemNames, rowstyle);
                row += BuilderXmlDocument.AddData(itemValues, rowstyle);
                if (hasFreeOption)
                    row += BuilderXmlDocument.AddData(item.Value.FreeText, rowstyle);
            }
        #endregion 
        //#region "Excel"
      
        //    #region "Evaluations"
        //        public static String CreateErrorExcelDocument()
        //        {
        //            String export = "";
        //            return export;
        //        }
        //        public static String CreateErrorExcelDocument(Dictionary<EvaluationTranslations, string> translations)
        //            {
        //                String export = "";
        //                return export;
        //            }
        //        public static String ExportEvaluations(String Name, String Edition, EvaluationType evaluationType, List<dtoEvaluationStatistic> evaluations, litePerson person, Dictionary<EvaluationTranslations, string> translations)
        //        {
        //            String export = "";
        //            String header = AddEvaluationInfo(Name, Edition, translations, person);
        //            header += EvaluationsTableHeader(evaluationType, translations);
        //            int rowNumber = 1;
        //            foreach (dtoEvaluationStatistic statistic in evaluations) {
        //                header += AddStatisticRow(evaluationType, statistic, translations, rowNumber);
        //                rowNumber++;
        //            }
        //            export += XMLDocument.AddWorkSheet(translations[EvaluationTranslations.WorkSheetName], header);
        //            return XMLDocument.AddMain(export, DefineEvaluationsStyle());
        //        }
        //        public static String ExportAdvancedEvaluations(String Name, String Edition, EvaluationType evaluationType, List<dtoEvaluationAdvancedStatistic> evaluations, List<dtoEvaluator> evaluators, litePerson person, Dictionary<EvaluationTranslations, string> translations)
        //        {

        //            String export = "" ;
        //            String header = AddEvaluationInfo(Name, Edition, translations, person);
        //            header += EvaluationsTableHeader(evaluationType, translations, evaluators);

        //            int rowNumber = 1;
        //            foreach (dtoEvaluationAdvancedStatistic statistic in evaluations)
        //            {
        //                header += AddStatisticRow(evaluationType,statistic, translations, rowNumber);
        //                rowNumber++;
        //            }
        //            export += XMLDocument.AddWorkSheet(translations[EvaluationTranslations.WorkSheetName], header);
        //            export += XMLDocument.AddWorkSheet(translations[EvaluationTranslations.WorkSheetPivot], ExportEvaluationsToPivot(evaluations,evaluators, translations));
        //            return XMLDocument.AddMain(export, DefineEvaluationsStyle());
        //        }

        //        private static String ExportEvaluationsToPivot(List<dtoEvaluationAdvancedStatistic> statistics, List<dtoEvaluator> evaluators, Dictionary<EvaluationTranslations, string> translations)
        //        {
        //            String pivotSheet = "";
        //            pivotSheet += EvaluationsPivotTableHeader(translations);
        //            int rowNumber = 1;
        //            foreach (dtoEvaluationAdvancedStatistic statistic in statistics)
        //            {
        //                foreach (var item in (from p in statistic.PivotResults
        //                                                     join e in evaluators on p.IdEvaluator equals e.Id
        //                                                     orderby e.DisplayName, p.CriterionName
        //                                                     select new { PivotItem = p, DisplayName = e.DisplayName }).ToList())
        //                {
        //                    pivotSheet += AddPivotRow((dtoEvaluationStatistic)statistic, item.PivotItem, item.DisplayName, translations, rowNumber);
        //                    rowNumber++;
        //                }
        //            }
        //            return pivotSheet;
        //        }
        //        private static String AddPivotRow(dtoEvaluationStatistic statistic, dtoEvaluationPivot pivotItem, String evaluatorName, Dictionary<EvaluationTranslations, string> translations, int rowNumber)
        //        {
        //            EvaluationsStyle rowstyle = (statistic.Deleted == BaseStatusDeleted.None ? (rowNumber % 2 == 0 ? EvaluationsStyle.RowAlternatingItem : EvaluationsStyle.RowItem) : EvaluationsStyle.RowDeleted);
        //            EvaluationsStyle valueStyle;
        //            if (statistic.EvaluationComplete)
        //                valueStyle = (rowNumber % 2 == 0 ? EvaluationsStyle.ItemEmptyAlternate : EvaluationsStyle.ItemEmpty);
        //            else if (statistic.EvaluationsCount == 0)
        //                valueStyle = EvaluationsStyle.ItemRed;
        //            else
        //                valueStyle = EvaluationsStyle.ItemYellow;
        //            String row = XMLDocument.AddRow(
        //                        XMLDocument.AddData(statistic.Id, rowstyle.ToString())
        //                        + XMLDocument.AddData(statistic.DisplayName, rowstyle.ToString())
        //                        + XMLDocument.AddData(statistic.SubmissionType, rowstyle.ToString())
        //                        + XMLDocument.AddData(evaluatorName, rowstyle.ToString())
        //                        + XMLDocument.AddData(pivotItem.EvaluationComplete, rowstyle.ToString())
        //                        + XMLDocument.AddData(pivotItem.CriterionName, rowstyle.ToString())
        //                        + XMLDocument.AddData(pivotItem.Value, rowstyle.ToString())
        //                        );
        //            return row;
        //        }

        //        private static String AddEvaluationInfo(String Name, String Edition, Dictionary<EvaluationTranslations, string> translations, litePerson person)
        //        {
        //            String infoRow = XMLDocument.AddData(translations[EvaluationTranslations.CallForPaperTitle]);

        //            if (String.IsNullOrEmpty(Edition))
        //                infoRow += XMLDocument.AddData(string.Format(translations[EvaluationTranslations.CallForPaperName], Name), EvaluationsStyle.CallForPaperTitle.ToString(), 6);
        //            else
        //                infoRow += XMLDocument.AddData(string.Format(translations[EvaluationTranslations.CallForPaperNameAndEdition], Name, Edition), EvaluationsStyle.CallForPaperTitle.ToString(), 6);

        //            infoRow = XMLDocument.AddRow(infoRow);

        //            DateTime printTime = DateTime.Now;
        //            if (person !=null)
        //                infoRow += XMLDocument.AddRow(XMLDocument.AddData(string.Format(translations[EvaluationTranslations.PrintInfo],person.SurnameAndName,printTime.ToShortDateString(),printTime.ToShortTimeString()),EvaluationsStyle.PrintInfo.ToString(),7 ));
        //            else
        //                infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //            infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //            infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //            infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //            return infoRow;
        //        }
        //        private static String AddStatisticRow(EvaluationType evaluationType, dtoEvaluationStatistic statistic, Dictionary<EvaluationTranslations, string> translations, int rowNumber)
        //        {
        //            String row = StatisticRow(evaluationType,statistic, translations, rowNumber);
        //            return XMLDocument.AddRow(row);
        //        }
        //        private static String AddStatisticRow(EvaluationType evaluationType, dtoEvaluationAdvancedStatistic statistic, Dictionary<EvaluationTranslations, string> translations, int rowNumber)
        //        {
        //            String row = StatisticRow(evaluationType, statistic, translations, rowNumber);
 
        //            foreach (dtoEvaluationResult evaluation in statistic.Evaluations){
        //                EvaluationsStyle averageStyle = EvaluationsStyle.RowItem;
        //                if (evaluation.EvaluationComplete || !evaluation.Assigned)
        //                    averageStyle = (rowNumber % 2 ==0 ? EvaluationsStyle.ItemEmptyAlternate : EvaluationsStyle.ItemEmpty) ;
        //                else if (evaluation.Assigned && !evaluation.EvaluationStarted)
        //                    averageStyle = EvaluationsStyle.ItemRed;
        //                else if (evaluation.Assigned && evaluation.EvaluationStarted)
        //                    averageStyle = EvaluationsStyle.ItemYellow;
        //                if (evaluation.Assigned)
        //                    row += XMLDocument.AddData((evaluationType== EvaluationType.Sum) ? evaluation.SumRating: evaluation.AverageRating, averageStyle.ToString());
        //                else
        //                    row += XMLDocument.AddData(translations[EvaluationTranslations.NotAssigned], averageStyle.ToString());
        //            }
                    

                    
        //            return XMLDocument.AddRow(row);
        //        }
        //        private static String StatisticRow(EvaluationType evaluationType, dtoEvaluationStatistic statistic, Dictionary<EvaluationTranslations, string> translations, int rowNumber)
        //        {
        //            EvaluationsStyle rowstyle = (statistic.Deleted== BaseStatusDeleted.None  ? (rowNumber % 2 ==0 ? EvaluationsStyle.RowAlternatingItem : EvaluationsStyle.RowItem) : EvaluationsStyle.RowDeleted );
        //            EvaluationsStyle averageStyle;
        //            if (statistic.EvaluationComplete )
        //                averageStyle = (rowNumber % 2 ==0 ? EvaluationsStyle.ItemEmptyAlternate : EvaluationsStyle.ItemEmpty) ;
        //            else if (statistic.EvaluationsCount==0)
        //                averageStyle = EvaluationsStyle.ItemRed;
        //            else
        //                averageStyle = EvaluationsStyle.ItemYellow;
        //            String row = XMLDocument.AddData(statistic.Position, rowstyle.ToString())
        //                        + XMLDocument.AddData(statistic.DisplayName, rowstyle.ToString())
        //                        + XMLDocument.AddData(statistic.SubmissionType, rowstyle.ToString())
        //                        + XMLDocument.AddData((evaluationType == EvaluationType.Sum) ? statistic.SumRating : statistic.AverageRating, averageStyle.ToString());
        //            return row;
        //        }
        //    #endregion

        //    #region "Header / Footer"
       

        //        private static String StandardEvaluationsTableHeader(EvaluationType evaluationType, Dictionary<EvaluationTranslations, string> translations)
        //        {
        //            return XMLDocument.AddData(translations[EvaluationTranslations.CellPosition], EvaluationsStyle.HeaderTable.ToString())
        //                                + XMLDocument.AddData(translations[EvaluationTranslations.CellSubmitterName], EvaluationsStyle.HeaderTable.ToString())
        //                                + XMLDocument.AddData(translations[EvaluationTranslations.CellType], EvaluationsStyle.HeaderTable.ToString())
        //                                + XMLDocument.AddData(translations[(evaluationType == EvaluationType.Sum) ? EvaluationTranslations.CellSumRating: EvaluationTranslations.CellAverageRating], EvaluationsStyle.HeaderTable.ToString());
        //        }
        //        private static String EvaluationsTableHeader(EvaluationType evaluationType, Dictionary<EvaluationTranslations, string> translations)
        //        {
        //            return XMLDocument.AddRow(StandardEvaluationsTableHeader(evaluationType,translations));
        //        }
        //        private static String EvaluationsTableHeader(EvaluationType evaluationType, Dictionary<EvaluationTranslations, string> translations, List<dtoEvaluator> evaluators)
        //        {
        //            String header = StandardEvaluationsTableHeader(evaluationType,translations);
        //            foreach (dtoEvaluator evaluator in evaluators){
        //                header += XMLDocument.AddData(evaluator.DisplayName, EvaluationsStyle.HeaderTable.ToString());
        //            }
        //            return XMLDocument.AddRow(header);
        //        }
        //        private static String EvaluationsPivotTableHeader(Dictionary<EvaluationTranslations, string> translations)
        //        {
        //            return XMLDocument.AddRow(
        //                XMLDocument.AddData(translations[EvaluationTranslations.CellSubmissionNumber], EvaluationsStyle.HeaderTable.ToString())
        //                              + XMLDocument.AddData(translations[EvaluationTranslations.CellSubmitterName], EvaluationsStyle.HeaderTable.ToString())
        //                              + XMLDocument.AddData(translations[EvaluationTranslations.CellType], EvaluationsStyle.HeaderTable.ToString())
        //                              + XMLDocument.AddData(translations[EvaluationTranslations.CellEvaluator], EvaluationsStyle.HeaderTable.ToString())
        //                              + XMLDocument.AddData(translations[EvaluationTranslations.CellEvaluationComplete], EvaluationsStyle.HeaderTable.ToString())
        //                              + XMLDocument.AddData(translations[EvaluationTranslations.CellCriterion], EvaluationsStyle.HeaderTable.ToString())
        //                              + XMLDocument.AddData(translations[EvaluationTranslations.CellEvaluation], EvaluationsStyle.HeaderTable.ToString())
        //                );
        //        }
        //        public enum EvaluationsStyle { 
        //            HeaderTable = 1,
        //            RowItem = 2,
        //            RowAlternatingItem =3,
        //            RowDeleted = 4,
        //            ItemEmpty = 5,
        //            ItemEmptyAlternate = 6,
        //            ItemYellow = 7,
        //            ItemRed = 8,
        //            CallForPaperTitle = 9,
        //            PrintInfo = 10
        //        }
        //    #endregion
        //#endregion
    }
}
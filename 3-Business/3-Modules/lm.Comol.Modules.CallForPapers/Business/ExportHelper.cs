using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public class ExportHelper //: lm.Comol.Core.DomainModel.Helpers.Export.ExportBaseHelper
    {
        private Dictionary<SubmissionTranslations, string> Translations { get; set; }
        private dtoExportSubmission Settings { get; set; }


        public ExportHelper() {
            Translations = new Dictionary<SubmissionTranslations, string>();
         }
        public ExportHelper(Dictionary<SubmissionTranslations, string> translations) 
        {
            Translations = translations;
        }


        //#region "Excel"
        //    #region "Submissions List"
        //        public static String ExportSubmissionsList(String Name, String Edition, IList<dtoSubmission> submissions,IList<LazySubscription> subscriptionInfo,litePerson person,FilterSubmission filter,  Dictionary<SubmissionListTranslations, string> translations)
        //            {
        //                String export = "";
        //                String header = AddSubmissionsListInfo(Name, Edition, translations, person);
        //                header += SubmissionsListTableHeader(translations,filter);
        //                int rowNumber = 1;
        //                foreach (dtoSubmission submission in submissions)
        //                {
        //                    header += AddSubmissionListRow(submission, (from s in subscriptionInfo where s.IdPerson == submission.PersonId select s).FirstOrDefault(), translations, filter,rowNumber);
        //                    rowNumber++;
        //                }
        //                export += XMLDocument.AddWorkSheet("--", header);
        //                return XMLDocument.AddMain(export, DefineEvaluationsStyle());
        //            }
        //        private static String AddSubmissionListRow(dtoSubmission submission, LazySubscription subscriptionInfo, Dictionary<SubmissionListTranslations, string> translations, FilterSubmission filter, int rowNumber)
        //        {
        //            EvaluationsStyle rowstyle = (submission.Deleted == BaseStatusDeleted.None ? (rowNumber % 2 == 0 ? EvaluationsStyle.RowAlternatingItem : EvaluationsStyle.RowItem) : EvaluationsStyle.RowDeleted);
        //            String row = "";

        //            if (!submission.SubmittedOn.HasValue)
        //                row += XMLDocument.AddData(String.Format(translations[SubmissionListTranslations.CreatedByInfo], submission.Owner.SurnameAndName), rowstyle.ToString());
        //            else if (submission.Owner == submission.SubmittedBy)
        //                row += XMLDocument.AddData(String.Format(translations[SubmissionListTranslations.SubmittedByInfo], submission.Owner.SurnameAndName),  rowstyle.ToString());
        //            else
        //                row +=XMLDocument.AddData(String.Format(translations[SubmissionListTranslations.SubmittedForInfo], submission.Owner.SurnameAndName, submission.SubmittedBy.SurnameAndName));

        //            if (subscriptionInfo!=null){
        //                row += XMLDocument.AddData(submission.Owner.Mail, rowstyle.ToString());
        //                row += XMLDocument.AddData((subscriptionInfo.SubscriptedOn.HasValue ? subscriptionInfo.SubscriptedOn.Value.ToShortDateString() + ' '+ subscriptionInfo.SubscriptedOn.Value.ToShortTimeString() : " ")  , rowstyle.ToString());
        //            }
        //            else{
        //                row += XMLDocument.AddData(" ", rowstyle.ToString());
        //                row += XMLDocument.AddData(" ", rowstyle.ToString());
        //            }

        //            row += XMLDocument.AddData(submission.Type.Name, rowstyle.ToString());
        //            if (filter== FilterSubmission.OnlySubmitted && submission.SubmittedOn.HasValue)
        //                row += XMLDocument.AddData(submission.SubmittedOn.Value.ToShortDateString() + ' ' + submission.SubmittedOn.Value.ToShortTimeString(), rowstyle.ToString());
        //            else
        //                row += XMLDocument.AddData(submission.ModifiedOn.Value.ToShortDateString() + ' ' + submission.ModifiedOn.Value.ToShortTimeString(), rowstyle.ToString());

        //            row += XMLDocument.AddData(translations[(SubmissionListTranslations)submission.Status], rowstyle.ToString());

        //            return XMLDocument.AddRow(row);
        //        }
        //        private static String AddSubmissionsListInfo(String Name, String Edition, Dictionary<SubmissionListTranslations, string> translations, litePerson person)
        //            {
        //                String infoRow = XMLDocument.AddData(translations[SubmissionListTranslations.CallForPaperTitle]);

        //                if (String.IsNullOrEmpty(Edition))
        //                    infoRow += XMLDocument.AddData(string.Format(translations[SubmissionListTranslations.CallForPaperName], Name), EvaluationsStyle.CallForPaperTitle.ToString(), 6);
        //                else
        //                    infoRow += XMLDocument.AddData(string.Format(translations[SubmissionListTranslations.CallForPaperNameAndEdition], Name, Edition), EvaluationsStyle.CallForPaperTitle.ToString(), 6);

        //                infoRow = XMLDocument.AddRow(infoRow);

        //                DateTime printTime = DateTime.Now;
        //                if (person != null)
        //                    infoRow += XMLDocument.AddRow(XMLDocument.AddData(string.Format(translations[SubmissionListTranslations.PrintInfo], person.SurnameAndName, printTime.ToShortDateString(), printTime.ToShortTimeString()), EvaluationsStyle.PrintInfo.ToString(), 7));
        //                else
        //                    infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //                infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //                infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //                infoRow += XMLDocument.AddRow(XMLDocument.AddData("  "));
        //                return infoRow;
        //            }
        //        private static String SubmissionsListTableHeader(Dictionary<SubmissionListTranslations, string> translations, FilterSubmission filter)
        //            {
        //                return XMLDocument.AddRow(XMLDocument.AddData(translations[SubmissionListTranslations.CellSubmitter], EvaluationsStyle.HeaderTable.ToString())
        //                                    + XMLDocument.AddData(translations[SubmissionListTranslations.CellSubmitterMail], EvaluationsStyle.HeaderTable.ToString())
        //                                    + XMLDocument.AddData(translations[SubmissionListTranslations.CellSubmitterSubscriptionOn], EvaluationsStyle.HeaderTable.ToString())
        //                                     + XMLDocument.AddData(translations[SubmissionListTranslations.CellSubmissionType], EvaluationsStyle.HeaderTable.ToString())
        //                                     + XMLDocument.AddData(translations[(filter== FilterSubmission.WaitingSubmission) ? SubmissionListTranslations.CellLastUpdateOn : (filter== FilterSubmission.VirtualDeletedSubmission) ? SubmissionListTranslations.CellDeletedOn : SubmissionListTranslations.CellSubmittedOn ], EvaluationsStyle.HeaderTable.ToString())
        //                                     + XMLDocument.AddData(translations[SubmissionListTranslations.CellStatus], EvaluationsStyle.HeaderTable.ToString()));
        //            }
        //    #endregion
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

        //            String export = "";
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
        //        private static List<String> DefineEvaluationsStyle() {
        //            List<String> styles = new List<String>();
        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.HeaderTable.ToString(), XMLStyles.Alignment.center,
        //                            XMLStyles.Border.continuous,XMLStyles.Color.white,true,XMLStyles.Color.darkgray));

        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.RowItem.ToString(), XMLStyles.Alignment.left,
        //                            XMLStyles.Border.continuous,XMLStyles.Color.black, false,XMLStyles.Color.white));

        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.RowAlternatingItem.ToString(), XMLStyles.Alignment.left,
        //                            XMLStyles.Border.continuous,XMLStyles.Color.black, false,XMLStyles.Color.lightgray));

        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemEmpty.ToString(), XMLStyles.Alignment.center,
        //                           XMLStyles.Border.continuous,XMLStyles.Color.black, false,XMLStyles.Color.white));

        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemEmptyAlternate.ToString(), XMLStyles.Alignment.center,
        //                          XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.lightgray));

        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemYellow.ToString(), XMLStyles.Alignment.center,
        //                         XMLStyles.Border.continuous,XMLStyles.Color.black, false,XMLStyles.Color.yellow));

        //            styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemRed.ToString(), XMLStyles.Alignment.center,
        //                         XMLStyles.Border.continuous,XMLStyles.Color.black, false,XMLStyles.Color.red));


        //             styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.CallForPaperTitle.ToString(), XMLStyles.Alignment.left ,
        //                         XMLStyles.Border.none,XMLStyles.Color.black, true ,XMLStyles.Color.white));
        //             styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.PrintInfo.ToString(), XMLStyles.Alignment.left,
        //                         XMLStyles.Border.none, XMLStyles.Color.black, false, XMLStyles.Color.white));
  
        //            return styles;
        //        }

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
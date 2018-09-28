using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public static class ExportHelpers
    {
        private static String GetStatus(StatusStatistic status, Dictionary<EduPathTranslations, String> translations)
        {
            switch (status)
            {

                case StatusStatistic.Started:
                    return translations[EduPathTranslations.Started];

                case StatusStatistic.Completed:
                    return translations[EduPathTranslations.Completed];

                case StatusStatistic.Passed:
                    return translations[EduPathTranslations.Passed];

                case StatusStatistic.CompletedPassed:
                    return translations[EduPathTranslations.CompletedPassed];

                default:
                    return translations[EduPathTranslations.NotStarted];
            }

        }


        private static String GetItemType(ItemType type, Dictionary<EduPathTranslations, String> translations)
        {
            switch (type)
            {

                case ItemType.Path:
                    return translations[EduPathTranslations.Path];

                case ItemType.Unit:
                    return translations[EduPathTranslations.Unit];

                case ItemType.Activity:
                    return translations[EduPathTranslations.Activity];

                case ItemType.SubActivity:
                    return translations[EduPathTranslations.SubActivity];
            }
            return "";
        }


        //#endregion

        #region xml
        
        //private static string UsersStatTableHeader_ToXml(bool isTimeBased, bool isAutoEp, ItemType type,  Dictionary<EduPathTranslations, String> translations)
        //{


        //    String row = BuilderXmlDocument.AddData(translations[EduPathTranslations.SurnameAndName], EpExportStyle.HeaderTable.ToString());

        //    if (type != ItemType.SubActivity)
        //    {
        //        if (isTimeBased)
        //        {
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Time], EpExportStyle.HeaderTable.ToString());
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.MinTime], EpExportStyle.HeaderTable.ToString());
        //        }
        //        else
        //        {
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Completion], EpExportStyle.HeaderTable.ToString());
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.MinCompletion], EpExportStyle.HeaderTable.ToString());
        //        }
        //        if (!isAutoEp)
        //        {
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Mark], EpExportStyle.HeaderTable.ToString());
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.MinMark], EpExportStyle.HeaderTable.ToString());
        //        }
        //    }
        //    else
        //    {
        //        if (isTimeBased)
        //        {
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Time], EpExportStyle.HeaderTable.ToString());
        //        }
        //        else
        //        {
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Completion], EpExportStyle.HeaderTable.ToString());
        //        }
        //        if (!isAutoEp)
        //        {
        //            row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Mark], EpExportStyle.HeaderTable.ToString());
        //        }
        //    }

        //    row += BuilderXmlDocument.AddData(translations[EduPathTranslations.Status], EpExportStyle.HeaderTable.ToString());
        //    return BuilderXmlDocument.AddRow(row);
        //}

        //public static string WriteUserStat_ToXml(dtoUserStatExtended item, ItemType type, bool isAutoEp, bool isTimeBased, int rowNumber, Dictionary<EduPathTranslations, String> translations)
        //{
        //    String row = "";
        //    String rowstyle = (rowNumber % 2 == 0 ? EpExportStyle.RowAlternatingItem.ToString() : EpExportStyle.RowItem.ToString());

        //    row = BuilderXmlDocument.AddData(item.SurnameAndName, rowstyle);
            
        //    if (isTimeBased)
        //    {
        //        row += BuilderXmlDocument.AddData(GetTime(item.Completion), rowstyle);
        //        row += BuilderXmlDocument.AddData(GetTime(item.MinCompletion), rowstyle);
        //    }
        //    else
        //    {
        //        row += BuilderXmlDocument.AddData(item.Completion, rowstyle);
        //        row += BuilderXmlDocument.AddData(item.MinCompletion, rowstyle);
        //    }
        //    if (!isAutoEp)
        //    {
        //        row += BuilderXmlDocument.AddData(item.Mark, rowstyle);
        //        row += BuilderXmlDocument.AddData(item.MinMark, rowstyle);
        //    }


        //    row += BuilderXmlDocument.AddData(GetStatus(item.StatusStat, translations), rowstyle);
        //    return BuilderXmlDocument.AddRow(row);
        //}

        //public static string WriteUserStat_ToXml(dtoUserStat item, ItemType type, bool isAutoEp, bool isTimeBased, int rowNumber, Dictionary<EduPathTranslations, String> translations)
        //{
        //    String row = "";
        //    String rowstyle = (rowNumber % 2 == 0 ? EpExportStyle.RowAlternatingItem.ToString() : EpExportStyle.RowItem.ToString());

        //    row = BuilderXmlDocument.AddData(item.SurnameAndName, rowstyle);

        
        //        if (isTimeBased)
        //        {
        //            row += BuilderXmlDocument.AddData(GetTime(item.Completion), rowstyle);
        //        }
        //        else
        //        {
        //            row += BuilderXmlDocument.AddData(item.Completion, rowstyle);
        //        }
        //        if (!isAutoEp)
        //        {
        //            row += BuilderXmlDocument.AddData(item.Mark, rowstyle);
        //        }


        //        row += BuilderXmlDocument.AddData(GetStatus(item.StatusStat, translations), rowstyle);
        //        return BuilderXmlDocument.AddRow(row);
        //}

       

        //public static String ExportUsersStat_ToXml(dtoListUserStat itemStat, ItemType type, bool isAutoEp, bool isTimeBased, Dictionary<EduPathTranslations, String> translations)
        //{
        //    String export = "";
        //    String retString = "";
        //    String row = "";

        //    row = BuilderXmlDocument.AddData(GetItemType(type, translations) + ": " + itemStat.ItemName);
        //    retString += BuilderXmlDocument.AddRow(row);

        //    retString += BuilderXmlDocument.AddRow("");

        //    row = BuilderXmlDocument.AddData(translations[EduPathTranslations.Mandatory], EpExportStyle.ItemEmpty.ToString());
        //    row += BuilderXmlDocument.AddData(CheckStatus(itemStat.Status, Status.Mandatory), EpExportStyle.ItemEmpty.ToString());

        //    retString += BuilderXmlDocument.AddRow(row);

        //    retString += BuilderXmlDocument.AddRow("");

        //    retString += UsersStatTableHeader_ToXml(isTimeBased, isAutoEp, type, translations);
            
        //    int rowNumber = 0;

        //    foreach (dtoUserStatExtended item in itemStat.usersStat)
        //    {               
        //        retString += WriteUserStat_ToXml(item,type,isAutoEp,isTimeBased,rowNumber,translations);
        //        rowNumber++;
        //    }

        //    export += BuilderXmlDocument.AddWorkSheet("--", retString);
        //    return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());

        //}

        //public static String ExportGlobalEpStats_ToXml(dtoEpGlobalStat stats, bool isAutoEp, Dictionary<EduPathTranslations, String> translations, lm.Comol.Core.DomainModel.Common.iModuleActionView oDisplayAction)
        //{
        //    String export = "";
        //    String header = "";

        //    int rowNumber = 0;

        //    //header += AddGlobalStatHeader(translations, isAutoEp);
        //    //header += AddGlobalStatRow(stats, isAutoEp,  ItemType.Path, translations, rowNumber);
        //    //rowNumber++;

        //    foreach (dtoUnitGlobalStat dtoUnit in stats.childrenStat)
        //    {
        //        if (!CheckStatus(dtoUnit.status, Status.Text))
        //        {
        //            header += AddGlobalStatRow(dtoUnit, isAutoEp, ItemType.Unit, translations, rowNumber);
        //            rowNumber++;

        //            foreach (dtoActivityGlobalStat dtoAct in dtoUnit.childrenStat)
        //            {
        //                if (!CheckStatus(dtoUnit.status, Status.Text))
        //                {
        //                    header += AddGlobalStatRow(dtoUnit, isAutoEp, ItemType.Activity, translations, rowNumber);
        //                    rowNumber++;

        //                    foreach (dtoSubActGlobalStat dtoSubAct in dtoAct.childrenStat)
        //                    {
        //                        header += AddGlobalStatRow(dtoSubAct, isAutoEp, ItemType.SubActivity, translations, rowNumber);
        //                        rowNumber++;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    export += BuilderXmlDocument.AddWorkSheet("--", header);
        //    return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
          
        //}

        //private static String AddGlobalStatHeader(Dictionary<EduPathTranslations, String> translations, bool isAutoEp)
        //{
        //    String header = "";
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.LevelType], EpExportStyle.HeaderTable.ToString());
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.Type], EpExportStyle.HeaderTable.ToString());
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.Name], EpExportStyle.HeaderTable.ToString());
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.Weight], EpExportStyle.HeaderTable.ToString());
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.CompletedPassed], EpExportStyle.HeaderTable.ToString());
        //    if (!isAutoEp)
        //    {
        //        header += BuilderXmlDocument.AddData(translations[EduPathTranslations.Completed], EpExportStyle.HeaderTable.ToString());
        //        header += BuilderXmlDocument.AddData(translations[EduPathTranslations.Passed], EpExportStyle.HeaderTable.ToString());
        //    }
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.Started], EpExportStyle.HeaderTable.ToString());
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.NotStarted], EpExportStyle.HeaderTable.ToString());
        //    header += BuilderXmlDocument.AddData(translations[EduPathTranslations.ParticipantNumber], EpExportStyle.HeaderTable.ToString());

        //    return BuilderXmlDocument.AddRow(header);
        //}

        //private static String AddGlobalStatRow(dtoSubActGlobalStat genericStat, bool isAutoEp, ItemType type, Dictionary<EduPathTranslations, String> translations, int rowNumber, lm.Comol.Core.DomainModel.Common.iModuleActionView oDisplayAction)
        //{
        //    EpExportStyle rowstyle = (rowNumber % 2 == 0 ? EpExportStyle.RowAlternatingItem : EpExportStyle.RowItem);

        //    String row = "";
        //    row += BuilderXmlDocument.AddData((int)type, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(GetType(type, translations), rowstyle.ToString());
        //    if (genericStat.itemName.Count() == 0)
        //    {
        //        row += BuilderXmlDocument.AddData(oDisplayAction.getDescriptionByLink(genericStat.ModuleLink), rowstyle.ToString());
        //    }
        //    else
        //    {
        //        row += BuilderXmlDocument.AddData(genericStat.itemName, rowstyle.ToString());
        //    }
        //    row += BuilderXmlDocument.AddData(genericStat.Weight, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.compPassedCount, rowstyle.ToString());
        //    if (!isAutoEp)
        //    {
        //        row += BuilderXmlDocument.AddData(genericStat.completedCount, rowstyle.ToString());
        //        row += BuilderXmlDocument.AddData(genericStat.passedCount, rowstyle.ToString());
        //    }
        //    row += BuilderXmlDocument.AddData(genericStat.startedCount, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.notStartedCount, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.userCount, rowstyle.ToString());

        //    return BuilderXmlDocument.AddRow(row);
        //}

        //private static String AddGlobalStatRow(dtoGenericGlobalStat genericStat, bool isAutoEp, ItemType type, Dictionary<EduPathTranslations, String> translations, int rowNumber)
        //{
        //    EpExportStyle rowstyle = (rowNumber % 2 == 0 ? EpExportStyle.RowAlternatingItem : EpExportStyle.RowItem);

        //    String row = "";
        //    row += BuilderXmlDocument.AddData((int)type, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(GetType(type, translations), rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.itemName, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.Weight, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.compPassedCount, rowstyle.ToString());
        //    if (!isAutoEp)
        //    {
        //        row += BuilderXmlDocument.AddData(genericStat.completedCount, rowstyle.ToString());
        //        row += BuilderXmlDocument.AddData(genericStat.passedCount, rowstyle.ToString());
        //    }
        //    row += BuilderXmlDocument.AddData(genericStat.startedCount, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.notStartedCount, rowstyle.ToString());
        //    row += BuilderXmlDocument.AddData(genericStat.userCount, rowstyle.ToString());

        //    return BuilderXmlDocument.AddRow(row);
        //}

        #endregion

        #region pdf

        //private static void UserStatHeader(String name, bool isMandatory, Document doc, Dictionary<EduPathTranslations, String> translations)
        //{
        //    doc.Add(new Chunk((translations[EduPathTranslations.Name] + ": "), ItemCellFont()));
        //    doc.Add(new Chunk(name, TitleItemFont()));
        //    doc.Add(TextSeparator());
        //    doc.Add(new Chunk(translations[EduPathTranslations.Mandatory] + ": ", ItemCellFont()));
        //    doc.Add(new Chunk(isMandatory.ToString(), ItemCellFont()));
        //    doc.Add(TextSeparator());
        //}

        //private static Table InitTable(int dim)
        //{
        //    Table table = new Table(dim);
        //    table.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
        //    table.Border = 0;
        //    table.Cellpadding = 1;
        //    table.Cellspacing = 0;
        //    table.TableFitsPage = true;
        //    table.WidthPercentage = 100;
        //    table.AutoFillEmptyCells = true;
        //    return table;
        //}


        //private static void UsersStatTableHeader_Pdf(bool isTimeBased,bool isAutoEp, ItemType type,  Table table, Dictionary<EduPathTranslations, String> translations)
        //{
        //    table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.SurnameAndName], TitleCellFont())));

        //    if (type != ItemType.SubActivity)
        //    {
        //        if (isTimeBased)
        //        {
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Time], TitleCellFont())));
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.MinTime], TitleCellFont()))); 
        //        }
        //        else
        //        {
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Completion], TitleCellFont())));
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.MinCompletion], TitleCellFont())));                  
        //        }
        //        if (!isAutoEp)
        //        {
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Mark], TitleCellFont())));
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.MinMark], TitleCellFont())));
        //        }
        //    }
        //    else
        //    {
        //        if (isTimeBased)
        //        {
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Time], TitleCellFont())));
        //        }
        //        else
        //        {
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Completion], TitleCellFont())));              
        //        }
        //        if (!isAutoEp)
        //        {
        //            table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Mark], TitleCellFont())));
        //        }
        //     }
                   
        //    table.AddCell(new Cell(new Chunk(translations[EduPathTranslations.Status], TitleCellFont())));
        
        //}
          
        //public static void WriteUserStat_ToPdf(dtoUserStatExtended stat, ItemType type, bool isAutoEp, bool isTimeBased, Table table, Dictionary<EduPathTranslations, String> translations)
        //{
        //    table.AddCell(new Cell(new Chunk(stat.SurnameAndName, ItemCellFont())));

        //    if (type != ItemType.SubActivity)
        //    {
        //        if (isTimeBased)
        //        {
        //            table.AddCell(new Cell(new Chunk(GetTime(stat.Completion), ItemCellFont())));
        //            table.AddCell(new Cell(new Chunk(GetTime(stat.MinCompletion), ItemCellFont())));
        //        }
        //        else
        //        {
        //            table.AddCell(new Cell(new Chunk(stat.Completion.ToString(), ItemCellFont())));
        //            table.AddCell(new Cell(new Chunk(stat.MinCompletion.ToString(), ItemCellFont())));
        //        }
        //        if (!isAutoEp)
        //        {
        //            table.AddCell(new Cell(new Chunk(stat.Mark.ToString(), ItemCellFont())));
        //            table.AddCell(new Cell(new Chunk(stat.MinMark.ToString(), ItemCellFont())));
        //        }
        //    }
        //    else
        //    {
        //        if (isTimeBased)
        //        {
        //            table.AddCell(new Cell(new Chunk(GetTime(stat.Completion), ItemCellFont())));
        //        }
        //        else
        //        {
        //            table.AddCell(new Cell(new Chunk(stat.Completion.ToString(), ItemCellFont())));
        //        }
        //        if (!isAutoEp)
        //        {
        //            table.AddCell(new Cell(new Chunk(stat.Mark.ToString(), ItemCellFont())));
        //        }
        //    }

        //    table.AddCell(new Cell(new Chunk(GetStatus(stat.StatusStat, translations), ItemCellFont())));
                   
        //}

        //public static void WriteUserStat_ToPdf(dtoUserStat stat, ItemType type, bool isAutoEp, bool isTimeBased, Table table, Dictionary<EduPathTranslations, String> translations)
        //{
        //    table.AddCell(new Cell(new Chunk(stat.SurnameAndName, ItemCellFont())));

        //        if (isTimeBased)
        //        {
        //            table.AddCell(new Cell(new Chunk(GetTime(stat.Completion), ItemCellFont())));
        //        }
        //        else
        //        {
        //            table.AddCell(new Cell(new Chunk(stat.Completion.ToString(), ItemCellFont())));
        //        }
        //        if (!isAutoEp)
        //        {
        //            table.AddCell(new Cell(new Chunk(stat.Mark.ToString(), ItemCellFont())));
        //        }
            

        //    table.AddCell(new Cell(new Chunk(GetStatus(stat.StatusStat, translations), ItemCellFont())));

        //}


        //public static Document ExportUsersStat_ToPdf(dtoListUserStat itemStat,bool isAutoEp, bool isTimeBased, ItemType type, Dictionary<EduPathTranslations, String> translations, System.IO.Stream stream)
        //{

        //    Document doc = new Document(PageSize.A4, 30, 30, 50, 50);
          
        //    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, stream);
                  
        //    doc.Open();
            
        //    Table table;
            
        //    if (isAutoEp)
        //    {
        //        table = InitTable(4);
        //    }           
        //    else
        //    {
        //        table = InitTable(6);
        //    }
        //    UserStatHeader(itemStat.ItemName, CheckStatus(itemStat.Status, Status.Mandatory), doc, translations);
        //    UsersStatTableHeader_Pdf(isTimeBased, isAutoEp, type, table, translations);
            
        //    foreach (dtoUserStatExtended item in itemStat.usersStat)
        //    {
        //        WriteUserStat_ToPdf(item, type, isAutoEp, isTimeBased, table, translations);
        //    }

        //    doc.Add(table);
        //    doc.NewPage();
        //    doc.Close();
        //    return doc;          
        //}

        #endregion

        #region "Header / Footer"
        //private static List<String> DefineEvaluationsStyle()
        //{
        //    List<String> styles = new List<String>();
        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.HeaderTable.ToString(), XMLStyles.Alignment.center,
        //                    XMLStyles.Border.continuous, XMLStyles.Color.white, true, XMLStyles.Color.darkgray));

        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.RowItem.ToString(), XMLStyles.Alignment.left,
        //                    XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.white));

        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.RowAlternatingItem.ToString(), XMLStyles.Alignment.left,
        //                    XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.lightgray));

        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemEmpty.ToString(), XMLStyles.Alignment.center,
        //                   XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.white));

        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemEmptyAlternate.ToString(), XMLStyles.Alignment.center,
        //                  XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.lightgray));

        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemYellow.ToString(), XMLStyles.Alignment.center,
        //                 XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.yellow));

        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.ItemRed.ToString(), XMLStyles.Alignment.center,
        //                 XMLStyles.Border.continuous, XMLStyles.Color.black, false, XMLStyles.Color.red));


        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.CallForPaperTitle.ToString(), XMLStyles.Alignment.left,
        //                XMLStyles.Border.none, XMLStyles.Color.black, true, XMLStyles.Color.white));
        //    styles.Add(XMLStyles.StyleDefinition(EvaluationsStyle.PrintInfo.ToString(), XMLStyles.Alignment.left,
        //                XMLStyles.Border.none, XMLStyles.Color.black, false, XMLStyles.Color.white));

        //    return styles;
        //}

        //private static String StandardEvaluationsTableHeader(EvaluationType evaluationType, Dictionary<EvaluationTranslations, string> translations)
        //{
        //    return XMLDocument.AddData(translations[EvaluationTranslations.CellPosition], EvaluationsStyle.HeaderTable.ToString())
        //                        + XMLDocument.AddData(translations[EvaluationTranslations.CellSubmitterName], EvaluationsStyle.HeaderTable.ToString())
        //                        + XMLDocument.AddData(translations[EvaluationTranslations.CellType], EvaluationsStyle.HeaderTable.ToString())
        //                        + XMLDocument.AddData(translations[(evaluationType == EvaluationType.Sum) ? EvaluationTranslations.CellSumRating : EvaluationTranslations.CellAverageRating], EvaluationsStyle.HeaderTable.ToString());
        //}
        //private static String EvaluationsTableHeader(EvaluationType evaluationType, Dictionary<EvaluationTranslations, string> translations)
        //{
        //    return XMLDocument.AddRow(StandardEvaluationsTableHeader(evaluationType, translations));
        //}
        //private static String EvaluationsTableHeader(EvaluationType evaluationType, Dictionary<EvaluationTranslations, string> translations, List<dtoEvaluator> evaluators)
        //{
        //    String header = StandardEvaluationsTableHeader(evaluationType, translations);
        //    foreach (dtoEvaluator evaluator in evaluators)
        //    {
        //        header += XMLDocument.AddData(evaluator.DisplayName, EvaluationsStyle.HeaderTable.ToString());
        //    }
        //    return XMLDocument.AddRow(header);
        //}
        //private static String EvaluationsPivotTableHeader(Dictionary<EvaluationTranslations, string> translations)
        //{
        //    return XMLDocument.AddRow(
        //        XMLDocument.AddData(translations[EvaluationTranslations.CellSubmissionNumber], EvaluationsStyle.HeaderTable.ToString())
        //                      + XMLDocument.AddData(translations[EvaluationTranslations.CellSubmitterName], EvaluationsStyle.HeaderTable.ToString())
        //                      + XMLDocument.AddData(translations[EvaluationTranslations.CellType], EvaluationsStyle.HeaderTable.ToString())
        //                      + XMLDocument.AddData(translations[EvaluationTranslations.CellEvaluator], EvaluationsStyle.HeaderTable.ToString())
        //                      + XMLDocument.AddData(translations[EvaluationTranslations.CellEvaluationComplete], EvaluationsStyle.HeaderTable.ToString())
        //                      + XMLDocument.AddData(translations[EvaluationTranslations.CellCriterion], EvaluationsStyle.HeaderTable.ToString())
        //                      + XMLDocument.AddData(translations[EvaluationTranslations.CellEvaluation], EvaluationsStyle.HeaderTable.ToString())
        //        );
        //}
        public enum EvaluationsStyle
        {
            HeaderTable = 1,
            RowItem = 2,
            RowAlternatingItem = 3,
            RowDeleted = 4,
            ItemEmpty = 5,
            ItemEmptyAlternate = 6,
            ItemYellow = 7,
            ItemRed = 8,
            CallForPaperTitle = 9,
            PrintInfo = 10
        }
        #endregion ->fare cernita

        public static bool CheckStatus(Status Actual, Status Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public static String GetTime(Int64 totMin)
        {
            Int64 h = totMin / 60;
            Int64 min = totMin % 60;
            return h + ":" + (min < 10 ? "0" : "") + min;
        }

        private static String GetType(ItemType type, Dictionary<EduPathTranslations, String> translations)
        {
            switch (type)
            {
                case ItemType.Path:
                    return translations[EduPathTranslations.Path];

                case ItemType.Unit:
                    return translations[EduPathTranslations.Unit];

                case ItemType.Activity:
                    return translations[EduPathTranslations.Activity];

                case ItemType.SubActivity:
                    return translations[EduPathTranslations.SubActivity];

                default:
                    return "";
            }

        }

    }

    //public enum EpExportStyle
    //{
    //    HeaderTable = 1,
    //    RowItem = 2,
    //    RowAlternatingItem = 3,
    //    RowDeleted = 4,
    //    ItemEmpty = 5,
    //    ItemEmptyAlternate = 6,
    //    ItemYellow = 7,
    //    ItemRed = 8,
    //    Title = 9,
        
    //}
}

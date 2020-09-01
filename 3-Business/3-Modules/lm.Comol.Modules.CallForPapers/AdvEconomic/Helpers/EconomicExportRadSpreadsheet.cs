using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Web.Spreadsheet;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming;




using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Txt;

using Telerik.Windows.Documents.Spreadsheet.Utilities;
using System.Threading.Tasks;


using Eco = lm.Comol.Modules.CallForPapers.AdvEconomic;

namespace lm.Comol.Modules.CallForPapers.AdvEconomic.Helpers
{
    public class EconomicExportRadSpreadsheet
    {



        /// <summary>
        /// Crea via stream un documento Excel (csv non previsto per via di fogli multipli) con tutte le tabelle economiche di una valutazione
        /// </summary>
        /// <param name="Eval">Dati valutazione (definizioni, tabelle, ammissioni, importi)</param>
        /// <param name="documentStream">Stream output</param>
        /// <param name="DocFormat">Formato - Solo XLSX</param>
        /// <param name="settings">Impostazioni di esportazione (caratteri, larghezze, colori) - Per personalizzazioni future.</param>
        public static void EcoEvalTableExportStream(
            Eco.Domain.EconomicEvaluation Eval,
            Stream documentStream,
            SpreadDocumentFormat DocFormat = SpreadDocumentFormat.Xlsx,
            dto.dtoEcoTableExportSettings settings = null)
        {
            if (settings == null)
                settings = new dto.dtoEcoTableExportSettings();


            int exportedCellsCount = 0;

            SpreadDocumentFormat selectedDocumentFormat;
            int totalCellsCount;
            DateTime exportStarted;
            bool canExport;

            if (Eval == null || Eval.Tables == null || !Eval.Tables.Any())
                return;


            using (IWorkbookExporter workbookExporter = SpreadExporter.CreateWorkbookExporter(DocFormat, documentStream))
            {
                int SheetNumber = 0;
                foreach (Eco.Domain.EconomicTable table in Eval.Tables)
                {
                    if (table != null && table.FieldDefinition != null)
                    {
                        SheetNumber++;
                        int headCols = table.HeaderValues.Count();
                        int totalCols = headCols + 7;

                        string sheetName = string.Format("{0}-{1}", SheetNumber, table.FieldDefinition.Name);

                        if (sheetName.Length > 25)
                        {
                            sheetName = string.Format("{0}...", sheetName.Substring(0, 20));
                        }

                        sheetName = CleanFileName(sheetName);


                        using (IWorksheetExporter worksheetExporter = workbookExporter.CreateWorksheetExporter(sheetName))
                        {
                            for (int i = 0; i < totalCols; i++)
                            {
                                using (IColumnExporter columnExporter = worksheetExporter.CreateColumnExporter())
                                {
                                    if (i >= headCols)
                                    {
                                        columnExporter.SetWidthInCharacters(settings.ColumnWidths[i - headCols]);
                                    }
                                    else
                                    {
                                        columnExporter.SetWidthInCharacters(settings.ColumnAddWidth);
                                    }
                                }
                            }

                            ExportHeaderRows(worksheetExporter, table.HeaderValues, settings, table.FieldDefinition.Name);


                            foreach (AdvEconomic.Domain.EconomicItem itm in table.Items)
                            {
                                using (IRowExporter rowExporter = worksheetExporter.CreateRowExporter())
                                {
                                    rowExporter.SetHeightInPoints(settings.RowHeight);

                                    int columnIndex = 0;

                                    int iv = 0;


                                    foreach (string value in itm.InfoValues)
                                    {
                                        iv++;

                                        if (iv <= headCols)
                                            using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                            {
                                                cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.NormalFormat, itm.IsAdmit));
                                                cellExporter.SetValue(value.Replace("&nbsp;", " "));
                                            }
                                    }

                                    if (iv < headCols)
                                    {
                                        using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                        {
                                            cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.NormalFormat, itm.IsAdmit));
                                            cellExporter.SetValue("");
                                        }
                                    }


                                    //Quantità
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.DoubleFormat, itm.IsAdmit));
                                        cellExporter.SetValue(itm.RequestQuantity);
                                    }

                                    //Prezzo unitario
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.DoubleEcoFormat, itm.IsAdmit));
                                        cellExporter.SetValue(itm.RequestUnitPrice);
                                    }

                                    //Totale richiesto
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.DoubleEcoFormat, itm.IsAdmit));
                                        cellExporter.SetValue(itm.RequestTotal);
                                    }
                                    //Approvato
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.NormalFormat, itm.IsAdmit));
                                        cellExporter.SetValue(itm.IsAdmit ? settings.BoolValue[1] : settings.BoolValue[0]);
                                    }
                                    //Quantità ammessa
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.DoubleFormat, itm.IsAdmit));
                                        cellExporter.SetValue(itm.AdmitQuantity);
                                    }
                                    //Totale ammesso
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.DoubleEcoFormat, itm.IsAdmit));
                                        cellExporter.SetValue(itm.AdmitTotal);
                                    }
                                    //Commenti
                                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                                    {
                                        cellExporter.SetFormat(dto.dtoEcoTableExportSettings.InvalidCellFormat(settings.NormalFormat, itm.IsAdmit));
                                        if (itm.Comment != null)
                                            cellExporter.SetValue(itm.Comment.Replace("&nbsp;", " ").Replace("<br>", "\r\n"));
                                        else
                                            cellExporter.SetValue("");

                                    }
                                }
                            }
                        }
                    }


                }

            }

        }

        public static string CleanFileName(string value) 
        {
            string cleanedString = value
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("?", "")
                .Replace("*", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(":", "");

            if(String.IsNullOrWhiteSpace(cleanedString))
            {
                cleanedString = "empty";
            }

            return cleanedString;
        }


        /// <summary>
        /// Creazione riga Header
        /// </summary>
        /// <param name="worksheetExporter">Exporter (per stream)</param>
        /// <param name="Headers">Colonne Header aggiuntive (da definizione tabelle)</param>
        /// <param name="settings">Impostazioni esportazione - per future configurazioni</param>
        private static void ExportHeaderRows(
            IWorksheetExporter worksheetExporter, 
            string[] Headers, 
            dto.dtoEcoTableExportSettings settings,
            string TableName)
        {
            // Nome tabella
            if(!string.IsNullOrWhiteSpace(TableName))
            {
                using (IRowExporter rowExporter = worksheetExporter.CreateRowExporter())
                {
                    rowExporter.SetHeightInPoints(settings.HeaderRowHeight);
                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                    {
                        cellExporter.SetFormat(settings.HeaderFormat);
                        cellExporter.SetValue(TableName.Replace("&nbsp;", " "));
                    }
                }
            }
            

            using (IRowExporter rowExporter = worksheetExporter.CreateRowExporter())
            {
                rowExporter.SetHeightInPoints(settings.HeaderRowHeight);
                foreach(string hval in Headers)
                {
                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                    {   
                        cellExporter.SetFormat(settings.HeaderFormat);
                        cellExporter.SetValue(hval.Replace("&nbsp;", " "));
                    }
                }
                     
                for(int i = 0; i < settings.HeadStrings.Length; i++)
                {
                    using (ICellExporter cellExporter = rowExporter.CreateCellExporter())
                    {
                        cellExporter.SetFormat(settings.HeaderFormat);
                        cellExporter.SetValue(settings.HeadStrings[i]);
                    }
                }
                             
            }
                
        }

        #region Creazione Workbook, per edit on-line (rivedere se si usa)
        //public Workbook EcoExportGetWorkbook()
        //{
        //    Workbook WB = new Workbook();

        //    if (Table != null && Table.Summaries != null && Table.Summaries.Any())
        //    {
        //        foreach(Eco.dto.dtoEcoSummary summary in Table.Summaries)
        //        {
        //            var curSheet = WB.AddSheet();
        //            curSheet.Name = summary.SubmissionName;

        //            summary.
        //            foreach(EconomicSummaryHTMLexpHelper.)
        //        }

        //    }





        //    return WB;
        //}

        //public static Workbook EcoExportGetWorkbookPerUser(Eco.Domain.EconomicEvaluation Eval)
        //{
        //    Workbook WB = new Workbook();

        //    if (Eval != null && Eval.Tables != null && Eval.Tables.Any())
        //    {

        //        int shNum = 0;

        //        foreach (Eco.Domain.EconomicTable table in Eval.Tables)
        //        {
        //            shNum++;

        //            //Header

        //            int headCol = 0;

        //            var curSheet = WB.AddSheet();
        //            curSheet.Name = string.Format("{0} - {1}", shNum, table.FieldDefinition.Name);

        //            curSheet.Columns = new List<Column>();

        //            var row = new Row() { Index = 0 };
        //            int columnIndex = 0;

        //            Cell cell = new Cell();

        //            foreach (string hdVal in table.HeaderValues)
        //            {
        //                curSheet.Columns.Add(new Column());
        //                cell = new Cell() { Index = columnIndex++, Value = hdVal, Bold = true };
        //                row.AddCell(cell);
        //                headCol++;
        //            }

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Quantità", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Prezzo unitario", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Totale richiesto", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Approvato", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Quantità ammessa", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Spesa ammessa", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.Columns.Add(new Column());
        //            cell = new Cell() { Index = columnIndex++, Value = "Commenti", Bold = true };
        //            row.AddCell(cell);

        //            curSheet.AddRow(row);
        //            //RowData

        //            int rowIndex = 1;
        //            foreach (Eco.Domain.EconomicItem dataItem in table.Items)
        //            {
        //                row = new Row() { Index = rowIndex++ };
        //                columnIndex = 0;

        //                int iv = 0;

        //                foreach(string value in dataItem.InfoValues)
        //                {
        //                    iv++;
        //                    cell = new Cell() { Index = columnIndex++, Value = value, Bold = true };
        //                    row.AddCell(cell);
        //                }

        //                if(iv < headCol)
        //                {
        //                    for(int i = 1; i <= (headCol-iv); i++)
        //                    {
        //                        cell = new Cell() { Index = columnIndex++, Value = "", Bold = true };
        //                        row.AddCell(cell);
        //                    }
        //                }

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.RequestQuantity, Bold = true };
        //                cell.Format = "#.##";
        //                row.AddCell(cell);

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.RequestUnitPrice, Bold = true };
        //                cell.Format = "#.## €";
        //                row.AddCell(cell);

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.RequestTotal, Bold = true };
        //                cell.Format = "#.## €";
        //                row.AddCell(cell);

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.IsAdmit, Bold = true };
        //                cell.Format = "@";
        //                row.AddCell(cell);

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.AdmitQuantity, Bold = true };
        //                cell.Format = "#.##";
        //                row.AddCell(cell);

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.AdmitTotal, Bold = true };
        //                cell.Format = "#.## €";
        //                row.AddCell(cell);

        //                cell = new Cell() { Index = columnIndex++, Value = dataItem.Comment, Bold = true };
        //                cell.Format = "@";
        //                row.AddCell(cell);

        //                curSheet.AddRow(row);

        //            }

        //            //WB.AddSheet(curSheet);
        //        }
        //    }    

        //    return WB;
        //}

        //private static readonly double[] ColumnWidths = { 15.00, 20.00, 20.00, 15.00, 20.00, 20.00, 50.00 };
        //private static readonly double ColumnAddWidth = 25.00;
        //private static readonly int HeaderRowHeight = 22;
        //private static readonly int RowHeight = 18;

        //private static readonly string[] HeadStrings = { "Quantità", "Prezzo unitario", "Totale richiesto", "Approvato", "Quantità ammessa", "Spesa ammessa", "Commenti" };


        //private static SpreadCellFormat HeaderFormat
        //{
        //    get
        //    {
        //        SpreadCellFormat format = new SpreadCellFormat();
        //        format.FontFamily = new SpreadThemableFontFamily("Segoe UI");
        //        format.FontSize = 12;
        //        format.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(51, 153, 51));
        //        format.ForeColor = new SpreadThemableColor(new SpreadColor(255, 255, 255));
        //        format.HorizontalAlignment = SpreadHorizontalAlignment.Left;
        //        format.VerticalAlignment = SpreadVerticalAlignment.Center;

        //        return format;
        //    }
        //}


        //private static SpreadCellFormat NormalFormat
        //{
        //    get
        //    {
        //        SpreadCellFormat normalFormat = new SpreadCellFormat();
        //        normalFormat.FontSize = 10;
        //        normalFormat.VerticalAlignment = SpreadVerticalAlignment.Center;
        //        normalFormat.HorizontalAlignment = SpreadHorizontalAlignment.Center;

        //        return normalFormat;
        //    }
        //}

        //private static SpreadCellFormat DoubleFormat
        //{
        //    get
        //    {
        //        SpreadCellFormat format = NormalFormat;
        //        format.NumberFormat = "0.00";

        //        return format;
        //    }
        //}

        //private static SpreadCellFormat DoubleEcoFormat
        //{
        //    get
        //    {
        //        SpreadCellFormat format = NormalFormat;
        //        format.NumberFormat = "0.00 €";

        //        return format;
        //    }
        //}
#endregion
    }
}

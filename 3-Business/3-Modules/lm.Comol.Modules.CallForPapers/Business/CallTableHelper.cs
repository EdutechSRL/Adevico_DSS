using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Business
{
    /// <summary>
    /// Helper tabelle economiche
    /// </summary>
    public static class CallTableHelper
    {
        /// <summary>
        /// "Decora" l'html di una tabella con classi e parametri opzionali per la visualizzazione
        /// </summary>
        /// <param name="item">Valore inserito dall'utente</param>
        /// <param name="forcompile">True se è per la compilazione</param>
        /// <returns>Una stringa con l'html completo della tabella</returns>
        public static string TableDecorateHtml(dtoSubmissionValueField item, bool forcompile)
        {
            String tablestr = (item != null && item.Value != null) ? item.Value.Text : "";
                
            //TableSetting.Cols.Split("|").ToList()
            if (String.IsNullOrEmpty(tablestr))
                tablestr = "<table></table>";

            if (item.Field.Type == FieldType.TableSimple)
            {
                tablestr = TableSimpleDecorateHtml(tablestr, item, forcompile);
            }
            else if (item.Field.Type == FieldType.TableReport)
            {
                tablestr = TableReportDecorateHtml(tablestr, item, forcompile);
            }

            return tablestr;
        }
        /// <summary>
        /// "Decora" l'html di una tabella semplice con classi e parametri opzionali per la visualizzazione
        /// </summary>
        /// <param name="contentTable">Tabella semplice - compilazione utente</param>
        /// <param name="item">elemento sottomesso dall'utente</param>
        /// <param name="forcompile">True se è epr la compilazione</param>
        /// <returns></returns>
        public static string TableSimpleDecorateHtml(string contentTable, dtoSubmissionValueField item, bool forcompile)
        {
            forcompile = forcompile || String.IsNullOrEmpty(contentTable);

            String CompileString = "&nbsp;";



            if (item.Field.TableFieldSetting != null && !String.IsNullOrEmpty(item.Field.TableFieldSetting.Cols))
            {
                String Header = "";

                foreach (String th in item.Field.TableFieldSetting.Cols.Split('|').ToList())
                {
                    Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, th);
                    CompileString = string.Format("{0}<td border=\"1\">&nbsp; </td>", CompileString);
                }

                CompileString = String.IsNullOrEmpty(CompileString) ? "<td>&nbsp;</td>" : CompileString;

                Header = String.IsNullOrEmpty(Header)
                    ? ""
                    : string.Format("<tr>{0}</tr>", Header);

                contentTable = contentTable.Replace(
                    "<table>", 
                    String.Format("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\">{0}", Header));
            }

            if (forcompile)
            {
                contentTable = contentTable.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
            }
            contentTable = contentTable.Replace("></td>", ">&nbsp; </td>");

            return contentTable.Replace(Environment.NewLine, "<br />");
        }

       
        /// <summary>
        /// Dato l'elemento associato alla compilazione utente (stringa), verifica se è superata la soglia massima prevista
        /// </summary>
        /// <param name="item">Contenuto elemento sottomissione utente (tabella)</param>
        /// <returns>TRUE se la soglia è stata superata</returns>
        public static bool IsOverRange(SubmissionFieldStringValue item)
        {
            if (item == null
                || item.Field == null
                || item.Field.Type != FieldType.TableReport
                || item.Field.TableMaxTotal == 0)
                return false;

            if (string.IsNullOrEmpty(item.Value))
                return false;

            return IsOverRange(item.Value, item.Field.TableMaxTotal);
        }
        /// <summary>
        /// Dato l'elemento associato alla compilazione utente (stringa), verifica se è superata la soglia massima prevista
        /// </summary>
        /// <param name="item">Contenuto elemento sottomissione utente (tabella)</param>
        /// <returns>TRUE se la soglia è stata superata</returns>
        public static bool IsOverRange(dtoSubmissionValueField item)
        {
            if (item == null
                || item.Field == null
                || item.Field.Type != FieldType.TableReport
                || item.Field.TableFieldSetting == null
                || item.Field.TableFieldSetting.MaxTotal == 0
                )
            {
                return false;
            }

            if (item.Value == null || String.IsNullOrEmpty(item.Value.Text))
                return false; //è vuoto, cioè 0 = sono sotto

            return IsOverRange(item.Value.Text, item.Field.TableFieldSetting.MaxTotal);
        }
        
        /// <summary>
        /// Data una tabella "grezza" (valore persistito) ed un valore massimo, verifica se è stato superaro il massimale indicato
        /// </summary>
        /// <param name="RawTable">Tabella economica</param>
        /// <param name="MaxTotal">Massimale ammesso</param>
        /// <returns>True se il totale supera il massimale ammesso</returns>
        public static bool IsOverRange(string RawTable, double MaxTotal)
        {
            Double total = 0;

            try
            {
                string contentXml = String.Format("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>{0}{1}",
               System.Environment.NewLine, RawTable)
               ;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(contentXml);


                foreach (XmlNode trXml in doc.GetElementsByTagName("tr"))
                {
                    string prow = "";

                    foreach (XmlNode tdXml in trXml.ChildNodes)
                    {
                        string ptd = "";
                        string cssclass = "";

                        if (tdXml.Attributes != null && tdXml.Attributes.Count > 0)
                        {
                            try
                            {
                                cssclass = tdXml.Attributes["class"].Value;
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        if (!String.IsNullOrEmpty(cssclass) && cssclass == "total")
                        {
                            Double val = 0;
                            Double.TryParse(tdXml.InnerText.ToString(), out val);

                            total += val;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            if (total > MaxTotal)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// "Decora" l'html di una tabella Economica con classi e parametri opzionali per la visualizzazione
        /// </summary>
        /// <param name="contentTable">Tabella sorgente</param>
        /// <param name="item">Elemento sottomissione</param>
        /// <param name="forcompile">True se è per la compilazione</param>
        /// <returns>Stringa html con la tabella</returns>
        public static string TableReportDecorateHtml(string contentTable, dtoSubmissionValueField item, bool forcompile)
        {
            try
            {
                String sourceTable = item.Value.Text;
                String outTable = "";

                String CompileString = "&nbsp;";

                String Header = "";


                int colCount = 0;

                if (item.Field.TableFieldSetting != null )
                {

                    if(!String.IsNullOrEmpty(item.Field.TableFieldSetting.Cols))
                    {
                        foreach (String th in item.Field.TableFieldSetting.Cols.Split('|').ToList())
                        {
                            Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, th);
                            CompileString = string.Format("{0}<td border=\"1\">&nbsp; </td>", CompileString);
                            colCount++;
                        }
                    } else
                    {
                        Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, "");
                        CompileString = string.Format("{0}<td border=\"1\">&nbsp; </td>", CompileString);
                        colCount++;
                    }

                    Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, "Quantità");
                    CompileString = string.Format("{0}<td border=\"1\">&nbsp; </td>", CompileString);

                    Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, "Costo unitario");
                    CompileString = string.Format("{0}<td border=\"1\">&nbsp; </td>", CompileString);

                    Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, "Totale");
                    CompileString = string.Format("{0}<td border=\"1\">&nbsp; </td>", CompileString);

                    if (!String.IsNullOrEmpty(Header))
                    {
                        Header = string.Format("<tr>{0}</tr>", Header);
                    }

                    colCount += 3;
                }

                //if (forcompile)
                //{
                //    sourceTable = sourceTable.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //    //tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //    //tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //}


                //Aggiungo Header
                //outTable = sourceTable.Replace("<table>", String.Format("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\"><tr>{0}</tr>", Header));

                Double total = 0;
                string rows = "";

                if (!(String.IsNullOrEmpty(sourceTable)))
                {
                    string contentXml = String.Format("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>{0}{1}",
                    System.Environment.NewLine, sourceTable)
                    ;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(contentXml);

                    string rowformat = "{0}<tr>{1}</tr>";


                    foreach (XmlNode trXml in doc.GetElementsByTagName("tr"))
                    {
                        string prow = "";
                        int currentColCount = 0;

                        foreach (XmlNode tdXml in trXml.ChildNodes)
                        {
                            string ptd = "";


                            string cssclass = "";


                            if (tdXml.Attributes != null && tdXml.Attributes.Count > 0)
                            {
                                try
                                {
                                    cssclass = tdXml.Attributes["class"].Value;
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                            //(tdXml.Attributes != null && tdXml.Attributes.("class") != null) ?   : "";

                            if (!String.IsNullOrEmpty(cssclass))
                            {
                                Double val = 0;
                                Double.TryParse(tdXml.InnerText.ToString(), out val);

                                string euro = (cssclass == "total" || cssclass == "unitycost") ? "&euro;" : ""; //€-


                                ptd = string.Format("<td border=\"1\" class=\"{0}\">{1} {2}</td>", cssclass, val, euro);

                                if (cssclass == "total")
                                {
                                    total += val;
                                }
                            }
                            else
                            {
                                ptd = string.Format("<td border=\"1\">{0}</td>", tdXml.InnerText);
                            }


                            colCount = (currentColCount > colCount) ? currentColCount : colCount;   //Fix se header vuoto

                            prow = String.Format("{0}{1}", prow, ptd);
                        }

                        rows = string.Format(rowformat, rows, prow);
                    }

                    //colCount = doc.GetElementsByTagName("tr").Item(0).ChildNodes.Count;
                }

                String totalRow = String.Format("<tr><td colspan=\"{0}\">&nbsp;</td><td>{1}</td><td>{2} &euro;</td></tr>",
                    (colCount - 2), "Totale", total);



                if (forcompile || String.IsNullOrEmpty(sourceTable))
                {
                    outTable = string.Format("<table  border=\"1\" cellspacing=\"0\">{0}{1}<tr>{2}</tr>{3}</table>",
                    Header,
                    rows,
                    CompileString,
                    totalRow);
                }
                else
                {
                    outTable = string.Format("<table border=\"1\" cellspacing=\"0\">{0}{1}{2}</table>",
                    Header,
                    rows,
                    totalRow);
                }

                outTable = outTable.Replace("></td>", ">&nbsp; </td>").Replace("<td>", "<td border=\"1\">");

                return outTable.Replace(Environment.NewLine, "<br />"); ; //tablestr;
            }
            catch (Exception ex)
            {

            }
            return contentTable;
        }


    }
}

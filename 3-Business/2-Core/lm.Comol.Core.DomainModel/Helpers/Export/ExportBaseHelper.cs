using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TemplVers_Export = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;
// Update VERS: OK!!!
namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    [Serializable]
    public abstract class ExportBaseHelper
    {

        #region "Initializers"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
    
        //public static PageSettings GetPageSettings(Rectangle pageSize, Boolean showPageNumber, float marginLeft, float marginRight, float marginTop, float marginBottom)
        //{
        //    PageSettings settings = GetDefaultPageSettings();

        //    settings.MarginBottom = marginBottom;
        //    settings.MarginLeft = marginLeft;
        //    settings.MarginRight = marginRight;
        //    settings.MarginTop = marginTop;
        //    settings.ShowPageNumber = showPageNumber;
        //    settings.Height = pageSize.Height;
        //    settings.Width = pageSize.Width;
        //    return settings;
        //}
        //private static Document InitializeDocument(PageSettings settings)
        //{
        //    if (settings == null)
        //        settings = GetDefaultPageSettings();

        //    Document doc = new Document(new Rectangle(settings.Width, settings.Height), settings.MarginLeft, settings.MarginRight, settings.MarginTop, settings.MarginBottom);

        //    return doc;
        //}
        #endregion

        #region "Export To file / Stream"
        //#region "Web Export"
        //    public Document WebExport(String fileName, System.Web.HttpResponse webResponse, ExportFileType type)
        //    {
        //        return WebExport(false, fileName, GetDefaultPageSettings(), webResponse, null, type);
        //    }
        //    public Document WebExport(Boolean openCloseConnection,String fileName, System.Web.HttpResponse webResponse, ExportFileType type)
        //    {
        //        return WebExport(openCloseConnection,fileName, GetDefaultPageSettings(), webResponse, null, type);
        //    }
        //    public Document WebExport(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, ExportFileType type)
        //    {
        //        return WebExport(openCloseConnection,fileName, GetDefaultPageSettings(), webResponse, cookie, type);
        //    }
        //    public Document WebExport(Boolean openCloseConnection, String fileName, PageSettings settings, System.Web.HttpResponse webResponse, ExportFileType type)
        //    {
        //        return WebExport(openCloseConnection, fileName, settings, webResponse, null, type);
        //    }
        //    //public Document WebExport(String fileName, PageSettings settings, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, ExportFileType type)
        //    //{
        //    //    return WebExport(fileName, settings, webResponse, cookie, type);
        //    //}
        //#endregion

        //#region "File Export"
        //    public Document FileExport(System.IO.FileStream fileStream, ExportFileType type)
        //    {
        //        return FileExport(GetDefaultPageSettings(), fileStream, type);
        //    }
        //    //public Document FileExport(Document doc,System.IO.FileStream fileStream, ExportFileType type)
        //    //{
        //    //    return FileExport(doc,GetDefaultPageSettings(), fileStream, type);
        //    //}
        //    public Document FileExport(PageSettings settings, System.IO.FileStream fileStream, ExportFileType type)
        //    {
        //        return ExportTo(settings,fileStream, type,false );
        //    }
        //    //public Document FileExport(Document doc, PageSettings settings, System.IO.FileStream fileStream, ExportFileType type)
        //    //{
        //    //    return ExportTo(doc,settings, fileStream, type);
        //    //}
        //#endregion

        //#region "Export"
        //    public Document WebExport(Boolean openCloseConnection, String fileName, PageSettings settings, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, ExportFileType type)
        //    {
        //        if (openCloseConnection)
        //            webResponse.Clear();
        //        if (cookie != null)
        //            webResponse.AppendCookie(cookie);
        //        webResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + type.ToString());

        //        switch (type)
        //        {
        //            case ExportFileType.pdf:
        //                webResponse.ContentType = "application/pdf";
        //                break;
        //            case ExportFileType.rtf:
        //                webResponse.ContentType = "application/rtf";
        //                break;
        //            }
        //        Document doc = ExportTo(settings, webResponse.OutputStream, type, false);

        //        if (doc != null && openCloseConnection)
        //            webResponse.End();
        //        return doc;
        //    }
        //    public Document GetErrorDocument(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse, ExportFileType type)
        //    {
        //        return GetErrorDocument(openCloseConnection,fileName, GetDefaultPageSettings(), webResponse, null, type);
        //    }
        //    public Document GetErrorDocument(Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, ExportFileType type)
        //    {
        //        return GetErrorDocument(openCloseConnection, fileName, GetDefaultPageSettings(), webResponse, cookie, type);
        //    }
        //    public Document GetErrorDocument(Boolean openCloseConnection, String fileName, PageSettings settings, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, ExportFileType type)
        //    {
        //        if (openCloseConnection)
        //            webResponse.Clear();
        //        if (cookie!=null)
        //            webResponse.AppendCookie(cookie);
        //        webResponse.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + type.ToString());

        //        switch (type)
        //        {
        //            case ExportFileType.pdf:
        //                webResponse.ContentType = "application/pdf";
        //                break;
        //            case ExportFileType.rtf:
        //                webResponse.ContentType = "application/rtf";
        //                break;
        //        }
        //        Document doc = ExportTo(settings, webResponse.OutputStream, type,true);

        //        if (doc != null && openCloseConnection)
        //            webResponse.End();
        //        return doc;
        //    }
        //    private Document ExportTo(PageSettings settings, System.IO.Stream stream, ExportFileType type, Boolean forErrorContent)
        //    {
        //        Document doc = null;
        //        try
        //        {
        //            doc = InitializeDocument(settings);
        //            switch (type)
        //            {
        //                case ExportFileType.pdf:
        //                    PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
        //                    if (!settings.HasHeaderOnFirstPage)
        //                        doc.Open();
        //                    if(!settings.HTMLheader){
        //                        HeaderFooter header = GetHeaderContent(doc, pdfWriter);
        //                        if (header != null)
        //                            doc.Header = header;
        //                    }
        //                    if(!settings.HTMLfooter){
        //                        HeaderFooter footer = GetFooterContent(doc, pdfWriter);
        //                        if (footer != null)
        //                            doc.Footer = footer;
        //                    }
        //                    if (settings.HTMLfooter || settings.HTMLheader){
        //                        PdfPageEventHandler ev = GetHeaderFooterContent(doc, pdfWriter);
        //                        pdfWriter.PageEvent = ev;
        //                    }
        //                    if (settings.HasHeaderOnFirstPage)
        //                        doc.Open();
        //                    ////Compilazione documento
        //                    if (forErrorContent)
        //                        RenderErrorDocument(doc, type);
        //                    else
        //                        RenderDocument(doc, pdfWriter);
                           
        //                    doc.Close();

        //                    break;
        //                case ExportFileType.rtf:
        //                    iTextSharp.text.rtf.RtfWriter2 rtfWriter = iTextSharp.text.rtf.RtfWriter2.GetInstance(doc, stream);
        //                    if (!settings.HasHeaderOnFirstPage)
        //                        doc.Open();
        //                    if(!settings.HTMLheader){
        //                        HeaderFooter headerR = GetHeaderContent(doc, rtfWriter);
        //                        if (headerR != null)
        //                            doc.Header = headerR;
        //                    }

        //                    if(!settings.HTMLfooter){
        //                        HeaderFooter footerR = GetFooterContent(doc, rtfWriter);
        //                        if (footerR != null)
        //                            doc.Footer = footerR;
        //                    }
        //                    if (settings.HasHeaderOnFirstPage)
        //                        doc.Open();
        //                    ////Compilazione documento
        //                    if (forErrorContent)
        //                        RenderErrorDocument(doc, type);
        //                    else
        //                        RenderDocument(doc, rtfWriter);
        //                    doc.Close();

        //                    break;
        //            }
        //            // DIVERSIFICARE IN BASE AL TIPO DI FILE !
        //            //ORA, SE si vogliono le intestazioni sulla prima pagina,
        //            //Quanto segue VA PRIMA di doc.Open

        //            ////SE mi vengono passati HEADER e/o FOOTER,
        //            ////li gestisco alla solita maniera, altrimenti si dovrà arrangiare chi implementa la compilazione dell'aggeggio.
        //            //if (Header != null || Footer == null)
        //            //{
        //            //    if (PageSettings.HasHeaderOnFirstPage == false)
        //            //    {
        //            //        //doc.Open();
        //            //    }
        //            //    ////Rivedere la "traduzione" in C#
        //            //    //MyPageEventHandler ev = New MyPageEventHandler(Me.CTRL_Title.HTML);
        //            //    //pdfWriter.PageEvent = ev;
        //            //    if (PageSettings.HasHeaderOnFirstPage == true)
        //            //    {
        //            //        //doc.Open();
        //            //    }
        //            //}   
        //            //Se il doc non è stato chiuso precedentemente, lo chiudo!
        //        }
        //        catch (Exception ex)
        //        {
        //            doc = null;
        //        }
        //        finally{
        //            if (doc != null && doc.IsOpen())
        //                doc.Close();
        //        }
        //        return doc;
        //    }
        //    //private Document ExportTo(Document doc, PageSettings settings, System.IO.Stream stream, ExportFileType type)
        //    //{
        //    //    try
        //    //    {
        //    //        switch (type)
        //    //        {
        //    //            case ExportFileType.pdf:
        //    //                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
        //    //                doc.Close();

        //    //                break;
        //    //            case ExportFileType.rtf:
        //    //                iTextSharp.text.rtf.RtfWriter2 rtfWriter = iTextSharp.text.rtf.RtfWriter2.GetInstance(doc, stream);
        //    //                HeaderFooter headerR = GetHeaderContent(doc, rtfWriter);
        //    //                if (headerR != null)
        //    //                    doc.Header = headerR;
        //    //                HeaderFooter footerR = GetFooterContent(doc, rtfWriter);
        //    //                if (footerR != null)
        //    //                    doc.Footer = footerR;

        //    //                doc.Open();
        //    //                doc.Close();

        //    //                break;
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        doc = null;
        //    //    }
        //    //    finally
        //    //    {
        //    //        if (doc != null && doc.IsOpen())
        //    //            doc.Close();
        //    //    }
        //    //    return doc;
        //    //}
        //#endregion

        //    protected abstract HeaderFooter GetHeaderContent(Document doc, PdfWriter pdfWriter);
        //    protected abstract HeaderFooter GetHeaderContent(Document doc, iTextSharp.text.rtf.RtfWriter2 rtfWriter);
        //    protected abstract HeaderFooter GetFooterContent(Document doc, PdfWriter pdfWriter);
        //    protected abstract HeaderFooter GetFooterContent(Document doc, iTextSharp.text.rtf.RtfWriter2 rtfWriter);
        //    protected abstract PdfPageEventHandler GetHeaderFooterContent(Document doc, PdfWriter pdfWriter);

        //    protected abstract void RenderDocument(Document doc, PdfWriter pdfWriter);
        //    protected abstract void RenderDocument(Document doc, iTextSharp.text.rtf.RtfWriter2 rtfWriter);
        //    protected abstract void RenderErrorDocument(Document doc, ExportFileType type);
        #endregion

        #region "Doc Controls"
            //protected void AddFontsToDocument(Document doc, PdfWriter pdfWriter)
            //{ 
            
            //}
            //protected static Font GetFont(String fontFamily,float size, Color color)
            //{
            //    return GetFont(fontFamily, size, Font.NORMAL, color);
            //}
            //protected static Font GetFont(String fontFamily, float size, int style, Color color)
            //{
            //    return FontFactory.GetFont(fontFamily, size, style, color);
            //}
            //protected static Paragraph GetPageTitle(String title, Font font)
            //{
            //      return  GetParagraph(title, font,18,30);
            //}
            //protected static Paragraph GetWhiteParagraph(Font font)
            //{
            //    return GetParagraph("\r\n", font, Element.ALIGN_LEFT, 12, 12);
            //}
            //protected static Paragraph GetParagraph(String text, Font font)
            //{
            //    return GetParagraph(text, font, Element.ALIGN_JUSTIFIED, 12, 12);
            //}
            //protected static Paragraph GetParagraph(String text, Font font, int align)
            //{
            //    return GetParagraph(text, font, align, 12, 12);
            //}
           
            //protected static Paragraph GetParagraph(String text, Font font, int align, float spacingBefore, float spacingAfter)
            //{
            //    Paragraph paragraph = GetParagraph(text, font, spacingBefore, spacingAfter);
            //    paragraph.Alignment = align;
            //    return paragraph;
            //}
            //protected static Paragraph GetParagraph(String text, Font font, float spacingBefore, float spacingAfter)
            //{
            //    Paragraph paragraph = new Paragraph(new Chunk(text, font));
            //    paragraph.SpacingBefore = spacingBefore;
            //    paragraph.SpacingAfter = spacingAfter;
            //    return paragraph;
            //}

            //protected static Table GetTable(int columns) {
            //    return GetTable(columns, 0);
            //}
            //protected static Table GetTable(int columns, int borderSize)
            //{
            //    float[] Tbwidths;
            //    if (columns <= 1)
            //    {
            //        columns = 1;
            //        Tbwidths = new float[] { 100 };
            //    }
            //    else { 
            //        int c = (100 / columns);
            //        Tbwidths = new float[columns];
            //        foreach (int i in (Enumerable.Range(1, columns).ToList())){
            //            Tbwidths[i] = c;
            //        }
            //        Tbwidths[columns - 1] = c + (100 % columns);
            //    }
            //    return GetTable(columns, Tbwidths, borderSize);
            //}
            //protected static Table GetTable(int columns,float[] Tbwidths, int borderSize)
            //{
            //    Table table = new Table(columns);
            //    table.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
            //    if (borderSize == 0)
            //    {
            //        table.Border = Rectangle.NO_BORDER;
            //        table.BorderWidth = 0;
            //    }
            //    else
            //        table.BorderWidth = (float)borderSize;
            //    table.Cellpadding = 3;
            //    table.Cellspacing = 0;
            //    table.TableFitsPage = true;
            //    table.WidthPercentage = 100;
            //    table.AutoFillEmptyCells = true;
            //    table.Widths = Tbwidths;

            //    return table;
            //}
        #endregion

        #region Generic Helper
        /// <summary>
        /// Elimina i tag che creano problemi durante la conversione
        /// </summary>
        /// <param name="Html">Stringa contenente il codice HTML</param>
        /// <remarks>
        /// Ad esempio il tag "HR" nella vecchia versione di iTextSharp viene ignorato,
        /// mentre nella nuova versione manda in crash il motore di conversione
        /// e viene quindi eliminato.
        /// </remarks>
        //public static void HtmlCheck(String Html)
        //{
        //    Html = System.Text.RegularExpressions.Regex.Replace(Html, "</?(h|H)(r|R)( )*/?>", string.Empty);
        //    //Il TAG <CENTER> è ignorato. Viene quindi sostituito da <p style="text-align: center">
        //    String center_open = "<div style=\"text-align: center\">";
        //    Html = System.Text.RegularExpressions.Regex.Replace(Html, "<(c|C)(e|E)(n|N)(t|T)(e|E)(r|R)( )*>", center_open);
        //    String center_close = "</div>";
        //    Html = System.Text.RegularExpressions.Regex.Replace(Html, "</(c|C)(e|E)(n|N)(t|T)(e|E)(r|R)( )*>", center_close);
        //}

        /// <summary>
        /// Calcola il numero di colonne di un template Header/Footer
        /// </summary>
        /// <param name="TmpHF"></param>
        /// <returns></returns>
        public static int ColumnCount(TemplVers_Export.DTO_HeaderFooter HeaderFooter)
        {
            int ColCount = 0;
            //Implement Header
            if (HeaderFooter == null)
            {
                ColCount = 0;
            }
            else
            {
                if (HeaderFooter.Left != null)
                {
                    ColCount += CountElement(HeaderFooter.Left);
                }
                if (HeaderFooter.Right != null)
                {
                    ColCount += CountElement(HeaderFooter.Right);
                }
                if (HeaderFooter.Center != null)
                {
                    ColCount += CountElement(HeaderFooter.Center);
                }
            }
            return ColCount;
        }

        private static int CountElement(TemplVers_Export.DTO_Element Element)
        {
            if (Element == null)
                return 0;
            else if(Element.GetType() == typeof(TemplVers_Export.DTO_ElementImageMulti))
            {
                try
                {
                    TemplVers_Export.DTO_ElementImageMulti DTO_EIM = (TemplVers_Export.DTO_ElementImageMulti)Element;
                    if (DTO_EIM != null && DTO_EIM.ImgElements != null && DTO_EIM.ImgElements.Count > 0)
                        return 1;
                    else
                        return 0;
                }
                catch
                { return 0; }

            }
            //else if (Element.GetType() == typeof(TemplVers_Export.DTO_ElementText))
            //{
            //    try
            //    {
            //        TemplVers_Export.DTO_ElementText DTO_ET = (TemplVers_Export.DTO_ElementText)Element;
            //        if (DTO_ET != null && !string.IsNullOrEmpty(DTO_ET.Text))
            //            return 1;
            //        else
            //            return 0;
            //    }
            //    catch
            //    { return 0; }
            //}
            else
                return 1;
        }

            public static String HtmlCheckFileName(String fileName)
            {
                //fileName = fileName.Replace(" ", "_");
                return ReplaceInvalidFileName(fileName);//HttpUtility.UrlPathEncode(fileName);
            }
            public static String ReplaceInvalidFileName(String filename)
            {
                string regex = string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())));
                System.Text.RegularExpressions.Regex removeInvalidChars = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

                return removeInvalidChars.Replace((filename.Replace(",", "_").Replace(" ", "_")), "_");
            }
        #endregion

        /// <summary>
        /// Recupera delle impostazioni "di default"...
        /// </summary>
        /// <returns>
        /// Dei settings "base", hardcoded, validi a livello di sistema. Una sorta di "New Settings()",
        /// che viene usata nei casi in cui i settings NON sono passati alle funzioni di render.
            /// Per maggiori dettagli vedere lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings
        /// </returns>
        public static TemplVers_Export.DTO_Settings GetDefaultPageSettings()
        {
            TemplVers_Export.DTO_Settings settings = new TemplVers_Export.DTO_Settings();

            settings.Author = "";
            settings.Creator = "";
            settings.Keywords = "";
            settings.Producer = "";
            settings.Subject = "";
            settings.Title = "";

            //Se Alpha = 0 => no background!
            settings.BackgroundAlpha = 0;
            settings.BackgroundBlue = 0;
            settings.BackgroundGreen = 0;
            settings.BackgroundRed = 0;

            //Se image path = "" || image non esiste => no sfondo 
            settings.BackgroundImagePath = "";
            settings.BackGroundImageFormat = DocTemplateVers.BackgrounImagePosition.Center;

            settings.Size = DocTemplateVers.PageSize.A4;

            //Si usano SOLO se PageSize == PageSize.custom !
            settings.Height = 0;
            settings.Width = 0;

            settings.MarginBottom = 50;
            settings.MarginLeft = 30;
            settings.MarginRight = 30;
            settings.MarginTop = 50;

            //Se ShowPageNumber == false => no numeri pagina
            //settings.ShowPageNumber = false;
            //settings.PageNumberAlignment = DocTemplateVers.ElementAlignment.BottomCenter;
            settings.PagePlacingMask = 1; //TUTTE
            settings.PagePlacingRange = ""; //Se TUTTE non viene valutato.

            //settings.HasHeaderOnFirstPage = true;
            //settings.Id
            //settings.IsActive

            return settings;
        }
    }
}
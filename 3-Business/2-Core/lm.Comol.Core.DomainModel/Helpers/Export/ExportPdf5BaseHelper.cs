using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp5.text;
using iTextSharp5.text.pdf;
using NHibernate.SqlCommand;
using Chunk = iTextSharp5.text.Chunk;
using Document = iTextSharp5.text.Document;
using Element = iTextSharp5.text.Element;
using Font = iTextSharp5.text.Font;
using FontFactory = iTextSharp5.text.FontFactory;
using IElement = iTextSharp5.text.IElement;
using Paragraph = iTextSharp5.text.Paragraph;
using Phrase = iTextSharp5.text.Phrase;
using Rectangle = iTextSharp5.text.Rectangle;


namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    [Serializable]
    public abstract class ExportPdf5BaseHelper : ExportBaseHelper
    {

        #region "Initializers"
        public static DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings GetDefaultPageSettings()
        {
            return ExportBaseHelper.GetDefaultPageSettings();
        }
        public static DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings GetPageSettings(
            Rectangle pageSize,
            float marginLeft,
            float marginRight,
            float marginTop,
            float marginBottom) //Boolean showPageNumber,
        //PagePlacingMask
        //PagePlacingRange
        {
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings = ExportBaseHelper.GetDefaultPageSettings();//GetDefaultPageSettings();

            settings.MarginBottom = marginBottom;
            settings.MarginLeft = marginLeft;
            settings.MarginRight = marginRight;
            settings.MarginTop = marginTop;
            //settings.ShowPageNumber = showPageNumber;
            settings.Height = pageSize.Height;
            settings.Width = pageSize.Width;

            return settings;
        }
        private static Document InitializeDocument(ref DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings)
        {
            // ATTENZIONE!
            // Nell'esportazione in PDF PRIMA di arrivare qui i margini dei settings vengono
            // REIMPOSTATI! Quindi i SETTINGS DEVONO essere già stati impostati!
            // E' possibile comunque recuperare i DefaultSettings direttamente qui,
            // MA questo comporta che nell'output PDF l'header andrà a sovrapporsi al contenuto.
            // Almeno in fase di TEST, quindi, ho lasciato l'eccezione!

            if (settings == null)
                settings = Export.ExportBaseHelper.GetDefaultPageSettings();

            //settings.Size != DocTemplateVers.PageSize.none || 
            if (settings.Size != DocTemplateVers.PageSize.custom)
            {
                lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.PageSizeValue PgSzV =
                    lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.GetSize(settings.Size, "px");
                settings.Width = PgSzV.Width;
                settings.Height = PgSzV.Height;
            }

            if (settings.Width < lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5))
            {
                settings.Width = lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5);
            }
            if (settings.Height < lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5))
            {
                settings.Height = lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure.cm_To_Px(5);
            }

            if ((settings.MarginLeft + settings.MarginRight) > settings.Width)
            {
                settings.MarginLeft = 0;
                settings.MarginRight = 0;
            }

            if ((settings.MarginTop + settings.MarginBottom) > settings.Height)
            {
                settings.MarginTop = 0;
                settings.MarginBottom = 0;
            }

            Rectangle Page = new Rectangle(settings.Width, settings.Height);
            if (settings.BackgroundAlpha > 0)
            {
                Page.BackgroundColor = new BaseColor(settings.BackgroundRed, settings.BackgroundGreen, settings.BackgroundBlue);
            }

            Document doc = new Document(Page, settings.MarginLeft, settings.MarginRight, settings.MarginTop, settings.MarginBottom);

            return doc;

        }
        #endregion

        #region "Export To file / Stream"
        #region "Web Export"

        public Document WebExport(
            String fileName,
            System.Web.HttpResponse webResponse)
        {
            return WebExport(false, fileName, GetDefaultPageSettings(), webResponse, null, null, "");
        }

        //Settings!
        public Document WebExport(
            String fileName,
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings Settings,
            System.Web.HttpResponse webResponse,
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures,
                string waterMark = "")
        {
            return WebExport(false, fileName, Settings, webResponse, null, Signatures, waterMark);
        }

        public Document WebExport(
            String fileName,
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings Settings,
            System.Web.HttpResponse webResponse,
            System.Web.HttpCookie cookie,
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures,
                string waterMark = "")
        {
            return WebExport(false, fileName, Settings, webResponse, cookie, Signatures, waterMark);
        }

        public Document WebExport(
            Boolean openCloseConnection, String fileName, System.Web.HttpResponse webResponse,
                string waterMark = "")
        {
            return WebExport(openCloseConnection, fileName, null, webResponse, null, null, waterMark);
        }

        public Document WebExport(
            Boolean openCloseConnection,
            String fileName,
            System.Web.HttpResponse webResponse,
            System.Web.HttpCookie cookie,
                string waterMark = "")
        {
            return WebExport(openCloseConnection, fileName, null, webResponse, cookie, null, waterMark);
        }
        public Document WebExport(
                Boolean openCloseConnection,
                String fileName,
                DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings,
                System.Web.HttpResponse webResponse,
                System.Web.HttpCookie cookie,
                string waterMark = "")
        {
            return WebExport(openCloseConnection, fileName, settings, webResponse, cookie, null, waterMark);
        }
        public Document WebExport(
            Boolean openCloseConnection,
            String fileName,
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings,
            System.Web.HttpResponse webResponse,
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures,
            string waterMark = "")
        {
            return WebExport(openCloseConnection, fileName, settings, webResponse, null, Signatures, waterMark);
        }
        #endregion

        #region "File Export"
        public Document FileExport(System.IO.FileStream fileStream)
        {
            return FileExport(GetDefaultPageSettings(), fileStream, null);
        }
        public Document FileExport(
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings,
            System.IO.FileStream fileStream,
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures)
        {
            return ExportTo(settings, fileStream, false, Signatures, "");
        }
        #endregion

        #region "Export"
        public Document WebExport(
            Boolean openCloseConnection,
            String fileName,
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings Settings,
            System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie,
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures,
            String waterMark)
        {
            if (openCloseConnection)
                webResponse.Clear();
            if (cookie != null)
                webResponse.AppendCookie(cookie);
            webResponse.AddHeader("Content-Disposition", "attachment; filename=" + HtmlCheckFileName(fileName) + "." + ExportFileType.pdf.ToString());
            webResponse.ContentType = "application/pdf";

            Document doc = ExportTo(Settings, webResponse.OutputStream, false, Signatures, waterMark);

            if (doc != null && openCloseConnection)
                webResponse.End();
            return doc;
        }
        public Document GetErrorDocument(
            Boolean openCloseConnection, Boolean addContentDisposition,
            String fileName,
            System.Web.HttpResponse webResponse)
        {
            return GetErrorDocument(openCloseConnection, addContentDisposition, fileName, webResponse, null, null, null);
        }
        public Document GetErrorDocument(
            Boolean openCloseConnection, Boolean addContentDisposition,
            String fileName,
            System.Web.HttpResponse webResponse,
            System.Web.HttpCookie cookie)
        {
            return GetErrorDocument(openCloseConnection, addContentDisposition, fileName, webResponse, cookie, null, null);
        }
        public Document GetErrorDocument(
            Boolean openCloseConnection, Boolean addContentDisposition,
            String fileName,
            System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie,
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings,
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures)
        {
            if (openCloseConnection)
                webResponse.Clear();
            if (cookie != null)
                webResponse.AppendCookie(cookie);
            if (addContentDisposition || openCloseConnection)
            {
                webResponse.AddHeader("Content-Disposition", "attachment; filename=" + HtmlCheckFileName(fileName) + "." + ExportFileType.pdf.ToString());
                webResponse.ContentType = "application/pdf";
            }

            Document doc = ExportTo(settings, webResponse.OutputStream, true, Signatures, "");

            if (doc != null && openCloseConnection)
                webResponse.End();
            return doc;
        }

        /// <summary>
        /// Effettiva creazione di TUTTO il documento. TUTTE le fuzioni passano da qui.
        /// </summary>
        /// <param name="ValSettings">I SETTINGS. INTERNAMENTE vengono CLONATI, in modo da evitare il ricalcolo dei margini passando lo stesso oggetto.</param>
        /// <param name="stream"></param>
        /// <param name="forErrorContent"></param>
        /// <param name="Signatures"></param>
        /// <returns></returns>
        private Document ExportTo(DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings ValSettings, System.IO.Stream stream, Boolean forErrorContent, IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures,
            string waterMark)
        {
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings = null;

            if (ValSettings != null)
            {
                try
                {
                    settings = (DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings)ValSettings.Clone();
                }
                catch (Exception ex)
                {

                }
            }



            Document doc = null;
            try
            {
                //MODIFICA DI MARGIN TOP E MARGIN BOTTOM!!!
                //Necessario creare tale oggetto PRIMA di inizializzare la pagina per avere i margini corretti.

                //NOTA - POSSIBILE BUG!!!

                // Nel caso la prima pagina non presenti Header e Footer
                // è da rivedere COME impostare i margini SENZA HEADER/FOOTER
                // ed impostarli correttamente per le pagine successive.
                // Al momento può anche andare così, ma ci sarà una maggiore spaziatura nella prima pagina.

                //NOTA - IMPLEMENTAZIONI FUTURE
                // NON è previsto il numero di pagina ed ancor meno il numero pagine totali,
                // per le quali sono necessari ulteriori approfondimenti.

                // Lo inizializzo qui ed ora,
                // perchè internamente vengono controllati i settings,
                // e settate le dimensioni a seconda del formato.
                doc = InitializeDocument(ref settings);



                float marginTop = settings.MarginTop;
                float marginBottom = settings.MarginTop;

                Pdf5PageEventHandler ev = new Pdf5PageEventHandler(
                    GetHeader(), GetFooter(),
                    ref settings, Signatures,
                    waterMark);

                //REIMPOSTO I BORDI!!! 
                // NON posso farlo prima, per altre logiche, tipo dimensione pagina.
                if (settings.MarginTop > doc.PageSize.Height / 3)
                    settings.MarginTop = doc.PageSize.Height / 3;

                if (settings.MarginBottom > doc.PageSize.Height / 3)
                    settings.MarginBottom = doc.PageSize.Height / 3;

                if (settings.MarginLeft > doc.PageSize.Width / 3)
                    settings.MarginLeft = doc.PageSize.Width / 3;

                if (settings.MarginRight > doc.PageSize.Width / 3)
                    settings.MarginRight = doc.PageSize.Width / 3;



                doc.SetMargins(settings.MarginLeft, settings.MarginRight, settings.MarginTop, settings.MarginBottom);

                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);

                pdfWriter.PageEvent = ev;
                doc.Open();

                //Non serve più, 
                //if (settings.HasHeaderOnFirstPage)
                //    doc.Open();

                ////Compilazione documento
                if (forErrorContent)
                    RenderErrorDocument(doc);
                else
                    RenderDocument(doc, pdfWriter);

                //Serve per poter mettere il footer sull'ultima pagina.
                //L'evento OnEndDocument non aggiunge nulla al documento...
                ev.LastPageEnd(pdfWriter, doc);

                doc.Close();
            }
            catch (Exception ex)
            {
                doc = null;
                if (stream != null)
                    stream.Close();
            }
            finally
            {
                if (doc != null && doc.IsOpen())
                    doc.Close();
            }
            return doc;
        }
        //private Document ExportTo(Document doc, PageSettings settings, System.IO.Stream stream, ExportFileType type)
        //{
        //    try
        //    {
        //        switch (type)
        //        {
        //            case ExportFileType.pdf:
        //                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, stream);
        //                doc.Close();

        //                break;
        //            case ExportFileType.rtf:
        //                iTextSharp.text.rtf.RtfWriter2 rtfWriter = iTextSharp.text.rtf.RtfWriter2.GetInstance(doc, stream);
        //                HeaderFooter headerR = GetHeaderContent(doc, rtfWriter);
        //                if (headerR != null)
        //                    doc.Header = headerR;
        //                HeaderFooter footerR = GetFooterContent(doc, rtfWriter);
        //                if (footerR != null)
        //                    doc.Footer = footerR;

        //                doc.Open();
        //                doc.Close();

        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        doc = null;
        //    }
        //    finally
        //    {
        //        if (doc != null && doc.IsOpen())
        //            doc.Close();
        //    }
        //    return doc;
        //}
        #endregion

        protected abstract DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetHeader();
        protected abstract DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetFooter();
        //protected abstract Pdf5PageEventHandler GetHeaderFooterContent(Document doc, PdfWriter pdfWriter);

        protected abstract void RenderDocument(Document doc, PdfWriter pdfWriter);

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="doc">Il documento</param>
        /// <remarks>IL DOCUMENTO non è APERTO SE non si vuole l'header nella prima pagina...</remarks>
        protected abstract void RenderErrorDocument(Document doc);
        #endregion

        #region "Doc Controls"
        protected void AddFontsToDocument(Document doc, PdfWriter pdfWriter)
        {

        }
        protected static Font GetFont(String fontFamily, float size, BaseColor color)
        {
            return GetFont(fontFamily, size, Font.NORMAL, color);
        }
        protected static Font GetFont(String fontFamily, float size, int style, BaseColor color)
        {
            return FontFactory.GetFont(fontFamily, size, style, color);
        }
        protected static Paragraph GetPageTitle(String title, Font font)
        {
            return GetParagraph(title, font, 18, 30);
        }
        protected static Paragraph GetWhiteParagraph(Font font)
        {
            return GetParagraph("\r\n", font, Element.ALIGN_LEFT, 12, 12);
        }
        protected static Paragraph GetParagraph(String text, Font font)
        {
            return GetParagraph(text, font, Element.ALIGN_JUSTIFIED, 12, 12);
        }
        protected static Paragraph GetParagraph(String text, Font font, int align)
        {
            return GetParagraph(text, font, align, 12, 12);
        }

        protected static Paragraph GetParagraph(String text, Font font, int align, float spacingBefore, float spacingAfter)
        {
            Paragraph paragraph = GetParagraph(text, font, spacingBefore, spacingAfter);
            paragraph.Alignment = align;
            return paragraph;
        }
        protected static Paragraph GetParagraph(String text, Font font, float spacingBefore, float spacingAfter)
        {
            Paragraph paragraph = new Paragraph(new Chunk(text, font));
            paragraph.SpacingBefore = spacingBefore;
            paragraph.SpacingAfter = spacingAfter;
            return paragraph;
        }
        protected static Phrase GetPhrase(String text, Font font)
        {
            return new Phrase(new Chunk(text, font));
        }

        protected static PdfPTable GetTable(int columns)
        {
            return GetTable(columns, 0);
        }
        protected static PdfPTable GetTable(int columns, int borderSize)
        {
            float[] Tbwidths;
            if (columns <= 1)
            {
                columns = 1;
                Tbwidths = new float[] { 100 };
            }
            else
            {
                int c = (100 / columns);
                Tbwidths = new float[columns];
                foreach (int i in (Enumerable.Range(0, columns).ToList()))
                {
                    Tbwidths[i] = c;
                }
                Tbwidths[0] = c + (100 % columns);
            }
            return GetTable(columns, Tbwidths, borderSize);
        }
        protected static PdfPTable GetTable(int columns, float[] Tbwidths, int borderSize)
        {
            PdfPTable table = new PdfPTable(columns);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            //table.DefaultHorizontalAlignment = Element.ALIGN_LEFT;


            if (borderSize == 0)
            {
                //table.Border = Rectangle.NO_BORDER;
                //table.BorderWidth = 0;
                table.DefaultCell.Border = 0;
            }
            else
                table.DefaultCell.Border = borderSize;
            //table.BorderWidth = (float)borderSize;



            //table.Cellpadding = 3;
            table.DefaultCell.Padding = 3;

            //table.Cellspacing = 0;
            //table.TableFitsPage = true;

            //table.WidthPercentage = 100;
            table.WidthPercentage = 100;

            //table.AutoFillEmptyCells = true;

            table.SetWidths(Tbwidths);
            //table.Widths = Tbwidths;

            return table;
        }
        #endregion

        public static PdfPCell HtmlToCell(String Html)
        {
            PdfPCell cell = new PdfPCell();

            List<IElement> AL_Content = new List<IElement>();
            Boolean Error = false;

            Html = lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.HtmlCheckPDF(Html);

            try
            {
                AL_Content = iTextSharp5.text.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(Html), new iTextSharp5.text.html.simpleparser.StyleSheet());


            }
            catch (Exception ex)
            {
                Error = true;
            }

            if (Error)
            {
                Error = true;

                Html = lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.RemoveHtmlTag(Html);

                try
                {
                    AL_Content = iTextSharp5.text.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(Html), null);
                    Error = false;
                }
                catch (Exception ex)
                {
                    cell = new PdfPCell(
                        new iTextSharp5.text.Paragraph(lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.RemoveHtmlTag(Html))
                    );
                }
            }

            if (!Error)
            {
                foreach (iTextSharp5.text.IElement element in AL_Content)
                {
                    cell.AddElement(element);
                }
            }

            cell.Border = 0;
            cell.BorderWidth = 0f;
            cell.Padding = 0f;

            return cell;
        }



        //private static IDocListener defDoc
        //{
        //    return 
        //}
        //public static PdfPTable GetStandardTable(int ColumnNum)
        //{
        //    PdfPTable pTable = new PdfPTable(ColumnNum);
        //    if(pTable.DefaultCell != null)
        //    {
        //        pTable.DefaultCell.Border = 0;
        //        pTable.DefaultCell.BorderWidth = 0;
        //        pTable.
        //    }

        //}
    }
}
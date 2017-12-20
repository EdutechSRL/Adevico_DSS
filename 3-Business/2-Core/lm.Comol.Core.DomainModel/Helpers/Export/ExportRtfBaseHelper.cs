using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using iTS = iTextSharp.text;

using System.IO;

// NOTE DI SVILUPPO - RIGHE DA AGGIUSTARE!
//  161              DocTemplate.TemplateHeaderFooter Header = null;// GetHeader();
//  162              DocTemplate.TemplateHeaderFooter Footer = null;// GetFooter();
//  273             //protected abstract DocTemplate.TemplateHeaderFooter GetHeader();
//  281             //protected abstract DocTemplate.TemplateHeaderFooter GetHeader();
//  284      protected abstract HeaderFooter GetHeaderContent(Document doc, iTextSharp.text.rtf.RtfWriter2 rtfWriter);
//  285             //protected abstract HeaderFooter GetFooterContent(Document doc, iTextSharp.text.rtf.RtfWriter2 rtfWriter);

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    [Serializable]
    public abstract class ExportRtfBaseHelper : ExportBaseHelper
    {

        #region "Initializers"
        public static DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings GetDefaultPageSettings()
        {
            return ExportBaseHelper.GetDefaultPageSettings();
        }

        public static DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings GetPageSettings(
            iTS.Rectangle pageSize, 
            float marginLeft, 
            float marginRight, 
            float marginTop,
            float marginBottom) //Boolean showPageNumber, 
        {
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings = GetDefaultPageSettings();

            settings.MarginBottom = marginBottom;
            settings.MarginLeft = marginLeft;
            settings.MarginRight = marginRight;
            settings.MarginTop = marginTop;
            //settings.ShowPageNumber = showPageNumber;
            settings.Height = pageSize.Height;
            settings.Width = pageSize.Width;
            return settings;
        }

        //adding -START-...
        private static iTS.Document InitializeDocument(ref DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings)
        {
            if (settings == null)
                settings = ExportBaseHelper.GetDefaultPageSettings();

            //settings.Size != DocTemplateVers.PageSize.none || 
            if (settings.Size != DocTemplateVers.PageSize.custom)
            {

                DocTemplateVers.Helpers.PageSizeValue PgSzV = DocTemplateVers.Helpers.Measure.GetSize(settings.Size, "px");
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


            return new iTS.Document(new iTS.Rectangle(settings.Width, settings.Height), settings.MarginLeft, settings.MarginRight, settings.MarginTop, settings.MarginBottom);
        }
        //adding -END-...

        //private static Document InitializeDocument(DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings)
        //{
        //    if (settings == null)
        //        settings = GetDefaultPageSettings();

        //    Document doc = new Document(new Rectangle(settings.Width, settings.Height), settings.MarginLeft, settings.MarginRight, settings.MarginTop, settings.MarginBottom);

        //    return doc;
        //}
        #endregion

        #region "Export To file / Stream"
        #region "Web Export"

        public iTS.Document WebExport(
            String fileName, 
            System.Web.HttpResponse webResponse)
        {
            return WebExport(false, fileName, null, webResponse, null);
        }

        public iTS.Document WebExport(
            String fileName, 
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings Settings,
            System.Web.HttpResponse webResponse)
        {
            return WebExport(false, fileName, Settings, webResponse, null);
        }

        public iTS.Document WebExport(
            String fileName, 
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings Settings,
            System.Web.HttpResponse webResponse, 
            System.Web.HttpCookie cookie)
        {
            return WebExport(false, fileName, Settings, webResponse, cookie);
        }

        public iTS.Document WebExport(
            Boolean openCloseConnection, 
            String fileName, 
            System.Web.HttpResponse webResponse)
        {
            return WebExport(openCloseConnection, fileName, null, webResponse, null);
        }

        public iTS.Document WebExport(
            Boolean openCloseConnection, 
            String fileName, 
            System.Web.HttpResponse webResponse, 
            System.Web.HttpCookie cookie)
        {
            return WebExport(openCloseConnection, fileName, null, webResponse, cookie);
        }

        public iTS.Document WebExport(
            Boolean openCloseConnection, 
            String fileName, 
            DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings,
            System.Web.HttpResponse webResponse)
        {
            return WebExport(openCloseConnection, fileName, settings, webResponse, null);
        }
        #endregion

        #region "File Export"
            public iTS.Document FileExport(System.IO.FileStream fileStream)
            {
                return FileExport(GetDefaultPageSettings(), fileStream);
            }
            public iTS.Document FileExport(DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings, System.IO.FileStream fileStream)
            {
                return ExportTo(settings,fileStream,false );
            }
        #endregion

        #region "Export"
            public iTS.Document WebExport(Boolean openCloseConnection, String fileName, DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                if (openCloseConnection)
                    webResponse.Clear();
                if (cookie != null)
                    webResponse.AppendCookie(cookie);
                webResponse.AddHeader("Content-Disposition",
                    "attachment; filename=" + HtmlCheckFileName(fileName) + ".rtf");
                // + ExportFileType.rtf.ToString());
                webResponse.ContentType = "application/rtf";

                iTS.Document doc = ExportTo(settings, webResponse.OutputStream, false);

                if (doc != null && openCloseConnection)
                    webResponse.End();
                return doc;
            }
            public iTS.Document GetErrorDocument(Boolean openCloseConnection, Boolean addContentDisposition, String fileName, System.Web.HttpResponse webResponse)
            {
                return GetErrorDocument(openCloseConnection, addContentDisposition,fileName, webResponse, null, GetDefaultPageSettings());
            }
            public iTS.Document GetErrorDocument(Boolean openCloseConnection, Boolean addContentDisposition, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                return GetErrorDocument(openCloseConnection, addContentDisposition, fileName, webResponse, cookie, GetDefaultPageSettings());
            }
            public iTS.Document GetErrorDocument(Boolean openCloseConnection, Boolean addContentDisposition, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie, DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings)
            {
                if (openCloseConnection)
                    webResponse.Clear();
                if (cookie!=null)
                    webResponse.AppendCookie(cookie);
                if (addContentDisposition || openCloseConnection)
                {
                    webResponse.AddHeader("Content-Disposition",
                        "attachment; filename=" + HtmlCheckFileName(fileName) + ".rtf");
                    // + ExportFileType.rtf.ToString());
                    webResponse.ContentType = "application/rtf";
                }

                //if (settings == null)
                //    settings = GetDefaultPageSettings();

                iTS.Document doc = ExportTo(settings, webResponse.OutputStream, true);

                if (doc != null && openCloseConnection)
                    webResponse.End();
                return doc;
            }


            private iTS.Document ExportTo(DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings, System.IO.Stream stream, Boolean forErrorContent)
            {
                iTS.Document doc = null;
                try
                {
                   
                    doc = InitializeDocument(ref settings);

                    DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Header = GetHeader();
                    DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Footer = GetFooter();

                    float ContentWidth = settings.Width - (settings.MarginLeft + settings.MarginRight);


                    iTS.rtf.RtfWriter2 rtfWriter = iTS.rtf.RtfWriter2.GetInstance(doc, stream);

                    doc.Open();



                    //Add Header
                    iTS.Table table = null;
                    if (Header != null)
                    {
                        table = ConvertHeaderFooter(Header, ContentWidth);
                        if (table != null)
                        {
                            try
                            {
                                doc.Add(table);
                            }
                            catch { }
                            
                        }
                            
                    }

                    ////Compilazione documento

                    if (forErrorContent)
                        RenderErrorDocument(doc);
                    else
                        RenderDocument(doc, rtfWriter);

                    if (Footer != null)
                    {
                        table = ConvertHeaderFooter(Footer, ContentWidth);
                        if (table != null)
                        {
                            try
                            { doc.Add(table); }
                            catch { }
                        }
                            
                    }


                    doc.Close();
                }
                catch (Exception ex)
                {
                    doc = null;
                }
                finally
                {
                    if (doc != null && doc.IsOpen())
                        doc.Close();
                }
                return doc;
                //DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Header = GetHeader();
                ////DocTemplate.TemplateHeaderFooter Header = GetHeader();
                ////DocTemplate.TemplateHeaderFooter Footer = GetFooter();
                //DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter Footer = GetFooter();
                
                //float ContentWidth = settings.Width - (settings.MarginLeft + settings.MarginRight);

                //Document doc = null;
                //try
                //{
                //    doc = InitializeDocument(settings);

                //    iTextSharp.text.rtf.RtfWriter2 rtfWriter = iTextSharp.text.rtf.RtfWriter2.GetInstance(doc, stream);

                //    doc.Open();

                    

                //    //Add Header
                //    iTextSharp.text.Table table = null;
                //    if (!settings.HTMLheader && Header != null)
                //    {
                //        table = ConvertHeaderFooter(Header, ContentWidth);
                //        if (table != null)
                //            doc.Add(table);
                //    }
                    
                //    ////Compilazione documento
                    
                //    if (forErrorContent)
                //        RenderErrorDocument(doc);
                //    else
                //        RenderDocument(doc, rtfWriter);

                //    if (!settings.HTMLfooter && Footer != null) {
                //        table = ConvertHeaderFooter(Footer, ContentWidth);
                //        if (table!=null)
                //            doc.Add(table);
                //    }
                        

                //    doc.Close();
                //}
                //catch (Exception ex)
                //{
                //    doc = null;
                //}
                //finally{
                //    if (doc != null && doc.IsOpen())
                //        doc.Close();
                //}
                //return doc;
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


            protected abstract void RenderDocument(iTS.Document doc, iTextSharp.text.rtf.RtfWriter2 rtfWriter);
            protected abstract void RenderErrorDocument(iTS.Document doc);

            /// <summary>
            /// Restituirà l'Header da usare
            /// </summary>
            /// <returns>
            /// null se superfluo
            /// </returns>
            protected abstract DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetHeader();
        //DocTemplate.TemplateHeaderFooter GetHeader();

            /// <summary>
            /// Restituirà il Footer da usare
            /// </summary>
            /// <returns>
            /// null se superfluo
            /// </returns>
            protected abstract DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetFooter();
  

        #endregion

            #region Template Header/Footer to iTextSharp.Table

                public static iTextSharp.text.Table ConvertHeaderFooter(DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter HeaderFooter, float ContentWidth)
                {
                    int Col = ColumnCount(HeaderFooter);


                    

                    iTextSharp.text.Table table = null;

                    if (Col > 0)
                    {
                        float MaxWidth = ContentWidth / Col;

                        table = new iTextSharp.text.Table(Col);

                        if (HeaderFooter.Left != null)
                        {
                            table.AddCell(RenderElement(HeaderFooter.Left, MaxWidth));
                        }

                        if (HeaderFooter.Center != null)
                        {
                            table.AddCell(RenderElement(HeaderFooter.Center, MaxWidth));
                        }

                        if (HeaderFooter.Right != null)
                        {
                            table.AddCell(RenderElement(HeaderFooter.Right, MaxWidth));
                        }
                    }

                    return table;
                }

                /// <summary>
                /// Definisce quale function utilizzare per la conversione
                /// </summary>
                /// <param name="Element">
                /// Elemento Template generico (testo o immagine, evntualmente estendibile)
                /// </param>
                /// <returns>
                /// Una cella iTextSharp con gli elementi relativi
                /// </returns>
                private static iTextSharp.text.Cell RenderElement(DocTemplateVers.Domain.DTO.ServiceExport.DTO_Element Element, float MaxWidth)
                {
                    if (Element.GetType() == typeof(DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText))
                    {
                        return RenderElement((DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText)Element, MaxWidth);
                    }
                    else if (Element.GetType() == typeof(DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage))
                    {
                        return RenderElement((DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage)Element, MaxWidth);
                    }
                    else if (Element.GetType() == typeof(DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti))
                    {
                        return RenderElement((DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti)Element, MaxWidth);
                    }
                    return null;
                }

                /// <summary>
                /// Renderizza un elemento testuale in una cella iTextSharp.text.
                /// </summary>
                /// <param name="TxtElement">
                /// Elemento testuale
                /// </param>
                /// <returns>
                /// iTextSharp.text.Cell con i contenuti opportuni
                /// </returns>
                private static iTextSharp.text.Cell RenderElement(DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementText TxtElement, float MaxWidth)
                {

                    iTextSharp.text.Cell cell;// = new iTextSharp5.text.Cell();
                    if (TxtElement.IsHTML)
                    {
                        cell = HtmlToCell(TxtElement.Text);
                        //HtmlCheck(TxtElement.Text);

     //                   cell = new iTextSharp.text.Cell();

     //                   System.Collections.ArrayList AL_Content =
     //iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
     //new System.IO.StringReader(RtfHelper.HtmlCheckRtf(TxtElement.Text)),
     //RtfHelper.GetStyles());

     //                   for (int j = 0; j < AL_Content.Count; j++)
     //                   {
     //                       cell.AddElement(CheckconvertedElement((iTextSharp.text.IElement)AL_Content[j]));
     //                   }
                    }
                    else
                    {
                        cell = new iTextSharp.text.Cell(
                            new iTextSharp.text.Paragraph(TxtElement.Text)
                            );

                        
                       

                        //switch (TxtElement.Alignment)
                        //{
                        //    case Helpers.Export.ElementAlignment.BottomCenter:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER; // = iTextSharp5.text.Cell.ALIGN_CENTER;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                        //        break;
                        //    case Helpers.Export.ElementAlignment.BottomLeft:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                        //        break;
                        //    case Helpers.Export.ElementAlignment.BottomRight:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                        //        break;

                        //    case Helpers.Export.ElementAlignment.MiddleCenter:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        //        break;
                        //    case Helpers.Export.ElementAlignment.MiddleLeft:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        //        break;
                        //    case Helpers.Export.ElementAlignment.MiddleRight:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        //        break;

                        //    case Helpers.Export.ElementAlignment.TopCenter:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                        //        break;
                        //    case Helpers.Export.ElementAlignment.TopLeft:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                        //        break;
                        //    case Helpers.Export.ElementAlignment.TopRight:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                        //        break;

                        //    default:
                        //        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                        //        cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                        //        break;
                        //}


                    }

                    if (cell != null)
                    {
                        RtfHelper.SetAlignment(ref cell, TxtElement.Alignment);
                        cell.Border = 0;
                    }
                        

                    return cell;
                }

                /// <summary>
                /// Renderizza un elemento immagine in una cella iTextSharp.text.
                /// </summary>
                /// <param name="ImgElement">
                /// Elemento immagine. NOTA: il percorso dev'essere ASSOLUTO, ad esempio C:\img\img.gif
                /// </param>
                /// <returns>
                /// iTextSharp.text.Cell con i contenuti opportuni
                /// </returns>
                private static iTextSharp.text.Cell RenderElement(DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage ImgElement, float MaxWidth)
                {
                    iTextSharp.text.Cell cell = new iTextSharp.text.Cell();
                    cell.Border = 0;
                    RtfHelper.SetAlignment(ref cell, ImgElement.Alignment);
                    //cell.Width = MaxWidth;

                    iTextSharp.text.Image img;
                    try
                    {
                        img = iTextSharp.text.Image.GetInstance(ImgElement.Path);


                        if (ImgElement.Width > 0 && ImgElement.Height > 0)
                            img.ScaleAbsolute(ImgElement.Width, ImgElement.Height);
                        else if (ImgElement.Width > 0)
                            img.ScaleAbsoluteWidth(ImgElement.Width);
                        else if (ImgElement.Height > 0)
                            img.ScaleAbsoluteHeight(ImgElement.Height);

                        if (img.Width > MaxWidth)
                            img.ScaleAbsoluteWidth(MaxWidth);
                        
                        cell.AddElement(img);
                    }
                    catch (Exception ex)
                    {
                        //cell = null;
                    }

                    return cell;

                    //return new iTextSharp.text.Cell(new iTextSharp.text.Paragraph("NOT IMPLEMENT!"));
                }


                /// <summary>
                /// Renderizza una lista di immagini, come quelle del FOOTER delle SKIN!
                /// </summary>
                /// <param name="MultiImgElement">
                ///     Elemento generico con le immagini.
                ///     Eventualmente rivedere in caso di necessità...
                /// </param>
                /// <returns></returns>
                private static iTextSharp.text.Cell RenderElement(DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti MultiImgElement, float MaxWidth)
                {
                    iTextSharp.text.Cell cell = new iTextSharp.text.Cell();
                    cell.Border = 0;
                    RtfHelper.SetAlignment(ref cell, MultiImgElement.Alignment);

                    foreach (DocTemplateVers.Domain.DTO.ServiceExport.DTO_ElementImage ImgElement in MultiImgElement.ImgElements)
                    {
                        iTextSharp.text.Image img;
                        try
                        {
                            img = iTextSharp.text.Image.GetInstance(ImgElement.Path);

                            if (ImgElement.Width > 0 && ImgElement.Height > 0)
                                img.ScaleAbsolute(ImgElement.Width, ImgElement.Height);
                            else if (ImgElement.Width > 0)
                                img.ScaleAbsoluteWidth(ImgElement.Width);
                            else if (ImgElement.Height > 0)
                                img.ScaleAbsoluteHeight(ImgElement.Height);

                            if (img.Width > MaxWidth)
                                img.ScaleAbsoluteWidth(MaxWidth);

                            cell.AddElement(img);
                        }
                        catch (Exception ex)
                        {
                            //cell = null;
                        }
                    }
                    
                    return cell;
                }

                private static void SetAlignment(ref iTextSharp.text.Cell cell, Helpers.Export.ElementAlignment Alignment)
                {
                    switch (Alignment)
                    {
                        case Helpers.Export.ElementAlignment.BottomCenter:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_CENTER; // = iTextSharp5.text.Cell.ALIGN_CENTER;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_BOTTOM;
                            break;
                        case Helpers.Export.ElementAlignment.BottomLeft:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_BOTTOM;
                            break;
                        case Helpers.Export.ElementAlignment.BottomRight:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_BOTTOM;
                            break;

                        case Helpers.Export.ElementAlignment.MiddleCenter:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_CENTER;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_MIDDLE;
                            break;
                        case Helpers.Export.ElementAlignment.MiddleLeft:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_MIDDLE;
                            break;
                        case Helpers.Export.ElementAlignment.MiddleRight:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_MIDDLE;
                            break;

                        case Helpers.Export.ElementAlignment.TopCenter:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_CENTER;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                            break;
                        case Helpers.Export.ElementAlignment.TopLeft:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                            break;
                        case Helpers.Export.ElementAlignment.TopRight:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                            break;

                        default:
                            cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                            cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                            break;
                    }
                }
            #endregion

            #region "Doc Controls"
            //    protected void AddFontsToDocument(iTS.Document doc, iTS.pdf.PdfWriter pdfWriter)
            //{ 
            
            //}
                protected static iTS.Font GetFont(String fontFamily, float size, iTS.Color color)
            {
                return GetFont(fontFamily, size, iTS.Font.NORMAL, color);
            }
                protected static iTS.Font GetFont(String fontFamily, float size, int style, iTS.Color color)
            {
                return iTS.FontFactory.GetFont(fontFamily, size, style, color);
            }
                protected static iTS.Paragraph GetPageTitle(String title, iTS.Font font)
            {
                  return  GetParagraph(title, font,18,30);
            }
                protected static iTS.Paragraph GetWhiteParagraph(iTS.Font font)
            {
                return GetParagraph("\r\n", font, iTS.Element.ALIGN_LEFT, 12, 12);
            }
                protected static iTS.Paragraph GetParagraph(String text, iTS.Font font)
            {
                return GetParagraph(text, font, iTS.Element.ALIGN_JUSTIFIED, 12, 12);
            }
                protected static iTS.Paragraph GetParagraph(String text, iTS.Font font, int align)
            {
                return GetParagraph(text, font, align, 12, 12);
            }

                protected static iTS.Paragraph GetParagraph(String text, iTS.Font font, int align, float spacingBefore, float spacingAfter)
            {
                iTS.Paragraph paragraph = GetParagraph(text, font, spacingBefore, spacingAfter);
                paragraph.Alignment = align;
                return paragraph;
            }
                protected static iTS.Paragraph GetParagraph(String text, iTS.Font font, float spacingBefore, float spacingAfter)
            {
                iTS.Paragraph paragraph = new iTS.Paragraph(new iTS.Chunk(text, font));
                paragraph.SpacingBefore = spacingBefore;
                paragraph.SpacingAfter = spacingAfter;
                return paragraph;
            }
                protected static iTS.Phrase GetPhrase(String text, iTS.Font font)
            {
                return new iTS.Phrase(new iTS.Chunk(text, font));
            }

                protected static iTS.Table GetTable(int columns)
                {
                return GetTable(columns, 0);
            }
                protected static iTS.Table GetTable(int columns, int borderSize)
            {
                float[] Tbwidths;
                if (columns <= 1)
                {
                    columns = 1;
                    Tbwidths = new float[] { 100 };
                }
                else { 
                    int c = (100 / columns);
                    Tbwidths = new float[columns];
                    foreach (int i in (Enumerable.Range(1, columns).ToList())){
                        Tbwidths[i] = c;
                    }
                    Tbwidths[columns - 1] = c + (100 % columns);
                }
                return GetTable(columns, Tbwidths, borderSize);
            }
                protected static iTS.Table GetTable(int columns, float[] Tbwidths, int borderSize)
            {
                iTS.Table table = new iTS.Table(columns);
                table.DefaultHorizontalAlignment = iTS.Element.ALIGN_LEFT;
                if (borderSize == 0)
                {
                    table.Border = iTS.Rectangle.NO_BORDER;
                    table.BorderWidth = 0;
                }
                else
                    table.BorderWidth = (float)borderSize;
                table.Cellpadding = 3;
                table.Cellspacing = 0;
                table.TableFitsPage = true;

                table.WidthPercentage = 100;
                table.AutoFillEmptyCells = true;
                table.Widths = Tbwidths;

                return table;
            }
            

            /// <summary>
            /// Aggiunge nel flusso doc corrente gli elementi convertiti da una stringa HTML...
            /// </summary>
            /// <param name="doc">Il document corrente a cui aggiungere gli elementi</param>
            /// <param name="html">Il codice HTML da aggiungere</param>
            /// <remarks>
            ///     1. TESTARE!!!
            ///     2. Generalizzare. Questa funzione lavora su Document,
            ///     altre invece lavorano su 
            /// </remarks>
                protected static void AddHTMLcontent(iTS.Document doc, string html)
                {

                System.Collections.ArrayList AL_Content = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(RtfHelper.HtmlCheckRtf(html)), RtfHelper.GetStyles());

                for (int j = 0; j < AL_Content.Count; j++)
                {
                    //Eventuali funzioni per la correzine ad esempio delle Liste...
                    doc.Add(CheckconvertedElement((iTextSharp.text.IElement)AL_Content[j]));
                }
            }

            /// <summary>
            /// Controlla gli elementi convertiti ed effettua eventuali correzioni,
            /// es: OL/UL
            /// </summary>
            /// <param name="element">
            /// La lista di elementi convertiti.
            /// </param>
            /// <remarks>
            /// AGGIUNGERE controllo sulle liste,
            /// in RTF ad ogni LI aggiunge un LI vuoto...
            /// </remarks>
            protected static iTextSharp.text.IElement CheckconvertedElement(iTextSharp.text.IElement element)
            {
                return element;
            }

        #endregion

            public static iTS.Cell HtmlToCell(String Html)
            {
                iTS.Cell cell = new iTextSharp.text.Cell();

                Boolean Error = false;
                //                iTextSharp.text.html.simpleparser.StyleSheet styles =
                //new iTextSharp.text.html.simpleparser.StyleSheet();
                //                aaa
                //                styles.LoadTagStyle("ul", "indent", "10");
                //                styles.LoadTagStyle("ol", "indent", "10");

                Html = lm.Comol.Core.DomainModel.Helpers.Export.RtfHelper.HtmlCheckRtf(Html);
                System.Collections.ArrayList AL_Content = new System.Collections.ArrayList();

                try
                {
                    AL_Content = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
                        new System.IO.StringReader(Html),
                        lm.Comol.Core.DomainModel.Helpers.Export.RtfHelper.GetStyles());
                }
                catch
                {
                    Error = true;
                }


                if (Error)
                {
                    Error = false;
                    Html = lm.Comol.Core.DomainModel.Helpers.Export.RtfHelper.RemoveHtmlTag(Html);
                    try
                    {
                        AL_Content = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(
                        new System.IO.StringReader(Html),
                        lm.Comol.Core.DomainModel.Helpers.Export.RtfHelper.GetStyles());
                    }
                    catch
                    {
                        Error = true;
                    }
                }

                if (Error)
                {
                    cell.AddElement(new iTS.Paragraph(Html));
                }
                else
                {
                    for (int j = 0; j < AL_Content.Count; j++)
                    {
                        try
                        {
                            iTS.IElement element = CheckconvertedElement((iTS.IElement)AL_Content[j]);
                            if (element != null)
                            {
                                cell.AddElement(element);
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                return cell;
            }
    }
}
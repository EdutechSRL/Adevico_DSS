using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using lm.Comol.Core.File;
//using lm.Comol.Core.DomainModel.DocTemplateVers//.Export;
using lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;
using iTS = iTextSharp5.text;
using iTextSharp5.text.pdf;
using lm.Comol.Core.DomainModel.DocTemplateVers;
using lm.Comol.Core.DomainModel.Helpers;

namespace lm.Comol.Core.BaseModules.DocTemplate.Helpers
{
    public class HelperExportPDF : lm.Comol.Core.DomainModel.Helpers.Export.ExportPdf5BaseHelper //lm.Comol.Core.DomainModel.Helpers.Export.ExportPdf5BaseHelper
    {
        public Boolean ExportToPdf(
            DTO_Template Template,
            String clientFileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            if (Template == null)
            {
                return false;
            }

            _template = Template;

            webResponse.Clear();
            //Render effettivo...
            try
            {
                base.WebExport(clientFileName, _template.Settings, webResponse, cookie, Template.Signatures);
                webResponse.End();
            }
            catch (Exception ex)
            {
                string err = ex.ToString(); //DEBUG!

                return false;
            }

            return true;
        }

        public Boolean ExportToPdf(DTO_Template template, Boolean forWebDownload, String webFileName, Boolean saveToFile, String storeFileName, Boolean openCloseConnection, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            //DTO_Settings Sett1 = (DTO_Settings)template.Settings.Clone();
            //DTO_Settings Sett2 = (DTO_Settings)template.Settings.Clone();

            Boolean response = false;
            iTextSharp5.text.Document doc = null;
            _template = template;
            if (forWebDownload)
            {
                //doc = WebExport(openCloseConnection, webFileName, template.Settings, webResponse, cookie, template.Signatures);
                doc = WebExport(openCloseConnection, webFileName, template.Settings, webResponse, cookie, template.Signatures, "");
                if (doc == null)
                {
                    GetErrorDocument(openCloseConnection, false, webFileName, webResponse, cookie, template.Settings, template.Signatures);
                    return false;
                }
                else if (saveToFile && !String.IsNullOrEmpty(storeFileName))
                    //SavePDFtoFile(storeFileName, template.Settings, template.Signatures);
                    SavePDFtoFile(storeFileName, template.Settings, template.Signatures);
            }
            else if (saveToFile && !String.IsNullOrEmpty(storeFileName))
                doc = SavePDFtoFile(storeFileName, template.Settings, template.Signatures);
            if (!response)
                response = (doc != null);
            return response;

        }

        public Boolean ExportToFile(DTO_Template template, String storeFileName)
        {
            iTextSharp5.text.Document doc = null;
            _template = template;
            if (!String.IsNullOrEmpty(storeFileName))
                doc = SavePDFtoFile(storeFileName, template.Settings, template.Signatures);
            return (doc != null);
        }

        //Se Setting == null, viene preso quello di default!
        private iTextSharp5.text.Document SavePDFtoFile(String storeFileName, lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings,
            IList<lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> signatures)
        {
            iTS.Document doc = null;
            System.IO.FileStream stream = null;
            Impersonate oImpersonate = new Impersonate();
            Boolean wasImpersonated = Impersonate.isImpersonated();
            try
            {
                if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                    return null;
                else
                {
                    stream = new System.IO.FileStream(storeFileName, System.IO.FileMode.Create);
                    if (stream != null)
                        doc = FileExport(settings, stream, signatures);
                }
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();
                if (lm.Comol.Core.File.Exists.File(storeFileName))
                    lm.Comol.Core.File.Delete.File(storeFileName);
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
                doc = null;
            }
            finally
            {
                if (!wasImpersonated)
                { oImpersonate.UndoImpersonation(); }
            }
            return doc;
        }

        private DTO_Template _template { get; set; }

        protected override DTO_HeaderFooter GetHeader()
        {
            return _template.Header;
        }

        protected override DTO_HeaderFooter GetFooter()
        {
            return _template.Footer;
        }

        protected override void RenderDocument(iTS.Document doc, PdfWriter pdfWriter)
        {
            PdfPTable contentTable = new iTS.pdf.PdfPTable(1);

            contentTable.TotalWidth = _template.Settings.Width - (_template.Settings.MarginLeft + _template.Settings.MarginRight);

            contentTable.AddCell(RenderBody());

            doc.Add(contentTable);

        }

        protected override void RenderErrorDocument(iTS.Document doc)
        {
            PdfPTable contentTable = new PdfPTable(1);

            contentTable.TotalWidth = _template.Settings.Width - (_template.Settings.MarginLeft + _template.Settings.MarginRight);

            contentTable.AddCell(RenderBody());

            doc.Add(contentTable);
        }

        public iTextSharp5.text.Document GetErrorDocument(DTO_Template template, Boolean openCloseConnection, Boolean addContentDisposition, String fileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            if (template == null)
                template = new DTO_Template();

            _template = template;
            return GetErrorDocument(openCloseConnection, addContentDisposition, fileName, webResponse, cookie);
        }

        private PdfPCell RenderBody()
        {
            PdfPCell cell;// = new iTS.Cell();
            if (_template.Body.IsHTML)
            {
                cell = HtmlToCell(_template.Body.Text);
            }
            else
            {
                cell = new PdfPCell(
                    new iTS.Paragraph(_template.Body.Text)
                    );

                //SetAlignment(ref cell, TxtElement.Alignment);

            }

            cell.Border = 0;
            return cell;
        }

        //public static PdfPCell HtmlToCell(String Html)
        //{
        //    PdfPCell cell = new PdfPCell();

        //    List<iTS.IElement> AL_Content = new List<iTS.IElement>();
        //    Boolean Error = false;

        //    Html = lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.HtmlCheckPDF(Html);

        //    try
        //    {
        //        AL_Content = iTS.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(Html), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error = true;
        //    }

        //    if (Error)
        //    {
        //        Error = true;

        //        Html = lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.RemoveHtmlTag(Html);

        //        try
        //        {
        //            AL_Content = iTS.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(Html), null);
        //            Error = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            cell = new PdfPCell(
        //                new iTS.Paragraph(lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.RemoveHtmlTag(Html))
        //            );
        //        }
        //    }

        //    if (!Error)
        //    {
        //        foreach (iTS.IElement element in AL_Content)
        //        {
        //            cell.AddElement(element);
        //        }
        //    }

        //    return cell;
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using iTextSharp.text.pdf;
using iTextSharp5.text;
using iTextSharp5.text.html;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers.Export;
using Org.BouncyCastle.Security;
using PdfPCell = iTextSharp5.text.pdf.PdfPCell;
using PdfPTable = iTextSharp5.text.pdf.PdfPTable;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public class HelperExportToPDF : lm.Comol.Core.DomainModel.Helpers.Export.ExportPdf5BaseHelper

    {
        private Dictionary<SubmissionTranslations, string> Translations { get; set; }
        private String FontFamilyname { get; set; }
        private Font BaseFont { get; set; }
        private dtoExportSubmission Settings { get; set; }

        private CallPrintSettings PrintSettings { get; set; }
        private lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData phData { get; set; }


        private lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template Template { get; set; }

        public HelperExportToPDF() {
            Translations = new Dictionary<SubmissionTranslations, string>();
            FontFamilyname = FontFactory.TIMES_ROMAN;
        }

        public HelperExportToPDF(
            Dictionary<SubmissionTranslations, string> translations,
            lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template)
            : this(translations, "", template)
        {
        }
        public HelperExportToPDF(Dictionary<SubmissionTranslations, string> translations, String fontFamily, lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template template)
        {
            Translations = translations;
            Template = template;
            if (!String.IsNullOrEmpty(fontFamily))
            {
                FontFamilyname = fontFamily;
                Font font = null;
                try
                {
                    font = GetFont(fontFamily, 12,BaseColor.BLACK);
                    if (font != null)
                        BaseFont = font;
                }
                catch (Exception ex)
                {

                }
            }
            if (BaseFont ==null){
                BaseFont = GetFont(FontFactory.TIMES_ROMAN, 12, BaseColor.BLACK);
                if (BaseFont !=null)
                    FontFamilyname = BaseFont.Familyname;
            }
        }

        #region "Implemented"
        protected override lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetFooter()
            {
                return Template.Footer;
            }
        protected override lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_HeaderFooter GetHeader()
            {
                return Template.Header;
            }
            protected override void RenderErrorDocument(Document doc)
            {
                doc.Add(GetPageTitle(Translations[SubmissionTranslations.FileCreationError], GetFont(ItemType.Title)));
            }

            protected override void RenderDocument(iTextSharp5.text.Document doc, iTextSharp5.text.pdf.PdfWriter pdfWriter)
            {
                dtoExportSubmission settings = Settings;
                if (settings.ForCompile)
                {
                    //ExportSubmissionTagReplace(ref doc, settings.Call, settings.Submitter, settings.RequiredFiles, settings.Sections,settings.PrintBy, Translations);
                    
                    ExportSubmissionTagReplace(
                        ref doc,
                        settings.Submission,
                        settings.RequiredFiles,
                        settings.Sections,
                        Translations,
                        false,
                        this.phData);

                    //ExportSubmission(doc, settings.Call, settings.Submitter, settings.RequiredFiles, settings.Sections, settings.PrintBy, Translations);
                }
                    
                else
                {
                    //ExportSubmission(doc, settings.Submission, settings.RequiredFiles, settings.Sections, settings.PrintBy, Translations);
                    ExportSubmissionTagReplace(
                        ref doc, 
                        settings.Submission, 
                        settings.RequiredFiles, 
                        settings.Sections,
                        Translations, 
                        false,
                        this.phData);
                }
                    
                    
            }

 
        #endregion

        #region "Export Submission"
            public Document Submission(
                Boolean openCloseConnection, 
                UserSubmission submission, 
                String fileName, List<dtoCallSubmissionFile> requiredFiles, 
                List<dtoCallSection<dtoSubmissionValueField>> sections, 
                litePerson person, 
                System.Web.HttpResponse webResponse,
                System.Web.HttpCookie cookie,
                CallPrintSettings printSettings,
                lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData PlaceHolderData)
            {
                return Submission(
                    openCloseConnection,
                    submission, 
                    "", 
                    fileName, 
                    requiredFiles, 
                    sections, 
                    person, 
                    webResponse, 
                    cookie,
                    printSettings,
                    PlaceHolderData);
            }
            public Document Submission(
                Boolean openCloseConnection, 
                UserSubmission submission, 
                String clientFilename, 
                String fileName, 
                List<dtoCallSubmissionFile> requiredFiles, 
                List<dtoCallSection<dtoSubmissionValueField>> sections, 
                litePerson person, System.Web.HttpResponse webResponse,
                System.Web.HttpCookie cookie,
                CallPrintSettings printSettings,
                lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData PlaceHolderData)
            {
                dtoExportSubmission settings = new dtoExportSubmission() { ClientFilename = clientFilename, Filename = fileName, PrintBy = person, RequiredFiles = requiredFiles, Sections = sections, Submission = submission };

                return Submission(
                    openCloseConnection, 
                    settings, 
                    webResponse, 
                    cookie,
                    printSettings,
                    PlaceHolderData);
            }
            public Document SubmissionToFile(
                Boolean openCloseConnection, 
                UserSubmission submission, 
                String fileName, 
                List<dtoCallSubmissionFile> requiredFiles, 
                List<dtoCallSection<dtoSubmissionValueField>> sections, 
                litePerson person,
                CallPrintSettings printSettings,
                lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData placeHolderData)
            {
                dtoExportSubmission settings = new dtoExportSubmission() { Filename = fileName, PrintBy = person, RequiredFiles = requiredFiles, Sections = sections, Submission = submission };

                return Submission(
                    openCloseConnection, 
                    settings, 
                    null, 
                    null, 
                    printSettings,
                    placeHolderData
                    );
            }
            public Document SubmissionToCompile(
                Boolean openCloseConnection, 
                BaseForPaper call, 
                SubmitterType submitter, 
                String clientFilename, 
                List<dtoCallSubmissionFile> requiredFiles, 
                List<dtoCallSection<dtoSubmissionValueField>> sections, 
                litePerson person, 
                System.Web.HttpResponse webResponse, 
                System.Web.HttpCookie cookie
                )
            {
                dtoExportSubmission settings = new dtoExportSubmission() { ClientFilename = clientFilename, PrintBy = person, RequiredFiles = requiredFiles, Sections = sections, Call = call, Submitter = submitter };
                return Submission(openCloseConnection, settings, webResponse, cookie, PrintSettings, phData);
            }

            public Document Submission(
                Boolean openCloseConnection, 
                dtoExportSubmission settings, 
                System.Web.HttpResponse webResponse, 
                System.Web.HttpCookie cookie,
                CallPrintSettings printSettings,
                lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData placeHolderData,
                bool forDraft = false)
            {
                Document doc = null;
                Settings = settings;
                if(printSettings == null)
                    printSettings = new CallPrintSettings();

                this.phData = placeHolderData;
                
                PrintSettings = printSettings;
                if (settings.ForWebDownload)
                {
                    string watermark = "";
                    if (forDraft)
                    {
                        watermark = printSettings.DraftWaterMark;
                    }

                    doc = WebExport(openCloseConnection, settings.ClientFilename, Template.Settings, webResponse, cookie, watermark);

                    //doc = WebExport(openCloseConnection, settings.ClientFilename, webResponse, cookie);
                    if (doc == null)
                        return GetErrorDocument(openCloseConnection,false,settings.Filename, webResponse, cookie);
                    else if (settings.SaveToFile) {
                        SubmissionToFile(settings);
                    }
                }
                else if (settings.SaveToFile) {
                    doc = SubmissionToFile(settings);
                } else if (forDraft)
                {
                    doc = WebExport(openCloseConnection, settings.ClientFilename, Template.Settings, webResponse, cookie, printSettings.DraftWaterMark);
                }
                return doc;
            }

            private Document SubmissionToFile(dtoExportSubmission settings)
            {
                Document doc = null;
                System.IO.FileStream stream = null;
                Impersonate oImpersonate = new Impersonate();
                Boolean wasImpersonated = Impersonate.isImpersonated();
                try
                {
                    if (!wasImpersonated && oImpersonate.ImpersonateValidUser() == FileMessage.ImpersonationFailed)
                        return null;
                    else
                    {
                        stream = new System.IO.FileStream(settings.Filename, System.IO.FileMode.Create);
                        if (stream != null)
                            doc = FileExport(stream);

                    }
                }
                catch (Exception ex)
                {
                    if (stream != null)
                        stream.Close();
                    if (lm.Comol.Core.File.Exists.File(settings.Filename))
                        lm.Comol.Core.File.Delete.File(settings.Filename);

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

            #region "Render document"


        public string replaceText(
            string body,
            UserSubmission submission,
            lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData phData,
            bool forcompile,
             Dictionary<SubmissionTranslations, string> translations)
            {
                CompanyUser company = null;
                try
                {
                    company = (CompanyUser)phData.ModuleObject;
                }
                catch (Exception)
                {
                }

                if (company != null && company.CompanyInfo == null) company = null;

                //Replace TAG CFP
                foreach (CallForPeaperPlaceHoldersType placeholder in TemplateCallForPeaperPlaceHolders.PlaceHoldersGetAll())
                {
                    string tag = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(placeholder);

                    try
                    {
                        
                        
                        switch (placeholder)
                        {
                            case CallForPeaperPlaceHoldersType.CallBody:
                            case CallForPeaperPlaceHoldersType.NewPage:
                            case CallForPeaperPlaceHoldersType.Table:
                                break;

                            case CallForPeaperPlaceHoldersType.CreatedBy:
                                body = body.Replace(tag, submission.Call.CreatedBy.SurnameAndName);
                                break;
                            case CallForPeaperPlaceHoldersType.CreatedOn:
                                body = body.Replace(tag, submission.Call.CreatedOn.ToString());
                                break;
                            case CallForPeaperPlaceHoldersType.Description:
                                body = body.Replace(tag, submission.Call.Description);
                                break;
                            case CallForPeaperPlaceHoldersType.Edition:
                                body = body.Replace(tag, submission.Call.Edition);
                                break;
                            case CallForPeaperPlaceHoldersType.OpenOn:
                                body = body.Replace(tag, submission.Call.StartDate.ToString()?? "");
                                break;
                            case CallForPeaperPlaceHoldersType.OpenUntil:
                                body = body.Replace(tag, 
                                    (submission.Call.EndDate.ToString()?? ""));
                                break;
                            case CallForPeaperPlaceHoldersType.PrintedBy:
                                body = body.Replace(tag, phData.Person.SurnameAndName);
                                break;
                            case CallForPeaperPlaceHoldersType.PrintedOn:
                                body = body.Replace(tag, DateTime.Now.ToString());
                                break;

                            case CallForPeaperPlaceHoldersType.SubmissionStatus:
                                //Perchè non hanno considerato successive aggiunte!!!
                                if (submission.Status == SubmissionStatus.waitforsignature)
                                {
                                    body = body.Replace(tag, translations[SubmissionTranslations.StatusWaitForSign]);
                                }
                                else
                                {
                                    body = body.Replace(tag, translations[(SubmissionTranslations)(int)submission.Status]);    
                                }



                                
                                break;

                            case CallForPeaperPlaceHoldersType.SubmittedBy:
                                //if(submission.Owner.TypeID)
                                body = body.Replace(tag, forcompile ? "" : submission.SubmittedBy.SurnameAndName);
                                break;
                            case CallForPeaperPlaceHoldersType.SubmittedOn:
                                body = body.Replace(tag,
                                    (forcompile ? "" : submission.SubmittedOn.ToString() ?? ""));
                                break;
                            case CallForPeaperPlaceHoldersType.SubmitterType:
                                body = body.Replace(tag, forcompile ? "" : submission.Type.Name);
                                break;
                            case CallForPeaperPlaceHoldersType.Summary:
                                body = body.Replace(tag, forcompile ? "" : submission.Call.Summary);
                                break;
                            case CallForPeaperPlaceHoldersType.Title:
                                body = body.Replace(tag, forcompile ? "" : submission.Call.Name);
                                break;
                            case CallForPeaperPlaceHoldersType.SubmitterCompanyName:
                                body = body.Replace(tag, (forcompile || (company==null) ? "" : company.CompanyInfo.Name));
                                break;
                            case CallForPeaperPlaceHoldersType.SubmitterCompanyRea:
                                body = body.Replace(tag, (forcompile || (company == null) ? "" : company.CompanyInfo.ReaNumber));
                                break;
                            case CallForPeaperPlaceHoldersType.SubmitterCompanyTaxCode:
                                body = body.Replace(tag, (forcompile || (company == null) ? "" : company.CompanyInfo.TaxCode));
                                break;

                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                ////Override tag comuni (azienda)

            //if (phData.ModuleObject != null && phData.ModuleObject.GetType() == typeof (CompanyUser))
            //{
            //    CompanyUser company = (CompanyUser) phData.ModuleObject;

            //    body = body.Replace(
            //        TemplateCallForPeaperPlaceHolders.GetPlaceHolder(CallForPeaperPlaceHoldersType.SubmitterCompanyName),
            //        company.Name);

            //    if (company.CompanyInfo != null)
            //    {
            //        body = body.Replace(
            //            TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
            //                CallForPeaperPlaceHoldersType.SubmitterCompanyRea),
            //            company.CompanyInfo.ReaNumber);

            //        body = body.Replace(
            //            TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
            //                CallForPeaperPlaceHoldersType.SubmitterCompanyTaxCode),
            //            company.CompanyInfo.TaxCode);
            //    }
            //    else
            //    {
            //        body = body.Replace(
            //            TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
            //                CallForPeaperPlaceHoldersType.SubmitterCompanyRea),
            //            "");

            //        body = body.Replace(
            //            TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
            //                CallForPeaperPlaceHoldersType.SubmitterCompanyTaxCode),
            //            "");
            //    }
            //}
            //else
            //{
                //body = body.Replace(
                //        TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                //            CallForPeaperPlaceHoldersType.SubmitterCompanyName),
                //        "")
                //        .Replace(
                //        TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                //            CallForPeaperPlaceHoldersType.SubmitterCompanyRea),
                //        "")
                //        .Replace(
                //        TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                //            CallForPeaperPlaceHoldersType.SubmitterCompanyTaxCode),
                //        "");
            //}

                //Replace TAG comuni
                body = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.Translate(
                    body, phData);

                return body;
            }

        private UserSubmission SubmissionCheckVoid(UserSubmission submission)
        {
            if (submission == null) submission = new UserSubmission();
            if(submission.Call == null) submission.Call = new CallForPaper();
            if (submission.Call.CreatedBy == null) submission.Call.CreatedBy = new litePerson();
            if (submission.SubmittedBy == null) submission.SubmittedBy = new litePerson();
            if (submission.Type == null) submission.Type = new SubmitterType();
            return submission;
        }

        private CommonPlaceHolderData PHDataCheckVoid(CommonPlaceHolderData phData)
        {

            if(phData == null) phData = new CommonPlaceHolderData();
            if(phData.Person == null) phData.Person = new Person();

            return phData;
        }
            public void ExportSubmissionTagReplace(
                ref Document doc,
                UserSubmission submission,
                List<dtoCallSubmissionFile> requiredFiles,
                List<dtoCallSection<dtoSubmissionValueField>> sections,
                Dictionary<SubmissionTranslations, string> translations,
                Boolean forCompile,
                lm.Comol.Core.DomainModel.Helpers.CommonPlaceHolderData phData
                )
            {
                phData = PHDataCheckVoid(phData);
                submission = SubmissionCheckVoid(submission);



                //string Body = 
                //    replaceText(
                //    HttpContext.Current.Server.HtmlDecode(
                //        Template.Body.Text),
                //    submission, 
                //    phData,
                //    forCompile,
                //    translations
                //    );

                //NOTA: HtmlReplaceNbspToBR serve per mettere i giusti "br" all'interno dei paragrafi, altrimenti molti "a capo" vengono saltati!!!
                //Sostituire i "\r\n" con i "<br/>" crea invece grossi casini per le parti il cui render è lasciato a iTS.


                string Body =
                    lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.HtmlReplaceNbspToBR(
                        replaceText(
                            Template.Body.Text
                                .Replace("&amp;", "&"),
                            submission,
                            phData,
                            forCompile,
                            translations
                        )
                    );

                String tagNewPage = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                    CallForPeaperPlaceHoldersType.NewPage)
                    .Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "[")
                    .Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]");

                //String tagTable = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                //    CallForPeaperPlaceHoldersType.Table)
                //    .Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "[")
                //    .Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]");

                String tagCallBody = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                    CallForPeaperPlaceHoldersType.CallBody)
                    .Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "[")
                    .Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]");

                String tagCallInfo = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                    CallForPeaperPlaceHoldersType.CallBaseInfo)
                    .Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "[")
                    .Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]");



                //"{0}cfp.{1}{2}"
                String tagTableStart = string.Format(
                    TemplateCallForPeaperPlaceHolders.tagString,
                   "[", CallForPeaperPlaceHoldersType.Table.ToString(), "");
                String tagTableBorderStart = string.Format(
                   TemplateCallForPeaperPlaceHolders.tagString,
                  "[", CallForPeaperPlaceHoldersType.TableBorder.ToString(), "");

                Body = Body.Replace(TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                    CallForPeaperPlaceHoldersType.NewPage), "§" + tagNewPage + "§");

                //Body = Body.Replace(TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                //    CallForPeaperPlaceHoldersType.Table), "§" + tagTable + "§");

                Body = Body.Replace(TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                    CallForPeaperPlaceHoldersType.CallBody), "§" + tagCallBody + "§");

                Body = Body.Replace(TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                    CallForPeaperPlaceHoldersType.CallBaseInfo), "§" + tagCallInfo + "§");

                Body = Body.Replace(tagTableStart, "§" + tagTableStart);

                //NewPage, Body, [CallInfo]: OK

                //[CallInfo....]


                //Body.Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "§[");
                //Body.Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]§");

                String[] bodySplit = Body.Split('§');


                if(PrintSettings == null)
                    PrintSettings = new CallPrintSettings();

                foreach (string part in bodySplit)
                {
                    if (part == tagNewPage) { doc.NewPage(); }

                    else if (part == tagCallBody)
                    {
                            ExportSubmission(doc,
                            forCompile,
                            requiredFiles,
                            sections,
                            translations);    
                        //doc.Add(bodyRender);
                    }

                    else if (part == tagCallInfo)
                    {
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(WriteSubmissionInfo(submission, submission.Status, translations, forCompile));
                    }
                        // TABLE
                    else if (part.StartsWith(tagTableStart) || part.StartsWith(tagTableBorderStart))
                    {
                        string internalPart = part;
                        
                        bool hasBorder = part.StartsWith(tagTableBorderStart);

                        int closeindex = internalPart.LastIndexOf(']');

                        //int endIndex = internalPart.Length -1;


                        //[cfp.Table=30,70&Testo dell'utente|Valore|3col|Altro testo|AltroValore||ultimo]....
                        string tag = internalPart.Substring(0, closeindex+1); //dovrebbe togliere le []!
                        string tabletag = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                            CallForPeaperPlaceHoldersType.Table)
                            .Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "[")
                            .Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]");

                        string tabletagBorder = TemplateCallForPeaperPlaceHolders.GetPlaceHolder(
                            CallForPeaperPlaceHoldersType.TableBorder)
                            .Replace(TemplateCallForPeaperPlaceHolders.OpenTag, "[")
                            .Replace(TemplateCallForPeaperPlaceHolders.CloseTag, "]");

                        if (tag == tabletag || tag == tabletagBorder)
                        {
                            internalPart = internalPart.Replace(tabletag, "");
                            //Resto del testo:
                            PdfPTable tableTxt = new PdfPTable(1);
                            tableTxt = new PdfPTable(1);
                            tableTxt.AddCell(HtmlToCell(internalPart));
                            doc.Add(tableTxt);
                        }
                        else
                        {
                            try
                            {
                                string posttag = internalPart.Replace(tag, "");



                                //cfp.Table=30,70&Testo dell'utente|Valore|3col|Altro testo|AltroValore||ultimo

                                tag = tag.Replace(tagTableStart + "=", "");
                                tag = tag.Replace(tagTableBorderStart + "=", "");
                                tag = tag.Substring(0, tag.Length - 1); //rimuovo l'ultima ]

                                //30,70&Testo dell'utente|Valore|3col|Altro testo|AltroValore||ultimo
                                string[] tagSplit = tag.Split('&');
                                PdfPTable table = GetTable(1); // = new PdfPTable(1);

                                float[] colsize = new float[1];
                                try
                                {
                                    colsize = Array.ConvertAll(tagSplit[0].Split(','), float.Parse);
                                }
                                catch (Exception)
                                {

                                }

                                //Controlli sulla dimensione corretta della tabella: RIVEDERE!
                                if (colsize == null || colsize.Count() <= 0)
                                {
                                    colsize = new float[1];
                                    colsize[0] = 100;
                                }

                                float size = 100/colsize.Count();

                                float fullsize = 0;

                                for (int i = 0; i < colsize.Count(); i++)
                                {
                                    fullsize += colsize[i];
                                }

                                if (fullsize <= 0)
                                {
                                    colsize = new float[1];
                                    colsize[0] = 100;
                                }

                                if (colsize != null && colsize.Count() > 1)
                                {
                                    table = GetTable(colsize.Count(), colsize, 1);
                                }

                                if (tagSplit.Count() > 1)
                                {
                                    foreach (string field in tagSplit[1].Split('|'))
                                    {
                                        bool hasBackground = false;

                                        string celltext = field;
                                        if (field.StartsWith("*"))
                                        {
                                            celltext = field.Substring(1);
                                            hasBackground = true;
                                        }

                                        PdfPCell cell = new PdfPCell(HtmlToCell(celltext));
                                        cell.Border = Rectangle.NO_BORDER;
                                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cell.Padding = 2f;
                                        cell.PaddingTop = 0f;
                                        if (hasBorder)
                                        {
                                            cell.Border = Rectangle.BOX;
                                            cell.BorderWidth = 1f;
                                            //cell.Padding = 2f;
                                        }

                                        
                                        if(hasBackground)
                                            cell.BackgroundColor = new BaseColor(195, 195, 195);
                                        
                                        table.AddCell(cell);
                                    }

                                    doc.Add(table);
                                }

                                //Resto del testo:
                                PdfPTable tableTxt = new PdfPTable(1);
                                string text = posttag; //internalPart.Substring(endIndex + 1);
                                tableTxt = new PdfPTable(1);
                                tableTxt.AddCell(HtmlToCell(text));
                                doc.Add(tableTxt);
                                //VOID
                                //doc.Add(GetWhiteParagraph(GetFont(ItemType.VoidSmall)));
                            }
                            catch
                            {
                                
                            }
                        }



                        
                    }
                    else
                    {
                        PdfPTable table = new PdfPTable(1);

                        

                        table.AddCell(HtmlToCell(part));
                        doc.Add(table);
                    }
                }

            }


                private void ExportSubmission(
                    Document doc, 
                    UserSubmission submission, 
                    List<dtoCallSubmissionFile> requiredFiles, 
                    List<dtoCallSection<dtoSubmissionValueField>> sections, 
                    litePerson person, 
                    Dictionary<SubmissionTranslations, string> translations
                    )
                {

                    //Domanda partecipazione al bando XYZ, versione JJJ
                    doc.Add(WriteSubmissionHeader(submission, person, translations));

                    

                   // doc.Add(TextSeparator());
                    //doc.Add(TextSeparator());

                    //Informazioni di base
                    //doc.Add(GetPageTitle(translations[SubmissionTranslations.SubmissionInfo], GetFont(ItemType.Caption)));

                    //Spaziatura

                    Paragraph whitespace = GetParagraph("", GetFont(ItemType.Title));

                    for(int i = 0; i< 3; i++)
                    {
                        doc.Add(whitespace);    
                    }
                    
                    //Tabella info sottomissione
                    doc.Add(WriteSubmissionInfo(submission, submission.Status, translations, false));

                    
                    //Info stampa


                    //PdfPTable table = new PdfPTable(1);
                    for (int i = 0; i < 8; i++) //Con "Title" il massimo è 8!
                    {
                        doc.Add(whitespace);
                    }
                    
                    PdfPTable table = GetTable(1);
                    table.WidthPercentage = (float) 90;
                    
                    //PdfPCell separatortCell = new PdfPCell();
                    //separatortCell.AddElement(GetWhiteParagraph());

                    //separatortCell.Border = Rectangle.NO_BORDER;
                    //table.AddCell(separatortCell);
                    DateTime printDate = DateTime.Now;

                    String printedBy = String.Format(translations[SubmissionTranslations.PrintInfo], (person == null || person.TypeID == (int)UserTypeStandard.Guest) ? translations[SubmissionTranslations.AnonymousUser] : person.SurnameAndName, printDate.ToShortDateString(), printDate.ToShortTimeString());

                    PdfPCell printCell = new PdfPCell(new Phrase(printedBy, GetFont(ItemType.PrintInfos)));
                    printCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    printCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(printCell);
                    doc.Add(table);
                    
                    //doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));




                    ExportSubmission(doc, false, requiredFiles, sections, translations);
                }
                private void ExportSubmission(Document doc, BaseForPaper call, SubmitterType submitter, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson printer, Dictionary<SubmissionTranslations, string> translations)
                {
                    doc.Add(WriteCallHeader(call, submitter, translations, printer));
                    doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                    doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                    ExportSubmission(doc, true, requiredFiles, sections, translations);
                }
                private void ExportSubmission(Document doc, Boolean forCompile, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, Dictionary<SubmissionTranslations, string> translations)
                {
                    //doc.NewPage();
                    if (sections != null)
                    {
                        foreach (dtoCallSection<dtoSubmissionValueField> section in sections)
                        {
                            Paragraph par = new Paragraph(
                                GetParagraph(
                                    section.Name, 
                                    GetFont(PrintSettings.SectionTitle),
                                    Element.ALIGN_JUSTIFIED
                                    )
                                );//GetSectionTitle(section.Name);
                            doc.Add(par);

                            if (!String.IsNullOrEmpty(section.Description))
                            {
                                //doc.Add(new Paragraph(GetWhiteParagraph(GetFont(ItemType.VoidSmall))));
                                
                                //VOID
                                //doc.Add(GetWhiteParagraph(GetFont(ItemType.VoidSmall)));
                                doc.Add(
                                    GetParagraph(
                                        section.Description, 
                                        GetFont(PrintSettings.SectionDescription),
                                        Element.ALIGN_JUSTIFIED));

                                //VOID
                                doc.Add(GetWhiteParagraph(GetFont(ItemType.VoidSmall)));
                            }
                            
                            
                            

                            WriteSubmissionSection(doc, forCompile, section, translations);
                            //doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                        }
                    }
                    if (requiredFiles.Count > 0)
                        WriteRequiredFiles(doc, forCompile, requiredFiles, translations);
                }

                private iTextSharp5.text.pdf.PdfPTable WriteCallHeader(
                    BaseForPaper call, 
                    SubmitterType submitter, 
                    Dictionary<SubmissionTranslations, string> translations,
                    litePerson printer)
                {
                    iTextSharp5.text.pdf.PdfPTable table = GetTable(1);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;





                    String callName = "";
                    //PdfPCell titleCell = new PdfPCell(new Phrase(""));


                    //Nome del bando

                    switch (call.Type)
                    {
                        case CallForPaperType.CallForBids:
                            if (string.IsNullOrEmpty(call.Edition))
                                callName = call.Name;
                            else
                                callName = call.Name;
                            break;
                        case CallForPaperType.RequestForMembership:
                            callName = call.Name;
                            break;
                    }

                    PdfPCell titleCell = new PdfPCell(GetPageTitle(callName, GetFont(PrintSettings.FieldTitle)));
                    titleCell.Border = Rectangle.NO_BORDER;
                    titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(titleCell);

                    //Spaziatura
                    titleCell = new PdfPCell(GetParagraph("", GetFont(ItemType.Default)));
                    titleCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(titleCell);



                    //Domanda di partecipazione al bando
                    switch (call.Type)
                    {
                        case CallForPaperType.CallForBids:
                            if (string.IsNullOrEmpty(call.Edition))
                                callName = String.Format(translations[SubmissionTranslations.CallTitle], "\r\n");
                            else
                                callName = String.Format(translations[SubmissionTranslations.CallTitleAndEdition], " - ", call.Edition);
                            break;
                        case CallForPaperType.RequestForMembership:
                            callName = String.Format(translations[SubmissionTranslations.RequestTitle], "\r\n");
                            break;
                    }

                    titleCell = new PdfPCell(GetParagraph(callName, GetFont(PrintSettings.FieldTitle)));

                    titleCell.Border = Rectangle.NO_BORDER;
                    titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(titleCell);




                    


                    //PdfPCell separatortCell = new PdfPCell();
                    //separatortCell.AddElement(GetWhiteParagraph());

                    //separatortCell.Border = Rectangle.NO_BORDER;
                    //table.AddCell(separatortCell);
                    //DateTime printDate = DateTime.Now;
                    //String printedBy = String.Format(translations[SubmissionTranslations.PrintInfo], (printer == null || printer.TypeID == (int)UserTypeStandard.Guest) ? translations[SubmissionTranslations.AnonymousUser] : printer.SurnameAndName, printDate.ToShortDateString(), printDate.ToShortTimeString());
                    //PdfPCell printCell = new PdfPCell(new Phrase(printedBy, GetFont(ItemType.PrintInfos)));
                    //printCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //printCell.Border = Rectangle.NO_BORDER;
                    //table.AddCell(printCell);
                    

                    return table;


                }
                private PdfPTable WriteSubmissionHeader(UserSubmission submission, litePerson printer, Dictionary<SubmissionTranslations, string> translations)
                {
                    PdfPTable table = WriteCallHeader(submission.Call, submission.Type, translations, printer);
                    
                    
                    
                    return table;
                }
                private PdfPTable WriteSubmissionInfo(
                    UserSubmission submission, 
                    SubmissionStatus status, 
                    Dictionary<SubmissionTranslations, string> translations,
                    bool ForCompile)
                {
                    float minHeight = (float) 33;
                    
                    PdfPTable table = GetTable(2, new float[] { 30, 70 }, 1);
                    PdfPCell cell;
                    cell = new PdfPCell(GetParagraph(translations[SubmissionTranslations.SubmittedByTitle], GetFont(PrintSettings.FieldTitle)));
                    cell.Border = Rectangle.NO_BORDER; //1 = solo top
                    cell.MinimumHeight = minHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);


                    if (!ForCompile)
                    {
                        String owner = (submission.Owner == null ||
                                        submission.Owner.TypeID == (int) UserTypeStandard.Guest)
                            ? translations[SubmissionTranslations.AnonymousUser]
                            : submission.Owner.SurnameAndName;

                        String submitter = (submission.SubmittedBy == null ||
                                            submission.SubmittedBy.TypeID == (int) UserTypeStandard.Guest)
                            ? translations[SubmissionTranslations.AnonymousUser]
                            : submission.SubmittedBy.SurnameAndName;

                        if (!submission.SubmittedOn.HasValue)
                            cell =
                                new PdfPCell(
                                    GetParagraph(
                                        String.Format(translations[SubmissionTranslations.CreatedByInfo], owner,
                                            submission.ModifiedOn.Value.ToShortDateString(),
                                            submission.ModifiedOn.Value.ToShortTimeString()),
                                        GetFont(PrintSettings.FieldContent)));
                        else if (submission.Owner == submission.SubmittedBy)
                            cell =
                                new PdfPCell(
                                    GetParagraph(
                                        String.Format(translations[SubmissionTranslations.SubmittedByInfo], submitter,
                                            submission.SubmittedOn.Value.ToShortDateString(),
                                            submission.SubmittedOn.Value.ToShortTimeString()),
                                        GetFont(PrintSettings.FieldContent)));
                        else
                            cell =
                                new PdfPCell(
                                    GetParagraph(
                                        String.Format(translations[SubmissionTranslations.SubmittedForInfo], submitter,
                                            owner, submission.SubmittedOn.Value.ToShortDateString(),
                                            submission.SubmittedOn.Value.ToShortTimeString()),
                                        GetFont(PrintSettings.FieldContent)));
                    }
                    else
                    {
                        cell =
                            new PdfPCell(
                                GetParagraph("", 
                                GetFont(PrintSettings.FieldContent)));
                    }
                    


                    cell.MinimumHeight = minHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(translations[SubmissionTranslations.SubmittedTypeTitle], GetFont(PrintSettings.FieldTitle)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.MinimumHeight = minHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(submission.Type.Name, GetFont(PrintSettings.FieldContent)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.MinimumHeight = minHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(translations[SubmissionTranslations.SubmissionStatusTitle], GetFont(PrintSettings.FieldTitle)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.MinimumHeight = minHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);


                    string statusTranslated = (status == SubmissionStatus.waitforsignature)
                        ? translations[SubmissionTranslations.StatusWaitForSign]
                        : translations[(SubmissionTranslations) (int) status];

                    cell = new PdfPCell(new Phrase(statusTranslated, GetFont(PrintSettings.FieldContent)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.MinimumHeight = minHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("/r/n"));
                    //table.AddCell(cell);

                    //cell = new PdfPCell(GetParagraph(translations[SubmissionTranslations.SubmittedByTitle], GetFont(ItemType.FieldName)));
                    //table.AddCell(cell);
                    return table;
                }

            #region "Export fields"
                private void WriteSubmissionSection(Document doc, Boolean forCompile, dtoCallSection<dtoSubmissionValueField> section, Dictionary<SubmissionTranslations, string> translations)
                {
                    //doc.Add(Chunk.NEWLINE);
                    
                    PdfPTable tSectionfields;

                    switch (PrintSettings.Layout)
                    {
                        case PageLayout.Standard:
                            tSectionfields = GetTable(1, new float[] { 100 }, 1);
                            break;
                        case PageLayout.LeftRight:
                            tSectionfields = GetTable(2, new float[] { 40, 60 }, 1);
                            break;


                        case PageLayout.LeftRight1090:
                            tSectionfields = GetTable(2, new float[] { 10, 90 }, 1);
                            break;
                        case PageLayout.LeftRight2080:
                            tSectionfields = GetTable(2, new float[] { 20, 80 }, 1);
                            break;
                        case PageLayout.LeftRight3070:
                            tSectionfields = GetTable(2, new float[] { 30, 70 }, 1);
                            break;
                        case PageLayout.LeftRight4060:
                            tSectionfields = GetTable(2, new float[] { 40, 60 }, 1);
                            break;
                        case PageLayout.LeftRight5050:
                            tSectionfields = GetTable(2, new float[] { 50, 50 }, 1);
                            break;
                        case PageLayout.LeftRight6040:
                            tSectionfields = GetTable(2, new float[] { 60, 40 }, 1);
                            break;
                        case PageLayout.LeftRight7030:
                            tSectionfields = GetTable(2, new float[] { 70, 30 }, 1);
                            break;
                        case PageLayout.LeftRight8020:
                            tSectionfields = GetTable(2, new float[] { 80, 20 }, 1);
                            break;
                        case PageLayout.LeftRight9010:
                            tSectionfields = GetTable(2, new float[] { 90, 10 }, 1);
                            break;


                        default:
                            tSectionfields = GetTable(2, new float[] { 40, 60 }, 1);
                            break;
                    }

                    tSectionfields.SplitLate = false;
                    
                    String emptyItem = ""; //translations[SubmissionTranslations.EmptyItem];

                    String mandatory = translations[SubmissionTranslations.ItemMandatory];
                    String maxCharInfo = translations[SubmissionTranslations.MaxCharInfo];
                    
                    foreach (dtoSubmissionValueField item in section.Fields)
                    {
                        if(item.Field.Type != FieldType.TableSummary)
                        { 

                            //Nome e descrizione campo
                            PdfPCell cName = new PdfPCell();

                            cName.Border = Rectangle.NO_BORDER;  //1: solo sotto
                            cName.BorderWidth = 0;

                            Paragraph par = GetParagraph("", GetFont(PrintSettings.FieldTitle), 0); //GetFont(ItemType.Paragraph), 0);

                            par.Alignment = Element.ALIGN_JUSTIFIED;

                            //if (item.Field.Type != FieldType.Note)
                            //{
                            if (PrintSettings.ShowMandatory)
                            {
                                par.Add(new Chunk(((item.Mandatory) ? "*" : "") + item.Field.Name,
                                    GetFont(PrintSettings.FieldTitle)));
                            }
                            else
                            {
                                par.Add(new Chunk(item.Field.Name, GetFont(PrintSettings.FieldTitle)));
                            }
                            //}
   
                            if (!string.IsNullOrEmpty(item.Field.Description))
                            {
                                par.Add(Chunk.NEWLINE);
                                par.Add(GetPhrase(
                                    item.Field.Description,
                                    GetFont(PrintSettings.FieldDescription)
                                    ));
                                //(item.Field.Type != FieldType.Note) ? GetFont(ItemType.FieldDescription) : GetFont(ItemType.FieldNote)));
                            }

                            if (par != null)
                                cName.AddElement(par);
                        

                            if (PrintSettings.Layout != PageLayout.Standard &&
                                (item.Field.Type == FieldType.MultiLine 
                                || item.Field.Type == FieldType.Note
                                || item.Field.Type == FieldType.Disclaimer
                                || item.Field.Type == FieldType.TableSimple
                                || item.Field.Type == FieldType.TableReport
                                || item.Field.Type == FieldType.RadioButtonList
                                || item.Field.Type == FieldType.CheckboxList
                                || item.Field.Type == FieldType.DropDownList
                                )) //
                            {
                                cName.Colspan = 2;
                            }
                            tSectionfields.AddCell(cName);

                        
                            //Valori campo
                            PdfPCell cValue = new PdfPCell();

                            cValue.Border = Rectangle.NO_BORDER; //Rectangle.NO_BORDER;  //1 solo sotto
                            cValue.BorderWidth = 1;
                            cValue.Indent = (float)10;

                            bool evictEmpty = (PrintSettings.UnselectFields == ShowUnselect.HideAll);

                            switch (item.Field.Type)
                            {
                                case FieldType.MultiLine:
                                    if (PrintSettings.Layout != PageLayout.Standard)
                                        {
                                            cValue.Colspan = 2;
                                        }

                                        tSectionfields.AddCell(MultipleLineFieldToCell(cValue, forCompile, item, emptyItem, maxCharInfo));

                                    break;
                                //case FieldType.TableSimple:

                                //    cValue = TableToCell(cValue, forCompile, item, emptyItem, maxCharInfo);
                                
                                //    if (PrintSettings.Layout != PageLayout.Standard)
                                //    {
                                //        cValue.Colspan = 2;
                                //    }

                                //    tSectionfields.AddCell(cValue);
                                //    //Todo
                                //    break;
                                //case FieldType.TableReport:
                                
                                //    cValue = TableReportToCell(cValue, forCompile, item, emptyItem, maxCharInfo);
                                
                                //    if (PrintSettings.Layout != PageLayout.Standard)
                                //    {
                                //        cValue.Colspan = 2;
                                //    }

                                //    tSectionfields.AddCell(cValue);
                                //    //Todo
                                //    break;
                                case FieldType.TableSimple:
                                case FieldType.TableReport:
                                
                                    cValue = HtmlToCell(CallTableHelper.TableDecorateHtml(item, forCompile));
                            
                                    if (PrintSettings.Layout != PageLayout.Standard)
                                    {
                                        cValue.Colspan = 2;
                                    }

                                    tSectionfields.AddCell(cValue);

                                    break;
                                case FieldType.Disclaimer:
                                    if (PrintSettings.Layout != PageLayout.Standard)
                                        cValue.Colspan = 2;
                                    tSectionfields.AddCell(DisclaimerLineFieldToCell(cValue, forCompile, item, emptyItem, translations, evictEmpty));
                                    break;

                                case FieldType.CheckboxList:
                                case FieldType.RadioButtonList:
                                case FieldType.DropDownList:
                                    if (PrintSettings.Layout != PageLayout.Standard)
                                    {
                                        cValue.Colspan = 2;
                                    }
                                    tSectionfields.AddCell(MultipleChoiceFieldToCell(cValue, forCompile, item, emptyItem, translations, evictEmpty));
                                    break;

                                case FieldType.FileInput:
                                    tSectionfields.AddCell(FileInputFieldToCell(cValue, forCompile, item, emptyItem, translations));
                                    break;

                                case FieldType.Note:
                                    break;

                                default:
                                    tSectionfields.AddCell(AddGenericFieldToTable(cValue, forCompile, item, emptyItem, maxCharInfo));
                                    break;
                            }
                        }
                    }

                    doc.Add(tSectionfields);
                }
                private PdfPCell AddGenericFieldToTable(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, String maxCharInfo)
                {
                    Paragraph par = new Paragraph(
                        "   " +
                        (forCompile ? "" : 
                            (item.Value == null || String.IsNullOrEmpty(item.Value.Text) ?          emptyItem : item.Value.Text)
                        ),
                        GetFont(ItemType.Paragraph));

                    par.Alignment = Element.ALIGN_JUSTIFIED;

                    cell.AddElement(par);

                    if (forCompile && item.Field.MaxLength > 200)
                    {
                        Chunk ch = new Chunk("\r\n", GetFont(ItemType.Paragraph));
                        cell.AddElement(ch);
                    }

                    //if (item.Field.MaxLength > 0 && forCompile)
                    //    cell.AddElement(GetParagraph(String.Format(maxCharInfo, item.Field.MaxLength), GetFont(ItemType.Caption)));
                    return cell;
                }
                private PdfPCell MultipleLineFieldToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, String maxCharInfo)
                {
                    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                    string val = item.Value.Text.TrimStart();

                    Paragraph par =
                        new Paragraph(
                            (forCompile
                                ? ""
                                : ((item.Value == null || String.IsNullOrEmpty(val))
                                    ? emptyItem
                                    : val)), GetFont(PrintSettings.FieldContent));

                    par.Alignment = Element.ALIGN_JUSTIFIED;

                        //GetFont(ItemType.Paragraph));
                    if (forCompile)
                    {
                        PdfPTable tInput = GetTable(1, 1);
                        PdfPCell cInput = new PdfPCell();
                        if (item.Field.MaxLength > 4000)
                            cInput.Rowspan = 15;
                        else if (item.Field.MaxLength > 2000)
                            cInput.Rowspan = 10;
                        else
                            cInput.Rowspan = 5;
                        tInput.AddCell(cInput);
                        cell.AddElement(tInput);

                        
                        //if (item.Field.MaxLength > 0)
                        //    cell.AddElement(GetParagraph(String.Format(maxCharInfo, item.Field.MaxLength),
                        //        GetFont(PrintSettings.FieldContent)));
                    }
                    else
                    {
                        cell.AddElement(par);
                        cell.AddElement(GetWhiteParagraph());
                    }
                    
                    //GetFont(ItemType.Caption)));
                    //cell.NoWrap = false;

                    
                    switch (PrintSettings.Layout)
                    {
                        case PageLayout.LeftRight:
                            cell.Colspan = 2;
                            //cell.Border = Rectangle.NO_BORDER;//Rectangle.BOTTOM_BORDER;
                            break;
                        case PageLayout.Standard:
                            cell.Colspan = 1;
                            break;
                        default:
                            cell.Colspan = 2;
                            //cell.Border = Rectangle.NO_BORDER; //Rectangle.BOTTOM_BORDER;
                            break;
                    }


                    return cell;
                }
                
                //private PdfPCell TableToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, String maxCharInfo)
                //{
                //    //ToDo!!

                //    String tablestr = item.Value.Text;
                //    //TableSetting.Cols.Split("|").ToList()
                //    if (String.IsNullOrEmpty(tablestr))
                //        tablestr = "<table></table>";

                //    String CompileString = "&nbsp;";

                //    if (item.Field.TableFieldSetting != null && !String.IsNullOrEmpty(item.Field.TableFieldSetting.Cols))
                //    {
                //        String Header = "";
                        
                //        foreach (String th in item.Field.TableFieldSetting.Cols.Split('|').ToList())
                //        {
                //            Header = string.Format("{0}<td border=\"1\"><b>{1}</b></td>", Header, th);
                //            CompileString = string.Format("{0}<td border=\"1\">&nbsp;</td>", CompileString);
                //        }

                //        tablestr = tablestr.Replace("<table>", String.Format("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\"><tr>{0}</tr>", Header));
                //    }

                //    if (forCompile)
                //    {
                //        tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //        //tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //        //tablestr = tablestr.Replace("</table>", String.Format("<tr>{0}</tr></table>", CompileString));
                //    }
                //    tablestr = tablestr.Replace("<td></td>", "<td>&nbsp;</td>");

                //    cell = HtmlToCell(tablestr);
                //    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                //    return cell;
                    

                //    //string val = item.Value.Text.TrimStart();

                //    //Paragraph par =
                //    //    new Paragraph(
                //    //        (forCompile
                //    //            ? ""
                //    //            : ((item.Value == null || String.IsNullOrEmpty(val))
                //    //                ? emptyItem
                //    //                : val)), GetFont(PrintSettings.FieldContent));

                //    //par.Alignment = Element.ALIGN_JUSTIFIED;

                //    ////GetFont(ItemType.Paragraph));
                //    //if (forCompile)
                //    //{
                //    //    PdfPTable tInput = GetTable(1, 1);
                //    //    PdfPCell cInput = new PdfPCell();
                //    //    if (item.Field.MaxLength > 4000)
                //    //        cInput.Rowspan = 15;
                //    //    else if (item.Field.MaxLength > 2000)
                //    //        cInput.Rowspan = 10;
                //    //    else
                //    //        cInput.Rowspan = 5;
                //    //    tInput.AddCell(cInput);
                //    //    cell.AddElement(tInput);

                //    //    if (item.Field.MaxLength > 0)
                //    //        cell.AddElement(GetParagraph(String.Format(maxCharInfo, item.Field.MaxLength),
                //    //            GetFont(PrintSettings.FieldContent)));
                //    //}
                //    //else
                //    //{
                //    //    cell.AddElement(par);
                //    //    cell.AddElement(GetWhiteParagraph());
                //    //}

                //    ////GetFont(ItemType.Caption)));
                //    ////cell.NoWrap = false;


                //    //switch (PrintSettings.Layout)
                //    //{
                //    //    case PageLayout.LeftRight:
                //    //        cell.Colspan = 2;
                //    //        //cell.Border = Rectangle.NO_BORDER;//Rectangle.BOTTOM_BORDER;
                //    //        break;
                //    //    case PageLayout.Standard:
                //    //        cell.Colspan = 1;
                //    //        break;
                //    //    default:
                //    //        cell.Colspan = 2;
                //    //        //cell.Border = Rectangle.NO_BORDER; //Rectangle.BOTTOM_BORDER;
                //    //        break;
                //    //}


                //    //return cell;
                //}

        
                //private PdfPCell TableReportToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, String maxCharInfo)
                //{

                    
                    

                //    cell = HtmlToCell(tablestr);
                //    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                //    return cell;
                    
                //}

                
        
                private PdfPCell DisclaimerLineFieldToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, Dictionary<SubmissionTranslations, string> translations, bool evictEmpty)
                {
                    switch (item.Field.DisclaimerType)
                    {
                        case DisclaimerType.None:
                            cell.AddElement(
                                GetParagraph(
                                    emptyItem, 
                                    GetFont(PrintSettings.FieldContent), 
                                    Element.ALIGN_RIGHT));
                                //GetFont(ItemType.Paragraph), Element.ALIGN_RIGHT));
                            break;
                        case DisclaimerType.Standard:
                            if (forCompile)
                                cell.AddElement(
                                    GetParagraph("( )  " 
                                    + translations[SubmissionTranslations.DisclaimerAccept] 
                                    + "    ( )  " 
                                    + translations[SubmissionTranslations.DisclaimerReject], 
                                    GetFont(PrintSettings.FieldContent), 
                                    Element.ALIGN_RIGHT));
                            else
                                cell.AddElement(
                                    GetParagraph(
                                    (item.Value != null && item.Value.Text == "True" ? 
                                        translations[SubmissionTranslations.DisclaimerAccept] 
                                        : translations[SubmissionTranslations.DisclaimerReject]), 
                                    GetFont(PrintSettings.FieldContent), 
                                    Element.ALIGN_RIGHT));
                            break;
                        case DisclaimerType.CustomDisplayOnly:
                            cell.AddElement(
                                GetParagraph(
                                    emptyItem, 
                                    GetFont(PrintSettings.FieldContent), 
                                    Element.ALIGN_RIGHT));
                            break;
                        default:
                            MultipleChoiceFieldToCell(cell, forCompile, item, emptyItem, translations, true);
                            break;
                    }
                    return cell;
                }
                private PdfPCell MultipleChoiceFieldToCell(
                    PdfPCell cell, 
                    Boolean forCompile, 
                    dtoSubmissionValueField item, 
                    String emptyItem, 
                    Dictionary<SubmissionTranslations, string> translations, 
                    bool evictEmpty)
                {
                    //Chunk chunkUnselect = (item.Field.Type == FieldType.CheckboxList) ? whitesquare : whitebullet;
                    //Chunk chunkSelect = (item.Field.Type == FieldType.CheckboxList) ? new iTextSharp.text.Chunk("\u25A0", FontFactory.GetFont(FontFactory.ZAPFDINGBATS, 12, Font.NORMAL, Color.BLACK)) : new iTextSharp.text.Chunk("\u25CB", FontFactory.GetFont(FontFactory.ZAPFDINGBATS, 12, Font.NORMAL, Color.BLACK));

                    String unselect = "";
                    Chunk chOpen = new Chunk();
                    Chunk chSelected = new Chunk();
                    Chunk chClose = new Chunk();
                    String freeTextSpace = "";

                    //if (!evictEmpty)
                    //{
                    unselect = (item.Field.Type == FieldType.CheckboxList) ? "[   ]  " : "(   )  ";
                    freeTextSpace = (item.Field.Type == FieldType.CheckboxList) ? "       " : "       ";

                    String optOpen = (item.Field.Type == FieldType.CheckboxList) ? "[" : "(";
                    String optClose = (item.Field.Type == FieldType.CheckboxList) ? "]  " : ")  ";
                    chOpen = new Chunk(optOpen, GetFont(PrintSettings.FieldContent));
                    chSelected = new Chunk("X", GetFont(PrintSettings.FieldContent));
                    chClose = new Chunk(optClose, GetFont(PrintSettings.FieldContent));
                    //}
                    
                    List<String> mValue = 
                        (item.Value == null || String.IsNullOrEmpty(item.Value.Text)) ? 
                        new List<String>() : 
                        item.Value.Text.Split('|').ToList();


                    Paragraph par = GetParagraph("", GetFont(PrintSettings.FieldContent), 0);
                    par.IndentationLeft = 10;

                    foreach (dtoFieldOption opt in item.Field.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).ToList())
                    {
                        if (forCompile)
                        {
                            par.Add(new Chunk(unselect + opt.Name, GetFont(PrintSettings.FieldContent)));
                            if (opt.IsFreeValue)
                                par.Add(new Chunk("  ______________________________________________________________________", GetFont(PrintSettings.FieldContent)));
                            par.Add(Chunk.NEWLINE);
                        }
                        else
                        {
                            if (mValue.Contains(opt.Id.ToString()))
                            {
                                par.Add(chOpen);
                                par.Add(chSelected);
                                par.Add(chClose);
                                par.Add(new Chunk(opt.Name, GetFont(PrintSettings.FieldContent)));
                                if (opt.IsFreeValue)
                                {
                                    if (!String.IsNullOrEmpty(item.Value.FreeText))
                                    {
                                        par.Add(Chunk.NEWLINE);
                                        par.Add(new Chunk(freeTextSpace + item.Value.FreeText,
                                            GetFont(PrintSettings.FieldContent)));
                                    }
                                    else
                                    {
                                        par.Add(new Chunk("  ______________________________________________________________________", GetFont(PrintSettings.FieldContent)));
                                    }
                                        
                                }
                                par.Add(Chunk.NEWLINE);
                            }
                            else
                            {
                                if (!evictEmpty)
                                {
                                    par.Add(new Chunk(unselect + opt.Name, GetFont(PrintSettings.FieldContent)));
                                    if (opt.IsFreeValue)
                                        par.Add(new Chunk("  ______________________________________________________________________", GetFont(PrintSettings.FieldContent)));
                                    par.Add(Chunk.NEWLINE);    
                                }
                                
                            }
                        }
                    }
                    cell.AddElement(par);

                    //Stampa valori Max/Min: TOLTO!
                    if (item.Field.Type == FieldType.CheckboxList && forCompile)
                    {
                        if (item.Field.MaxOption > 0 && item.Field.MinOption == 0)
                        {
                            cell.AddElement(
                                GetParagraph(
                                    String.Format(translations[SubmissionTranslations.MaxOption], 
                                    item.Field.MaxOption), 
                                GetFont(PrintSettings.FieldContent)));
                        }
                        else if (item.Field.MaxOption == 0 && item.Field.MinOption > 0)
                        {
                            cell.AddElement(
                                GetParagraph(
                                    String.Format(translations[SubmissionTranslations.MinOption], 
                                    item.Field.MinOption), 
                                GetFont(PrintSettings.FieldContent)));
                        }
                        else if (item.Field.MinOption > 0 && item.Field.MaxOption > 0)
                        {
                            cell.AddElement(
                                    GetParagraph(
                                        String.Format(translations[SubmissionTranslations.MinOptionMaxOption], 
                                        item.Field.MinOption, 
                                        item.Field.MaxOption), 
                                    GetFont(PrintSettings.FieldContent)));
                        }
                            
                    }
                    return cell;
                }
                private PdfPCell FileInputFieldToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, Dictionary<SubmissionTranslations, string> translations)
                {
                    //Chunk chunkUnselect = (item.Field.Type == FieldType.CheckboxList) ? whitesquare : whitebullet;
                    //Chunk chunkSelect = (item.Field.Type == FieldType.CheckboxList) ? new iTextSharp.text.Chunk("\u25A0", FontFactory.GetFont(FontFactory.ZAPFDINGBATS, 12, Font.NORMAL, Color.BLACK)) : new iTextSharp.text.Chunk("\u25CB", FontFactory.GetFont(FontFactory.ZAPFDINGBATS, 12, Font.NORMAL, Color.BLACK));
                    String unselect = "[   ]  ";
                    if (forCompile)
                        cell.AddElement(GetParagraph(unselect, GetFont(ItemType.Paragraph)));
                    else
                    {
                        //string value = "";

                        //try
                        //{
                        //    if (item.Value != null 
                        //        && item.Value.Link != null 
                        //        && item.Value.IdLink > 0)
                        //    {
                        //        if(item.Value.Link.DestinationItem != null)
                        //            value = item.Value.Link.DestinationItem.FQN;

                        //        else if(item.Value.Link.SourceItem != null || string.IsNullOrEmpty(value))
                        //            value = item.Value.Link.SourceItem.FQN;
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //}

                        //if (string.IsNullOrEmpty(value))
                        //    value = " ";

                        //translations[SubmissionTranslations.FileNotSubmitted] //value

                        
                        cell.AddElement(GetParagraph(
                            (item.Value == null || item.Value.IdLink==0) ? "  " : 
                            item.Value.Text,    
                             GetFont(ItemType.Paragraph)
                            ));
                    }
                    return cell;
                }
                private void WriteRequiredFiles(Document doc, Boolean forCompile, List<dtoCallSubmissionFile> requiredFiles, Dictionary<SubmissionTranslations, string> translations)
                {
                    doc.Add(GetPageTitle(translations[SubmissionTranslations.SubmittedFilesTitle], GetFont(ItemType.Title)));

                    iTextSharp5.text.List files = new iTextSharp5.text.List(false, 5);
                    foreach (dtoCallSubmissionFile r in requiredFiles)
                    {
                        if (forCompile)
                            files.Add(new iTextSharp5.text.ListItem(r.FileToSubmit.Name, GetFont(ItemType.Paragraph)));
                        else
                            files.Add(new iTextSharp5.text.ListItem(r.FileToSubmit.Name + '(' + (r.Submitted != null && r.Submitted != null ? translations[SubmissionTranslations.FileSubmitted] : translations[SubmissionTranslations.FileNotSubmitted]) + ')', GetFont(ItemType.Paragraph)));
                    }
                    doc.Add(files);
                }
                #endregion
            #endregion
        #endregion

        #region "Font Management"
            private enum FontSize
            {
                DocumentTitle = 22,
                Title = 18,
                SubTitle = 14,
                Paragraph = 12,
                Caption = 10,
                Small = 8
            }
            private enum ItemType{
                Title = 1,
                PrintInfos = 2,
                Paragraph = 3,
                Caption = 4,
                Header = 5,
                Footer = 6,
                PageNumber = 7,
                SectionName = 9,
                SectionDescription = 10,
                FieldName = 11,
                FieldDescription = 12,
                FieldNote = 13,
                SelectedItem = 14,
                DocumentTitle = 15,
                Default = 0,
                VoidSmall = 16
            }
            private Font GetFont(ItemType itemType)
            {
                int fontStyle = Font.NORMAL;
                float fontSize = 12;
                switch (itemType)
                {
                    case ItemType.Header:
                    case ItemType.Footer:
                        fontSize = (float)FontSize.Caption;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.PageNumber:
                        fontSize = (float)FontSize.Caption;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.PrintInfos:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.NORMAL;
                        break;
                    case ItemType.Paragraph:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.NORMAL;
                        break;
                    case ItemType.SectionName:
                        fontSize = (float)FontSize.Title;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.SectionDescription:
                        fontSize = (float)FontSize.SubTitle;
                        fontStyle = Font.NORMAL;
                        break;
                        
                    case ItemType.FieldName:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.FieldDescription:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.ITALIC;
                        break;
                    case ItemType.Title:
                        fontSize = (float)FontSize.Title;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.Caption:
                        fontSize = (float)FontSize.Caption;
                        fontStyle = Font.ITALIC;
                        break;
                    case ItemType.FieldNote:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.ITALIC;
                        break;
                    case ItemType.SelectedItem:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.DocumentTitle:
                        fontSize = (float)FontSize.DocumentTitle;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.VoidSmall:
                        fontSize = (float)FontSize.Small;
                        fontStyle = Font.NORMAL;
                        break;
                    default:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.NORMAL;
                        break;
                }
                return GetFont(FontFamilyname, fontSize, fontStyle, BaseColor.BLACK);
            }
            protected Paragraph GetSectionTitle(String name)
            {
                return GetParagraph(name, GetFont(ItemType.SectionName), 18, 30);
            }
            protected Paragraph GetWhiteParagraph()
            {
                return GetParagraph("\r\n", GetFont(ItemType.Paragraph), Element.ALIGN_LEFT, 12, 12);
            }

        private Font GetFont(CallPrintFontSets fontSet)
        {
            int fontStyle = Font.NORMAL; //0
            float fontSize = (float)fontSet.Size;
            //int fontVariant = Font.BOLDITALIC;

            if ((fontSet.Variant & FontVariant.Bold) == FontVariant.Bold)
            {
                fontStyle += Font.BOLD;
            }

            if ((fontSet.Variant & FontVariant.Italic) == FontVariant.Italic)
            {
                fontStyle += Font.ITALIC;
            }

            if ((fontSet.Variant & FontVariant.Underline) == FontVariant.Underline)
            {
                fontStyle += Font.UNDERLINE;
            }

            return GetFont(GetFontFamily(fontSet), fontSize, fontStyle, BaseColor.BLACK);
        }


        private string GetFontFamily(CallPrintFontSets fontSet)
        {

            //UNDEFINED = -1,
            //COURIER = 0,
            //HELVETICA = 1,
            //TIMES_ROMAN = 2,
            //SYMBOL = 3,
            //ZAPFDINGBATS = 4,

            string font = FontFamilyname;
            
            if (fontSet.FontName.ToLower() == "times" || string.IsNullOrEmpty(font))
                font = FontFactory.TIMES; //"TIMES_ROMAN";

            else if (fontSet.FontName.ToLower() == "helvetica")
                font = FontFactory.HELVETICA; //"HELVETICA";
            
            else if (fontSet.FontName.ToLower() == "courier")
                font = FontFactory.COURIER; //"COURIER";
            
            return font;
        }

            #endregion
    }
}
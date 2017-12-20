using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp5.text;
using iTextSharp5.text.pdf;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.File;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public class HelperExportToPDF : lm.Comol.Core.DomainModel.Helpers.Export.ExportPdf5BaseHelper

    {
        private Dictionary<SubmissionTranslations, string> Translations { get; set; }
        private String FontFamilyname { get; set; }
        private Font BaseFont { get; set; }
        private dtoExportSubmission Settings { get; set; }
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
                    ExportSubmission(doc, settings.Call, settings.Submitter, settings.RequiredFiles, settings.Sections, settings.PrintBy, Translations);
                else
                    ExportSubmission(doc, settings.Submission, settings.RequiredFiles, settings.Sections, settings.PrintBy, Translations);
            }

 
        #endregion

        #region "Export Submission"
            public Document Submission(Boolean openCloseConnection, UserSubmission submission, String fileName, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson person, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                return Submission(openCloseConnection,submission, "", fileName, requiredFiles, sections, person, webResponse, cookie);
            }
            public Document Submission(Boolean openCloseConnection, UserSubmission submission, String clientFilename, String fileName, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson person, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                dtoExportSubmission settings = new dtoExportSubmission() { ClientFilename = clientFilename, Filename = fileName, PrintBy = person, RequiredFiles = requiredFiles, Sections = sections, Submission = submission };

                return Submission(openCloseConnection, settings, webResponse, cookie);
            }
            public Document SubmissionToFile(Boolean openCloseConnection, UserSubmission submission, String fileName, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson person)
            {
                dtoExportSubmission settings = new dtoExportSubmission() { Filename = fileName, PrintBy = person, RequiredFiles = requiredFiles, Sections = sections, Submission = submission };

                return Submission(openCloseConnection, settings, null, null);
            }
            public Document SubmissionToCompile(Boolean openCloseConnection, BaseForPaper call, SubmitterType submitter, String clientFilename, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson person, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                dtoExportSubmission settings = new dtoExportSubmission() { ClientFilename = clientFilename, PrintBy = person, RequiredFiles = requiredFiles, Sections = sections, Call = call, Submitter = submitter };
                return Submission(openCloseConnection, settings, webResponse, cookie);
            }

            public Document Submission(Boolean openCloseConnection, dtoExportSubmission settings, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
            {
                Document doc = null;
                Settings = settings;
                if (settings.ForWebDownload) {
                    doc = WebExport(openCloseConnection,settings.ClientFilename, webResponse, cookie);
                    if (doc == null)
                        return GetErrorDocument(openCloseConnection,false,settings.Filename, webResponse, cookie);
                    else if (settings.SaveToFile) {
                        SubmissionToFile(settings);
                    }
                }
                else if (settings.SaveToFile) {
                    doc = SubmissionToFile(settings);
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
                private void ExportSubmission(Document doc, UserSubmission submission, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson person, Dictionary<SubmissionTranslations, string> translations)
                {
                    doc.Add(WriteSubmissionHeader(submission, person, translations));
                   // doc.Add(TextSeparator());
                    //doc.Add(TextSeparator());
                    doc.Add(GetPageTitle(translations[SubmissionTranslations.SubmissionInfo], GetFont(ItemType.Title)));
                    doc.Add(WriteSubmissionInfo(submission, submission.Status, translations));
                    doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                    ExportSubmission(doc, false, requiredFiles, sections, translations);
                }
                private void ExportSubmission(Document doc, BaseForPaper call, SubmitterType submitter, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, litePerson person, Dictionary<SubmissionTranslations, string> translations)
                {
                    doc.Add(WriteCallHeader(call, submitter, translations));
                    doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                    doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                    ExportSubmission(doc, true, requiredFiles, sections, translations);
                }
                private void ExportSubmission(Document doc, Boolean forCompile, List<dtoCallSubmissionFile> requiredFiles, List<dtoCallSection<dtoSubmissionValueField>> sections, Dictionary<SubmissionTranslations, string> translations)
                {
                    doc.NewPage();
                    if (sections != null)
                    {
                        foreach (dtoCallSection<dtoSubmissionValueField> section in sections)
                        {
                            Paragraph par = GetSectionTitle(section.Name);
                           
                            if (!String.IsNullOrEmpty(section.Description))
                            {
                                par.Add(Chunk.NEWLINE);
                                par.Add(GetPhrase(section.Description, GetFont(ItemType.SectionDescription)));
                            }
                            doc.Add(par);
                            WriteSubmissionSection(doc, forCompile, section, translations);
                            doc.Add(GetWhiteParagraph(GetFont(ItemType.Paragraph)));
                        }
                    }
                    if (requiredFiles.Count > 0)
                        WriteRequiredFiles(doc, forCompile, requiredFiles, translations);
                }
                private iTextSharp5.text.pdf.PdfPTable WriteCallHeader(BaseForPaper call, SubmitterType submitter, Dictionary<SubmissionTranslations, string> translations)
                {
                    iTextSharp5.text.pdf.PdfPTable table = GetTable(1);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;

                    String callName = "";
                    switch (call.Type)
                    {
                        case CallForPaperType.CallForBids:
                            if (string.IsNullOrEmpty(call.Edition))
                                callName = String.Format(translations[SubmissionTranslations.CallTitle], "\r\n" + call.Name);
                            else
                                callName = String.Format(translations[SubmissionTranslations.CallTitleAndEdition], "\r\n" + call.Name, call.Edition);
                            break;
                        case CallForPaperType.RequestForMembership:
                            callName = String.Format(translations[SubmissionTranslations.RequestTitle], "\r\n" + call.Name);
                            break;
                    }
                    PdfPCell titleCell = new PdfPCell(new Phrase(callName, GetFont(ItemType.Title)));
                    titleCell.Border = 0;
                    table.AddCell(titleCell);
                    return table;
                }
                private PdfPTable WriteSubmissionHeader(UserSubmission submission, litePerson person, Dictionary<SubmissionTranslations, string> translations)
                {
                    PdfPTable table = WriteCallHeader(submission.Call, submission.Type, translations);
                    PdfPCell separatortCell = new PdfPCell();
                    separatortCell.AddElement(GetWhiteParagraph());
                    separatortCell.Border = 0;
                    table.AddCell(separatortCell);
                    DateTime printDate = DateTime.Now;
                    String printedBy = String.Format(translations[SubmissionTranslations.PrintInfo], (person == null || person.TypeID == (int)UserTypeStandard.Guest) ? translations[SubmissionTranslations.AnonymousUser] : person.SurnameAndName, printDate.ToShortDateString(), printDate.ToShortTimeString());
                    PdfPCell printCell = new PdfPCell(new Phrase(printedBy, GetFont(ItemType.PrintInfos)));
                    printCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    printCell.Border = 0;
                    table.AddCell(printCell);

                    return table;
                }
                private PdfPTable WriteSubmissionInfo(UserSubmission submission, SubmissionStatus status, Dictionary<SubmissionTranslations, string> translations)
                {
                    PdfPTable table = GetTable(2, new float[] { 30, 70 }, 1);
                    PdfPCell cell = new PdfPCell(GetParagraph(translations[SubmissionTranslations.SubmittedByTitle], GetFont(ItemType.FieldName)));
                    table.AddCell(cell);
                    String owner = (submission.Owner == null || submission.Owner.TypeID == (int)UserTypeStandard.Guest) ? translations[SubmissionTranslations.AnonymousUser] : submission.Owner.SurnameAndName;
                    String submitter = (submission.SubmittedBy == null || submission.SubmittedBy.TypeID == (int)UserTypeStandard.Guest) ? translations[SubmissionTranslations.AnonymousUser] : submission.SubmittedBy.SurnameAndName;

                    if (!submission.SubmittedOn.HasValue)
                        cell = new PdfPCell(GetParagraph(String.Format(translations[SubmissionTranslations.CreatedByInfo], owner, submission.ModifiedOn.Value.ToShortDateString(), submission.ModifiedOn.Value.ToShortTimeString()), GetFont(ItemType.Paragraph)));
                    else if (submission.Owner == submission.SubmittedBy)
                        cell = new PdfPCell(GetParagraph(String.Format(translations[SubmissionTranslations.SubmittedByInfo], submitter, submission.SubmittedOn.Value.ToShortDateString(), submission.SubmittedOn.Value.ToShortTimeString()), GetFont(ItemType.Paragraph)));
                    else
                        cell = new PdfPCell(GetParagraph(String.Format(translations[SubmissionTranslations.SubmittedForInfo], submitter, owner, submission.SubmittedOn.Value.ToShortDateString(), submission.SubmittedOn.Value.ToShortTimeString()), GetFont(ItemType.Paragraph)));

                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(translations[SubmissionTranslations.SubmittedTypeTitle], GetFont(ItemType.FieldName)));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(submission.Type.Name, GetFont(ItemType.Paragraph)));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(translations[SubmissionTranslations.SubmissionStatusTitle], GetFont(ItemType.FieldName)));
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(translations[(SubmissionTranslations)(int)status], GetFont(ItemType.Paragraph)));
                    table.AddCell(cell);

                    return table;
                }

            #region "Export fields"
                private void WriteSubmissionSection(Document doc, Boolean forCompile, dtoCallSection<dtoSubmissionValueField> section, Dictionary<SubmissionTranslations, string> translations)
                {
                    PdfPTable tSection = GetTable(1);
                    String emptyItem = translations[SubmissionTranslations.EmptyItem];
                    String mandatory = translations[SubmissionTranslations.ItemMandatory];
                    String maxCharInfo = translations[SubmissionTranslations.MaxCharInfo];

                    foreach (dtoSubmissionValueField item in section.Fields)
                    {
                        PdfPCell fieldC = new PdfPCell();
                        Paragraph par = GetParagraph("", GetFont(ItemType.Paragraph), 0);
                        if (item.Field.Type != FieldType.Note)
                            par.Add(new Chunk(((item.Mandatory) ? "*" : "") + item.Field.Name, GetFont(ItemType.FieldName)));
                        fieldC.Border = Rectangle.NO_BORDER;
                        fieldC.BorderWidth = 0;

                        //if (!string.IsNullOrEmpty(item.Field.Description))
                        //    fieldC.AddElement(GetParagraph(item.Field.Description, (item.Field.Type != FieldType.Note) ? GetFont(ItemType.FieldDescription) : GetFont(ItemType.FieldNote)));
                        if (!string.IsNullOrEmpty(item.Field.Description))
                        {
                            par.Add(Chunk.NEWLINE);
                            par.Add(GetPhrase(item.Field.Description, (item.Field.Type != FieldType.Note) ? GetFont(ItemType.FieldDescription) : GetFont(ItemType.FieldNote)));
                        }
                        if (par != null)
                            fieldC.AddElement(par);
                        switch (item.Field.Type)
                        {
                            case FieldType.MultiLine:
                                tSection.AddCell(MultipleLineFieldToCell(fieldC, forCompile, item, emptyItem, maxCharInfo));
                                break;
                            case FieldType.Disclaimer:
                                tSection.AddCell(DisclaimerLineFieldToCell(fieldC, forCompile, item, emptyItem, translations));
                                break;
                            case FieldType.CheckboxList:
                            case FieldType.RadioButtonList:
                            case FieldType.DropDownList:
                                tSection.AddCell(MultipleChoiceFieldToCell(fieldC, forCompile, item, emptyItem, translations));
                                break;
                            case FieldType.FileInput:
                                tSection.AddCell(FileInputFieldToCell(fieldC, forCompile,item, emptyItem, translations));
                                break;
                            case FieldType.Note:
                                break;
                            default:
                                tSection.AddCell(AddGenericFieldToTable(fieldC, forCompile, item, emptyItem, maxCharInfo));
                                break;
                        }
                    }
                    doc.Add(tSection);
                }
                private PdfPCell AddGenericFieldToTable(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, String maxCharInfo)
                {
                    Chunk chunk = new Chunk((forCompile ? "" : (item.Value == null || String.IsNullOrEmpty(item.Value.Text) ? emptyItem : item.Value.Text)), GetFont(ItemType.Paragraph));
                    cell.AddElement(chunk);
                    if (forCompile && item.Field.MaxLength > 200)
                    {
                        Chunk ch = new Chunk("\r\n", GetFont(ItemType.Paragraph));
                        cell.AddElement(ch);
                    }

                    if (item.Field.MaxLength > 0)
                        cell.AddElement(GetParagraph(String.Format(maxCharInfo, item.Field.MaxLength), GetFont(ItemType.Caption)));
                    return cell;
                }
                private PdfPCell MultipleLineFieldToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, String maxCharInfo)
                {
                    Chunk chunk = new Chunk((forCompile ? "" : ((item.Value == null || String.IsNullOrEmpty(item.Value.Text)) ? emptyItem : item.Value.Text)), GetFont(ItemType.Paragraph));
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
                    }
                    else
                        cell.AddElement(chunk);
                    if (item.Field.MaxLength > 0)
                        cell.AddElement(GetParagraph(String.Format(maxCharInfo, item.Field.MaxLength), GetFont(ItemType.Caption)));
                    return cell;
                }
                private PdfPCell DisclaimerLineFieldToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, Dictionary<SubmissionTranslations, string> translations)
                {
                    switch (item.Field.DisclaimerType)
                    {
                        case DisclaimerType.None:
                            cell.AddElement(GetParagraph(emptyItem, GetFont(ItemType.Paragraph), Element.ALIGN_RIGHT));
                            break;
                        case DisclaimerType.Standard:
                            if (forCompile)
                                cell.AddElement(GetParagraph("( )  " + translations[SubmissionTranslations.DisclaimerAccept] + "    ( )  " + translations[SubmissionTranslations.DisclaimerReject], GetFont(ItemType.Paragraph), Element.ALIGN_RIGHT));
                            else
                                cell.AddElement(GetParagraph((item.Value != null && item.Value.Text == "True" ? translations[SubmissionTranslations.DisclaimerAccept] : translations[SubmissionTranslations.DisclaimerReject]), GetFont(ItemType.Paragraph), Element.ALIGN_RIGHT));
                            break;
                        case DisclaimerType.CustomDisplayOnly:
                            cell.AddElement(GetParagraph(emptyItem, GetFont(ItemType.Paragraph), Element.ALIGN_RIGHT));
                            break;
                        default:
                            MultipleChoiceFieldToCell(cell, forCompile, item, emptyItem, translations);
                            break;
                    }
                    return cell;
                }
                private PdfPCell MultipleChoiceFieldToCell(PdfPCell cell, Boolean forCompile, dtoSubmissionValueField item, String emptyItem, Dictionary<SubmissionTranslations, string> translations)
                {
                    //Chunk chunkUnselect = (item.Field.Type == FieldType.CheckboxList) ? whitesquare : whitebullet;
                    //Chunk chunkSelect = (item.Field.Type == FieldType.CheckboxList) ? new iTextSharp.text.Chunk("\u25A0", FontFactory.GetFont(FontFactory.ZAPFDINGBATS, 12, Font.NORMAL, Color.BLACK)) : new iTextSharp.text.Chunk("\u25CB", FontFactory.GetFont(FontFactory.ZAPFDINGBATS, 12, Font.NORMAL, Color.BLACK));
                    String unselect = (item.Field.Type == FieldType.CheckboxList) ? "[   ]  " : "(   )  ";
                    String freeTextSpace = (item.Field.Type == FieldType.CheckboxList) ? "       " : "       ";

                    String optOpen = (item.Field.Type == FieldType.CheckboxList) ? "[" : "(";
                    String optClose = (item.Field.Type == FieldType.CheckboxList) ? "]  " : ")  ";
                    Chunk chOpen = new Chunk(optOpen, GetFont(ItemType.Paragraph));
                    Chunk chSelected = new Chunk("X", GetFont(ItemType.SelectedItem));
                    Chunk chClose = new Chunk(optClose, GetFont(ItemType.Paragraph));
                    List<String> mValue = (item.Value == null || String.IsNullOrEmpty(item.Value.Text)) ? new List<String>() : item.Value.Text.Split('|').ToList();

                    Paragraph par = GetParagraph("", GetFont(ItemType.Paragraph),0);
                    par.IndentationLeft = 10;

                    foreach (dtoFieldOption opt in item.Field.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).ToList())
                    {
                        if (forCompile)
                        {
                            par.Add(new Chunk(unselect + opt.Name, GetFont(ItemType.Paragraph)));
                            if (opt.IsFreeValue)
                                par.Add(new Chunk("  ______________________________________________________________________", GetFont(ItemType.Paragraph)));
                            par.Add(Chunk.NEWLINE);
                        }
                        else
                        {
                            if (mValue.Contains(opt.Id.ToString()))
                            {
                                par.Add(chOpen);
                                par.Add(chSelected);
                                par.Add(chClose);
                                par.Add(new Chunk(opt.Name, GetFont(ItemType.SelectedItem)));
                                if (opt.IsFreeValue)
                                {
                                    if (!String.IsNullOrEmpty(item.Value.FreeText))
                                    {
                                        par.Add(Chunk.NEWLINE);
                                        par.Add(new Chunk(freeTextSpace + item.Value.FreeText, GetFont(ItemType.Paragraph)));
                                    }
                                    else
                                        par.Add(new Chunk("  ______________________________________________________________________", GetFont(ItemType.Paragraph)));
                                }
                                par.Add(Chunk.NEWLINE);
                            }
                            else
                            {
                                par.Add(new Chunk(unselect + opt.Name, GetFont(ItemType.Paragraph)));
                                if (opt.IsFreeValue)
                                    par.Add(new Chunk("  ______________________________________________________________________", GetFont(ItemType.Paragraph)));
                                par.Add(Chunk.NEWLINE);
                            }
                        }
                    }
                    cell.AddElement(par);
                    if (item.Field.Type == FieldType.CheckboxList)
                    {
                        if (item.Field.MaxOption > 0 && item.Field.MinOption == 0)
                            cell.AddElement(GetParagraph(String.Format(translations[SubmissionTranslations.MaxOption], item.Field.MaxOption), GetFont(ItemType.Caption)));
                        else if (item.Field.MaxOption == 0 && item.Field.MinOption > 0)
                            cell.AddElement(GetParagraph(String.Format(translations[SubmissionTranslations.MinOption], item.Field.MinOption), GetFont(ItemType.Caption)));
                        else if (item.Field.MinOption > 0 && item.Field.MaxOption > 0)
                            cell.AddElement(GetParagraph(String.Format(translations[SubmissionTranslations.MinOptionMaxOption], item.Field.MinOption, item.Field.MaxOption), GetFont(ItemType.Caption)));
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
                    else {
                        cell.AddElement(GetParagraph((item.Value == null || item.Value.IdLink==0) ? translations[SubmissionTranslations.FileNotSubmitted] : translations[SubmissionTranslations.FileSubmitted], GetFont(ItemType.Paragraph)));
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
                Title = 16,
                SubTitle = 14,
                Paragraph = 12,
                Caption = 10,
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
                Default = 0
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
                        fontSize = (float)FontSize.SubTitle;
                        fontStyle = Font.BOLD;
                        break;
                    case ItemType.Paragraph:
                        fontSize = (float)FontSize.Paragraph;
                        fontStyle = Font.NORMAL;
                        break;
                    case ItemType.SectionName:
                        fontSize = (float)FontSize.SubTitle;
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
            #endregion
    }
}
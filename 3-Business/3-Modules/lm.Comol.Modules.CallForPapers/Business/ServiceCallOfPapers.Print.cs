using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.DocTemplateVers.Business;
using lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Business
{
    public partial class ServiceCallOfPapers
    {
        public CallPrintSettings PrintSettingsGetFromCall(Int64 callId)
        {
            //CallForPaper call = Manager.Get<CallForPaper>(callId);

            //if (call == null)
            //    return new CallPrintSettings();


            ////ToDo: X TEST!!!!
            //IList<CallPrintSettings> pslist =  Manager.GetAll<CallPrintSettings>();

            //pslist = pslist.Where(s => s.CallId == callId).ToList();

            CallPrintSettings ps = Manager.GetAll<CallPrintSettings>(
                p => p.CallId == callId
                )
                .Skip(0).Take(1).FirstOrDefault();
                //pslist.FirstOrDefault();


            //CallPrintSettings ps = Manager.GetAll<CallPrintSettings>();
            //s => s.CallId == callId).Skip(0).Take(1).ToList().FirstOrDefault();

            if (ps == null)
            {
                ps = new CallPrintSettings();
            }

            return ps;
        }

        public CallPrintSettings PrintSettingsGet(Int64 settingsId)
        {

            CallPrintSettings ps = Manager.Get<CallPrintSettings>(settingsId);

            if (ps == null)
            {
                ps = new CallPrintSettings();
            }


            return ps;
        }

        public Int64 PrintSettingsSet(CallPrintSettings settings)
        {
            if(!Manager.IsInTransaction())
                Manager.BeginTransaction();


            CallPrintSettings ps = PrintSettingsGetFromCall(settings.CallId);
            litePerson person = Manager.GetLitePerson(UC.CurrentUserID);

            if (ps == null || ps.Id <= 0)
            {
                ps = new CallPrintSettings();
                ps.CreateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            }
            else
            {
                ps.UpdateMetaInfo(person, UC.IpAddress, UC.ProxyIpAddress);
            }

            ps.TemplateId = settings.TemplateId;
            ps.CallId = settings.CallId;
            ps.FieldContent = settings.FieldContent;
            ps.FieldDescription = settings.FieldDescription;
            ps.FieldTitle = settings.FieldTitle;
            ps.Layout = settings.Layout;
            ps.SectionDescription = settings.SectionDescription;
            ps.SectionTitle = settings.SectionTitle;
            ps.ShowMandatory = settings.ShowMandatory;
            ps.UnselectFields = settings.UnselectFields;
            ps.VersionId = settings.VersionId;
            ps.DraftWaterMark = settings.DraftWaterMark;
            ps.AllowPrintDraft = settings.AllowPrintDraft;

            try
            {
                Manager.SaveOrUpdate<CallPrintSettings>(ps);
                Manager.Commit();
            }
            catch (Exception ex)
            {
                if (Manager.IsInTransaction())
                    Manager.RollBack();

                return -1;
            }

            return ps.Id;
        }
        
        public Int64 CallIdGetFromRevision(Int64 RevisionId)
        {
            /*Int64 IdCall = revision.Submission.Call.Id;
           helper = new HelperExportToPDF(translations, template);
           */

            Int64 IdCall = (from Revision rev in Manager.GetIQ<Revision>()
                where rev.Submission != null
                      && rev.Submission.Call != null
                select rev.Submission.Call.Id).Distinct().FirstOrDefault();

            return IdCall;

        }

        public Int64 CallIdGetFromSubmission(Int64 SubmissionId)
        {
            /*Int64 IdCall = revision.Submission.Call.Id;
           helper = new HelperExportToPDF(translations, template);
           */

            Int64 IdCall = (from UserSubmission sub in Manager.GetIQ<UserSubmission>()
                            where sub.Call != null
                            && sub.Id == SubmissionId
                            select sub.Call.Id).Distinct().FirstOrDefault();

            return IdCall;

        }

        public DTO_Template DocTemplateUpdate(DTO_Template source, Int64 TemplateId, Int64 VersionId, string BasePath)
        {
            DTO_Template template = _DtService.TemplateGet(TemplateId, VersionId, BasePath);

            if (template == null)
            {
                return source;
            }

            if (template.UseSkinHeaderFooter)
            {
                template.Header = source.Header;
                template.Footer = source.Footer;
            }

            return template;
        }
        
    }
}

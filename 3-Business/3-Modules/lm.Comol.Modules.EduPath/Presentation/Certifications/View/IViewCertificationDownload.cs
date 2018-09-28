using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.Domain.DTO;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewCertificationDownload : IViewCertificationPageBase
    {
        long PreloadReferenceTime { get; }
        Boolean PreloadRestore { get; }
        Guid PreloadUniqueId { get; }
        Int32 ForUserId { get; set; }
        Int32 IdCommunityContainer { get; set; }
        long IdPath { get; set; }

        List<dtoQuizInfo> GetQuizInfos(List<long> idItems, Int32 idUser, Boolean isEvaluable);

        void GenerateAndDownload(long idPath, long idSubActivity,Boolean allowEmptyPlaceholders,Int32 idUser, String fileName,lm.Comol.Core.Certifications.CertificationType type, Boolean saveFile, Boolean saveAction);
        void DownloadCertification(Boolean allowEmptyPlaceholders, Int32 idUser, String fileName, lm.Comol.Core.Certifications.CertificationType type, Boolean saveFile, Boolean saveAction, Guid uniqueId, String extension);


        void RestoreCertificate(Boolean allowEmptyPlaceholders, Int32 idUser, String webFileName, lm.Comol.Core.Certifications.CertificationType type, Boolean saveFile);
        String GetDefaulFileName();
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.Domain.DTO;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewModuleCertificationAction : lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction
    {
        lm.Comol.Core.DomainModel.ContentView PreLoadedContentView {get;}
        //String DestinationUrl {get;}
        //String PreviewCertificationUrl { get; }
        //String GetBaseUrl();
        //String CookieName { get; set; }
        //Boolean SaveRunningAction { get; set; }
        //Boolean SaveCertification { get; set; }
        Boolean AutoGenerated { get; set; }
        Boolean InsideOtherModule {get;set;}
        //Boolean RefreshContainer { get; set; }
        Boolean EvaluablePath { get; set; }
        Boolean AllowWithEmptyPlaceHolders { get; set; }
        String CertificationFilePath { get; }

        long IdPath { get; set; }
        long IdTemplate { get; set; }
        long IdTemplateVersion { get; set; }
        long IdUnit { get; set; }
        long IdActivity { get; set; }
        long IdSubActivity { get; set; }
        String ItemIdentifier {get;set;}
        Int32 ForUserId {get;set;}
        Int32 IdCommunityContainer { get; set; }
        String GetBaseUrl(Boolean useSSL);
        String CertificationName { get; set; }
        void DisplayUnknownAction();
        void DisplayUnselectedTemplate();
        void DisplayUnselectedTemplateInfo();
        void DisplayRemovedTemplate();

        void DisplayEmptyAction();
        void DisplayEmptyActions();
        void DisplayActions(List<dtoModuleActionControl> actions);
        void DisplayPlaceHolders(List<dtoPlaceHolder> items);
        void DisplayItem(string name);

        void DisplayItemAdminInfo(dtoSubActivity cAction,List<dtoSubActivityLink> links, Path p);
        //void DisplayItemForGenerate(string name, Boolean saveCertification);
        //void DisplayItem(string name, Guid fileUniqueId ,String fileExtension);
        void DisplayDownloadUrl(String name,String url, Boolean alreadyCreated, Boolean refreshContainer);

        List<dtoQuizInfo> GetQuizInfos(List<long> idItems);
        Boolean IsCertificationActive(long idPath, Int32 idUser, dtoSubActivity item, DateTime viewStatBefore);
        //Boolean GenerateCertification(long idPath, Int32 idUser, dtoSubActivity item, DateTime viewStatBefore, StatusStatistic status);
        lm.Comol.Core.Certifications.CertificationError AutoGenerateCertification(long idPath, Int32 idCommunity, Int32 idUser, dtoSubActivity item, DateTime viewStatBefore, StatusStatistic status, out StatusStatistic newstatus);
        Boolean HasCertificationToAutoGenerate(long idPath, Int32 idCommunity, Int32 idUser, dtoSubActivity item, DateTime viewStatBefore);

        String getDescriptionByActivity(dtoSubActivity cActio);
        //void DownloadCertification(long idPath, Int32 idUser, dtoSubActivity item, DateTime viewStatBefore, String cookieValue);
        //void RestoreCertificate(long idPath, Int32 idUser, dtoSubActivity item, String cookieValue);
    }
}
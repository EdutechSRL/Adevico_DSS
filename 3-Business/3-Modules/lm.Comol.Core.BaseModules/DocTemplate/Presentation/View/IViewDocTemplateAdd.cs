using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel.DocTemplateVers;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public interface IViewDocTemplateAdd : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        //Bind view (Add or Copy)
        Int64 PreloadedTemplateId { get; }
        Int64 PreloadedVersionId { get; }
        void BindView(lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_AddTemplate dtoAdd);

        //Data to create
        Boolean HasServices { get; }
        IList<Int64> SelectedServicesId { get; }
        lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType TemplateType { get; }
        String TemplateName { get; }
        void GoToEdit(Int64 TemplateId);
        void GoToList(String Url);

        //Base (permission/action)
        ModuleDocTemplate CurrentPermission { get; set; }
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
        void SendUserAction(int idCommunity, int idModule, ModuleDocTemplate.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idTemplate, ModuleDocTemplate.ActionType action);

        void ShowError(AddError error);

        //Navigation
        String PreloadBackUrl { get; }

        void SetBackUrls(string BackUrl, string ListUrl);

        String TemplateBasePath { get; }
        String TemplateTempPath { get; }
    }

    public enum AddError
    {
        invalidName = 0,
        genericError = 1
    }
}


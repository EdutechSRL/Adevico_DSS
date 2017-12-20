using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewScormPackageSettings : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String PreloadedBackUrl { get; }
        long PreloadedFileId { get; }
        long PreloadedLinkId { get; }
        String BackUrl { get; set; }
        long ModuleLinkId { get; set; }
        long FileId { get; set; }

        //void ShowSessionTimeout();
        void SendToSessionExpiredPage(int idCommunity, String languageCode);
        void ShowNoPermissionToEditMetadata(int IdCommunity, int IdModule, String ModuleCode, String filename);
        void ShowNoScormFile(String filename);
        void ShowUnkownFile(int IdCommunity, int IdModule, String ModuleCode);
        void InitializeMetadataControl(System.Guid fileUniqueId, String filename,ScormMetadataPermission permission );
        void UpdateMetadataControl();
        ScormMetadataPermission GetModuleLinkPermission(int IdCommunity,long LinkID, ModuleObject sourceObject, ModuleObject linkedObject,int UserID);
    }
}
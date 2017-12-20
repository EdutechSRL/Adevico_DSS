using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewEditViewDetailsPageBase : IViewItemPageBase
    {
        #region "Preload"
            long PreloadIdFolder { get; }
            String PreloadIdentifierPath { get; }
        #endregion

        #region "Context"
            String GetRepositoryDiskPath();
            long IdCurrentFolder { get; set; }
            Boolean IsInitialized { get; set; }
            FolderType CurrentFolderType { get; set; }
            String CurrentFolderIdentifierPath { get; set; }
            String RepositoryIdentifier { get; set; }
            lm.Comol.Core.FileRepository.Domain.RepositoryType RepositoryType { get; set; }
            Int32 RepositoryIdCommunity { get; set; }
            String DefaultLogoutUrl { get; set; }
        #endregion

        #region "Stop PostBack"
            Guid PageIdentifier { get; set; }
            long OperationTicket { get; set; }
            Dictionary<Guid, long> CurrentOperations { get; }
            long GetCurrentOperationTicket(Guid idPage);
            void AddCurrentTicket(Guid idPage, long idOperationTicket);
            Boolean isValidOperation();
        #endregion
        
        #region "Translations"
            Dictionary<FolderType, String> GetFolderTypeTranslation();
            Dictionary<ItemType, String> GetTypesTranslations();
            String GetUnknownUserName();
            String GetRootFolderFullname();
            String GetRootFolderName();
        #endregion
      
        String GetCurrentUrl();
        String GetPreviousUrl();
        String GetPreviousRelativeUrl();
        void DisplayBackUrl(String url);
        void GoToUrl(String url);
        lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository GetRepositoryCookie(RepositoryIdentifier identifier);
    }
}
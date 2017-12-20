using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewDownloadError : IViewPageBase
    {
        long PreloadIdItem { get; }
        long PreloadIdVersion { get; }
        Int32 PreloadIdModule { get; }
        long PreloadIdLink { get; }
        Guid PreloadIdNews { get; }
        Guid PreloadWorkingSessionId { get; }
        DownloadErrorType PreloadErrorType { get; }
        void InitializePortalView(String fullname,String fileExtension, DownloadErrorType errorType);
        void InitializeCommunityView(String fullname,String fileExtension, DownloadErrorType errorType, Int32 idCommunity,String communityName);
        void InitializeView(DownloadErrorType errorType);
        void InitializeContext(lm.Comol.Core.DomainModel.Helpers.ExternalPageContext context = null);
        lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository GetRepositoryCookie(RepositoryIdentifier identifier);
    }
}
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewRepositoryPageBase : IViewPageBase
    {
        #region "Preload"
            long PreloadIdItem { get; }
            Int32 PreloadIdPerson { get; }
            lm.Comol.Core.FileRepository.Domain.RepositoryType PreloadType { get; }
            String PreloadIdentifierPath { get; }
        #endregion

        lm.Comol.Core.FileRepository.Domain.RepositoryType RepositoryType { get; set; }
        String GetRepositoryDiskPath();
        Guid PageIdentifier { get; set; }
        long OperationTicket { get; set; }
        Dictionary<Guid, long> CurrentOperations { get; }
        long GetCurrentOperationTicket(Guid idPage);
        void AddCurrentTicket(Guid idPage, long idOperationTicket);
        Boolean isValidOperation();
        void WriteRepositoryCookie(lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository item);
        lm.Comol.Core.BaseModules.FileRepository.Domain.cookieRepository GetRepositoryCookie(RepositoryIdentifier identifier);
        void RedirectToUrl(String url);
    }
}
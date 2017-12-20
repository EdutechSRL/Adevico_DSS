using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewOtherModuleItemFilesGuid<statusIDtype> : IViewOtherModuleItemFiles 
    {
        System.Guid  ItemID { get; set; }
        void InitalizeControl(System.Guid ItemID,iCoreItemPermission ItemPermissions , IList<iCoreItemFileLink<System.Guid>> links, String publishUrl);
        void InitalizeControl(System.Guid ItemID, iCoreItemPermission ItemPermissions,IList<iCoreItemFileLink<System.Guid>> links, IList<TranslatedItem<statusIDtype>> statusList, String publishUrl);
        void LoadFiles(IList<iCoreItemFileLinkPermission<System.Guid>> files);
        IList<GenericItemStatus<System.Guid, statusIDtype>> GetItemFilesStatus();

    }
}
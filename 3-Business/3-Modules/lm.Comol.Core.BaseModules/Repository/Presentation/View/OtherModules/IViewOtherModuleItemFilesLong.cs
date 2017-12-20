using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewOtherModuleItemFilesLong : IViewOtherModuleItemFiles 
    {
        long ItemID { get; set; }
        void InitalizeControl(long ItemID,iCoreItemPermission ItemPermissions , IList<iCoreItemFileLink<long>> links, String publishUrl);
        void InitalizeControl(long ItemID,iCoreItemPermission ItemPermissions , IList<iCoreItemFileLink<long>> links, IList<TranslatedItem<long>> statusList, String publishUrl);
        void LoadFiles(IList<iCoreItemFileLinkPermission<long>> files);
        IList<GenericItemStatus<long, long >> GetItemFilesStatus();
    }
}
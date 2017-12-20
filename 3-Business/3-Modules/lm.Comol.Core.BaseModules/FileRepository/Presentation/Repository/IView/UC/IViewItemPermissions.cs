using lm.Comol.Core.BaseModules.FileRepository.Domain;
using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation
{
    public interface IViewItemPermissions : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            long IdRepositoryItem { get; set; }
            ItemType RepositoryItemType { get; set; }
            Boolean HasPendingChanges { get; set; }
            Boolean AllowUpload { get; set; }
        
        #endregion
        
        void InitializeControl(Boolean editMode, dtoDisplayRepositoryItem item);

        #region "Translations"
            Dictionary<FolderType, String> GetFolderTypeTranslation();
            Dictionary<ItemType, String> GetTypesTranslations();
            String GetUnknownUserName();
        #endregion
        void DisplayUserMessage(UserMessageType messageType);
        void DisplayUserMessage(UserMessageType messageType,Int32 items);
        void DisplaySessionTimeout();
        void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, long idItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType objType);

        void InitializeCommands(Boolean allowAddRole,  Boolean allowAddUsers);
        void InitializeRolesSelector(List<lm.Comol.Core.DomainModel.dtoTranslatedRoleType> roles);
        void InitializePortalUsersSelector(List<Int32> removeUsers);
        void InitializeMyUsersSelector(List<Int32> idCommunities,List<Int32> removeUsers);
        void InitializeUsersSelector(Int32 idCommunity, List<Int32> removeUsers);
        void LoadAssignments(List<dtoEditAssignment> assignments);
        List<dtoEditAssignment> GetPermissions();
        Boolean HasItemsToSave();
        void AskUserForApply(String name);
    }
}
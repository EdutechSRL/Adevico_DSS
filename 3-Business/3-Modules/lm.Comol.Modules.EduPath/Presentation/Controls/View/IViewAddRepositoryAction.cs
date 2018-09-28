using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.DomainModel.Repository;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewAddRepositoryAction  : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        long IdActivity { get; set; }
        List<long> UnloadItems { get; set; }
        RepositoryIdentifier Identifier { get; set; }
        DisplayRepositoryAction CurrentAction { get; set; }

        void InitializeControl(RepositoryIdentifier identifier,long idAction, List<long> unloadItems = null);
        void InitializeLinkRepositoryItems(Int32 idUser, ModuleRepository rPermissions, RepositoryIdentifier identifier, List<long> alreadyLinkedFiles, ItemType type);
        void InitializeCommunityUploader(RepositoryIdentifier identifier);
        void InitializeInternalUploader(RepositoryIdentifier identifier);

        void DisplayWorkingSessionExpired(Int32 idCommunity, Int32 idModule);
        void DisplayItemsAdded();
        void DisplayItemsNotAdded();
        void DisplayNoFilesToAdd();
        void DisplayActivityNotFound();

        List<dtoModuleUploadedItem> UploadFiles(String moduleCode, Int32 idObjectType, Int32 idAction,  Boolean addToRepository);

        void LoadAvailableActions(List<DisplayRepositoryAction> actions, DisplayRepositoryAction dAction);
        void DisplayAction(DisplayRepositoryAction action);
        void ChangeDisplayAction(DisplayRepositoryAction action);
        String GetActionTitle(DisplayRepositoryAction action);
        Boolean IsCreateActionAvailable { get; set; }
    }
}
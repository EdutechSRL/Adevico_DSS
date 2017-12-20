using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewImportItemsToRepository : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String Portalname {get;}
        String BaseFolder {get;}
        String CommunityTitle {get;}
        Int32 PreloadPageIndex {get;}
        long PreloadIdFolder {get;}
        Int32 PreloadIdCommunity {get;}
        //ReadOnly Person PreLoadedView() As IViewExploreCommunityRepository.ViewRepository
        Boolean AllowImport {set;}

        Int32 SourceIdCommunity {get;set;}
        String SourceCommunityName {set;}
        Int32 DestinationIdCommunity {get;set;}
        String DestinationCommunityName {set;}
        Boolean VisibleToAll {get;set;}
        String RepositoryPath {set;}


        // WriteOnly Person AllowManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository) As Boolean


    //    Sub NoPermissionToManagementFiles(ByVal CommunityID As Integer)
    //    Sub NoPermissionToImportItems(ByVal CommunityID As Integer)
    //    Sub NoPermissionToAccessPage(ByVal CommunityID As Integer)

    //    Sub InitializeSourceItemsSelector(ByVal CommunityID As Integer, ByVal ShowHiddenItems As Boolean, ByVal AdminPurpose As Boolean)


    //    Sub LoadMultipleFileName(ByVal oList As List(Of dtoFileExist(Of Long)))
    //    Person SelectedFolder() As Long

    //    ReadOnly Person SelectedFolderName() As String
    //    ReadOnly Person CommunitySelectionLoaded() As Boolean

    //    Sub InitCommunitySelection(ByVal CommunityID As Integer)
    //    Sub InitializeFolderSelector(ByVal CommunityID As Integer, ByVal SelectedFolderID As Long, ByVal ShowAllFolder As Boolean)
    //    Sub ShowFoldersList()
    //    Sub ShowSelectCommunity()
    //    Sub ShowFileList()
    //    Sub ShowRenamedFileList()

    //    Sub ReturnToFileManagement(ByVal FolderID As Long, ByVal CommunityID As Integer, ByVal oView As IViewExploreCommunityRepository.ViewRepository)

    //    Function GetChangedFileName() As List(Of dtoFileExist(Of Long))

    //    Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
    //    Sub SendActionImportCompleted(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
    //    Sub NotifyImportedItems(ByVal CommunityID As Integer, ByVal oContext As dtoImportedItem)
    //End Interface
    }
}
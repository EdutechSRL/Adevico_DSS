Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports lm.Comol.Core

Namespace lm.Comol.Modules.Base.Presentation
    Public Class MultipleFileUploaderPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Public Overloads Property CurrentManager() As ManagerCommunityFiles
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunityFiles)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Public Overloads ReadOnly Property View() As IviewMultipleFileUploader
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewMultipleFileUploader)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView(ByVal FolderID As Long, ByVal CommunityID As Integer)
            If FolderID <> 0 Then
                Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderID)
                If IsNothing(oFolder) OrElse oFolder.isFile Then
                    FolderID = 0
                ElseIf Not IsNothing(oFolder.CommunityOwner) AndAlso oFolder.CommunityOwner.Id <> CommunityID Then
                    CommunityID = oFolder.CommunityOwner.Id
                End If
            End If
            Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
            If CommunityID > 0 AndAlso oCommunity Is Nothing Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If

            Me.View.RepositoryCommunityID = CommunityID
            If CommunityID = 0 Then
                Me.View.CommunityName = Me.View.Portalname
            Else
                oCommunity = Me.CurrentManager.GetCommunity(CommunityID)
                Me.View.CommunityName = oCommunity.Name
            End If

            Me.View.AllowFolderChange = Me.CurrentManager.CommunityHasFolders(CommunityID)
            Me.View.FolderID = FolderID
            Dim FolderPath As String = Me.View.BaseFolder
            If FolderID > 0 Then
                Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(FolderID)
                FolderPath &= Me.CurrentManager.GetFolderPathName(FolderID)
            End If
            Me.View.FilePath = FolderPath
            Me.View.DownlodableByCommunity = True
            Me.View.VisibleToDonwloaders = True
            Me.View.LoadFolderSelector(FolderID, CommunityID, False, False)
        End Sub

        Public Function AddFileToRepository(ByVal oFile As CommunityFile, ByVal SavedFile As String, ByVal CommunityPath As String) As dtoUploadedFile
            Dim iResponse As New dtoUploadedFile(oFile, ItemRepositoryStatus.CreationError)
            If Not IsNothing(oFile) AndAlso File.Exists.File(SavedFile) Then
                Dim SavedFolder As CommunityFile = Nothing
                Dim CommunityID As Integer = Me.View.RepositoryCommunityID
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                Dim fileStatus As ItemRepositoryStatus = ItemRepositoryStatus.None
                If (CommunityID > 0 AndAlso Not IsNothing(CommunityID)) OrElse (IsNothing(oCommunity) AndAlso CommunityID = 0) Then
                    oFile.Owner = Me.CommonManager.GetPerson(Me.UserContext.CurrentUserID)
                    oFile.CommunityOwner = oCommunity
                    oFile.CreatedOn = Now
                    oFile.ModifiedOn = Now
                    oFile.ModifiedBy = oFile.Owner

                    Dim Permission As Long = UCServices.Services_File.Base2Permission.DownloadFile
                    If Me.View.DownlodableByCommunity Then
                        SavedFolder = Me.CurrentManager.AddFile(oFile, CommunityPath, Permission, fileStatus)
                    Else
                        SavedFolder = Me.CurrentManager.AddFile(oFile, oFile.Owner, CommunityPath, Permission, fileStatus)
                    End If
                    If Not IsNothing(SavedFolder) Then
                        iResponse.Link = (Me.CurrentManager.CreateModuleActionLink(SavedFolder, UCServices.Services_File.Base2Permission.DownloadFile, Me.CurrentManager.ModuleId))
                    End If
                End If
                iResponse.Status = fileStatus
                'If IsNothing(SavedFolder) Then
                '    iResponse.Status = ItemRepositoryStatus.FileExist
                'ElseIf SavedFolder.Id > 0 Then
                '    iResponse.Status = ItemRepositoryStatus.FileUploaded
                'End If
            End If
            Return iResponse
        End Function
        Public Function GetFolderName(ByVal FolderID As Long) As String
            Return Me.CurrentManager.GetFolderName(FolderID)
        End Function
    End Class
End Namespace
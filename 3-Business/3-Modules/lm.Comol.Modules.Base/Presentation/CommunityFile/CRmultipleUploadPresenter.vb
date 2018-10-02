Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRmultipleUploadPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon

        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_File.Codex)
                End If
                Return _ModuleID
            End Get
        End Property

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

        Public Overloads ReadOnly Property View() As IViewCommunityFileMultipleUpload
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityFileMultipleUpload)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub


        Public Sub InitView()
            Dim CommunityID As Integer = Me.View.PreloadedCommunityID
            If CommunityID = 0 Then
                CommunityID = Me.UserContext.CurrentCommunityID
            End If
            If Not Me.UserContext.isAnonymous Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityID)
                If oCommunity Is Nothing AndAlso CommunityID <> 0 Then
                    Me.View.NoPermission(CommunityID, Me.ModuleID)
                Else
                    Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)
                    Dim FolderID As Long = Me.View.PreloadedFolderID
                    Me.View.AllowBackToDownloads(FolderID, CommunityID, Me.View.PreloadedPreviousView) = oPermission.Administration OrElse oPermission.ListFiles
                    Me.View.AllowBackToManagement(FolderID, CommunityID, Me.View.PreloadedPreviousView) = oPermission.Administration
                    If oPermission.Administration OrElse oPermission.UploadFile Then
                        If Me.View.PreloadedFolderID > 0 Then
                            If Not Me.CurrentManager.HasPermissionToSeeItem(Me.View.PreloadedFolderID, oPermission.Administration, oPermission.Administration, Me.UserContext.CurrentUserID) Then
                                Me.View.NoPermission(CommunityID, Me.ModuleID)
                                Exit Sub
                            End If
                        End If
                        Me.View.RepositoryCommunityID = CommunityID
                        Me.View.ActionInitialize(CommunityID, Me.ModuleID)
                        Me.View.InitializeUploader(Me.View.PreloadedFolderID, CommunityID, oPermission)
                    Else
                        Me.View.NoPermissionToAdd(CommunityID, Me.ModuleID)
                    End If
                End If
            Else
                Me.View.NoPermission(CommunityID, Me.ModuleID)
            End If
        End Sub


        Public Sub GotoManagementDownloadPage(ByVal oFile As CommunityFile)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            Dim FolderID As Long = 0
            If Not IsNothing(oFile) Then
                If oFile.CommunityOwner Is Nothing Then
                    CommunityID = 0
                Else
                    CommunityID = oFile.CommunityOwner.Id
                End If
                FolderID = oFile.FolderId

                Me.View.ActionUpload(CommunityID, Me.ModuleID, oFile.Id)
            End If
            Me.View.LoadRepositoryPage(CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
        End Sub
        Public Sub GotoPermissionManagement(ByVal oFile As CommunityFile)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            Dim FolderID As Long = 0
            If Not IsNothing(oFile) Then
                If oFile.CommunityOwner Is Nothing Then
                    CommunityID = 0
                Else
                    CommunityID = oFile.CommunityOwner.Id
                End If
                FolderID = oFile.FolderId
                '  Me.View.LoadEditingPermission(oFile.Id, CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
            Else
                Me.View.LoadRepositoryPage(CommunityID, FolderID, Me.View.PreloadedPreviousView, Me.View.PreLoadedPage)
            End If
        End Sub
    End Class
End Namespace
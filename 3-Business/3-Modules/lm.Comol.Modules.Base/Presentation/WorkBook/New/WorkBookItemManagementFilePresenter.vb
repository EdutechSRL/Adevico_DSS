Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WorkBookItemManagementFilePresenter
        Inherits DomainPresenter

        Private _ModuleID As Integer
        Private _CommonManager As ManagerCommon
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_WorkBook.Codex)
                End If
                Return _ModuleID
            End Get
        End Property
        Private Overloads Property CommonManager() As ManagerCommon
            Get
                Return _CommonManager
            End Get
            Set(ByVal value As ManagerCommon)
                _CommonManager = value
            End Set
        End Property

        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IWorkBookItemManagementFile
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWorkBookItemManagementFile)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            If Not Me.UserContext.isAnonymous Then
                Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                If IsNothing(oItem) Then
                    Me.View.NoPermissionToManagementFiles(CommunityID)
                Else
                    Dim oPermission As WorkBookItemPermission = GetWorkBookItemPermission(oItem)
                    If oPermission.Write OrElse oPermission.Read Then
                        Dim oFilePermission As New ModuleCommunityRepository
                        If oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                            Me.View.AllowCommunityUpload = False
                            Me.View.AllowCommunityLink = False
                        Else
                            oFilePermission = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
                            Me.View.AllowCommunityUpload = oPermission.Write AndAlso (oFilePermission.Administration OrElse oFilePermission.UploadFile)
                            Me.View.AllowCommunityLink = oPermission.Write AndAlso (oFilePermission.Administration OrElse oFilePermission.UploadFile OrElse oFilePermission.ListFiles OrElse oFilePermission.DownLoad)
                            If oPermission.Write AndAlso (oFilePermission.Administration OrElse oFilePermission.UploadFile) Then
                                Me.View.InitializeCommunityUploader(0, oItem.WorkBookOwner.CommunityOwner.Id, oFilePermission)
                            End If
                        End If

                        Me.View.AllowUpload = oPermission.Write
                        Me.View.BackToWorkbook = oItem.WorkBookOwner.Id
                        Me.View.SetBackToItemUrl(oItem.WorkBookOwner.Id, oItem.Id)
                        Me.View.SetMultipleUploadUrl(oItem.Id, Me.View.PreviousWorkBookView)
                        If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                            CommunityID = oItem.WorkBookOwner.CommunityOwner.Id
                        Else
                            CommunityID = 0
                        End If
                        Me.View.LoadFilesToManage(oItem.Id, CommunityID, oPermission.Write, oPermission, oFilePermission, Me.View.AllowPublish)
                    Else
                        Me.View.ReturnToItemsList(Me.View.PreloadedItemID)
                    End If
                End If
            Else
                Me.View.NoPermissionToManagementFiles(CommunityID)
            End If
        End Sub
        Public Sub AddCommunityFile(ByVal CommunityFiles As MultipleUploadResult(Of dtoUploadedFile), ByVal ForAllCommunityMembers As Boolean)
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            If IsNothing(oItem) OrElse IsNothing(CommunityFiles) OrElse CommunityFiles.UploadedFile.Count = 0 Then
                If CommunityFiles.NotuploadedFile.Count = 0 Then
                    Me.View.ReturnToFileManagement(Me.View.PreloadedItemID, Me.View.PreviousWorkBookView)
                Else
                    Me.View.ReturnToFileManagementWithErrors(Nothing, CommunityFiles.NotuploadedFile, Me.View.PreviousWorkBookView)
                End If
            Else
                Dim CommunityId As Integer = 0
                If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                    CommunityId = oItem.WorkBookOwner.CommunityOwner.Id
                End If

                Me.CurrentManager.AddCommunityFilesToItem(Me.View.PreloadedItemID, (From f In CommunityFiles.UploadedFile Select CLng(f.File.Id)).ToList(), Me.UserContext.CurrentUserID)
                Me.View.AddCommunityFileAction(CommunityId, Me.ModuleID)

                Dim isPersonal As Boolean = oItem.WorkBookOwner.isPersonal
                Dim Authors As List(Of Integer) = (From a In oItem.WorkBookOwner.Authors Select a.Id).ToList
                If CommunityFiles.UploadedFile.Count > 0 Then
                    Me.View.NotifyAddCommunityFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                End If

                If CommunityFiles.NotuploadedFile.Count = 0 Then
                    If ForAllCommunityMembers Then
                        Me.View.ReturnToFileManagement(Me.View.PreloadedItemID, Me.View.PreviousWorkBookView)
                    Else
                        Me.View.LoadEditingPermission(CommunityFiles.UploadedFile(0).File.Id, CommunityId, CommunityFiles.UploadedFile(0).File.FolderId, RepositoryPage.WorkBookManagementFile, Me.View.PreloadedItemID)
                    End If
                Else
                    Me.View.ReturnToFileManagementWithErrors(Nothing, CommunityFiles.NotuploadedFile, Me.View.PreviousWorkBookView)
                End If

            End If
        End Sub


        Public Sub AddInternalFile(ByVal InternalFiles As List(Of lm.Comol.Core.DomainModel.BaseFile))
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            If IsNothing(oItem) OrElse IsNothing(InternalFiles) OrElse InternalFiles.Count = 0 Then
                Me.View.ReturnToFileManagement(Me.View.PreloadedItemID, Me.View.PreviousWorkBookView)
            Else
                Dim CommunityId As Integer = 0
                If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                    CommunityId = oItem.WorkBookOwner.CommunityOwner.Id
                End If
                Me.CurrentManager.AddInternalFilesToItem(Me.View.PreloadedItemID, InternalFiles, Me.UserContext.CurrentUserID)
                Me.View.AddInternalFileAction(CommunityId, Me.ModuleID)

                Dim isPersonal As Boolean = oItem.WorkBookOwner.isPersonal
                Dim Authors As List(Of Integer) = (From a In oItem.WorkBookOwner.Authors Select a.Id).ToList
                If InternalFiles.Count > 0 Then
                    Me.View.NotifyAddInternalFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                End If

                Me.View.ReturnToFileManagement(Me.View.PreloadedItemID, Me.View.PreviousWorkBookView)
            End If
        End Sub
        Private Function GetWorkBookItemPermission(ByVal oItem As WorkBookItem) As WorkBookItemPermission
            Dim ModulePermission As ModuleWorkBook
            If oItem.WorkBookOwner.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oItem.WorkBookOwner.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookItemPermission = Me.CurrentManager.GetWorkBookItemPermission(Me.UserContext.CurrentUserID, oItem, ModulePermission)

            If IsNothing(oPermission) Then
                Return New WorkBookItemPermission
            Else
                Return oPermission
            End If
        End Function

        Public Sub ReloadManagementFileView()
            If Not Me.UserContext.isAnonymous Then
                Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                If IsNothing(oItem) Then
                    Me.View.NoPermissionToManagementFiles(Me.UserContext.CurrentCommunityID)
                Else
                    Dim oPermission As WorkBookItemPermission = GetWorkBookItemPermission(oItem)
                    If oPermission.Write OrElse oPermission.Read Then
                        Dim oFilePermission As New ModuleCommunityRepository
                        If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                            oFilePermission = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
                        End If
                        Dim CommunityID As Integer = 0
                        If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                            CommunityID = oItem.WorkBookOwner.CommunityOwner.Id
                        Else
                            CommunityID = 0
                        End If
                        Me.View.LoadFilesToManage(oItem.Id, CommunityID, oPermission.Write, oPermission, oFilePermission, Me.View.AllowPublish)
                    Else
                        Me.View.ReturnToItemsList(Me.View.PreloadedItemID)
                    End If
                End If
            Else
                Me.View.NoPermissionToManagementFiles(Me.UserContext.CurrentCommunityID)
            End If
        End Sub
    End Class
End Namespace
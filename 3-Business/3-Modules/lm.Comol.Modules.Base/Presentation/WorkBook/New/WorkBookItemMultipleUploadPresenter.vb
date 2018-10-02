Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WorkBookItemMultipleUploadPresenter
        Inherits DomainPresenter

        Private _CommonManager As ManagerCommon
        Private _ModuleID As Integer
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
        Private Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Private Overloads ReadOnly Property View() As IWorkbookMultipleFileUpload
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWorkbookMultipleFileUpload)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            If Not Me.UserContext.isAnonymous Then
                Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                If IsNothing(oItem) Then
                    Me.View.NoPermissionToAddFiles(CommunityID, Me.ModuleID)
                ElseIf HasPermission(oItem) Then
                    If oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                        Me.View.AllowCommunityUpload = False
                    Else
                        Dim oFilePermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
                        Me.View.AllowCommunityUpload = oFilePermission.Administration OrElse oFilePermission.UploadFile
                        If oFilePermission.Administration OrElse oFilePermission.UploadFile Then
                            Me.View.InitializeCommunityUploader(oItem.WorkBookOwner.CommunityOwner.Id, oFilePermission)
                        End If
                    End If
                    Me.View.SetUrlToFileManagement(oItem.Id, Me.View.PreviousWorkBookView)
                    Me.View.SetUrlToWorkbook(oItem.WorkBookOwner.Id, oItem.Id, Me.View.PreviousWorkBookView)
                    Me.View.AllowUpload = True
                    'Me.View.BackToItem = oItem.Id
                Else
                    Me.View.ReturnToFileManagement(Me.View.PreloadedItemID, Me.View.PreviousWorkBookView)
                End If
            Else
                Me.View.NoPermissionToAddFiles(CommunityID, Me.ModuleID)
            End If
        End Sub
        Public Sub AddFiles(ByVal CommunityFiles As MultipleUploadResult(Of dtoUploadedFile), ByVal InternalFiles As List(Of lm.Comol.Core.DomainModel.BaseFile))
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            If IsNothing(oItem) Then
                Me.View.ReturnToFileManagement(Me.View.PreloadedItemID, Me.View.PreviousWorkBookView)
            Else
                Dim CommunityId As Integer = 0
                If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                    CommunityId = oItem.WorkBookOwner.CommunityOwner.Id
                End If

                If Not IsNothing(CommunityFiles) AndAlso CommunityFiles.UploadedFile.Count > 0 Then
                    Me.CurrentManager.AddCommunityFilesToItem(Me.View.PreloadedItemID, (From f In CommunityFiles.UploadedFile Select CLng(f.File.Id)).ToList(), Me.UserContext.CurrentUserID)
                    Me.View.AddCommunityFileAction(CommunityId, Me.ModuleID)
                End If
                If Not IsNothing(InternalFiles) AndAlso InternalFiles.Count > 0 Then
                    Me.CurrentManager.AddInternalFilesToItem(Me.View.PreloadedItemID, InternalFiles, Me.UserContext.CurrentUserID)
                    Me.View.AddInternalFileAction(CommunityId, Me.ModuleID)
                End If

                If (Not IsNothing(InternalFiles) AndAlso InternalFiles.Count > 0) OrElse (Not IsNothing(CommunityFiles) AndAlso CommunityFiles.UploadedFile.Count > 0) Then
                    Dim isPersonal As Boolean = oItem.WorkBookOwner.isPersonal
                    Dim Authors As List(Of Integer) = (From a In oItem.WorkBookOwner.Authors Select a.Id).ToList
                    If CommunityFiles.UploadedFile.Count > 0 Then
                        Me.View.NotifyAddCommunityFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                    If InternalFiles.Count > 0 Then
                        Me.View.NotifyAddInternalFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
                    End If
                End If
                Me.View.ReturnToFileManagementWithErrors(oItem.Id, Nothing, CommunityFiles.NotuploadedFile, Me.View.PreviousWorkBookView)
            End If
        End Sub

        Private Function HasPermission(ByVal oItem As WorkBookItem) As Boolean
            Dim ModulePermission As ModuleWorkBook
            If oItem.WorkBookOwner.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oItem.WorkBookOwner.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookItemPermission = Me.CurrentManager.GetWorkBookItemPermission(Me.UserContext.CurrentUserID, oItem, ModulePermission)

            If IsNothing(oPermission) Then
                Return False
            Else
                Return oPermission.Write
            End If
        End Function

    End Class
End Namespace
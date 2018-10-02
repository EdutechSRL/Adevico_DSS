Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WorkBookItemSelectCommunityFile
        Inherits DomainPresenter

        Private _ModuleID As Integer
        Private _CommonManager As ManagerCommon
        'Private _RepositoryManager As ManagerCommunityFiles
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(UCServices.Services_WorkBook.Codex)
                End If
                Return _ModuleID
            End Get
        End Property

        'Private Overloads Property RepositoryManager() As ManagerCommunityFiles
        '    Get
        '        Return _RepositoryManager
        '    End Get
        '    Set(ByVal value As ManagerCommunityFiles)
        '        _RepositoryManager = value
        '    End Set
        'End Property
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
        Public Overloads ReadOnly Property View() As IWKSelectCommunityFile
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IWKSelectCommunityFile)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            If Not Me.UserContext.isAnonymous Then
                Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
                If IsNothing(oItem) Then
                    Me.View.NoPermissionToManagementFiles(CommunityID, Me.ModuleID)
                ElseIf HasPermission(oItem) OrElse Not IsNothing(oItem.WorkBookOwner.CommunityOwner) Then
                    If IsNothing(oItem.WorkBookOwner.CommunityOwner) Then
                        CommunityID = 0
                    Else
                        CommunityID = oItem.WorkBookOwner.CommunityOwner.Id
                    End If
                    Dim oFilePermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)

                    Me.View.AllowCommunityLink = oFilePermission.Administration OrElse oFilePermission.UploadFile OrElse oFilePermission.ListFiles OrElse oFilePermission.DownLoad

                    Dim oSelectedFiles As List(Of Long) = (From f In Me.CurrentManager.NEW_WorkbookItemCommunityFiles(oItem) Select f.FileCommunity.Id).ToList


                    Me.View.InitializeFileSelector(CommunityID, oSelectedFiles, (oFilePermission.Administration), (oFilePermission.Administration))
                    Me.View.BackToManagement = oItem.Id
                Else
                    Me.View.BackToManagement = oItem.Id
                    Me.View.AllowCommunityLink = False
                End If
            Else
                Me.View.NoPermissionToManagementFiles(CommunityID, Me.ModuleID)
            End If
        End Sub
        'Public Sub AddCommunityFile(ByVal CommunityFiles As MultipleUploadResult(Of COL_File_Disponibile))
        '    Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
        '    If IsNothing(oItem) OrElse IsNothing(CommunityFiles) OrElse CommunityFiles.UploadedFile.Count = 0 Then
        '        Me.View.ReturnToFileManagement(Me.View.PreloadedItemID)
        '    Else
        '        Me.CurrentManager.AddCommunityFilesToItem(Me.View.PreloadedItemID, (From f In CommunityFiles.UploadedFile Select CLng(f.Id)).ToList(), Me.UserContext.CurrentUserID)
        '        Me.View.AddCommunityFileAction()

        '        Dim isPersonal As Boolean = oItem.WorkBookOwner.isPersonal
        '        Dim CommunityId As Integer = 0
        '        Dim Authors As List(Of Integer) = (From a In oItem.WorkBookOwner.Authors Select a.Id).ToList
        '        If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
        '            CommunityId = oItem.WorkBookOwner.CommunityOwner.Id
        '        End If

        '        If CommunityFiles.UploadedFile.Count > 0 Then
        '            Me.View.NotifyAddCommunityFile(isPersonal, CommunityId, oItem.WorkBookOwner.Id, oItem.WorkBookOwner.Title, oItem.Id, oItem.Title, oItem.StartDate, Me.UserContext.CurrentUser.SurnameAndName, Authors)
        '        End If

        '        If CommunityFiles.NotuploadedFile.Count = 0 Then
        '            Me.View.ReturnToFileManagement(Me.View.PreloadedItemID)
        '        Else
        '            Me.View.ReturnToFileManagement(Nothing, CommunityFiles.NotuploadedFile)
        '        End If

        '    End If
        'End Sub
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

        Public Sub UpdateFileLink(ByVal SelectedFilesID As List(Of Long))
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(Me.View.PreloadedItemID)
            Dim CommunityID As Integer = Me.UserContext.CurrentCommunityID
            If Not IsNothing(oItem) Then
                If oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                    CommunityID = 0
                Else
                    CommunityID = oItem.WorkBookOwner.CommunityOwner.Id
                End If
                Dim oFilePermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityID)



                'File assigned to workbook item
                Dim oAssignedFiles As List(Of WorkBookCommunityFile) = (From f In Me.CurrentManager.NEW_WorkbookItemCommunityFiles(oItem) Select f).ToList

                ' ID of file assigned
                Dim oAssignedFilesID As List(Of Long) = (From f As WorkBookCommunityFile In oAssignedFiles Select f.FileCommunity.Id).ToList
                ' Id of file to assign to workbook item
                Dim oFileToAdd As List(Of Long) = (From selectedID As Long In SelectedFilesID Where Not oAssignedFilesID.Contains(selectedID) Select selectedID).ToList

                'File to remove from assignment
                Dim oFileToRemove As List(Of WorkBookCommunityFile) = (From assigned As WorkBookCommunityFile In oAssignedFiles _
                 Where Not SelectedFilesID.Contains(assigned.FileCommunity.Id) Select assigned).ToList

                If Not IsNothing(oFileToAdd) AndAlso oFileToAdd.Count > 0 Then
                    Me.CurrentManager.AddCommunityFilesToItem(Me.View.PreloadedItemID, oFileToAdd, Me.UserContext.CurrentUserID)
                    Me.View.AddCommunityFileAction(CommunityID, ModuleID)
                End If

                If Not IsNothing(oFileToRemove) AndAlso oFileToRemove.Count > 0 Then
                    Dim Onwer As Person = Me.CurrentManager.GetPerson(Me.UserContext.CurrentUserID)
                    oFileToRemove = (From f In oFileToRemove Where f.FileCommunity.isVisible _
                          OrElse (Not f.FileCommunity.isVisible AndAlso (oFilePermission.Administration OrElse f.FileCommunity.Owner Is Onwer)) Select f).ToList
                    Me.CurrentManager.UnLinkToCommunityFileFromItem(Me.View.PreloadedItemID, oFileToRemove)
                End If
            End If
            Me.View.ReturnToFileManagement(Me.View.PreloadedItemID)
        End Sub
    End Class
End Namespace
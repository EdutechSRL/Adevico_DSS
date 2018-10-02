Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
    Public Class WorkBookItemFilePresenter
        Inherits DomainPresenter


        Private _CommonManager As ManagerCommon
        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID() As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = Me.CommonManager.GetModuleID(COL_BusinessLogic_v2.UCServices.Services_WorkBook.Codex)
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
        Public Overloads ReadOnly Property View() As IviewWorkBookItemFileList
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewWorkBookItemFileList)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView(ByVal ItemID As System.Guid, ByVal CommunityID As Integer, ByVal OnlyVisibleFiles As Boolean, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository)
            Me.View.AllowOnlyVisibleFiles = OnlyVisibleFiles
            Me.View.WorkBookModuleID = Me.ModuleID
            Me.View.WorkBookCommunityID = CommunityId
            Me.View.ItemID = ItemID

            If Not Me.View.AutoUpdate Then
                Me.View.ItemPermissions = oPermission
            End If
            Me.View.CommunityRepositoryPermissions = oModule
            Me.LoadFiles(ItemID, oPermission, oModule)
        End Sub

        Public Sub UnlinkCommunityFile(ByVal FileID As System.Guid)
            Me.CurrentManager.UnLinkToCommunityFileFromItem(FileID)

            If Me.View.AutoUpdate Then
                Me.View.RequireUpdate()
            Else
                Me.LoadFiles(Me.View.ItemID, Me.View.ItemPermissions, Me.View.CommunityRepositoryPermissions)
            End If
        End Sub
        Public Sub VirtualDelete(ByVal FileID As System.Guid)
            Me.CurrentManager.VirtualDeleteFileFromItem(FileID, Me.UserContext.CurrentUser.Id)

            If Me.View.AutoUpdate Then
                Me.View.RequireUpdate()
            Else
                Me.LoadFiles(Me.View.ItemID, Me.View.ItemPermissions, Me.View.CommunityRepositoryPermissions)
            End If
        End Sub
        Public Sub VirtualUndelete(ByVal FileID As System.Guid)
            Me.CurrentManager.VirtualUnDeleteFileFromItem(FileID, Me.UserContext.CurrentUser.Id)

            If Me.View.AutoUpdate Then
                Me.View.RequireUpdate()
            Else
                Me.LoadFiles(Me.View.ItemID, Me.View.ItemPermissions, Me.View.CommunityRepositoryPermissions)
            End If
        End Sub

        Public Sub Delete(ByVal FileID As System.Guid, ByVal PersonalFilePath As String)
            Me.CurrentManager.RemoveFileFromItem(FileID, PersonalFilePath)

            If Me.View.AutoUpdate Then
                Me.View.RequireUpdate()
            Else
                Me.LoadFiles(Me.View.ItemID, Me.View.ItemPermissions, Me.View.CommunityRepositoryPermissions)
            End If
        End Sub

        Private Sub LoadFiles(ByVal ItemID As System.Guid, ByVal oPermission As WorkBookItemPermission, ByVal oModule As ModuleCommunityRepository)
            Dim oList As List(Of dtoWorkBookFile) = Nothing
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(ItemID)
            oList = Me.CurrentManager.NEW_GetWorkBookItemDTOFiles(oItem, Me.View.AllowOnlyVisibleFiles, oPermission, oModule)

            Me.View.LoadFiles(oList)
        End Sub
    End Class

End Namespace
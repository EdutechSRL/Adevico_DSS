Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.Presentation
	Public Class PrintWorkBookPresenter
		Inherits DomainPresenter

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerWorkBook
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerWorkBook)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IviewPrintWorkBook
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewPrintWorkBook)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerWorkBook(MyBase.AppContext)
        End Sub
        Public Sub Init()

        End Sub
#End Region

        Public Sub InitPrintItems()
            If Not Me.UserContext.isAnonymous Then
                Dim oWorkBook As WorkBook = Me.CurrentManager.NEW_GetWorkBook(Me.View.PreloadedWorkBookID)
                If IsNothing(oWorkBook) Then
                    Me.View.NoWorkBookWithThisID()
                Else
                    Dim oPermission As WorkBookPermission = GetWorkBookPermission(oWorkBook)
                    If PermissionToSee(oPermission) Then
                        Me.LoadWorkBookItems(oWorkBook)
                    Else
                        Me.View.NoPermissionToViewItems()
                    End If
                End If
            Else
                Me.View.NoPermissionToViewItems()
            End If
        End Sub
        Public Sub LoadWorkBookItems(ByVal oWorkBook As WorkBook)
            Dim oPermission As WorkBookPermission = GetWorkBookPermission(oWorkBook)
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oList As List(Of dtoWorkBookItem) = Me.CurrentManager.GetWorkBookItemsListWithPermission(Me.UserContext.CurrentUser, oWorkBook, ModulePermission, Me.View.DisplayOrderAscending, ObjectStatus.Active, Me.UserContext.Language)
            Me.View.LoadItems(oList)
        End Sub
        Public Function GetItemFiles(ByVal ItemID As System.Guid, ByVal oPermission As WorkBookItemPermission) As List(Of dtoWorkBookFile)
            Dim oFiles As List(Of dtoWorkBookFile) = Nothing
            Dim oItem As WorkBookItem = Me.CurrentManager.GetWorkBookItem(ItemID)
            Dim oFilePermission As New ModuleCommunityRepository
            If Not oItem.WorkBookOwner.CommunityOwner Is Nothing Then
                oFilePermission = Me.View.CommunityRepositoryPermission(oItem.WorkBookOwner.CommunityOwner.Id)
            End If

            oFiles = Me.CurrentManager.NEW_GetWorkBookItemDTOFiles(oItem, True, oPermission, oFilePermission)
            If oFiles Is Nothing Then
                oFiles = New List(Of dtoWorkBookFile)
            End If
            Return oFiles
        End Function

#Region "Permission"
        Private Function GetWorkBookPermission(ByVal oWorkBook As WorkBook) As WorkBookPermission
            Dim ModulePermission As ModuleWorkBook
            If oWorkBook.CommunityOwner Is Nothing Then : ModulePermission = ModuleWorkBook.CreatePortalmodule
            Else : ModulePermission = (From p In Me.View.CommunitiesPermission Where p.ID = oWorkBook.CommunityOwner.Id Select p.Permissions).FirstOrDefault
            End If

            Dim oPermission As WorkBookPermission = Me.CurrentManager.GetWorkBookPermission(Me.UserContext.CurrentUserID, oWorkBook, ModulePermission)
            If IsNothing(oPermission) Then
                Return New WorkBookPermission
            Else
                Return oPermission
            End If
        End Function
        Private Function PermissionToSee(ByVal oPermission As WorkBookPermission) As Boolean
            Return oPermission.AddItems OrElse oPermission.Admin OrElse oPermission.EditWorkBook OrElse oPermission.ReadWorkBook
        End Function
        Private Function PermissionToEdit(ByVal oPermission As WorkBookPermission) As Boolean
            Return oPermission.AddItems OrElse oPermission.Admin OrElse oPermission.EditWorkBook
        End Function
#End Region

	End Class
End Namespace
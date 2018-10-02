Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports System.Linq
Imports lm.Comol.Core.DataLayer.LinqExtension

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRmultipleDeletePresenter
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

        Public Overloads ReadOnly Property View() As IviewMultipleDelete
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewMultipleDelete)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView()
            Dim CommunityId As Integer = Me.View.PreLoadedCommunityID
            If CommunityId = 0 Then
                CommunityId = Me.UserContext.CurrentCommunityID
            End If
            If Not Me.UserContext.isAnonymous Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(CommunityId)
                Dim oPermission As ModuleCommunityRepository
                oPermission = Me.View.CommunityRepositoryPermission(CommunityId)

                Me.View.AllowManagement(Me.View.PreLoadedFolder, Me.View.PreLoadedCommunityID, Me.View.PreLoadedView) = oPermission.Administration
                If oCommunity Is Nothing AndAlso Not (oPermission.Administration) Then
                    Me.View.TitleCommunity = Me.View.Portalname
                    Me.View.NoPermissionToDelete(CommunityId)
                Else
                    Dim CommunityName As String = ""
 
                    If CommunityId = 0 Then
                        CommunityName = Me.View.Portalname
                    ElseIf Not oCommunity Is Nothing Then
                        CommunityName = oCommunity.Name
                    End If
                    Me.View.RepositoryCommunityID = CommunityId
                    Me.View.TitleCommunity = CommunityName
                    Me.View.AllowMultipleDelete = True
                    Me.View.InitializeFileSelector(CommunityId, oPermission.Administration, oPermission.Administration)
                End If
            Else
                Me.View.NoPermission(CommunityId)
            End If
        End Sub

        Public Sub DeleteSelectedItems(ByVal Items As List(Of Long), ByVal CommunityPath As String)
            Dim CurrentFolderID As Long = Me.View.PreLoadedFolder
            Dim oList As List(Of dtoDeletedItem) = Me.CurrentManager.DeleteItems(Items, False, Me.View.RepositoryCommunityID, CommunityPath)

            If Me.CurrentManager.ExistItem(CurrentFolderID) Then
                CurrentFolderID = 0
            End If
            Me.View.LoadRepositoryPage(Me.View.RepositoryCommunityID, CurrentFolderID, Me.View.PreLoadedView, RepositoryPage.ManagementPage)
        End Sub
    End Class
End Namespace
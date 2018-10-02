Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic
Imports COL_BusinessLogic_v2
Imports System.Linq
Imports lm.Comol.Core.DataLayer.LinqExtension

Namespace lm.Comol.Modules.Base.Presentation
    Public Class CRfileDetailPresenter
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

        Public Overloads ReadOnly Property View() As IViewCommunityFileDetail
            Get
                Return MyBase.View
            End Get
        End Property

        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityFileDetail)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunityFiles(MyBase.AppContext)
            Me.CommonManager = New ManagerCommon(MyBase.AppContext)
        End Sub

        Public Sub InitView(ByVal ItemId As Long)
            Dim CommunityId As Integer = Me.UserContext.CurrentCommunityID

            If Not Me.UserContext.isAnonymous Then
                Dim oItem As CommunityFile = Me.CurrentManager.GetFileItemById(ItemId)
                If IsNothing(oItem) Then
                    Me.View.LoadNoDetails(ItemId, CommunityId)
                Else
                    Dim CommunityName As String = ""
                    If Not IsNothing(oItem.CommunityOwner) Then
                        CommunityId = oItem.CommunityOwner.Id
                        CommunityName = oItem.CommunityOwner.Name
                    Else
                        CommunityId = 0
                        CommunityName = Me.View.Portalname
                    End If
                    Dim oPermission As ModuleCommunityRepository = Me.View.CommunityRepositoryPermission(CommunityId)
                    Dim oDto As New dtoCommunityItemRepository(oItem, oPermission, Me.UserContext.CurrentUserID, Me.CurrentManager.HasCommunityAssignment(oItem.Id, False))
                    If oItem.isFile Then
                        Me.View.LoadFolderContent(oDto, CommunityName)
                    Else
                        Me.View.LoadFolder(oDto, CommunityName)
                    End If
                    If oDto.Permission.ViewPermission Then
                        ' AllowItems
                        Dim MaxResult As Integer = Me.View.MaxResult
                        Dim AllowToCommunity As Boolean = Me.CurrentManager.HasCommunityAssignment(ItemId, False)
                        Dim UserCount As Long = Me.CurrentManager.GetPersonAssignmentCount(ItemId, False)
                        Dim RolesId As List(Of Integer) = Me.CurrentManager.GetRolesAssignmentID(ItemId, False)
                        Dim Roles = CL_permessi.COL_TipoRuolo.List(Me.UserContext.Language.Id)
                        Dim UsersName As List(Of String)

                        If UserCount > 0 Then
                            UsersName = Me.CurrentManager.GetFirstPersonNamesAssignment(ItemId, False, MaxResult)
                        Else
                            UsersName = New List(Of String)
                        End If
                        Me.View.LoadAllowPermission(AllowToCommunity, (From r In Roles Where RolesId.Contains(r.ID) Select r.Name).ToList, UsersName, UserCount - MaxResult)


                        Dim DenyToCommunity As Boolean = Me.CurrentManager.HasCommunityAssignment(ItemId, True)
                        UserCount = Me.CurrentManager.GetPersonAssignmentCount(ItemId, True)
                        RolesId = Me.CurrentManager.GetRolesAssignmentID(ItemId, True)
                        If UserCount > 0 Then
                            UsersName = Me.CurrentManager.GetFirstPersonNamesAssignment(ItemId, True, MaxResult)
                        Else
                            UsersName = New List(Of String)
                        End If
                        If Not DenyToCommunity AndAlso UserCount = 0 AndAlso RolesId.Count = 0 Then
                            Me.View.LoadNoDenyPermission()
                        Else
                            Me.View.LoadDenyPermission(DenyToCommunity, (From r In Roles Where RolesId.Contains(r.ID) Select r.Name).ToList, UsersName, UserCount - MaxResult)
                        End If
                    End If
                End If
            Else
                Me.View.LoadNoDetails(ItemId, CommunityId)
            End If
        End Sub

        Public Function GetFolderPath(ByVal ItemId As Long) As String
            If ItemId = 0 Then
                Return ""
            Else
                Dim oFolder As CommunityFile = Me.CurrentManager.GetFileItemById(ItemId)

                Return GetFolderPath(oFolder.FolderId) & "/" & oFolder.Name
            End If
        End Function
    End Class
End Namespace
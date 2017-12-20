Imports lm.Comol.Core.BaseModules.FileStatistics.Presentation
Imports lm.Comol.Core.BaseModules.Presentation

Public Class UC_ModuleItemPersonSelector
    Inherits BaseControl
    Implements IVIewModuleItemPersonSelector
    Implements IViewModuleItemUserSelector

#Region "Base"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'SOLO PER TEST
        Me.GRVuser.DataSource = Me.GetUserDTO(25)
        Me.GRVuser.DataBind()
    End Sub

    Protected Friend Sub CBselectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each oRow As GridViewRow In GRVuser.Rows
            Dim oCheckBox As CheckBox = CType(oRow.FindControl("CBselect"), CheckBox)
            If Not IsNothing(oCheckBox) Then
                oCheckBox.Checked = DirectCast(sender, CheckBox).Checked
            End If
        Next
    End Sub

#Region "Test - cancellare tutto"
    'SOLO PER TEST GRAFICO, SUCCESSIVAMENTE ELIMINARE!!!
    Private Function GetUserDTO(ByVal NumElement As Integer) As IList(Of UserDTO)
        Dim outList As New List(Of UserDTO)
        For i As Integer = 1 To NumElement
            Dim UserDto As New UserDTO
            With UserDto
                .Id = i
                .Name = "Nomenome" & i.ToString
                .Surname = "CognomeCognome" & i.ToString
                .Info = "InfoEsempio"
            End With
            outList.Add(UserDto)
        Next
        Return outList
    End Function
    Private Class UserDTO
        Private _Id As String
        Public Property Id() As String
            Get
                Return _Id
            End Get
            Set(ByVal value As String)
                _Id = value
            End Set
        End Property

        Private _Name As String
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Private _Surname As String
        Public Property Surname() As String
            Get
                Return _Surname
            End Get
            Set(ByVal value As String)
                _Surname = value
            End Set
        End Property

        Private _Info As String
        Public Property Info() As String
            Get
                Return _Info
            End Get
            Set(ByVal value As String)
                _Info = value
            End Set
        End Property
    End Class
#End Region

#Region "IView"
    Public ReadOnly Property HasFileToSelect As Boolean Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.HasFileToSelect
        Get

        End Get
    End Property

    Public ReadOnly Property HasPermissionToSelectUser As Boolean Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.HasPermissionToSelectUser
        Get

        End Get
    End Property

    Public Sub InitializeNoPermission(ByVal idCommunity As Integer) Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.InitializeNoPermission

    End Sub

    Public Sub InitializeView(ByVal moduleCode As String, ByVal objectId As Integer, ByVal objectTypeId As Integer, ByVal UsersIds As System.Collections.Generic.IList(Of Integer), ByVal idCommunity As Integer) Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.InitializeView

    End Sub

    Public Property isInitialized As Boolean Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.isInitialized
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public ReadOnly Property SelectedUsersId As System.Collections.Generic.List(Of Integer) Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.SelectedUsersId
        Get
            'ONLY FOR TEST
            Dim outlist As New List(Of Integer)
            outlist.Add(1) 'Admin sistema
            outlist.Add(60) 'borsi
            Return outlist
        End Get
    End Property

    Public Sub UnselectAllUsers() Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.UnselectAllUsers

    End Sub

    Public ReadOnly Property UserCount As Integer Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IVIewModuleItemPersonSelector.UserCount
        Get

        End Get
    End Property
#End Region


    Public Property AllowByAnonymous As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.AllowByAnonymous
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property CurrentOrder As lm.Comol.Core.DomainModel.OrderContact Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.CurrentOrder
        Get

        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.OrderContact)

        End Set
    End Property

    Public Property EnableControl As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.EnableControl
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property FiltersAvailable As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.FilterParameter) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.FiltersAvailable
        Get

        End Get
        Set(ByVal value As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.FilterParameter))

        End Set
    End Property

    Public ReadOnly Property HasPermissionToSelectUsers As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.HasPermissionToSelectUsers
        Get

        End Get
    End Property

    Public ReadOnly Property HasPersonToSelect As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.HasPersonToSelect
        Get

        End Get
    End Property

    Public Sub InitializeNoPermission1(ByVal idCommunity As Integer) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.InitializeNoPermission

    End Sub

    Public Sub InitializeView1(ByVal moduleCode As String, ByVal objectId As Integer, ByVal objectTypeId As Integer, ByVal selectedUsersId As System.Collections.Generic.IList(Of Integer), ByVal exceptUsersId As System.Collections.Generic.IList(Of Integer), ByVal idCommunity As Integer) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.InitializeView

    End Sub

    Public Sub InitializeView1(ByVal moduleCode As String, ByVal objectId As Integer, ByVal objectTypeId As Integer, ByVal selectedUsersId As System.Collections.Generic.IList(Of Integer), ByVal exceptUsersId As System.Collections.Generic.IList(Of Integer), ByVal idCommunities As System.Collections.Generic.List(Of Integer)) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.InitializeView

    End Sub

    Public Property isInitialized1 As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.isInitialized
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Sub LoadPreviewSelectedUsers(ByVal users As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoMemberContact)) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.LoadPreviewSelectedUsers

    End Sub

    Public Sub LoadSelectedFilters(ByVal filters As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.FilterParameter)) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.LoadSelectedFilters

    End Sub

    Public Sub LoadUsers(ByVal users As System.Collections.Generic.List(Of lm.Comol.Core.DomainModel.dtoMemberContact)) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.LoadUsers

    End Sub

    Public Property OrderAscending As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.OrderAscending
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property PageSize As Integer Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.PageSize
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public ReadOnly Property SelectedCommunitiesId As System.Collections.Generic.List(Of Integer) Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.SelectedCommunitiesId
        Get

        End Get
    End Property

    Public Function SelectedContacts(ByRef selectedId As System.Collections.Generic.List(Of Integer), ByRef removedId As System.Collections.Generic.List(Of Integer)) As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.SelectedContacts

    End Function

    Public Property SelectionMode As System.Web.UI.WebControls.ListSelectionMode Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.SelectionMode
        Get

        End Get
        Set(ByVal value As System.Web.UI.WebControls.ListSelectionMode)

        End Set
    End Property

    Public Property ShowMailColumns As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.ShowMailColumns
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property ShowOtherColumns As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.ShowOtherColumns
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property ShowPreview As Boolean Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.ShowPreview
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Sub UnselectAll() Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.UnselectAll

    End Sub

    Public Property UsersCount As Long Implements lm.Comol.Core.BaseModules.Presentation.IViewModuleItemUserSelector.UsersCount
        Get

        End Get
        Set(ByVal value As Long)

        End Set
    End Property
End Class
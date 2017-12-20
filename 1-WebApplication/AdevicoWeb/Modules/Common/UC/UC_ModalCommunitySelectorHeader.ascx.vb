Public Class UC_ModalCommunitySelectorHeader
    Inherits DBbaseControl


#Region "Internal"
    Private _Width As Integer
    Private _Height As Integer
    Private _MinWidth As Integer
    Private _MinHeight As Integer
    Private _ModalTitle As String

    Public Property Width As Integer
        Get
            If _Width <= 0 Then
                Return CInt(LTdefaultWidth.Text)
            Else
                Return _Width
            End If
        End Get
        Set(value As Integer)
            _Width = value
        End Set
    End Property
    Public Property Height As Integer
        Get
            If _Height <= 0 Then
                Return CInt(LTdefaultHeight.Text)
            Else
                Return _Height
            End If
        End Get
        Set(value As Integer)
            _Height = value
        End Set
    End Property
    Public Property MinWidth As Integer
        Get
            If _MinWidth <= 0 Then
                Return CInt(LTdefaultMinWidth.Text)
            Else
                Return _MinWidth
            End If
        End Get
        Set(value As Integer)
            _MinWidth = value
        End Set
    End Property
    Public Property MinHeight As Integer
        Get
            If _MinHeight <= 0 Then
                Return CInt(LTdefaultMinHeight.Text)
            Else
                Return _MinHeight
            End If
        End Get
        Set(value As Integer)
            _MinHeight = value
        End Set
    End Property
    Public Property ModalTitle As String
        Get
            Return ViewStateOrDefault("ModalTitle", "")
        End Get
        Set(value As String)
            ViewState("ModalTitle") = value
        End Set
    End Property
    Public ReadOnly Property ModalIdentifier As String
        Get
            Return LTidentifier.Text
        End Get
    End Property
    Public ReadOnly Property ClientSideOpenDialogCssClass As String
        Get
            Return LTclientOpenDialogCssClass.Text
        End Get
    End Property
    Public WriteOnly Property LoadCss As Boolean
        Set(value As Boolean)
            CTRLheader.LoadCss = value
        End Set
    End Property
    Public WriteOnly Property LoadScripts As Boolean
        Set(value As Boolean)
            CTRLheader.LoadScripts = value
        End Set
    End Property
    Public Property SelectionMode As ListSelectionMode
        Get
            Return CTRLheader.SelectionMode
        End Get
        Set(value As ListSelectionMode)
            CTRLheader.SelectionMode = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Internal"
    Public Sub SetDefaultFilters(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), requiredPermissions As Dictionary(Of Integer, Long), unloadIdCommunities As List(Of Integer), availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability, onlyFromOrganizations As List(Of Integer))
        CTRLheader.SetDefaultFilters(filters, requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
    Public Sub SetInitializeVariables(Optional requiredPermissions As Dictionary(Of Integer, Long) = Nothing, Optional unloadIdCommunities As List(Of Integer) = Nothing, Optional availability As lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability = lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability.Subscribed, Optional onlyFromOrganizations As List(Of Integer) = Nothing)
        CTRLheader.SetInitializeVariables(requiredPermissions, unloadIdCommunities, availability, onlyFromOrganizations)
    End Sub
#End Region

End Class
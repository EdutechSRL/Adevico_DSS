Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Tags.Presentation
Imports lm.Comol.Core.Tag.Domain
Public Class UC_TagsSelectorForCommunity
    Inherits TGbaseControl
    Implements IViewTagsSelectorForCommunity

#Region "Context"
    Private _presenter As TagsSelectorForCommunityPresenter
    Private ReadOnly Property CurrentPresenter As TagsSelectorForCommunityPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New TagsSelectorForCommunityPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdCommunityToApply As Integer Implements IViewTagsSelectorForCommunity.IdCommunityToApply
        Get
            Return ViewStateOrDefault("IdCommunityToApply", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCommunityToApply") = value
        End Set
    End Property
    Private Property IdOrganizations As List(Of Integer) Implements IViewTagsSelectorForCommunity.IdOrganizations
        Get
            Return ViewStateOrDefault("IdOrganizations", New List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("IdOrganizations") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Property InLineStyle() As String
        Get
            Return ViewStateOrDefault("InLineStyle", "")
        End Get
        Set(value As String)
            ViewState("InLineStyle") = value
            If String.IsNullOrEmpty(value) Then
                SLtags.Attributes.Add("style", "")
            Else
                SLtags.Attributes.Add("style", value)
            End If
        End Set
    End Property
    Public Property Enabled() As Boolean
        Get
            Return Not SLtags.Disabled
        End Get
        Set(value As Boolean)
            SLtags.Disabled = Not value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControlForCommunity(idCommunity As Integer) Implements IViewTagsSelectorForCommunity.InitializeControlForCommunity
        CurrentPresenter.InitViewForCommunity(idCommunity)
    End Sub
    Public Sub InitializeControlForCommunityToAdd(idFatherCommunity As Integer, idCommunityType As Integer) Implements IViewTagsSelectorForCommunity.InitializeControlForCommunityToAdd
        CurrentPresenter.InitViewForCommunityToAdd(idFatherCommunity, idCommunityType)
    End Sub
    Public Sub InitializeControlForOrganization(idOrganization As Integer, Optional idCommunityType As Integer = -1) Implements IViewTagsSelectorForCommunity.InitializeControlForOrganization
        CurrentPresenter.InitViewForOrganization(idOrganization, idCommunityType)
    End Sub
    Public Sub InitializeControl(tags As List(Of dtoTagSelectItem), Optional idOrganization As Integer = 0, Optional idCommunity As Integer = 0) Implements IViewTagsSelectorForCommunity.InitializeControl
        IdCommunityToApply = idCommunity
        If idOrganization > 0 Then
            IdOrganizations = New List(Of Integer)(idOrganization)
        End If
        LoadTags(tags)
    End Sub
    Private Function HasAvailableTags() As Boolean Implements IViewTagsSelectorForCommunity.HasAvailableTags
        Return SLtags.Items.Count > 0
    End Function
    Private Sub LoadTags(tags As List(Of dtoTagSelectItem)) Implements IViewTagsSelectorForCommunity.LoadTags
        SLtags.Attributes.Add("data-placeholder", Resource.getValue("SelectTags.data-placeholder"))
        SLtags.DataSource = tags
        SLtags.DataTextField = "Name"
        SLtags.DataValueField = "Id"
        SLtags.DataBind()
        If Not IsNothing(tags) Then
            For Each tag As dtoTagSelectItem In tags.Where(Function(t) t.IsSelected)
                SLtags.Items.FindByValue(tag.Id).Selected = True
            Next
            For Each tag As dtoTagSelectItem In tags.Where(Function(t) Not t.CanBeUnselected OrElse (Not t.CanBeSelected))
                SLtags.Items.FindByValue(tag.Id).Enabled = False
            Next
        End If
    End Sub
    Public Sub ApplyTags() Implements IViewTagsSelectorForCommunity.ApplyTags
        CurrentPresenter.ApplyTags(IdCommunityToApply, GetSelectedTags)
    End Sub
    Public Sub ApplyTags(idCommunity As Integer) Implements IViewTagsSelectorForCommunity.ApplyTags
        CurrentPresenter.ApplyTags(idCommunity, GetSelectedTags)
    End Sub
    Public Sub ApplyTags(idCommunities As List(Of Integer)) Implements IViewTagsSelectorForCommunity.ApplyTags
        CurrentPresenter.ApplyTags(idCommunities, GetSelectedTags)
    End Sub
    Public Sub ReloadTags(idCommunityType As Integer) Implements IViewTagsSelectorForCommunity.ReloadTags
        CurrentPresenter.ReloadTags(IdCommunityToApply, IdOrganizations, idCommunityType, GetSelectedTags)
    End Sub
    Public Sub ReloadTags(idCommunityType As Integer, idOrganization As Integer) Implements IViewTagsSelectorForCommunity.ReloadTags
        CurrentPresenter.ReloadTags(IdCommunityToApply, idOrganization, idCommunityType, GetSelectedTags)
    End Sub
    Private Sub DisplayMessage(action As ModuleTags.ActionType) Implements IViewTagsSelectorForCommunity.DisplayMessage

    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewTagsSelectorForCommunity.DisplaySessionTimeout

    End Sub
    Public Function GetSelectedTags() As List(Of Long) Implements IViewTagsSelectorForCommunity.GetSelectedTags
        If SLtags.Items.Count > 0 Then
            Return (From i As ListItem In SLtags.Items Where i.Selected Select CLng(i.Value)).ToList
        Else
            Return New List(Of Long)
        End If
    End Function
#End Region

#Region "Internal"

#End Region

   
End Class
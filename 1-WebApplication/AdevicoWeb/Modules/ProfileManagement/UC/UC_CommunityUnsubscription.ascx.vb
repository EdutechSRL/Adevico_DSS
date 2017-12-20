Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.CommunityManagement
Imports lm.Comol.Core.BaseModules.CommunityManagement.Presentation
Imports lm.Comol.UI.Presentation

Public Class UC_ProfileCommunityUnsubscriptions
    Inherits BaseControl
    Implements IViewStandardUnsubscriptionsList

#Region "Property"
    Public Property HasUnsubscriptions As Boolean Implements IViewStandardUnsubscriptionsList.HasUnsubscriptions
        Get
            Return ViewStateOrDefault("HasUnsubscriptions", False)
        End Get
        Set(value As Boolean)
            ViewState("HasUnsubscriptions") = value
        End Set
    End Property

    Private Property IdProfile As Integer Implements IViewStandardUnsubscriptionsList.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", 0)
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Protected Function BackGroundItem(ByVal oType As ListItemType) As String
        'If isVisible Then
        Select Case oType
            Case ListItemType.Item
                Return "ROW_Normal_Small"
            Case ListItemType.AlternatingItem
                Return "ROW_Alternate_Small"
            Case Else
                Return ""
        End Select
        'Else
        'Return "ROW_Disabilitate_Small"
        'End If

        Return ""
    End Function
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property

    Private _RolesName As List(Of Comol.Entity.Role)
    Private ReadOnly Property RolesName As List(Of Comol.Entity.Role)
        Get
            If _RolesName Is Nothing Then
                _RolesName = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
            End If
            Return _RolesName
        End Get
    End Property
   
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Page.IsPostBack = False Then
        '    Me.SetInternazionalizzazione()
        'End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AddCommunityToProfile", "Modules", "ProfileManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            '   .setLabel(LBcommunityStatus_t)
        End With
    End Sub
#End Region

    Public Sub InitializeControl(idProfile As Integer, unsubscriptions As List(Of dtoBaseUserSubscription)) Implements IViewStandardUnsubscriptionsList.InitializeControl
        Me.IdProfile = idProfile
        Me.isInitialized = True
        Me.MLVdata.SetActiveView(VIWlist)
        Me.RPTsubscriptions.DataSource = unsubscriptions
        Me.RPTsubscriptions.DataBind()
        Me.HasUnsubscriptions = (unsubscriptions.Count > 0)
    End Sub

    Public Sub LoadNothing() Implements IViewStandardUnsubscriptionsList.LoadNothing
        Me.MLVdata.SetActiveView(VIWempty)
    End Sub

    Public Function SelectedUnsubscriptions() As List(Of dtoBaseUserSubscription) Implements IViewStandardUnsubscriptionsList.SelectedUnsubscriptions
        Dim result As New List(Of dtoBaseUserSubscription)

        For Each r As RepeaterItem In RPTsubscriptions.Items
            Dim dto As New dtoBaseUserSubscription

            Dim oCheckBox As CheckBox = r.FindControl("CBXconfirmDelete")
            If oCheckBox.Checked Then
                Dim oLiteral As Literal = r.FindControl("LTidCommunity")
                dto.IdCommunity = CInt(oLiteral.Text)
                oLiteral = r.FindControl("LTidSubscriptions")
                dto.Id = CInt(oLiteral.Text)
                oLiteral = r.FindControl("LTidPreviousRole")
                dto.IdPreviousRole = CInt(oLiteral.Text)

                oLiteral = r.FindControl("LTpath")
                dto.Path = oLiteral.Text.Split("|").ToList

                Dim oLabel As Label = r.FindControl("LBcommunityName")
                dto.CommunityName = oLabel.Text

                result.Add(dto)
            End If
        Next

        Return result
    End Function


    Private Sub RPTsubscriptions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubscriptions.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBcommunityName_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBcommunityRole_t")
            Me.Resource.setLabel(oLabel)
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoBaseUserSubscription = DirectCast(e.Item.DataItem, dtoBaseUserSubscription)

            Dim oLabel As Label = e.Item.FindControl("LBcommunityName")
            oLabel.Text = dto.CommunityName

            Dim oLiteral As Literal = e.Item.FindControl("LTidCommunity")
            oLiteral.Text = dto.IdCommunity
            oLiteral = e.Item.FindControl("LTidSubscriptions")
            oLiteral.Text = dto.Id
            oLiteral = e.Item.FindControl("LTidPreviousRole")
            oLiteral.Text = dto.IdPreviousRole
            oLiteral = e.Item.FindControl("LTpath")

            Dim builder As New System.Text.StringBuilder
            For Each item As String In dto.Path
                builder.Append(item).Append("|")
            Next
            oLiteral.Text = builder.ToString()

            oLabel = e.Item.FindControl("LBrole")
            oLabel.Text = (From r In RolesName Where r.ID = dto.IdPreviousRole Select r.Name).FirstOrDefault()

        End If
    End Sub
End Class
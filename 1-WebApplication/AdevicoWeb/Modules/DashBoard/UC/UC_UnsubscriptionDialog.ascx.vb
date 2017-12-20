Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.Dashboard.Domain
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Public Class UC_UnsubscriptionDialog
    Inherits DBbaseControl
    Implements IViewConfirmUnsubscription

#Region "Implements"
#Region "Settings"
    Public Property isInitialized As Boolean Implements IViewConfirmUnsubscription.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property DisplayDescription As Boolean Implements IViewConfirmUnsubscription.DisplayDescription
        Get
            Return ViewStateOrDefault("DisplayDescription", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayDescription") = value
            Me.DVdescription.Visible = value
        End Set
    End Property
    Public Property RaiseCommandEvents As Boolean Implements IViewConfirmUnsubscription.RaiseCommandEvents
        Get
            Return ViewStateOrDefault("RaiseCommandEvents", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("RaiseCommandEvents") = value
        End Set
    End Property
    Public Property DisplayCommands As Boolean Implements IViewConfirmUnsubscription.DisplayCommands
        Get
            Return ViewStateOrDefault("DisplayCommands", True)
        End Get
        Set(value As Boolean)
            ViewState("DisplayCommands") = value
            DVcommands.Visible = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewConfirmUnsubscription.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property Path As String Implements IViewConfirmUnsubscription.Path
        Get
            Return ViewStateOrDefault("Path", "")
        End Get
        Set(value As String)
            ViewState("Path") = value
        End Set
    End Property
#End Region
#End Region

#Region "Internal"
    Public Property PreSelectedAction As RemoveAction
        Get
            Return ViewStateOrDefault("RemoveAction", RemoveAction.None)
        End Get
        Set(value As RemoveAction)
            ViewState("RemoveAction") = value
        End Set
    End Property
    Public Property InModalWindow As Boolean
        Get
            Return ViewStateOrDefault("InModalWindow", True)
        End Get
        Set(value As Boolean)
            ViewState("InModalWindow") = value
            DVcommands.Visible = value
        End Set
    End Property

    Public Event CloseWindow()
    Public Event UnsubscribeFromCommunity(idCommunity As Integer, path As String, rAction As RemoveAction)
    Public ReadOnly Property DialogIdentifier As String
        Get
            Return LTcssClassDialog.Text
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

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNapplyUnsubscribeFromCommunity, True)
            .setHyperLink(HYPcloseUnsubscribeFromCommunityDialog, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idC As Integer, communityPath As String, community As lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode, actions As List(Of RemoveAction), selected As RemoveAction, Optional alsoFromCommunities As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode) = Nothing, Optional description As String = "") Implements IViewConfirmUnsubscription.InitializeControl
        LoadAvailableActions(actions, selected)
        IdCommunity = idC
        Path = communityPath
        InitializeMessage(community, alsoFromCommunities)
    End Sub
    Private Sub LoadAvailableActions(actions As List(Of RemoveAction), selected As RemoveAction) Implements IViewConfirmUnsubscription.LoadAvailableActions
        PreSelectedAction = selected
        Me.RPTactions.DataSource = actions
        Me.RPTactions.DataBind()
    End Sub
    Private Function GetSelectedAction() As RemoveAction Implements IViewConfirmUnsubscription.GetSelectedAction
        Dim value As String = Request.Form("RDremoveaction")
        If Not String.IsNullOrEmpty(value) AndAlso IsNumeric(value) Then
            Return value
        Else
            Return RemoveAction.None
        End If
        Return RemoveAction.None
    End Function
#End Region

#Region "Internal"
    Private Sub InitializeMessage(community As lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode, alsoFromCommunities As List(Of lm.Comol.Core.BaseModules.CommunityManagement.dtoUnsubscribeTreeNode))
        Dim messageKey As String = "UnsubscribeMessage"
        Dim withSubCommunities As Boolean = Not (IsNothing(alsoFromCommunities) OrElse Not alsoFromCommunities.Any())
        If withSubCommunities Then
            messageKey &= ".withSubCommunities"
        End If

        If (community.AllowUnsubscribe) Then
            Dim message As String = ""
            If Not community.CommunityAllowSubscription Then
                messageKey &= ".NotAllowSubscription"
            Else
                If community.MaxUsersWithDefaultRole > 0 Then
                    messageKey &= ".WithLimits"
                End If
                If community.CommunitySubscriptionEndOn.HasValue Then
                    messageKey &= ".TimeLimits"
                End If
            End If
            Dim usItems As String = ""
            If withSubCommunities Then
                usItems = LTtemplateMessageDetails.Text
                usItems = String.Format(usItems, String.Join("", alsoFromCommunities.Select(Function(s) String.Format(LTtemplateMessageDetail.Text, s.Name)).ToList()))
            End If
            message = Resource.getValue(messageKey)
            If Not community.CommunityAllowSubscription AndAlso withSubCommunities Then
                LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, usItems)
            ElseIf Not community.CommunityAllowSubscription Then
                LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name)
            ElseIf withSubCommunities Then
                If community.MaxUsersWithDefaultRole > 0 AndAlso community.CommunitySubscriptionEndOn.HasValue Then
                    LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, community.MaxUsersWithDefaultRole, GetDateToString(community.CommunitySubscriptionEndOn, ""), GetTimeToString(community.CommunitySubscriptionEndOn, ""), usItems)
                ElseIf community.MaxUsersWithDefaultRole > 0 Then
                    LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, community.MaxUsersWithDefaultRole, usItems)
                ElseIf community.CommunitySubscriptionEndOn.HasValue Then
                    LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, GetDateToString(community.CommunitySubscriptionEndOn, ""), GetTimeToString(community.CommunitySubscriptionEndOn, ""), usItems)
                Else
                    LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, usItems)
                End If
            ElseIf community.MaxUsersWithDefaultRole > 0 AndAlso community.CommunitySubscriptionEndOn.HasValue Then
                LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, community.MaxUsersWithDefaultRole, GetDateToString(community.CommunitySubscriptionEndOn, ""), GetTimeToString(community.CommunitySubscriptionEndOn, ""))
            ElseIf community.MaxUsersWithDefaultRole > 0 Then
                LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, community.MaxUsersWithDefaultRole)
            ElseIf community.CommunitySubscriptionEndOn.HasValue Then
                LBdescription.Text = String.Format(Resource.getValue(messageKey), community.Name, GetDateToString(community.CommunitySubscriptionEndOn, ""), GetTimeToString(community.CommunitySubscriptionEndOn, ""))
            Else
                LBdescription.Text = String.Format(Resource.getValue("UnsubscribeMessage.community"), community.Name)
            End If
        Else
            LBdescription.Text = String.Format(Resource.getValue("UnsubscribeMessage.notAllowUnsubscribe"), community.Name)
        End If
    End Sub
    Protected Function GetDialogTitle() As String
        Return Resource.getValue("IViewConfirmUnsubscription.GetDialogTitle")
    End Function
    Private Sub BTNapplyUnsubscribeFromCommunity_Click(sender As Object, e As System.EventArgs) Handles BTNapplyUnsubscribeFromCommunity.Click
        If RaiseCommandEvents Then
            RaiseEvent UnsubscribeFromCommunity(IdCommunity, Path, GetSelectedAction())
        End If
    End Sub
    Protected Function SetChecked(action As RemoveAction) As String
        Return IIf(action = PreSelectedAction, "checked=""checked""", "")
    End Function

    Private Sub RPTactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTactions.ItemDataBound
        Dim action As RemoveAction = DirectCast(e.Item.DataItem, RemoveAction)
        Dim oLabel As Label = e.Item.FindControl("LBactionDescription")
        oLabel.Text = Resource.getValue("description.RemoveAction." & action.ToString)
    End Sub
#End Region
   

   
End Class
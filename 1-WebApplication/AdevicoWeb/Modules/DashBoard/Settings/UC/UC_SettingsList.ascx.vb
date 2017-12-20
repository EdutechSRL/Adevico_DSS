Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.Dashboard.Presentation
Imports lm.Comol.Core.Dashboard.Domain

Public Class UC_SettingsList
    Inherits TGbaseControl
    Implements IViewSettingsList

#Region "Context"
    Private _presenter As SettingsListPresenter
    Protected Friend ReadOnly Property CurrentPresenter As SettingsListPresenter
        Get
            If IsNothing(_presenter) Then
                _presenter = New SettingsListPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property IdContainerCommunity As Integer Implements IViewSettingsList.IdContainerCommunity
        Get
            Return ViewStateOrDefault("IdContainerCommunity", -1)
        End Get
        Set(value As Integer)
            ViewState("IdContainerCommunity") = value
        End Set
    End Property
    Public Property FromRecycleBin As Boolean Implements IViewSettingsList.FromRecycleBin
        Get
            Return ViewStateOrDefault("FromRecycleBin", False)
        End Get
        Set(value As Boolean)
            ViewState("FromRecycleBin") = value
        End Set
    End Property
    Private Property CurrentType As DashboardType Implements IViewSettingsList.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", DashboardType.Portal)
        End Get
        Set(value As DashboardType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewSettingsList.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(value As Boolean)
            ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property CurrentOrderBy As OrderSettingsBy Implements IViewSettingsList.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", OrderSettingsBy.Default)
        End Get
        Set(value As OrderSettingsBy)
            ViewState("CurrentOrderBy") = value
        End Set
    End Property
#End Region

#Region "Internal"
    Public Event SessionTimeout()
    Public Event NoPermission()
    Public Event SettingsLoaded(count As Integer)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLiteral(LTthSettingsStatus)
            .setLiteral(LTthSettingsName)
            .setLiteral(LTthModifiedBy)
            .setLiteral(LTthModifiedOn)
            LBactions.ToolTip = .getValue("LBactions.ToolTip")

            .setLinkButton(LNBorderBySettingsNameUp, False, True)
            .setLinkButton(LNBorderBySettingsNameDown, False, True)
            .setLinkButton(LNBorderSettingsByStatusUp, False, True)
            .setLinkButton(LNBorderSettingsByStatusDown, False, True)
            .setLinkButton(LNBorderByModifiedByDown, False, True)
            .setLinkButton(LNBorderByModifiedByUp, False, True)
            .setLinkButton(LNBorderByModifiedOnDown, False, True)
            .setLinkButton(LNBorderByModifiedOnUp, False, True)

            .setLabel(LBtableLegend)
            LTdraftItem.Text = String.Format(LTtemplateLegendItem.Text, .getValue("Settings.Draft"), AvailableStatus.Draft.ToString.ToLower)
            LTdefaultItem.Text = String.Format(LTtemplateLegendItem.Text, .getValue("Settings.Default"), LTcssClassDefault.Text)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(permissions As ModuleDashboard, type As DashboardType, idCommunity As Integer, fromRecycleBin As Boolean) Implements IViewSettingsList.InitializeControl
        CurrentPresenter.InitView(permissions, type, idCommunity, fromRecycleBin)
    End Sub
    Private Sub DisplayMessage(action As ModuleDashboard.ActionType) Implements IViewSettingsList.DisplayMessage
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case action
            Case ModuleDashboard.ActionType.DashboardSettingsDisable, ModuleDashboard.ActionType.DashboardSettingsEnable, ModuleDashboard.ActionType.DashboardSettingsVirtualDelete, ModuleDashboard.ActionType.DashboardSettingsVirtualUndelete, ModuleDashboard.ActionType.DashboardSettingsClone
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.success
        End Select
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayMessage.ModuleDashboard.ActionType." & action.ToString), mType)
    End Sub
    Private Sub DisplayErrorLoadingFromDB() Implements IViewSettingsList.DisplayErrorLoadingFromDB
        CTRLmessages.Visible = True

        CTRLmessages.InitializeControl(Resource.getValue("DisplayErrorLoadingFromDB"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
        LoadSettings(New List(Of dtoDashboardSettings))
    End Sub
    Private Sub DisplayMessage(dbError As DashboardErrorType) Implements IViewSettingsList.DisplayMessage
        Dim mType = lm.Comol.Core.DomainModel.Helpers.MessageType.error
        Select Case dbError
            Case DashboardErrorType.MultipleAssignmentForPerson, DashboardErrorType.MultipleAssignmentForProfileType, DashboardErrorType.MultipleAssignmentForRole, DashboardErrorType.DefaultSettingsUndeletable, DashboardErrorType.DefaultSettingsUnavailable, DashboardErrorType.NoAssignmentsForUndelete, DashboardErrorType.NoAssignmentsForEnable, DashboardErrorType.NoAssignmentsForActiveSettings, DashboardErrorType.NotActivableSettings, DashboardErrorType.NoAssignmentsForUndelete
                mType = lm.Comol.Core.DomainModel.Helpers.MessageType.alert
        End Select

        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayMessage.ModuleDashboard.DashboardErrorType." & dbError.ToString), mType)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        RaiseEvent SessionTimeout()
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        SendUserAction(idCommunity, idModule, ModuleDashboard.ActionType.NoPermission)
        RaiseEvent NoPermission()
    End Sub
    Private Function GetUnknownUserName() As String Implements IViewSettingsList.GetUnknownUserName
        Return Resource.getValue("UnknownUserName")
    End Function
    Private Sub LoadSettings(items As List(Of dtoDashboardSettings)) Implements IViewSettingsList.LoadSettings
        RPTsettings.DataSource = items
        RPTsettings.DataBind()
        Dim display As Boolean = items.Any AndAlso items.Count > 1
        LNBorderBySettingsNameUp.Visible = display
        LNBorderBySettingsNameDown.Visible = display
        LNBorderSettingsByStatusDown.Visible = display
        LNBorderSettingsByStatusUp.Visible = display
        LNBorderByModifiedByDown.Visible = display
        LNBorderByModifiedByUp.Visible = display
        LNBorderByModifiedOnDown.Visible = display
        LNBorderByModifiedOnUp.Visible = display
        DVlegend.Visible = items.Any()
        RaiseEvent SettingsLoaded(items.Count)
    End Sub
    Private Function GetTranslatedTileTypes() As Dictionary(Of AvailableStatus, String) Implements IViewSettingsList.GetTranslatedStatus
        Return (From t In [Enum].GetValues(GetType(AvailableStatus)).Cast(Of AvailableStatus).ToList Select t).ToDictionary(Of AvailableStatus, String)(Function(t) t, Function(t) Resource.getValue("AvailableStatus." & t.ToString))
    End Function
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleDashboard.ActionType) Implements IViewSettingsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idDashboard As Long, action As ModuleDashboard.ActionType) Implements IViewSettingsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleDashboard.ObjectType.Dashboard, idDashboard.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

#Region "Internal"
    Private Sub RPTsettings_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RPTsettings.ItemCommand
        If PageUtility.CurrentContext.UserContext.isAnonymous Then
            RaiseEvent SessionTimeout()
        Else
            Select Case e.CommandName
                Case "hide"
                    CurrentPresenter.SetStatus(CLng(e.CommandArgument), lm.Comol.Core.Dashboard.Domain.AvailableStatus.Unavailable, CurrentType, IdContainerCommunity, FromRecycleBin, CurrentOrderBy, CurrentAscending)
                Case "show"
                    CurrentPresenter.SetStatus(CLng(e.CommandArgument), lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available, CurrentType, IdContainerCommunity, FromRecycleBin, CurrentOrderBy, CurrentAscending)
                Case "virtualdelete"
                    CurrentPresenter.VirtualDelete(CLng(e.CommandArgument), True, CurrentType, IdContainerCommunity, FromRecycleBin, CurrentOrderBy, CurrentAscending)
                Case "recover"
                    CurrentPresenter.VirtualDelete(CLng(e.CommandArgument), False, CurrentType, IdContainerCommunity, FromRecycleBin, CurrentOrderBy, CurrentAscending)
                Case "copy"
                    CurrentPresenter.Clone(CLng(e.CommandArgument), Resource.getValue("DashboardSettings.cloneOf"), CurrentType, IdContainerCommunity, FromRecycleBin, CurrentOrderBy, CurrentAscending)
            End Select
        End If
    End Sub
    Private Sub RPTsettings_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTsettings.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim dto As dtoDashboardSettings = e.Item.DataItem

                Dim oLabel As Label = e.Item.FindControl("LBsettingsName")
                Dim oControl As HtmlControl = e.Item.FindControl("SPNsettingsName")
                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewSettings")
                Resource.setHyperLink(oHyperlink, False, True)
                oLabel.Visible = Not dto.Permissions.AllowView
                oControl.Visible = dto.Permissions.AllowView
                If dto.Permissions.AllowView Then
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.DashboardPreview(dto.Id, dto.Type, dto.IdCommunity)
                End If
                oControl = e.Item.FindControl("DVextraInfo")
                oControl.Visible = Not dto.ForAll
                If Not dto.ForAll Then
                    oLabel = e.Item.FindControl("LBassignmentsInfo_t")
                    oLabel.Text = Resource.getValue("LBassignmentsInfo_t.DashboardType." & dto.Type.ToString)
                    oLabel.ToolTip = Resource.getValue("LBassignmentsInfo_t.DashboardType." & dto.Type.ToString & ".ToolTip")
                End If

                oLabel = e.Item.FindControl("LBmodifiedOn")
                oLabel.Text = GetDateToString(dto.ModifiedOn, "-")
                oLabel.ToolTip = GetDateTimeString(dto.ModifiedOn, "-")

                oHyperlink = e.Item.FindControl("HYPeditSettings")
                oHyperlink.Visible = dto.Permissions.AllowEdit
                If dto.Permissions.AllowEdit Then
                    oHyperlink.NavigateUrl = BaseUrl & RootObject.DashboardEdit(dto.Id, dto.Type, dto.IdCommunity)
                End If

                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBhideSettings")
                oLinkbutton.Visible = dto.Permissions.AllowSetUnavailable
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBcloneSettings")
                oLinkbutton.Visible = dto.Permissions.AllowClone
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBshowSettings")
                oLinkbutton.Visible = dto.Permissions.AllowSetAvailable
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBvirtualDeleteSettings")
                oLinkbutton.Visible = dto.Permissions.AllowVirtualDelete
                Resource.setLinkButton(oLinkbutton, False, True)

                oLinkbutton = e.Item.FindControl("LNBvirtualUnDeleteSettings")
                oLinkbutton.Visible = dto.Permissions.AllowUnDelete
                Resource.setLinkButton(oLinkbutton, False, True)

            Case ListItemType.Footer
                Dim oTableItem As HtmlControl = e.Item.FindControl("TRempty")
                oTableItem.Visible = (RPTsettings.Items.Count = 0)
                If (RPTsettings.Items.Count = 0) Then
                    Dim oLabel As Label = e.Item.FindControl("LBemptyItems")
                    oLabel.Text = Resource.getValue("NoSettingsFound.DashboardType." & CurrentType.ToString)
                End If
        End Select
    End Sub
    Protected Sub LNBorderBy_Click(sender As Object, e As System.EventArgs)
        Dim ascending As Boolean = CBool(DirectCast(sender, LinkButton).CommandArgument)
        Dim order As OrderSettingsBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of OrderSettingsBy).GetByString(DirectCast(sender, LinkButton).CommandName, OrderSettingsBy.Default)
        CTRLmessages.Visible = False
        CurrentOrderBy = order
        CurrentAscending = ascending
        CurrentPresenter.LoadSettings(CurrentType, IdContainerCommunity, FromRecycleBin, order, ascending)
    End Sub

    Public Function GetItemCssClass(item As dtoDashboardSettings) As String
        Dim cssClass As String = item.Status.ToString.ToLower
        If item.Active AndAlso item.ForAll Then
            cssClass &= " " & LTcssClassDefault.Text
        End If
        Return cssClass
    End Function
#End Region

   
End Class
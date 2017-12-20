Imports lm.Comol.Core.TemplateMessages.Domain

Public Class UC_TemplateWizardSteps
    Inherits BaseControl

    Private Property DisableAll As Boolean
        Get
            Return ViewStateOrDefault("DisableAll", False)
        End Get
        Set(value As Boolean)
            ViewState("DisableAll") = value
        End Set
    End Property

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
    Private Property IdTemplate As Long
        Get
            Return ViewStateOrDefault("IdTemplate", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdTemplate") = value
        End Set
    End Property
    Private Property IdTemplateVersion As Long
        Get
            Return ViewStateOrDefault("IdTemplateVersion", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdTemplateVersion") = value
        End Set
    End Property
    Private Property IdTemplateCommunity As Integer
        Get
            Return ViewStateOrDefault("IdTemplateCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdTemplateCommunity") = value
        End Set
    End Property
#End Region

    Public Event ItemCommand(ByVal tStep As lm.Comol.Core.TemplateMessages.Domain.WizardTemplateStep)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Templates", "Modules", "Templates")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Sub InitalizeControl(idItem As Long, idVersion As Long, idCommunity As Integer, idContainerCommunity As Integer, moduleCode As String, modulePermission As Long, type As TemplateType, ownerInfo As dtoBaseTemplateOwner, sessionId As Guid, backUrl As String, items As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep)), Optional preview As Boolean = False)
        DisableAll = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext.isAnonymous
        IdTemplate = idItem
        IdTemplateCommunity = idCommunity
        IdTemplateVersion = idVersion
        For Each item As lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep) In items
            item.Name = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString)
            item.Tooltip = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Tooltip")
            item.Url = lm.Comol.Core.TemplateMessages.RootObject.EditByStep(sessionId, type, ownerInfo, item.Id.Type, idContainerCommunity, moduleCode, modulePermission, backUrl, idItem, idVersion, preview)
            Dim m As String = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message")
            Select Case item.Id.Type
                Case WizardTemplateStep.Settings
                    Dim oItem As dtoSettingsStep = DirectCast(item.Id, dtoSettingsStep)
                    item.Message = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & "." & oItem.VersionStatus.ToString)
                    'If oItem.Notifications > 1 Then
                    '    item.Message = String.Format(m, oItem.Notifications)
                    'ElseIf oItem.Notifications = 1 Then
                    '    item.Message = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message.1")
                    'End If
                Case WizardTemplateStep.Translations
                    Dim oItem As dtoTranslationsStep = DirectCast(item.Id, dtoTranslationsStep)
                    If item.Status <> lm.Comol.Core.Wizard.WizardItemStatus.disabled Then
                        If oItem.Errors.Count = 0 OrElse oItem.Errors.Contains(EditingErrors.None) Then
                            item.Message = String.Format(m, oItem.Count)
                        Else
                            If oItem.Errors.Contains(EditingErrors.EmptyMessage) Then
                                item.Message = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.EmptyMessage.ToString)
                            Else
                                m = Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message." & oItem.HasMultilingua)
                                item.Message = String.Format(m, oItem.Count)
                            End If

                            If oItem.Errors.Contains(EditingErrors.EmptyTranslations) Then
                                item.Message &= "<br/>" & String.Format(Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.EmptyTranslations.ToString), oItem.Empty)
                            End If
                            If oItem.Errors.Contains(EditingErrors.InvalidTranslations) Then
                                item.Message &= "<br/>" & String.Format(Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.InvalidTranslations.ToString), oItem.InvalidItems)
                            End If
                        End If
                    End If

                Case WizardTemplateStep.Permission
                    If item.Status <> lm.Comol.Core.Wizard.WizardItemStatus.disabled Then
                        Dim oItem As dtoPermissionStep = DirectCast(item.Id, dtoPermissionStep)
                        item.Message = String.Format(m, oItem.Count)
                        If oItem.Errors.Contains(EditingErrors.NoPermission) Then
                            If oItem.NoPermissionsCount = 1 Then
                                item.Message &= "<br/>" & Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.NoPermission.ToString & ".1")
                            ElseIf oItem.NoPermissionsCount > 1 Then
                                item.Message &= "<br/>" & String.Format(Resource.getValue("WizardTemplateStep." & item.Id.Type.ToString & ".Message.EditingErrors." & EditingErrors.NoPermission.ToString), oItem.NoPermissionsCount)
                            End If
                        End If
                    End If
            End Select
        Next
        Me.RPTsteps.DataSource = items
        Me.RPTsteps.DataBind()
    End Sub

    Private Sub RPTsteps_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsteps.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim baseItem As lm.Comol.Core.Wizard.NavigableWizardItemBase(Of dtoTemplateStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItemBase(Of dtoTemplateStep))

            Dim oDiv As HtmlControl = e.Item.FindControl("DVnavigationStep")
            If TypeOf baseItem Is lm.Comol.Core.Wizard.NavigableWizardItemSeparator(Of dtoTemplateStep) Then
                Dim oLiteral As Literal = e.Item.FindControl("LTseparator")
                oDiv.Visible = False
                oLiteral.Visible = True
            Else
                Dim item As lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoTemplateStep))

                oDiv.Visible = True
                Dim oLabel As Label = e.Item.FindControl("LBitem")
                Dim oHyperlink As HyperLink = e.Item.FindControl("HTPlink")
                Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBlink")

                If item.Status = lm.Comol.Core.Wizard.WizardItemStatus.disabled OrElse DisableAll OrElse item.Active Then
                    oLabel.Visible = True
                    oLabel.Text = item.Name
                Else
                    oHyperlink.Visible = Not item.AutoPostBack

                    oLinkbutton.Visible = item.AutoPostBack
                    If item.AutoPostBack Then
                        oLinkbutton.Text = item.Name
                        oLinkbutton.ToolTip = item.Tooltip
                        oLinkbutton.CommandName = item.Id.Type.ToString
                        oLinkbutton.CommandArgument = item.Url
                    Else
                        oHyperlink.Text = item.Name
                        oHyperlink.ToolTip = item.Tooltip
                        oHyperlink.NavigateUrl = BaseUrl & item.Url
                        '  oHyperlink.NavigateUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(IdCall, IdCallCommunity, item.Id.Type, CurrentView)
                    End If
                End If
                oLabel = e.Item.FindControl("LBmessage")
                oLabel.Visible = Not String.IsNullOrEmpty(item.Message)
                If Not String.IsNullOrEmpty(item.Message) Then
                    oLabel.Text = item.Message
                End If


            End If
        End If
    End Sub

    Private Sub RPTsteps_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsteps.ItemCommand
        If Not String.IsNullOrEmpty(e.CommandName) Then
            PageUtility.RedirectToUrl(e.CommandArgument)
        End If
    End Sub

End Class
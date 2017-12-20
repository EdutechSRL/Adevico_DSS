Imports lm.Comol.Modules.Standard.ProjectManagement.Domain

Public Class ProjectWizardSteps
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

        End Get
    End Property

    Private Property IdProject As Long
        Get
            Return ViewStateOrDefault("IdProject", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProject") = value
        End Set
    End Property

    Private Property IdProjectCommunity As Integer
        Get
            Return ViewStateOrDefault("IdProjectCommunity", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProjectCommunity") = value
        End Set
    End Property
#End Region

    Public Event ItemCommand(ByVal tStep As lm.Comol.Core.TemplateMessages.Domain.WizardTemplateStep)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProjectManagement", "Modules", "ProjectManagement")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Sub InitializeControl(idProject As Long, idCommunity As Integer, personal As Boolean, forPortal As Boolean, items As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep)), fromPage As PageListType, idContainerCommunity As Integer)
        idProject = idProject
        IdProjectCommunity = idCommunity
        DisableAll = lm.Comol.UI.Presentation.SessionHelpers.CurrentUserContext.isAnonymous
        For Each item As lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep) In items
            item.Name = Resource.getValue("WizardProjectStep." & item.Id.Type.ToString)
            item.Tooltip = Resource.getValue("WizardProjectStep." & item.Id.Type.ToString & ".Tooltip")
            Dim m As String = Resource.getValue("WizardProjectStep." & item.Id.Type.ToString & ".Message")
            Select Case item.Id.Type
                Case WizardProjectStep.Settings
                    Dim oItem As dtoSettingsStep = DirectCast(item.Id, dtoSettingsStep)
                    item.Message = Resource.getValue("WizardProjectStepMessage." & item.Id.Type.ToString & "." & oItem.Availability.ToString())

                    item.Url = RootObject.EditProject(idProject, idCommunity, forPortal, personal, fromPage, idContainerCommunity)

                Case WizardProjectStep.ProjectUsers
                    Dim baseMessage As String = "WizardProjectStepMessage." & item.Id.Type.ToString & "."
                    Dim oItem As dtoResourcesStep = DirectCast(item.Id, dtoResourcesStep)
                    item.Url = RootObject.ProjectResources(idProject, idCommunity, forPortal, personal, fromPage, idContainerCommunity)
                    If oItem.Count = 0 Then
                        item.Message = Resource.getValue(baseMessage & "NoResource")
                    Else
                        Select Case oItem.Resources
                            Case 0, 1
                                baseMessage &= oItem.Resources & "."
                            Case Else
                                If oItem.Resources > 1 Then
                                    baseMessage &= "n."
                                Else
                                    baseMessage &= "0."
                                End If
                        End Select

                        Select Case oItem.Managers
                            Case 0, 1
                                baseMessage &= oItem.Managers
                            Case Else
                                If oItem.Managers > 1 Then
                                    baseMessage &= "n"
                                Else
                                    baseMessage &= "0"
                                End If
                        End Select
                        If oItem.Managers > 1 AndAlso oItem.Resources > 1 Then
                            item.Message = String.Format(Resource.getValue(baseMessage), oItem.Resources, oItem.Managers)
                        ElseIf oItem.Managers > 1 Then
                            item.Message = String.Format(Resource.getValue(baseMessage), oItem.Managers)
                        ElseIf oItem.Resources > 1 Then
                            item.Message = String.Format(Resource.getValue(baseMessage), oItem.Resources)
                        Else
                            item.Message = Resource.getValue(baseMessage)
                        End If

                    End If
                Case WizardProjectStep.Calendars
                    Dim baseMessage As String = "WizardProjectStepMessage." & item.Id.Type.ToString & "."
                    Dim oItem As dtoCalendarsStep = DirectCast(item.Id, dtoCalendarsStep)
                    item.Url = RootObject.ProjectCalendars(idProject, idCommunity, forPortal, personal, fromPage, idContainerCommunity)
                    If oItem.Calendars = 0 Then
                        item.Message = Resource.getValue(baseMessage & "NoCalendars")
                    Else
                        Select Case oItem.Calendars
                            Case 0, 1
                                baseMessage &= oItem.Calendars & "."
                            Case Else
                                If oItem.Calendars > 1 Then
                                    baseMessage &= "n."
                                Else
                                    baseMessage &= "0."
                                End If
                        End Select

                        Select Case oItem.Exceptions
                            Case 0, 1
                                baseMessage &= oItem.Exceptions
                            Case Else
                                If oItem.Exceptions > 1 Then
                                    baseMessage &= "n"
                                Else
                                    baseMessage &= "0"
                                End If
                        End Select
                        If oItem.Calendars > 1 AndAlso oItem.Exceptions > 1 Then
                            item.Message = String.Format(Resource.getValue(baseMessage), oItem.Calendars, oItem.Exceptions)
                        ElseIf oItem.Calendars > 1 Then
                            item.Message = String.Format(Resource.getValue(baseMessage), oItem.Calendars)
                        ElseIf oItem.Exceptions > 1 Then
                            item.Message = String.Format(Resource.getValue(baseMessage), oItem.Exceptions)
                        Else
                            item.Message = Resource.getValue(baseMessage)
                        End If

                    End If
                Case WizardProjectStep.Documents
                    If item.Status <> lm.Comol.Core.Wizard.WizardItemStatus.disabled Then
                        Dim baseMessage As String = "WizardProjectStepMessage." & item.Id.Type.ToString & "."
                        Dim oItem As dtoDocumentsStep = DirectCast(item.Id, dtoDocumentsStep)

                        item.Url = RootObject.ProjectAttachments(idProject, idCommunity, forPortal, personal, fromPage, idContainerCommunity)
                        If oItem.ActivitiesItemCount = 0 AndAlso oItem.ItemCount = 0 Then
                            item.Message = Resource.getValue(baseMessage & "NoDocuments")
                        Else
                            Select Case oItem.ItemCount
                                Case 0, 1
                                    baseMessage &= oItem.ItemCount & "."
                                Case Else
                                    If oItem.ItemCount > 1 Then
                                        baseMessage &= "n."
                                    Else
                                        baseMessage &= "0."
                                    End If
                            End Select

                            Select Case oItem.ActivitiesItemCount
                                Case 0, 1
                                    baseMessage &= oItem.ActivitiesItemCount
                                Case Else
                                    If oItem.ActivitiesItemCount > 1 Then
                                        baseMessage &= "n"
                                    Else
                                        baseMessage &= "0"
                                    End If
                            End Select
                         
                            If oItem.ItemCount > 1 AndAlso oItem.ActivitiesItemCount > 1 Then
                                item.Message = String.Format(Resource.getValue(baseMessage), oItem.ItemCount, oItem.ActivitiesItemCount)
                            ElseIf oItem.ItemCount > 1 Then
                                item.Message = String.Format(Resource.getValue(baseMessage), oItem.ItemCount)
                            ElseIf oItem.ActivitiesItemCount > 1 Then
                                item.Message = String.Format(Resource.getValue(baseMessage), oItem.ActivitiesItemCount)
                            Else
                                item.Message = Resource.getValue(baseMessage)
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
            Dim baseItem As lm.Comol.Core.Wizard.NavigableWizardItemBase(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoProjectManagementStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItemBase(Of dtoProjectManagementStep))

            Dim oDiv As HtmlControl = e.Item.FindControl("DVnavigationStep")
            If TypeOf baseItem Is lm.Comol.Core.Wizard.NavigableWizardItemSeparator(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoProjectManagementStep) Then
                Dim oLiteral As Literal = e.Item.FindControl("LTseparator")
                oDiv.Visible = False
                oLiteral.Visible = True
            Else
                Dim item As lm.Comol.Core.Wizard.NavigableWizardItem(Of lm.Comol.Modules.Standard.ProjectManagement.Domain.dtoProjectManagementStep) = DirectCast(e.Item.DataItem, lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoProjectManagementStep))

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
                        'oHyperlink.NavigateUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(IdCall, IdCallCommunity, item.Id.Type, CurrentView)
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
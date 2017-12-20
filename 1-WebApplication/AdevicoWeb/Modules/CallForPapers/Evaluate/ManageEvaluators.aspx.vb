Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation.Evaluation
Imports lm.Comol.Modules.CallForPapers.Domain.Evaluation
Imports lm.ActionDataContract
Imports System.Linq
Public Class ManageEvaluators
    Inherits PageBase
    Implements IViewManageEvaluators

#Region "Context"
    Private _Presenter As ManageEvaluatorsPresenter
    Private ReadOnly Property CurrentPresenter() As ManageEvaluatorsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ManageEvaluatorsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewManageEvaluators.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewManageEvaluators.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewManageEvaluators.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters Implements IViewManageEvaluators.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters).GetByString(Request.QueryString("View"), lm.Comol.Modules.CallForPapers.Domain.CallStatusForSubmitters.SubmissionClosed)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewManageEvaluators.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.BTNaddCommitteeMemberTop.Visible = value
            Me.BTNsaveEvaluatorsBottom.Visible = value
            Me.BTNsaveEvaluatorsTop.Visible = value
            Me.CBXallowMultipleCommittees.Visible = value
            Me.LBallowMultipleCommittees.Visible = value
        End Set
    End Property
    Private Property AllowAddEvaluator As Boolean Implements IViewManageEvaluators.AllowAddEvaluator
        Get
            Return ViewStateOrDefault("AllowAddEvaluator", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowAddEvaluator") = value
            Me.BTNaddCommitteeMemberTop.Visible = value
        End Set
    End Property
    Private Property AllowMultipleCommittees As Boolean Implements IViewManageEvaluators.AllowMultipleCommittees
        Get
            Return ViewStateOrDefault("AllowMultipleCommittees", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowMultipleCommittees") = value
            Me.CBXallowMultipleCommittees.Enabled = value
        End Set
    End Property
    Private Property MultipleCommittees As Boolean Implements IViewManageEvaluators.MultipleCommittees
        Get
            Return Me.CBXallowMultipleCommittees.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CBXallowMultipleCommittees.Checked = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewManageEvaluators.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewManageEvaluators.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewManageEvaluators.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdOnlyOneCommittee As Long Implements IViewManageEvaluators.IdOnlyOneCommittee
        Get
            Return ViewStateOrDefault("IdOnlyOneCommittee", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdOnlyOneCommittee") = value
        End Set
    End Property
    Private Property AvailableCommittees As List(Of dtoBaseCommittee) Implements IViewManageEvaluators.AvailableCommittees
        Get
            Return ViewStateOrDefault("AvailableCommittees", New List(Of dtoBaseCommittee))
        End Get
        Set(value As List(Of dtoBaseCommittee))
            Me.ViewState("AvailableCommittees") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
#Region ""
    Protected ReadOnly Property GetTableCss() As String
        Get
            If AvailableCommittees.Count <= 1 Then
                Return "onecommission"
            ElseIf Me.CBXallowMultipleCommittees.Checked AndAlso AvailableCommittees.Count > 1 Then
                Return "multiple"
            Else
                Return "single"
            End If
        End Get
    End Property
    Protected ReadOnly Property DialogTitleTranslation() As String
        Get
            Return Resource.getValue("DialogTitleTranslation.SelectEvaluators")
        End Get
    End Property


#End Region
   

    Private Sub EditEvaluationCommittees_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCommittees", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnocalls.Text = Resource.getValue("LBnoCalls")

            .setHyperLink(HYPbackTop, True, True)
            .setHyperLink(HYPbackBottom, True, True)
            .setButton(BTNsaveEvaluatorsBottom, True, , , True)
            .setButton(BTNsaveEvaluatorsTop, True, , , True)
            .setButton(BTNaddCommitteeMemberTop, True, , , True)
            .setLabel(LBallowMultipleCommittees)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType) Implements IViewManageEvaluators.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewManageEvaluators.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, lm.Comol.Modules.CallForPapers.Domain.ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewManageEvaluators.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = lm.Comol.Modules.CallForPapers.Domain.RootObject.EditCommiteeByStep(PreloadIdCall, idCommunity, WizardEvaluationStep.FullManageEvaluators, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(url As String) Implements IViewManageEvaluators.SetActionUrl
        Me.HYPbackBottom.Visible = True
        Me.HYPbackBottom.NavigateUrl = BaseUrl & url
        Me.HYPbackTop.Visible = True
        Me.HYPbackTop.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub SetContainerName(itemName As String) Implements IViewBaseEditEvaluationSettings.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.FullManageEvaluators.ToString())
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = itemName
    End Sub
    Private Sub SetContainerName(communityName As String, callName As String) Implements IViewManageEvaluators.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & WizardEvaluationStep.FullManageEvaluators.ToString())
        Master.ServiceTitle = title
        If String.IsNullOrEmpty(communityName) Then
            Master.ServiceTitleToolTip = title & " - " & callName
        Else
            Master.ServiceTitleToolTip = title & " - " & callName
        End If
    End Sub

    Private Sub LoadWizardSteps(idCall As Long, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep))) Implements IViewBaseEditEvaluationSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, idCommunity, PreloadView, steps)
    End Sub
    Private Sub LoadWizardSteps(idCall As Long, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of dtoEvaluationStep)), err As EvaluationEditorErrors) Implements IViewBaseEditEvaluationSettings.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, idCommunity, PreloadView, steps, err)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType) Implements IViewManageEvaluators.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Public Sub HideErrorMessages() Implements IViewManageEvaluators.HideErrorMessages
        Me.CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors) Implements IViewManageEvaluators.DisplayError
        DisplayError(err, Helpers.MessageType.error)
    End Sub
    Private Sub DisplayWarning(err As EvaluationEditorErrors) Implements IViewManageEvaluators.DisplayWarning
        DisplayError(err, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayError(err As EvaluationEditorErrors, errorType As Helpers.MessageType)
        Me.CTRLmessages.Visible = (err <> EvaluationEditorErrors.None)
        Me.CTRLmessages.InitializeControl(Resource.getValue("EvaluationEditorErrors." & err.ToString), errorType)
        Me.DVselectUsers.Visible = False
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewManageEvaluators.DisplaySettingsSaved
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Resource.getValue("DisplayCommitteesSettingsSaved"), Helpers.MessageType.success)
        Me.DVselectUsers.Visible = False
    End Sub
    Private Sub LoadEvaluators(evaluators As List(Of dtoCommitteeMember), multipleCommittee As Boolean, committees As List(Of dtoBaseCommittee)) Implements IViewManageEvaluators.LoadEvaluators
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.CBXallowMultipleCommittees.Checked = multipleCommittee
        Me.AvailableCommittees = committees

        Me.RPTassignments.DataSource = evaluators
        Me.RPTassignments.DataBind()
    End Sub
    Private Function GetMembers() As List(Of dtoCommitteeMember) Implements IViewManageEvaluators.GetMembers
        Dim items As New List(Of dtoCommitteeMember)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTassignments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim member As New dtoCommitteeMember
            Dim oLiteral As Literal = row.FindControl("LTidCallEvaluator")
            member.IdCallEvaluator = CLng(oLiteral.Text)
            oLiteral = row.FindControl("LTidPerson")
            member.IdPerson = CInt(oLiteral.Text)

            If IdOnlyOneCommittee > 0 Then
                member.Committees.Add(IdOnlyOneCommittee)
            Else
                Dim mView As MultiView = row.FindControl("MLVselectCommittee")
                Dim v As View = row.FindControl("VIWsingle")
                If mView.GetActiveView Is v Then
                    Dim oDropdownlist As DropDownList = row.FindControl("DDLcommittees")
                    If oDropdownlist.SelectedIndex > -1 AndAlso oDropdownlist.SelectedValue > 0 Then
                        member.Committees.Add(CLng(oDropdownlist.SelectedValue))
                    End If
                ElseIf mView.ActiveViewIndex > 0 Then
                    Dim oSelect As HtmlSelect = row.FindControl("SLBcommittees")
                    member.Committees = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
                End If
            End If
            items.Add(member)
        Next
        Return items
    End Function

    Private Sub ReloadEditor(url As String) Implements IViewManageEvaluators.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    'Private Sub InitializeAddFieldControl(idCall As Long) Implements IViewCallEditor.InitializeAddFieldControl
    '    Me.CTRLaddField.InitializeControl(idCall)
    'End Sub
#End Region

    Private Sub RPTassignments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTassignments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim member As dtoCommitteeMember = DirectCast(e.Item.DataItem, dtoCommitteeMember)

            Dim oGeneric As HtmlTableCell = e.Item.FindControl("TDcommittee")
            oGeneric.Visible = AvailableCommittees.Count > 1
            If AvailableCommittees.Count > 1 Then
                Dim mView As MultiView = e.Item.FindControl("MLVselectCommittee")
                Dim v As View = e.Item.FindControl("VIWsingle")
                If AllowMultipleCommittees AndAlso MultipleCommittees Then
                    v = e.Item.FindControl("VIWmultiple")

                    Dim oSelect As HtmlSelect = e.Item.FindControl("SLBcommittees")
                    oSelect.DataSource = AvailableCommittees
                    oSelect.DataTextField = "Name"
                    oSelect.DataValueField = "Id"
                    oSelect.DataBind()
                    For Each idCommittee As Long In member.Committees
                        Dim oListItem As ListItem = oSelect.Items.FindByValue(idCommittee)
                        If Not IsNothing(oListItem) Then
                            oListItem.Selected = True
                        End If
                    Next
                    oSelect.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
                    oSelect.Disabled = Not AllowSave
                    Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNevaluatorsCommitteeSelectAll")
                    oHtmlControl.Attributes.Add("title", Resource.getValue("SPNevaluatorsCommitteeSelectAll.ToolTip"))
                    oHtmlControl.Visible = AllowSave
                    oHtmlControl = e.Item.FindControl("SPNevaluatorsCommitteeSelectNone")
                    oHtmlControl.Attributes.Add("title", Resource.getValue("SPNevaluatorsCommitteeSelectNone.ToolTip"))
                    oHtmlControl.Visible = AllowSave
                Else
                    Dim oDropDownList As DropDownList = e.Item.FindControl("DDLcommittees")
                    oDropDownList.DataSource = AvailableCommittees
                    oDropDownList.DataTextField = "Name"
                    oDropDownList.DataValueField = "Id"
                    oDropDownList.DataBind()
                    oDropDownList.Items.Insert(0, New ListItem(Resource.getValue("select-committee"), 0))
                    If member.Committees.Any Then
                        If Not IsNothing(oDropDownList.Items.FindByValue(member.Committees.FirstOrDefault())) Then
                            oDropDownList.SelectedValue = member.Committees.FirstOrDefault()
                        Else
                            oDropDownList.SelectedIndex = 0
                        End If
                    Else
                        oDropDownList.SelectedIndex = 0
                    End If
                End If
                mView.SetActiveView(v)
            End If
            Dim oButton As Button = e.Item.FindControl("BTNdeleteMember")
            oButton.CommandArgument = member.IdCallEvaluator
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTevaluator_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTcommittee_t")
            Me.Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTactions_t")
            Me.Resource.setLiteral(oLiteral)

            Dim oGeneric As HtmlTableCell = e.Item.FindControl("THcommittee")
            oGeneric.Visible = AvailableCommittees.Count > 1
        End If
    End Sub
    Private Sub RPTassignments_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTassignments.ItemCommand
        Dim IdCallEvaluator As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            IdCallEvaluator = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Me.CurrentPresenter.RemoveEvaluator(GetMembers(), IdCallEvaluator)
        End If
    End Sub

    Private Sub BTNsaveEvaluatorsTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveEvaluatorsBottom.Click, BTNsaveEvaluatorsTop.Click
        Me.CurrentPresenter.SaveSettings(GetMembers())
    End Sub
    Private Sub BTNaddCommitteeMemberTop_Click(sender As Object, e As System.EventArgs) Handles BTNaddCommitteeMemberTop.Click
        If IdCommunity > 0 Then
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.CommunityUsers, True, IdCommunity, AllUsers, Nothing, Resource.getValue("addMemberDescription"))
        Else
            Me.CTRLselectUsers.InitializeControl(lm.Comol.Core.BaseModules.ProfileManagement.UserSelectionType.SystemUsers, True, False, AllUsers, Nothing, Resource.getValue("addMemberDescription"))
        End If
        '  Me.SetFocus(Me.CTRLselectUsers.GetDefaultTextField)
        Me.DVselectUsers.Visible = True
    End Sub

    Private Function AllUsers() As List(Of Integer)
        Dim users As New List(Of Int32)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTassignments.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oLiteral As Literal = row.FindControl("LTidPerson")
            users.Add(CInt(oLiteral.Text))
        Next
        Return users
    End Function
    Private Sub CTRLselectUsers_UsersSelected(idUsers As System.Collections.Generic.List(Of Integer)) Handles CTRLselectUsers.UsersSelected
        Me.DVselectUsers.Visible = False
        If IdCall > 0 Then
            Me.DVselectUsers.Visible = False
            Me.CurrentPresenter.AddEvaluators(GetMembers(), idUsers)
        End If
    End Sub
    Protected Sub CloseWindow() Handles CTRLselectUsers.CloseWindow
        Me.DVselectUsers.Visible = False
        '  Me.SetFocus(Me.btn)
    End Sub
    Private Sub RedirectToUrl(url As String) Implements IViewBaseEditEvaluationSettings.RedirectToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Sub CBXallowMultipleCommittees_CheckedChanged(sender As Object, e As System.EventArgs) Handles CBXallowMultipleCommittees.CheckedChanged
        Me.CurrentPresenter.SaveCommitteeAssignmentPolicy(GetMembers(), Me.CBXallowMultipleCommittees.Checked)
    End Sub

   
End Class
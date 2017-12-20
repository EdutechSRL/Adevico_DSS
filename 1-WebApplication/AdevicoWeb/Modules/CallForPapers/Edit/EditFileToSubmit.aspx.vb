Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditFileToSubmit
    Inherits PageBase
    Implements IViewEditFileToSubmit

#Region "Context"
    Private _Presenter As EditFileToSubmitPresenter
    Private ReadOnly Property CurrentPresenter() As EditFileToSubmitPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditFileToSubmitPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseEditCall.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewBaseEditCall.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseEditCall.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadType As CallForPaperType Implements IViewBaseEditCall.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewBaseEditCall.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewBaseEditCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
            Me.BTNaddRequiredFile.Visible = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewBaseEditCall.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
            Resource.setHyperLink(HYPpreviewCallBottom, value.ToString(), True, True)
            Resource.setHyperLink(HYPpreviewCallTop, value.ToString(), True, True)
            Resource.setHyperLink(HYPbackTop, value.ToString, True, True)
            Resource.setHyperLink(HYPbackBottom, value.ToString, True, True)
        End Set
    End Property
    Private Property IdCall As Long Implements IViewBaseEditCall.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewBaseEditCall.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewBaseEditCall.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property Availablesubmitters As List(Of dtoSubmitterType) Implements IViewEditFileToSubmit.Availablesubmitters
        Get
            Return ViewStateOrDefault("Availablesubmitters", New List(Of dtoSubmitterType))
        End Get
        Set(value As List(Of dtoSubmitterType))
            Me.ViewState("Availablesubmitters") = value
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

    Public WriteOnly Property AllowUpdateTags As Boolean Implements IViewBaseEditCall.AllowUpdateTags
        Set(value As Boolean)

        End Set
    End Property
#End Region

    Private Sub EditCallAvailability_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
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
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnocalls.Text = Resource.getValue("LBnoCalls." & CallType.ToString())

            .setHyperLink(HYPpreviewCallBottom, CallType.ToString(), True, True)
            .setHyperLink(HYPpreviewCallTop, CallType.ToString(), True, True)
            .setHyperLink(HYPbackTop, CallType.ToString, True, True)
            .setHyperLink(HYPbackBottom, CallType.ToString, True, True)
            .setButton(BTNsaveFileToSubmitBottom, True, , , True)
            .setButton(BTNsaveFileToSubmitTop, True, , , True)

            .setLabel(LBrequiredFilesHideTop)
            .setLabel(LBrequiredFileShowTop)
            .setLabel(LBrequiredFilesDescription)
            .setButton(BTNaddRequiredFile, True, , , True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditFileToSubmit.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditFileToSubmit.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.EditCallTemplateMail(CallType, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseEditCall.SetActionUrl
        If action = CallStandardAction.PreviewCall Then
            Me.HYPpreviewCallBottom.Visible = True
            Me.HYPpreviewCallBottom.NavigateUrl = BaseUrl & url
            Me.HYPpreviewCallTop.Visible = True
            Me.HYPpreviewCallTop.NavigateUrl = BaseUrl & url
        Else
            'Select Case action
            '    Case CallStandardAction.List
            Me.HYPbackBottom.Visible = True
            Me.HYPbackBottom.NavigateUrl = BaseUrl & url
            Me.HYPbackTop.Visible = True
            Me.HYPbackTop.NavigateUrl = BaseUrl & url
            '    Case CallStandardAction.Manage
            '        Me.HYPmanage.Visible = True
            '        Me.HYPmanage.NavigateUrl = BaseUrl & url
            '    Case CallStandardAction.Add
            '        Me.HYPcreateCall.Visible = True
            '        Me.HYPcreateCall.NavigateUrl = BaseUrl & url
            'End Select
        End If
    End Sub

    Private Sub SetContainerName(action As CallStandardAction, name As String, itemName As String) Implements IViewBaseEditCall.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & action.ToString & "." & CallType.ToString())
        Master.ServiceTitle = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = String.Format(title, itemName)
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & action.Edit.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub

    Private Sub LoadWizardSteps(idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of WizardCallStep))) Implements IViewBaseEditCall.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, type, idCommunity, PreloadView, steps)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewBaseEditCall.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Private Sub HideErrorMessages() Implements IViewEditFileToSubmit.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayNoTemplates() Implements IViewEditFileToSubmit.DisplayNoFileToSubmit
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoFileToSubmit"), Helpers.MessageType.info)

        Me.LBrequiredFilesHideTop.Visible = False
        Me.LBrequiredFileShowTop.Visible = False
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewEditFileToSubmit.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayFileToSubmitSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub DisplayError(errors As EditorErrors) Implements IViewEditFileToSubmit.DisplayError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("EditorErrors." & errors.ToString), Helpers.MessageType.error)
    End Sub

    Private Function GetDefaultRequestedFile() As dtoCallRequestedFile Implements IViewEditFileToSubmit.GetDefaultRequestedFile
        Dim dto As New dtoCallRequestedFile
        With dto
            .Id = 0
            .Mandatory = False
            .Name = Resource.getValue("RequiredFile.Default.Name")
            .Description = Resource.getValue("RequiredFile.Default.Description")
            .Tooltip = Resource.getValue("RequiredFile.Default.Tooltip")
            .SubmitterAssignments = (From a In Availablesubmitters Select a.Id).ToList()
        End With
        Return dto
    End Function
    Private Function GetRequestedFile() As List(Of dtoCallRequestedFile) Implements IViewEditFileToSubmit.GetRequestedFiles
        Dim items As New List(Of dtoCallRequestedFile)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTrequiredFiles.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim requestedFile As New dtoCallRequestedFile()
            Dim oLiteral As Literal = row.FindControl("LTidRequiredFile")
            Dim oTextBox As TextBox = row.FindControl("TXBdescription")

            requestedFile.Id = CLng(oLiteral.Text)
            requestedFile.Description = oTextBox.Text

            oTextBox = row.FindControl("TXBhelp")
            requestedFile.Tooltip = oTextBox.Text

            oTextBox = row.FindControl("TXBrequiredFileName")
            requestedFile.Name = oTextBox.Text

            Dim oChecbox As HtmlInputCheckBox = row.FindControl("CBXrequiredFileMandatory")
            oChecbox.Checked = requestedFile.Mandatory

            Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
            For Each item As ListItem In oSelect.Items
                If item.Selected Then
                    requestedFile.SubmitterAssignments.Add(CLng(item.Value))
                End If
            Next
            Dim hidden As HtmlInputHidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                requestedFile.DisplayOrder = CInt(hidden.Value)
            End If

            items.Add(requestedFile)
        Next

        Return items.OrderBy(Function(s) s.DisplayOrder).ToList
    End Function
    Private Sub LoadFiles(items As List(Of dtoRequestedFilePermission)) Implements IViewEditFileToSubmit.LoadFiles
        Me.RPTrequiredFiles.DataSource = items
        Me.RPTrequiredFiles.DataBind()

        Me.LBrequiredFilesHideTop.Visible = (items.Count > 0)
        Me.LBrequiredFileShowTop.Visible = (items.Count > 0)
    End Sub
    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewEditFileToSubmit.LoadSubmitterTypes
        Me.Availablesubmitters = submitters
    End Sub
#End Region

    Private Sub BTNsaveFileToSubmitBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveFileToSubmitBottom.Click, BTNsaveFileToSubmitTop.Click
        Me.CurrentPresenter.SaveSettings(GetRequestedFile)
    End Sub

    Private Sub RPTrequiredFiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoRequestedFilePermission = DirectCast(e.Item.DataItem, dtoRequestedFilePermission)

            Dim oLabel As Label = e.Item.FindControl("LBmoveRequiredFile")
            oLabel.Visible = AllowSave

            oLabel = e.Item.FindControl("LBrequiredFileName_t")
            Resource.setLabel(oLabel)

            Dim oTextBox As TextBox = e.Item.FindControl("TXBrequiredFileName")
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBrequiredFileMandatory_t")
            Resource.setLabel(oLabel)

            Dim oButton As Button = e.Item.FindControl("BTNrequiredFileVirtualDelete")
            oButton.CommandArgument = dto.Id
            oButton.Visible = dto.AllowVirtualDelete AndAlso AllowSave

            oLabel = e.Item.FindControl("LBfieldDescription_t")
            Resource.setLabel(oLabel)

            oTextBox = e.Item.FindControl("TXBdescription")
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBfieldHelp_t")
            Resource.setLabel(oLabel)

            oTextBox = e.Item.FindControl("TXBhelp")
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBrequiredFileMandatory_t")
            Me.Resource.setLabel(oLabel)
            Dim oChecbox As HtmlInputCheckBox = e.Item.FindControl("CBXrequiredFileMandatory")
            oChecbox.Checked = dto.File.Mandatory
            oChecbox.Disabled = Not AllowSave

            oLabel = e.Item.FindControl("LBrequiredFileSubmitters_t")
            Me.Resource.setLabel(oLabel)

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = dto.File.DisplayOrder

            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBsubmitters")
            oSelect.DataSource = Availablesubmitters
            oSelect.DataTextField = "Name"
            oSelect.DataValueField = "Id"
            oSelect.DataBind()
            For Each idSubmitter As Long In dto.File.SubmitterAssignments
                Dim oListItem As ListItem = oSelect.Items.FindByValue(idSubmitter)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next

            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
            oSelect.Disabled = Not AllowSave
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNrequiredFileSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmittersSelecttAll.ToolTip"))
            oHtmlControl.Visible = AllowSave
            oHtmlControl = e.Item.FindControl("SPNrequiredFileSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmittersSelecttNone.ToolTip"))
            oHtmlControl.Visible = AllowSave
        End If
    End Sub
    Private Sub RPTrequiredFiles_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTrequiredFiles.ItemCommand
        Dim idRequiredFile As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idRequiredFile = CLng(e.CommandArgument)
        End If
        If e.CommandName = "delete" Then
            Me.CurrentPresenter.RemoveRequestedFile(idRequiredFile)
        End If
    End Sub

    Private Sub BTNaddRequiredFile_Click(sender As Object, e As System.EventArgs) Handles BTNaddRequiredFile.Click
        Me.CurrentPresenter.AddRequiredFile(GetDefaultRequestedFile)
    End Sub

    
End Class
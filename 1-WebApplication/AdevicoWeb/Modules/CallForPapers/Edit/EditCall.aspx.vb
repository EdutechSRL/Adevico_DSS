Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditCall
    Inherits PageBase
    Implements IViewCallEditor
    
#Region "Context"
    Private _Presenter As EditCallEditorPresenter
    Private ReadOnly Property CurrentPresenter() As EditCallEditorPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditCallEditorPresenter(Me.PageUtility.CurrentContext, Me)
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
            Me.BTNaddSectionTop.Visible = value
            Me.BTNsaveEditorBottom.Visible = value
            Me.BTNsaveEditorTop.Visible = value
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
    Private Property Availablesubmitters As List(Of dtoSubmitterType) Implements IViewCallEditor.Availablesubmitters
        Get
            Return ViewStateOrDefault("Availablesubmitters", New List(Of dtoSubmitterType))
        End Get
        Set(value As List(Of dtoSubmitterType))
            Me.ViewState("Availablesubmitters") = value
        End Set
    End Property
    Private ReadOnly Property DefaultDescriptionName As String Implements IViewCallEditor.DefaultSectionDescription
        Get
            Return Resource.getValue("DefaultSectionDescription")
        End Get
    End Property

    Private ReadOnly Property DefaultSectionName As String Implements IViewCallEditor.DefaultSectionName
        Get
            Return Resource.getValue("DefaultSectionName")
        End Get
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
#Region "Internal"
    Protected ReadOnly Property AddFieldDialogTitle As String
        Get
            Return Resource.getValue("AddFieldDialogTitle")
        End Get
    End Property

#End Region
    Private Sub EditCallAvailability_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()


    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True


        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")

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
            .setButton(BTNsaveEditorBottom, True, , , True)
            .setButton(BTNsaveEditorTop, True, , , True)
            .setButton(BTNaddSectionTop, True, , , True)
            .setLabel(LBcollapseAllTop)
            .setLabel(LBexpandAllTop)
            .setLabel(LBfieldsHideTop)
            .setLabel(LBfieldsShowTop)

            .setButton(BTNcreateField, True, , , True)
            .setButton(BTNcloseCreateFieldWindow, True, , , True)
        End With
    End Sub


    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewCallEditor.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewCallEditor.SendUserAction
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

        dto.DestinationUrl = RootObject.EditCallSubmissionEditor(CallType, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")

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
        Dim title As String = Me.Resource.getValue("serviceTitle." & action.Edit.ToString & "." & CallType.ToString())
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
    Public Sub HideErrorMessages() Implements IViewCallEditor.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(err As EditorErrors) Implements IViewCallEditor.DisplayError
        CTRLmessages.Visible = (err <> EditorErrors.None)
        CTRLmessages.InitializeControl(Resource.getValue("EditorErrors." & err.ToString), Helpers.MessageType.error)
    End Sub
    Private Sub DisplaySettingsSaved() Implements IViewCallEditor.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplaySectionsSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoCallField))) Implements IViewCallEditor.LoadSections
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewCallEditor.LoadSubmitterTypes
        Me.Availablesubmitters = submitters
    End Sub
    Private Function GetSections() As List(Of dtoCallSection(Of dtoCallField)) Implements IViewCallEditor.GetSections
        Dim sections As New List(Of dtoCallSection(Of dtoCallField))
        Dim fields As New List(Of dtoCallField)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim section As New dtoCallSection(Of dtoCallField)()
            Dim oLiteral As Literal = row.FindControl("LTidSection")
            Dim oTextBox As TextBox = row.FindControl("TXBsectionName")
            section.Id = CLng(oLiteral.Text)
            section.Name = oTextBox.Text

            oTextBox = row.FindControl("TXBsectionDescription")
            section.Description = oTextBox.Text
            Dim oRepeater As Repeater = row.FindControl("RPTfields")

            fields.AddRange(GetFields(oRepeater))

            Dim hidden As HtmlInputHidden = row.FindControl("HDNdisplayOrderSection")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                section.DisplayOrder = CInt(hidden.Value)
            End If
            sections.Add(section)
        Next
        For Each section As dtoCallSection(Of dtoCallField) In sections
            section.Fields = (From f In fields Where f.IdSection = section.Id Select f).OrderBy(Function(f) f.DisplayOrder).ToList
        Next
        Return sections.OrderBy(Function(s) s.DisplayOrder).ToList
    End Function
    Private Function GetFields(oRepeater As Repeater) As List(Of dtoCallField)
        Dim fields As New List(Of dtoCallField)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoCallField
            Dim oLiteral As Literal = row.FindControl("LTidField")
            Dim oTextBox As TextBox = row.FindControl("TXBfieldName")
            Dim oCheck As HtmlInputCheckBox = row.FindControl("CBXmandatory")

            Dim oControl As UC_EditField = row.FindControl("CTRLeditField")
            field = oControl.GetField

            field.Id = CLng(oLiteral.Text)
            field.Name = oTextBox.Text
            field.Mandatory = oCheck.Checked

            TagsAdd(field.Tags)

            Dim hidden As HtmlInputHidden = row.FindControl("HDNsectionOwner")

            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value.Replace("section_", "")) Then
                field.IdSection = CLng(hidden.Value.Replace("section_", ""))
            End If
            hidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                field.DisplayOrder = CInt(hidden.Value)
            End If

            Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
            field.Submitters = (From i As ListItem In oSelect.Items Where i.Selected Select CLng(i.Value)).ToList()
            fields.Add(field)
        Next
        Return fields
    End Function
    Private Sub ReloadEditor(url As String) Implements IViewCallEditor.ReloadEditor
        PageUtility.RedirectToUrl(url)
    End Sub
    'Private Sub InitializeAddFieldControl(idCall As Long) Implements IViewCallEditor.InitializeAddFieldControl
    '    Me.CTRLaddField.InitializeControl(idCall)
    'End Sub
#End Region

    Private Sub RPTsections_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim section As dtoCallSection(Of dtoCallField) = DirectCast(e.Item.DataItem, dtoCallSection(Of dtoCallField))

            Dim oLiteral As Literal = e.Item.FindControl("LTidSection")
            oLiteral.Text = section.Id
            Dim oLabel As Label = e.Item.FindControl("LBsectionName_t")
            Me.Resource.setLabel(oLabel)
            Dim oTextBox As TextBox = e.Item.FindControl("TXBsectionName")
            oTextBox.Text = section.Name
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBsectionDescription_t")
            Me.Resource.setLabel(oLabel)
            oTextBox = e.Item.FindControl("TXBsectionDescription")
            oTextBox.Text = section.Description
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBmoveSection")
            oLabel.ToolTip = Resource.getValue("LBmoveSection.Text")
            oLabel.Visible = AllowSave

            Dim oButton As Button = e.Item.FindControl("BTNaddField")
            oButton.CommandArgument = section.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            oButton = e.Item.FindControl("BTNcloneSection")
            oButton.CommandArgument = section.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            oButton = e.Item.FindControl("BTNdeleteSection")
            oButton.CommandArgument = section.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            Dim hyperlink As HyperLink = e.Item.FindControl("HYPtoTopSection")
            hyperlink.Visible = (section.Fields.Count > 2)
            Resource.setHyperLink(hyperlink, True, True)
            hyperlink.NavigateUrl = "#section_" & section.Id.ToString
        End If
    End Sub
    Private Sub RPTsections_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsections.ItemCommand
        Dim idSection As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idSection = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Me.CurrentPresenter.RemoveSection(GetSections(), idSection)
        ElseIf e.CommandName = "addfield" Then
            Me.HDNidSection.Value = idSection
            Me.CTRLaddField.InitializeControl(IdCall)
            Me.LTscriptOpen.Visible = True
            Me.Page.MaintainScrollPositionOnPostBack = False
            'ClientScript.RegisterClientScriptBlock(GetType(), "updateAccordian", LTupdateAccordian.Text)

        ElseIf e.CommandName = "clonesection" Then
            Me.CurrentPresenter.CloneSection(GetSections(), idSection)
        End If
    End Sub

    Protected Sub RPTfields_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim field As dtoCallField = DirectCast(e.Item.DataItem, dtoCallField)

            Dim oLiteral As Literal = e.Item.FindControl("LTidField")
            oLiteral.Text = field.Id
            Dim oLabel As Label = e.Item.FindControl("LBfieldName_t")
            Me.Resource.setLabel(oLabel)
            Dim oTextBox As TextBox = e.Item.FindControl("TXBfieldName")
            oTextBox.Text = field.Name
            oTextBox.Enabled = AllowSave

            oLabel = e.Item.FindControl("LBfieldType")
            oLabel.Text = Resource.getValue("Field.FieldType." & field.Type.ToString)

            oLabel = e.Item.FindControl("LBfieldMandatory_t")
            Me.Resource.setLabel(oLabel)

            oLabel = e.Item.FindControl("LBmoveField")
            oLabel.ToolTip = Resource.getValue("LBmoveField.Text")
            oLabel.Visible = AllowSave


            Dim oCheck As HtmlInputCheckBox = e.Item.FindControl("CBXmandatory")
            oCheck.Checked = field.Mandatory
            oCheck.Disabled = Not AllowSave

            Dim oButton As Button = e.Item.FindControl("BTNdeleteField")
            oButton.CommandArgument = field.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            oButton = e.Item.FindControl("BTNcloneField")
            oButton.CommandArgument = field.Id
            oButton.Visible = AllowSave
            Resource.setButton(oButton, True)

            Dim oControl As UC_EditField = e.Item.FindControl("CTRLeditField")
            oControl.InitializeControl(field, AllowSave)
            oLabel = e.Item.FindControl("LBfieldSubmitters_t")
            Me.Resource.setLabel(oLabel)

            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBsubmitters")
            oSelect.DataSource = Availablesubmitters
            oSelect.DataTextField = "Name"
            oSelect.DataValueField = "Id"
            oSelect.DataBind()
            For Each idSubmitter As Long In field.Submitters
                Dim oListItem As ListItem = oSelect.Items.FindByValue(idSubmitter)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next
            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
            oSelect.Disabled = Not AllowSave
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNfieldSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNfieldSelectAll.ToolTip"))
            oHtmlControl.Visible = AllowSave
            oHtmlControl = e.Item.FindControl("SPNfieldSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNfieldSelectNone.ToolTip"))
            oHtmlControl.Visible = AllowSave

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNsectionOwner")
            hidden.Value = "section_" & field.IdSection
            hidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = field.DisplayOrder
        End If
    End Sub
    Protected Sub RPTfields_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
        Dim idField As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idField = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Me.CurrentPresenter.RemoveField(GetSections(), idField)
        ElseIf e.CommandName = "clonefield" Then
            Me.CurrentPresenter.CloneField(GetSections(), idField)
        End If
    End Sub

    Protected Sub AddOption(idField As Long, name As String, isDefault As Boolean, isFreeText As Boolean)
        Me.CurrentPresenter.AddOption(GetSections(), idField, name, isDefault, isFreeText)
    End Sub
    Protected Sub SetAsDefaultOption(idOption As Long, isDefault As Boolean)
        Me.CurrentPresenter.SetAsDefaultOption(GetSections(), idOption, isDefault)
    End Sub
    Protected Sub RemoveOption(idOption As Long)
        Me.CurrentPresenter.RemoveOption(GetSections(), idOption)
    End Sub
    Protected Sub SaveDisclaimerType(idField As Long, dType As lm.Comol.Modules.CallForPapers.Domain.DisclaimerType)
        Me.CurrentPresenter.SaveDisclaimerType(GetSections(), idField, dType)
    End Sub
    Private Sub BTNsaveEditorBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveEditorBottom.Click, BTNsaveEditorTop.Click
        Me.CurrentPresenter.SaveSettings(GetSections())

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
    End Sub
    Private Sub BTNaddSectionTop_Click(sender As Object, e As System.EventArgs) Handles BTNaddSectionTop.Click
        Me.CurrentPresenter.AddSection(GetSections(), DefaultSectionName, DefaultDescriptionName)
    End Sub


    Protected Sub BTNcreateField_Click(sender As Object, e As System.EventArgs) Handles BTNcreateField.Click
        Dim idSection As Long = 0
        If Not String.IsNullOrEmpty(Me.HDNidSection.Value) Then
            idSection = CLng(Me.HDNidSection.Value.Replace("section-", ""))
            Dim fields As List(Of FieldDefinition) = Me.CTRLaddField.CreateFields(GetSections(), idSection)
            If (fields.Count > 0) Then
                PageUtility.RedirectToUrl(RootObject.CallSubmissionEditorFieldAdded(fields(0).Id, CallType, IdCall, IdCommunity, PreloadView))
            Else
                Me.LTscriptOpen.Visible = False
                Me.Page.MaintainScrollPositionOnPostBack = True
            End If
        End If
    End Sub
    Protected Sub BTNcloseCreateFieldWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseCreateFieldWindow.Click
        Me.LTscriptOpen.Visible = False
    End Sub


    Dim _currentTags As String = ""

    Private Sub TagsAdd(ByVal itemTags As String)

        Dim addedTags As String = String.Format("{0},{1}", _currentTags, itemTags)
        Dim allTags As String() = addedTags.Split(",")

        '_currentTags = ""
        Dim IsFirst As Boolean = True

        For Each tg As String In allTags.Distinct().ToList()

            If Not String.IsNullOrWhiteSpace(tg) Then
                If IsFirst Then
                    _currentTags = tg
                    IsFirst = False
                Else
                    _currentTags = String.Format("{0},{1}", _currentTags, tg)
                End If
            End If
        Next

        Me.tagsinputSuggest.Text = _currentTags
    End Sub

    Public Property TagCurrent As String Implements IViewCallEditor.TagCurrent
        Get
            Return _currentTags
        End Get
        Set(value As String)
            _currentTags = value
            Me.tagsinputSuggest.Text = value
        End Set
    End Property

    Public WriteOnly Property AllowUpdateTags As Boolean Implements IViewBaseEditCall.AllowUpdateTags
        Set(value As Boolean)
            Me.BTNsaveTagsBottom.Visible = value
            Me.BTNsaveTagsTop.Visible = value
        End Set
    End Property

    Private Sub SaveTags()
        Dim Tags As New List(Of lm.Comol.Modules.CallForPapers.Advanced.dto.dtoTag)()

        For Each sectrow As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()

            Dim oRepeater As Repeater = sectrow.FindControl("RPTfields")

            For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()

                Dim tag As New lm.Comol.Modules.CallForPapers.Advanced.dto.dtoTag()

                Dim oLiteral As Literal = row.FindControl("LTidField")
                tag.FieldId = CLng(oLiteral.Text)

                Dim oControl As UC_EditField = row.FindControl("CTRLeditField")
                tag.Tags = oControl.GetField.Tags

                TagsAdd(tag.Tags)

                Tags.Add(tag)
            Next
        Next

        Me.CurrentPresenter.UpdateTags(TagCurrent, Tags)

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSaveTag,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "CallFields")
    End Sub

    Private Sub BTNsaveTagsBottom_Click(sender As Object, e As EventArgs) Handles BTNsaveTagsBottom.Click
        SaveTags()
    End Sub

    Private Sub BTNsaveTagsTop_Click(sender As Object, e As EventArgs) Handles BTNsaveTagsTop.Click
        SaveTags()
    End Sub
End Class
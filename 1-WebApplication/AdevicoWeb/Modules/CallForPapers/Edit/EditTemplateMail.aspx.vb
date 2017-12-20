Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Core.Mail

Public Class EditTemplateMail
    Inherits PageBase
    Implements IViewEditSubmittersMail

#Region "Context"
    Private _Presenter As EditSubmittersMailPresenter
    Private ReadOnly Property CurrentPresenter() As EditSubmittersMailPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditSubmittersMailPresenter(Me.PageUtility.CurrentContext, Me)
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
    Private Property AllowAdd As Boolean Implements IViewEditSubmittersMail.AllowAdd
        Get
            Return ViewStateOrDefault("AllowAdd", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowAdd") = value
            Me.BTNaddTemplate.Visible = value
        End Set
    End Property
    Private Property Availablesubmitters As List(Of dtoSubmitterType) Implements IViewEditSubmittersMail.Availablesubmitters
        Get
            Return ViewStateOrDefault("Availablesubmitters", New List(Of dtoSubmitterType))
        End Get
        Set(value As List(Of dtoSubmitterType))
            Me.ViewState("Availablesubmitters") = value
        End Set
    End Property
    Private Property InEditing As List(Of Long) Implements IViewEditSubmittersMail.InEditing
        Get
            Return ViewStateOrDefault("InEditing", New List(Of Long))
        End Get
        Set(value As List(Of Long))
            Me.ViewState("InEditing") = value
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
        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditView,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

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
            .setButton(BTNsaveTemplatesBottom, True, , , True)
            .setButton(BTNsaveTemplatesTop, True, , , True)
            .setButton(BTNaddNewTemplate, True, , , True)
            .setButton(BTNaddTemplate, True, , , True)
            .setButton(BTNundo, True, , , True)
            .setLabel(LBtemplateSubmitters_t)
            .setLabel(LBtemplateName_t)
            .setLabel(LBtemplateAddTitle)
            .setButton(BTNpreviewMailFromTemplate, True, , , True)

            .setButton(BTNcloseMailMessageWindow, True, , , True)
            DVpreview.Attributes.Add("title", Resource.getValue("MailMessagePreview.Title"))
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditSubmittersMail.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditSubmittersMail.SendUserAction
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
        Dim title As String = Me.Resource.getValue("serviceTitle." & action.ToString & "." & CallType.ToString())
        Master.ServiceTitle = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = String.Format(title, itemName)
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & action.ToString() & "." & CallType.ToString())
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
    Private Sub HideErrorMessages() Implements IViewEditSubmittersMail.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(errors As EditorErrors) Implements IViewEditSubmittersMail.DisplayError
        CTRLmessages.Visible = (errors <> EditorErrors.None)
        CTRLmessages.InitializeControl(Resource.getValue("EditorErrors." & errors.ToString), Helpers.MessageType.error)
    End Sub
    Private Sub DisplayNoTemplates() Implements IViewEditSubmittersMail.DisplayNoTemplates
        Me.BTNsaveTemplatesBottom.Visible = False
        Me.BTNsaveTemplatesTop.Visible = False
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoTemplates"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplaySettingsSaved(missing As Boolean) Implements IViewEditSubmittersMail.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplatesSettingsSaved.missing." & missing.ToString), IIf(missing, Helpers.MessageType.alert, Helpers.MessageType.success))
    End Sub
    Private Sub DisplayTemplateAdded(missing As Boolean) Implements IViewEditSubmittersMail.DisplayTemplateAdded
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateAdded.missing." & missing.ToString), IIf(missing, Helpers.MessageType.alert, Helpers.MessageType.success))
    End Sub
    Private Sub DisplayTemplateMissing() Implements IViewEditSubmittersMail.DisplayTemplateMissing
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateMissing"), Helpers.MessageType.alert)
    End Sub
    Private Sub DisplayTemplateRemoved() Implements IViewEditSubmittersMail.DisplayTemplateRemoved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayTemplateRemoved"), Helpers.MessageType.success)
    End Sub

    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewEditSubmittersMail.LoadSubmitterTypes
        Me.Availablesubmitters = submitters
    End Sub
    Private Sub LoadTemplates(templates As List(Of dtoSubmitterTemplateMail)) Implements IViewEditSubmittersMail.LoadTemplates
        LItemplateAdd.Visible = False
        Me.RPTtemplates.DataSource = templates
        Me.RPTtemplates.DataBind()
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTtemplates.Visible = (templates.Count > 0)
        Me.UpdateSelectors()
        Me.BTNsaveTemplatesBottom.Visible = (templates.Count > 0) AndAlso AllowSave
        Me.BTNsaveTemplatesTop.Visible = (templates.Count > 0) AndAlso AllowSave
    End Sub
    Public Function GetTemplates() As List(Of dtoSubmitterTemplateMail) Implements IViewEditSubmittersMail.GetTemplates
        Dim templates As New List(Of dtoSubmitterTemplateMail)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTtemplates.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
            Dim oLiteral As Literal = row.FindControl("LTidTemplate")
            templates.Add(GetFullTemplate(CLng(oLiteral.Text), row))
        Next
        Return templates
    End Function

    Public Sub AddNewTemplate(number As Integer) Implements lm.Comol.Modules.CallForPapers.Presentation.IViewEditSubmittersMail.AddNewTemplate
        LItemplateAdd.Visible = True
        Me.TXBtemplateName.Text = String.Format(Resource.getValue("NewTemplateMail"), number.ToString)
        SLBsubmitters.DataSource = Availablesubmitters
        SLBsubmitters.DataTextField = "Name"
        SLBsubmitters.DataValueField = "Id"
        SLBsubmitters.DataBind()
        SLBsubmitters.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
        SLBsubmitters.Items.Insert(0, "")
        SLBsubmitters.Disabled = Not AllowSave
        If AllowSave Then
            If number = 1 Then
                For Each item As ListItem In SLBsubmitters.Items
                    item.Selected = True
                Next
            Else
                Dim items As List(Of Long) = GetSelectedSubmittersType()
                For Each item As ListItem In (From o As ListItem In SLBsubmitters.Items Where Not String.IsNullOrEmpty(o.Value) AndAlso items.Contains(CInt(o.Value)) Select o).ToList
                    item.Attributes.Add("disabled", "")
                Next
            End If
        End If

        Dim attributes As New List(Of TranslatedItem(Of String))
        attributes = (From e In TemplatePlaceHolders.GetPlaceHoldersType(False) Where e <> PlaceHoldersType.None Select New TranslatedItem(Of String) With {.Id = e.ToString, .Translation = Me.Resource.getValue(CallType.ToString & ".PlaceHoldersType." & e.ToString)}).ToList()

        attributes = attributes.OrderBy(Function(t) t.Translation).ToList()
        Dim dto As New dtoMailContent
        With dto
            dto.Subject = Resource.getValue(CallType.ToString & ".MailSubmitter.Subject")
            dto.Body = Resource.getValue(CallType.ToString & ".MailSubmitter.Standard").Replace("<br>", vbCrLf)
            dto.Settings = New lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings
            dto.Settings.IsBodyHtml = True
            dto.Settings.SenderType = lm.Comol.Core.MailCommons.Domain.SenderUserType.System
            dto.Settings.PrefixType = lm.Comol.Core.MailCommons.Domain.SubjectPrefixType.SystemConfiguration
        End With
        Me.CTRLtemplate.InitializeControl(dto, True, True, attributes, False)
        Me.BTNaddTemplate.Visible = False
    End Sub

    Public Sub UpdateSelectors()
        Dim items As New Dictionary(Of Long, List(Of Long))
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTtemplates.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
            Dim options As New List(Of Long)
            Dim oLiteral As Literal = row.FindControl("LTidTemplate")
            Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
            For Each item As ListItem In oSelect.Items
                If item.Selected And Not String.IsNullOrEmpty(item.Value) Then
                    options.Add(CLng(item.Value))
                End If
            Next
            items.Add(CLng(oLiteral.Text), options)
        Next
        Dim selectedItems As List(Of Long) = GetSelectedSubmittersType()
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTtemplates.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
            Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
            Dim oLiteral As Literal = row.FindControl("LTidTemplate")
            For Each item As ListItem In (From o As ListItem In oSelect.Items Where Not String.IsNullOrEmpty(o.Value) AndAlso _
                                          selectedItems.Contains(CInt(o.Value)) AndAlso Not items(CLng(oLiteral.Text)).Contains(CInt(o.Value)) Select o).ToList
                item.Attributes.Add("disabled", "")
            Next
        Next
    End Sub
    Private Function GetSelectedSubmittersType() As List(Of Long)
        Dim items As New List(Of Long)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTtemplates.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList
            Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
            For Each item As ListItem In oSelect.Items
                If item.Selected AndAlso Not String.IsNullOrEmpty(item.Value) Then
                    items.Add(CLng(item.Value))
                End If
            Next
        Next
        Return items
    End Function
    Private Sub DisplayMessagePreview(mailContent As lm.Comol.Core.Mail.dtoMailMessagePreview, recipient As String) Implements IViewEditSubmittersMail.DisplayMessagePreview
        Me.DVpreview.Visible = True
        Me.CTRLmailpreview.InitializeControlForPreview(mailContent, recipient)
    End Sub
#End Region

    Private Sub BTNaddTemplate_Click(sender As Object, e As System.EventArgs) Handles BTNaddTemplate.Click
        Me.CurrentPresenter.AddTemplate()
    End Sub
    Private Sub BTNsaveTemplatesBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveTemplatesBottom.Click, BTNsaveTemplatesTop.Click
        If HasAddingTemplate() Then
            Me.CurrentPresenter.SaveSettings(GetAddingTemplate(), GetTemplates())
        Else
            Me.CurrentPresenter.SaveSettings(GetTemplates())
        End If

        CallTrapHelper.SendTrap(
                    lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
                    Me.IdCall,
                    lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
                    "UsersNotifications")
    End Sub

    Private Sub BTNaddNewTemplate_Click(sender As Object, e As System.EventArgs) Handles BTNaddNewTemplate.Click
        If Me.CurrentPresenter.AddTemplate(GetAddingTemplate(), GetTemplates()) Then
            LItemplateAdd.Visible = False
            BTNaddTemplate.Visible = AllowSave
        End If
    End Sub

    Private Sub BTNundo_Click(sender As Object, e As System.EventArgs) Handles BTNundo.Click
        LItemplateAdd.Visible = False
        BTNaddTemplate.Visible = AllowSave
    End Sub

    Private Sub RPTtemplates_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtemplates.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoSubmitterTemplateMail = DirectCast(e.Item.DataItem, dtoSubmitterTemplateMail)

            Dim oLiteral As Literal = e.Item.FindControl("LTidTemplate")
            oLiteral.Text = dto.Id

            Dim templateRow As HtmlGenericControl = e.Item.FindControl("LItemplateRow")
            Dim oButton As Button


            Dim oControl As HtmlControl = e.Item.FindControl("DVinEditingTitle")
            oControl.Visible = InEditing.Contains(dto.Id)
            oControl = e.Item.FindControl("DVinEditingHeader")
            oControl.Visible = InEditing.Contains(dto.Id)
            oControl = e.Item.FindControl("DVreadOnlyHeader")
            oControl.Visible = Not InEditing.Contains(dto.Id)

            If InEditing.Contains(dto.Id) Then
                templateRow.Attributes.Add("class", "template edit")

                Dim oGenericControl As HtmlGenericControl = e.Item.FindControl("HRediting")
                oGenericControl.Visible = True
                oGenericControl = e.Item.FindControl("DVediting")
                oGenericControl.Visible = True

                Dim mailContent As UC_MailEditor = e.Item.FindControl("CTRLtemplate")
                mailContent.Visible = True
                Dim attributes As New List(Of TranslatedItem(Of String))
                attributes = (From p In TemplatePlaceHolders.GetPlaceHoldersType(False) Where p <> PlaceHoldersType.None Select New TranslatedItem(Of String) With {.Id = p.ToString, .Translation = Me.Resource.getValue(CallType.ToString & ".PlaceHoldersType." & p.ToString)}).ToList()

                attributes = attributes.OrderBy(Function(t) t.Translation).ToList()
                Dim dtoContent As New dtoMailContent
                With dtoContent
                    .Subject = dto.Subject
                    .Body = dto.Body
                    .Settings = dto.MailSettings
                End With
                mailContent.InitializeControl(dtoContent, True, True, attributes, False)

                oButton = e.Item.FindControl("BTNundo")
                oButton.CommandArgument = dto.Id
                Me.Resource.setButton(oButton, True, , , True)

                oButton = e.Item.FindControl("BTNsaveTemplate")
                oButton.CommandArgument = dto.Id
                oButton.Visible = AllowSave
                Me.Resource.setButton(oButton, True, , , True)

                oButton = e.Item.FindControl("BTNpreviewMailFromTemplate")
                oButton.CommandArgument = dto.Id
                Me.Resource.setButton(oButton, True, , , True)

            Else
                templateRow.Attributes.Add("class", "template")

                oButton = e.Item.FindControl("BTNeditTemplate")
                oButton.CommandArgument = dto.Id
                oButton.Visible = AllowSave
                Me.Resource.setButton(oButton, True, , , True)

                oButton = e.Item.FindControl("BTNdeleteTemplate")
                oButton.CommandArgument = dto.Id
                oButton.Visible = AllowSave
                Me.Resource.setButton(oButton, True, , , True)
            End If

            Dim oLabel As Label = e.Item.FindControl("LBtemplateEditTitle")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtemplateName_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBtemplateNameHeader_t")
            Resource.setLabel(oLabel)

            Dim oText As TextBox = e.Item.FindControl("TXBtemplateName")
            oText.ReadOnly = Not AllowSave
            oText.Text = dto.Name

            oText = e.Item.FindControl("TXBtemplateName")
            oText.Text = dto.Name
            oText.ReadOnly = Not AllowSave

            oLabel = e.Item.FindControl("LBattachmentSubmitters_t")
            Me.Resource.setLabel(oLabel)

            Dim oSelect As HtmlSelect = e.Item.FindControl("SLBsubmitters")
            oSelect.DataSource = Availablesubmitters
            oSelect.DataTextField = "Name"
            oSelect.DataValueField = "Id"
            oSelect.DataBind()
            oSelect.Items.Insert(0, "")
            For Each idSubmitter As Long In dto.SubmitterAssignments
                Dim oListItem As ListItem = oSelect.Items.FindByValue(idSubmitter)
                If Not IsNothing(oListItem) Then
                    oListItem.Selected = True
                End If
            Next

            oSelect.Attributes.Add("data-placeholder", Resource.getValue("Submitters.data-placeholder"))
            oSelect.Disabled = Not AllowSave
            Dim oHtmlControl As HtmlGenericControl = e.Item.FindControl("SPNsubmittersSelectAll")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmittersSelectAll.ToolTip"))
            oHtmlControl.Visible = AllowSave
            oHtmlControl = e.Item.FindControl("SPNsubmittersSelectNone")
            oHtmlControl.Attributes.Add("title", Resource.getValue("SPNsubmittersSelectNone.ToolTip"))
            oHtmlControl.Visible = AllowSave
        End If
    End Sub
    Private Sub RPTtemplates_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTtemplates.ItemCommand
        Dim idTemplate As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idTemplate = CLng(e.CommandArgument)
        End If
        Select Case e.CommandName
            Case "edit"
                Me.CurrentPresenter.StartEditingTemplate(idTemplate, GetTemplates())
            Case "virtualdelete"
                Me.CurrentPresenter.RemoveTemplate(idTemplate, GetTemplates())
            Case "undoAdd"
                Me.CurrentPresenter.UndoEditingTemplate(idTemplate, GetTemplates())
            Case "savetemplate"
                Me.CurrentPresenter.SaveTemplate(GetFullTemplate(idTemplate, e.Item), GetTemplates())
            Case "preview"
                Dim sTranslations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String)
                For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations))
                    sTranslations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
                Next

                '            Me.CurrentPresenter.SaveCompleteSubmission(GetValues(), , baseFilePath, Me.Resource.getValue("SubmissionTranslations." & SubmissionTranslations.FilesSubmitted.ToString))
                'End Sub

                Me.CurrentPresenter.PreviewMessage(GetFullTemplate(idTemplate, e.Item), Resource.getValue("fakeName"), Resource.getValue("fakeSurname"), Resource.getValue("fakeTaxCode"), Resource.getValue("fakeMail"), PageUtility.CurrentSmtpConfig, PageUtility.ApplicationUrlBase, sTranslations)
        End Select
    End Sub

    Private Function GetFullTemplate(idTemplate As Long, row As RepeaterItem) As dtoSubmitterTemplateMail
        Dim dto As New dtoSubmitterTemplateMail
        With dto
            .Id = idTemplate
            .IdCallForPaper = idTemplate
            .IdLanguage = 1
            Dim oSelect As HtmlSelect = row.FindControl("SLBsubmitters")
            For Each item As ListItem In oSelect.Items
                If item.Selected AndAlso IsNumeric(item.Value) Then
                    .SubmitterAssignments.Add(CLng(item.Value))
                End If
            Next
            Dim oText As TextBox = row.FindControl("TXBtemplateName")

            If InEditing.Contains(idTemplate) Then
                oText = row.FindControl("TXBtemplateNameHeader")

                Dim mailContent As UC_MailEditor = row.FindControl("CTRLtemplate")
                Dim oContent As dtoMailContent = mailContent.Mail
                .MailSettings = oContent.Settings
                .Subject = oContent.Subject
                .Body = oContent.Body

            End If
            .Name = oText.Text
        End With
        Return dto
    End Function
    Private Function GetAddingTemplate() As dtoSubmitterTemplateMail
        Dim template As New dtoSubmitterTemplateMail
        With template
            .Id = 0
            .IdCallForPaper = IdCall
            .Name = Me.TXBtemplateName.Text
            .SubmitterAssignments = (From i As ListItem In SLBsubmitters.Items Where IsNumeric(i.Value) AndAlso i.Selected Select CLng(i.Value)).ToList
            .IdLanguage = 1

            Dim oContent As dtoMailContent = Me.CTRLtemplate.Mail
            .MailSettings = oContent.Settings
            .Subject = oContent.Subject
            .Body = oContent.Body
        End With
        Return template
    End Function
    Private Function HasAddingTemplate() As Boolean
        Dim iResponse As Boolean = LItemplateAdd.Visible
        If iResponse Then
            Dim oContent As dtoMailContent = Me.CTRLtemplate.Mail
            iResponse = Not String.IsNullOrEmpty(oContent.Body)
        End If
        Return iResponse
    End Function

#Region "Preview Template"
    Private Sub BTNpreviewMailFromTemplate_Click(sender As Object, e As System.EventArgs) Handles BTNpreviewMailFromTemplate.Click
        Me.DVpreview.Visible = True
    End Sub
    Private Sub BTNcloseMailMessageWindow_Click(sender As Object, e As System.EventArgs) Handles BTNcloseMailMessageWindow.Click
        Me.DVpreview.Visible = False
    End Sub
#End Region



  
End Class
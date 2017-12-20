Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditSubmittersType
   Inherits PageBase
    Implements IViewEditSubmittersType

#Region "Context"
    Private _Presenter As EditSubmittersPresenter
    Private ReadOnly Property CurrentPresenter() As EditSubmittersPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditSubmittersPresenter(Me.PageUtility.CurrentContext, Me)
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
            BTNsaveSubmittersTop.Visible = value
            BTNsaveSubmittersBottom.Visible = value
            BTNaddSubmitter.Visible = value


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
            .setButton(BTNsaveSubmittersBottom, True, , , True)
            .setButton(BTNsaveSubmittersTop, True, , , True)
            .setButton(BTNaddSubmitter, True, , , True)
            .setLabel(LBsubmittersType)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditSubmittersType.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditSubmittersType.SendUserAction
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

        dto.DestinationUrl = RootObject.EditCallSubmittersType(CallType, PreloadIdCall, idCommunity, PreloadView)

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
    Public Sub HideErrorMessages() Implements IViewEditSubmittersType.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayError(errors As SubmitterTypeError) Implements IViewEditSubmittersType.DisplayError
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("SubmitterTypeError." & errors.ToString), Helpers.MessageType.error)
    End Sub
    Public Sub DisplayErrors(errors As Dictionary(Of Long, SubmitterTypeError)) Implements IViewEditSubmittersType.DisplayErrors
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplaySubmitterTypeError"), Helpers.MessageType.error)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsubmittersType.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim idItem As Long = CLng(DirectCast(row.FindControl("LTidSubmitter"), Literal).Text)
            If errors.ContainsKey(idItem) Then
                Dim err As SubmitterTypeError = errors(idItem)
                If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(SubmitterTypeError.EmptyMaxNumber, err) OrElse lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(SubmitterTypeError.InvalidMaxNumber, err) Then
                    Dim oTextBox As TextBox = row.FindControl("TXBmaxSubmissions")
                    If Not oTextBox.CssClass.Contains("errorInput") Then
                        oTextBox.CssClass = " errorInput"
                    End If
                End If
                If lm.Comol.Core.DomainModel.PermissionHelper.CheckPermissionSoft(SubmitterTypeError.InvalidName, err) Then
                    Dim oTextBox As TextBox = row.FindControl("TXBsubmitterName")
                    If Not oTextBox.CssClass.Contains("errorInput") Then
                        oTextBox.CssClass = " errorInput"
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub DisplayNoSubmitters() Implements IViewEditSubmittersType.DisplayNoSubmitters
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoSubmitters"), Helpers.MessageType.alert)
    End Sub
    Public Sub DisplaySettingsSaved() Implements IViewEditSubmittersType.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplaySubmittersSettingsSaved"), Helpers.MessageType.success)
    End Sub
    Private Sub LoadSubmitters(items As List(Of dtoSubmitterTypePermission)) Implements IViewEditSubmittersType.LoadSubmitters
        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTsubmittersType.DataSource = items
        Me.RPTsubmittersType.DataBind()
        Me.RPTsubmittersType.Visible = True
    End Sub
#End Region

    Private Sub BTNsaveMessagesBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveSubmittersBottom.Click, BTNsaveSubmittersTop.Click
        Me.CurrentPresenter.SaveSettings(GetSubmitters())

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SubmitterType")

    End Sub

    Private Sub BTNaddSubmitter_Click(sender As Object, e As System.EventArgs) Handles BTNaddSubmitter.Click
        Me.CurrentPresenter.AddSubmitter(GetSubmitters())

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.CallEditSave,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SubmitterType")
    End Sub
    Private Sub RPTsubmittersType_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsubmittersType.ItemCommand
        Dim idSubmitter As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idSubmitter = CLng(e.CommandArgument)
        End If
        If e.CommandName = "delete" Then
            Me.CurrentPresenter.RemoveSubmitter(idSubmitter)
        End If
    End Sub

    Protected Function GetSubmitters() As List(Of dtoSubmitterType)
        Dim items As New List(Of dtoSubmitterType)

        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsubmittersType.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim submitter As New dtoSubmitterType()
            submitter.Id = CLng(DirectCast(row.FindControl("LTidSubmitter"), Literal).Text)
            submitter.Name = DirectCast(row.FindControl("TXBsubmitterName"), TextBox).Text
            submitter.Description = DirectCast(row.FindControl("TXBsubmitterDescription"), TextBox).Text
            submitter.AllowMultipleSubmissions = DirectCast(row.FindControl("CBXmultipleSubmissions"), HtmlInputCheckBox).Checked

            Dim oTextBox As TextBox = DirectCast(row.FindControl("TXBmaxSubmissions"), TextBox)
            If IsNumeric(oTextBox.Text) Then
                submitter.MaxMultipleSubmissions = CInt(oTextBox.Text)
            End If

            Dim hidden As HtmlInputHidden = row.FindControl("HDNdisplayOrder")
            If Not String.IsNullOrEmpty(hidden.Value) AndAlso IsNumeric(hidden.Value) Then
                submitter.DisplayOrder = CInt(hidden.Value)
            End If

            items.Add(submitter)
        Next
        Return items.OrderBy(Function(s) s.DisplayOrder).ToList
    End Function

    Private Sub RPTsubmittersType_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubmittersType.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoSubmitterTypePermission = DirectCast(e.Item.DataItem, dtoSubmitterTypePermission)


            With dto.Submitter
                Dim oTextBox As TextBox = e.Item.FindControl("TXBsubmitterName")
                oTextBox.Text = .Name
                oTextBox.Enabled = AllowSave AndAlso dto.AllowEdit

                oTextBox = e.Item.FindControl("TXBsubmitterDescription")
                oTextBox.Text = .Description
                oTextBox.Enabled = AllowSave AndAlso dto.AllowEdit

                Dim oLabel As Label = e.Item.FindControl("LBmultipleSubmissions")
                Dim oCheckBox As HtmlControls.HtmlInputCheckBox = e.Item.FindControl("CBXmultipleSubmissions")
                oTextBox = e.Item.FindControl("TXBmaxSubmissions")
                oTextBox.Text = .MaxMultipleSubmissions
                oCheckBox.Checked = .AllowMultipleSubmissions

                If CallType = CallForPaperType.RequestForMembership Then
                    oTextBox.Visible = False
                    oCheckBox.Visible = False


                    oLabel.Visible = False
                    oLabel = e.Item.FindControl("LBmaxSubmissions")
                    oLabel.Visible = False
                Else
                    Me.Resource.setLabel(oLabel)
                    oLabel = e.Item.FindControl("LBmaxSubmissions")
                    Me.Resource.setLabel(oLabel)

                    oTextBox.Enabled = AllowSave ' &&  Not ((Not dto.AllowEdit AndAlso dto.SubmissionCount = 0) OrElse Not oCheckBox.Checked)
                    oCheckBox.Disabled = Not AllowSave ' &&  Not (dto.AllowEdit AndAlso dto.SubmissionCount = 0)
                End If

                oLabel = e.Item.FindControl("LBmoveSubmitter")
                Me.Resource.setLabel(oLabel)
                oLabel.Visible = AllowSave


                oLabel = e.Item.FindControl("LBsubmitterName_t")
                Me.Resource.setLabel(oLabel)
                oLabel = e.Item.FindControl("LBsubmitterDescription_t")
                Me.Resource.setLabel(oLabel)

                Dim oButton As Button = e.Item.FindControl("BTNremoveSubmitter")
                oButton.CommandArgument = dto.Id
                oButton.Visible = AllowSave AndAlso dto.AllowVirtualDelete

                Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
                hidden.Value = .DisplayOrder

                Dim CTRL_PrintDraft As UC_PrintDraft = e.Item.FindControl("CTRL_PrintDraft")

                If Not IsNothing(CTRL_PrintDraft) Then
                    CTRL_PrintDraft.InitializeAsUserControl(Me) 'check this!
                    CTRL_PrintDraft.InitializeUC(Me.IdCall, .Id, 0, 0)
                End If

            End With
        End If
    End Sub
End Class
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class SubmitCall
    Inherits PageBase
    Implements IViewSubmitCall

#Region "Context"
    Private _Presenter As SubmitCallPresenter
    Private ReadOnly Property CurrentPresenter() As SubmitCallPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SubmitCallPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseSubmission.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewBaseSubmission.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseSubmission.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdOtherCommunity As Integer Implements IViewBaseSubmission.PreloadIdOtherCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idOtherCommunity")) Then
                Return CInt(Me.Request.QueryString("idOtherCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedIdSubmitter As Long Implements IViewBaseSubmission.PreloadedIdSubmitter
        Get
            If IsNumeric(Me.Request.QueryString("idType")) Then
                Return CInt(Me.Request.QueryString("idType"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadedUniqueID As Guid Implements IViewBaseSubmission.PreloadedUniqueID
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("UniqueID")) Then
                Return Guid.Empty
            Else
                Try
                    Return New Guid(Me.Request.QueryString("UniqueID"))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End If
        End Get
    End Property
    Private ReadOnly Property FromPublicList As Boolean Implements IViewSubmitCall.FromPublicList
        Get
            If String.IsNullOrEmpty(Me.Request.QueryString("FromPublicList")) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewBaseSubmission.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewBaseSubmission.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewBaseSubmission.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewBaseSubmission.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdSelectedSubmitterType As Long Implements IViewBaseSubmission.IdSelectedSubmitterType
        Get
            Return ViewStateOrDefault("IdSelectedSubmitterType", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSelectedSubmitterType") = value
        End Set
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewBaseSubmission.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property IdSubmission As Long Implements IViewBaseSubmission.IdSubmission
        Get
            Return ViewStateOrDefault("IdSubmission", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmission") = value
        End Set
    End Property
    Private ReadOnly Property PreloadedIdSubmission As Long Implements IViewBaseSubmission.PreloadedIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property IdRevision As Long Implements IViewBaseSubmission.IdRevision
        Get
            Return ViewStateOrDefault("IdRevision", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdRevision") = value
        End Set
    End Property
    Private Property AllowCompleteSubmission As Boolean Implements IViewBaseSubmitCall.AllowCompleteSubmission
        Get
            Return ViewStateOrDefault("AllowCompleteSubmission", False)
        End Get
        Set(value As Boolean)
            Me.BTNsubmitSubmissionBottom.Visible = value
            'Me.LBKsubmitSubmissionBottom.Visible = value

            Me.BTNsubmitSubmissionTop.Visible = value
            'Me.LBKsubmitSubmissionTop.Visible = value

            ViewState("AllowCompleteSubmission") = value
        End Set
    End Property
    Private Property AllowDeleteSubmission As Boolean Implements IViewBaseSubmitCall.AllowDeleteSubmission
        Get
            Return ViewStateOrDefault("AllowDeleteSubmission", False)
        End Get
        Set(value As Boolean)
            Me.BTNvirtualDeleteSubmission.Visible = value
            ViewState("AllowDeleteSubmission") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewBaseSubmitCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            Me.BTNsaveSubmissionBottom.Visible = value
            Me.BTNsaveSubmissionTop.Visible = value
            ViewState("AllowSave") = value
        End Set
    End Property
    Private WriteOnly Property AllowSubmitterSelection As Boolean Implements IViewBaseSubmitCall.AllowSubmitterSelection
        Set(value As Boolean)
            Me.FLDsubmitters.Visible = value
        End Set
    End Property
    Private Property InitSubmissionTime As DateTime Implements IViewBaseSubmitCall.InitSubmissionTime
        Get
            Return ViewStateOrDefault("InitSubmissionTime", DateTime.Now)
        End Get
        Set(ByVal value As DateTime)
            Me.ViewState("InitSubmissionTime") = value
        End Set
    End Property
    Private Property isAnonymousSubmission As Boolean Implements IViewBaseSubmission.isAnonymousSubmission
        Get
            Return ViewStateOrDefault("isAnonymousSubmission", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isAnonymousSubmission") = value
        End Set
    End Property
    Private Property TryToComplete As Boolean Implements IViewBaseSubmission.TryToComplete
        Get
            Return ViewStateOrDefault("TryToComplete", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("TryToComplete") = value
        End Set
    End Property
    Private Property HasMultipleSubmitters As Boolean Implements IViewBaseSubmitCall.HasMultipleSubmitters
        Get
            Return ViewStateOrDefault("HasMultipleSubmitters", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("HasMultipleSubmitters") = value
        End Set
    End Property
    Private Property CallRepository As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier Implements IViewBaseSubmitCall.CallRepository
        Get
            Return ViewStateOrDefault("CallRepository", New lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier() With {.Type = lm.Comol.Core.FileRepository.Domain.RepositoryType.Community})
        End Get
        Set(value As lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier)
            Me.ViewState("CallRepository") = value
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

    Private Property ContainerName As String
        Get
            Return ViewStateOrDefault("ContainerName", "")
        End Get
        Set(value As String)
            Me.ViewState("ContainerName") = value
        End Set
    End Property

    Private ClickDt As DateTime = DateTime.Now()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClickDt = DateTime.Now()
        Page.MaintainScrollPositionOnPostBack = True
        
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionBottom)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionTop)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitSubmissionBottom)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitSubmissionTop)
    End Sub

#Region "Inherits"


    Public Overrides Sub BindDati()
        If Not Page.IsPostBack() Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "BindDati")
        End If

        Me.CurrentPresenter.InitView(False)


    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("ServiceViewSubmissionNopermission"), Helpers.MessageType.error)

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "NoPermission")

    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CallSubmission", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setHyperLink(HYPlist, True, True)
            .setButton(BTNsaveSubmissionBottom, True, , , True)
            .setButton(BTNsaveSubmissionTop, True, , , True)
            .setButton(BTNsubmitSubmissionBottom, True, , , True)
            .setButton(BTNsubmitSubmissionTop, True, , , True)
            '.setLinkButton(LBKsubmitSubmissionTop, True, True, False, True)
            '.setLinkButton(LBKsubmitSubmissionBottom, True, True, False, True)

            .setButton(BTNvirtualDeleteSubmission, True, , True, True)
            .setButtonByValue(BTNstartCompile, CallType.ToString, True, , , True)
            .setLabel(LBcallDescriptionTitle)
            .setLiteral(LTsaveAndSubmitBottomExplanation)
            .setLiteral(LTsaveBottomExplanation)
            .setLabel(LBtimeValidity_t)
            .setLiteral(LTsubmittersSelectorMessage)
        End With
    End Sub
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewSubmitCall.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendStartSubmission(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewSubmitCall.SendStartSubmission
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBaseSubmission.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBaseSubmission.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.StartNewSubmission(CallForPaperType.CallForBids, PreloadIdCall, PreloadedIdSubmission, FromPublicList, PreloadView, PreloadIdOtherCommunity))
    End Sub
    Private Sub DisplaySessionTimeout(url As String) Implements IViewBaseSubmission.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = url

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileView,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub
    Private Sub LoadError(errorView As SubmissionErrorView, idCommunity As Integer, idCall As Long, view As CallStatusForSubmitters) Implements IViewBaseSubmitCall.LoadError
        Me.CTRLmessages.Visible = (errorView <> SubmissionErrorView.None)
        Me.CTRLmessages.InitializeControl(Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & errorView.ToString()), Helpers.MessageType.error)
    End Sub
    'Private Sub DisplaySubmissionTimeExpired() Implements IViewBaseSubmitCall.DisplaySubmissionTimeExpired
    '    Me.CTRLmessages.Visible = True
    '    Me.AllowSave = False
    '    Me.AllowCompleteSubmission = False
    '    Me.AllowDeleteSubmission = False
    '    Me.CTRLmessages.InitializeControl(Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & SubmissionErrorView.SubmissionTimeExpired.ToString), Helpers.MessageType.info)
    'End Sub
    Private Sub DisplaySubmissionTimeAfter(submitAfter As Date?) Implements IViewBaseSubmitCall.DisplaySubmissionTimeAfter
        MLVpreview.SetActiveView(VIWempty)

        Dim Key As String = "SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & SubmissionErrorView.SubmissionTimeExpired.ToString
        Dim message As String = Me.Resource.getValue(Key)
        If String.IsNullOrWhiteSpace(message) Then
            message = Key
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileSubmit,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            message)

        If submitAfter.HasValue AndAlso (message.Contains("{0}") AndAlso message.Contains("{1}")) Then
            message = String.Format(message, submitAfter.Value.ToString("dd/MM/yy"), submitAfter.Value.ToString("HH:mm"))
        Else
            message = Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & SubmissionErrorView.SubmissionTimeExpired.ToString & ".None")
        End If
        CTRLerrorMessages.InitializeControl(message, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplaySubmissionTimeBefore(submitBefore As Date) Implements IViewBaseSubmitCall.DisplaySubmissionTimeBefore
        MLVpreview.SetActiveView(VIWempty)

        Dim Key As String = "SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & SubmissionErrorView.SubmissionEarlyTime.ToString
        Dim message As String = Me.Resource.getValue(Key)
        If String.IsNullOrWhiteSpace(Key) Then
            message = Key
        End If

        If (message.Contains("{0}") AndAlso message.Contains("{1}")) Then
            message = String.Format(message, submitBefore.ToString("dd/MM/yy"), submitBefore.ToString("HH:mm"))
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileSubmit,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            message)


        CTRLerrorMessages.InitializeControl(message, Helpers.MessageType.alert)

    End Sub
    Public Sub DisplayCallUnavailableForPublic() Implements IViewBaseSubmitCall.DisplayCallUnavailableForPublic
        MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("DisplayCallUnavailableForPublic." & CallForPaperType.CallForBids.ToString()), Helpers.MessageType.alert)
    End Sub
    Public Sub DisplayCallUnavailable(status As CallForPaperStatus) Implements IViewBaseSubmitCall.DisplayCallUnavailable
        MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("DisplayCallUnavailable." & CallForPaperType.CallForBids.ToString() & ".CallForPaperStatus." & status.ToString()), Helpers.MessageType.alert)
    End Sub
    Private Sub InitializeView(skin As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext, displayProgress As Boolean) Implements IViewBaseSubmitCall.InitializeView
        InitializeView(displayProgress)
    End Sub
    Private Sub InitializeView(displayProgress As Boolean) Implements IViewBaseSubmitCall.InitializeView
        Me.BTNsaveSubmissionBottom.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        Me.BTNsaveSubmissionTop.OnClientClick = IIf(displayProgress, "updateProgress();", "")


        Me.BTNsubmitSubmissionBottom.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        'Me.LBKsubmitSubmissionBottom.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        
        Me.BTNsubmitSubmissionTop.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        'Me.LBKsubmitSubmissionTop.OnClientClick = IIf(displayProgress, "updateProgress();", "")

        Me.CTRL_SavePrintDraft.PrintButton.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        
        If displayProgress Then
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionBottom)
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionTop)
            'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.LBKsubmitSubmissionBottom)
            'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.LBKsubmitSubmissionTop)

            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.CTRL_SavePrintDraft.PrintButton)
        End If
    End Sub
    Private Sub InitializeEmptyView() Implements IViewBaseSubmitCall.InitializeEmptyView
        '  Master.BindSkin()
    End Sub
    Private Sub SetContainerName(name As String, itemName As String) Implements IViewBaseSubmission.SetContainerName
        Dim translation As String = Me.Resource.getValue("serviceTitle.Submit." & CallType.ToString())
        Dim title As String = ""
        If String.IsNullOrEmpty(itemName) Then
            title = String.Format(translation, "", "")
        Else
            title = String.Format(translation, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        End If
        If Not String.IsNullOrEmpty(itemName) Then
            Me.Master.ServiceTitleToolTip = String.Format(translation, itemName)

        ElseIf Not String.IsNullOrEmpty(name) Then
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community.Submit." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Me.Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
        Me.Master.ServiceTitle = title

        ContainerName = title
    End Sub
    Private Sub SetSubmitterName(submitterName As String) Implements IViewBaseSubmitCall.SetSubmitterName
        Me.Master.ServiceTitle = String.Format(Resource.getValue("SubmitAs." & CallType.ToString), submitterName)
        Me.Master.ServiceTitleToolTip = String.Format(Resource.getValue("SubmitAs." & CallType.ToString), submitterName)
    End Sub
    Private Sub SetGenericSubmitterName() Implements IViewBaseSubmitCall.SetGenericSubmitterName
        Me.Master.ServiceTitle = Resource.getValue("SubmitAs." & CallType.ToString & ".Generic")
        Me.Master.ServiceTitleToolTip = Resource.getValue("SubmitAs." & CallType.ToString & ".Generic")
    End Sub
    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewBaseSubmission.LoadUnknowCall
        Me.MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("LoadUnknowCall." & type.ToString), Helpers.MessageType.alert)
    End Sub
    Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewBaseSubmission.LoadAttachments
        Me.RPTattachments.Visible = (items.Count > 0)
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
    End Sub
    Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewSubmitCall.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWcall)
        Me.DVbottomBox.Visible = False
        Me.BTNvirtualDeleteSubmission.Visible = False
        Me.BTNsubmitSubmissionTop.Visible = False
        'Me.LBKsubmitSubmissionTop.Visible = False


        Me.BTNsaveSubmissionTop.Visible = False
        Me.LTcallDescription.Text = baseCall.Description

        Me.LTsubmitterTypesTitle.Text = Resource.getValue("LTsubmitterTypesTitle." & baseCall.Type.ToString() & ".text")

        Me.Resource.setLabel(LBstartDate)
        Me.LBstartDate.Text = String.Format(Me.LBstartDate.Text, baseCall.StartDate.ToString("dd/MM/yy"), baseCall.StartDate.ToString("HH:mm"))

        If baseCall.EndDate.HasValue Then
            Me.Resource.setLabel(LBendDate)
            Me.LBendDate.Text = String.Format(Me.LBendDate.Text, baseCall.EndDate.Value.ToString("dd/MM/yy"), baseCall.EndDate.Value.ToString("HH:mm"))
        Else
            Me.LBendDate.Text = Resource.getValue("NoExpirationDate")
        End If
        Me.LBwinnerinfo.Text = baseCall.AwardDate
        Me.LBwinnerinfo.Visible = Not String.IsNullOrEmpty(baseCall.AwardDate)

        Dim itemName As String = baseCall.Name
        Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub

    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewBaseSubmitCall.LoadSections
        Me.DVstartSubmission.Visible = False
        Me.DVbottomBox.Visible = (sections.Count > 0) AndAlso (AllowCompleteSubmission OrElse AllowSave)
        'Me.BTNvirtualDeleteSubmission.Visible = (sections.Count > 0)
        'Me.BTNsubmitSubmissionTop.Visible = (sections.Count > 0)
        'Me.BTNsaveSubmissionTop.Visible = (sections.Count > 0)
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewBaseSubmission.LoadSubmitterTypes
        Me.RBLsubmitters.Items.Clear()
        For Each submitter As dtoSubmitterType In submitters
            Me.RBLsubmitters.Items.Add(New ListItem(String.Format("<dl><dt>{0}</dt><dd>{1}</dd></dl>", submitter.Name, submitter.Description), submitter.Id))
        Next
        If (submitters.Count = 1) Then
            Me.RBLsubmitters.SelectedIndex = 0
            Me.IdSelectedSubmitterType = Me.RBLsubmitters.SelectedValue
        Else
            Me.RBLsubmitters.SelectedIndex = -1
            Me.IdSelectedSubmitterType = 0
        End If
    End Sub
    Private Sub LoadRequiredFiles(files As List(Of dtoCallSubmissionFile)) Implements IViewSubmitCall.LoadRequiredFiles
        Me.RPTrequiredFiles.Visible = (files.Count > 0)
        Me.RPTrequiredFiles.DataSource = files
        Me.RPTrequiredFiles.DataBind()
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseSubmission.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.NavigateUrl = BaseUrl & url
                Me.HYPlist.Visible = True
        End Select
    End Sub
    Private Sub GoToUrl(action As CallStandardAction, url As String) Implements IViewBaseSubmission.GoToUrl
        Select Case action
            Case CallStandardAction.List
                PageUtility.RedirectToUrl(url)
            Case Else
                PageUtility.RedirectToUrl(url)
        End Select
    End Sub
    Private Sub GoToUrl(url As String) Implements IViewBaseSubmission.GoToUrl
        PageUtility.RedirectToUrl(url)
    End Sub
    Private Function GetValues() As List(Of dtoSubmissionValueField) Implements IViewBaseSubmission.GetValues
        Dim fields As New List(Of dtoSubmissionValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            fields.AddRange(GetFields(oRepeater))
        Next
        Return fields
    End Function
    Private Function GetFields(oRepeater As Repeater) As List(Of dtoSubmissionValueField)
        Dim fields As New List(Of dtoSubmissionValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoSubmissionValueField
            Dim oControl As UC_InputField = row.FindControl("CTRLinputField")
            field = oControl.GetField
            fields.Add(field)
        Next
        Return fields
    End Function
    Private Function GetRequiredSubmittedFiles(submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As List(Of dtoRequestedFileUpload) Implements IViewSubmitCall.GetRequiredSubmittedFiles
        Dim oList As IList(Of dtoRequestedFileUpload) = New List(Of dtoRequestedFileUpload)
        For Each RowSection As RepeaterItem In RPTrequiredFiles.Items
            Dim oControl As UC_InputRequiredFile = RowSection.FindControl("CTRLrequiredFile")
            If oControl.ToUpload Then
                Dim oDto As dtoRequestedFileUpload = oControl.AddInternalFile(submission, moduleCode, idModule, moduleAction, objectType)
                If Not IsNothing(oDto) Then
                    oList.Add(oDto)
                End If
            End If
        Next
        Return oList
    End Function
    Private Function GetFileValues(submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As List(Of dtoSubmissionFileValueField) Implements IViewBaseSubmission.GetFileValues
        Dim fields As List(Of dtoSubmissionFileValueField) = New List(Of dtoSubmissionFileValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            fields.AddRange(GetFileFields(oRepeater, submission, moduleCode, idModule, moduleAction, objectType))
        Next
        Return fields
    End Function
    Private Function GetFileFields(oRepeater As Repeater, submission As UserSubmission, moduleCode As String, idModule As Integer, moduleAction As Integer, objectType As Integer) As List(Of dtoSubmissionFileValueField)
        Dim fields As New List(Of dtoSubmissionFileValueField)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim field As New dtoSubmissionFileValueField
            Dim oControl As UC_InputField = row.FindControl("CTRLinputField")
            If oControl.FieldType = FieldType.FileInput Then
                field = oControl.AddInternalFile(submission, moduleCode, idModule, moduleAction, objectType)
                If Not IsNothing(field) Then
                    fields.Add(field)
                End If
            End If
        Next
        Return fields
    End Function
    Private Sub DisableSubmitterTypesSelection() Implements IViewBaseSubmission.DisableSubmitterTypesSelection
        Me.DVstartSubmission.Visible = False
        Me.RBLsubmitters.Enabled = False
    End Sub
#End Region

    Private Sub BTNstartCompile_Click(sender As Object, e As System.EventArgs) Handles BTNstartCompile.Click

        Dim ClickDt As DateTime = DateTime.Now()

        Dim idSubmitter As Long = IdSelectedSubmitterType

        If Me.RBLsubmitters.SelectedIndex > -1 Then
            idSubmitter = CLng(RBLsubmitters.SelectedValue)
            IdSelectedSubmitterType = idSubmitter
        End If
        If idSubmitter > 0 Then
            Me.CurrentPresenter.SelectSubmitterType(IdCall, idSubmitter)

            CallTrapHelper.SendTrap(
               lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileStart,
               Me.IdSubmission,
               lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
               "StartCompile")

            Me.CurrentPresenter.SaveSubmission(GetValues(), ClickDt)
        Else
            'CallTrapHelper.SendTrap(
            '   lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileStart,
            '   Me.IdSubmission,
            '   lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            '   "")
        End If


    End Sub

#Region "Repeater binding"
    Private Sub RPTattachments_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTattachments.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoAttachmentFile = DirectCast(e.Item.DataItem, dtoAttachmentFile)
            Dim renderItem As UC_RepositoryRenderAction = e.Item.FindControl("CTRLdisplayItem")

            Dim initializer As New lm.Comol.Core.ModuleLinks.dtoObjectRenderInitializer
            initializer.RefreshContainerPage = False
            initializer.SaveObjectStatistics = True
            initializer.Link = dto.Link
            initializer.SetOnModalPageByItem = True
            initializer.SetPreviousPage = False
            renderItem.InitializeControl(initializer, lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction)
            'Dim renderFile As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplayFile")

            'Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

            '' DIMENSIONI IMMAGINI
            'initializer.IconSize = Helpers.IconSize.Small
            'renderFile.EnableAnchor = True
            'initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction
            'initializer.Link = dto.Link
            'renderFile.InsideOtherModule = True
            'Dim actions As List(Of dtoModuleActionControl)
            'actions = renderFile.InitializeRemoteControl(initializer, StandardActionType.Play)

            Dim oLabel As Label = e.Item.FindControl("LBattachmentDescription")
            oLabel.Text = dto.Description

            Dim oControl As HtmlGenericControl = e.Item.FindControl("DVdescription")
            oControl.Visible = Not String.IsNullOrEmpty(dto.Description)


        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTattachmentsTitle")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
    'Private Sub RPTsubmitterTypes_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsubmitterTypes.ItemCommand
    '    Dim idItem As Long = 0
    '    If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
    '        idItem = CLng(e.CommandArgument)
    '    End If
    '    IdSelectedSubmitterType = idItem
    '    Me.CurrentPresenter.SelectSubmitterType(IdCall, idItem)
    'End Sub
    'Private Sub RPTsubmitterTypes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubmitterTypes.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim dto As dtoSubmitterType = DirectCast(e.Item.DataItem, dtoSubmitterType)

    '        Dim oLink As LinkButton = e.Item.FindControl("LNBsubmitterType")
    '        oLink.CssClass = IIf(dto.Id = IdSelectedSubmitterType, "active", "")
    '    End If
    'End Sub
    Private Sub RPTsections_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound


        'Reimposto a zero le attuali sommatorie delle tabelle.
        CurrentReport = New lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal()


        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim section As dtoCallSection(Of dtoSubmissionValueField) = DirectCast(e.Item.DataItem, dtoCallSection(Of dtoSubmissionValueField))

            Dim oLiteral As Literal = e.Item.FindControl("LTsectionTitle")
            oLiteral.Text = section.Name
            oLiteral = e.Item.FindControl("LTsectionDescription")
            oLiteral.Text = section.Description.Replace(Environment.NewLine, "<br/>")
        End If
    End Sub
    Protected Sub RPTfields_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim field As dtoSubmissionValueField = DirectCast(e.Item.DataItem, dtoSubmissionValueField)

            Dim inputField As UC_InputField = e.Item.FindControl("CTRLinputField")


            If field.Field.Type = FieldType.FileInput Then
                inputField.PostBackTriggers = "BTNsaveSubmissionBottom,BTNsaveSubmissionTop,BTNsubmitSubmissionTop,BTNsubmitSubmissionBottom"
            End If

            If inputField.CurrentError <> FieldError.None OrElse field.FieldError <> FieldError.None Then
                inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False, field.FieldError)
            Else
                inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False)
            End If


            If field.Field.Type = FieldType.TableSummary Then
                'inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False)
                inputField.BindTableSummaryReport(Me.CurrentReport)

                'Reset dati
                Me.CurrentReport = New dtoSummaryTableReportTotal()
            End If


            Dim itemSummary As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportItem = inputField.CurrentSummaryItem
            If Not IsNothing(itemSummary) AndAlso Not String.IsNullOrEmpty(itemSummary.Name) Then
                CurrentReport.Tables.Add(itemSummary)
            End If

        End If
    End Sub
    Protected Sub RPTfields_ItemCreated(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim inputField As UC_InputField = e.Item.FindControl("CTRLinputField")

            AddHandler inputField.RemoveFile, AddressOf OnRemoveFieldFile
        End If
    End Sub
    Private Sub RPTrequiredFiles_ItemCreated(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemCreated
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oControl As UC_InputRequiredFile = e.Item.FindControl("CTRLrequiredFile")
            AddHandler oControl.RemoveFile, AddressOf OnRemoveFile
        End If
    End Sub
    Private Sub RPTrequiredFiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCallSubmissionFile = DirectCast(e.Item.DataItem, dtoCallSubmissionFile)

            Dim oControl As UC_InputRequiredFile = e.Item.FindControl("CTRLrequiredFile")
            oControl.InitializeControl(IdCall, 0, CallRepository, dto, Not AllowSave, False)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTrequiredFilesTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTrequiredFilesDescription")
            Resource.setLiteral(oLiteral)
        End If
    End Sub
    Protected Sub OnRemoveFile(ByVal idSubmitted As Long)
        Me.CurrentPresenter.RemoveSubmittedFile(idSubmitted, GetValues(), ClickDt)
    End Sub

    Protected Sub OnRemoveFieldFile(ByVal idSubmittedField As Long)
        Me.CurrentPresenter.RemoveFieldFile(idSubmittedField, GetValues(), ClickDt)
    End Sub
#End Region

    Private Sub BTNsaveSubmissionBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveSubmissionBottom.Click, BTNsaveSubmissionTop.Click
        Me.CTRLmessages.Visible = False
        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileSaveDraft,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "SaveDraft")

        Me.CurrentPresenter.SaveSubmission(GetValues(), ClickDt)



    End Sub

    Private Sub BTNvirtualDeleteSubmission_Click(sender As Object, e As System.EventArgs) Handles BTNvirtualDeleteSubmission.Click

        CallTrapHelper.SendTrap(
          lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileDelete,
          Me.IdSubmission,
          lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
          "VirtualDelete")

        Me.CurrentPresenter.VirtualDeleteSubmission(ClickDt)


    End Sub

    Private Sub SubmitCall_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True

        If (BTNsaveSubmissionTop.Visible) Then
            CTRL_PrintEmptyDraft.InitializeUC(Me.IdCall, Me.IdSelectedSubmitterType, 0, Me.IdSubmission)
            CTRL_PrintEmptyDraft.Visible = True

            CTRL_SavePrintDraft.InitializeUC(Me.IdCall, Me.IdSelectedSubmitterType, Me.IdRevision, Me.IdSubmission)
            CTRL_SavePrintDraft.Visible = True

            CTRL_PrintEmptyDraft.SetButtonText(UC_PrintDraft.ButtonTextType.empytdraft)
            CTRL_SavePrintDraft.SetButtonText(UC_PrintDraft.ButtonTextType.compiledDraft)
        Else
            CTRL_PrintEmptyDraft.Visible = False
            CTRL_SavePrintDraft.Visible = False
        End If

        'Me.BTNsaveSubmissionBottom.Focus()
        Me.Form.DefaultButton = Me.BTNsaveSubmissionBottom.UniqueID
    End Sub


    Private Sub CTRL_SavePrintDraft_PrePrintDraft() Handles CTRL_SavePrintDraft.PrePrintDraft
        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.SaveSubmission(GetValues(), ClickDt)

        CTRL_SavePrintDraft.InitializeUC(Me.IdCall, Me.IdSelectedSubmitterType, Me.IdRevision, Me.IdSubmission)
        CTRL_SavePrintDraft.Visible = True
    End Sub

    Public Sub SetTextForSignatures() Implements IViewSubmitCall.SetTextForSignatures
        With Resource
            '.setLinkButtonToValue(LBKsubmitSubmissionBottom, "signature", True, True, False, True)
            '.setLinkButtonToValue(LBKsubmitSubmissionTop, "signature", True, True, False, True)

            .setButtonByValue(BTNsubmitSubmissionBottom, "signature", True, , , True)
            .setButtonByValue(BTNsubmitSubmissionTop, "signature", True, , , True)

            LTsaveAndSubmitBottomExplanation.Text = .getValue("LTsaveAndSubmitBottomExplanation.signature.text")
        End With
    End Sub

    Private Sub LBKsubmitSubmissionBottom_Click(sender As Object, e As EventArgs) Handles BTNsubmitSubmissionBottom.Click, BTNsubmitSubmissionTop.Click
        'Handles LBKsubmitSubmissionBottom.Click, LBKsubmitSubmissionTop.Click
        Dim ClickTime As DateTime = ClickDt

        CallTrapHelper.SendTrap(
          lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileSubmit,
          Me.IdSubmission,
          lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
          "Submission.Start")

        Me.CTRLmessages.Visible = False
        Me.CurrentPresenter.SaveSubmission(GetValues(), ClickTime)

        Dim baseFilePath As String = ""
        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
            baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
        Else
            baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
        End If

        Dim sTranslations As New Dictionary(Of lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations, String)
        For Each name As String In [Enum].GetNames(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations))
            sTranslations.Add([Enum].Parse(GetType(lm.Comol.Modules.CallForPapers.Domain.SubmissionTranslations), name), Me.Resource.getValue("SubmissionTranslations." & name))
        Next

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileSubmit,
            Me.IdSubmission,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
            "Submission.Saved")

        Me.CurrentPresenter.SaveCompleteSubmission(GetValues(), _
                                                   PageUtility.CurrentSmtpConfig, PageUtility.ApplicationUrlBase, _
                                                   sTranslations, _
                                                   baseFilePath, _
                                                   Me.Resource.getValue("SubmissionTranslations." & SubmissionTranslations.FilesSubmitted.ToString), _
                                                   ClickTime)
    End Sub

    Public Sub SendToList() Implements IViewSubmitCall.SendToList
        'SE hai aperte più pagine con più tipi sottomittori in selezione,ma non puoi fare altre sottomissioni,
        'ti rimando alla lista.
        Response.Redirect(HYPlist.NavigateUrl)
    End Sub

#Region "manager Summary"
    Private Property CurrentReport As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal
        Get
            Return ViewStateOrDefault("CurrentSummaryReportData", New lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal())
        End Get
        Set(value As lm.Comol.Modules.CallForPapers.Domain.dtoSummaryTableReportTotal)
            ViewState("CurrentSummaryReportData") = value
        End Set
    End Property
#End Region

End Class
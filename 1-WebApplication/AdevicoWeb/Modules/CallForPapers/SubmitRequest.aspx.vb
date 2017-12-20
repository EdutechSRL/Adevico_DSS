Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class SubmitRequest
    Inherits PageBase
    Implements IViewSubmitRequest

#Region "Context"
    Private _Presenter As SubmitRequestPresenter
    Private ReadOnly Property CurrentPresenter() As SubmitRequestPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SubmitRequestPresenter(Me.PageUtility.CurrentContext, Me)
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
    Private ReadOnly Property FromPublicList As Boolean Implements IViewSubmitRequest.FromPublicList
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
            Return ViewStateOrDefault("CallType", CallForPaperType.RequestForMembership)
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
            Me.BTNsubmitRequest.Visible = value
            Me.BTNsubmitRequestTop.Visible = value
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
            Me.BTNsubmitRequest.Visible = value
            Me.BTNsubmitRequestTop.Visible = value
            ViewState("AllowSave") = value
        End Set
    End Property
    Public WriteOnly Property AllowSubmitterChange As Boolean Implements IViewSubmitRequest.AllowSubmitterChange
        Set(value As Boolean)
            Me.BTNgoToFirstStep.Visible = value
            Me.BTNgoToFirstStepTop.Visible = value
        End Set
    End Property
    Private WriteOnly Property AllowSubmitterSelection As Boolean Implements IViewBaseSubmitCall.AllowSubmitterSelection
        Set(value As Boolean)
            Me.FLDsubmittersSelector.Visible = value
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
        ClickDt = DateTime.Now
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionBottom)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsaveSubmissionTop)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitSubmissionBottom)
        'DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitSubmissionTop)
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView(False)
    End Sub
    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("ServiceViewSubmissionNopermission"), Helpers.MessageType.error)
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
            HYPbackTolist.Text = HYPlist.Text

            .setButton(BTNgoToFirstStep, True, , , True)
            .setButton(BTNgoToFirstStepTop, True, , , True)

            .setButton(BTNsubmitRequest, True, , , True)
            .setButton(BTNsubmitRequestTop, True, , , True)
            .setButton(BTNvirtualDeleteSubmission, True, , , True)
            .setButtonByValue(BTNstartCompile, CallType.ToString, True, , , True)
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
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewSubmitRequest.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendStartSubmission(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewSubmitRequest.SendStartSubmission
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBaseSubmission.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleRequestForMembership.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBaseSubmission.DisplaySessionTimeout
        DisplaySessionTimeout(RootObject.StartNewSubmission(CallForPaperType.RequestForMembership, PreloadIdCall, PreloadedIdSubmission, FromPublicList, PreloadView, PreloadIdOtherCommunity))
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
        webPost.Redirect(dto)
    End Sub
    Private Sub LoadError(errorView As SubmissionErrorView, idCommunity As Integer, idCall As Long, view As CallStatusForSubmitters) Implements IViewBaseSubmitCall.LoadError
        If MLVpreview.GetActiveView Is VIWcall Then
            Me.CTRLviewMessages.Visible = (errorView <> SubmissionErrorView.None)
            Me.CTRLviewMessages.InitializeControl(Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & errorView.ToString()), Helpers.MessageType.error)

        Else
            Me.CTRLstartMessages.Visible = (errorView <> SubmissionErrorView.None)
            Me.CTRLstartMessages.InitializeControl(Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & errorView.ToString()), Helpers.MessageType.error)
        End If
    End Sub
    'Private Sub DisplaySubmissionTimeExpired() Implements IViewBaseSubmitCall.DisplaySubmissionTimeExpired
    '    Me.AllowSave = False
    '    Me.AllowCompleteSubmission = False
    '    Me.AllowDeleteSubmission = False
    '    Me.CTRLstartMessages.Visible = True
    '    Me.CTRLstartMessages.InitializeControl(Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.CallForBids.ToString & "." & SubmissionErrorView.SubmissionTimeExpired.ToString), Helpers.MessageType.info)
    'End Sub
    Private Sub DisplaySubmissionTimeAfter(submitAfter As Date?) Implements IViewBaseSubmitCall.DisplaySubmissionTimeAfter
        MLVpreview.SetActiveView(VIWempty)
        Dim message As String = Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.RequestForMembership.ToString & "." & SubmissionErrorView.SubmissionTimeExpired.ToString)
        If submitAfter.HasValue AndAlso (message.Contains("{0}") AndAlso message.Contains("{1}")) Then
            message = String.Format(message, submitAfter.Value.ToString("dd/MM/yy"), submitAfter.Value.ToString("HH:mm"))
        Else
            message = Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.RequestForMembership.ToString & "." & SubmissionErrorView.SubmissionTimeExpired.ToString & ".None")
        End If
        CTRLerrorMessages.InitializeControl(message, Helpers.MessageType.alert)
    End Sub
    Private Sub DisplaySubmissionTimeBefore(submitBefore As Date) Implements IViewBaseSubmitCall.DisplaySubmissionTimeBefore
        MLVpreview.SetActiveView(VIWempty)
        Dim message As String = Me.Resource.getValue("SubmissionErrorView." & CallForPaperType.RequestForMembership.ToString & "." & SubmissionErrorView.SubmissionEarlyTime.ToString)
        If (message.Contains("{0}") AndAlso message.Contains("{1}")) Then
            message = String.Format(message, submitBefore.ToString("dd/MM/yy"), submitBefore.ToString("HH:mm"))
        End If
        CTRLerrorMessages.InitializeControl(message, Helpers.MessageType.alert)
    End Sub
    Public Sub DisplayCallUnavailableForPublic() Implements IViewBaseSubmitCall.DisplayCallUnavailableForPublic
        MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("DisplayCallUnavailableForPublic." & CallForPaperType.RequestForMembership.ToString()), Helpers.MessageType.alert)
    End Sub
    Public Sub DisplayCallUnavailable(status As CallForPaperStatus) Implements IViewBaseSubmitCall.DisplayCallUnavailable
        MLVpreview.SetActiveView(VIWempty)
        CTRLerrorMessages.InitializeControl(Resource.getValue("DisplayCallUnavailable." & CallForPaperType.RequestForMembership.ToString() & ".CallForPaperStatus." & status.ToString()), Helpers.MessageType.alert)
    End Sub
    Private Sub InitializeView(skin As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext, displayProgress As Boolean) Implements IViewBaseSubmitCall.InitializeView
        InitializeView(displayProgress)
    End Sub
    Private Sub InitializeView(displayProgress As Boolean) Implements IViewBaseSubmitCall.InitializeView
        Me.BTNsubmitRequest.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        Me.BTNsubmitRequestTop.OnClientClick = IIf(displayProgress, "updateProgress();", "")
        If displayProgress Then
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitRequestTop)
            DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.BTNsubmitRequest)
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
    Private Sub DisableSubmitterTypesSelection() Implements IViewBaseSubmission.DisableSubmitterTypesSelection
        Me.DVstartSubmission.Visible = False
        Me.RBLsubmitters.Enabled = False
    End Sub
    Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewBaseSubmission.LoadAttachments
        Me.RPTattachments.Visible = (items.Count > 0)
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
    End Sub
    Private Sub LoadCallInfo(baseCall As dtoRequest) Implements IViewSubmitRequest.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWstartMessage)
        Me.LTsubmitterTypesTitle.Text = Resource.getValue("LTsubmitterTypesTitle." & baseCall.Type.ToString() & ".text")

        Me.Resource.setLabel(LBstartDate)
        Me.LBstartDate.Text = String.Format(Me.LBstartDate.Text, baseCall.StartDate.ToString("dd/MM/yy"), baseCall.StartDate.ToString("HH:mm"))

        If baseCall.EndDate.HasValue Then
            Me.Resource.setLabel(LBendDate)
            Me.LBendDate.Text = String.Format(Me.LBendDate.Text, baseCall.EndDate.Value.ToString("dd/MM/yy"), baseCall.EndDate.Value.ToString("HH:mm"))
        Else
            Me.LBendDate.Text = Resource.getValue("NoExpirationDate")
        End If
        Me.LTstartMessage.Text = baseCall.StartMessage
        'Dim itemName As String = baseCall.Name
        'Me.LBcallDescriptionTitle.Text = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString()), IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        'Me.LBcallDescriptionTitle.ToolTip = String.Format(Resource.getValue("DescriptionTitle." & baseCall.Type.ToString() & ".ToolTip"), itemName)
    End Sub

    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewBaseSubmitCall.LoadSections
        Me.MLVpreview.SetActiveView(VIWcall)
        Me.DVstartSubmission.Visible = False
        Me.BTNvirtualDeleteSubmission.Visible = (sections.Count > 0 AndAlso IdSubmission > 0)
        Me.BTNsubmitRequest.Visible = (sections.Count > 0)
        Me.BTNsubmitRequestTop.Visible = (sections.Count > 0)
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
        Me.BTNstartCompile.Enabled = (submitters.Count > 0)
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseSubmission.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.NavigateUrl = BaseUrl & url
                Me.HYPlist.Visible = True
                Me.HYPbackTolist.NavigateUrl = BaseUrl & url
                Me.HYPbackTolist.Visible = True

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
#End Region

    Private Sub BTNstartCompile_Click(sender As Object, e As System.EventArgs) Handles BTNstartCompile.Click
        If Me.RBLsubmitters.SelectedIndex > -1 Then
            Me.CurrentPresenter.SelectSubmitterType(IdCall, CLng(RBLsubmitters.SelectedValue))
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
    Private Sub RPTsections_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim section As dtoCallSection(Of dtoSubmissionValueField) = DirectCast(e.Item.DataItem, dtoCallSection(Of dtoSubmissionValueField))

            Dim oLiteral As Literal = e.Item.FindControl("LTsectionTitle")
            oLiteral.Text = section.Name
            oLiteral = e.Item.FindControl("LTsectionDescription")
            oLiteral.Text = section.Description
        End If
    End Sub
    Protected Sub RPTfields_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim field As dtoSubmissionValueField = DirectCast(e.Item.DataItem, dtoSubmissionValueField)
            Dim inputField As UC_InputField = e.Item.FindControl("CTRLinputField")
            If field.Field.Type = FieldType.FileInput Then
                inputField.PostBackTriggers = "BTNsubmitRequest,BTNsubmitRequestTop"
            End If

            If inputField.CurrentError <> FieldError.None OrElse field.FieldError <> FieldError.None Then
                inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False, field.FieldError)
            Else
                inputField.InitializeControl(IdCall, IdSubmission, CallRepository, field, Not AllowSave, False)
            End If
        End If
    End Sub
    Protected Sub RPTfields_ItemCreated(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim inputField As UC_InputField = e.Item.FindControl("CTRLinputField")

            AddHandler inputField.RemoveFile, AddressOf OnRemoveFieldFile
        End If
    End Sub

    Protected Sub OnRemoveFieldFile(ByVal idSubmittedField As Long)
        Me.CurrentPresenter.RemoveFieldFile(idSubmittedField, GetValues(), ClickDt)
    End Sub
#End Region

    Private Sub BTNsubmitSubmissionBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsubmitRequest.Click, BTNsubmitRequestTop.Click
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

        Me.CurrentPresenter.SaveCompleteSubmission(GetValues(), PageUtility.CurrentSmtpConfig, PageUtility.ApplicationUrlBase, sTranslations, baseFilePath, Me.Resource.getValue("SubmissionTranslations." & SubmissionTranslations.FilesSubmitted.ToString), ClickDt)
    End Sub
    Private Sub BTNgoToFirstStep_Click(sender As Object, e As System.EventArgs) Handles BTNgoToFirstStep.Click, BTNgoToFirstStepTop.Click
        Me.MLVpreview.SetActiveView(VIWstartMessage)
        Me.FLDsubmittersSelector.Visible = (Me.RBLsubmitters.Items.Count > 1)
        Me.DVstartSubmission.Visible = True
    End Sub
    Private Sub BTNvirtualDeleteSubmission_Click(sender As Object, e As System.EventArgs) Handles BTNvirtualDeleteSubmission.Click
        Me.CurrentPresenter.VirtualDeleteSubmission(ClickDt)
    End Sub

    Private Sub SubmitRequest_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub

End Class
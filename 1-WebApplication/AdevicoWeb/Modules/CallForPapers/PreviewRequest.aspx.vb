Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Public Class PreviewRequest
    Inherits PageBase
    Implements IViewPreviewRequest

#Region "Context"
    Private _Presenter As RequestPreviewPresenter
    Private ReadOnly Property CurrentPresenter() As RequestPreviewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RequestPreviewPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewPreviewBaseForPaper.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewPreviewBaseForPaper.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewPreviewBaseForPaper.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property CallType As CallForPaperType Implements IViewPreviewBaseForPaper.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewPreviewBaseForPaper.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewPreviewBaseForPaper.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewPreviewBaseForPaper.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property IdSubmitterType As Long Implements IViewPreviewBaseForPaper.IdSubmitterType
        Get
            Return ViewStateOrDefault("IdSubmitterType", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdSubmitterType") = value
        End Set
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewPreviewBaseForPaper.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property HasMultipleSubmitters As Boolean Implements IViewPreviewBaseForPaper.HasMultipleSubmitters
        Get
            Return ViewStateOrDefault("HasMultipleSubmitters", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("HasMultipleSubmitters") = value
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        If Not Page.IsPostBack Then
            Dim dto As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext
            dto = CurrentPresenter.GetSkin()
            dto.ShowDocType = True
            Me.Master.InitializeMaster(dto)
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        MLVpreview.SetActiveView(VIWempty)
        Me.LBpreviewMessage.Text = "Preview.NoPermission"
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CallSubmission", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNgoToCompileStep, True, , , True)
            .setButton(BTNgoToFirstStep, True, , , True)
            .setButtonByValue(BTNstartCompile, CallForPaperType.RequestForMembership.ToString, True, , , True)
            .setButton(BTNsubmitRequest, True, , , True)
            .setLiteral(LTsubmittersSelectorMessage)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewPreviewRequest.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewPreviewBaseForPaper.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewPreviewBaseForPaper.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.NewWindow

        dto.DestinationUrl = RootObject.PreviewCall(CallType, PreloadIdCall, idCommunity, CallStatusForSubmitters.None)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
    Private Sub SetContainerName(name As String, itemName As String) Implements IViewPreviewBaseForPaper.SetContainerName
        Dim translation As String = Me.Resource.getValue("serviceTitle.Preview." & CallType.ToString())
        Dim title As String = ""
        If String.IsNullOrEmpty(itemName) Then
            title = String.Format(translation, "", "")
        Else
            title = String.Format(translation, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        End If

        If Not String.IsNullOrEmpty(itemName) Then
            Master.ServiceTitleToolTip = String.Format(translation, itemName)
        ElseIf Not String.IsNullOrEmpty(name) Then
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community.Preview." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
        Master.ServiceTitle = title
        ContainerName = title
    End Sub
    Private Sub SetSubmitterName(submitterName As String) Implements IViewPreviewBaseForPaper.SetSubmitterName
        Master.ServiceTitle = String.Format(Resource.getValue("SubmitAs." & CallType.ToString), submitterName)
        Master.ServiceTitleToolTip = String.Format(Resource.getValue("SubmitAs." & CallType.ToString), submitterName)
    End Sub
    Private Sub SetGenericSubmitterName() Implements IViewPreviewBaseForPaper.SetGenericSubmitterName
        Me.Master.ServiceTitle = Resource.getValue("SubmitAs." & CallType.ToString & ".Generic")
        Me.Master.ServiceTitleToolTip = Resource.getValue("SubmitAs." & CallType.ToString & ".Generic")
    End Sub
    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewPreviewBaseForPaper.LoadUnknowCall
        Me.MLVpreview.SetActiveView(VIWempty)
        Me.LBpreviewMessage.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Private Sub LoadAttachments(items As List(Of dtoAttachmentFile)) Implements IViewPreviewBaseForPaper.LoadAttachments
        Me.RPTattachments.Visible = (items.Count > 0)
        Me.RPTattachments.DataSource = items
        Me.RPTattachments.DataBind()
    End Sub
    Private Sub LoadCallInfo(baseCall As dtoRequest) Implements IViewPreviewRequest.LoadCallInfo
        Me.MLVpreview.SetActiveView(VIWstartMessage)
        Me.LTsubmitterTypesTitle.Text = Resource.getValue("LTsubmitterTypesTitle." & baseCall.Type.ToString() & ".text")
        Me.Resource.setLabel(LBstartDate)
        Me.LBstartDate.Text = String.Format(Me.LBstartDate.Text, baseCall.StartDate.ToString("dd/MM/yy"), baseCall.StartDate.ToString("HH:mm"))

        If baseCall.EndDate.HasValue Then
            Me.Resource.setLabel(LBendDate)
            Me.LBendDate.Text = String.Format(Me.LBendDate.Text, baseCall.EndDate.Value.ToString("dd/MM/yy"), baseCall.EndDate.Value.ToString("HH:mm"))
        ElseIf Now < baseCall.StartDate Then
            Me.LBendDate.Text = Resource.getValue("NoExpirationDate")
        Else
            Me.DVexpiration.Visible = False
        End If
        Me.LTstartMessage.Text = baseCall.StartMessage
        Me.LTendMessage.Text = baseCall.EndMessage

        Dim itemName As String = baseCall.Name
    End Sub

    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewPreviewBaseForPaper.LoadSections
        Me.MLVpreview.SetActiveView(VIWcall)
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewPreviewBaseForPaper.LoadSubmitterTypes
        Me.IdSubmitterType = 0
        Me.RBLsubmitters.Items.Clear()
        For Each submitter As dtoSubmitterType In submitters
            Me.RBLsubmitters.Items.Add(New ListItem(String.Format("<dl><dt>{0}</dt><dd>{1}</dd></dl>", submitter.Name, submitter.Description), submitter.Id))
        Next
        If (submitters.Count = 1) Then
            Me.RBLsubmitters.SelectedIndex = 0
        Else
            Me.RBLsubmitters.SelectedIndex = -1
        End If

        FLDsubmittersSelector.Visible = (submitters.Count > 1)
        BTNstartCompile.Enabled = Not (Me.RBLsubmitters.SelectedIndex = -1)
    End Sub

#End Region

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
            oControl.Visible = String.IsNullOrEmpty(dto.Description)

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
            inputField.InitializeControl(IdCall, 0, New lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier() With {.Type = lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal}, field, field.Field.Type = FieldType.FileInput, False)
        End If
    End Sub


    Private Sub RBLsubmitters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLsubmitters.SelectedIndexChanged
        Me.BTNstartCompile.Enabled = (Me.RBLsubmitters.SelectedIndex > -1)
    End Sub
    Private Sub BTNstartCompile_Click(sender As Object, e As System.EventArgs) Handles BTNstartCompile.Click
        If Me.RBLsubmitters.SelectedIndex > -1 Then
            Me.CurrentPresenter.LoadSections(IdCall, RBLsubmitters.SelectedValue)
        End If
    End Sub
    Private Sub BTNgoToFirstStep_Click(sender As Object, e As System.EventArgs) Handles BTNgoToFirstStep.Click
        Me.MLVpreview.SetActiveView(VIWstartMessage)
    End Sub
    Private Sub BTNgoToCompileStep_Click(sender As Object, e As System.EventArgs) Handles BTNgoToCompileStep.Click
        Me.MLVpreview.SetActiveView(VIWcall)
    End Sub

    Private Sub BTNsubmitRequest_Click(sender As Object, e As System.EventArgs) Handles BTNsubmitRequest.Click
        Me.MLVpreview.SetActiveView(VIWendMessage)
    End Sub

    Private Sub PreviewRequest_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
End Class
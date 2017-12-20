Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic
Imports lm.Comol.Modules.CallForPapers.Advanced.Domain
Imports Telerik.Web.UI

Public Class PreviewCall
    Inherits PageBase
    Implements IViewPreviewCall

#Region "Context"
    Private _Presenter As CallPreviewPresenter
    Private ReadOnly Property CurrentPresenter() As CallPreviewPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CallPreviewPresenter(Me.PageUtility.CurrentContext, Me)
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

    Private Sub PreviewCall_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
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
            .setHyperLink(HYPlist, True, True)
            .setButton(BTNsaveSubmissionBottom, True, , , True)
            .setButton(BTNsaveSubmissionTop, True, , , True)
            .setButton(BTNsubmitSubmissionBottom, True, , , True)
            .setButton(BTNsubmitSubmissionTop, True, , , True)
            .setButton(BTNvirtualDeleteSubmission, True, , , True)
            .setLabel(LBcallDescriptionTitle)
            .setLiteral(LTsaveAndSubmitBottomExplanation)
            .setLiteral(LTsaveBottomExplanation)
            .setLabel(LBtimeValidity_t)
            .setLabel(LBsubmitterSelector_t)
            .setLiteral(LTpreviewSubmittersMessage)
        End With
    End Sub


    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewPreviewCall.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
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
    Private Sub LoadCallInfo(baseCall As dtoCall) Implements IViewPreviewCall.LoadCallInfo

        'Tag
        BintTags(baseCall.Tags)


        Me.MLVpreview.SetActiveView(VIWcall)
        Me.DVbottomBox.Visible = False
        Me.BTNvirtualDeleteSubmission.Visible = False
        Me.BTNsubmitSubmissionTop.Visible = False
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

    Private Sub LoadSections(sections As List(Of dtoCallSection(Of dtoSubmissionValueField))) Implements IViewPreviewBaseForPaper.LoadSections


        Me.DVbottomBox.Visible = (sections.Count > 0)
        Me.BTNvirtualDeleteSubmission.Visible = (sections.Count > 0)
        Me.BTNsubmitSubmissionTop.Visible = (sections.Count > 0)
        Me.BTNsaveSubmissionTop.Visible = (sections.Count > 0)
        Me.RPTsections.DataSource = sections
        Me.RPTsections.DataBind()
    End Sub
    Private Sub LoadSubmitterTypes(submitters As List(Of dtoSubmitterType)) Implements IViewPreviewBaseForPaper.LoadSubmitterTypes
        Me.DDLsubmitters.DataSource = submitters
        Me.DDLsubmitters.DataTextField = "Name"
        Me.DDLsubmitters.DataValueField = "Id"
        Me.DDLsubmitters.DataBind()

        If submitters.Count = 1 Then
            Me.IdSubmitterType = submitters(0).Id
        ElseIf submitters.Count > 1 Then
            Me.IdSubmitterType = 0
            Me.DDLsubmitters.Items.Insert(0, New ListItem(Me.Resource.getValue("SubmitterSelect"), 0))
        Else
            Me.IdSubmitterType = 0
        End If
        DVsubmittersLoader.Visible = True
        FLDpartecipants.Visible = True
        Me.RBLsubmitters.Items.Clear()
        For Each submitter As dtoSubmitterType In submitters
            Me.RBLsubmitters.Items.Add(New ListItem(String.Format("<dl><dt>{0}</dt><dd>{1}</dd></dl>", submitter.Name, submitter.Description), submitter.Id))
        Next


    End Sub
    Private Sub LoadRequiredFiles(files As List(Of dtoCallSubmissionFile)) Implements IViewPreviewCall.LoadRequiredFiles
        Me.RPTrequiredFiles.Visible = (files.Count > 0)
        Me.RPTrequiredFiles.DataSource = files
        Me.RPTrequiredFiles.DataBind()
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
    'Private Sub RPTsubmitterTypes_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsubmitterTypes.ItemCommand
    '    Dim idItem As Long = 0
    '    If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
    '        idItem = CLng(e.CommandArgument)
    '    End If
    '    IdSubmitterType = idItem


    '    For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsubmitterTypes.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
    '        Dim oLink As LinkButton = row.FindControl("LNBsubmitterType")
    '        oLink.CssClass = IIf(oLink.CommandArgument = idItem.ToString, "active", "")
    '    Next
    'End Sub
    'Private Sub RPTsubmitterTypes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubmitterTypes.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim dto As dtoSubmitterType = DirectCast(e.Item.DataItem, dtoSubmitterType)

    '        Dim oLink As LinkButton = e.Item.FindControl("LNBsubmitterType")
    '        oLink.CssClass = IIf(dto.Id = IdSubmitterType, "active", "")
    '    End If
    'End Sub

    Private Sub RPTsections_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound
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

            inputField.TagHelper = Me.TagsHelper

            inputField.InitializeControl( _
                IdCall, _
                0, _
                New lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier() With { _
                    .Type = lm.Comol.Core.FileRepository.Domain.RepositoryType.Portal}, _
                field, _
                field.Field.Type = FieldType.FileInput, _
                False)
        End If
    End Sub

    Private Sub RPTrequiredFiles_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrequiredFiles.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoCallSubmissionFile = DirectCast(e.Item.DataItem, dtoCallSubmissionFile)

            Dim oControl As UC_InputRequiredFile = e.Item.FindControl("CTRLrequiredFile")
            'oControl.InitializeControl(IdCall, 0, 0, dto, False, False)
            oControl.InitializeControl(IdCall, 0, New lm.Comol.Core.FileRepository.Domain.RepositoryIdentifier() With {.Type = lm.Comol.Core.FileRepository.Domain.RepositoryType.Community}, dto, True, False)
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTrequiredFilesTitle")
            Resource.setLiteral(oLiteral)
            oLiteral = e.Item.FindControl("LTrequiredFilesDescription")
            Resource.setLiteral(oLiteral)
        End If
    End Sub

    Private Sub DDLsubmitters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLsubmitters.SelectedIndexChanged
        If Me.DDLsubmitters.SelectedValue < 1 Then
            RBLsubmitters.SelectedIndex = -1
        Else
            RBLsubmitters.SelectedValue = Me.DDLsubmitters.SelectedValue
        End If
        Me.CurrentPresenter.LoadSections(IdCall, Me.DDLsubmitters.SelectedValue)
    End Sub

    Private Sub RBLsubmitters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLsubmitters.SelectedIndexChanged
        Me.DDLsubmitters.SelectedValue = RBLsubmitters.SelectedValue
        Me.CurrentPresenter.LoadSections(IdCall, RBLsubmitters.SelectedValue)
    End Sub

#Region "Tags Management"


    Private Sub BintTags(ByVal Tags As String)

        Dim tagHelperTmp As New AdvTagHelper(Tags)

        'ToDo: continuare sostituendo TagHelper!
        'ToDo: il contenitore iniziale diventerà un UC nel cui viewstate verrà salvato AdvTagHelper
        
        If Not tagHelperTmp.HasValue Then
            FStagContainer.Visible = False
            Return
        End If

        FStagContainer.Visible = True

        TagsHelper = tagHelperTmp

        Me.RPTtag.DataSource = tagHelperTmp.GetKvpList()
        Me.RPTtag.DataBind()

    End Sub

    Private Property TagsHelper As AdvTagHelper
        Get

            Return ViewStateOrDefault("TagsKvps", New AdvTagHelper(""))
        End Get
        Set(value As AdvTagHelper)
            ViewState("TagsKvps") = value
        End Set
    End Property

    Private Sub RPTtag_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTtag.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Try
                Dim kvp As KeyValuePair(Of Integer, String) = e.Item.DataItem

                Dim LTtag As Literal = e.Item.FindControl("LTtag")

                LTtag.Text = String.Format(LTtag.Text, TagsHelper.GetTagCssSingleString(kvp.Value), kvp.Value)

            Catch ex As Exception

            End Try


        End If
    End Sub

#End Region



End Class
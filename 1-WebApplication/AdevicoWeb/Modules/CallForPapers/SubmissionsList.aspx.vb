Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq

Public Class SubmissionsList
    Inherits PageBase
    Implements IViewSubmissionsList

#Region "Context"
    Private _Presenter As SubmissionsListPresenter
    Private ReadOnly Property CurrentPresenter() As SubmissionsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SubmissionsListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewSubmissionsList.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property

    Private ReadOnly Property PreloadIdCall As Long Implements IViewSubmissionsList.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdSubmission As Long Implements IViewSubmissionsList.PreloadIdSubmission
        Get
            If IsNumeric(Me.Request.QueryString("idSubmission")) Then
                Return CLng(Me.Request.QueryString("idSubmission"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdRevision As Long Implements IViewSubmissionsList.PreloadIdRevision
        Get
            If IsNumeric(Me.Request.QueryString("idRevision")) Then
                Return CLng(Me.Request.QueryString("idRevision"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadAction As CallStandardAction Implements IViewSubmissionsList.PreloadAction
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStandardAction).GetByString(Request.QueryString("action"), CallStandardAction.List)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewSubmissionsList.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewSubmissionsList.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private ReadOnly Property PreloadCallType As CallForPaperType Implements IViewSubmissionsList.PreloadCallType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewSubmissionsList.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("PageIndex")) Then
                Return CInt(Me.Request.QueryString("PageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewSubmissionsList.PreloadPageSize
        Get
            If IsNumeric(Me.Request.QueryString("PageSize")) Then
                Return CInt(Me.Request.QueryString("PageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOrderBy As SubmissionsOrder Implements IViewSubmissionsList.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(Request.QueryString("OrderBy"), SubmissionsOrder.BySubmittedOn)
        End Get
    End Property
    Private ReadOnly Property PreloadFilterBy As SubmissionFilterStatus Implements IViewSubmissionsList.PreloadFilterBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionFilterStatus).GetByString(Request.QueryString("Filter"), SubmissionFilterStatus.OnlySubmitted)
        End Get
    End Property
    Private ReadOnly Property PreloadSearchForName As String Implements IViewSubmissionsList.PreloadSearchForName
        Get
            Return Request.QueryString("SearchForName")
        End Get
    End Property
    Private ReadOnly Property PreloadFilters As dtoSubmissionFilters Implements IViewSubmissionsList.PreloadFilters
        Get
            Dim dto As New dtoSubmissionFilters
            With dto
                .Ascending = PreloadAscending
                .CallType = PreloadCallType
                .OrderBy = PreloadOrderBy
                .SearchForName = PreloadSearchForName
                .Status = PreloadFilterBy

                Dim translations As New Dictionary(Of RevisionStatus, String)
                For Each name As String In [Enum].GetNames(GetType(RevisionStatus))
                    translations.Add([Enum].Parse(GetType(RevisionStatus), name), Me.Resource.getValue("RevisionStatus." & name))
                Next
                .TranslationsRevision = translations

                Dim types As New Dictionary(Of SubmissionStatus, String)
                For Each name As String In [Enum].GetNames(GetType(SubmissionStatus))
                    types.Add([Enum].Parse(GetType(SubmissionStatus), name), Me.Resource.getValue("SubmissionStatus." & name))
                Next
                .TranslationsSubmission = types
            End With
            Return dto
        End Get
    End Property

    Private Property AllowManage As Boolean Implements IViewSubmissionsList.AllowManage
        Get
            Return ViewStateOrDefault("AllowManage", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowManage") = value
        End Set
    End Property
    Private Property AllowView As Boolean Implements IViewSubmissionsList.AllowView
        Get
            Return ViewStateOrDefault("AllowView", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowView") = value
        End Set
    End Property
    Private Property CurrentAction As CallStandardAction Implements IViewSubmissionsList.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", CallStandardAction.List)
        End Get
        Set(ByVal value As CallStandardAction)
            Me.ViewState("CurrentAction") = value
        End Set
    End Property
    Private Property CurrentView As CallStatusForSubmitters Implements IViewSubmissionsList.CurrentView
        Get
            Return ViewStateOrDefault("CurrentView", CallStandardAction.Manage)
        End Get
        Set(ByVal value As CallStatusForSubmitters)
            Me.ViewState("CurrentView") = value
        End Set
    End Property
    Private Property IdCall As Long Implements IViewSubmissionsList.IdCall
        Get
            Return ViewStateOrDefault("IdCall", CLng(0))
        End Get
        Set(ByVal value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewSubmissionsList.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewSubmissionsList.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewSubmissionsList.Pager
        Get
            Return ViewStateOrDefault("Pager", New lm.Comol.Core.DomainModel.PagerBase)
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgridTop.Pager = value
            Me.PGgridBottom.Pager = value
            Me.DVpagerTop.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
            Me.DVpagerBottom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewSubmissionsList.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewSubmissionsList.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return False
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property CurrentFilters As dtoSubmissionFilters Implements IViewSubmissionsList.CurrentFilters
        Get
            Dim filter As dtoSubmissionFilters = ViewStateOrDefault("CurrentFilters", PreloadFilters)
            filter.SearchForName = Me.TXBusername.Text
            If Me.RBLstatus.SelectedIndex = -1 Then
                filter.Status = SubmissionFilterStatus.OnlySubmitted
            Else
                filter.Status = Me.RBLstatus.SelectedValue
            End If
            Return filter
        End Get
        Set(value As dtoSubmissionFilters)
            ViewState("CurrentFilters") = value
        End Set
    End Property
    Private Property CurrentFilterBy As SubmissionFilterStatus Implements IViewSubmissionsList.CurrentFilterBy
        Get
            Return ViewStateOrDefault("CurrentFilterBy", SubmissionFilterStatus.OnlySubmitted)
        End Get
        Set(ByVal value As SubmissionFilterStatus)
            Me.ViewState("CurrentFilterBy") = value
        End Set
    End Property
    Private Property CurrentOrderBy As SubmissionsOrder Implements IViewSubmissionsList.CurrentOrderBy
        Get
            Return ViewStateOrDefault("CurrentOrderBy", SubmissionsOrder.ByUser)
        End Get
        Set(ByVal value As SubmissionsOrder)
            Me.ViewState("CurrentOrderBy") = value
        End Set
    End Property
    Private Property CurrentAscending As Boolean Implements IViewSubmissionsList.CurrentAscending
        Get
            Return ViewStateOrDefault("CurrentAscending", True)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("CurrentAscending") = value
        End Set
    End Property
    Private Property PageSize As Integer Implements IViewSubmissionsList.PageSize
        Get
            Return ViewStateOrDefault("PageSize", 50)
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("PageSize") = value
        End Set
    End Property
    Private Property AllowExport As Boolean Implements IViewSubmissionsList.AllowExport
        Get
            Return ViewStateOrDefault("AllowExport", False)
        End Get
        Set(value As Boolean)
            Me.ViewState("AllowExport") = value
            LNBexportToXML.Visible = value
            LNBexportDataToXML.Visible = value
            LNBexportToCsv.Visible = value
            LNBexportToCsv.Visible = value
            Me.DVexport.Visible = value
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

#Region "Internal"
    Public ReadOnly Property GetRevisionCssClass(dto As dtoSubmissionDisplay) As String
        Get
            Return IIf(dto.Revisions.Count > 0, "hasrevision" & IIf(PreloadIdSubmission = dto.Id, " expanded", ""), "")
        End Get
    End Property
    Public ReadOnly Property GetRevisionCssClass(display As displayAs) As String
        Get
            Return IIf(display <> displayAs.item, display.ToString, "")
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False

        If Not Page.IsPostBack Then
            CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionList,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "")
        End If

        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionList,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "NoPermission")
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Calls", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnoSubmissions.Text = Resource.getValue("LBnoSubmissions." & PreloadCallType.ToString())
            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setHyperLink(HYPmanage, PreloadCallType.ToString(), True, True)
            .setHyperLink(HYPlist, PreloadCallType.ToString(), True, True)
            .setLabel(LBsearchSubmissionFor_t)
            .setLabel(LBsubmissionStatusFilter_t)
            .setButton(BTNfindSubmissions, True)
            .setLinkButton(LNBexportDataToCsv, False, True)
            .setLinkButton(LNBexportDataToXML, True, True)
            .setLinkButton(LNBexportToCsv, False, True)
            .setLinkButton(LNBexportToXML, False, True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SetContainerName(callName As String, edition As String, type As CallForPaperType) Implements IViewSubmissionsList.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitleSubmissions." & type.ToString())
        Dim toolTip As String = Me.Resource.getValue("serviceToolTipSubmissions." & type.ToString() & IIf(String.IsNullOrEmpty(edition), "", ".edition"))

        title = String.Format(title, callName)
        If String.IsNullOrEmpty(edition) Then
            toolTip = String.Format(title, callName)
        Else
            toolTip = String.Format(title, callName, edition)
        End If
        Master.ServiceTitle = title
        Master.ServiceTitleToolTip = toolTip
    End Sub
    Private Sub DisplayUnknownCall() Implements IViewSubmissionsList.DisplayUnknownCall
        Me.MLVsubmissions.SetActiveView(VIWnoItems)
        Me.LBnoSubmissions.Text = Resource.getValue("DisplayUnknownCall." & CallType.ToString)
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewSubmissionsList.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.Visible = True
                Me.HYPlist.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Manage
                Me.HYPmanage.Visible = True
                Me.HYPmanage.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Sub LoadRevisions(items As List(Of dtoSubmissionDisplayItemPermission)) Implements IViewSubmissionsList.LoadSubmissions
        If IsNothing(items) OrElse items.Count = 0 Then
            Me.MLVsubmissions.SetActiveView(VIWnoItems)
        Else
            Me.MLVsubmissions.SetActiveView(VIWlist)
            Me.RPTsubmissions.DataSource = items
            Me.RPTsubmissions.DataBind()
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.ViewSubmissions(CallType, PreloadIdCall, PreloadIdSubmission, PreloadIdRevision, PreloadView, PreloadFilterBy, PreloadOrderBy, PreloadAscending, PreloadPageIndex, PreloadPageSize)
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If

        CallTrapHelper.SendTrap(
            lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionList,
            Me.IdCall,
            lm.Comol.Modules.CallForPapers.Trap.CallObjectId.CallForPeaper,
            "SessionTimeOut")

        webPost.Redirect(dto)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleCallForPaper.ActionType) Implements IViewSubmissionsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRequestForMembership.ActionType) Implements IViewSubmissionsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idSubmission As Long, action As ModuleCallForPaper.ActionType) Implements IViewSubmissionsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.UserSubmission, idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idSubmission As Long, action As ModuleRequestForMembership.ActionType) Implements IViewSubmissionsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.UserSubmission, idSubmission.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub LoadNoRevisionsFound() Implements IViewSubmissionsList.LoadNoSubmissionsFound
        Me.MLVsubmissions.SetActiveView(VIWnoItems)
        Me.LBnoSubmissions.Text = Resource.getValue("LBnoSubmissions." & CallType.ToString)
    End Sub
    Private Sub LoadRevisionStatus(items As List(Of SubmissionFilterStatus), selected As SubmissionFilterStatus) Implements IViewSubmissionsList.LoadSubmissionStatus
        Me.RBLstatus.Items.Clear()
        Me.RBLstatus.Items.AddRange((From s As SubmissionFilterStatus In items Where s <> SubmissionFilterStatus.All Select New ListItem(Resource.getValue("SubmissionFilterStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)

        If Not IsNothing(items) AndAlso items.Contains(SubmissionFilterStatus.All) Then
            Me.RBLstatus.Items.Insert(0, New ListItem(Resource.getValue("SubmissionFilterStatus." & SubmissionFilterStatus.All.ToString), CInt(SubmissionFilterStatus.All)))
        End If

        If Me.RBLstatus.Items.Count = 0 Then
            Me.RBLstatus.Items.Add(New ListItem(Resource.getValue("SubmissionFilterStatus." & selected.ToString), CInt(selected)))
        ElseIf Not IsNothing(RBLstatus.Items.FindByValue(selected)) Then
            Me.RBLstatus.SelectedValue = CInt(selected)
        Else
            Me.RBLstatus.SelectedIndex = 0
        End If
        DVstatusfilter.visible = (items.Count > 0)
    End Sub
    Private Sub GotoUrl(url As String) Implements IViewSubmissionsList.GotoUrl
        PageUtility.RedirectToUrl(url)
    End Sub

   
#End Region

    Private Sub RPTsubmissions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsubmissions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoSubmissionDisplayItemPermission = DirectCast(e.Item.DataItem, dtoSubmissionDisplayItemPermission)

            Dim oLiteral As Literal = e.Item.FindControl("LTpartecipant")
            Dim Owner As String
            Dim idUser As Integer = 0

            If Not IsNothing(dto.Submission.Owner) Then
                idUser = dto.Submission.Owner.Id
            End If

            If Not IsNothing(dto.Submission.Owner) AndAlso dto.Submission.Owner.TypeID <> UserTypeStandard.Guest Then
                Owner = dto.Submission.Owner.SurnameAndName
            Else
                Owner = Resource.getValue("AnonymousOwnerName")
            End If
            oLiteral.Text = Owner
            Dim oLabel As Label = e.Item.FindControl("LBrevisionField")
            oLabel.Visible = dto.Submission.Revisions.Count > 0
            Resource.setLabel(oLabel)

            oLiteral = e.Item.FindControl("LTsubPartecipantType")
            oLiteral.Text = dto.Submission.Type.Name

            Dim oCell As HtmlControls.HtmlTableCell
            If Me.RBLstatus.SelectedIndex > -1 AndAlso Me.RBLstatus.SelectedValue = SubmissionFilterStatus.OnlySubmitted Then
                oCell = e.Item.FindControl("TDsubmittedOn")
                oCell.Visible = True
                oLiteral = e.Item.FindControl("LTsubSubmittedOn")

                If dto.Submission.SubmittedOn.HasValue Then
                    oLiteral.Text = FormatDateTime(dto.Submission.SubmittedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.Submission.SubmittedOn.Value, DateFormat.ShortTime)
                Else
                    oLiteral.Text = " "
                End If
            Else
                oCell = e.Item.FindControl("TDmodifedOn")
                oCell.Visible = True

                oLiteral = e.Item.FindControl("LTsubModifedOn")
                If dto.Submission.SubmittedOn.HasValue Then
                    oLiteral.Text = FormatDateTime(dto.Submission.SubmittedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.Submission.SubmittedOn.Value, DateFormat.ShortTime)
                ElseIf dto.Submission.ModifiedOn.HasValue Then
                    oLiteral.Text = FormatDateTime(dto.Submission.ModifiedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.Submission.ModifiedOn.Value, DateFormat.ShortTime)
                Else
                    oLiteral.Text = " "
                End If
            End If

            oLabel = e.Item.FindControl("LBstatus")
            Dim revStatus As RevisionStatus = RevisionStatus.None
            If dto.Submission.Revisions.Count > 0 Then
                revStatus = dto.Submission.Revisions.FirstOrDefault.RevisionStatus
            End If
            oLabel.CssClass = "icon status "
            oLiteral = e.Item.FindControl("LTsubStatus")

            ''If revStatus = RevisionStatus.None OrElse revStatus = RevisionStatus.Approved Then
            oLabel.CssClass &= "submission " & dto.Submission.Status.ToString.ToLower
            oLabel.ToolTip = Resource.getValue("SubmissionStatus." & dto.Submission.Status.ToString())
            oLiteral.Text = Resource.getValue("SubmissionStatus." & dto.Submission.Status.ToString())
            'Else
            '    oLabel.CssClass &= "revision " & revStatus.ToString.ToLower
            '    oLabel.ToolTip = Resource.getValue("RevisionStatus." & revStatus.ToString())
            '    oLiteral.Text = Resource.getValue("RevisionStatus." & revStatus.ToString())
            'End If


            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewSubmission")
            oLiteral = e.Item.FindControl("LTemptyActions")

            Me.Resource.setHyperLink(oHyperlink, True, True)
            oHyperlink.Visible = dto.Permission.Edit

            'If revStatus = RevisionStatus.None OrElse revStatus = RevisionStatus.Approved Then
            oHyperlink.NavigateUrl = BaseUrl & RootObject.ViewSubmissionAsManager(CallType, IdCall, dto.Submission.Id, dto.Submission.IdRevision, dto.Submission.UniqueID, False, CurrentView, Me.CurrentFilterBy, CurrentOrderBy, CurrentAscending, Pager.PageIndex, PageSize, 0)
            oHyperlink.NavigateUrl = oHyperlink.NavigateUrl.Replace("&cmmId=0", String.Format("&cmmId={0}", Me.CommissionId))



            'Else
            '    oHyperlink.NavigateUrl = BaseUrl & RootObject.ManageReviewSubmission(CallType, IdCall, dto.Submission.Id, dto.Submission.IdRevision, CallStandardAction.Manage, CurrentView, Me.CurrentFilterBy, CurrentOrderBy, CurrentAscending, Pager.PageIndex, PageSize)
            'End If

            Dim oLinkbutton As LinkButton = e.Item.FindControl("LNBsubDelete")
            Resource.setLinkButton(oLinkbutton, True, True)
            oLinkbutton.Visible = dto.Permission.Delete

            oLinkbutton = e.Item.FindControl("LNBsubVirtualDelete")
            Resource.setLinkButton(oLinkbutton, True, True)
            oLinkbutton.Visible = dto.Permission.VirtualDelete

            oLinkbutton = e.Item.FindControl("LNBsubRecover")
            oLinkbutton.Visible = dto.Permission.VirtualUndelete
            Resource.setLinkButton(oLinkbutton, True, True)

            oLiteral.Visible = Not (dto.Permission.Edit OrElse dto.Permission.Delete OrElse dto.Permission.VirtualDelete OrElse dto.Permission.VirtualUndelete)

            Dim oControl As UC_SubmissionExport = e.Item.FindControl("CTRLreport")
            Dim loadTypes As New List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
            loadTypes.Add(Helpers.Export.ExportFileType.pdf)
            loadTypes.Add(Helpers.Export.ExportFileType.zip)

            oControl.WebOnlyRender = True
            oControl.InitializeControl( _
                False, _
                idUser, _
                Owner, _
                IdCall, _
                dto.Submission.Id, _
                dto.Submission.IdRevision, _
                IdCallModule, _
                IdCallCommunity, _
                CallType, _
                loadTypes)

            Dim CTRLprintDraf As UC_PrintDraft = e.Item.FindControl("CTRLprintDraf")


            CTRLprintDraf.visible = False

            If dto.Submission.Status = SubmissionStatus.draft Then
                oControl.ShowForDraft()
                CTRLprintDraf.Visible = True
                CTRLprintDraf.InitializeUC(IdCall, dto.Submission.Type.Id, dto.Submission.IdRevision, dto.Submission.Id)

            End If

            'sign
            Dim CTRLdisplaySignNoRev As UC_ModuleRepositoryAction = e.Item.FindControl("CTRLdisplaySignNoRev")
            InitUcDownloadSign(dto.Submission.SignLink, CTRLdisplaySignNoRev)

            Dim LBLNoSign As Label = e.Item.FindControl("LBLNoSign")
            LBLNoSign.Visible = Not CTRLdisplaySignNoRev.Visible


            Dim LTintegration As Literal = e.Item.FindControl("LTintegration")
            If Not IsNothing(LTintegration) Then
                If IsAdvance Then
                    Dim totalIntegration As Integer = (dto.Submission.RevisionSended + dto.Submission.RevisionAswered)
                    If (totalIntegration > 0) Then
                        LTintegration.Text = String.Format(LTintegrationContent.Text, dto.Submission.RevisionSended, dto.Submission.RevisionAswered, totalIntegration)
                        LTintegration.Visible = True
                    Else
                        LTintegration.Visible = False
                    End If
                Else
                    LTintegration.Visible = False
                End If
            End If


        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTsubPartecipant_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubPartecipantType_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubSubmittedOn_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubModifedOn_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubStatus_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTsubActions_t")
            Resource.setLiteral(oLiteral)


            Dim oCell As HtmlControls.HtmlTableCell
            If Me.RBLstatus.SelectedIndex > -1 AndAlso Me.RBLstatus.SelectedValue = SubmissionFilterStatus.OnlySubmitted Then
                oCell = e.Item.FindControl("THsubmittedOn")
                oCell.Visible = True
            Else
                oCell = e.Item.FindControl("THmodifedOn")
                oCell.Visible = True
            End If


            Dim oLinkButton As LinkButton

            For Each name As String In [Enum].GetNames(GetType(SubmissionsOrder))
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Up")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("SubmissionsOrder.Ascending") & Resource.getValue("SubmissionsOrder." & name)
                End If
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Down")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("SubmissionsOrder.Descending") & Resource.getValue("SubmissionsOrder." & name)
                End If
            Next
        End If
    End Sub

    Protected Sub RPTrevisions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        Dim dto As dtoSubmissionItem = DirectCast(e.Item.DataItem, dtoSubmissionItem)
        Dim oLabel As Label = e.Item.FindControl("LBrevisionName")

        If dto.RevisionType <> RevisionType.Original Then
            oLabel.Text = String.Format(Resource.getValue("RevisionVersion"), dto.DisplayNumber)

            oLabel = e.Item.FindControl("LBrevisionby")
            oLabel.Text = Resource.getValue("RevisionType." & dto.RevisionType.ToString())
        Else
            oLabel.Text = Resource.getValue("RevisionType." & dto.RevisionType.ToString())
        End If

       

        Dim oLiteral As Literal = e.Item.FindControl("LTsubSubmittedOn")

        If dto.ModifiedOn.HasValue Then
            oLiteral.Text = FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.ModifiedOn.Value, DateFormat.ShortTime)
        Else
            oLiteral.Text = " "
        End If

        If dto.RevisionType = RevisionType.Original Then
            oLabel = e.Item.FindControl("LBstatus")
            oLabel.CssClass = "icon status submission " & dto.Submission.Status.ToString().ToLower()
            oLabel.ToolTip = Resource.getValue("SubmissionStatus." & dto.Submission.Status.ToString())

            oLiteral = e.Item.FindControl("LTsubStatus")
            oLiteral.Text = Resource.getValue("SubmissionStatus." & dto.Submission.Status.ToString())
        Else
            oLabel = e.Item.FindControl("LBstatus")
            oLabel.CssClass = "icon status revision " & dto.RevisionStatus.ToString().ToLower()

            oLabel.ToolTip = Resource.getValue("RevisionStatus." & dto.RevisionStatus.ToString())

            oLiteral = e.Item.FindControl("LTsubStatus")
            oLiteral.Text = Resource.getValue("RevisionStatus." & dto.RevisionStatus.ToString())
        End If


        Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewSubmission")
        oLiteral = e.Item.FindControl("LTemptyActions")

        Me.Resource.setHyperLink(oHyperlink, True, True)
        oHyperlink.Visible = True
        If dto.isActive Then
            oHyperlink.NavigateUrl = BaseUrl & RootObject.ViewSubmissionAsManager(CallType, IdCall, dto.Submission.Id, dto.Id, dto.Submission.UniqueID, False, CurrentView, Me.CurrentFilterBy, CurrentOrderBy, CurrentAscending, Pager.PageIndex, PageSize, 0)
        Else
            oHyperlink.NavigateUrl = BaseUrl & RootObject.ManageReviewSubmission(CallType, IdCall, dto.Submission.Id, dto.Id, CallStandardAction.Manage, CurrentView, Me.CurrentFilterBy, CurrentOrderBy, CurrentAscending, Pager.PageIndex, PageSize)
        End If

        Dim Owner As String
        Dim idUser As Integer = 0

        If Not IsNothing(dto.Submission.Owner) Then
            idUser = dto.Submission.Owner.Id
        End If

        If Not IsNothing(dto.Submission.Owner) AndAlso dto.Submission.Owner.TypeID <> UserTypeStandard.Guest Then
            Owner = dto.Submission.Owner.SurnameAndName
        Else
            Owner = Resource.getValue("AnonymousOwnerName")
        End If
        Dim oControl As UC_SubmissionExport = e.Item.FindControl("CTRLreport")
        Dim loadTypes As New List(Of lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        loadTypes.Add(Helpers.Export.ExportFileType.pdf)
        loadTypes.Add(Helpers.Export.ExportFileType.zip)

        oControl.WebOnlyRender = Not dto.isActive
        oControl.InitializeControl(False, idUser, Owner, IdCall, dto.Submission.Id, dto.Submission.IdRevision, IdCallModule, IdCallCommunity, CallType, loadTypes)
        'oHyperlink.NavigateUrl = BaseUrl & RootObject.ManageReviewSubmission(CallType, dto.Revision.IdCall, dto.Revision.IdSubmission, dto.Revision.Id, CallStandardAction.Manage, CallStatusForSubmitters.Revisions)

    End Sub

    Private Sub RPTsubmissions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTsubmissions.ItemCommand
        Select Case e.CommandName
            Case "orderby"
                Dim filter As dtoSubmissionFilters = CurrentFilters
                filter.Ascending = e.CommandArgument.contains("True")
                filter.OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SubmissionsOrder).GetByString(e.CommandArgument.replace("." & filter.Ascending.ToString, ""), SubmissionsOrder.ByUser)

                Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
            Case "delete", "virtualdelete", "recover"
                Dim idSubmission As Long = 0
                If IsNumeric(e.CommandArgument) Then
                    idSubmission = CLng(e.CommandArgument)
                End If

                If idSubmission > 0 Then
                    If e.CommandName = "delete" Then
                        Dim baseFilePath As String = ""
                        If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                            baseFilePath = Server.MapPath(Me.PageUtility.BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath)
                        Else
                            baseFilePath = Me.SystemSettings.File.Materiale.DrivePath
                        End If

                        CallTrapHelper.SendTrap(
                          lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileDelete,
                          idSubmission,
                          lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
                          "Admin-DeletePhisical")


                        Me.CurrentPresenter.PhisicalDeleteSubmission(idSubmission, baseFilePath, CallType, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                    Else

                        CallTrapHelper.SendTrap(
                          lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileDelete,
                          idSubmission,
                          lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
                          "Admin-VirtualDelete")

                        Me.CurrentPresenter.VirtualDeleteSubmission(idSubmission, (e.CommandName = "virtualdelete"), CallType, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                    End If
                Else
                    'CallTrapHelper.SendTrap(
                    '      lm.Comol.Modules.CallForPapers.Trap.CallTrapId.SubmissionCompileSaveDraft,
                    '      idSubmission,
                    '      lm.Comol.Modules.CallForPapers.Trap.CallObjectId.UserSubmission,
                    '      "Admin-Recover")

                    Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                End If
            Case Else



                Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
        End Select
    End Sub


    Protected Sub CTRLreport_GetConfigTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)
        template = Me.Master.getConfigTemplate()
    End Sub
    Protected Sub CTRLreport_GetContainerTemplate(ByRef template As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Template)
        template.Header = Me.Master.getTemplateHeader(" ")
        template.Footer = Me.Master.getTemplateFooter()
    End Sub
    Protected Sub CTRLreport_GetHiddenIdentifierValueEvent(ByRef value As String)
        value = HDNdownloadTokenValue.Value
    End Sub
    Private Sub CTRLreport_RefreshContainerEvent() Handles CTRLreport.RefreshContainerEvent

    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub

    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub


    Private Sub BTNfindSubmissions_Click(sender As Object, e As System.EventArgs) Handles BTNfindSubmissions.Click
        LoadSubmission()
    End Sub

    Private Sub RBLstatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLstatus.SelectedIndexChanged
        LoadSubmission()
    End Sub

    Private Sub LoadSubmission()
        Dim filters As dtoSubmissionFilters = Me.CurrentFilters
        filters.SearchForName = Me.TXBusername.Text
        filters.Ascending = False
        If Me.RBLstatus.SelectedIndex > -1 Then
            filters.Status = Me.RBLstatus.SelectedValue
        End If
        Select Case filters.Status
            Case SubmissionFilterStatus.All
                filters.OrderBy = SubmissionsOrder.ByDate
            Case SubmissionFilterStatus.OnlySubmitted
                filters.OrderBy = SubmissionsOrder.BySubmittedOn
                filters.Ascending = True
            Case Else
                filters.OrderBy = SubmissionsOrder.ByDate
        End Select
        Me.CurrentPresenter.LoadSubmissions(IdCallCommunity, CallType, filters, 0, PreloadPageSize)
    End Sub

    Private Sub LNBexportDataToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportDataToCsv.Click
        ExportData(True, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportDataToXML_Click(sender As Object, e As System.EventArgs) Handles LNBexportDataToXML.Click
        ExportData(True, Helpers.Export.ExportFileType.xml)
    End Sub
    Private Sub LNBexportToCsv_Click(sender As Object, e As System.EventArgs) Handles LNBexportToCsv.Click
        ExportData(False, Helpers.Export.ExportFileType.csv)
    End Sub
    Private Sub LNBexportToXML_Click(sender As Object, e As System.EventArgs) Handles LNBexportToXML.Click
        ExportData(False, Helpers.Export.ExportFileType.xml)
    End Sub

    Private Sub ExportData(fullData As Boolean, exportType As lm.Comol.Core.DomainModel.Helpers.Export.ExportFileType)
        Dim cookie As  New HttpCookie(Me.CTRLreport.CookieName, HDNdownloadTokenValue.Value)
        Dim clientFileName As String = ""

        Dim data As DateTime = DateTime.Now
        If fullData Then
            clientFileName = Resource.getValue("ExportDataListFileName")
            If String.IsNullOrEmpty(clientFileName) Then
                clientFileName = "SubmissionsData_On_{0}_{1}_{2}.{3}"
            End If
        Else
            clientFileName = Resource.getValue("ExportListFileName")
            If String.IsNullOrEmpty(clientFileName) Then
                clientFileName = "SubmissionsList_On_{0}_{1}_{2}.{3}"
            End If
        End If
        clientFileName = String.Format(clientFileName, data.Year, data.Month, data.Day, exportType.ToString())
        Response.Clear()
        Try
            Response.AppendCookie(cookie)
            Response.AddHeader("Content-Disposition", "attachment; filename=" & clientFileName)
            Response.Charset = ""
            Response.ContentEncoding = System.Text.Encoding.Default


            Dim filter As dtoSubmissionFilters = Me.CurrentFilters
            Dim translations As New Dictionary(Of SubmissionsListTranslations, String)
            For Each name As String In [Enum].GetNames(GetType(SubmissionsListTranslations))
                translations.Add([Enum].Parse(GetType(SubmissionsListTranslations), name), Me.Resource.getValue("SubmissionsListTranslations." & name))
            Next

            Select Case exportType
                Case Helpers.Export.ExportFileType.xml
                    Response.ContentType = "application/ms-excel"
                Case Else
                    Response.ContentType = "text/csv"
            End Select
            If fullData Then
                Response.Write(CurrentPresenter.ExportSubmissionsData(exportType, filter, translations, filter.TranslationsSubmission, filter.TranslationsRevision))
            Else
                Response.Write(CurrentPresenter.ExportSubmissions(exportType, filter, translations, filter.TranslationsSubmission, filter.TranslationsRevision))
            End If
        Catch ex As Exception
            Select Case exportType
                Case Helpers.Export.ExportFileType.xml
                    Response.Write(lm.Comol.Modules.CallForPapers.Business.HelperExportToXml.GetErrorDocument(Me.Resource.getValue("SubmissionsListTranslations." & SubmissionsListTranslations.FileCreationError.ToString())))
                Case Else
                    Response.Write(lm.Comol.Modules.CallForPapers.Business.HelperExportToCsv.GetErrorDocument(Me.Resource.getValue("SubmissionsListTranslations." & SubmissionsListTranslations.FileCreationError.ToString())))
            End Select

        End Try
        Response.End()
    End Sub

    'Private Sub LNBexportToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportToXLS.Click
    '    Response.Clear()
    '    Response.AppendCookie(New HttpCookie(Me.CTRLreport.CookieName, HDNdownloadTokenValue.Value)) '; //downloadTokenValue will have been provided in the form submit via the hidden input field
    '    Dim fileName As String = Resource.getValue("ExportListFileName")
    '    Dim data As DateTime = DateTime.Now

    '    If String.IsNullOrEmpty(fileName) Then
    '        fileName = "ListaDomande_al_{0}_{1}_{2}.{3}"
    '    End If
    '    fileName = String.Format(fileName, data.Year, data.Month, data.Day, "xml")
    '    'HttpUtility.UrlEncode(MyBase.ComunitaCorrente.Nome)
    '    'Response.Clear()
    '    Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
    '    '  Response.ContentType = "application/xml"
    '    Response.ContentType = "application/ms-excel"

    '    Try

    '    Catch ex As ExportError
    '        '  Response.Write(ex.ExcelDocument)
    '    Catch de As Exception

    '    End Try
    '    Response.End()
    'End Sub

    'Private Sub LNBexportDataToXLS_Click(sender As Object, e As System.EventArgs) Handles LNBexportDataToXLS.Click
    '    Response.Clear()
    '    Response.AppendCookie(New HttpCookie(Me.CTRLreport.CookieName, HDNdownloadTokenValue.Value)) '; //downloadTokenValue will have been provided in the form submit via the hidden input field
    '    Dim fileName As String = Resource.getValue("ExportDataListFileName")
    '    Dim data As DateTime = DateTime.Now

    '    If String.IsNullOrEmpty(fileName) Then
    '        fileName = "DatiDomande_al_{0}_{1}_{2}.{3}"
    '    End If
    '    fileName = String.Format(fileName, data.Year, data.Month, data.Day, "xml")
    '    'HttpUtility.UrlEncode(MyBase.ComunitaCorrente.Nome)
    '    'Response.Clear()//corretto con benestare di fra
    '    Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
    '    '  Response.ContentType = "application/xml"
    '    Response.ContentType = "application/ms-excel"

    '    Try
    '        Dim filter As dtoSubmissionFilters = Me.CurrentFilters
    '        Dim translations As New Dictionary(Of SubmissionsListTranslations, String)
    '        For Each name As String In [Enum].GetNames(GetType(SubmissionsListTranslations))
    '            translations.Add([Enum].Parse(GetType(SubmissionsListTranslations), name), Me.Resource.getValue("SubmissionsListTranslations." & name))
    '        Next

    '    Catch ex As ExportError
    '        '  Response.Write(ex.ExcelDocument)
    '    Catch de As Exception

    '    End Try
    '    Response.End()
    'End Sub
#Region "sign"
    Private Property HasSign As Boolean Implements IViewSubmissionsList.HasSignature
        Get
            Return ViewStateOrDefault("HasSign", False)
        End Get
        Set(value As Boolean)
            ViewState("HasSign") = value
        End Set
    End Property

    Public ReadOnly Property GetSignCss As String
        Get
            If HasSign Then
                Return "sign"
            Else
                Return "hide"
            End If
        End Get
    End Property

    Private Sub InitUcDownloadSign(ByVal link As ModuleLink, ByVal oCtrlSign As UC_ModuleRepositoryAction)

        If Not IsNothing(oCtrlSign) Then

            If HasSign AndAlso Not IsNothing(link) AndAlso link.Id > 0 Then
                oCtrlSign.Visible = True
                Dim initializer As New lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer

                ' DIMENSIONI IMMAGINI
                initializer.IconSize = Helpers.IconSize.Small
                oCtrlSign.EnableAnchor = True

                initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.defaultAction _
                    Or lm.Comol.Core.ModuleLinks.DisplayActionMode.actions

                initializer.Link = link
                oCtrlSign.InsideOtherModule = True
                Dim actions As List(Of dtoModuleActionControl)
                actions = oCtrlSign.InitializeRemoteControl(initializer, initializer.Display)
            Else
                oCtrlSign.Visible = False
            End If

        End If
    End Sub

#End Region

    Private ReadOnly Property CommissionId As Int64
        Get
            Dim cmId As Int64 = 0

            Try
                cmId = System.Convert.ToInt64(Request.QueryString("cmmId"))
            Catch ex As Exception

            End Try

            Return cmId
        End Get
    End Property

    Public Property IsAdvance As Boolean Implements IViewSubmissionsList.IsAdvance
        Get
            Return ViewStateOrDefault("CallIsAdvance", False)
        End Get
        Set(value As Boolean)
            ViewState("CallIsAdvance") = value
        End Set
    End Property
End Class
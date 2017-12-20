Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq

Public Class Revisions
    Inherits PageBase
    Implements IViewRevisionList

#Region "Context"
    Private _Presenter As RevisionListPresenter
    Private ReadOnly Property CurrentPresenter() As RevisionListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New RevisionListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewRevisionList.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadAction As CallStandardAction Implements IViewRevisionList.PreloadAction
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStandardAction).GetByString(Request.QueryString("action"), CallStandardAction.List)
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewRevisionList.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewRevisionList.PreloadView
        Get
            Return CallStatusForSubmitters.Revisions
        End Get
    End Property
    Private ReadOnly Property PreloadCallType As CallForPaperType Implements IViewRevisionList.PreloadCallType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadPageIndex As Integer Implements IViewRevisionList.PreloadPageIndex
        Get
            If IsNumeric(Me.Request.QueryString("PageIndex")) Then
                Return CInt(Me.Request.QueryString("PageIndex"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadPageSize As Integer Implements IViewRevisionList.PreloadPageSize
        Get
            If IsNumeric(Me.Request.QueryString("PageSize")) Then
                Return CInt(Me.Request.QueryString("PageSize"))
            Else
                Return 50
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadOrderBy As RevisionOrder Implements IViewRevisionList.PreloadOrderBy
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RevisionOrder).GetByString(Request.QueryString("OrderBy"), IIf(PreloadAction = CallStandardAction.List, RevisionOrder.ByDeadline, RevisionOrder.ByDate))
        End Get
    End Property
    Private ReadOnly Property PreloadSearchForName As String Implements IViewRevisionList.PreloadSearchForName
        Get
            Return Request.QueryString("SearchForName")
        End Get
    End Property
    Private ReadOnly Property PreloadFilters As dtoRevisionFilters Implements lm.Comol.Modules.CallForPapers.Presentation.IViewRevisionList.PreloadFilters
        Get
            Dim dto As New dtoRevisionFilters
            With dto
                .Ascending = PreloadAscending
                .CallType = PreloadCallType
                .OrderBy = PreloadOrderBy
                .SearchForName = PreloadSearchForName
                .Status = RevisionStatus.None

                Dim translations As New Dictionary(Of RevisionStatus, String)
                For Each name As String In [Enum].GetNames(GetType(RevisionStatus))
                    translations.Add([Enum].Parse(GetType(RevisionStatus), name), Me.Resource.getValue("RevisionStatus." & name))
                Next
                .TranslationsStatus = translations

                Dim types As New Dictionary(Of RevisionType, String)
                For Each name As String In [Enum].GetNames(GetType(RevisionType))
                    types.Add([Enum].Parse(GetType(RevisionType), name), Me.Resource.getValue("RevisionType." & name))
                Next
                .TranslationsType = types
            End With
            Return dto
        End Get
    End Property

    Private Property AllowManage As Boolean Implements IViewRevisionList.AllowManage
        Get
            Return ViewStateOrDefault("AllowManage", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowManage") = value
        End Set
    End Property
    Private Property AllowView As Boolean Implements IViewRevisionList.AllowView
        Get
            Return ViewStateOrDefault("AllowView", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowView") = value
        End Set
    End Property
    Private Property CurrentAction As CallStandardAction Implements IViewRevisionList.CurrentAction
        Get
            Return ViewStateOrDefault("CurrentAction", CallStandardAction.List)
        End Get
        Set(ByVal value As CallStandardAction)
            Me.ViewState("CurrentAction") = value
            DVfilter.Visible = (value = CallStandardAction.Manage)
        End Set
    End Property
    Private Property CurrentView As CallStatusForSubmitters Implements IViewRevisionList.CurrentView
        Get
            Return CallStatusForSubmitters.Revisions
        End Get
        Set(ByVal value As CallStatusForSubmitters)
            Dim oTab As Telerik.Web.UI.RadTab = Me.TBScall.FindTabByValue(value.ToString)
            If Not IsNothing(oTab) Then
                oTab.Selected = True
                Me.ViewState("CurrentView") = value
            End If
        End Set
    End Property
    Private Property IdCallCommunity As Integer Implements IViewRevisionList.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewRevisionList.Pager
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
    Private Property CallType As CallForPaperType Implements IViewRevisionList.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
        End Set
    End Property
    Private ReadOnly Property PreloadAscending As Boolean Implements IViewRevisionList.PreloadAscending
        Get
            If String.IsNullOrEmpty(Request.QueryString("Ascending")) Then
                Return False
            Else
                Return (Request.QueryString("Ascending") = "true" OrElse Request.QueryString("Ascending") = "True")
            End If
        End Get
    End Property
    Private Property CurrentFilters As dtoRevisionFilters Implements IViewRevisionList.CurrentFilters
        Get
            Dim filter As dtoRevisionFilters = ViewStateOrDefault("CurrentFilters", PreloadFilters)
            filter.SearchForName = Me.TXBusername.Text
            If Me.RBLstatus.SelectedIndex = -1 Then
                filter.Status = RevisionStatus.None
            Else
                filter.Status = Me.RBLstatus.SelectedValue
            End If
            Return filter
        End Get
        Set(value As dtoRevisionFilters)
            ViewState("CurrentFilters") = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.PGgridTop.Pager = Me.Pager
        Me.PGgridBottom.Pager = Me.Pager
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
        MyBase.SetCulture("pg_Calls", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnoRevisions.Text = Resource.getValue("LBnoRevisions." & PreloadCallType.ToString())
            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setHyperLink(HYPmanage, PreloadCallType.ToString(), True, True)
            .setHyperLink(HYPlist, PreloadCallType.ToString(), True, True)
            .setLabel(LBrevisionStatusFilter_t)
            .setLabel(LBsearchRevisionFor_t)
            .setButton(BTNfindRevisions, True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SetContainerName(idCommunity As Integer, name As String) Implements IViewRevisionList.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & CurrentAction.ToString() & "." & CallType.ToString())
        Master.ServiceTitle = title
        If idCommunity = 0 Then
            Master.ServiceTitleToolTip = title
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & CurrentAction.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub
    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewRevisionList.SetActionUrl
        Select Case action
            Case CallStandardAction.List
                Me.HYPlist.Visible = True
                Me.HYPlist.NavigateUrl = BaseUrl & url
            Case CallStandardAction.Manage
                Me.HYPmanage.Visible = True
                Me.HYPmanage.NavigateUrl = BaseUrl & url
        End Select
    End Sub
    Private Sub LoadAvailableView(idCommunity As Integer, views As List(Of CallStatusForSubmitters)) Implements IViewRevisionList.LoadAvailableView
        Me.TBScall.Enabled = (views.Count > 0)
        Dim SelectedIndex As Integer = 0
        If views.Count > 0 Then
            For Each view As CallStatusForSubmitters In views
                Dim oTabView As Telerik.Web.UI.RadTab = Me.TBScall.Tabs.FindTabByValue(view.ToString)
                If Not IsNothing(oTabView) Then
                    If view = CallStatusForSubmitters.Revisions Then
                        oTabView.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ViewRevisions(CallType, CurrentAction, idCommunity, view)
                    Else
                        oTabView.NavigateUrl = Me.PageUtility.BaseUrl & RootObject.ViewCalls(CallType, CurrentAction, idCommunity, view)
                    End If
                    oTabView.Text = Resource.getValue("CallStatusForSubmitters." & CallType.ToString & "." & view.ToString)
                    oTabView.Visible = True
                End If
            Next
        End If
        Me.TBScall.SelectedIndex = SelectedIndex
    End Sub

    Private Sub LoadRevisions(items As List(Of dtoRevisionDisplayItemPermission)) Implements IViewRevisionList.LoadRevisions
        If IsNothing(items) OrElse items.Count = 0 Then
            Me.MLVresults.SetActiveView(VIWnoItems)
        Else
            Me.MLVresults.SetActiveView(VIWlist)
            Me.RPTrevisions.DataSource = items
            Me.RPTrevisions.DataBind()

            Dim oCell As HtmlTableCell = Nothing
            If items.Where(Function(i) i.Revision.Type = RevisionType.Manager).Any = False AndAlso Me.CurrentAction = CallStandardAction.List Then
                oCell = RPTrevisions.Controls(0).FindControl("THsubmitter")
                oCell.Visible = False
                For Each item As RepeaterItem In (From r As RepeaterItem In RPTrevisions.Items Where r.ItemType <> ListItemType.Footer Select r).ToList
                    oCell = item.FindControl("THsubmitter")
                    oCell.Visible = False
                Next
            End If
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
        dto.DestinationUrl = RootObject.ViewRevisions(CallType, PreloadAction, idCommunity, PreLoadedContentView)
        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleCallForPaper.ActionType) Implements IViewRevisionList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As ModuleRequestForMembership.ActionType) Implements IViewRevisionList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Private Sub LoadNoRevisionsFound() Implements IViewRevisionList.LoadNoRevisionsFound
        Me.MLVresults.SetActiveView(VIWnoItems)
    End Sub
    Private Sub LoadRevisionStatus(items As List(Of RevisionStatus), selected As RevisionStatus) Implements IViewRevisionList.LoadRevisionStatus
        Me.RBLstatus.Items.Clear()
        Me.RBLstatus.Items.AddRange((From s As RevisionStatus In items Where s <> RevisionStatus.None Select New ListItem(Resource.getValue("RevisionStatus." & s.ToString), CInt(s))).OrderBy(Function(l) l.Text).ToArray)

        If Not IsNothing(items) AndAlso items.Contains(RevisionStatus.None) Then
            Me.RBLstatus.Items.Insert(0, New ListItem(Resource.getValue("RevisionStatus." & RevisionStatus.None.ToString), CInt(RevisionStatus.None)))
        End If

        If Me.RBLstatus.Items.Count = 0 Then
            Me.RBLstatus.Items.Add(New ListItem(Resource.getValue("RevisionStatus." & selected.ToString), CInt(selected)))
        ElseIf Not IsNothing(RBLstatus.Items.FindByValue(selected)) Then
            Me.RBLstatus.SelectedValue = CInt(selected)
        Else
            Me.RBLstatus.SelectedIndex = 0
        End If

    End Sub
#End Region

    Private Sub RPTrevisions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTrevisions.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoRevisionDisplayItemPermission = DirectCast(e.Item.DataItem, dtoRevisionDisplayItemPermission)

            Dim oLiteral As Literal = e.Item.FindControl("LTrevCallName")
            oLiteral.Text = dto.Revision.CallName

            oLiteral = e.Item.FindControl("LTrevRequiredBy")
            If CurrentAction = CallStandardAction.List AndAlso Not IsNothing(dto.Revision.Manager) Then
                oLiteral.Text = dto.Revision.Manager.SurnameAndName
            ElseIf CurrentAction = CallStandardAction.Manage AndAlso Not IsNothing(dto.Revision.Submitter) Then
                oLiteral.Text = dto.Revision.Submitter.SurnameAndName
            Else
                oLiteral.Text = " "
            End If

            oLiteral = e.Item.FindControl("LTrevDate")
            If Not IsNothing(dto.Revision.CreatedOn) Then
                oLiteral.Text = FormatDateTime(dto.Revision.CreatedOn, DateFormat.ShortDate)
            Else
                oLiteral.Text = " "
            End If

            oLiteral = e.Item.FindControl("LTrevDeadline")
            If dto.Revision.EndDate.HasValue Then
                oLiteral.Text = FormatDateTime(dto.Revision.EndDate.Value, DateFormat.ShortDate) & " " & FormatDateTime(dto.Revision.EndDate.Value, DateFormat.ShortTime)
            Else
                oLiteral.Text = " "
            End If

            Dim oLabel As Label = e.Item.FindControl("LBrevisionType")
            oLabel.Text = Resource.getValue("RevisionType." & dto.Revision.Type.ToString)

            oLabel = e.Item.FindControl("LBstatus")
            oLabel.CssClass = "icon status " & dto.Revision.Status.ToString.ToLower()
            oLabel.ToolTip = Resource.getValue("RevisionStatus." & dto.Revision.Status.ToString())
            
            oLiteral = e.Item.FindControl("LTrevStatus")
            oLiteral.Text = Resource.getValue("RevisionStatus." & dto.Revision.Status.ToString())

            Dim oButton As Button = e.Item.FindControl("BTNlistCancelRequest")
            Dim oHyperlink As HyperLink = e.Item.FindControl("HYPviewRevision")
            oLiteral = e.Item.FindControl("LTemptyActions")

            oButton.Visible = dto.Permission.Cancell OrElse dto.Permission.RefuseUserRequest
            If dto.Permission.Cancell OrElse dto.Permission.RefuseUserRequest Then
                oButton.CommandArgument = dto.Id
                If dto.Permission.RefuseUserRequest Then
                    oButton.CommandName = "refuseUserRequest"
                ElseIf dto.Permission.Cancell Then
                    oButton.CommandName = IIf(dto.Revision.Type = RevisionType.Manager, "cancellManagerRequest", "cancellUserRequest")
                End If
                Resource.setButtonByValue(oButton, oButton.CommandName, True)
            End If
            Me.Resource.setHyperLink(oHyperlink, True, True)
            oHyperlink.Visible = dto.Permission.Compile OrElse dto.Permission.Manage OrElse dto.Permission.RefuseUserRequest

            If dto.Permission.Compile Then
                oHyperlink.NavigateUrl = BaseUrl & RootObject.UserReviewCall(CallType, dto.Revision.IdCall, dto.Revision.IdSubmission, dto.Revision.Id, CallStatusForSubmitters.Revisions, PreloadIdCommunity)
            ElseIf dto.Permission.Manage Then
                oHyperlink.NavigateUrl = BaseUrl & RootObject.ManageReviewSubmission(CallType, dto.Revision.IdCall, dto.Revision.IdSubmission, dto.Revision.Id, CallStandardAction.Manage, CallStatusForSubmitters.Revisions)
            End If
            oLiteral.Visible = Not (dto.Permission.Cancell OrElse dto.Permission.Compile OrElse dto.Permission.Manage)


        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLiteral As Literal = e.Item.FindControl("LTrevCallName_t")
            oLiteral.Text = Resource.getValue("RevisionOrder." & RevisionOrder.ByCall.ToString & "." & CallType.ToString)

            oLiteral = e.Item.FindControl("LTrevRequiredBy_t")
            Resource.setLiteral(oLiteral, IIf(CurrentAction = CallStandardAction.List, RevisionType.Manager.ToString(), RevisionType.UserRequired.ToString()))

            oLiteral = e.Item.FindControl("LTrevDate_t")
            Resource.setLiteral(oLiteral)
           
            oLiteral = e.Item.FindControl("LTrevDeadline_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTrevStatus_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTrevActions_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTrevType_t")
            Resource.setLiteral(oLiteral)

            Dim oLinkButton As LinkButton

            For Each name As String In [Enum].GetNames(GetType(RevisionOrder))
                If name = RevisionOrder.ByUser.ToString() Then
                    name = RevisionOrder.ByUser.ToString() & IIf(CurrentAction = CallStandardAction.List, RevisionType.Manager.ToString(), RevisionType.UserRequired.ToString())
                End If
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Up")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("RevisionOrderBy.Ascending") & Resource.getValue("RevisionOrder." & name)
                End If
                oLinkButton = e.Item.FindControl("LNBorder" & name & "Down")
                If Not IsNothing(oLinkButton) Then
                    oLinkButton.ToolTip = Resource.getValue("RevisionOrderBy.Descending") & Resource.getValue("RevisionOrder." & name)
                End If
            Next
        End If
    End Sub

    Private Sub RPTrevisions_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTrevisions.ItemCommand
        Select Case e.CommandName
            Case "orderby"
                Dim filter As dtoRevisionFilters = CurrentFilters
                filter.Ascending = e.CommandArgument.contains("True")
                filter.OrderBy = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of RevisionOrder).GetByString(e.CommandArgument.replace("." & filter.Ascending.ToString, ""), RevisionOrder.ByDate)

                Me.CurrentPresenter.LoadRevisions(CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
            Case "cancellUserRequest", "cancellManagerRequest", "refuseUserRequest"
                Dim idRevision As Long = 0
                If IsNumeric(e.CommandArgument) Then
                    idRevision = CLng(e.CommandArgument)
                End If
                If idRevision > 0 Then
                    Select Case e.CommandName
                        Case "cancellUserRequest"
                            Me.CurrentPresenter.RemoveSelfRequest(idRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveSelfRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveSelfRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "cancellManagerRequest"
                            Me.CurrentPresenter.RemoveManagerRequest(idRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveManagerRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                        Case "refuseUserRequest"
                            Me.CurrentPresenter.RemoveUserRequest(idRevision, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase, CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                    End Select
                    'Private Sub BTNcancelUserRevisionRequest_Click(sender As Object, e As System.EventArgs) Handles BTNcancelUserRevisionRequest.Click
                    '    Me.CurrentPresenter.RemoveUserRequest(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RemoveUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RemoveUserRequest." & CallType.ToString() & ".Body" & IIf(String.IsNullOrEmpty(Me.TXBfeedback.Text), "", ".Reason"))}, PageUtility.ApplicationUrlBase)
                    'End Sub
                    'Private Sub BTNcancelManagerRequest_Click(sender As Object, e As System.EventArgs) Handles BTNcancelManagerRequest.Click
                    '   
                    'End Sub
                    'Private Sub BTNrefuseUserRequest_Click(sender As Object, e As System.EventArgs) Handles BTNrefuseUserRequest.Click
                    '    Me.CurrentPresenter.RefuseUserRequest(IdSubmission, IdRevision, Me.TXBfeedback.Text, New dtoRevisionMessage() With {.SmtpConfig = PageUtility.CurrentSmtpConfig, .Subject = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Subject"), .Body = Resource.getValue("RefuseUserRequest." & CallType.ToString() & ".Body")}, PageUtility.ApplicationUrlBase)
                    'End Sub
                Else
                    Me.CurrentPresenter.LoadRevisions(CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
                End If
            Case Else
                Me.CurrentPresenter.LoadRevisions(CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
        End Select
        'Dim idCall As Long = 0
        'If IsNumeric(e.CommandArgument) Then
        '    idCall = CLng(e.CommandArgument)
        'End If
        'Select Case e.CommandName
        '    Case "virtualdelete"
        '        Me.CurrentPresenter.VirtualDelete(idCall)
        '    Case "recover"
        '        Me.CurrentPresenter.UnDelete(idCall)
        '    Case Else
        '        Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
        'End Select
    End Sub

    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.CurrentPresenter.LoadRevisions(Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadRevisions(Me.CurrentFilters, Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub

    Private Sub ListCalls_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub


    Private Sub BTNfindRevisions_Click(sender As Object, e As System.EventArgs) Handles BTNfindRevisions.Click
        Dim filters As dtoRevisionFilters = Me.CurrentFilters
        filters.SearchForName = Me.TXBusername.Text
        filters.Ascending = True
        filters.OrderBy = IIf(CurrentAction = CallStandardAction.List, RevisionOrder.ByDeadline, RevisionOrder.ByDate)
        filters.Status = RevisionStatus.None
        If Not IsNothing(Me.RBLstatus.Items.FindByValue(CInt(RevisionStatus.None))) Then
            Me.RBLstatus.SelectedValue = CInt(RevisionStatus.None)
        End If

        Me.CurrentPresenter.LoadRevisions(filters, 0, PreloadPageSize)
    End Sub

    Private Sub RBLstatus_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLstatus.SelectedIndexChanged
        Dim filters As dtoRevisionFilters = Me.CurrentFilters
        filters.SearchForName = Me.TXBusername.Text
        filters.Ascending = True
        filters.OrderBy = IIf(CurrentAction = CallStandardAction.List, RevisionOrder.ByDeadline, RevisionOrder.ByDate)
        filters.Status = Me.RBLstatus.SelectedValue
        Me.CurrentPresenter.LoadRevisions(filters, 0, PreloadPageSize)
    End Sub
End Class
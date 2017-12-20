Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract

Public Class PublicRequestsCollector
    Inherits PageBase
    Implements IViewPublicRequestsList



#Region "Context"
    Private _Presenter As PublicRequestsListPresenter
    Private ReadOnly Property CurrentPresenter() As PublicRequestsListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PublicRequestsListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewPublicRequestsList.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewPublicRequestsList.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("CommunityID")) Then
                Return CInt(Me.Request.QueryString("CommunityID"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private Property IdCallCommunity As Integer Implements IViewPublicRequestsList.IdCallCommunity
        Get
            Return ViewStateOrDefault("IdCallCommunity", CInt(0))
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("IdCallCommunity") = value
        End Set
    End Property
    Private Property Pager As lm.Comol.Core.DomainModel.PagerBase Implements IViewPublicRequestsList.Pager
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
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Calls", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnoCalls.Text = Resource.getValue("LBnoCalls." & CallForPaperType.RequestForMembership.ToString())
            .setLiteral(LTpageTop)
            .setLiteral(LTpageBottom)
            .setHyperLink(HYPtoLoginPage, True, True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub InitializeSkin(skin As lm.Comol.Core.DomainModel.Helpers.ExternalPageContext) Implements IViewBasePublicCallList(Of lm.Comol.Modules.CallForPapers.Domain.dtoRequestItemPermission).InitializeSkin
        Me.Master.InitializeMaster(skin)
    End Sub
    Private Sub SetContainerName(idCommunity As Integer, name As String) Implements IViewPublicRequestsList.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & CallStandardAction.List.ToString() & "." & CallForPaperType.RequestForMembership.ToString())
        Master.ServiceTitle = title
        If idCommunity = 0 Then
            Master.ServiceTitleToolTip = title
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & CallStandardAction.List.ToString() & "." & CallForPaperType.RequestForMembership.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub
    Private Sub SetActionUrl(url As String) Implements IViewPublicRequestsList.SetActionUrl
        Me.HYPtoLoginPage.Visible = True
        Me.HYPtoLoginPage.NavigateUrl = BaseUrl & url
    End Sub
    Private Sub LoadCalls(items As List(Of dtoRequestItemPermission)) Implements IViewPublicRequestsList.LoadCalls
        If items.Count = 0 Then
            Me.MLVresults.SetActiveView(VIWnoItems)
        Else
            Me.MLVresults.SetActiveView(VIWlist)
            Me.RPTcalls.DataSource = items
            Me.RPTcalls.DataBind()
        End If
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, ModuleCallForPaper.ActionType.NoPermission, , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        'Dim idCommunity As Integer = PreloadIdCommunity
        'Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        'Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        'dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        'dto.DestinationUrl = RootObject.ViewCalls(CallForPaperType.RequestForMembership, PreloadAction, idCommunity, PreLoadedContentView)
        'If idCommunity > 0 Then
        '    dto.IdCommunity = idCommunity
        'End If
        'webPost.Redirect(dto)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewPublicRequestsList.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
#End Region

    Private Sub RPTcalls_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcalls.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dto As dtoRequestItemPermission = DirectCast(e.Item.DataItem, dtoRequestItemPermission)

            If dto.Call.Status = CallForPaperStatus.SubmissionClosed Then
                Dim lockedInfo As Label = e.Item.FindControl("LBlocked")
                lockedInfo.Visible = True
                lockedInfo.ToolTip = Me.Resource.getValue("SubmissionClosed." & CallForPaperType.RequestForMembership.ToString())
            End If
            Dim oLiteral As Literal = e.Item.FindControl("LTname")
            oLiteral.Text = dto.Call.Name
            If Not String.IsNullOrEmpty(dto.Call.Edition) Then
                oLiteral = e.Item.FindControl("LTedition")
                oLiteral.Visible = True
                oLiteral.Text = dto.Call.Edition
            End If
            Dim oLabel As Label
            If dto.isNewItem Then
                oLabel = e.Item.FindControl("LBnewItem")
                oLabel.Visible = True
                oLabel.ToolTip = Me.Resource.getValue("NewItem." & CallForPaperType.RequestForMembership.ToString())
            End If
            If dto.isExpiringItem Then
                oLabel = e.Item.FindControl("LBexpiringItem")
                oLabel.Visible = True
                oLabel.ToolTip = Me.Resource.getValue("ExpiringItem." & CallForPaperType.RequestForMembership.ToString())
            End If

            oLabel = e.Item.FindControl("LBendDateTime_t")
            Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBendDate")
            If dto.Call.EndDate.HasValue Then
                oLabel.Text = FormatDateTime(dto.Call.EndDate, DateFormat.GeneralDate)
            Else
                oLabel.Text = Resource.getValue("NoEndDate")
            End If

            Dim oHyperlink As HyperLink = e.Item.FindControl("HTPcallInfo")
            oHyperlink.Visible = False
            Resource.setHyperLink(oHyperlink, CallForPaperType.RequestForMembership.ToString, True, True)

            oHyperlink = e.Item.FindControl("HTPsubmitPublicCall")
            oHyperlink.NavigateUrl = BaseUrl & RootObject.StartNewSubmission(CallForPaperType.RequestForMembership, dto.Id, True, True, CallStatusForSubmitters.SubmissionOpened, IdCallCommunity)
            oHyperlink.Visible = dto.Permission.Submit
            Resource.setHyperLink(oHyperlink, CallForPaperType.RequestForMembership.ToString, True, True)
        End If
    End Sub
    Private Sub PGgridBottom_OnPageSelected() Handles PGgridBottom.OnPageSelected
        Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub
    Private Sub PGgridTop_OnPageSelected() Handles PGgridTop.OnPageSelected
        Me.CurrentPresenter.LoadCalls(Me.Pager.PageIndex, Me.Pager.PageSize)
    End Sub



    Private Sub PublicRequestsCollector_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Me.Master.ShowDocType = True
    End Sub
End Class
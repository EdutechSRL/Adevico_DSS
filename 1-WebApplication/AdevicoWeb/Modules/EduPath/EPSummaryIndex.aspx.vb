Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Modules.EduPath.Domain.DTO
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Presentation

Public Class EPSummaryIndex
    Inherits PageBase
    Implements IViewSummaryIndex


#Region "Context"
    Private _Presenter As SummaryIndexPresenter
    Private ReadOnly Property CurrentPresenter() As SummaryIndexPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New SummaryIndexPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewSummaryIndex.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadSummaryType As SummaryType Implements IViewSummaryIndex.PreloadSummaryType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of SummaryType).GetByString(Request.QueryString("SummaryType"), lm.Comol.Modules.EduPath.Domain.SummaryType.PortalIndex)
        End Get
    End Property
    Private Property SummaryIdCommunity As Integer Implements IViewSummaryIndex.SummaryIdCommunity
        Get
            Return ViewStateOrDefault("SummaryIdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("SummaryIdCommunity") = value
        End Set
    End Property
    Private Property SummaryType As SummaryType Implements IViewSummaryIndex.SummaryType
        Get
            Return ViewStateOrDefault("SummaryType", SummaryType.CommunityIndex)
        End Get
        Set(value As SummaryType)
            Me.ViewState("SummaryType") = value
            Master.ServiceTitle = Resource.getValue("ServiceTitle." & value.ToString())
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

    End Sub

    Private Sub EPSummaryIndex_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub


#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        'MLVsummary.SetActiveView(VIWsummary)

        'setHref(Asummaryuser) = Me.BaseUrl + RootObject.SearchUsersForModule(Service.ModuleCode, "summaryIndex", CmntId)

        'setHref(Asummarycommunity) = Me.BaseUrl + RootObject.EpSummaryCommunity

        'setHref(Asummaryorganization) = Me.BaseUrl + RootObject.EpSummaryOrganization

        'setHref(Asummarypath) = Me.BaseUrl + RootObject.EpSummaryPath

        'Asummaryuser.Attributes("href") = Me.BaseUrl + RootObject.SearchUsersForModule(Service.ModuleCode, "summaryIndex", CmntId)

    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.Master.ServiceNopermission = Me.Resource.getValue("Error.NotPermission")
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Summary", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(LBsummaryDescription)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub DisplayNoPermission() Implements IViewSummaryIndex.DisplayNoPermission
        Me.BindNoPermessi()
    End Sub
    Private Sub DisplayWrongPageAccess() Implements IViewSummaryIndex.DisplayWrongPageAccess
        Me.Master.ServiceNopermission = Me.Resource.getValue("DisplayWrongPageAccess")
        Me.Master.ShowNoPermission = True
    End Sub

    Private Sub LoadAvailableItems(items As List(Of SummaryType)) Implements IViewSummaryIndex.LoadAvailableItems
        Me.MLVsummary.SetActiveView(VIWsummary)
        Me.RPTtypes.DataSource = items
        Me.RPTtypes.DataBind()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewSummaryIndex.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryIndex(PreloadSummaryType, PreloadIdCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub
#End Region

    Private Sub RPTtypes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtypes.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim st As SummaryType = DirectCast(e.Item.DataItem, SummaryType)
            Dim oLabel As Label = e.Item.FindControl("LBsummaryTypeIcon")
            oLabel.CssClass &= " " & st.ToString.ToLower()

            oLabel = e.Item.FindControl("LBsummaryTypeText")
            oLabel.Text = Resource.getValue("Name.SummaryType." & st.ToString())

            Dim oLiteral As Literal = e.Item.FindControl("LTdescription")
            oLiteral.Text = Resource.getValue("Description.SummaryType." & st.ToString())

            Dim oControl As HtmlAnchor = e.Item.FindControl("AsummaryType")

            Select Case st
                Case lm.Comol.Modules.EduPath.Domain.SummaryType.Community
                    oControl.HRef = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryCommunity(SummaryType, SummaryIdCommunity)
                Case lm.Comol.Modules.EduPath.Domain.SummaryType.Organization
                    oControl.HRef = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryOrganization(SummaryType, SummaryIdCommunity)
                Case lm.Comol.Modules.EduPath.Domain.SummaryType.Path
                    oControl.HRef = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryPath(SummaryType, SummaryIdCommunity)
                Case lm.Comol.Modules.EduPath.Domain.SummaryType.User
                    oControl.HRef = BaseUrl & lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SearchUsersForModule(SummaryType, SummaryIdCommunity)
                Case Else
                    oControl.HRef = "#"
            End Select
        End If
    End Sub

    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType) Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseSummary.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub
End Class
Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain

Public Class EPCertificationList
    Inherits PageBase
    Implements IViewCertificationList




    Private _Presenter As CertificationListPresenter

    Private ReadOnly Property CurrentPresenter() As CertificationListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CertificationListPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack Then
        Else

        End If
    End Sub

    Private Sub EPCertificationList_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub

    Public Sub DisplayNoPermission() Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseEduPath.DisplayNoPermission
        Me.BindNoPermessi()
    End Sub

    Public Sub DisplaySessionTimeout() Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseEduPath.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = "" 'lm.Comol.Modules.EduPath.BusinessLogic.RootObject.SummaryIndex(PreloadSummaryType, PreloadIdCommunity)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Public Sub DisplayWrongPageAccess() Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseEduPath.DisplayWrongPageAccess
        Me.Master.ServiceNopermission = Me.Resource.getValue("DisplayWrongPageAccess")
        Me.Master.ShowNoPermission = True
    End Sub

    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Modules.EduPath.Domain.ModuleEduPath.ActionType) Implements lm.Comol.Modules.EduPath.Presentation.IViewBaseEduPath.SendUserAction
        Me.PageUtility.AddActionToModule(idCommunity, idModule, action, , InteractionType.UserWithLearningObject)
    End Sub

    Dim _certificates As IList(Of SubActivity)
    Public Property Certificates As System.Collections.Generic.IList(Of lm.Comol.Modules.EduPath.Domain.SubActivity) Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.Certificates
        Get
            '_certificates = ViewStateOrDefault("certificates", _certificates)
            Return _certificates
        End Get
        Set(value As System.Collections.Generic.IList(Of lm.Comol.Modules.EduPath.Domain.SubActivity))
            _certificates = value

            'ViewState("certificates") = _certificates



        End Set
    End Property

    Dim _certificateStats As IList(Of dtoCertificateStat)

    Public Property CertificateStats As System.Collections.Generic.IList(Of lm.Comol.Modules.EduPath.Domain.dtoCertificateStat) Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.CertificateStats
        Get
            Return _certificateStats

        End Get
        Set(value As System.Collections.Generic.IList(Of lm.Comol.Modules.EduPath.Domain.dtoCertificateStat))
            _certificateStats = value
            RPTcertifications.DataSource = _certificateStats
            RPTcertifications.DataBind()

        End Set
    End Property

    Public Property CurrentIdPath As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.CurrentIdPath
        Get
            Return ViewStateOrDefault("CurrentIdPath", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdPath") = value
        End Set
    End Property

    Public Property CurrentPathIdCommunity As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.CurrentPathIdCommunity
        Get
            Return ViewStateOrDefault("CurrentPathIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentPathIdCommunity") = value
        End Set
    End Property

    Public Sub InitializeFilters() Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.InitializeFilters

        DDLCertificateFilters.DataSource = Certificates
        DDLCertificateFilters.DataValueField = "IdCertificate"
        DDLCertificateFilters.DataTextField = "Name"
        DDLCertificateFilters.DataBind()
        If Certificates.Count > 1 Then
            Me.DDLCertificateFilters.Items.Insert(0, New ListItem(Me.Resource.getValue("NONE"), -1))
            FilteredCertificate = -1
        End If

        Dim myroles As List(Of Comol.Entity.Role) = COL_BusinessLogic_v2.CL_permessi.COL_TipoRuolo.List(PageUtility.LinguaID).OrderBy(Function(t) t.Name).ToList
        Dim roleTranslations As Dictionary(Of Integer, String) = Roles.ToDictionary(Function(r) r.Id, Function(r) r.Name)

        DDLRolesFilters.DataSource = (From item In Roles Select New With {.Id = item.Id, .Name = roleTranslations(item.Id)}).ToList()
        DDLRolesFilters.DataValueField = "Id"
        DDLRolesFilters.DataTextField = "Name"
        DDLRolesFilters.DataBind()
        If Roles.Count > 1 Then
            Me.DDLRolesFilters.Items.Insert(0, New ListItem(Me.Resource.getValue("NONE"), -1))
            FilteredRole = -1
        End If

    End Sub

    Public ReadOnly Property PreloadIdCommunity As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadIdPath As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.PreloadIdPath
        Get
            If IsNumeric(Me.Request.QueryString("idPath")) Then
                Return CInt(Me.Request.QueryString("idPath"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        Me.MLVcertificationList.SetActiveView(VIWcertificationList)
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
        With Me.Resource
            .setLabel(LBcertificateNameHeader)
            .setLabel(LBfilterEndDateTitle)
            .setLabel(LBfilterStartDateTitle)
            .setLabel(LBLempty)
            .setLabel(LBparticipantsStatHeader)
            .setLabel(LBfilterCertificateTitle)
            .setLabel(LBfilterCommunityRoleTitle)
            .setCheckBox(CHBshowall)
            .setButton(BTNupdate)
            LBserviceCertificationList.Text = .getValue("ServiceTitle.CertificationList")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Dim _filteredCertificate As Long
    Public Property FilteredCertificate As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.FilteredCertificate
        Get
            If (DDLCertificateFilters.SelectedIndex > 0) Then
                _filteredCertificate = DDLCertificateFilters.SelectedValue
            End If

            Return _filteredCertificate
        End Get
        Set(value As Long)
            _filteredCertificate = value
        End Set
    End Property

    Private Sub DDLCertificateFilters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLCertificateFilters.SelectedIndexChanged
        FilteredCertificate = DDLCertificateFilters.SelectedValue

    End Sub

    Private Sub BTNupdate_Click(sender As Object, e As System.EventArgs) Handles BTNupdate.Click
        CurrentPresenter.UpdateFilters()
    End Sub

    Private Sub RPTcertifications_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcertifications.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoCertificateStat = e.Item.DataItem


            Dim olbl As Label
            'olbl = e.Item.FindControl("LBcertificateName")
            'olbl.Text = dto.CertificateName

            Dim ohyp As HyperLink
            ohyp = e.Item.FindControl("HYPcertificateName")
            ohyp.Text = dto.CertificateName

            ohyp.NavigateUrl = Me.BaseUrl + RootObject.EPCertification(CurrentPathIdCommunity, CurrentIdPath, dto.CertificateId)

            olbl = e.Item.FindControl("LBparticipants")
            olbl.Text = String.Format("{0} / {1}", dto.Obtained, dto.Total)

        End If

    End Sub

    Public Property Empty As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.Empty
        Get
            Return TRempty.Visible
        End Get
        Set(value As Boolean)
            TRempty.Visible = value
        End Set
    End Property

    Dim _onlyone As Boolean
    Public Property OnlyOne As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.OnlyOne
        Get
            Return _onlyone
        End Get
        Set(value As Boolean)
            _onlyone = value
            If (_onlyone) Then
                Response.Redirect(Me.BaseUrl + RootObject.EPCertification(CurrentPathIdCommunity, CurrentIdPath, CertificateStats.First().CertificateId))
            End If
        End Set
    End Property

    Public Property EndDate As Date? Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.EndDate
        Get
            Return RDPEndDate.SelectedDate
        End Get
        Set(value As Date?)
            If (value.HasValue) Then
                RDPEndDate.SelectedDate = value.Value
            Else
                RDPEndDate.Clear()
            End If

        End Set
    End Property

    Public Property StartDate As Date? Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.StartDate
        Get
            Return RDPStartDate.SelectedDate
        End Get
        Set(value As Date?)
            If (value.HasValue) Then
                RDPStartDate.SelectedDate = value.Value
            Else
                RDPStartDate.Clear()
            End If
        End Set
    End Property

    Public Property AllStatistics As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.AllStatistics
        Get
            Return CHBshowall.Checked
        End Get
        Set(value As Boolean)
            CHBshowall.Checked = value
        End Set
    End Property

    Private Sub CHBallstatistics_CheckedChanged(sender As Object, e As System.EventArgs) Handles CHBshowall.CheckedChanged
        'BindDati()
        CurrentPresenter.UpdateFilters()
    End Sub

    Dim _roles As IList(Of lm.Comol.Core.DomainModel.Role)
    Public Property Roles As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.Role) Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.Roles
        Get
            Return _roles
        End Get
        Set(value As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.Role))
            _roles = value
        End Set
    End Property

    Dim _filteredrole As Int32
    Public Property FilteredRole As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationList.FilteredRole
        Get
            If (DDLRolesFilters.SelectedIndex > 0) Then
                _filteredrole = DDLRolesFilters.SelectedValue
            End If

            Return _filteredrole
        End Get
        Set(value As Integer)
            _filteredrole = value
        End Set
    End Property

    Private Sub DDLRolesFilters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLRolesFilters.SelectedIndexChanged
        FilteredRole = DDLRolesFilters.SelectedValue
    End Sub
End Class
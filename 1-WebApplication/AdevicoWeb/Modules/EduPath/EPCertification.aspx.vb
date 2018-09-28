Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Core.DomainModel
Public Class EPCertification
    Inherits EPlitePageBaseEduPath
    Implements IViewCertification

#Region "Context"
    Private _Presenter As CertificationPresenter
    Private ReadOnly Property CurrentPresenter() As CertificationPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CertificationPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service
    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property AllStatistics As Boolean Implements IViewCertification.AllStatistics
        Get
            Return CHBshowall.Checked
        End Get
        Set(value As Boolean)
            CHBshowall.Checked = value
        End Set
    End Property
    Dim _certificates As IList(Of SubActivity)
    Public Property Certificates As System.Collections.Generic.IList(Of lm.Comol.Modules.EduPath.Domain.SubActivity) Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.Certificates
        Get
            '_certificates = ViewStateOrDefault("certificates", _certificates)
            Return _certificates
        End Get
        Set(value As System.Collections.Generic.IList(Of lm.Comol.Modules.EduPath.Domain.SubActivity))
            _certificates = value
            'ViewState("certificates") = _certificates
        End Set
    End Property
    Private Property CurrentIdPath As Long Implements IViewCertification.CurrentIdPath
        Get
            Return ViewStateOrDefault("CurrentIdPath", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdPath") = value
        End Set
    End Property
    Private Property CurrentPathIdCertificate As Long Implements IViewCertification.CurrentPathIdCertificate
        Get
            Return ViewStateOrDefault("CurrentPathIdCertificate", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentPathIdCertificate") = value
        End Set
    End Property
    Private Property CurrentPathIdCommunity As Integer Implements IViewCertification.CurrentPathIdCommunity
        Get
            Return ViewStateOrDefault("CurrentPathIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentPathIdCommunity") = value
        End Set
    End Property
    Private Property Empty As Boolean Implements IViewCertification.Empty
        Get
            Return TRempty.Visible
        End Get
        Set(value As Boolean)
            TRempty.Visible = value
        End Set
    End Property

    Dim _filteredCertificate As Long
    Public Property FilteredCertificate As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.FilteredCertificate
        Get
            If DDLCertificateFilters.SelectedIndex > 0 Then
                _filteredCertificate = DDLCertificateFilters.SelectedValue
            End If

            Return _filteredCertificate
        End Get
        Set(value As Long)
            _filteredCertificate = value
        End Set
    End Property

    Dim _filteredrole As Int32
    Public Property FilteredRole As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.FilteredRole
        Get
            If DDLRolesFilters.SelectedIndex > 0 Then
                _filteredrole = DDLRolesFilters.SelectedValue
            End If

            Return _filteredrole
        End Get
        Set(value As Integer)
            _filteredrole = value
        End Set
    End Property
    Public ReadOnly Property PreloadIdCertificate As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.PreloadIdCertificate
        Get
            If IsNumeric(Me.Request.QueryString("idCertificate")) Then
                Return CInt(Me.Request.QueryString("idCertificate"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadIdCommunity As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadIdPath As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.PreloadIdPath
        Get
            If IsNumeric(Me.Request.QueryString("idPath")) Then
                Return CInt(Me.Request.QueryString("idPath"))
            Else
                Return 0
            End If
        End Get
    End Property

    Dim _roles As IList(Of lm.Comol.Core.DomainModel.Role)
    Public Property Roles As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.Role) Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.Roles
        Get
            Return _roles
        End Get
        Set(value As System.Collections.Generic.IList(Of lm.Comol.Core.DomainModel.Role))
            _roles = value
        End Set
    End Property

    Public Property EndDate As Date? Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.EndDate
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

    Public Property StartDate As Date? Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.StartDate
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

    Dim _certificateStats As dtoCertificateStat

    Public Property CertificateStat As lm.Comol.Modules.EduPath.Domain.dtoCertificateStat Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.CertificateStat
        Get
            Return _certificateStats
        End Get
        Set(value As lm.Comol.Modules.EduPath.Domain.dtoCertificateStat)
            _certificateStats = value

            ViewState("subactivity") = value.dtoSubActivity

            LBcertificateName.Text = _certificateStats.CertificateName

            RPTcertifications.DataSource = value.Users
            RPTcertifications.DataBind()
        End Set
    End Property
    Private Property IsPathManager As Boolean Implements IViewCertification.IsPathManager
        Get
            Return ViewStateOrDefault("IsPathManager", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPathManager") = value
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

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        Me.MLVcertification.SetActiveView(VIWcertification)
        HYPback.NavigateUrl = BaseUrl + RootObject.EPCertificationList(CurrentPathIdCommunity, CurrentIdPath, IsMoocPath)
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
        'If PreloadIsMooc Then
        '    MyBase.SetCulture("pg_MoocsSummary", "EduPath")
        'Else
        MyBase.SetCulture("pg_Summary", "EduPath")
        'End If
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            '.setLabel(LBcertificateNameHeader)
            .setLabel(LBfilterEndDateTitle)
            .setLabel(LBfilterStartDateTitle)
            .setLabel(LBLempty)
            '.setLabel(LBparticipantsStatHeader)
            .setLabel(LBfilterCertificateTitle)
            .setLabel(LBfilterCommunityRoleTitle)
            .setLabel(LBfilterUserTitle)

            .setLabel(LBusernameHeader)
            .setLabel(LBdateHeader)
            .setLabel(LBactionsHeader)
            .setHyperLink(HYPback, False, False)

            .setCheckBox(CHBshowall)
            .setButton(BTNupdate)
            LBserviceCertification.Text = .getValue("ServiceTitle.Certification")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
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
#End Region

  

    Private Sub EPCertification_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub

  

   
    Public Sub InitializeFilters() Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.InitializeFilters
        DDLCertificateFilters.DataSource = Certificates
        DDLCertificateFilters.DataValueField = "IdCertificate"
        DDLCertificateFilters.DataTextField = "Name"
        DDLCertificateFilters.DataBind()

        DDLCertificateFilters.SelectedValue = CurrentPathIdCertificate
        If Certificates.Count = 0 Then
            Me.DDLCertificateFilters.Items.Add(New ListItem(Me.Resource.getValue("NONE"), -1))
        End If
        'If Certificates.Count > 1 Then
        '    Me.DDLCertificateFilters.Items.Insert(0, New ListItem(Me.Resource.getValue("NONE"), -1))
        '    FilteredCertificate = -1
        'End If

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

  

    Private Sub RPTcertifications_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcertifications.ItemCommand
        CTRLmessages.Visible = False
        Select Case e.CommandName
            'Case "certification"
            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            '    Dim viewdate As DateTime = DateTime.Now

            '    If (Not EndDate Is Nothing AndAlso EndDate.HasValue) Then
            '        viewdate = EndDate.Value
            '    End If

            '    control.DownloadCertification(CurrentIdPath, CInt(e.CommandArgument), ViewState("subactivity"), viewdate, Me.HDNdownloadTokenValue.Value)
            'Case "refreshcertification"
            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            '    control.RestoreCertificate(CurrentIdPath, CInt(e.CommandArgument), ViewState("subactivity"), Me.HDNdownloadTokenValue.Value)
        End Select
    End Sub

    Private Sub RPTcertifications_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcertifications.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoCertificateUsersStat = e.Item.DataItem

            Dim olbl As Label
            Dim ohyp As HyperLink

            ohyp = e.Item.FindControl("HYPusername")
            ohyp.Text = dto.Person.SurnameAndName
            ohyp.NavigateUrl = Me.BaseUrl + RootObject.EPCertificationUser(CurrentPathIdCommunity, CurrentIdPath, dto.PersonId, CurrentPathIdCertificate)


            olbl = e.Item.FindControl("LBdate")
            olbl.Text = dto.Date.ToShortDateString() + " " + dto.Date.ToShortTimeString()

            Dim initializer As New dtoInternalActionInitializer
            initializer.IdPath = CurrentIdPath
            initializer.IdPathCommunity = CurrentPathIdCommunity
            initializer.CookieName = CookieName
            initializer.SubActivity = dto.Parent.dtoSubActivity

            initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.none

            Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            'control.Visible = False
            control.EvaluablePath = False
            Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
            'oCertAction.RefreshContainer = True
            ' DIMENSIONI IMMAGINI
            oCertAction.IconSize = Helpers.IconSize.Small

            Dim actions As List(Of dtoModuleActionControl) = oCertAction.InitializeRemoteControl(dto.PersonId, initializer, StandardActionType.None)
            Dim actStat As dtoModuleActionControl = actions.Where(Function(x) x.ControlType = StandardActionType.ViewPersonalStatistics).FirstOrDefault()


            Dim viewdate As DateTime = DateTime.Now
            If (Not EndDate Is Nothing AndAlso EndDate.HasValue) Then
                viewdate = EndDate.Value
            End If
            If (Not actStat Is Nothing) Then
                'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBcertificate")
                'Me.Resource.setLinkButton(oLinkButton, False, True)
                'oLinkButton.Visible = True
                'oLinkButton.CommandArgument = dto.PersonId

                'oLinkButton = e.Item.FindControl("LNBrefreshCertificate")
                'Me.Resource.setLinkButton(oLinkButton, False, True)
                'oLinkButton.Visible = IsPathManager
                'oLinkButton.CommandArgument = dto.PersonId

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = True
                If IsPathManager Then
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(viewdate.Ticks, dto.Parent.PathId, dto.Parent.ActivityId, dto.Parent.SubActivityId, dto.PersonId)
                Else
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationUserDownloadPage(viewdate.Ticks, dto.Parent.PathId, dto.Parent.ActivityId, dto.Parent.SubActivityId, dto.PersonId)
                End If
                oHyperlink = e.Item.FindControl("HYPrefreshCertificate")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = IsPathManager

                '(PathID, UserToViewStat, ServiceEP.GetDtoSubActivity(ItemId), Me.HDNdownloadTokenValue.Value)
                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & ServiceEP.GetCertificationRestoreUrl(dto.Parent.PathId, dto.Parent.ActivityId, dto.Parent.SubActivityId, dto.PersonId)



            ElseIf IsPathManager Then
                'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBexportCertification")
                'Me.Resource.setLinkButton(oLinkButton, False, True)
                'oLinkButton.Visible = True
                'oLinkButton.CommandArgument = dto.PersonId

              
                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(viewdate.Ticks, dto.Parent.PathId, dto.Parent.ActivityId, dto.Parent.SubActivityId, dto.PersonId)

            End If

        End If
    End Sub
    Protected Sub ItemActionResult(err As lm.Comol.Core.Certifications.CertificationError, ByVal savingRequired As Boolean, ByVal saved As Boolean)
        BindDati()
        Dim tString As String = String.Format("CertificationError.{0}.savingRequired.{1}.saved.{2}", err.ToString, savingRequired, saved)
        CTRLmessages.Visible = True
        Select Case err
            Case lm.Comol.Core.Certifications.CertificationError.None
                CTRLmessages.InitializeControl(Resource.getValue(tString), Helpers.MessageType.success)
            Case lm.Comol.Core.Certifications.CertificationError.TransmittingFile
                CTRLmessages.InitializeControl(Resource.getValue(tString), Helpers.MessageType.alert)
            Case Else
                CTRLmessages.InitializeControl(Resource.getValue(tString), Helpers.MessageType.error)
        End Select
    End Sub
    Protected Sub GetHiddenIdentifierValueEvent(ByRef value As String)
        CTRLmessages.Visible = False
        value = Me.HDNdownloadTokenValue.Value
    End Sub
    Protected ReadOnly Property DisplayMessageToken() As String
        Get
            Return Resource.getValue("DisplayMessageToken.EPCertification")
        End Get
    End Property
    Protected ReadOnly Property DisplayTitleToken() As String
        Get
            Return Resource.getValue("DisplayTitleToken.EPCertification")
        End Get
    End Property

    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_EPCertification"
        End Get
    End Property

    Dim _filtereduser As String
    Public Property FilteredUser As String Implements lm.Comol.Modules.EduPath.Presentation.IViewCertification.FilteredUser
        Get
            _filtereduser = TXBuserFilter.Text
            Return _filtereduser
        End Get
        Set(value As String)
            TXBuserFilter.Text = value
            _filtereduser = value
        End Set
    End Property

    Private Sub BTNupdate_Click(sender As Object, e As System.EventArgs) Handles BTNupdate.Click
        CTRLmessages.Visible = False
        CurrentPresenter.UpdateFilters()
    End Sub
End Class
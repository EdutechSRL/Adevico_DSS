Imports lm.Comol.Modules.EduPath.BusinessLogic
Imports lm.Comol.Modules.EduPath.Presentation
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.Core.DomainModel

Public Class EPUserCertification
    Inherits EPlitePageBaseEduPath
    Implements IViewCertificationUser

#Region "Context"
    Private _Presenter As CertificationUserPresenter
    Private ReadOnly Property CurrentPresenter() As CertificationUserPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CertificationUserPresenter(Me.PageUtility.CurrentContext, Me)
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
#Region "Preload"
    Public ReadOnly Property PreloadIdCommunity As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadIdPath As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.PreloadIdPath
        Get
            If IsNumeric(Me.Request.QueryString("idPath")) Then
                Return CInt(Me.Request.QueryString("idPath"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadIdCertificate As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.PreloadIdCertificate
        Get
            If IsNumeric(Me.Request.QueryString("IdCertificate")) Then
                Return CInt(Me.Request.QueryString("IdCertificate"))
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreloadIdUser As Int32 Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.PreloadIdUser
        Get
            If IsNumeric(Me.Request.QueryString("idUser")) Then
                Return CInt(Me.Request.QueryString("idUser"))
            Else
                Return PageUtility.CurrentUser.ID
            End If
        End Get
    End Property

#End Region
    Private _certificatesStas As dtoCertificatesUserStat

    Public Property CertificatesStats As lm.Comol.Modules.EduPath.Domain.dtoCertificatesUserStat Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.CertificatesStats
        Get
            Return _certificatesStas

        End Get
        Set(value As lm.Comol.Modules.EduPath.Domain.dtoCertificatesUserStat)
            _certificatesStas = value

            LBuserInfo.Text = _certificatesStas.Person.SurnameAndName
            LBpathInfo.Text = _certificatesStas.PathName
            RPTcertificates.DataSource = value.Certificates
            RPTcertificates.DataBind()
        End Set
    End Property

    Public Property CurrentIdPath As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.CurrentIdPath
        Get
            Return ViewStateOrDefault("CurrentIdPath", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdPath") = value
        End Set
    End Property

    Public Property CurrentIdCertificate As Long Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.CurrentIdCertificate
        Get
            Return ViewStateOrDefault("CurrentIdCertificate", 0)
        End Get
        Set(value As Long)
            ViewState("CurrentIdCertificate") = value
        End Set
    End Property

    Public Property CurrentPathIdCommunity As Integer Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.CurrentPathIdCommunity
        Get
            Return ViewStateOrDefault("CurrentPathIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentPathIdCommunity") = value
        End Set
    End Property

    Private Property CurrentIdUser As Integer Implements IViewCertificationUser.CurrentIdUser
        Get
            Return ViewStateOrDefault("CurrentIdUser", 0)
        End Get
        Set(value As Integer)
            ViewState("CurrentIdUser") = value
        End Set
    End Property
    Private Property IsPathManager As Boolean Implements IViewCertificationUser.isPathManager
        Get
            Return ViewStateOrDefault("IsPathManager", False)
        End Get
        Set(value As Boolean)
            ViewState("IsPathManager") = value
        End Set
    End Property


    Public Property Empty As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.Empty
        Get
            Return TRempty.Visible
        End Get
        Set(value As Boolean)
            TRempty.Visible = value
        End Set
    End Property

    Public Property EndDate As Date? Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.EndDate
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

    Public Property StartDate As Date? Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.StartDate
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
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Page/Control"
    Protected ReadOnly Property CookieName() As String
        Get
            Return "COMOL_EPUserCertification"
        End Get
    End Property
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
#End Region


   


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

   

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
        MLVcertification.SetActiveView(VIWcertification)
        If (CurrentIdCertificate > 0) Then
            HYPback.NavigateUrl = BaseUrl + RootObject.EPCertification(CurrentPathIdCommunity, CurrentIdPath, CurrentIdCertificate, IsMoocPath)
        Else
            HYPback.NavigateUrl = BaseUrl + RootObject.EPCertificationList(CurrentPathIdCommunity, CurrentIdPath, IsMoocPath)
        End If

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
            ''.setLabel(LBparticipantsStatHeader)
            '.setLabel(LBfilterCertificateTitle)
            '.setLabel(LBfilterCommunityRoleTitle)
            '.setLabel(LBfilterUserTitle)

            '.setLabel(LBusernameHeader)
            .setLabel(LBdateHeader)
            .setLabel(LBactionsHeader)
            .setHyperLink(HYPback, False, False)

            '.setCheckBox(CHBshowall)
            .setButton(BTNupdate)
            LBserviceCertification.Text = .getValue("ServiceTitle.CertificationUser")
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

   
    Private Sub EPUserCertification_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
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

    'Public Property AllStatistics As Boolean Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.AllStatistics
    '    Get

    '    End Get
    '    Set(value As Boolean)

    '    End Set
    'End Property

  

    Public Sub InitializeFilters() Implements lm.Comol.Modules.EduPath.Presentation.IViewCertificationUser.InitializeFilters

    End Sub



    Private Sub BTNupdate_Click(sender As Object, e As System.EventArgs) Handles BTNupdate.Click
        CTRLmessages.visible = False
        CurrentPresenter.UpdateFilters()
    End Sub

    Private Sub RPTcertificates_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcertificates.ItemCommand
        CTRLmessages.Visible = False
        'Dim sa As dtoSubActivity = Nothing
        'Try
        '    If Not IsNothing(ViewState("subactivity-" + e.CommandArgument)) AndAlso TypeOf ViewState("subactivity-" + e.CommandArgument) Is dtoSubActivity Then
        '        sa = ViewState("subactivity-" + e.CommandArgument)
        '    End If
        'Catch ex As Exception

        'End Try
        'If Not IsNothing(sa) Then
        '    Select Case e.CommandName
        '        Case "certification"
        '            Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")

        '            Dim viewdate As DateTime = DateTime.Now

        '            If (Not EndDate Is Nothing AndAlso EndDate.HasValue) Then
        '                viewdate = EndDate.Value
        '            End If
        '            control.DownloadCertification(CurrentIdPath, CurrentIdUser, sa, viewdate, Me.HDNdownloadTokenValue.Value)
        '            'Case "refreshcertification"
        '            '    Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
        '            '    control.RestoreCertificate(CurrentIdPath, CurrentIdUser, sa, Me.HDNdownloadTokenValue.Value)
        '    End Select
        'Else
        '    ItemActionResult(lm.Comol.Core.Certifications.CertificationError.Unknown, False, False)
        'End If
    End Sub

    Private Sub RPTcertificates_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcertificates.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoCertificateUserStat = e.Item.DataItem

            Dim olbl As Label

            olbl = e.Item.FindControl("LBcertificate")
            olbl.Text = dto.CertificateName

            olbl = e.Item.FindControl("LBdate")
            olbl.Text = dto.CompletedOn.ToShortDateString

            Dim lnb As HyperLink
            lnb = e.Item.FindControl("HYPcertificate")
            lnb.Text = dto.CertificateName
            lnb.NavigateUrl = BaseUrl + RootObject.EPCertification(dto.CommunityId, dto.PathId, dto.CertificateId, IsMoocPath)

            Dim initializer As New dtoInternalActionInitializer
            initializer.IdPath = CurrentIdPath
            initializer.IdPathCommunity = CurrentPathIdCommunity
            initializer.CookieName = CookieName
            initializer.SubActivity = dto.SubActivity

            initializer.Display = lm.Comol.Core.ModuleLinks.DisplayActionMode.none

            Dim control As UC_CertificationAction = e.Item.FindControl("CTRLcertificationAction")
            'control.Visible = False
            control.EvaluablePath = False
            Dim oCertAction As lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction = e.Item.FindControl("CTRLcertificationAction")
            'oCertAction.RefreshContainer = True
            ' DIMENSIONI IMMAGINI
            oCertAction.IconSize = Helpers.IconSize.Small

            Dim actions As List(Of dtoModuleActionControl) = oCertAction.InitializeRemoteControl(CurrentIdUser, initializer, StandardActionType.None)
            Dim actStat As dtoModuleActionControl = actions.Where(Function(x) x.ControlType = StandardActionType.ViewPersonalStatistics).FirstOrDefault()

            Dim viewdate As DateTime = DateTime.Now
            If (Not EndDate Is Nothing AndAlso EndDate.HasValue) Then
                viewdate = EndDate.Value
            End If

            If (Not actStat Is Nothing) Then
                'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBcertificate")
                'Me.Resource.setLinkButton(oLinkButton, False, True)
                'oLinkButton.Visible = True
                'oLinkButton.CommandArgument = dto.SubActivityId.ToString()

                'ViewState("subactivity-" + oLinkButton.CommandArgument) = dto.SubActivity

                'oLinkButton = e.Item.FindControl("LNBrefreshCertificate")
                'oLinkButton.CommandArgument = dto.SubActivityId.ToString()
                'Me.Resource.setLinkButton(oLinkButton, False, True)
                'oLinkButton.Visible = IsPathManager

                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = True
                If IsPathManager Then
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(viewdate.Ticks, dto.PathId, dto.ActivityId, dto.SubActivityId, CurrentIdUser)
                Else
                    oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationUserDownloadPage(viewdate.Ticks, dto.PathId, dto.ActivityId, dto.SubActivityId, CurrentIdUser)
                End If
                oHyperlink = e.Item.FindControl("HYPrefreshCertificate")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = IsPathManager

                '(PathID, UserToViewStat, ServiceEP.GetDtoSubActivity(ItemId), Me.HDNdownloadTokenValue.Value)
                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & ServiceEP.GetCertificationRestoreUrl(dto.PathId, dto.ActivityId, dto.SubActivityId, CurrentIdUser)

            ElseIf IsPathManager Then
                'Dim oLinkButton As LinkButton = e.Item.FindControl("LNBexportCertification")
                'Me.Resource.setLinkButton(oLinkButton, False, True)
                'oLinkButton.Visible = True
                Dim oHyperlink As HyperLink = e.Item.FindControl("HYPexportCertification")
                Resource.setHyperLink(oHyperlink, False, True)
                oHyperlink.Visible = True
                oHyperlink.NavigateUrl = PageUtility.ApplicationUrlBase & RootObject.CertificationManagerDownloadPage(viewdate.Ticks, dto.PathId, dto.ActivityId, dto.SubActivityId, CurrentIdUser)

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
End Class
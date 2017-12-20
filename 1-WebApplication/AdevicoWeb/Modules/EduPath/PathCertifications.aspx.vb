Imports lm.Comol.Modules.EduPath.Domain

Public Class PathCertifications
    Inherits PageBaseEduPath

#Region "Page property"
    Private ReadOnly Property PathID() As Long
        Get
            If IsNumeric(Request.QueryString("PId")) Then
                Return Request.QueryString("PId")
            ElseIf IsNumeric(Request.QueryString("saId")) Then
                Return ServiceEP.GetActivity(Request.QueryString("saId")).Path.Id
            Else
                Return -1
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return Me.CurrentContext.UserContext.CurrentCommunityID
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property
    Private ReadOnly Property CurrentCommRoleID As Integer
        Get
            Return UtenteCorrente.GetIDRuoloForComunita(CurrentCommunityID)
        End Get
    End Property

    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get
            If _PathType = lm.Comol.Modules.EduPath.Domain.EPType.None Then
                _PathType = ServiceEP.GetEpType(PathID, ItemType.Path)
            End If
            Return _PathType
        End Get
    End Property
#End Region

    Public Enum PathCertificationView
        Certificates
        Users
        User
    End Enum

    Public Property View As PathCertificationView
        Get
            Dim query As String = Request.QueryString("View")
            If (String.IsNullOrEmpty(query)) Then

                Dim nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString())
                nameValues.Set("View", PathCertificationView.Certificates.ToString())
                Dim url As String = Request.Url.AbsolutePath
                Dim updatedQueryString As String = "?" + nameValues.ToString()
                Response.Redirect(url + updatedQueryString)

            End If

            Return [Enum].Parse(GetType(PathCertificationView), query)
        End Get
        Set(value As PathCertificationView)
            Dim nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString())
            nameValues.Set("View", PathCertificationView.Certificates.ToString())
            Dim url As String = Request.Url.AbsolutePath
            Dim updatedQueryString As String = "?" + nameValues.ToString()
            Response.Redirect(url + updatedQueryString)
        End Set
    End Property


#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Select Case View
            Case PathCertificationView.Certificates


                RPTCertificates.DataSource = ServiceEP.GetSubactiviesByPathIdAndTypes(PathID, SubActivityType.Certificate)
                RPTCertificates.DataBind()

                MLVpathCertifications.SetActiveView(VIWcertificates)
            Case PathCertificationView.Users

                MLVpathCertifications.SetActiveView(VIWusers)
            Case PathCertificationView.User

                MLVpathCertifications.SetActiveView(VIWuser)
        End Select
    End Sub

    Public Overrides Sub BindNoPermessi()
        MLVpathCertifications.SetActiveView(VIWerror)
        LBerror.Text = "Errore"
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Dim ok As Boolean = False

        ok = ServiceEP.HasPermessi_ByItem(Of Path)(PathID, CurrentCommunityID) And ServiceEP.GetUserPermission_ByPath(PathID, CurrentUserId, CurrentCommRoleID).ViewUserStat

        Return ok
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Stat", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region


#Region ""
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(COL_BusinessLogic_v2.UCServices.Services_EduPath.Codex)
        '  CouldActivityWithSingleSubActivityBeOmitted = True 'EduPathConfiguration.UseSingleActionView
        Me.Master.ShowDocType = True
    End Sub
    Private Sub ShowError(ByVal errorType As EpError)
        'Me.MLVpathCertifications.SetActiveView(Me.VIWerror)
        'Me.Resource.setHyperLink(Me.HYPerror, False, True)
        'Me.HYPerror.NavigateUrl = Me.BaseUrl & RootObject.EduPathList(Me.CurrentCommunityID, Me.ViewModeType)
        'Select Case errorType
        '    Case EpError.Generic
        '        Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Generic.ToString)
        '        Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
        '    Case EpError.NotPermission
        '        Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.NotPermission.ToString)
        '        Me.PageUtility.AddAction(Services_EduPath.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
        '    Case EpError.Url
        '        Me.LBerror.Text = Me.Resource.getValue("Error." & EpError.Url.ToString)
        '        Me.PageUtility.AddAction(Services_EduPath.ActionType.GenericError, Nothing, InteractionType.UserWithLearningObject)
        'End Select

    End Sub
#End Region


    Private Sub RPTCertificates_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTCertificates.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

        End If
    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)

    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)

    End Sub
    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return False
        End Get
    End Property
End Class
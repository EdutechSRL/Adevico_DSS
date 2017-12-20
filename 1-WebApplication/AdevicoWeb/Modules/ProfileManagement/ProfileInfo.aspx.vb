Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication


Public Class ProfileInfo
    Inherits PageBase
    Implements IViewProfileInfo


#Region "Implements"
    Public Property idDefaultProvider As Long Implements IViewProfileInfo.idDefaultProvider
        Get
            Return ViewStateOrDefault("idDefaultProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("idDefaultProvider") = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewProfileInfo.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewProfileInfo.IdProfileType
        Get
            Return ViewStateOrDefault("IdProfileType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedIdProfile As Integer Implements IViewProfileInfo.PreloadedIdProfile
        Get
            If IsNumeric(Request.QueryString("IdUser")) Then
                Return CInt(Request.QueryString("IdUser"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property SelectedTab As ProfileInfoTab Implements IViewProfileInfo.SelectedTab
        Get
            If Me.TBSprofile.TabIndex >= 0 Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileInfoTab).GetByString(Me.TBSprofile.SelectedTab.Value, ProfileInfoTab.none)
            Else
                Return ProfileInfoTab.none
            End If
        End Get
    End Property
#End Region

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As ProfileInfoPresenter
    Private ReadOnly Property CurrentPresenter() As ProfileInfoPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfileInfoPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
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
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView(PreloadedIdProfile)
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function


    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            'Me.Master.ServiceTitle = .getValue("serviceTitleEditProfile")
            Me.Master.ServiceNopermission = .getValue("serviceProfileInfoNopermission")
            .setLabel(LBmessages)
            .setLabel(LBdisplayName_t)
            .setLabel(LBprofileType_t)
            IMGavatar.ToolTip = .getValue("IMFoto.Tooltip")
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Public Sub LoadProfileInfo(idProfile As Integer, displayName As String, profileAvatar As String, idProfileType As Integer) Implements IViewProfileInfo.LoadProfileInfo
        Me.LBdisplayName.Text = displayName
        Dim profileName As String = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID = idProfileType Select o.Descrizione).FirstOrDefault

        If idProfileType <> lm.Comol.Core.DomainModel.UserTypeStandard.Company AndAlso String.IsNullOrEmpty(profileAvatar) Then
            Dim person As New COL_Persona With {.ID = idProfile}
            person.Estrai(LinguaID)

            profileAvatar = person.FotoPath
        End If

        Dim noImage As String = PageUtility.BaseUrlDrivePath & "images\noImage.jpg"
        Dim url As String = BaseUrl & "images/noImage.jpg"
        If Not String.IsNullOrEmpty(profileAvatar) Then
            If lm.Comol.Core.File.Exists.File(PageUtility.BaseUrlDrivePath & "profili\" & idProfile.ToString & "\" & profileAvatar) Then
                url = BaseUrl & "Profili/" & idProfile.ToString & "/" & profileAvatar
            End If
        End If
        Me.IMGavatar.ImageUrl = url
        Me.IMGavatar.Visible = Not String.IsNullOrEmpty(url)

        Me.LBprofileType.Text = profileName
        Me.MLVcontent.SetActiveView(VIWprofile)
    End Sub
    Public Sub LoadTabs(tabs As List(Of ProfileInfoTab)) Implements IViewProfileInfo.LoadTabs
        Me.TBSprofile.Enabled = (tabs.Count > 0)
        Dim SelectedIndex As Integer = 0
        If tabs.Count > 0 Then
            For Each tab As ProfileInfoTab In tabs
                Dim oTabView As Telerik.Web.UI.RadTab = Me.TBSprofile.Tabs.FindTabByValue(tab.ToString)
                If Not IsNothing(oTabView) Then
                    oTabView.Text = Resource.getValue("ProfileInfoTab." & tab.ToString)
                    oTabView.Visible = True
                End If
            Next
        End If
    End Sub

    Public Sub DisplayBaseInfo(idProfile As Integer, alsoProfileTypeInfo As Boolean) Implements IViewProfileInfo.DisplayBaseInfo
        CTRLbaseInfo.AlsoProfileTypeInfo = alsoProfileTypeInfo
        DisplayTab(idProfile, ProfileInfoTab.baseInfo)
    End Sub
    Public Sub DisplayTab(idProfile As Integer, tab As ProfileInfoTab) Implements IViewProfileInfo.DisplayTab
        Select Case tab
            Case ProfileInfoTab.baseInfo
                If Not Me.CTRLbaseInfo.IsInitialized Then
                    Me.CTRLbaseInfo.InitializeControl(idProfile, CTRLbaseInfo.AlsoProfileTypeInfo)
                End If
                Me.MLVuserInfo.SetActiveView(VIWbaseInfo)
            Case ProfileInfoTab.advancedInfo

                If Not Me.CTRLadvancedInfo.IsInitialized Then
                    Me.CTRLadvancedInfo.InitializeControl(idProfile)
                End If
                Me.MLVuserInfo.SetActiveView(VIWadvancedInfo)
            Case ProfileInfoTab.communityInfo
                If Not Me.CTRLprofileCommunities.IsInitialized Then
                    Me.CTRLprofileCommunities.InitializeControl(idProfile)
                End If
                Me.MLVuserInfo.SetActiveView(VIWcommunitiesInfo)

        End Select
        Dim tTab As Telerik.Web.UI.RadTab = Me.TBSprofile.FindTabByValue(tab.ToString)
        If Not IsNothing(tTab) Then
            tTab.Selected = True
        End If
    End Sub
    Public Sub DisplayNoPermission() Implements IViewProfileInfo.DisplayNoPermission
        Me.Master.ShowNoPermission = True
    End Sub
    Public Sub DisplayProfileUnknown() Implements IViewProfileInfo.DisplayProfileUnknown
        Me.MLVcontent.SetActiveView(VIWmessages)
        Me.LBmessages.Text = Resource.getValue("DisplayProfileUnknown")
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewProfileInfo.DisplaySessionTimeout
        Me.MLVcontent.SetActiveView(VIWmessages)
        Me.LBmessages.Text = Resource.getValue("DisplaySessionTimeout")
    End Sub
    Public Sub DisplayNoPermissionForProfile() Implements IViewProfileInfo.DisplayNoPermissionForProfile
        Me.MLVcontent.SetActiveView(VIWmessages)
        Me.LBmessages.Text = Resource.getValue("DisplayNoPermissionForProfile")
    End Sub
#End Region

    Private Sub TBSprofile_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSprofile.TabClick

        Dim selectedTab As ProfileInfoTab = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileInfoTab).GetByString(e.Tab.Value, ProfileInfoTab.none)
        DisplayTab(IdProfile, selectedTab)
    End Sub

    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub
End Class
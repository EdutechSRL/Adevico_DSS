Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProfileManagement
Imports lm.Comol.Core.BaseModules.ProfileManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class ProfileSettings
    Inherits PageBase
    Implements IViewProfileSettings

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
    Private _Presenter As ProfileSettingsPresenter
    Private ReadOnly Property CurrentPresenter() As ProfileSettingsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New ProfileSettingsPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property idDefaultProvider As Long Implements IViewProfileSettings.idDefaultProvider
        Get
            Return ViewStateOrDefault("idDefaultProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("idDefaultProvider") = value
        End Set
    End Property
    Public Property IdProfile As Integer Implements IViewProfileSettings.IdProfile
        Get
            Return ViewStateOrDefault("IdProfile", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfile") = value
        End Set
    End Property
    Public Property IdProfileType As Integer Implements IViewProfileSettings.IdProfileType
        Get
            Return ViewStateOrDefault("IdProfileType", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdProfileType") = value
        End Set
    End Property
    Public ReadOnly Property SelectedTab As ProfileSettingsTab Implements IViewProfileSettings.SelectedTab
        Get
            If Me.TBSprofile.TabIndex >= 0 Then
                Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileSettingsTab).GetByString(Me.TBSprofile.SelectedTab.Value, ProfileSettingsTab.none)
            Else
                Return ProfileSettingsTab.none
            End If
        End Get
    End Property
#End Region

#Region "Page"
    Public ReadOnly Property OnLoadingTranslation As String
        Get
            Return Me.Resource.getValue("OnLoadingTranslation")
        End Get
    End Property
#End Region

    Public ReadOnly Property TranslateModalView(viewName As String) As String
        Get
            Return Me.Resource.getValue("TranslateModalView." & viewName)
        End Get
    End Property

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.Master.ShowNoPermission = False
        If Page.IsPostBack = False Then
            Me.CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_ProfileInfo", "Modules", "ProfileManagement")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            Me.Master.ServiceTitle = .getValue("serviceTitleSettingsProfile")
            Me.Master.ServiceNopermission = .getValue("serviceSettingsProfileNopermission")
            '   .setLabel(LBmessages)
            .setLabel(LBdisplayName_t)
            .setLabel(LBprofileType_t)
            IMGavatar.ToolTip = .getValue("IMFoto.Tooltip")

            .setButton(BTNedit, True, , , True)
            .setButton(BTNsaveTop, True, , , True)
            .setButton(BTNsaveBottom, True, , , True)

            .setHyperLink(HYPeditPassword, True, True)
        End With
    End Sub
    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub
#End Region


#Region "Implements"
    Public Sub LoadProfileInfo(idProfile As Integer, displayName As String, profileAvatar As String, idProfileType As Integer) Implements IViewProfileSettings.LoadProfileInfo
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
        Me.MLVprofile.SetActiveView(VIWedit)
        Me.HYPeditPassword.Visible = True
        Me.HYPeditPassword.NavigateUrl = PageUtility.SecureBaseUrl & RootObject.EditPassword
        '   Me.UDPsettings.Update()
    End Sub
    Public Sub LoadTabs(tabs As List(Of ProfileSettingsTab)) Implements IViewProfileSettings.LoadTabs
        Me.TBSprofile.Enabled = (tabs.Count > 0)
        Dim SelectedIndex As Integer = 0
        If tabs.Count > 0 Then
            For Each tab As ProfileSettingsTab In tabs
                Dim oTabView As Telerik.Web.UI.RadTab = Me.TBSprofile.Tabs.FindTabByValue(tab.ToString)
                If Not IsNothing(oTabView) Then
                    oTabView.Text = Resource.getValue("ProfileSettingsTab." & tab.ToString)
                    oTabView.Visible = True
                End If
            Next
        End If
        '   Me.UDPsettings.Update()
    End Sub

    Public Sub DisplayTab(idProfile As Integer, tab As ProfileSettingsTab) Implements IViewProfileSettings.DisplayTab
        CTRLmessages.Visible = False
        Select Case tab
            Case ProfileSettingsTab.profileData
                If Not Me.CTRLprofileData.IsInitialized Then
                    Me.CTRLprofileData.InitializeControlForUserEdit(idProfile, IdProfileType, SystemSettings.Presenter.AllowUserRegistration)
                End If
                'Me.Page.Form.DefaultButton = Me.BTNsaveTop.ClientID
                ''Me.Page.Form.DefaultFocus = Me.TBSprofile.Tabs(0).ClientID
                'Me.Master.Page.Form.DefaultButton = Me.BTNsaveTop.ClientID
                'Me.Master.Page.Form.DefaultFocus = Me.TBSprofile.Tabs(0).ClientID

                Me.MLVuserInfo.SetActiveView(VIWprofileInfo)
            Case ProfileSettingsTab.mailPolicy
                If Not Me.CTRLmailPolicy.IsInitialized Then
                    Me.CTRLmailPolicy.InitializeControl(idProfile)
                End If
                'Me.Page.Form.DefaultButton = Me.CTRLmailPolicy.SearchButtonClientID
                'Me.Page.Form.DefaultFocus = Me.CTRLmailPolicy.SearchTextBoxClientID
                'Me.Master.Page.Form.DefaultButton = Me.CTRLmailPolicy.SearchButtonClientID
                'Me.Master.Page.Form.DefaultFocus = Me.CTRLmailPolicy.SearchTextBoxClientID

                Me.MLVuserInfo.SetActiveView(VIWmailPolicy)
            Case ProfileSettingsTab.advancedSettings
                'If Not Me.CTRLprofileCommunities.IsInitialized Then
                '    Me.CTRLprofileCommunities.InitializeControl(idProfile)
                'End If
                Me.MLVuserInfo.SetActiveView(VIWadvancedSettings)
        End Select
        Dim tTab As Telerik.Web.UI.RadTab = Me.TBSprofile.FindTabByValue(tab.ToString)
        If Not IsNothing(tTab) Then
            tTab.Selected = True
        End If
        '     Me.UDPsettings.Update()
    End Sub
    Public Sub DisplayNoPermission() Implements IViewProfileSettings.DisplayNoPermission
        Me.Master.ShowNoPermission = True
    End Sub
    Public Sub DisplayProfileUnknown() Implements IViewProfileSettings.DisplayProfileUnknown
        Me.MLVprofile.SetActiveView(VIWdefault)
        Me.LBmessages.Text = Resource.getValue("DisplayProfileUnknown")
        '   Me.UDPsettings.Update()
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewProfileSettings.DisplaySessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = RootObject.MyProfile
        webPost.Redirect(dto)
    End Sub
    'Public Sub DisplayNoPermissionForProfile() Implements IViewProfileSettings.DisplayNoPermissionForProfile
    '    Me.MLVcontent.SetActiveView(VIWmessages)
    '    Me.LBmessages.Text = Resource.getValue("DisplayNoPermissionForProfile")
    'End Sub
#End Region

    Private Sub TBSprofile_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles TBSprofile.TabClick
        Dim selectedTab As ProfileSettingsTab = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileSettingsTab).GetByString(e.Tab.Value, ProfileSettingsTab.none)
        DisplayTab(IdProfile, selectedTab)
    End Sub

#Region "Dialog"
    Private Sub OpenDialog(ByVal dialogId As String)
        Dim script As String = String.Format("showDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub CloseDialog(ByVal dialogId As String)
        Dim script As String = String.Format("closeDialog('{0}')", dialogId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
    Private Sub OpenCloseDialog(ByVal closeId As String, ByVal openId As String)
        Dim script As String = String.Format("closeDialog('{0}');showDialog('{1}');", closeId, openId)
        ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType, Me.UniqueID, script, True)
    End Sub
#End Region

#Region "Common"
    Private Sub BTNsaveTop_Click(sender As Object, e As System.EventArgs) Handles BTNsaveBottom.Click, BTNsaveTop.Click
        If ValidateContent() Then
            Select Case SelectedTab
                Case ProfileSettingsTab.profileData
                    Me.CurrentPresenter.ProfileSaved(Me.CTRLprofileData.SaveData())
                Case ProfileSettingsTab.mailPolicy
                    If Me.CTRLmailPolicy.SaveData() Then
                        DisplaySavedInfo()
                    Else
                        DisplayErrorSaving()
                    End If
            End Select
        End If
    End Sub
    Public Sub DisplaySavedInfo() Implements IViewProfileSettings.DisplaySavedInfo
        CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Me.Resource.getValue("DisplaySavedInfo"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
    Public Sub DisplayErrorSaving() Implements IViewProfileSettings.DisplayErrorSaving
        CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Me.Resource.getValue("DisplayErrorSaving"), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub
    Public Function ValidateContent() As Boolean Implements IViewProfileSettings.ValidateContent
        Select Case SelectedTab
            Case ProfileSettingsTab.profileData
                Return Me.CTRLprofileData.ValidateContent
            Case ProfileSettingsTab.mailPolicy
                Return True 'Me.CTRLmailPolicy.ValidateContent
            Case Else
                Return False
        End Select
    End Function
#End Region

#Region "Tab 1 - Profile"

#Region "Avatar"
    Private Sub BTNedit_Click(sender As Object, e As System.EventArgs) Handles BTNedit.Click
        Me.DVavatarEditor.Visible = True
        Me.CTRLavatar.InitializeControl(Me.IdProfile)
        DirectCast(Me.Master.FindControl("SCMmanager"), ScriptManager).RegisterPostBackControl(Me.CTRLavatar.UploadButton)
    End Sub
    Private Sub CTRLavatar_UploadedAvatar(avatar As String) Handles CTRLavatar.UploadedAvatar
        Me.DVavatarEditor.Visible = False
        If Me.CurrentPresenter.UpdateAvatar(avatar) Then
            Dim url As String = PageUtility.BaseUrlDrivePath & "profili\" & IdProfile.ToString & "\" & avatar
            If lm.Comol.Core.File.Exists.File(url) Then
                Me.IMGavatar.ImageUrl = BaseUrl & "Profili/" & IdProfile.ToString & "/" & avatar
            End If
        Else
            DisplayErrorSaving()
        End If
    End Sub
    Private Sub CTRLavatar_CloseWindow() Handles CTRLavatar.CloseWindow
        Me.DVavatarEditor.Visible = False
    End Sub
#End Region

#Region "Mail"
    Private Sub CTRLprofileData_EditMailAddress() Handles CTRLprofileData.EditMailAddress
        Me.CTRLmessages.Visible = False
        Me.DVmailEditor.Visible = True
        Me.CTRLmailEditor.InitializeControl(Me.IdProfile)
    End Sub
    Private Sub CTRLmailEditor_MailUpdated(mail As String) Handles CTRLmailEditor.MailUpdated
        Me.DVmailEditor.Visible = False
        Me.CTRLprofileData.MailUpdated(mail)
        Me.CTRLmessages.Visible = True
        Me.CTRLmessages.InitializeControl(Me.Resource.getValue("DisplaySavedInfo"), lm.Comol.Core.DomainModel.Helpers.MessageType.success)
    End Sub
    Private Sub CTRLmailEditor_CloseWindow() Handles CTRLmailEditor.CloseWindow
        Me.DVmailEditor.Visible = False
    End Sub
#End Region

#End Region

#Region "Tab 2 - Mail Policy"
    Public Function TreeViewClientID() As String
        Return Me.CTRLmailPolicy.TreeViewClientID
    End Function
#End Region
    Private Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Master.ShowDocType = True
    End Sub

End Class
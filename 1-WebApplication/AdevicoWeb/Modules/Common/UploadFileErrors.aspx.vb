Imports lm.Comol.UI.Presentation
'Imports COL_BusinessLogic_v2.UCServices
Imports lm.ActionDataContract
Imports lm.Comol.Core.DomainModel
'Imports lm.Comol.Core.BaseModules.CommunityDiary.Presentation
Imports lm.Comol.Core.BaseModules.CommunityDiary.Domain
Imports lm.Comol.Core.BaseModules.Domain
Imports lm.Comol.Core.BaseModules.Repository.Presentation

Public Class UploadFileErrors
    Inherits PageBase
    Implements IViewGenericUploadFileErrors

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private _Presenter As GenericUploadFileErrorsPresenter
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As GenericUploadFileErrorsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GenericUploadFileErrorsPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private Sub UploadFileErrors_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'If Me.CurrentContext.UserContext.isAnonymous Then
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/Registrazione.master"
        'Else
        '    Me.MasterPageFile = Me.Request.ApplicationPath & "/AjaxPortal.master"
        'End If
    End Sub
#End Region

#Region "Implements"
    Public ReadOnly Property PortalHome As String Implements IViewGenericUploadFileErrors.PortalHome
        Get
            Return Resource.getValue("PortalHome")
        End Get
    End Property
    Public ReadOnly Property BaseFolder As String Implements IViewGenericUploadFileErrors.BaseFolder
        Get
            Return Resource.getValue("BaseFolder")
        End Get
    End Property

    Private Function FormQuery(ByVal key As String) As IQueryable(Of String)
        If Me.Request.Form.Count = 0 Then
            Return (From s In New List(Of String) Select s).AsQueryable
        Else
            Return (From v As String In Me.Request.Form.Keys Where v.StartsWith(key) Select Me.Request.Form(v))
        End If
    End Function
    Public ReadOnly Property PreloadedBackUrl As String Implements IViewGenericUploadFileErrors.PreloadedBackUrl
        Get
            Return FormQuery("BackUrl").FirstOrDefault
        End Get
    End Property
    Public ReadOnly Property PreloadedModuleOwnerCode As String Implements IViewGenericUploadFileErrors.PreloadedModuleOwnerCode
        Get
            Return Me.Request.QueryString("ServiceCode")
        End Get
    End Property
    Public Property DeafultBackUrl As String Implements IViewGenericUploadFileErrors.DeafultBackUrl
        Get
            Return ViewStateOrDefault("DeafultBackUrl", "")
        End Get
        Set(ByVal value As String)
            HYPbackToModule.Visible = Not String.IsNullOrEmpty(value)
            If Not String.IsNullOrEmpty(value) Then
                HYPbackToModule.NavigateUrl = "~/" & PageUtility.GetUrlDecoded(value)
                ViewState("DeafultBackUrl") = PageUtility.GetUrlDecoded(value)
            End If
        End Set
    End Property
    Public Property ModuleOwnerCode As String Implements IViewGenericUploadFileErrors.ModuleOwnerCode
        Get
            Return ViewStateOrDefault("ModuleOwnerCode", "")
        End Get
        Set(ByVal value As String)
            ViewState("ModuleOwnerCode") = value
        End Set
    End Property
    Public Property ModuleOwnerID As Integer Implements IViewGenericUploadFileErrors.ModuleOwnerID
        Get
            Return ViewStateOrDefault("ModuleOwnerID", -1)
        End Get
        Set(ByVal value As Integer)
            ViewState("ModuleOwnerID") = value
        End Set
    End Property
    Public ReadOnly Property PreloadedModuleFiles As List(Of String) Implements IViewGenericUploadFileErrors.PreloadedModuleFiles
        Get
            Return FormQuery("FILE_I_Name_").ToList
        End Get
    End Property
    Public ReadOnly Property PreloadedRepositoryFiles As List(Of String) Implements IViewGenericUploadFileErrors.PreloadedRepositoryFiles
        Get
            Return FormQuery("FILE_C_Name_").ToList
        End Get
    End Property
    Private Property RepositoryFiles() As List(Of dtoErrorFile)
        Get
            Return ViewStateOrDefault("RepositoryFiles", New List(Of dtoErrorFile))
        End Get
        Set(ByVal value As List(Of dtoErrorFile))
            ViewState("RepositoryFiles") = value
        End Set
    End Property
    Private Property ModuleFiles() As List(Of dtoErrorFile)
        Get
            Return ViewStateOrDefault("ModuleFiles", New List(Of dtoErrorFile))
        End Get
        Set(ByVal value As List(Of dtoErrorFile))
            ViewState("ModuleFiles") = value
        End Set
    End Property
    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = False
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = False
        End If
        If Not Page.IsPostBack Then
            CurrentPresenter.InitView()
        End If
    End Sub
    Public Overrides Sub BindNoPermessi()
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = True
            DirectCast(Me.Master, AjaxPortal).ServiceNopermission = Resource.getValue("nopermission")
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = True
            '    DirectCast(Me.Master, Registrazione).ServiceNopermission = Resource.getValue("nopermission")
        End If
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UploadFileErrors", "Modules", "Repository")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPbackToModule, True, True)
            If TypeOf Me.Master Is AjaxPortal Then
                DirectCast(Me.Master, AjaxPortal).ServiceTitle = Resource.getValue("serviceUploadErrors_Title")
                'ElseIf TypeOf Me.Master Is Registrazione Then
                '    DirectCast(Me.Master, Registrazione).ServiceNopermission = Resource.getValue("serviceUploadErrors_Title")
            ElseIf TypeOf Me.Master Is AjaxPopup Then
                DirectCast(Me.Master, AjaxPopup).ServiceNopermission = Resource.getValue("serviceUploadErrors_Title")
            End If
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

#End Region

    Public Sub LoadFiles(ByVal moduleFiles As List(Of String), ByVal repositoryFiles As List(Of String)) Implements IViewGenericUploadFileErrors.LoadFiles
        Me.RepositoryFiles = (From f In repositoryFiles Select New dtoErrorFile With {.Name = f, .Extension = GetFileExtension(f), .ExtensionImage = BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(GetFileExtension(f))}).ToList
        Me.ModuleFiles = (From f In moduleFiles Select New dtoErrorFile With {.Name = f, .Extension = GetFileExtension(f), .ExtensionImage = BaseUrl & Me.PageUtility.SystemSettings.Extension.FindIconImage(GetFileExtension(f))}).ToList
    End Sub

    Private Sub LoadErrors()
        Dim ErrorString As String = "<li><img src='{0}'>&nbsp;{1}</li>"
        If Me.RepositoryFiles.Count > 0 Then
            Me.LBfileError.Text &= Me.Resource.getValue("communityfile.error")
            Me.LBfileError.Text &= "<ul>"
            For Each dto As dtoErrorFile In RepositoryFiles
                Me.LBfileError.Text &= String.Format(ErrorString, dto.ExtensionImage, dto.Name)
            Next
            Me.LBfileError.Text &= "</ul>"
        End If

        If ModuleFiles.Count > 0 Then
            Me.LBfileError.Text &= Me.Resource.getValue("internalfile.error")
            Me.LBfileError.Text &= "<ul>"
            For Each dto As dtoErrorFile In ModuleFiles
                Me.LBfileError.Text &= String.Format(ErrorString, dto.ExtensionImage, dto.Name)
            Next
            Me.LBfileError.Text &= "</ul>"
        End If
    End Sub

    Public Sub NoPermission(ByVal IdCommunity As Integer, ByVal IdModule As Integer, ByVal moduleCode As String) Implements IViewGenericUploadFileErrors.NoPermission
        Me.BindNoPermessi()
        Select Case moduleCode
            Case ModuleCommunityDiary.UniqueID

        End Select
    End Sub

    Public Sub ReturnToManagement() Implements IViewGenericUploadFileErrors.ReturnToManagement
        If Me.DeafultBackUrl <> "" Then
            PageUtility.RedirectToUrl(DeafultBackUrl)
        End If
    End Sub

    Public Sub ShowSessionTimeout() Implements IViewGenericUploadFileErrors.ShowSessionTimeout
        If TypeOf Me.Master Is AjaxPortal Then
            DirectCast(Me.Master, AjaxPortal).ShowNoPermission = True
            DirectCast(Me.Master, AjaxPortal).ServiceNopermission = Resource.getValue("ShowSessionTimeout")
            'ElseIf TypeOf Me.Master Is Registrazione Then
            '    DirectCast(Me.Master, Registrazione).ShowNoPermission = True
            '    DirectCast(Me.Master, Registrazione).ServiceNopermission = Resource.getValue("ShowSessionTimeout")
        End If
    End Sub

    Private Function GetFileExtension(ByVal FileName As String) As String
        Return Right(FileName, Len(FileName) - InStrRev(FileName, ".") + 1)
    End Function

    <Serializable()> Private Class dtoErrorFile
        Public Name As String
        Public Extension As String
        Public ExtensionImage As String
    End Class

End Class
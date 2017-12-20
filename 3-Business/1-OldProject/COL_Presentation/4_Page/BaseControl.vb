Imports COL_BusinessLogic_v2
Public MustInherit Class BaseUserControl
    Inherits System.Web.UI.UserControl
    Private _Resource As ResourceManager

    Private _PageUtility As PresentationLayer.OLDpageUtility
 
    Protected ReadOnly Property PageUtility() As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property

    Public Sub New()
    End Sub
    Protected ReadOnly Property Resource() As ResourceManager
        Get
            Try
                If IsNothing(_Resource) Then
                    Me.SetCultureSettings()
                End If
                Resource = _Resource
            Catch ex As Exception
                Resource = New ResourceManager
            End Try
        End Get
    End Property

    Protected Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
        Me._Resource = New ResourceManager

        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub
    Protected Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String)
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.Folder_Level2 = ResourceFolderLevel2
        Me._Resource.setCulture()
    End Sub
    Protected Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String, ByVal ResourceFolderLevel2 As String, ByVal ResourceFolderLevel3 As String)
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.Folder_Level2 = ResourceFolderLevel2
        Me._Resource.Folder_Level3 = ResourceFolderLevel3
        Me._Resource.setCulture()
    End Sub


    Protected MustOverride Sub SetCultureSettings()
    Protected MustOverride Sub SetInternazionalizzazione()

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.SetCultureSettings()
    End Sub

    Protected ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance '(Me.LinguaCode, Me.BaseUrlDrivePath, BaseUrl)
        End Get
    End Property

    Protected Overridable Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
    End Sub
    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
End Class
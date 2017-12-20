Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.UI.Presentation

Partial Public Class UC_CommunityFolderSize
    Inherits BaseControlSession
    Implements IviewCommunityRepositorySize

#Region "Implements"
    Private _DrivePath As String
    Private _Presenter As CRsizePresenter
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private ReadOnly Property CurrentPresenter() As CRsizePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRsizePresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Private WriteOnly Property UsageRate100() As Integer Implements IviewCommunityRepositorySize.UsageRate100
        Set(ByVal value As Integer)
            Me.IMGsize100.Width = Unit.Parse(value)
        End Set
    End Property
    Private WriteOnly Property UsageRate150() As Integer Implements IviewCommunityRepositorySize.UsageRate150
        Set(ByVal value As Integer)
            Me.IMGsize150.Width = Unit.Parse(value)
        End Set
    End Property
    Private WriteOnly Property UsageRate25() As Integer Implements IviewCommunityRepositorySize.UsageRate25
        Set(ByVal value As Integer)
            Me.IMGsize25.Width = Unit.Parse(value)
        End Set
    End Property
    Private WriteOnly Property UsageRate50() As Integer Implements IviewCommunityRepositorySize.UsageRate50
        Set(ByVal value As Integer)
            Me.IMGsize50.Width = Unit.Parse(value)
        End Set
    End Property
    Private WriteOnly Property UsageRate75() As Integer Implements IviewCommunityRepositorySize.UsageRate75
        Set(ByVal value As Integer)
            Me.IMGsize75.Width = Unit.Parse(value)
        End Set
    End Property
    Private ReadOnly Property BaseUnit() As Integer Implements IviewCommunityRepositorySize.BaseUnit
        Get
            Return 50
        End Get
    End Property
    Private ReadOnly Property DrivePath() As String Implements IviewCommunityRepositorySize.DrivePath
        Get
            Dim Path As String = ""
            If IsNothing(_DrivePath) Then
                If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                    _DrivePath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & "\"
                Else
                    _DrivePath = Me.SystemSettings.File.Materiale.DrivePath & "\"
                End If
            End If
            Path = _DrivePath & Me.RepositoryCommunityID & "\"
            Path = Replace(Path, "\/", "\")
            Return Path
        End Get
    End Property
    Private ReadOnly Property AvailableMB() As Long Implements IviewCommunityRepositorySize.AvailableMB
        Get
            Return Me.SystemSettings.File.GetByCode(FileSettings.ConfigType.File).MaxSize
        End Get
    End Property
    Private Property ChachedSize() As Long Implements IviewCommunityRepositorySize.ChachedSize
        Get
            Dim cacheKey As String = "CommunityRepositorySize_" & Me.RepositoryCommunityID
            If GenericCacheManager.Cache(cacheKey) Is Nothing Then
                Return 0
            Else
                Return CType(GenericCacheManager.Cache(cacheKey), Long)
            End If
        End Get
        Set(ByVal value As Long)
            Dim cacheKey As String = "CommunityRepositorySize_" & Me.RepositoryCommunityID
            If GenericCacheManager.Cache(cacheKey) Is Nothing Then
                GenericCacheManager.Cache.Insert(cacheKey, value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
            Else
                GenericCacheManager.Cache(cacheKey) = value
            End If
        End Set
    End Property
    Public Property EvaluateDeletedFiles() As Boolean Implements IviewCommunityRepositorySize.EvaluateDeletedFiles
        Get
            If TypeOf Me.ViewState("EvaluateDeletedFiles") Is Boolean Then
                Return CBool(Me.ViewState("EvaluateDeletedFiles"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("EvaluateDeletedFiles") = value
        End Set
    End Property
    Public Property isPersonalRepository() As Boolean Implements IviewCommunityRepositorySize.isPersonalRepository
        Get
            If TypeOf Me.ViewState("isPersonalRepository") Is Boolean Then
                Return CBool(Me.ViewState("isPersonalRepository"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("isPersonalRepository") = value
        End Set
    End Property
    Public Property RepositoryCommunityID() As Integer Implements IviewCommunityRepositorySize.RepositoryCommunityID
        Get
            If TypeOf Me.ViewState("RepositoryCommunityID") Is Integer Then
                Return CInt(Me.ViewState("RepositoryCommunityID"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RepositoryCommunityID") = value
        End Set
    End Property
    Public Property EnabledInit() As Boolean
        Get
            If TypeOf Me.ViewState("EnabledInit") Is Boolean Then
                Return CBool(Me.ViewState("EnabledInit"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("EnabledInit") = value
        End Set
    End Property
    Public WriteOnly Property TestoSezioneOver() As String Implements IviewCommunityRepositorySize.TestoSezioneOver
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Me.LTover.Visible = False
            Else
                Me.LTover.Text = value
                Me.LTover.Visible = True
            End If
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
    End Sub

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pc_UC_SpazioDisco", "Generici", "UC_File")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        Me.Resource.setLabel(LBinfoSize)
        Me.IMGsize150.ImageUrl = Me.BaseUrl & "images/rosso.gif"
        Me.IMGsize100.ImageUrl = Me.BaseUrl & "images/giallo.gif"
        Me.IMGsize75.ImageUrl = Me.BaseUrl & "images/verde.gif"
        'Me.IMGsize150.ImageUrl = Me.BaseUrl & "images/verde.gif"
        Me.IMGsize25.ImageUrl = Me.BaseUrl & "images/verde.gif"
        Me.IMGsize50.ImageUrl = Me.BaseUrl & "images/verde.gif"
    End Sub

    Public Sub SetDisplayInfo(ByVal SizeUsed As Double, ByVal SizeAvailable As Long) Implements IviewCommunityRepositorySize.SetDisplayInfo
        If SizeUsed < SizeAvailable Then
            Me.LBLspazioOccupato.Text = Me.Resource.getValue("InfoSpazio.Inferiore")
            Me.LBLspazioOccupato.Text = String.Format(Me.LBLspazioOccupato.Text, IIf((SizeUsed = 0), "0", (SizeAvailable - SizeUsed).ToString("0.00")), SizeAvailable)
        ElseIf SizeUsed = SizeAvailable Then
            Me.LBLspazioOccupato.Text = Me.Resource.getValue("InfoSpazio.Uguale")
            Me.LBLspazioOccupato.Text = String.Format(Me.LBLspazioOccupato.Text, SizeAvailable)
        Else
            Me.LBLspazioOccupato.Text = Me.Resource.getValue("InfoSpazio.Superiore")
            Me.LBLspazioOccupato.Text = String.Format(Me.LBLspazioOccupato.Text, IIf(SizeUsed = 0, "0", (SizeUsed - SizeAvailable).ToString("0.00")), SizeAvailable)
        End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.BindDati()
    End Sub
End Class
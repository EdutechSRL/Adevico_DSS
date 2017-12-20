Imports lm.Comol.Core.BaseModules.Editor
Public Class UC_Editor
    Inherits BaseControl
    Implements IViewEditorLoader

#Region "Context"
    Private _Presenter As EditorLoaderPresenter
    Private ReadOnly Property CurrentPresenter() As EditorLoaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditorLoaderPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
#Region "Editor Display"

    Public Property AllAvailableFontnames As Boolean Implements IViewEditorLoader.AllAvailableFontnames
        Get
            Return ViewStateOrDefault("AllAvailableFontnames", False)
        End Get
        Set(value As Boolean)
            ViewState("AllAvailableFontnames") = value
        End Set
    End Property
    Public Property FontNames As String Implements IViewEditorLoader.FontNames
        Get
            Return ViewStateOrDefault("FontNames", "")
        End Get
        Set(value As String)
            ViewState("FontNames") = value
        End Set
    End Property
    Public Property RealFontSizes As String Implements IViewBaseEditor.RealFontSizes
        Get
            Return ViewStateOrDefault("RealFontSizes", "")
        End Get
        Set(value As String)
            ViewState("RealFontSizes") = value
        End Set
    End Property
    Public Property UseRealFontSize As Boolean Implements IViewBaseEditor.UseRealFontSize
        Get
            Return ViewStateOrDefault("UseRealFontSize", False)
        End Get
        Set(value As Boolean)
            ViewState("UseRealFontSize") = value
        End Set
    End Property
    Public Property FontSizes As String Implements IViewEditorLoader.FontSizes
        Get
            Return ViewStateOrDefault("FontSizes", "")
        End Get
        Set(value As String)
            ViewState("FontSizes") = value
        End Set
    End Property
    Public Property ItemsToLink As String Implements IViewEditorLoader.ItemsToLink
        Get
            Return ViewStateOrDefault("ItemsToLink", "")
        End Get
        Set(value As String)
            ViewState("ItemsToLink") = value
        End Set
    End Property
    Public Property SmartTags As String Implements IViewEditorLoader.SmartTags
        Get
            Return ViewStateOrDefault("SmartTags", "")
        End Get
        Set(value As String)
            ViewState("SmartTags") = value
        End Set
    End Property
    Public Property Toolbar As ToolbarType Implements IViewEditorLoader.Toolbar
        Get
            Return ViewStateOrDefault("Toolbar", ToolbarType.bySettings)
        End Get
        Set(value As ToolbarType)
            ViewState("Toolbar") = value
        End Set
    End Property
    Public Property MaxHtmlLength As Long Implements IViewBaseEditor.MaxHtmlLength
        Get
            Return ViewStateOrDefault("MaxHtmlLength", 0)
        End Get
        Set(value As Long)
            ViewState("MaxHtmlLength") = value
        End Set
    End Property
    Public Property MaxTextLength As Long Implements IViewBaseEditor.MaxTextLength
        Get
            Return ViewStateOrDefault("MaxHtmlLength", 0)
        End Get
        Set(value As Long)
            ViewState("MaxHtmlLength") = value
        End Set
    End Property
    Public Property StripFormattingOnPaste As Boolean Implements IViewBaseEditor.StripFormattingOnPaste
        Get
            Return ViewStateOrDefault("StripFormattingOnPaste", True)
        End Get
        Set(value As Boolean)
            ViewState("StripFormattingOnPaste") = value
            If isInitialized Then

            End If
        End Set
    End Property
    Public Property AllowHTMLTags As Boolean Implements IViewBaseEditor.AllowHTMLTags
        Get
            Return ViewStateOrDefault("AllowHTMLTags", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowHTMLTags") = value
        End Set
    End Property

    Public Property LoaderCssClass As String Implements lm.Comol.Core.BaseModules.Editor.IViewEditorLoader.LoaderCssClass
        Get
            Return ViewStateOrDefault("LoaderCssClass", "")
        End Get
        Set(value As String)
            ViewState("LoaderCssClass") = value
        End Set
    End Property
    'Public Property ContainerCssClass As String Implements IViewBaseEditor.ContainerCssClass
    '    Get
    '        Return ViewStateOrDefault("ContainerCssClass", "")
    '    End Get
    '    Set(value As String)
    '        ViewState("ContainerCssClass") = value
    '    End Set
    'End Property
    Public Property EditorCssClass As String Implements IViewBaseEditor.EditorCssClass
        Get
            Return ViewStateOrDefault("EditorCssClass", "")
        End Get
        Set(value As String)
            ViewState("EditorCssClass") = value
        End Set
    End Property

    Public Property DefaultBackground As String Implements IViewEditorLoader.DefaultBackground
        Get
            Return ViewStateOrDefault("DefaultBackground", "Black")
        End Get
        Set(value As String)
            ViewState("DefaultBackground") = value
        End Set
    End Property
    Public Property DefaultColor As String Implements IViewEditorLoader.DefaultColor
        Get
            Return ViewStateOrDefault("DefaultColor", "White")
        End Get
        Set(value As String)
            ViewState("DefaultColor") = value
        End Set
    End Property
    Public Property DefaultFontName As String Implements IViewEditorLoader.DefaultFontName
        Get
            Return ViewStateOrDefault("DefaultFontName", "Tahoma")
        End Get
        Set(value As String)
            ViewState("DefaultFontName") = value
        End Set
    End Property
    Public Property DefaultFontSize As String Implements IViewEditorLoader.DefaultFontSize
        Get
            Return ViewStateOrDefault("DefaultFontSize", "1")
        End Get
        Set(value As String)
            ViewState("DefaultFontSize") = value
        End Set
    End Property
    Public Property NewLineMode As lm.Comol.Core.BaseModules.Editor.EditorNewLineModes Implements IViewEditorLoader.NewLineMode
        Get
            Return ViewStateOrDefault("NewLineMode", lm.Comol.Core.BaseModules.Editor.EditorNewLineModes.P)
        End Get
        Set(value As lm.Comol.Core.BaseModules.Editor.EditorNewLineModes)
            ViewState("NewLineMode") = value
        End Set
    End Property

    'Public Property DefaultFontConfiguration As FontConfiguration Implements IViewEditorLoader.DefaultFontConfiguration
    '    Get
    '        Return ViewStateOrDefault("DefaultFontConfiguration", Nothing)
    '    End Get
    '    Set(value As FontConfiguration)
    '        ViewState("DefaultFontConfiguration") = value
    '    End Set
    'End Property

    Public Property AutoResizeHeight As ItemPolicy Implements IViewEditorLoader.AutoResizeHeight
        Get
            Return ViewStateOrDefault("AutoResizeHeight", ItemPolicy.byconfiguration)
        End Get
        Set(value As ItemPolicy)
            ViewState("AutoResizeHeight") = value
        End Set
    End Property
    Public Property SetDefaultFont As ItemPolicy Implements IViewEditorLoader.SetDefaultFont
        Get
            Return ViewStateOrDefault("SetDefaultFont", ItemPolicy.byconfiguration)
        End Get
        Set(value As ItemPolicy)
            ViewState("SetDefaultFont") = value
        End Set
    End Property
    Public Property EnabledTags As String Implements IViewBaseEditor.EnabledTags
        Get
            Return ViewStateOrDefault("EnabledTags", "-")
        End Get
        Set(value As String)
            ViewState("EnabledTags") = value
        End Set
    End Property
    Public Property DisabledTags As String Implements IViewBaseEditor.DisabledTags
        Get
            Return ViewStateOrDefault("DisabledTags", "-")
        End Get
        Set(value As String)
            ViewState("DisabledTags") = value
        End Set
    End Property
    Public Property EditorHeight As String Implements IViewBaseEditor.EditorHeight
        Get
            Return ViewStateOrDefault("EditorHeight", "")
        End Get
        Set(value As String)
            ViewState("EditorHeight") = value
        End Set
    End Property
    Public Property EditorWidth As String Implements IViewBaseEditor.EditorWidth
        Get
            Return ViewStateOrDefault("EditorWidth", "")
        End Get
        Set(value As String)
            ViewState("EditorWidth") = value
        End Set
    End Property
#End Region
  
    Public Property AllowPreview As Boolean Implements IViewBaseEditor.AllowPreview
        Get
            Return ViewStateOrDefault("AllowPreview", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowPreview") = value
        End Set
    End Property
    Public Property EditorIdCommunity As Integer Implements IViewBaseEditor.EditorIdCommunity
        Get
            Return ViewStateOrDefault("EditorIdCommunity", 0)
        End Get
        Set(value As Integer)
            ViewState("EditorIdCommunity") = value
        End Set
    End Property
    Public Property EditorIdLanguage As Integer Implements IViewBaseEditor.EditorIdLanguage
        Get
            Return ViewStateOrDefault("EditorIdLanguage", 0)
        End Get
        Set(value As Integer)
            ViewState("EditorIdLanguage") = value
        End Set
    End Property
    Public Property EditorIdUser As Integer Implements IViewBaseEditor.EditorIdUser
        Get
            Return ViewStateOrDefault("EditorIdUser", 0)
        End Get
        Set(value As Integer)
            ViewState("EditorIdUser") = value
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewBaseEditor.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property IsEnabled As Boolean Implements IViewBaseEditor.IsEnabled
        Get
            Return ViewStateOrDefault("IsEnabled", True)
        End Get
        Set(value As Boolean)
            ViewState("IsEnabled") = value
            Me.CTRLtelerikEditor.IsEnabled = value
        End Set
    End Property
    Public Property AllowAnonymous As Boolean Implements IViewBaseEditor.AllowAnonymous
        Get
            Return ViewStateOrDefault("AllowAnonymous", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymous") = value
        End Set
    End Property
    Public Property RenderAsDiv As Boolean Implements IViewBaseEditor.RenderAsDiv
        Get
            Return ViewStateOrDefault("RenderAsDiv", False)
        End Get
        Set(value As Boolean)
            ViewState("RenderAsDiv") = value
            Me.CTRLtelerikEditor.RenderAsDiv = value
        End Set
    End Property
    Public ReadOnly Property Text As String Implements lm.Comol.Core.BaseModules.Editor.IViewBaseEditor.Text
        Get
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        Return CTRLtelerikEditor.Text
                End Select
            Else
                Return HTML
            End If
        End Get
    End Property
    Public Property HTML As String Implements lm.Comol.Core.BaseModules.Editor.IViewEditorLoader.HTML
        Get
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        Return CTRLtelerikEditor.HTML
                End Select
            Else
                Return ViewStateOrDefault("HTML", "")
            End If
        End Get
        Set(value As String)
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        CTRLtelerikEditor.HTML = value
                End Select
                ViewState("HTML") = ""
            Else
                ViewState("HTML") = value
            End If
           
        End Set
    End Property
    Public Property OnClientCommandExecuted As String Implements lm.Comol.Core.BaseModules.Editor.IViewEditorLoader.OnClientCommandExecuted
        Get
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        Return CTRLtelerikEditor.OnClientCommandExecuted
                End Select
            Else
                Return ViewStateOrDefault("OnClientCommandExecuted", "")
            End If
        End Get
        Set(value As String)
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        CTRLtelerikEditor.OnClientCommandExecuted = value
                End Select
                ViewState("OnClientCommandExecuted") = ""
            Else
                ViewState("OnClientCommandExecuted") = value
            End If

        End Set
    End Property
    Public Property OnClientLoadScript As String Implements lm.Comol.Core.BaseModules.Editor.IViewEditorLoader.OnClientLoadScript
        Get
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        Return CTRLtelerikEditor.OnClientLoadScript
                End Select
            Else
                Return ViewStateOrDefault("OnClientLoadScript", "")
            End If
        End Get
        Set(value As String)
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        CTRLtelerikEditor.OnClientLoadScript = value
                End Select
                ViewState("OnClientLoadScript") = ""
            Else
                ViewState("OnClientLoadScript") = value
            End If

        End Set
    End Property

    Public ReadOnly Property EditorClientId As String Implements lm.Comol.Core.BaseModules.Editor.IViewBaseEditor.EditorClientId
        Get
            Select Case CurrentType
                Case EditorType.telerik
                    Return CTRLtelerikEditor.EditorClientId
            End Select
        End Get
    End Property
    Public Property CurrentType As lm.Comol.Core.BaseModules.Editor.EditorType Implements IViewEditorLoader.CurrentType
        Get
            Return ViewStateOrDefault("CurrentType", lm.Comol.Core.BaseModules.Editor.EditorType.none)
        End Get
        Set(value As lm.Comol.Core.BaseModules.Editor.EditorType)
            ViewState("CurrentType") = value
        End Set
    End Property
    Public Property ModuleCode As String Implements IViewEditorLoader.ModuleCode
        Get
            Return ViewStateOrDefault("ModuleCode", "")
        End Get
        Set(value As String)
            ViewState("ModuleCode") = value
        End Set
    End Property
    Public Property AutoInitialize As Boolean Implements IViewEditorLoader.AutoInitialize
        Get
            Return ViewStateOrDefault("AutoInitialize", False)
        End Get
        Set(value As Boolean)
            ViewState("AutoInitialize") = value
        End Set
    End Property

    Public Property AllowHtmlEdit As Boolean Implements IViewBaseEditor.AllowHtmlEdit
        Get
            If isInitialized Then
                Select Case CurrentType
                    Case EditorType.telerik
                        Return CTRLtelerikEditor.AllowHtmlEdit
                    Case Else
                        Return False
                End Select
            End If
            Return ViewStateOrDefault("AllowHtmlEdit", False)
        End Get
        Set(value As Boolean)
            Select Case CurrentType
                Case EditorType.telerik
                    CTRLtelerikEditor.AllowHtmlEdit = value
            End Select
        End Set
    End Property
    Public Property AllowHtmlEditToAll As Boolean Implements IViewBaseEditor.AllowHtmlEditToAll
        Get
            '  If isInitialized Then
            Select Case CurrentType
                Case EditorType.telerik
                    Return CTRLtelerikEditor.AllowHtmlEditToAll
                Case EditorType.textarea
                    Return False
                Case EditorType.lite
                    Return False
                Case Else
                    Return False
            End Select
            ' End If
            '  Return ViewStateOrDefault("AllowHtmlEditToAll", False)
        End Get
        Set(value As Boolean)
            Select Case CurrentType
                Case EditorType.telerik
                    CTRLtelerikEditor.AllowHtmlEditToAll = value
            End Select
            '       ViewState("AllowHtmlEditToAll") = value
        End Set
    End Property
    Public Property AllowHtmlEditTo As String Implements IViewBaseEditor.AllowHtmlEditTo
        Get
            '  If isInitialized Then
            Select Case CurrentType
                Case EditorType.telerik
                    Return CTRLtelerikEditor.AllowHtmlEditTo
                Case EditorType.textarea
                    Return ""
                Case EditorType.lite
                    Return ""
                Case Else
                    Return ""
            End Select
            ' End If
            '  Return ViewStateOrDefault("AllowHtmlEditToAll", False)
        End Get
        Set(value As String)
            Select Case CurrentType
                Case EditorType.telerik
                    CTRLtelerikEditor.AllowHtmlEditTo = value
            End Select
            '       ViewState("AllowHtmlEditToAll") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack AndAlso AutoInitialize Then
            InitializeControl(ModuleCode)
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl() Implements IViewEditorLoader.InitializeControl
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), "", CurrentType, AllowAnonymous)
    End Sub

    Public Sub InitializeControl(type As EditorType) Implements IViewEditorLoader.InitializeControl
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), "", CurrentType, AllowAnonymous)
    End Sub

    Public Sub InitializeControl(moduleCode As String, section As String) Implements IViewEditorLoader.InitializeControl
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), moduleCode, CurrentType, AllowAnonymous)
    End Sub

    Public Sub InitializeControl(moduleCode As String) Implements IViewEditorLoader.InitializeControl
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), moduleCode, EditorType.none, AllowAnonymous)
    End Sub

    Public Sub InitializeControl(moduleCode As String, type As lm.Comol.Core.BaseModules.Editor.EditorType) Implements IViewEditorLoader.InitializeControl
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), moduleCode, type, AllowAnonymous)
    End Sub
    Public Sub InitializeControl(moduleCode As String, section As String, type As EditorType) Implements IViewEditorLoader.InitializeControl
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), moduleCode, type, AllowAnonymous)
    End Sub

    Public Sub LoadEditor(type As EditorType, ByVal settings As EditorInitializer) Implements IViewEditorLoader.LoadEditor
        Me.CurrentType = type
        Select Case type
            Case EditorType.telerik
                Me.MLVeditor.SetActiveView(VIWtelerikEditor)
                InitializeTelerik(settings)
          
                'Me.DefaultFontName = settings.DefaultFont.Family
                'Me.DefaultFontSize = settings.DefaultFont.Size
                'Me.DefaultColor = settings.DefaultFont.Color
                'Me.DefaultBackground = settings.DefaultFont.Background
              
        End Select
    End Sub
    Public Sub DisplayAsLabel() Implements IViewBaseEditor.DisplayAsLabel

    End Sub
    Public Sub DisplayPreview() Implements IViewBaseEditor.DisplayPreview

    End Sub

    Private Sub InitializeTelerik(ByVal settings As EditorInitializer)
        Me.CTRLtelerikEditor.FontNames = settings.FontNames
        Me.CTRLtelerikEditor.FontSizes = settings.FontSizes
        Me.CTRLtelerikEditor.AllowHTMLTags = AllowHTMLTags
        Me.CTRLtelerikEditor.AllowPreview = AllowPreview
        Me.CTRLtelerikEditor.StripFormattingOnPaste = StripFormattingOnPaste
        Me.CTRLtelerikEditor.MaxHtmlLength = MaxHtmlLength
        Me.CTRLtelerikEditor.MaxTextLength = MaxTextLength
        Me.CTRLtelerikEditor.EditorCssClass = EditorCssClass
        ' 'Me.CTRLtelerikEditor.ContainerCssClass = ContainerCssClass
        Me.CTRLtelerikEditor.EditorWidth = settings.Width
        Me.CTRLtelerikEditor.EditorHeight = settings.Height
        Me.CTRLtelerikEditor.UseRealFontSize = settings.UseRealFontSize
        Me.CTRLtelerikEditor.RealFontSizes = settings.RealFontSizes
        Me.CTRLtelerikEditor.DefaultFontName = DefaultFontName
        Me.CTRLtelerikEditor.DefaultFontSize = DefaultFontSize
        Me.CTRLtelerikEditor.DefaultColor = DefaultColor
        Me.CTRLtelerikEditor.DefaultBackground = DefaultBackground
        CTRLtelerikEditor.NewLineMode = NewLineMode
        If Not isInitialized Then
            Me.CTRLtelerikEditor.HTML = HTML
        End If
        Me.CTRLtelerikEditor.InitializeEditor(settings)
    End Sub
#End Region

    
End Class
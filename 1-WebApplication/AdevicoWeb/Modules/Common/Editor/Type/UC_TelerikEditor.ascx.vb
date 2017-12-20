Imports lm.Comol.Core.BaseModules.Editor

Public Class UC_TelerikEditor
    Inherits BaseControl
    Implements IViewTelerikEditor

#Region "Context"
    Private _Service As Business.ServiceRepositoryContent
    Private ReadOnly Property CurrentService() As Business.ServiceRepositoryContent
        Get
            If _Service Is Nothing Then
                _Service = New Business.ServiceRepositoryContent(PageUtility.CurrentContext, PageUtility.CurrentContext.UserContext.CurrentUserID, PageUtility.CurrentContext.UserContext.CurrentCommunityID, "", "")
            End If
            Return _Service
        End Get
    End Property
    'Private _Presenter As EditorPresenter
    'Private ReadOnly Property CurrentPresenter() As EditorPresenter
    '    Get
    '        If IsNothing(_Presenter) Then
    '            _Presenter = New EditorPresenter(Me.PageUtility.CurrentContext, Me)
    '        End If
    '        Return _Presenter
    '    End Get
    'End Property
#End Region

#Region "Implements"
#Region "Display"
    Public Property MaxHtmlLength As Long Implements IViewBaseEditor.MaxHtmlLength
        Get
            Return Me.RDEtelerik.MaxHtmlLength
        End Get
        Set(value As Long)

            Me.RDEtelerik.MaxHtmlLength = value
        End Set
    End Property
    Public Property MaxTextLength As Long Implements IViewBaseEditor.MaxTextLength
        Get
            Return Me.RDEtelerik.MaxTextLength
        End Get
        Set(value As Long)
            Me.RDEtelerik.MaxTextLength = value
        End Set
    End Property
    Public Property StripFormattingOnPaste As Boolean Implements IViewBaseEditor.StripFormattingOnPaste
        Get
            Return False ' RDEtelerik.StripFormattingOnPaste
        End Get
        Set(value As Boolean)
            'RDEtelerik.StripFormattingOptions =
            '  RDEtelerik.StripFormattingOnPaste = value
        End Set
    End Property
    Private Property AutoResizeHeight As ItemPolicy Implements IViewBaseEditor.AutoResizeHeight
        Get
            Return ViewStateOrDefault("AutoResizeHeight", ItemPolicy.byconfiguration)
        End Get
        Set(value As ItemPolicy)
            ViewState("AutoResizeHeight") = value
        End Set
    End Property
    Public Property SetDefaultFont As ItemPolicy Implements IViewBaseEditor.SetDefaultFont
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
            Return Me.RDEtelerik.Height.ToString
        End Get
        Set(ByVal value As String)
            Try
                If value = "" Then
                    Me.RDEtelerik.Height = System.Web.UI.WebControls.Unit.Pixel(250)
                ElseIf value.EndsWith("%") Then
                    Me.RDEtelerik.Height = System.Web.UI.WebControls.Unit.Percentage(Left(value, Len(value) - 1))
                ElseIf value.EndsWith("px") Then
                    Me.RDEtelerik.Height = System.Web.UI.WebControls.Unit.Pixel(Left(value, Len(value) - 2))
                End If
            Catch ex As Exception
                Me.RDEtelerik.Height = System.Web.UI.WebControls.Unit.Pixel(250)
            End Try
        End Set
    End Property
    Public Property EditorWidth As String Implements IViewBaseEditor.EditorWidth
        Get
            Return Me.RDEtelerik.Width.ToString
        End Get
        Set(ByVal value As String)
            Try
                If value = "" Then
                    Me.RDEtelerik.Width = System.Web.UI.WebControls.Unit.Pixel(850)
                ElseIf value.EndsWith("%") Then
                    Me.RDEtelerik.Width = System.Web.UI.WebControls.Unit.Percentage(Left(value, Len(value) - 1))
                ElseIf value.EndsWith("px") Then
                    Me.RDEtelerik.Width = System.Web.UI.WebControls.Unit.Pixel(Left(value, Len(value) - 2))
                End If
            Catch ex As Exception
                Me.RDEtelerik.Width = System.Web.UI.WebControls.Unit.Pixel(850)
            End Try
        End Set
    End Property

  
    'Public Property ContainerCssClass As String Implements IViewBaseEditor.ContainerCssClass
    '    Get
    '        Return RDEtelerik.ContentAreaCssFile
    '    End Get
    '    Set(value As String)
    '        RDEtelerik.ContentAreaCssFile = value
    '    End Set
    'End Property
    Public Property EditorCssClass As String Implements IViewBaseEditor.EditorCssClass
        Get
            Return RDEtelerik.CssClass
        End Get
        Set(value As String)
            RDEtelerik.CssClass = value
        End Set
    End Property
    Public Property DefaultBackground As String Implements IViewTelerikEditor.DefaultBackground
        Get
            Return ViewStateOrDefault("DefaultBackground", "Black")
        End Get
        Set(value As String)
            ViewState("DefaultBackground") = value
        End Set
    End Property
    Public Property DefaultColor As String Implements IViewTelerikEditor.DefaultColor
        Get
            Return ViewStateOrDefault("DefaultColor", "White")
        End Get
        Set(value As String)
            ViewState("DefaultColor") = value
        End Set
    End Property
    Public Property DefaultFontName As String Implements IViewTelerikEditor.DefaultFontName
        Get
            Return ViewStateOrDefault("DefaultFontName", "Tahoma")
        End Get
        Set(value As String)
            ViewState("DefaultFontName") = value
        End Set
    End Property
    Public Property DefaultFontSize As String Implements IViewTelerikEditor.DefaultFontSize
        Get
            Return ViewStateOrDefault("DefaultFontSize", "1")
        End Get
        Set(value As String)
            ViewState("DefaultFontSize") = value
        End Set
    End Property
    Public Property FontNames As String Implements IViewBaseEditor.FontNames
        Get
            Return ViewStateOrDefault("FontNames", "")
        End Get
        Set(value As String)
            ViewState("FontNames") = value
        End Set
    End Property
    Public Property FontSizes As String Implements IViewBaseEditor.FontSizes
        Get
            Return ViewStateOrDefault("FontSizes", "")
        End Get
        Set(value As String)
            ViewState("FontSizes") = value
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
    Public Property NewLineMode As lm.Comol.Core.BaseModules.Editor.EditorNewLineModes Implements IViewBaseEditor.NewLineMode
        Get
            Select Case RDEtelerik.NewLineMode
                Case Telerik.Web.UI.EditorNewLineModes.Br
                    Return EditorNewLineModes.Br
                Case Telerik.Web.UI.EditorNewLineModes.Div
                    Return EditorNewLineModes.Div
                Case Telerik.Web.UI.EditorNewLineModes.P
                    Return EditorNewLineModes.P
                Case Else
                    Return EditorNewLineModes.P
            End Select
        End Get
        Set(value As lm.Comol.Core.BaseModules.Editor.EditorNewLineModes)
            Select Case value
                Case EditorNewLineModes.Br
                    RDEtelerik.NewLineMode = Telerik.Web.UI.EditorNewLineModes.Br
                Case EditorNewLineModes.Div
                    RDEtelerik.NewLineMode = Telerik.Web.UI.EditorNewLineModes.Div
                Case EditorNewLineModes.P
                    RDEtelerik.NewLineMode = Telerik.Web.UI.EditorNewLineModes.P
            End Select
        End Set
    End Property
    Public ReadOnly Property EditorClientId As String Implements lm.Comol.Core.BaseModules.Editor.IViewBaseEditor.EditorClientId
        Get
            Return RDEtelerik.ClientID
        End Get
    End Property

    Public Property OnClientCommandExecuted As String Implements IViewBaseEditor.OnClientCommandExecuted
        Get
            Return ViewStateOrDefault("OnClientCommandExecuted", "")
        End Get
        Set(value As String)
            ViewState("OnClientCommandExecuted") = value
        End Set
    End Property
    Public Property OnClientLoadScript As String Implements IViewBaseEditor.OnClientLoadScript
        Get
           Return ViewStateOrDefault("OnClientLoadScript", "")
        End Get
        Set(value As String)
             ViewState("OnClientLoadScript") = value
        End Set
    End Property


    Private _DefaultIdLanguage As Integer
    Private ReadOnly Property GetDefaultIdLanguage As Integer
        Get
            If _DefaultIdLanguage < 0 Then
                _DefaultIdLanguage = CurrentService.GetIdDefaultLanguage
            End If
            Return _DefaultIdLanguage
        End Get
    End Property
    Public Property IsEnabled As Boolean Implements IViewBaseEditor.IsEnabled
        Get
            Return RDEtelerik.Enabled
        End Get
        Set(value As Boolean)
            RDEtelerik.Enabled = value
        End Set
    End Property
    Public Property RenderAsDiv As Boolean Implements IViewBaseEditor.RenderAsDiv
        Get
            Return (RDEtelerik.ContentAreaMode = Telerik.Web.UI.EditorContentAreaMode.Div)
        End Get
        Set(value As Boolean)
            If value Then
                RDEtelerik.ContentAreaMode = Telerik.Web.UI.EditorContentAreaMode.Div
            ElseIf RDEtelerik.ContentAreaMode <> Telerik.Web.UI.EditorContentAreaMode.Iframe Then
                RDEtelerik.ContentAreaMode = Telerik.Web.UI.EditorContentAreaMode.Iframe
            End If
        End Set
    End Property
#End Region
    Public Property AllowHTMLTags As Boolean Implements IViewBaseEditor.AllowHTMLTags
        Get
            Return ViewStateOrDefault("AllowHTMLTags", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowHTMLTags") = value
        End Set
    End Property

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
    Public Property AllowAnonymous As Boolean Implements IViewBaseEditor.AllowAnonymous
        Get
            Return ViewStateOrDefault("AllowAnonymous", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowAnonymous") = value
        End Set
    End Property

    Public ReadOnly Property Text As String Implements lm.Comol.Core.BaseModules.Editor.IViewBaseEditor.Text
        Get
            Return Me.RDEtelerik.Text
        End Get
    End Property
    Public Property HTML As String Implements lm.Comol.Core.BaseModules.Editor.IViewTelerikEditor.HTML
        Get
            Return Me.RDEtelerik.Html
        End Get
        Set(value As String)
            Me.RDEtelerik.Html = value
            'If value = "" Then
            '    Me.DIVmenuPreview.Style("display") = "none"
            '    Me.DIVpreview.Style("display") = "none"
            'End If
        End Set
    End Property
    Public ReadOnly Property CurrentType As lm.Comol.Core.BaseModules.Editor.EditorType Implements IViewTelerikEditor.CurrentType
        Get
            Return lm.Comol.Core.BaseModules.Editor.EditorType.telerik
        End Get
    End Property

    Public Property OnCommandcripts As String Implements lm.Comol.Core.BaseModules.Editor.IViewEditor.OnCommandcripts
        Get

        End Get
        Set(value As String)

        End Set
    End Property

    Public Property OnLoadScripts As String Implements lm.Comol.Core.BaseModules.Editor.IViewEditor.OnLoadScripts
        Get

        End Get
        Set(value As String)

        End Set
    End Property
    Public Property AllowHtmlEdit As Boolean Implements IViewBaseEditor.AllowHtmlEdit
        Get
            Return (RDEtelerik.EditModes = Telerik.Web.UI.EditModes.All)
        End Get
        Set(value As Boolean)
            If value Then
                RDEtelerik.EditModes = Telerik.Web.UI.EditModes.All
            Else
                RDEtelerik.EditModes = Telerik.Web.UI.EditModes.Design
            End If
        End Set
    End Property
    Public Property AllowHtmlEditToAll As Boolean Implements IViewBaseEditor.AllowHtmlEditToAll
        Get
            Return ViewStateOrDefault("AllowHtmlEditToAll", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowHtmlEditToAll") = value
        End Set
    End Property
    Public Property AllowHtmlEditTo As String Implements IViewBaseEditor.AllowHtmlEditTo
        Get
            Return ViewStateOrDefault("AllowHtmlEditTo", "")
        End Get
        Set(value As String)
            ViewState("AllowHtmlEditTo") = value
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

#Region "Control"
    Public ReadOnly Property AppUrl As String
        Get
            Return PageUtility.ApplicationUrlBase(True)
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack AndAlso Not String.IsNullOrEmpty(PageUtility.LinguaCode) Then
            RDEtelerik.Language = PageUtility.LinguaCode
        End If
        'If Not String.IsNullOrEmpty(FontNames) Then
        '    Me.RDEtelerik.FontNames.Clear()
        '    For Each fName As String In FontNames.Split(",")
        '        Me.RDEtelerik.FontNames.Add(fName)
        '    Next
        'End If
        'If Not String.IsNullOrEmpty(FontSizes) Then
        '    Me.RDEtelerik.FontSizes.Clear()
        '    For Each fSize As String In FontSizes.Split(",")
        '        Me.RDEtelerik.FontSizes.Add(fSize)
        '    Next
        'End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Editor", "Modules", "Common")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeEditor(settings As EditorInitializer) Implements IViewEditor.InitializeEditor
        InitializeLayout(settings)
    End Sub
    Private Sub InitializeLayout(settings As EditorInitializer)
        Me.AllowHtmlEdit = settings.AllowEditHTML
        Me.DefaultFontName = settings.DefaultFont.Family
        Me.DefaultFontSize = settings.DefaultFont.Size
        Me.DefaultColor = settings.DefaultFont.Color
        Me.DefaultBackground = settings.DefaultFont.Background
        Me.SetDefaultFont = settings.SetDefaultFont

        '   Me.RDEtelerik.AutoResizeHeight = False
        '   Me.RDEtelerik.AutoResizeHeight = (settings.AutoResizeHeight = AutoResizePolicy.allow)

        InitializeFontSettings(settings)
        InitializeTools(settings)
    End Sub
    Private Sub InitializeFontSettings(settings As EditorInitializer)
        Dim quote As String = """"
        Dim onLoad As String = ""

        If IsNothing(settings.Toolbar) Then
            Me.RDEtelerik.ToolsFile = "~/" & RootObject.ToolsBasePath(PageUtility.SystemSettings.EditorConfigurationPath) & settings.ToolsPath & settings.Toolbar.ToString & "Toolbar.xml"
        Else
            Me.RDEtelerik.ToolsFile = "~/" & RootObject.ToolsBasePath(PageUtility.SystemSettings.EditorConfigurationPath) & settings.Toolbar.Url
        End If
        Me.RDEtelerik.EnsureToolsFileLoaded()

        ' TEMPORANEAMENTE DISATTIVATO
        If SetDefaultFont = ItemPolicy.allow Then
            Dim useRealFontSize As Boolean = settings.UseRealFontSize AndAlso settings.DefaultRealFont.isRealFontSize
            Dim fSize As String = ""
            Dim fFamily As String = settings.DefaultFont.Family
            Dim fBackGround As String = settings.DefaultFont.Background
            Dim fColor As String = settings.DefaultFont.Color
            If useRealFontSize Then
                fSize = settings.DefaultRealFont.Size
                fFamily = settings.DefaultRealFont.Family
                fBackGround = settings.DefaultRealFont.Background
                fColor = settings.DefaultRealFont.Color
            Else
                fSize = settings.GetRenderFont(FontSizeType.point, settings.DefaultFont.Size)
            End If


            onLoad = "<script type='text/javascript' language=" & quote & "javascript" & quote & " > " & vbCrLf & _
            "function RadEditorLoad(editor, eventArgs) {" & vbCrLf
            If (Not String.IsNullOrEmpty(fFamily)) Then
                onLoad &= "editor.get_contentArea().style.fontFamily='" & settings.DefaultFont.Family & "';" & vbCrLf
            End If

            If Not String.IsNullOrEmpty(fSize) Then
                onLoad &= "editor.get_contentArea().style.fontSize = '" & fSize & "';" & vbCrLf & ""
            End If
            'If String.IsNullOrEmpty(fSize) Then
            '    fSize = "12pt"
            'End If
            'onLoad &= "editor.get_contentArea().style.fontSize = '" & fSize & "';" & vbCrLf & ""
            If Not String.IsNullOrEmpty(fBackGround) Then
                onLoad &= "editor.get_contentArea().style.backgroundColor = '" & fBackGround & "';" & vbCrLf
            End If
            If Not String.IsNullOrEmpty(fColor) Then
                onLoad &= "editor.get_contentArea().style.color = '" & fColor & "';" & vbCrLf
            End If
            onLoad &= "}" & vbCrLf & "</script>"
            Me.LTscript.Text = onLoad
            Me.RDEtelerik.OnClientLoad = "RadEditorLoad"
        End If

        If Not IsNothing(settings.CssFiles) AndAlso settings.CssFiles.Any() Then
            Me.RDEtelerik.CssFiles.AddRange((From i In settings.CssFiles Select New Telerik.Web.UI.EditorCssFile(IIf(i.isFullAddress, "", PageUtility.ApplicationUrlBase() & settings.DefaultCssFilesPath) & i.FileName)).ToList())
        End If
        If RDEtelerik.Modules.Count = 0 Then
            Dim htmlInspector As New Telerik.Web.UI.EditorModule()
            htmlInspector.Name = "RadEditorStatistics"
            htmlInspector.Visible = True
            RDEtelerik.Modules.Add(htmlInspector)
        End If

        Me.RDEtelerik.FontNames.Clear()
        For Each fName As String In settings.FontNames.Split(",")
            Me.RDEtelerik.FontNames.Add(fName)
        Next

        Me.RDEtelerik.FontSizes.Clear()
        Me.RDEtelerik.RealFontSizes.Clear()
        If settings.UseRealFontSize Then
            For Each fSize As String In settings.RealFontSizes.Split(",")
                Me.RDEtelerik.RealFontSizes.Add(fSize)
            Next
            lm.Comol.Core.BaseModules.Editor.TelerikHelper.EditorHelper.RemoveButton(RDEtelerik, TelerikHelper.ToolsName.FontSize, TelerikHelper.GroupName._ALL)
        Else
            For Each fSize As String In settings.FontSizes.Split(",")
                Me.RDEtelerik.FontSizes.Add(fSize)
            Next
            lm.Comol.Core.BaseModules.Editor.TelerikHelper.EditorHelper.RemoveButton(RDEtelerik, TelerikHelper.ToolsName.RealFontSize, TelerikHelper.GroupName._ALL)
        End If
       

      
    End Sub
    Private Sub InitializeTools(settings As EditorInitializer)
        Dim script As String = ""
        InitializeImageManager(settings)

        script = GetEmoticonsScript(settings)
        script &= GetLatexScript(settings)
        script &= GetYouTubeScript(settings)
        script &= GetWikiScript(settings)
        script &= GetGlossaryScript(settings)
        If Not String.IsNullOrEmpty(script) Then
            script = String.Format("<script type=""text/javascript"">" & vbCrLf & "//<![CDATA[" & vbCrLf & "{0}" & vbCrLf & "//]]>" & vbCrLf & "</script>", script)
        End If
        'LTpostEditorScripts.Visible = False
        LTpostEditorScripts.Text = script
    End Sub
    Private Sub InitializeImageManager(settings As EditorInitializer)
        If settings.EnabledTags.Contains("img") AndAlso Not PageUtility.CurrentContext.UserContext.isAnonymous Then
            Me.RDEtelerik.ImageManager.EnableAsyncUpload = True
            Me.RDEtelerik.ImageManager.ContentProviderTypeName = GetType(TelerikDBContentProvider).AssemblyQualifiedName
            Me.RDEtelerik.ImageManager.AllowFileExtensionRename = False
            Me.RDEtelerik.ImageManager.ViewPaths = New String() {"/"}
            Me.RDEtelerik.ImageManager.UploadPaths = New String() {"/"}
            Me.RDEtelerik.ImageManager.DeletePaths = New String() {"/"}
        Else
            lm.Comol.Core.BaseModules.Editor.TelerikHelper.EditorHelper.RemoveButton(RDEtelerik, TelerikHelper.ToolsName.ImageManager, TelerikHelper.GroupName._ALL)
        End If
    End Sub
    Private Function GetEmoticonsScript(settings As EditorInitializer) As String
        Dim toolInfo As ItemAvailability = GetTool(settings, "emoticons", TelerikHelper.ToolsName.InsertEmoticons)
        If Not IsNothing(toolInfo) Then
            Dim script As String = FileHelpers.ReadTextFile(PageUtility.BaseUrlDrivePath & toolInfo.Script)
            If Not (String.IsNullOrEmpty(script)) Then
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.commandname), TelerikHelper.ToolsName.InsertEmoticons.ToString())
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogheight), toolInfo.DialogHeight)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogwidth), toolInfo.DialogWidth)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialoghtitle), Resource.getValue("InsertEmoticons.Title"))
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogurl), PageUtility.ApplicationUrlBase() & toolInfo.Url)
                Return vbCrLf & script
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Private Function GetLatexScript(settings As EditorInitializer) As String
        Dim toolInfo As ItemAvailability = GetTool(settings, "latex", TelerikHelper.ToolsName.InsertLatex)
        If Not IsNothing(toolInfo) Then
            Dim script As String = FileHelpers.ReadTextFile(PageUtility.BaseUrlDrivePath & toolInfo.Script)
            If Not (String.IsNullOrEmpty(script)) Then
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.commandname), TelerikHelper.ToolsName.InsertLatex.ToString())
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogheight), toolInfo.DialogHeight)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogwidth), toolInfo.DialogWidth)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialoghtitle), Resource.getValue("InsertLatex.Title"))
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogurl), PageUtility.ApplicationUrlBase() & toolInfo.Url)
                Return vbCrLf & script
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Private Function GetYouTubeScript(settings As EditorInitializer) As String
        Dim toolInfo As ItemAvailability = GetTool(settings, "youtube", TelerikHelper.ToolsName.InsertYoutube)
        If Not IsNothing(toolInfo) Then
            Dim script As String = FileHelpers.ReadTextFile(PageUtility.BaseUrlDrivePath & toolInfo.Script)
            If Not (String.IsNullOrEmpty(script)) Then
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.commandname), TelerikHelper.ToolsName.InsertYoutube.ToString())
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogheight), toolInfo.DialogHeight)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogwidth), toolInfo.DialogWidth)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialoghtitle), Resource.getValue("InsertYoutube.Title"))
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogurl), PageUtility.ApplicationUrlBase() & toolInfo.Url)
                Return vbCrLf & script
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Private Function GetWikiScript(settings As EditorInitializer) As String
        Dim toolInfo As ItemAvailability = GetTool(settings, "wiki", TelerikHelper.ToolsName.InsertWiki)
        If Not IsNothing(toolInfo) Then
            Dim script As String = FileHelpers.ReadTextFile(PageUtility.BaseUrlDrivePath & toolInfo.Script)
            If Not (String.IsNullOrEmpty(script)) Then
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.commandname), TelerikHelper.ToolsName.InsertWiki.ToString())
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogheight), toolInfo.DialogHeight)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogwidth), toolInfo.DialogWidth)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialoghtitle), Resource.getValue("InsertWiki.Title"))
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogurl), PageUtility.ApplicationUrlBase() & toolInfo.Url)
                Return vbCrLf & script
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Private Function GetGlossaryScript(settings As EditorInitializer) As String
        Dim toolInfo As ItemAvailability = GetTool(settings, "glossary", TelerikHelper.ToolsName.InsertGlossary)
        If Not IsNothing(toolInfo) Then
            Dim script As String = FileHelpers.ReadTextFile(PageUtility.BaseUrlDrivePath & toolInfo.Script)
            If Not (String.IsNullOrEmpty(script)) Then
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.commandname), TelerikHelper.ToolsName.InsertGlossary.ToString())
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogheight), toolInfo.DialogHeight)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogwidth), toolInfo.DialogWidth)
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialoghtitle), Resource.getValue("InsertGlossary.Title"))
                script = Replace(script, GetPlaceHolder(ScriptPlaceHolders.dialogurl), PageUtility.ApplicationUrlBase() & toolInfo.Url)
                Return vbCrLf & script
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Private Function GetTool(settings As EditorInitializer, tagName As String, tName As TelerikHelper.ToolsName) As ItemAvailability
        Dim tool As AdvancedLinkItem = Nothing
        Dim toolInfo As ItemAvailability = Nothing
        If settings.ItemsToLink.Any() Then
            tool = settings.ItemsToLink.Where(Function(i) i.Tag = tagName).FirstOrDefault
            If Not IsNothing(tool) Then
                toolInfo = tool.Availability.Where(Function(a) a.AvailableFor = EditorType.telerik).FirstOrDefault()
            End If
        End If
        Dim eTool As Telerik.Web.UI.EditorTool = lm.Comol.Core.BaseModules.Editor.TelerikHelper.EditorHelper.GetToolButton(RDEtelerik, tName)
        If settings.EnabledTags.Contains(tagName) AndAlso Not IsNothing(toolInfo) AndAlso Not IsNothing(eTool) Then
            eTool.Text = tool.GetTooltip(PageUtility.LinguaID, GetDefaultIdLanguage)
            Return toolInfo
        Else
            lm.Comol.Core.BaseModules.Editor.TelerikHelper.EditorHelper.RemoveButton(RDEtelerik, tName, TelerikHelper.GroupName._ALL)
            Return Nothing
        End If
    End Function

    Public Sub DisplayAsLabel() Implements IViewBaseEditor.DisplayAsLabel

    End Sub
    Public Sub DisplayPreview() Implements IViewBaseEditor.DisplayPreview

    End Sub
#End Region

    Private Function GetPlaceHolder(type As ScriptPlaceHolders) As String
        Return "#" & type.ToString() & "#"
    End Function

    Private Enum ScriptPlaceHolders
        commandname = 0
        dialogurl = 1
        dialogwidth = 2
        dialogheight = 3
        dialoghtitle = 4
    End Enum

End Class
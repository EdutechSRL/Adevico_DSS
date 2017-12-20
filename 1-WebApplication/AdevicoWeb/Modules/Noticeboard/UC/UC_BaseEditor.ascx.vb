Imports lm.Comol.Core.BaseModules.NoticeBoard.Presentation
Imports lm.Comol.Core.BaseModules.NoticeBoard.Domain

Partial Public Class UC_BaseEditor
    Inherits BaseControl
    Implements IViewBaseEditorLoader

#Region "Context"
    Private _Presenter As BaseEditorLoaderPresenter
    Public ReadOnly Property CurrentPresenter() As BaseEditorLoaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New BaseEditorLoaderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewBaseEditorLoader.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property UseRealFontSize As Boolean Implements IViewBaseEditorLoader.UseRealFontSize
        Get
            Return ViewStateOrDefault("UseRealFontSize", False)
        End Get
        Set(value As Boolean)
            ViewState("UseRealFontSize") = value
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
#Region "Internal"
    Public Property EditorAlign() As String
        Get
            EditorAlign = Me.DIVeditorHTML.Attributes("align")
        End Get
        Set(ByVal value As String)
            Me.DIVeditorHTML.Attributes("align") = value
        End Set
    End Property
    Public Property Html() As String
        Get
            Dim Testo As String = Me.TXBanteprima.Text
            'If Testo <> "" Then
            '	Testo = Server.HtmlEncode(Testo)
            'End If
            ''.Testo = Server.HtmlEncode(Me.TAanteprima.Value)
            ''.Testo = Server.HtmlDecode(Me.TAanteprima.Value)

            Return Testo
        End Get
        Set(ByVal value As String)
            Me.TXBanteprima.Text = value
        End Set
    End Property

    Public Property Alignment() As String
        Get
            Alignment = Me.HDNalign.Value
        End Get
        Set(ByVal value As String)
            If value = "" Then
                value = "left"
            End If
            Me.HDNalign.Value = value
            Me.TXBanteprima.Style.Item("textalign") = value
            Me.TXBanteprima.Style.Item("text-align") = value
            Me.TXBanteprima.Style.Item("align") = value
            Me.IMBleft.ImageUrl = Me.BaseUrl & "images/Editors/left.gif"
            Me.IMBcenter.ImageUrl = Me.BaseUrl & "images/Editors/centrato.gif"
            Me.IMBright.ImageUrl = Me.BaseUrl & "images/Editors/right.gif"
            Select Case value
                Case "left"
                    Me.IMBleft.ImageUrl = Me.BaseUrl & "images/Editors/leftO.gif"
                Case "center"
                    Me.IMBcenter.ImageUrl = Me.BaseUrl & "images/Editors/centratoO.gif"
                Case "right"
                    Me.IMBright.ImageUrl = Me.BaseUrl & "images/Editors/rightO.gif"
            End Select
        End Set
    End Property
    Public Property BackGround() As String
        Get
            BackGround = Me.HDNbackground.Value
        End Get
        Set(ByVal value As String)
            If value = "" Then
                value = "white"
            End If
            Me.HDNbackground.Value = value
            Me.TXBanteprima.Style.Item("background-Color") = value
            Me.TXBanteprima.Style.Item("backgroundColor") = value
        End Set
    End Property
    Public Property FontColor() As String
        Get
            FontColor = Me.HDNcolor.Value
        End Get
        Set(ByVal value As String)
            If value = "" Then
                value = "black"
            End If
            Me.HDNcolor.Value = value
            Me.TXBanteprima.Style("color") = value
        End Set
    End Property
    Public Property FontFace() As String
        Get
            FontFace = Me.DDLfont.SelectedValue
        End Get
        Set(ByVal value As String)
            If value = "" Then
                value = "Verdana"
            End If
            Me.TXBanteprima.Style("fontFamily") = value
            Try
                Me.DDLfont.SelectedValue = value
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Property FontSize() As Integer
        Get
            FontSize = DDLFontSize.SelectedValue
        End Get
        Set(ByVal value As Integer)
            If value = 0 Then
                value = 2
            End If
            Me.DDLFontSize.SelectedValue = value
            setSizeEditorHTML(value)
        End Set
    End Property
    Private Sub setSizeEditorHTML(ByVal Size As Integer)
        Select Case Size
            Case 1
                Me.TXBanteprima.Style.Add("fontSize", "8pt")
            Case 2
                Me.TXBanteprima.Style.Add("fontSize", "10pt")
            Case 3
                Me.TXBanteprima.Style.Add("fontSize", "12pt")
            Case 4
                Me.TXBanteprima.Style.Add("fontSize", "14pt")
            Case 5
                Me.TXBanteprima.Style.Add("fontSize", "18pt")
            Case 6
                Me.TXBanteprima.Style.Add("fontSize", "24pt")
            Case 7
                Me.TXBanteprima.Style.Add("fontSize", "36pt")
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Me.Page.IsPostBack = False Then
        '    Me.SetInternazionalizzazione()
        '    Me.BindDati()
        'End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_noticeboard", "Modules", "Noticeboard")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setImageButton(Me.IMBGrassetto, False, True, True)
            .setImageButton(Me.IMBitalic, False, True, True)
            .setImageButton(Me.IMBsottolineato, False, True, True)
            .setImageButton(Me.IMBleft, False, True, True)
            .setImageButton(Me.IMBcenter, False, True, True)
            .setImageButton(Me.IMBright, False, True, True)
            .setImage(Me.IMGcolor, True)
            .setImageButton(Me.IMBcolorN, False, True, True)
            .setImageButton(Me.IMBcolorG, False, True, True)
            .setImageButton(Me.IMBcolorB, False, True, True)
            .setImageButton(Me.IMBcolorR, False, True, True)
            .setImageButton(Me.IMBcolorV, False, True, True)
            .setImageButton(Me.IMBcolorBlu, False, True, True)
            .setImage(Me.IMGsfondo, True)
            .setImageButton(Me.IMGunderColoreB, False, True, True)
            .setImageButton(Me.IMGunderColoreN, False, True, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(message As NoticeboardMessage) Implements IViewBaseEditorLoader.InitializeControl
        InitializeControls()
        CurrentPresenter.InitView(PageUtility.BaseUrlDrivePath & lm.Comol.Core.BaseModules.Editor.RootObject.ConfigurationFile(PageUtility.SystemSettings.EditorConfigurationPath), message)
    End Sub
    Private Sub DisplayEditingDisabled() Implements IViewBaseEditorLoader.DisplayEditingDisabled
        DisableItems()
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewBaseEditorLoader.DisplaySessionTimeout
        DisableItems()
    End Sub
    Private Sub DisableItems()
        TXBanteprima.Enabled = False
        DDLfont.Enabled = False
        DDLFontSize.Enabled = False
        For Each item As ImageButton In (From c As Control In Me.Controls() Where TypeOf (c) Is ImageButton Select c)
            item.Enabled = False
        Next
    End Sub
    Private Sub LoadFontFamily(items As List(Of String), selected As String) Implements IViewBaseEditorLoader.LoadFontFamily
        Me.DDLfont.DataSource = items
        Me.DDLfont.DataBind()
        Me.DDLfont.SelectedValue = selected
    End Sub
    Private Sub LoadFontSize(items As List(Of String), selected As String) Implements IViewBaseEditorLoader.LoadFontSize

    End Sub

    Private Sub LoadMessage(message As NoticeboardMessage) Implements IViewBaseEditorLoader.LoadMessage
        If IsNothing(message) Then
            Html = ""
        Else
            Html = message.PlainText
            If Not message.CreateByAdvancedEditor AndAlso Not IsNothing(message.StyleSettings) Then
                FontSize = message.StyleSettings.FontSize
                '   FontFace = message.StyleSettings.FontFamily
                FontColor = message.StyleSettings.FontColor
                BackGround = message.StyleSettings.BackgroundColor
                Alignment = message.StyleSettings.TextAlign
            End If
        End If
    End Sub
    Private Sub InitializeControls()
        Dim insertCode, InsertText As String
        insertCode = Me.Resource.getValue("insertCode")
        InsertText = Me.Resource.getValue("InsertText")

        insertCode = Replace(insertCode, "'", "\'")
        InsertText = Replace(InsertText, "'", "\'")
        Me.DDLfont.Attributes.Add("onchange", "AggiornaFont();")
        Me.DDLFontSize.Attributes.Add("onchange", "AggiornaFont();")
        Me.IMBGrassetto.Attributes.Add("onClick", "addTag('b','" & Replace(InsertText, "#code#", "b") & "') ;return false;")
        Me.IMBitalic.Attributes.Add("onClick", "addTag('i','" & Replace(InsertText, "#code#", "b") & "');return false;")
        Me.IMBsottolineato.Attributes.Add("onClick", "addTag('u','" & Replace(InsertText, "#code#", "b") & "');return false;")

        Me.IMGunderColoreB.Attributes.Add("onClick", "AggiornaFontSfondo('white');return false;")
        Me.IMGunderColoreN.Attributes.Add("onClick", "AggiornaFontSfondo('black');return false;")
        Me.IMBcolorN.Attributes.Add("onClick", "AggiornaFontColor('black');return false;")
        Me.IMBcolorG.Attributes.Add("onClick", "AggiornaFontColor('#666666');return false;")
        Me.IMBcolorB.Attributes.Add("onClick", "AggiornaFontColor('white');return false;")
        Me.IMBcolorR.Attributes.Add("onClick", "AggiornaFontColor('red');return false;")
        Me.IMBcolorV.Attributes.Add("onClick", "AggiornaFontColor('#009966');return false;")
        Me.IMBcolorBlu.Attributes.Add("onClick", "AggiornaFontColor('#3366FF');return false;")

        Me.IMBleft.Attributes.Add("onClick", "AggiornaAlign('left');return false;")
        Me.IMBright.Attributes.Add("onClick", "AggiornaAlign('right');return false;")
        Me.IMBcenter.Attributes.Add("onClick", "AggiornaAlign('center');return false;")
    End Sub
#End Region
 
    'Public Overrides Sub SetCultureSettings()
    '    
    'End Sub

    'Public Overrides Sub SetInternazionalizzazione()
    
    'End Sub



End Class
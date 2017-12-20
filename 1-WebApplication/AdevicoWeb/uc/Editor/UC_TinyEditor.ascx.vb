Imports PresentationLayer

Public Class UC_TinyEditor
    Inherits BaseControlSession
    Implements IviewEditor

#Region "Property"
    'PresenterEditor
    Private _Presenter As PresenterEditor
    Public ReadOnly Property CurrentPresenter() As PresenterEditor
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New PresenterEditor(Me)
            End If
            Return _Presenter
        End Get
    End Property


    Public ReadOnly Property CurrentCommunity() As Comol.Entity.Community Implements PresentationLayer.IviewEditor.CurrentCommunity
        Get
            If Me.ComunitaCorrenteID = 0 Then
                Return Nothing
            Else
                Return New Community(Me.ComunitaCorrente.Id, Me.ComunitaCorrente.Nome, Me.ComunitaCorrente.IdPadre)
            End If
        End Get
    End Property

    Public ReadOnly Property CurrentUser() As Comol.Entity.Person Implements PresentationLayer.IviewEditor.CurrentUser
        Get
            '	Return New Person(1)
            If MyBase.UtenteCorrente Is Nothing Then
                Dim oPersona As COL_Persona = COL_Persona.GetUtenteAnonimo(MyBase.UserSessionLanguage)
                If IsNothing(oPersona) Then
                    Return Nothing
                Else
                    Return New Person(oPersona.ID, oPersona.Nome, oPersona.Cognome)
                End If
            Else
                Return New Person(MyBase.UtenteCorrente.ID, MyBase.UtenteCorrente.Nome, MyBase.UtenteCorrente.Cognome)
            End If
        End Get
    End Property

    Public ReadOnly Property UserLanguage() As Comol.Entity.Lingua Implements PresentationLayer.IviewEditor.UserLanguage
        Get
            Return MyBase.UserSessionLanguage
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetEditor(New TinySettings)
    End Sub


#Region "Editor properties"
    Public Property ShowScrollingSpeed() As Boolean Implements PresentationLayer.IviewEditor.ShowScrollingSpeed
        Get
            ShowScrollingSpeed = (Me.DIVmenuScrolling.Style("display") = "block")
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.DIVmenuScrolling.Style("display") = "block"
            Else
                Me.DIVmenuScrolling.Style("display") = "none"
            End If
        End Set
    End Property
    Public Property AutoScrollingSpeed() As ScrollingSpeed Implements PresentationLayer.IviewEditor.AutoScrollingSpeed
        Get
            If Me.DDLScorrimento.SelectedValue = -1 Then
                Return ScrollingSpeed.Slow
            Else
                Return Me.DDLScorrimento.SelectedValue
            End If
        End Get
        Set(ByVal value As ScrollingSpeed)
            Try
                Me.DDLScorrimento.SelectedValue = value
            Catch ex As Exception
                Me.DDLScorrimento.SelectedValue = ScrollingSpeed.Slow
            End Try
        End Set
    End Property


    Public Property HTML() As String Implements PresentationLayer.IviewEditor.HTML
        Get
            Return Me.TinyText.InnerText    'Me.RDEtext.Html
        End Get
        Set(ByVal value As String)
            'Me.RDEtext.Html = value
            Me.TinyText.InnerText = value
            If value = "" Then
                Me.DIVmenuPreview.Style("display") = "none"
                Me.DIVpreview.Style("display") = "none"
            End If
        End Set
    End Property
    Public ReadOnly Property Text() As String Implements PresentationLayer.IviewEditor.Text
        Get
            Return Me.TinyText.InnerText
        End Get
    End Property

    Public Property EditorMaxChar() As Long Implements PresentationLayer.IviewEditor.EditorMaxChar
        Get
            Try
                If String.IsNullOrEmpty(Me.ViewState("EditorMaxChar")) Then
                    Me.CVlunghezza.Visible = False
                    Me.CVlunghezza.Enabled = False
                    Return 0
                Else
                    Return DirectCast(Me.ViewState("EditorMaxChar"), Long)
                End If
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(ByVal value As Long)
            Me.ViewState("EditorMaxChar") = value
            If value > 0 Then
                Me.CVlunghezza.Enabled = True
                Me.CVlunghezza.Visible = True
                Me.CVlunghezza.ErrorMessage = String.Format(Me.Resource.getValue("CVlunghezza.ErrorMessage"), value)
                Me.CVlunghezza.Text = String.Format(Me.Resource.getValue("CVlunghezza.ErrorMessage"), value)

            Else
                Me.CVlunghezza.Visible = False
                Me.CVlunghezza.Enabled = False
            End If
        End Set
    End Property


    Public WriteOnly Property AllowPreview() As Boolean Implements PresentationLayer.IviewEditor.AllowPreview
        Set(ByVal value As Boolean)
            If value Then
                Me.DIVmenuPreview.Style("display") = "block"
            Else
                Me.DIVmenuPreview.Style("display") = "none"
            End If
            Me.DIVpreview.Style("display") = "none"
        End Set
    End Property




#End Region



#Region "Editor Tools"
    'Molte proprietà sono da Valutare/Rivedere: serve ROB...
    'Magari con parametri nel viewstate o simili?


#Region "Tools Advanced"
    'Da valutare...
#End Region
#End Region

    Public Property DisabledTags() As String Implements PresentationLayer.IviewEditor.DisabledTags
        Get
            Return Me.ViewState("DisabledTags")
        End Get
        Set(ByVal value As String)
            Me.ViewState("DisabledTags") = value
        End Set
    End Property

#Region "Da valutare"
    Public ReadOnly Property CustomDialogScript As String Implements PresentationLayer.IviewEditor.CustomDialogScript
        Get

        End Get
    End Property

    Public Property EditorEnabled As Boolean Implements PresentationLayer.IviewEditor.EditorEnabled
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property FontNames As String Implements PresentationLayer.IviewEditor.FontNames
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Property FontSizes As String Implements PresentationLayer.IviewEditor.FontSizes
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Property ImagesPaths As String() Implements PresentationLayer.IviewEditor.ImagesPaths
        Get

        End Get
        Set(ByVal value As String())

        End Set
    End Property

    Public Sub SetAdvancedTools(ByVal oList As System.Collections.Generic.List(Of Comol.Entity.SmartTag)) Implements PresentationLayer.IviewEditor.SetAdvancedTools

    End Sub

    Public WriteOnly Property ShowAddDocument As Boolean Implements PresentationLayer.IviewEditor.ShowAddDocument
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public WriteOnly Property ShowAddImage As Boolean Implements PresentationLayer.IviewEditor.ShowAddImage
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property ShowAddSmartTag As Boolean Implements PresentationLayer.IviewEditor.ShowAddSmartTag
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property


    Private Sub SetEditor(ByVal Settings As TinySettings)
        Dim LiteralString As String = ""

        LiteralString += "<script  type=""text/javascript"">" + vbCrLf
        'Questa puo' andare nella pagina...
        'LiteralString += "var TinyMceUrl = '../uc/Editor/tiny_mce/tiny_mce.js'" + vbCrLf

        LiteralString += Settings.GetButton1List()

        LiteralString += "' " + vbCrLf

        LiteralString += Settings.GetButton2List()

        LiteralString += "var TinyMceFontNames = '" + Settings.FontNames + "'" + vbCrLf
        LiteralString += "var TinyMceFontSizes = '" + Settings.FontSizes + "'" + vbCrLf

        LiteralString += "</script>" + vbCrLf

        Lit_EditorConfig.Text = LiteralString

    End Sub
#End Region

    Public Overrides Sub BindDati()
        Me.CurrentPresenter.Init(Me.ApplicationUrlBase())
        Me.DIVpreview.Style("display") = "none"
    End Sub


    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_UC_Editor", "UC")
        'Me.RDEtext.Language = Me.UserSessionLanguage.Codice
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBvelocitaAvanzato_t)
            .setButton(Me.BTNclosePreview, True)
            .setButton(Me.BTNpreview, True)
            Me.BTNclosePreview.Attributes.Add("onclick", "SetPreview();return false;")
        End With
    End Sub

    Private Sub BTNpreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNpreview.Click
        Me.DIVpreview.Style("display") = "block"
        Me.LTpreview.Text = Me.TinyText.InnerText
        If Me.TinyText.InnerText <> "" Then
            Me.LTpreview.Text = ManagerConfiguration.GetSmartTags(Me.BaseUrl).TagAll(Me.LTpreview.Text)
        End If
    End Sub


End Class


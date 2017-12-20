Public Class UC_LanguageChange
    Inherits BaseControl

    Public Event UpdateInternationalization(ByVal oLingua As Lingua)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindLanguages()
        End If
    End Sub

    'Public Sub BindLanguages(ByVal Languages As IList(Of lm.Comol.Core.DomainModel.Language), ByVal SelectedLangCode As String)
    '    Me.DDLLanguage.Items.Clear()

    '    For Each lang As lm.Comol.Core.DomainModel.Language In Languages
    '        Dim li As New ListItem(lang.Name, lang.Code)
    '        If lang.Code = SelectedLangCode Then
    '            li.Selected = True
    '        End If
    '        Me.DDLLanguage.Items.Add(li)
    '    Next
    'End Sub

    Public Property DDLAutoPostBack As Boolean
        Get
            Return Me.DDLLanguage.AutoPostBack
        End Get
        Set(value As Boolean)
            Me.DDLLanguage.AutoPostBack = value
            Me.LNBchangeLang.Visible = Not value
        End Set
    End Property

    'Public Sub SetLabelText(ByVal Text As String)
    '    If String.IsNullOrEmpty(Text) Then
    '        Me.LITlanguage_t.Visible = False
    '    Else
    '        Me.LITlanguage_t.Visible = True
    '        Me.LITlanguage_t.Text = Text
    '    End If
    'End Sub

    Private Sub ChangeLanguage()
        'Prendo le lingue di ToolBarSetting, che usano l'ID e NON il CODE
        
        '_Language = oLingua
        Dim oLingua As Lingua = GetSetLanguage()

        'Set Cookies
        Dim oBrowser As System.Web.HttpBrowserCapabilities
        oBrowser = Request.Browser

        If oBrowser.Cookies Then
            Dim oCookie_ID As New System.Web.HttpCookie("LinguaID", oLingua.ID)
            Dim oCookie_Code As New System.Web.HttpCookie("LinguaCode", oLingua.Codice)

            oCookie_ID.Expires = Now.AddYears(1)
            oCookie_Code.Expires = Now.AddYears(1)

            Me.Response.Cookies.Add(oCookie_ID)
            Me.Response.Cookies.Add(oCookie_Code)
        End If

        'Altra sessione da CambiaImpostazioniLingua nel Base (PageBase)
        Session("NewLinguaID") = 0

        'Imposta l'oggetto Lingua nel base
        'e richiama le funzioni di internazionalizzazione
        MyBase.UpdateLanguage(oLingua)

        RaiseEvent UpdateInternationalization(oLingua)
    End Sub

    Private Sub DDLLanguage_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DDLLanguage.SelectedIndexChanged
        ChangeLanguage()
    End Sub

    Private Sub LNBchangeLang_Click(sender As Object, e As System.EventArgs) Handles LNBchangeLang.Click
        ChangeLanguage()
    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AccessExternal", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(Me.LBlanguage_t)
            '.setLiteral(Me.LITlanguage_t)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

    Private Sub BindLanguages()
        'SystemSettings.TopBar.Languages
        'For Each lang In Me.Languages
        'Dim lang As KeyValuePair(Of Integer, String)

        Dim LinguaID As Integer = 0

        'Base
        If Not IsNothing(MyBase._Language) Then
            LinguaID = MyBase._Language.ID
        End If

        'Session
        If LinguaID <= 0 Then
            If Not IsNothing(Session(TicketHelper.SessionLanguageId)) Then
                Try
                    LinguaID = System.Convert.ToInt32(Session(TicketHelper.SessionLanguageId))
                Catch ex As Exception

                End Try
            End If

        End If

        'Cookie
        If (LinguaID <= 0) Then
            Dim oCookie As HttpCookie = Request.Cookies("LinguaID")
            If Not IsNothing(oCookie) Then
                Try
                    LinguaID = System.Convert.ToInt32(oCookie.Value)
                Catch ex As Exception

                End Try
            End If
        End If

        'System Default
        If LinguaID <= 0 Then
            LinguaID = SystemSettings.DefaultLanguage.ID
        End If

        Me.DDLLanguage.Items.Clear()
        For Each lang As KeyValuePair(Of Integer, String) In SystemSettings.TopBar.Languages
            Dim li As New ListItem(lang.Value, lang.Key)
            If (lang.Key = LinguaID) Then
                li.Selected = True
            End If
            Me.DDLLanguage.Items.Add(li)
        Next

        If Not IsNothing(Session(TicketHelper.SessionLanguageId)) Then
            'Se non ho sessione, imposto lingua!
            GetSetLanguage()
        End If
    End Sub

    Public ReadOnly Property GetLanguageCode As String
        Get
            If String.IsNullOrEmpty(Session(TicketHelper.SessionLanguageCode)) Then
                GetSetLanguage()
            End If
            'Session("LinguaID") = oLingua.ID
            Return Session(TicketHelper.SessionLanguageCode)
        End Get
        
    End Property

    Public ReadOnly Property GetLanguageId As Integer
        Get
            If String.IsNullOrEmpty(Session(TicketHelper.SessionLanguageId)) Then
                GetSetLanguage()
            End If

            Return Session(TicketHelper.SessionLanguageId)
        End Get

    End Property

    Private Sub InternalBindLanguage()

    End Sub

    Private Function GetSetLanguage() As Lingua
        Dim LinguaID As Integer = 0
        Try
            LinguaID = System.Convert.ToInt32(Me.DDLLanguage.SelectedValue)
        Catch ex As Exception

        End Try
        'Dim LanguageCode As String = 


        Dim oLingua As Lingua = Nothing

        If LinguaID > 0 Then 'Not String.IsNullOrEmpty(LanguageCode) Then
            'oLingua = ManagerLingua.GetByCodeOrDefault(LanguageCode)
            'Usando l'ID non ho il "OrDefault"
            oLingua = ManagerLingua.GetByID(LinguaID)
        End If

        'Se non la trova per ID, prendo il "GetDefault",
        'come in ManagerLingua.GetByCodeOrDefault(LanguageCode)
        If IsNothing(oLingua) Then
            oLingua = ManagerLingua.GetDefault()
        End If

        'Altrimenti configurazione
        If IsNothing(oLingua) Then
            oLingua = SystemSettings.DefaultLanguage
        End If

        'OverloadLanguage: set sessioni varie
        Session(TicketHelper.SessionLanguageId) = oLingua.ID
        Session(TicketHelper.SessionLanguageCode) = oLingua.Codice
        Session(TicketHelper.SessionLanguage) = New lm.Comol.Core.DomainModel.Language() With {.Id = oLingua.ID, .Code = oLingua.Codice, .isDefault = oLingua.isDefault, .Name = oLingua.Nome}

        Return oLingua
    End Function
End Class
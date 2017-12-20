Imports TK = lm.Comol.Core.BaseModules.Tickets

Public Class UC_ExternalTopBar
    Inherits BaseControl

    Public Event UpdateInternationalization(ByVal oLingua As Lingua)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.BindLanguages()
        End If
    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("uc_ExternalTopBar", "Modules", "Ticket")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(Me.LTwelcome_t)
            .setLiteral(Me.LTmanage_m)

            .setHyperLink(Me.HYPadd, True, True)
            HYPadd.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalAdd

            .setHyperLink(Me.HYPlist, True, True)
            HYPlist.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalList

            .setHyperLink(Me.HYPuserSettings, True, True)
            HYPuserSettings.NavigateUrl = ApplicationUrlBase & TK.Domain.RootObject.ExternalUserSettings

            .setLinkButton(Me.LKBlogout, True, True)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property


    Private Sub ChangeLanguage(ByVal LinguaID As Integer)
        ''Prendo le lingue di ToolBarSetting, che usano l'ID e NON il CODE
        'Dim LinguaID As Integer = 0
        'Try
        '    LinguaID = System.Convert.ToInt32(Me.DDLLanguage.SelectedValue)
        'Catch ex As Exception

        'End Try

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
        '_Language = oLingua

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

        BindLanguages()
    End Sub

    Private _LanguageId As Integer = 0
    Private Sub BindLanguages()
        'SystemSettings.TopBar.Languages
        'For Each lang In Me.Languages
        'Dim lang As KeyValuePair(Of Integer, String)

        If Not IsNothing(Me.CurrentUser) Then

            Me.BindUserName(Me.CurrentUser.Name & " " & Me.CurrentUser.SName, ApplicationUrlBase & TK.Domain.RootObject.ExternalUserSettings)
        End If
        'TK.Domain.RootObject.SettingsUser(0)) 'TEMPORANEO!!!


        Dim LinguaID As Integer = 0

        If Not IsNothing(MyBase._Language) Then
            LinguaID = MyBase._Language.ID
            'Else
            '    Dim oCookie As HttpCookie = Request.Cookies("LinguaID")
            '    If Not IsNothing(oCookie) Then
            '        Try
            '            LinguaID = System.Convert.ToInt32(oCookie.Value)
            '        Catch ex As Exception

            '        End Try
            '    End If
        End If

        If LinguaID <= 0 Then
            LinguaID = SystemSettings.DefaultLanguage.ID
        End If

        _LanguageId = LinguaID

        Me.RPTlanguages.DataSource = SystemSettings.TopBar.Languages
        Me.RPTlanguages.DataBind()

        'For Each lang As KeyValuePair(Of Integer, String) In 
        '    Dim li As New ListItem(lang.Value, lang.Key)
        '    If (lang.Key = LinguaID) Then
        '        li.Selected = True
        '    End If
        '    Me.DDLLanguage.Items.Add(li)
        'Next
    End Sub

    Private Sub RPTlanguages_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTlanguages.ItemCommand
        If e.CommandName = "ChangeLanguage" Then

            Dim LinguaID As Integer = 0

            Try
                LinguaID = System.Convert.ToInt32(e.CommandArgument)
            Catch ex As Exception

            End Try

            If (LinguaID > 0) Then
                Me.ChangeLanguage(LinguaID)
            End If

        End If
    End Sub

    Private Sub RPTlanguages_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTlanguages.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim itm As KeyValuePair(Of Integer, String) = e.Item.DataItem

            Dim LKBlanguage As LinkButton = e.Item.FindControl("LKBlanguage")

            If Not IsNothing(LKBlanguage) Then
                LKBlanguage.CommandName = "ChangeLanguage"
                LKBlanguage.CommandArgument = itm.Key
                LKBlanguage.Text = itm.Value
                LKBlanguage.ToolTip = itm.Value

                If itm.Key = _LanguageId Then
                    Me.HYPlanguageCurrent.Text = itm.Value
                End If

            End If
        End If
    End Sub
    Private Sub BindUserName(ByVal Name As String, ByVal Url As String)
        Me.HYPuser.Text = Name

        If String.IsNullOrEmpty(Url) Then
            HYPuser.Enabled = False
            HYPuser.NavigateUrl = ""
        Else
            HYPuser.Enabled = True
            HYPuser.NavigateUrl = Url
        End If
    End Sub


    Public ReadOnly Property CurrentUser As TK.Domain.DTO.DTO_User
        Get
            Dim Usr As New TK.Domain.DTO.DTO_User

            Try
                Usr = DirectCast(Session(TicketHelper.SessionExtUser), TK.Domain.DTO.DTO_User)
            Catch ex As Exception

            End Try

            Return Usr
        End Get
    End Property



#Region "From PAGEBASE"
    Private ReadOnly Property ApplicationUrlBase
        Get
            Dim Redirect As String = "http"

            If RequireSSL Then  'AndAlso Not WithoutSSLfromConfig => andalso not false = and true
                Redirect &= "s://" & Me.Request.Url.Host & Me.BaseUrl
            Else
                Redirect &= "://" & Me.Request.Url.Host & Me.BaseUrl
            End If
            ApplicationUrlBase = Redirect
        End Get
    End Property

    Public ReadOnly Property RequireSSL() As Boolean
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property
#End Region

    Private Sub LKBlogout_Click(sender As Object, e As System.EventArgs) Handles LKBlogout.Click
        Me.LogOut()
    End Sub

    Public Sub LogOut()
        'Cancellazione sessioni
        Session(TicketHelper.SessionExtUser) = Nothing

        'Tengo le sessioni della lingua...
        'Session(TicketHelper.SessionLanguageCode) = Nothing
        'Session(TicketHelper.SessionLanguageId) = Nothing
        'Session(TicketHelper.SessionLanguage) = Nothing

        'Session.Clear()
        'Session.Abandon()

        Response.Redirect(ApplicationUrlBase & TK.Domain.RootObject.ExternalLogin)
    End Sub
End Class
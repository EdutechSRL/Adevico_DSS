Public Class LanguageSettingsChange
    Inherits PageBase
    'Inherits BaseControlMenu

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Dim IdLanguage As Integer
        Try
            IdLanguage = CInt(Request.QueryString("LanguageID"))
        Catch ex As Exception
            IdLanguage = -1
        End Try

        Me.ChangeDefaultLanguage(IdLanguage)
    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub
    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

#End Region

    Private Sub ChangeDefaultLanguage(ByVal IdLanguage As Integer)
        If IdLanguage > 0 AndAlso MyBase.SystemSettings.TopBar.Languages.ContainsKey(IdLanguage) Then
            Try
                Dim oLingua As New Lingua
                oLingua = ManagerLingua.GetByID(IdLanguage)
                If Not Me.isUtenteAnonimo Then
                    Me.UtenteCorrente.SalvaImpostazioneLingua(IdLanguage)
                    Me.UtenteCorrente.EstraiTutto(IdLanguage)
                    Me.UtenteCorrente.Lingua = oLingua
                End If
                Me.CambiaImpostazioniLingua(IdLanguage, oLingua.Codice)
            Catch ex As Exception
                Exit Sub
            End Try
        End If
        If Me.isUtenteAnonimo Then
            If IsNothing(Me.Request.UrlReferrer) Then
                Response.Redirect(SystemSettings.Presenter.DefaultStartPage)
            Else
                Response.Redirect(Me.Request.UrlReferrer.AbsoluteUri, True)
            End If
        Else
            Me.GoToPortale()
        End If
    End Sub

End Class
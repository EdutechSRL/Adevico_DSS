Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.CL_persona
Imports System.DirectoryServices


Public MustInherit Class UC_LoginChat
    Inherits System.Web.UI.UserControl
    Protected oResource As ResourceManager

    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            PageUtility = _PageUtility
        End Get
    End Property

    Private Enum ErroreLabel
        MailInviata = 0
        MailNonInviata = 1
        PasswordErrata = 2
        AccountDisabilitato = 3
        LogonBloccato = 4
        AccountNonAttivato = 5
        AccountNonRegistrato = 6
    End Enum

    Public ReadOnly Property IsConnected() As Boolean
        Get
            Try
                If Session("objPersona").id > 0 Then
                    IsConnected = True
                Else
                    IsConnected = False
                End If
            Catch ex As Exception
                IsConnected = False
            End Try
        End Get
    End Property
    Public Property Comunita_ID() As Integer
        Get
            Try
                Comunita_ID = Me.HDNcomunita_ID.Value
            Catch ex As Exception
                Me.HDNcomunita_ID.Value = 0
                Comunita_ID = 0
            End Try
        End Get
        Set(ByVal Value As Integer)
            Me.HDNcomunita_ID.Value = Value
        End Set
    End Property

    Protected WithEvents HDNcomunita_ID As System.Web.UI.HtmlControls.HtmlInputHidden

#Region "Pannello Logon"
    Protected WithEvents PNLLogin As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLlogin As System.Web.UI.WebControls.Table
    Protected WithEvents TBRnoAccess As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBnoAccess As System.Web.UI.WebControls.Label
    Protected WithEvents LBlogin_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBlogin As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBpassword_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBpwd As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBerrore As System.Web.UI.WebControls.Label
    Protected WithEvents BTNlogin As System.Web.UI.WebControls.Button
    Protected WithEvents LNBPwdDimenticata As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LBpasswdDimenticata As System.Web.UI.WebControls.Label
    Protected WithEvents LBiscrizione_t As System.Web.UI.WebControls.Label
#End Region

#Region "Elementi Form"
    Protected WithEvents VLDLogin As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents VLDPassword As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents VLDSum As System.Web.UI.WebControls.ValidationSummary
#End Region

#Region "Invio Mail"
    Protected WithEvents LBInvioMail As System.Web.UI.WebControls.Label
    Protected WithEvents PNL_avvisoMail As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNGoLogin As System.Web.UI.WebControls.Button
#End Region

#Region "Pannello Recupero Password"
    Protected WithEvents PNLPwdDimenticata As System.Web.UI.WebControls.Panel
    Protected WithEvents LBmail_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents BTNPwdDimenticata As System.Web.UI.WebControls.Button
#End Region

#Region "Pannello Restart"
    Protected WithEvents PNLrestart As System.Web.UI.WebControls.Panel
    Protected WithEvents LBrestart As System.Web.UI.WebControls.Label
#End Region

#Region "No User"
    Protected WithEvents PNLNoUser As System.Web.UI.WebControls.Panel
    Protected WithEvents LBuserNotFound As System.Web.UI.WebControls.Label
    Protected WithEvents LBadminContacts As System.Web.UI.WebControls.Label
    Protected WithEvents BTNRiprova As System.Web.UI.WebControls.Button
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    'IdPersona > 0 indica il vero valore dell'id univoco della persona
    'IdPersona = -1 utente non trovato nel db
    'IdPersona = -2 serve per far apparire la form di invio della password

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            Session("Azione") = "load"
            If Me.Application.Item("SystemAcess") = True Then
                Me.TBRnoAccess.Visible = False
            Else
                Me.TBRnoAccess.Visible = True
            End If
            Me.AggiornaStato()
        End If

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub

    Public Function isJustConnected() As Boolean
        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                Return True
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

#Region "Pannello Logon"
    Private Sub BTNlogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlogin.Click
        'If Not Me.isJustConnected Then 'TENTATIVO....
        Dim oPersona As New COL_Persona
        oPersona.Login = TXBlogin.Text
        oPersona.Pwd = TXBpwd.Text

        Try
			Dim oUtility As New OLDpageUtility(Me.Context)

     
            oPersona.Logon(Me.Request.UserHostAddress, Session.SessionID, Me.Request.Browser.Browser, oUtility.SystemSettings.CodiceDB)


            If Not oPersona.Errore = Errori_Db.None Or oPersona.Id <= 0 Then
                If oPersona.Id = -2 Then
                    'non registrato
                    oResource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.AccountNonRegistrato, ErroreLabel))
                Else
                    oResource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.PasswordErrata, ErroreLabel))
                End If
            Else
                If oPersona.Bloccata = True Then
                    If oPersona.isInattesaAttivazione Then
                        oResource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.AccountNonAttivato, ErroreLabel))
                    Else
                        oResource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.AccountDisabilitato, ErroreLabel))
                    End If
                Else
                    If Me.Application.Item("SystemAcess") = False Then
                        ' Accesso consentito solo agli admin
                        If oPersona.TipoPersona.id <> Main.TipoPersonaStandard.SysAdmin Then
                            '        Me.LBerrore.Text = "<br>Spiacente, solo gli amministratori del sistema possono accedere in questo momento.<div>Riprovare più tardi.<br>"
                            oResource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.LogonBloccato, ErroreLabel))
                            oPersona.Logout()

                            If oPersona.Id > 0 Then
                                Dim TotaleConnessioni As Integer
                                TotaleConnessioni = oPersona.CancellaConnessione(oPersona.Id, Session.SessionID)
                                Try
                                    Me.Application.Lock()
                                    If CInt(Me.Application.Item("utentiConnessi")) > 0 Then
                                        Me.Application.Item("utentiConnessi") = TotaleConnessioni
                                    Else
                                        Me.Application.Item("utentiConnessi") = 0
                                    End If
                                    Me.Application.UnLock()
                                Catch ex As Exception
                                    Me.Application.Lock()
                                    If Me.Application.Item("utentiConnessi") > 0 Then
                                        Me.Application.Item("utentiConnessi") = Me.Application.Item("utentiConnessi") - 1
                                    End If
                                    Me.Application.UnLock()
                                End Try
                            End If
                            Exit Sub
                        End If
                    End If

                    Session("limbo") = True
                    Session("objPersona") = oPersona
                    Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
                    Session("Istituzione") = oPersona.GetIstituzione


                    Try
						Dim oLingua As New Lingua
                        oLingua = oPersona.Lingua
						oLingua = ManagerLingua.GetByID(oLingua.Id)
                        Session("LinguaID") = oLingua.Id
                        Session("LinguaCode") = oLingua.Codice
                        Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
                    Catch ex As Exception
                        Session("LinguaID") = 1
                        Session("LinguaCode") = "it-IT"
                        Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
                    End Try

                    'Aggiornamento file XML

                    Dim oTreeComunita As New COL_TreeComunita
                    Dim PRSN_ID As Integer
                    Try
                        PRSN_ID = oPersona.Id
                        oTreeComunita.Directory = Server.MapPath(".\profili\") & PRSN_ID & "\"
                        oTreeComunita.Nome = PRSN_ID & ".xml"
						oTreeComunita.AggiornaInfo(PRSN_ID, Session("LinguaID"), oUtility.SystemSettings.Login.DaysToUpdateProfile, True)
                    Catch ex As Exception

                    End Try

                    'Memorizzo impostazioni utente
                    Dim oImpostazioni As New COL_ImpostazioniUtente
                    Try
                        oImpostazioni.Directory = Server.MapPath("./profili/") & PRSN_ID & "\"
                        oImpostazioni.Nome = "settings_" & PRSN_ID & ".xml"
                        If oImpostazioni.Exist Then
                            oImpostazioni.RecuperaImpostazioni()
                            Session("oImpostazioni") = oImpostazioni
                        Else
                            Session("oImpostazioni") = Nothing
                        End If
                    Catch ex As Exception
                        Session("oImpostazioni") = Nothing
                    End Try
                    'registro la connessione !
                    Try
                        Dim Totaleconnessi As Integer

                        Totaleconnessi = oPersona.GetNumeroConnessioni
                        If Totaleconnessi <= 0 Then
                            Totaleconnessi = 1
                        End If
                        Me.Application.Lock()
                        Me.Application.Item("utentiConnessi") = Totaleconnessi
                        Me.Application.UnLock()
                    Catch ex As Exception
                        Me.Application.Lock()
                        Me.Application.Item("utentiConnessi") = 1
                        Me.Application.UnLock()
                    End Try

                    ' METTO IL CODICE PER ACCEDERE AL'ELENCO DELLE COMUNITA !!!!!
                    'Forse nn serve...
                    Response.Redirect("Chat_Ext.aspx") 'Tentativo...
                End If
            End If
        Catch ex As Exception

            'Me.LBerrore.Text = "Login o Password errate"
            'qui ci va il redirect alla pagina di errore
        End Try
        If Me.LBerrore.Text <> "" Then
            Me.LBerrore.Text = Me.LBerrore.Text & "<br>"
        End If
        'End If
    End Sub
    'Se premo sul pulsante per il recupero della password
    Private Sub LNBPwdDimenticata_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBPwdDimenticata.Click
        Session("Azione") = "lostPWD"
        AggiornaStato()
    End Sub
#End Region

#Region "Pannello Richiesta invio mail"
    ' Private Sub BTNPwdDimenticata_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNPwdDimenticata.Click
    '     'tiro su login e password in base alla mail e li spedisco ad essa
    '     Dim errore As Integer

    '     Try
    '        Dim oUtility As New OLDpageUtility(Me.Context)
    'errore = COL_Persona.InviaLoginPwdByEmail(Me.TXBEmail.Text, Session("LinguaCode"), oUtility.LocalizedMail)
    '         Me.PNL_avvisoMail.Visible = True
    '         Me.PNLPwdDimenticata.Visible = False
    '         Me.PNLLogin.Visible = False
    '         'nella classe c  sono altri codici di errore + dettagliati... secondo me cmq nn serve mostrarli 
    '         Select Case errore
    '             Case 0
    '                 'Me.LBInvioMail.Text = "Mail inviata correttamente"
    '                 oResource.setLabel_To_Value(Me.LBInvioMail, "erroreLogin." & CType(Me.ErroreLabel.MailInviata, ErroreLabel))
    '             Case Else
    '                 'Me.LBInvioMail.Text = "Problemi nell'invio della mail"
    '                 oResource.setLabel_To_Value(Me.LBInvioMail, "erroreLogin." & CType(Me.ErroreLabel.MailNonInviata, ErroreLabel))
    '         End Select
    '     Catch ex As Exception
    '         oResource.setLabel_To_Value(Me.LBInvioMail, "erroreLogin." & CType(Me.ErroreLabel.MailNonInviata, ErroreLabel))
    '     End Try
    '     Session("Azione") = "showMessage"
    ' End Sub
#End Region

#Region "Pannello Gestione No User"
    Private Sub BTNRiprova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNRiprova.Click
        Session("Azione") = "reloaded"
        AggiornaStato()
    End Sub
#End Region

    Private Sub AggiornaStato() ' serve per fare il rendering del controllo utente
        Select Case Session("Azione")
            Case "lostPWD"
                'gestione dell'invio login e password via mail
                PNLPwdDimenticata.Visible = True
                PNLLogin.Visible = False
                PNLNoUser.Visible = False
                Me.PNL_avvisoMail.Visible = False
            Case "reloaded"
                Me.PNL_avvisoMail.Visible = False
                Me.PNLNoUser.Visible = False
                Me.PNLPwdDimenticata.Visible = False
                Me.PNLrestart.Visible = False
                Me.PNLLogin.Visible = True
            Case "showMessage"
                Me.PNL_avvisoMail.Visible = True
                Me.PNLPwdDimenticata.Visible = False
                Me.PNLLogin.Visible = False

            Case Else
                Me.PNL_avvisoMail.Visible = False
                Me.PNLNoUser.Visible = False
                Me.PNLPwdDimenticata.Visible = False
                Me.PNLrestart.Visible = False
                Me.PNLLogin.Visible = True
        End Select

    End Sub

    Private Sub SetCookies(ByVal LinguaID As Integer, ByVal LinguaCode As String)
        Dim oBrowser As System.Web.HttpBrowserCapabilities
        oBrowser = Request.Browser

        If oBrowser.Cookies Then
            Dim oCookie_ID As New System.Web.HttpCookie("LinguaID", LinguaID.ToString)
            Dim oCookie_Code As New System.Web.HttpCookie("LinguaCode", LinguaCode)

            oCookie_ID.Expires = Now.AddYears(1)
            oCookie_Code.Expires = Now.AddYears(1)

            Me.Response.Cookies.Add(oCookie_ID)
            Me.Response.Cookies.Add(oCookie_Code)
        End If
    End Sub

#Region "Localizzazione"
    Public Function SetCulture(ByVal Code As String)
        'oResource = New COL_Localizzazione

        'oResource.UserLanguages = Code
        'oResource.ProjectName = "Comunita_OnLine"
        'oResource.ResourcesName = "page_UC_LoginForum"
        'oResource.FileAssembly = GetType(UC_Login).Assembly
        'oResource.setCulture()
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "page_UC_LoginForum"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Function
    Public Sub SetupInternazionalizzazione()
        ' Pannello Login
        With oResource
            .setLabel(Me.LBlogin_t)
            .setLabel(Me.LBpassword_t)
            .setButton(Me.BTNlogin)

            'PNLPwdDimenticata
            .setLabel(Me.LBmail_t)
            .setButton(Me.BTNPwdDimenticata)

            .setLabel(Me.LBiscrizione_t)
            .setButton(Me.BTNGoLogin)
            .setLabel(Me.LBpasswdDimenticata)
            .setLinkButton(Me.LNBPwdDimenticata, True, True)

        End With



        'PNLNoUser
        With oResource
            .setLabel(Me.LBuserNotFound)
            .setButton(Me.BTNRiprova)
            .setLabel(Me.LBadminContacts)
        End With

        'required field 

        oResource.setRequiredFieldValidator(Me.VLDLogin, True, False)
        oResource.setRequiredFieldValidator(Me.VLDPassword, True, False)
        oResource.setValidationSummary(Me.VLDSum)

        oResource.setLabel(Me.LBnoAccess)


        Dim oResourceConfig As New ResourceManager
        oResourceConfig = GetResourceConfig(Session("LinguaCode"))

       
    End Sub
#End Region
    Private Sub BTNGoLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNGoLogin.Click
        Me.PNL_avvisoMail.Visible = False
        Me.PNLPwdDimenticata.Visible = False
        Me.PNLLogin.Visible = True
        Session("Azione") = "loaded"
    End Sub

End Class
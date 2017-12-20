Imports COL_BusinessLogic_v2.Comol.Authentication
Imports COL_BusinessLogic_v2.Comol.Authentication.Manager


Public Class PresenterGenericLogin
	Inherits GenericPresenter

	Private _IsDBAvailable As Boolean

	Public ReadOnly Property IsDBAvailable() As Boolean
		Get
			Return _IsDBAvailable
		End Get
	End Property

	Public Sub New(ByVal view As IviewGenericLogin)
		MyBase.view = view
	End Sub
	Private Shadows ReadOnly Property View() As IviewGenericLogin
		Get
			View = MyBase.view
		End Get
	End Property

	Public Sub Init()
		_IsDBAvailable = COL_BusinessLogic_v2.MainNotifica.HasConnessioniDB

        Me.View.ShowLink(IviewGenericLogin.LinkType.RetrivePassword) = True
		Me.View.ShowLink(IviewGenericLogin.LinkType.Subscription) = Me.View.Config.SubscriptionActive
		Me.View.EnableHelpToSubscription = Me.View.Config.showHelpToSubscription

		Me.View.EnableLink(IviewGenericLogin.LinkType.RetrivePassword) = Me.View.SystemActive And IsDBAvailable
		Me.View.EnableLink(IviewGenericLogin.LinkType.Subscription) = Me.View.SystemActive And IsDBAvailable

        If Not (Me.View.SystemActive AndAlso IsDBAvailable) Then
            Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.NoConnessioneDB)
        End If
	End Sub

	Private Sub VerificaConnessioneLDAP()
		Dim oList As New List(Of AuthenticationConnection)
		oList = ManagerAuthentication.FindServerConnection(Me.View.LinguaID, Main.TipoAutenticazione.LDAP)

        If oList.Count > 0 Then
            Dim HasConnectionAvailable As Boolean = False
            Dim HasConnectionNOTAvailable As Boolean = False

            HasConnectionAvailable = oList.FindAll(New GenericPredicate(Of AuthenticationConnection, Boolean)(True, AddressOf AuthenticationConnection.FindByConnectionAvailable)).Count > 0
            HasConnectionNOTAvailable = oList.FindAll(New GenericPredicate(Of AuthenticationConnection, Boolean)(False, AddressOf AuthenticationConnection.FindByConnectionAvailable)).Count > 0
            If HasConnectionAvailable And HasConnectionNOTAvailable Then
                Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.NoConnessioneAlcuniLDAP)
            ElseIf HasConnectionAvailable Then
                Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.ConnessioneLDAPdisponibile)
            ElseIf HasConnectionNOTAvailable Then
                Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.NoConnessioneLDAP)
            End If
        End If
	End Sub

	Public Sub RecuperoMail()
		Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.NoMessage)
		If Me.View.SystemActive Then
			Me.View.MailToRetrieve = Nothing
			Me.View.ShowForm(IviewGenericLogin.Form.RetrivePassword)
		Else
			Me.View.EnableLink(IviewGenericLogin.LinkType.RetrivePassword) = False
		End If
	End Sub
	Public Sub SendRetrievePassword()
		Dim oUser As COL_Persona = ManagerPersona.FindLoginAccessByEmail(Me.View.MailToRetrieve)

		If IsNothing(oUser) Then
			Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.AccountNonTrovato)
		Else
			Dim oLogonMessage As ManagerAuthentication.LogonMessage
			oLogonMessage = ManagerAuthentication.InviaLoginPwdByEmail(oUser, Me.View.MailConfig, Me.View.MailTitle(IIf(oUser.AUTN_ID = Main.TipoAutenticazione.ComunitaOnLine, True, False)), Me.View.MailBody(IIf(oUser.AUTN_ID = Main.TipoAutenticazione.ComunitaOnLine, True, False)))
			Select Case oLogonMessage
				Case ManagerAuthentication.LogonMessage.MailToRetreivePasswordSent
					Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.MailInviata)
				Case ManagerAuthentication.LogonMessage.MailNotSent
					Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.MailNonInviata)
				Case ManagerAuthentication.LogonMessage.UserNotFound
					Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.AccountNonTrovato)
			End Select
		End If
		
	End Sub
	Public Sub TornaAlLogin()
		Me.View.ShowMessageToUser(IviewGenericLogin.MessageType.NoMessage)
		Me.View.MailToRetrieve = Nothing
		Me.View.ShowForm(IviewGenericLogin.Form.Login)
	End Sub

	Public Sub LogonUser()
		'	Dim oManagerPersona As ManagerPersona
		'oManagerPersona.FindLoginAccessByEmail()
		'If oPersona.isLoginLDAP(TXBlogin.Text, Me.TXBpwd.Text) Then
		'	oPersona.AUTN_RemoteUniqueID = Me.TXBlogin.Text
		'	oPersona.LogonLDAP(Me.Request.UserHostAddress, Session.SessionID, Me.Request.Browser.Browser, oUtility.SystemSettings.CodiceDB, Me.TXBpwd.Text)
		'Else
		'	oPersona.Logon(Me.Request.UserHostAddress, Session.SessionID, Me.Request.Browser.Browser, oUtility.SystemSettings.CodiceDB)
		'End If

		'If Not oPersona.Errore = Errori_Db.None Or oPersona.Id <= 0 Then
		'	If oPersona.Id = -2 Then
		'		'non registrato
		'		Me.Resource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.AccountNonRegistrato, ErroreLabel))
		'	Else
		'		Resource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.PasswordErrata, ErroreLabel))
		'	End If
		'Else
		'	If oPersona.Bloccata = True Then
		'		If oPersona.isInattesaAttivazione Then
		'			Resource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.AccountNonAttivato, ErroreLabel))
		'		Else
		'			Resource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.AccountDisabilitato, ErroreLabel))
		'		End If
		'	Else
		'		If Me.Application.Item("SystemAcess") = False Then
		'			' Accesso consentito solo agli admin
		'			If oPersona.TipoPersona.id <> Main.TipoPersonaStandard.SysAdmin Then
		'				'        Me.LBerrore.Text = "<br>Spiacente, solo gli amministratori del sistema possono accedere in questo momento.<div>Riprovare più tardi.<br>"
		'				Resource.setLabel_To_Value(Me.LBerrore, "erroreLogin." & CType(Me.ErroreLabel.LogonBloccato, ErroreLabel))
		'				oPersona.Logout()

		'				If oPersona.Id > 0 Then
		'					Dim TotaleConnessioni As Integer
		'					TotaleConnessioni = oPersona.CancellaConnessione(oPersona.Id, Session.SessionID)
		'					Try
		'						Me.Application.Lock()
		'						If CInt(Me.Application.Item("utentiConnessi")) > 0 Then
		'							Me.Application.Item("utentiConnessi") = TotaleConnessioni
		'						Else
		'							Me.Application.Item("utentiConnessi") = 0
		'						End If
		'						Me.Application.UnLock()
		'					Catch ex As Exception
		'						Me.Application.Lock()
		'						If Me.Application.Item("utentiConnessi") > 0 Then
		'							Me.Application.Item("utentiConnessi") = Me.Application.Item("utentiConnessi") - 1
		'						End If
		'						Me.Application.UnLock()
		'					End Try
		'				End If
		'				Exit Sub
		'			End If
		'		End If

		'		Session("limbo") = True
		'		Session("objPersona") = oPersona
		'		Session("ORGN_id") = oPersona.GetOrganizzazioneDefault
		'		Session("Istituzione") = oPersona.GetIstituzione

		'		Session("IdRuolo") = ""
		'		Session("IdComunita") = 0
		'		Session("ArrPermessi") = ""
		'		Session("ArrComunita") = ""
		'		Session("RLPC_ID") = ""
		'		Session("AdminForChange") = False
		'		Session("CMNT_path_forAdmin") = ""
		'		Session("idComunita_forAdmin") = ""

		'		Try
		'			Dim oLingua As New COL_Lingua
		'			If Me.NewLinguaID > 0 AndAlso Me.NewLinguaID <> oPersona.Lingua.ID Then
		'				oLingua.ID = Me.NewLinguaID
		'				oPersona.SalvaImpostazioneLingua(oLingua.ID)
		'				oPersona.Estrai(oLingua.ID)
		'			Else
		'				oLingua = COL_Lingua.GetByID(oPersona.Lingua.ID)
		'			End If
		'			Me.NewLinguaID = 0
		'			Session("LinguaID") = oLingua.ID
		'			Session("LinguaCode") = oLingua.Codice
		'			Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
		'		Catch ex As Exception
		'			Session("LinguaID") = 1
		'			Session("LinguaCode") = "it-IT"
		'			Me.SetCookies(Session("LinguaID"), Session("LinguaCode"))
		'		End Try

		'		'Aggiornamento file XML

		'		Dim oTreeComunita As New COL_TreeComunita
		'		Dim PRSN_ID As Integer
		'		Try
		'			PRSN_ID = oPersona.Id
		'			oTreeComunita.Directory = Server.MapPath(".\profili\") & PRSN_ID & "\"
		'			oTreeComunita.Nome = PRSN_ID & ".xml"
		'			oTreeComunita.AggiornaInfo(PRSN_ID, Session("LinguaID"), oResourceConfig, True)
		'		Catch ex As Exception

		'		End Try

		'		'Memorizzo impostazioni utente
		'		Dim oImpostazioni As New COL_ImpostazioniUtente
		'		Try
		'			oImpostazioni.Directory = Server.MapPath("./profili/") & PRSN_ID & "\"
		'			oImpostazioni.Nome = "settings_" & PRSN_ID & ".xml"
		'			If oImpostazioni.Exist Then
		'				oImpostazioni.RecuperaImpostazioni()
		'				Session("oImpostazioni") = oImpostazioni
		'			Else
		'				Session("oImpostazioni") = Nothing
		'			End If
		'		Catch ex As Exception
		'			Session("oImpostazioni") = Nothing
		'		End Try
		'		'registro la connessione !
		'		Try
		'			Dim Totaleconnessi As Integer

		'			Totaleconnessi = oPersona.GetNumeroConnessioni
		'			If Totaleconnessi <= 0 Then
		'				Totaleconnessi = 1
		'			End If
		'			Me.Application.Lock()
		'			Me.Application.Item("utentiConnessi") = Totaleconnessi
		'			Me.Application.UnLock()
		'		Catch ex As Exception
		'			Me.Application.Lock()
		'			Me.Application.Item("utentiConnessi") = 1
		'			Me.Application.UnLock()
		'		End Try


		'		Try
		'			Me.Application.Lock()
		'			Dim oTutor As KnowledgeTutor
		'			oTutor = Application("KnowledgeTutor")
		'			Me.Application.UnLock()
		'			If IsNothing(oTutor) Then
		'				oResourceConfig = GetResourceConfig(Session("LinguaCode"))
		'				Try
		'					oTutor = New KnowledgeTutor(oUtility.SystemSettings.CodiceDB)
		'					With oTutor
		'						Try
		'							.PercorsoFile = oResourceConfig.getValue("Tutor.PercorsoFile")
		'							If Not InStr(.PercorsoFile, ":\") > 0 Then
		'								.PercorsoFile = GetPercorsoApplicazione(Me.Request) & "/" & .PercorsoFile
		'								.PercorsoFile = Replace(.PercorsoFile, "///", "/")
		'								.PercorsoFile = Replace(.PercorsoFile, "//", "/")
		'								.PercorsoFile = Replace(.PercorsoFile, "//", "/")
		'								.PercorsoFile = Me.Server.MapPath(.PercorsoFile)
		'							End If
		'							.PercorsoFile = .PercorsoFile & "\"
		'							.PercorsoFile = Replace(.PercorsoFile, "\\", "\")
		'						Catch ex As Exception

		'						End Try
		'						If Not COL_BusinessLogic_v2.FileLayer.COL_File.DirectoryExist(.PercorsoFile) Then
		'							COL_BusinessLogic_v2.FileLayer.COL_File.CreaDirectoryByPath(.PercorsoFile)
		'						End If

		'						Try
		'							.AmpiezzaCoda = oResourceConfig.getValue("Tutor.AmpiezzaCoda")
		'						Catch ex As Exception
		'							.AmpiezzaCoda = 20
		'						End Try

		'						Try
		'							.SaveActivated = oResourceConfig.getValue("Tutor.SaveActivated")
		'						Catch ex As Exception
		'							.SaveActivated = False
		'						End Try

		'						Try
		'							.SaveOnDB = oResourceConfig.getValue("Tutor.SaveOnDB")
		'						Catch ex As Exception
		'							.SaveOnDB = False
		'						End Try
		'						Try
		'							.SaveOnFile = oResourceConfig.getValue("Tutor.SaveOnFile")
		'						Catch ex As Exception
		'							.SaveOnFile = False
		'						End Try
		'						If Not .SaveOnFile And Not .SaveOnDB Then
		'							.SaveActivated = False
		'						End If
		'					End With
		'				Catch ex As Exception

		'				End Try
		'			End If
		'			oTutor.RegistraAccesso(Page, Session("LinguaCode"), oPersona, "login_Standard")
		'			oPersona.RegistraAccesso(oUtility.SystemSettings.CodiceDB)
		'		Catch ex As Exception

		'		End Try


		'		Dim RichiediSSL As Boolean = False
		'		Try
		'			RichiediSSL = oResourceConfig.getValue("RichiediSSL")
		'		Catch ex As Exception

		'		End Try


		'		Dim LinkElenco As String = ""
		'		Try
		'			If oImpostazioni.Visualizza_Iscritto = Main.ElencoRecord.Normale Then
		'				LinkElenco = "Comunita/EntrataComunita.aspx"
		'			Else
		'				LinkElenco = "Comunita/NavigazioneTreeView.aspx"
		'			End If
		'		Catch ex As Exception
		'			LinkElenco = "Comunita/EntrataComunita.aspx"
		'		End Try


		'		Response.Redirect(Me.ApplicationUrlBase & LinkElenco)
	End Sub
End Class
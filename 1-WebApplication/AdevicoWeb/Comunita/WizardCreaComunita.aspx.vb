Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita


Public Class WizardCreaComunita
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager
	Private _OldPageUtility As OLDpageUtility

	Private ReadOnly Property Utility() As OLDpageUtility
		Get
			If IsNothing(_OldPageUtility) Then
				_OldPageUtility = New OLDpageUtility(Me.Context)
			End If
			Return _OldPageUtility
		End Get
	End Property

    Public Enum IndiceFasi
        Fase1_Dati = 0
        Fase2_SceltaResponsabile = 1
        Fase3_SceltaAltreComunita = 2
        Fase4_SceltaServizi = 3
        Fase5_Finale = 4
    End Enum

#Region "Definizioni Generali"

    Protected WithEvents HDN_ComunitaAttualeID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_ComunitaCreataID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
#End Region

#Region "Gestione Navigazione"
    Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalva As System.Web.UI.WebControls.Button
    Protected WithEvents PNLnavigazione2 As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalva2 As System.Web.UI.WebControls.Button
#End Region

#Region "Gestione Navigazione"
    Protected WithEvents TBLinserimento As System.Web.UI.WebControls.Table
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLdati As Comunita_OnLine.UC_Fase1DatiComunita
    Protected WithEvents TBLresponsabile As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLresponsabile As Comunita_OnLine.UC_Fase2Responsabile
    Protected WithEvents TBLpadri As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLpadri As Comunita_OnLine.UC_Fase3ComunitaPadri
    Protected WithEvents TBLservizi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLservizi As Comunita_OnLine.UC_Fase4DefinizioneServizi
    Protected WithEvents TBLfinale As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLfinale As Comunita_OnLine.UC_Fase5Finale
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Not Page.IsPostBack Then 'solo al primo giro
            Dim oPersona As COL_Persona
            Dim oServizio As New UCServices.Services_AmministraComunita
            Dim ForAdmin As Boolean = False
            Dim ComunitaID As Integer = 0

            oPersona = Session("objPersona")
            Try
                If Session("AdminForChange") = True Then
                    ComunitaID = Session("idComunita_forAdmin")
                Else
                    ComunitaID = Session("idComunita")
                End If
            Catch ex As Exception
                Try
                    ComunitaID = Session("idComunita")
                Catch ex2 As Exception
                    ComunitaID = 0
                End Try
            End Try

            Try
                If ForAdmin = False Then
                    oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                Else
                    If ComunitaID = Session("idComunita") Then
                        oServizio.PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                    Else
                        oServizio.PermessiAssociati = oPersona.GetPermessiForServizioForAdmin(ComunitaID, oServizio.Codex, False, oServizio.GetPermission_Change, oServizio.GetPermission_Moderate, oServizio.GetPermission_Moderate)
                    End If
                End If

                If (oServizio.PermessiAssociati = "") Then
                    oServizio.PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try
            Me.SetupInternazionalizzazione()
            Session("Azione") = "loaded"

            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1

            If oServizio.Admin Or oServizio.Moderate Or oServizio.CreateComunity Then
                Me.PNLcontenuto.Visible = True
                Me.TBRmenu.Visible = False
                Me.PNLpermessi.Visible = False
                Me.PNLnavigazione.Visible = True
                Me.PNLnavigazione2.Visible = True
                Me.Bind_Dati(ComunitaID)
            Else
                Me.TBRmenu.Visible = True
                Me.PNLcontenuto.Visible = False
                Me.PNLnavigazione.Visible = False
                Me.PNLnavigazione2.Visible = False
                Me.PNLpermessi.Visible = True
            End If
        End If

        Me.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

        End Try
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Return True
        Else
            Try
                Dim CMNT_ID As Integer = 0
                Try
                    If Session("AdminForChange") = True Then
                        CMNT_ID = Session("idComunita_forAdmin")
                    Else
                        CMNT_ID = Session("idComunita")
                    End If
                Catch ex As Exception
                    Try
                        CMNT_ID = Session("idComunita")
                    Catch ex2 As Exception
                        CMNT_ID = 0
                    End Try
                End Try
                If CMNT_ID <= 0 Then
                    Me.ExitToLimbo()
                    Return True
                End If
            Catch ex As Exception
                Me.ExitToLimbo()
                Return True
            End Try
        End If
    End Function
    Private Sub ExitToLimbo()
        Session("Limbo") = True
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
		Me.Utility.RedirectToUrl("Comunita/EntrataComunita.aspx")
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_WizardCreaComunita"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(LBNopermessi)
            .setButton(Me.BTNavanti, True, , , True)
            .setButton(Me.BTNconferma, True, , , True)
            .setButton(Me.BTNsalva, True, , , True)
            .setButton(Me.BTNavanti2, True, , , True)
            .setButton(Me.BTNconferma2, True, , , True)
            .setButton(Me.BTNsalva2, True, , , True)
            .setLinkButton(Me.LNBindietro, True, True)

            .setButton(Me.BTNelenco, True, , , True)
            .setButton(Me.BTNindietro, True, , , True)
            .setButton(Me.BTNelenco2, True, , , True)
            .setButton(Me.BTNindietro2, True, , , True)
        End With
    End Sub
#End Region

    Private Sub Bind_Dati(ByVal ComunitaID As Integer)
        Me.resetForm_ToFase1()
        Me.HDN_ComunitaAttualeID.Value = ComunitaID
        Me.HDN_ComunitaCreataID.Value = 0
        If Me.CTRLdati.SetupIniziale(0, ComunitaID, True) Then
            Me.BTNavanti.Enabled = True
            Me.BTNavanti2.Enabled = True
        Else
            Me.BTNavanti.Enabled = False
            Me.BTNavanti2.Enabled = False
        End If
    End Sub

    Private Sub resetForm_HideAll()
        Me.TBLdati.Visible = False
        Me.TBLfinale.Visible = False
        Me.TBLpadri.Visible = False
        Me.TBLresponsabile.Visible = False
        Me.TBLservizi.Visible = False
    End Sub
    Private Sub resetForm_ToFase1()
        Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
        Me.BTNconferma.Visible = False
        Me.BTNindietro.Visible = False
        Me.BTNavanti.Visible = True
        Me.BTNsalva.Visible = False

        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.TBLdati.Visible = True
        Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase1_Dati)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase1_Dati)
    End Sub
    Private Sub resetForm_ToFase2()
        Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
        Me.BTNconferma.Visible = False
        Me.BTNindietro.Visible = True
        Me.BTNavanti.Visible = True
        Me.BTNsalva.Visible = False

        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.TBLresponsabile.Visible = True
        Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase2_SceltaResponsabile)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase2_SceltaResponsabile)
    End Sub
    Private Sub resetForm_ToFase3()
        Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
        Me.BTNconferma.Visible = False
        Me.BTNindietro.Visible = True
        Me.BTNavanti.Visible = True
        Me.BTNsalva.Visible = False

        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.TBLpadri.Visible = True
        Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase3_SceltaAltreComunita)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase3_SceltaAltreComunita)
    End Sub

    Private Sub resetForm_ToFase4()
        Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
        Me.BTNconferma.Visible = False
        Me.BTNindietro.Visible = True
        Me.BTNavanti.Visible = True
        Me.BTNsalva.Visible = False

        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.TBLservizi.Visible = True
        Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase4_SceltaServizi)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase4_SceltaServizi)
    End Sub
    Private Sub resetForm_ToFase5()
        Me.resetForm_HideAll()
		If Request.QueryString("fromGestione") <> "" Or Utility.isModalitaAmministrazione Then
			Me.BTNelenco.Visible = True
		Else
			Me.BTNelenco.Visible = False
		End If
        Me.BTNconferma.Visible = True
        Me.BTNindietro.Visible = True
        Me.BTNavanti.Visible = False
        Me.BTNsalva.Visible = False

        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.TBLfinale.Visible = True
        Me.PNLcontenuto.Visible = True
        'Me.LBTitolo.Text = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase5_Finale)
        Me.Master.ServiceTitle = Me.oResource.getValue("LBTitolo." & Me.IndiceFasi.Fase5_Finale)
    End Sub
    Private Sub ReturnToPreviousPage()
        If Request.QueryString("fromGestione") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
        Else
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement)
        End If
    End Sub

    Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click, BTNavanti2.Click
        Dim ComunitaID As Integer
        Dim ComunitaPath As String
        Try
            If Session("AdminForChange") = True Then
                ComunitaID = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            Else

                ComunitaID = Session("idComunita")

                Try
                    Dim ArrComunita(,) As String = Session("ArrComunita")
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch ex As Exception
                    ComunitaPath = ""
                End Try
            End If
        Catch ex As Exception
			Try
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")
				ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
			Catch ex2 As Exception
				ComunitaID = 0
				ComunitaPath = ""
			End Try
        End Try

        If Me.TBLdati.Visible Then
            Me.Page.Validate()
            If Not Me.Page.IsValid Then
                Exit Sub
            End If
            If Me.CTRLresponsabile.isInizializzato = False Then
                Dim oComunita As New COL_Comunita

                oComunita.Id = ComunitaID
                Me.CTRLresponsabile.SetupControllo(0, ComunitaID, oComunita.GetOrganizzazioneID, Me.CTRLdati.TipoComunita_ID, True)
            Else
                Me.CTRLresponsabile.AggiornaDati()
            End If
            Me.resetForm_ToFase2()
        ElseIf Me.TBLresponsabile.Visible Then

            If Me.CTRLpadri.isInizializzato = False Then
                Dim oComunita As New COL_Comunita

                oComunita.Id = ComunitaID
                oComunita.Estrai()
                Me.CTRLpadri.SetupControl(oComunita.Livello, oComunita.Id, ComunitaPath)
            End If
            Me.resetForm_ToFase3()
        ElseIf Me.TBLpadri.Visible Then
            If Me.CTRLservizi.isInizializzato = False Then
                Dim oComunita As New COL_Comunita

                oComunita.Id = ComunitaID
                oComunita.Estrai()
                Me.CTRLservizi.SetupControl(Me.CTRLdati.TipoComunita_ID, oComunita.GetOrganizzazioneID, Session("objPersona").id, Me.CTRLresponsabile.ResponsabileID)
            Else
                Me.CTRLservizi.AggiornaDati(Me.CTRLdati.TipoComunita_ID, 0, Me.CTRLresponsabile.ResponsabileID)
            End If
            Me.resetForm_ToFase4()
        ElseIf Me.TBLservizi.Visible Then
            Me.CTRLfinale.SetupControl(ComunitaID, Me.CTRLdati.ComunitaNome, Me.CTRLresponsabile.GetNomeResponsabile, Me.CTRLpadri.GetPadri, Me.CTRLservizi.GetServizi)
            Me.resetForm_ToFase5()
        End If
    End Sub

    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click, BTNindietro2.Click
        Dim ComunitaID As Integer
        Dim ComunitaPath As String
        Try
            If Session("AdminForChange") = True Then
                ComunitaID = Session("idComunita_forAdmin")
                ComunitaPath = Session("CMNT_path_forAdmin")
            Else
                Dim ArrComunita(,) As String = Session("ArrComunita")
                ComunitaID = Session("idComunita")

                Try
                    ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
                Catch ex As Exception
                    ComunitaPath = ""
                End Try
            End If
        Catch ex As Exception
			Try
				Dim ArrComunita(,) As String = Session("ArrComunita")
				ComunitaID = Session("idComunita")
				ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2))
			Catch ex2 As Exception
				ComunitaID = 0
				ComunitaPath = ""
			End Try
        End Try

        If Me.TBLresponsabile.Visible Then
            Me.resetForm_ToFase1()
        ElseIf Me.TBLpadri.Visible Then
            If Me.CTRLresponsabile.isInizializzato = False Then
                Dim oComunita As New COL_Comunita
            
                oComunita.Id = ComunitaID
                Me.CTRLresponsabile.SetupControllo(0, ComunitaID, oComunita.GetOrganizzazioneID, Me.CTRLdati.TipoComunita_ID, True)
            Else
                Me.CTRLresponsabile.AggiornaDati()
            End If
            Me.resetForm_ToFase2()
        ElseIf Me.TBLservizi.Visible = True Then
            Me.resetForm_ToFase3()
        ElseIf Me.TBLfinale.Visible Then
            If Me.CTRLservizi.isInizializzato = False Then
                Dim oComunita As New COL_Comunita

                oComunita.Id = ComunitaID
                oComunita.Estrai()
                Me.CTRLservizi.SetupControl(Me.CTRLdati.TipoComunita_ID, oComunita.GetOrganizzazioneID, Session("objPersona").id, Me.CTRLresponsabile.ResponsabileID)
            Else
                Me.CTRLservizi.AggiornaDati(Me.CTRLdati.TipoComunita_ID, 0, Me.CTRLresponsabile.ResponsabileID)
            End If
            Me.resetForm_ToFase4()
        End If
    End Sub

    Private Sub CTRLresponsabile_AggiornamentoVisualizzazione(ByVal Selezionato As Boolean) Handles CTRLresponsabile.AggiornamentoVisualizzazione
        Me.BTNavanti.Enabled = Selezionato
        Me.BTNavanti2.Enabled = Selezionato
    End Sub

    Private Sub CTRLservizi_AggiornamentoVisualizzazione(ByVal Selezionato As Boolean) Handles CTRLservizi.AggiornamentoVisualizzazione
        Me.BTNavanti.Enabled = Selezionato
        Me.BTNavanti2.Enabled = Selezionato
    End Sub

    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconferma.Click, BTNconferma2.Click
        If Session("Azione") = "loaded" And Me.HDN_ComunitaCreataID.Value = 0 Then
            Dim iResponse As WizardComunita_Message
            iResponse = Me.CTRLdati.CreaComunita()

            If iResponse = WizardComunita_Message.ComunitaCreata Then
                Dim iResponseResponsabile As WizardComunita_Message
                Dim iResponsePadri As WizardComunita_Message
                Dim iResponseServizi As WizardComunita_Message
                Dim ComunitaPath As String = ""

                Dim totaleC, totalePadri, j As Integer


                Try
                    If Session("AdminForChange") = True Then
                        ComunitaPath = Session("CMNT_path_forAdmin")
                    Else
                        Dim ArrComunita(,) As String = Session("ArrComunita")
                        ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2)) & Me.CTRLdati.ComunitaID & "." 'quello della cmnt a cui sono loggato e quella appena creata

                    End If
                Catch ex As Exception
                    Try
                        Dim ArrComunita(,) As String = Session("ArrComunita")
                        ComunitaPath = ArrComunita(2, UBound(ArrComunita, 2)) & Me.CTRLdati.ComunitaID & "." 'quello della cmnt a cui sono loggato e quella appena creata
                    Catch ex2 As Exception
                    End Try
                End Try
                iResponseResponsabile = Me.CTRLresponsabile.RegistraResponsabile(Me.CTRLdati.ComunitaID, ComunitaPath, Me.CTRLdati.isSubscripted)
                iResponsePadri = Me.CTRLpadri.RegistraPadri(Me.CTRLdati.ComunitaID, Session("objPersona").id, Me.CTRLresponsabile.responsabileID)
                iResponseServizi = Me.CTRLservizi.RegistraImpostazioni(Me.CTRLdati.ComunitaID)

                Dim Messaggio As String = ""
                Messaggio = Me.oResource.getValue("WizardComunita_Message." & CType(iResponse, WizardComunita_Message))
                If iResponseResponsabile <> ModuloEnum.WizardComunita_Message.ResponsabileAssociato Then
                    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseResponsabile, WizardComunita_Message))
                End If
                If iResponsePadri <> WizardComunita_Message.PadriAssociati And iResponsePadri <> WizardComunita_Message.NesunaOperazione Then
                    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponsePadri, WizardComunita_Message))
                End If
                If iResponseServizi <> ModuloEnum.WizardComunita_Message.ServiziAttivati Then
                    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseServizi, WizardComunita_Message))
                End If

                If Messaggio <> "" Then
                    Messaggio = Messaggio.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
                End If
                Response.Expires = -1
                Response.ExpiresAbsolute = Now

                Session("Azione") = "created"
                Me.CTRLservizi.ResetControllo()
                Me.CTRLpadri.ResetControllo()
                Me.CTRLresponsabile.ResetControllo()

                Dim ComunitaID As Integer
                Try
                    If Session("AdminForChange") = True Then
                        ComunitaID = Session("idComunita_forAdmin")
                    Else
                        ComunitaID = Session("idComunita")
                    End If
                Catch ex As Exception
                    Try
                        ComunitaID = Session("idComunita")
                    Catch ex2 As Exception
                        ComunitaID = 0
                    End Try
                End Try
                Me.HDN_ComunitaAttualeID.Value = ComunitaID
                Me.HDN_ComunitaCreataID.Value = 0
                If Me.CTRLdati.SetupIniziale(0, ComunitaID, True) Then
                    Me.BTNavanti.Enabled = True
                    Me.BTNavanti2.Enabled = True
                Else
                    Me.BTNavanti.Enabled = False
                    Me.BTNavanti2.Enabled = False
                End If

                If Request.QueryString("fromGestione") <> "" Then
                    If Request.QueryString("toTree") <> "" Then
                        Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&toTree=true")
                    Else
                        Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
                    End If
                Else
                    Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement)
                End If
            End If
            lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.AllTree)
        End If
    End Sub

    Private Sub BTNsalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalva.Click

    End Sub

    Private Sub BTNelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNelenco.Click, BTNelenco2.Click
        If Request.QueryString("toTree") <> "" Then
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true&toTree=true")
        Else
            Me.Utility.RedirectToUrl(Me.Utility.SystemSettings.Presenter.DefaultManagement & "?re_set=true")
        End If
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property

    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Master.ShowDocType = True
    End Sub
End Class